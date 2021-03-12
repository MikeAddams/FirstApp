using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public RoleType Role { get; set; }
    }

    public enum RoleType
    {
        Client,
        Manager,
        Administrator
    }
}
