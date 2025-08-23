using System.ComponentModel.DataAnnotations;

namespace FatFood_T2.ViewModels
{
    public class SignUpResViewModel
    {
        // User Info
        [Required(ErrorMessage = "Owner name is required")]
        public string FName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string PasswordHash { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        // Restaurant Info
        [Required(ErrorMessage = "Restaurant name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = null!;

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Description { get; set; }

        // Policy
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the policy")]
        public bool AcceptPolicy { get; set; }
    }
}
