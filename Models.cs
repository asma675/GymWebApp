using System.ComponentModel.DataAnnotations;


namespace GymWebApp.Models
{
    public class Members
    {
        [Required, MinLength(3)]
        public string Name { get; set; }

        [Required, Range(16, 150)]
        public int Age { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(10)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Membership Type")]
        public string MembershipType { get; set; } 

        [Range(1, 100)]
        public int? YearlyDiscountPercent { get; set; }
    }
}