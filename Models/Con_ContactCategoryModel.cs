using System.ComponentModel.DataAnnotations;

namespace AddressBook2.Models
{
    public class Con_ContactCategoryModel
    {
        public int ContactCategoryID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string ContactCategoryName { get; set; }

        public DateTime CreatDate { get; set; }

        public DateTime ModifiedDate { get; set; }




    }
}
