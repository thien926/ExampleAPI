using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class CurrentUser
    {
        public UserDto? user { get; set; }
        
        public string? permissions { get; set; }
    }
}