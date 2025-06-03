using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Models
{
    public record RegisterDto
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string FullName { get; init; }

        // You can add more properties as needed, such as PhoneNumber, etc.
        
    }
}