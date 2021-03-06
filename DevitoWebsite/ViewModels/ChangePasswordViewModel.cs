using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Email adresa je obavezna. ")]
        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Stara lozinka je obavezna. "), DataType(DataType.Password)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])[\\w~@#$%^&*+=`|{}:;!.?\\()\\[\\]-]{8,}$", ErrorMessage = "Lozinka mora imati najmanje 8 karaktera i cifru")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nova lozinka je obavezna. "), DataType(DataType.Password)]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])[\\w~@#$%^&*+=`|{}:;!.?\\()\\[\\]-]{8,}$", ErrorMessage = "Lozinka mora imati najmanje 8 karaktera i cifru")]
        public string NewPassword { get; set; }

        //------------------------------------------------------------------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Unesite ime. ")]
        [RegularExpression("^[a-zA-ZàáâäãåąčćđęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĐĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$", ErrorMessage = "Nepravilno uneto ime. ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Unesite prezime. ")]
        [RegularExpression("^[a-zA-ZàáâäãåąčćđęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĐĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$", ErrorMessage = "Nepravilno uneto prezime. ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Unesite adresu. ")]
        [RegularExpression(@"^[A-ZŠĐČĆŽ][a-zA-ZŠĐČĆŽšđčćž]{1,}\s([A-Za-zšđčćžŠĐČĆŽ]{1,}\s)*([0-9a-zA-ZšđčćžŠĐČĆŽ/]*\s*)*$", ErrorMessage = "Nepravilno uneta adresa. ")]
        public string Address { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Nepravilno unet poštanski broj. ")]
        [Required(ErrorMessage = "Poštanski broj je obavezan. ")]
        public int? PostalNumber { get; set; }

        [RegularExpression("^[a-zA-ZàáâäãåąčćđęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĐĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$", ErrorMessage = "Nepravilno uneto ime grada. ")]
        [Required(ErrorMessage = "Ime grada je obavezno. ")]
        public string City { get; set; }

        [Required(ErrorMessage = "Odaberite državu. ")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Broj telefona je obavezan. ")]
        public string PhoneNumber { get; set; }


    }
}
