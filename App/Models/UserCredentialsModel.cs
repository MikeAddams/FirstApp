using Data;

namespace App.Models
{
    public class UserCredentialsModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RoleType Role { get; set; }
    }
}