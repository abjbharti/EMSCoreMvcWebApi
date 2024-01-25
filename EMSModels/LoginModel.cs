using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSModels
{
    public class LoginModel
    {
        public bool IsSuccess { get; set; }
        public string Username { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
