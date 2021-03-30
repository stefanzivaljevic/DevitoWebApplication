using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email adresa je obavezna. ")]
        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna."),DataType(DataType.Password)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])[\\w~@#$%^&*+=`|{}:;!.?\\()\\[\\]-]{8,}$", ErrorMessage = "Lozinka mora imati najmanje 8 karaktera i cifru")]
        public string Password { get; set; }
    }
}
