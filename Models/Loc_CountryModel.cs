using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AddressBook2.Models
{
    public class Loc_CountryModel
    {
        public int? CountryID { get; set; }
        
        public string CountryCode { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string CountryName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }




    }
    public class Loc_CountryDropDownModel
    {
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
    }
}

