using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Route("api/users/{userId}/photos")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _CloudinaryConfig;

private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> CloudinaryConfig)
        {
            this._CloudinaryConfig = CloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;

            Account account =new Account(_CloudinaryConfig.Value.CloudName ,
                 _CloudinaryConfig.Value.ApiKey ,
                 _CloudinaryConfig.Value.ApiSecret );

                _cloudinary = new Cloudinary(account);       

        }


[HttpGet("{id}" , Name="GetPhoto")]

public async Task<IActionResult> GetPhoto(int id) {

    var photofromrepo = await _repo.GetPhoto(id);

    var photo = _mapper.Map<PhotoForReturnDto>(photofromrepo);

    return Ok(photo);

    
}


        [HttpPost] 
public async Task<IActionResult> AddPhotoForUser(int userId ,[FromForm]PhotoForCreationDto photoForCreation)
{

 if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

          var user = await _repo.GetUser(userId);

          var file= photoForCreation.File ;

          var uploadResult =new ImageUploadResult();
        //    if (file ==null)
        //    return  new UnsupportedMediaTypeResult();
          if (file.Length > 0)
          {
          
          using(var fileStream =file.OpenReadStream() )
          {
               var uploadParams =new ImageUploadParams(){

                   File= new FileDescription(file.Name , fileStream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                
               };
               uploadResult = _cloudinary.Upload(uploadParams);
          }
          }
          photoForCreation.url =uploadResult.Uri.ToString();         
          photoForCreation.publicId = uploadResult.PublicId;

          var photo =_mapper.Map<Photo>(photoForCreation);

          if(!user.Photos.Any(u=>u.IsMain))
         photo.IsMain=true;
         user.Photos.Add(photo);

        

         if(await _repo.SaveAll())
         {
             var phototoreturn = _mapper.Map<PhotoForReturnDto>(photo);

             return CreatedAtRoute("GetPhoto" ,new {id =photo.ID} ,phototoreturn);
         }

         return BadRequest("Error: Could not Upload photo of user {userId}");
}

[HttpPost("{photoId}/setmain")]

public async Task<IActionResult> SetMainPhoto(int userId , int photoId){

if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var user = await _repo.GetUser(userId) ;
          
       if(!user.Photos.Any(x=>x.ID ==photoId))
          return Unauthorized();

          var photoFromrepo =  await _repo.GetPhoto(photoId);

          if(photoFromrepo.IsMain)
          return BadRequest("Already Main photo") ;

          var currentMainPhoto = await _repo.GetMainPhoto(userId);


          currentMainPhoto.IsMain =false;
photoFromrepo.IsMain=true;


if(await _repo.SaveAll())
return NoContent();

return BadRequest("Could not set Main Photo");

}


[HttpDelete("{photoid}")]

public async Task<IActionResult> DeletePhoto(int userId , int photoid) {

if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var user = await _repo.GetUser(userId) ;
          
       if(!user.Photos.Any(x=>x.ID ==photoid))
          return Unauthorized();

          var photoFromrepo =  await _repo.GetPhoto(photoid);

          if(photoFromrepo.IsMain)
          return BadRequest("You cannot delete main photo") ;

          if (photoFromrepo.PublicID !=null)
          {

var deleteParams = new DeletionParams(photoFromrepo.PublicID);
          var result = _cloudinary.Destroy(deleteParams); 

          if (result.Result =="ok") {

              _repo.Delete(photoFromrepo);
          }


          } 

          if (photoFromrepo.PublicID ==null){
_repo.Delete(photoFromrepo);

          }



          if (await _repo.SaveAll())
          return Ok(); 

          return BadRequest("Could not delete Photo");

}

    }
}