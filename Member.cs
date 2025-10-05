using System.ComponentModel.DataAnnotations;

namespace GymWebApp.Models
{
    public class Member
    {
        private const string V = "That does not look like a valid email.";

        [Required(ErrorMessage = "Please tell us your full name.")]
        [MinLength(3, ErrorMessage = "Name should be at 3 characters or more.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        [Range(16, 130, ErrorMessage = "You must be 16 or older to enroll.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "We need an email to confirm your signup.")]
        [EmailAddress(ErrorMessage = V)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please include a phone number with an area code.")]
        [RegularExpression(@"[0-9\-\(\)\s\+]{10,}", ErrorMessage = "Please include at 10 digits or more.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Pick a plan to proceed.")]
        public string MembershipType { get; set; } 

        public int? YearlyDiscountPercent { get; set; }
    }
}