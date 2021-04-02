using Data;

namespace App.Models
{
    public class UserAuthModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public RoleType Role { get; set; }
    }
}
