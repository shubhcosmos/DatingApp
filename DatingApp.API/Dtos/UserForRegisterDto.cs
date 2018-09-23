using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string  Username { get; set; }
          
          [Required]
          [StringLength(8, MinimumLength=4 ,ErrorMessage="Length of password must be between 4-8")]
        public string Password { get; set; }
    }
}