using System.ComponentModel.DataAnnotations;

namespace AddressBook2.Models
{
    public class Con_ContactModel
    {
        public int? ContactID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string ContactName { get; set; }

        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }

        public string ContactLinkdin { get; set; }
        public string ContactTweeter { get; set; }
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
    
}

