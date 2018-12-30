using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {

        public UserForRegisterDto()
        {
            this.Created = System.DateTime.Now;

            this.LastActive = System.DateTime.Now;

        }
        [Required]
        public string  Username { get; set; }
          
          [Required]
          [StringLength(8, MinimumLength=4 ,ErrorMessage="Length of password must be between 4-8")]
        public string Password { get; set; }

 [Required]
        public string Gender { get; set; }

 [Required]
         public string KnownAs { get; set; }

 [Required]        
   public System.DateTime DateOfBirth { get; set; }

 [Required]
            public string City { get; set; }

            [Required]
              public string Country { get; set; }

               public System.DateTime Created { get; set; }


               public System.DateTime LastActive { get; set; }


    }
}