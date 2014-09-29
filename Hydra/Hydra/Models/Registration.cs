namespace Hydra.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Registration
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string ConfirmPassword { get; set; }

        public bool? RememberMe { get; set; }
    }
}