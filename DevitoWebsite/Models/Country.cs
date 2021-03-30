using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevitoWebsite.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ThreeCountryCode{ get; set; }
        public string TwoCountryCode { get; set; }
    }
}
