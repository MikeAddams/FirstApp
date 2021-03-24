using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "There's no username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "There's no email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "There's no password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "There's no nickname")]
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
