using System.ComponentModel.DataAnnotations;

namespace AddressBook2.Models
{
    public class Loc_CityModel
    {
        public int? CityID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string CityName { get; set; }

        public string CityCode { get; set; }
        public int CountryID { get; set; }

        public int StateID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }




    }
    public class Loc_CityDropDownModel
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
}
