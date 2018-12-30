using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
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
        public async Task<IActionResult> GetUsers()
        {

            var users = await repo.GetUsers();
               var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);
               

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

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}