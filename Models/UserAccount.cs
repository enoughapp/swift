using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swift.Models
{
    [Table("UserAccount")]
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter username")]
        [MinLength(3)]
        [MaxLength(15)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [MinLength(3)]
        [MaxLength(15)]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Dob { get; set; }
        
        [Required]
        public string Occupation { get; set; }

        [Required]
        public int Income { get; set; }

        [Required]
        public string SocialMediaLink { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}