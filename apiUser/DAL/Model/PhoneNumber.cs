using System.ComponentModel.DataAnnotations;

namespace apiUser.DAL.Model
{
    public class PhoneNumber
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Number must be exactly 11 characters long.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Number must be exactly 11 digits.")]
        public string Number { get; set; }
        public int UserId { get; set; }
    }
}
