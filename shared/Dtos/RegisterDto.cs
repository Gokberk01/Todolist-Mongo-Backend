using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.shared.Dtos
{
    public class RegisterDto
    {
        public string  UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
    }
}