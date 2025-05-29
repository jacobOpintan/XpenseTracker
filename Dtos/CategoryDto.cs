using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XpenseTracker.Dtos
{
    public record CategoryDto
    {
        public string Name { get; set; } = string.Empty;
    }
}