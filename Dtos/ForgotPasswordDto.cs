using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Dtos
{
    public record ForgotPasswordDto
    {
        public string Email { get; init; }
        
    }
}