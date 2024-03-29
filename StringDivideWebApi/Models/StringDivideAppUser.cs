using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StringDivideWebApp.Models
{
    public class StringDivideAppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]

        public string email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string password { get; set; }
        public string PasswordHash { get; set; } // This will store the hashed password
    }
}
