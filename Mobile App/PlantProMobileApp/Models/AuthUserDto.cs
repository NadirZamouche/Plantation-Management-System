using System;
using System.Collections.Generic;
using System.Text;

namespace PlantProMobileApp.Models
{
    public class AuthUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}