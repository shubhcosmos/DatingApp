using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        public string url { get; set; }

        public IFormFile File { get; set; }

        public string Description { get; set; }

        public System.DateTime DateAdded { get; set; }


        public string publicId { get; set; }


        public  PhotoForCreationDto()
        {
            this.DateAdded=System.DateTime.Now;
        }
    }
}