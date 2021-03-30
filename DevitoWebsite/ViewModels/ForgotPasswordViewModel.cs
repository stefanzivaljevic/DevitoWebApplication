using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email adresa je obavezna. ")]
        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
