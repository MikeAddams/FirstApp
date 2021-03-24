using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="There's no username")]
        public string Username { get; set; }

        [Required(ErrorMessage ="There's no password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
