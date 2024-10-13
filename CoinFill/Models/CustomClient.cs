using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoinFill.Models
{
    public class CustomClient : IdentityUser
    {
        [MaxLength(128)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [MaxLength(128)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public decimal WebCredit { get; set; } = 0;

        public string ProfilePhoto { get; set; }

        public string FullNameAddress { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
