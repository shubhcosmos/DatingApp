using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
   [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository repo;
        private readonly IMapper mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams )
        {
            var currentUserID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) ;

            var userFromRepo = await repo.GetUser(currentUserID) ; 
            

            userParams.UserId = currentUserID ; 

            if(string.IsNullOrEmpty(userParams.Gender))
            {

                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male" ;
            }

            var users = await repo.GetUsers(userParams);
               var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);

               Response.AddPagination(users.CurrentPage , users.PageSize , users.TotalCount , users.TotalPages);
               

            return Ok(usersToReturn);
            
        }

        [HttpGet("{id}" , Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {

            var user = await repo.GetUser(id);
            var userToReturn = mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

         [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id ,UserForUpdateDto userForUpdate)
        {

         if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();
                 var user = await repo.GetUser(id);
                 mapper.Map(userForUpdate,user);
              // user.City="Test";

               if(await repo.SaveAll())
               {
                   return NoContent();
               }

               throw new Exception($"Updaing User with {id} failed on save");
        }

[HttpPost("{id}/like/{recipientID}")]

public async Task<IActionResult> LikeUser(int id , int recipientID) {

  if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();


         var like = await repo.GetLike(id , recipientID) ;

         if (like!=null)
return BadRequest("You already Like this user");


if(await repo.GetUser(recipientID) ==null )
return NotFound();

 like = new Like
{
LikerId =id,
LikeeId = recipientID

};
         repo.Add<Like>(like) ;

        if(await repo.SaveAll()) 
        return Ok();

        return BadRequest("failed to like User {recipientId}") ;


}


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}