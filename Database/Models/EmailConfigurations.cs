using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class EmailConfigurations
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}
