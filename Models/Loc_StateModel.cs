using System.ComponentModel.DataAnnotations;

namespace AddressBook2.Models
{
    public class Loc_StateModel
    {
        public int? StateID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string StateName { get; set; }

        public string StateCode { get; set; }
        public int CountryID { get; set; }
       
        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }




    }
    public class Loc_StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
