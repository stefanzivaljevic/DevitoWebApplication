using DevitoWebsite.Data;
using DevitoWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.ViewModels
{
    public class UserDetailsViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int? PostalNumber { get; set; }

        public string City { get; set; }

        public Country Country { get; set; }

        public int CountryId { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public WishList WishList { get; set; }

    }
}
