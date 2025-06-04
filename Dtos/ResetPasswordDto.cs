using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Dtos
{
    public record ResetPasswordDto
    {
        public string Email { get; init; }
        public string Token { get; init; }
        public string NewPassword { get; init; }
        
    }
}