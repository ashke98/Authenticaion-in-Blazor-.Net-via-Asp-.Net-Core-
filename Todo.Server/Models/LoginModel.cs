using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Server.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
