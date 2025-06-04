using Microsoft.AspNetCore.Mvc;
using XpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using XpenseTracker.Dtos;
using XpenseTracker.Helpers;
using System.Web;





namespace XpenseTracker.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly MailHelper _mailHelper;

    // Constructor to inject dependencies
    public AuthController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, MailHelper mailHelper)
    {
        _userManager = usermanager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mailHelper = mailHelper;
    }

    //endpoint for user registration
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
       // Send confirmation email
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";
        // creating email body and subject
        var emailSubject = "Confirm your email";
        var emailBody = $"<p>Click <a href='{confirmationLink}'>here</a> to confirm your email.</p>";

        // Send email using MailHelper
        await _mailHelper.SendEmailAsync(user.Email, emailSubject,
            emailBody);

        return Ok("Registration successful. Please check your email to confirm.");
    }

    // edpoint for email confirmation
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return BadRequest("Invalid user.");

        var decodedToken = HttpUtility.UrlDecode(token);
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (result.Succeeded)
            return Ok("Email confirmed successfully.");
        return BadRequest("Email confirmation failed.");
    }

    //endpoint for user login
        [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid Credentials" });

        }
        // Check if the user is confirmed
         if (!await _userManager.IsEmailConfirmedAsync(user))
            return Unauthorized("Email not confirmed.");

        // Check password
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid Credentials" });
        }

        // Generate JWT token
        var token = await GenerateJwtToken(user);
        return Ok(new { token });
    }

    //endpoint for forgot password 
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Ok(new {message ="If an account with that email exists, a password reset link will be sent."});
        }

        // Generate password reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);
        var resetLink = $"{Request.Scheme}://{Request.Host}/api/auth/reset-password?email={model.Email}&token={encodedToken}";

        // creating email body and subject
        var emailSubject = "Reset your password";
        var emailBody = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

        // Send email using MailHelper
        await _mailHelper.SendEmailAsync(model.Email, emailSubject, emailBody);

        return Ok("Password reset link sent to your email.");
    }

    // endpoint for resetting password
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest("Invalid request.");
        }

        var decodedToken = HttpUtility.UrlDecode(model.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

        if (result.Succeeded)
            return Ok("Password reset successfully.");
        return BadRequest(result.Errors);
    }
    
    //generate JWT token
    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FullName", user.FullName)
            };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }





}