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
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId ,[FromQuery] MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();
                    messageParams.UserId =userId ;

                    var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

                var messages  =  _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage ,messagesFromRepo.PageSize ,
             messagesFromRepo.TotalCount ,messagesFromRepo.TotalPages);

            return Ok(messages);

        }

        [HttpGet("{id}" , Name="GetMessage")]

        public async Task<IActionResult> GetMessage(int userId , int id)
        {

        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var messageFromRepo = await _repo.GetMessage(id) ; 
         
         if (messageFromRepo==null)
         return NotFound();
// var message = _mapper.Map<MessageToReturnDto>(messageFromRepo);
         return Ok(messageFromRepo) ;

        }

 [HttpGet("thread/{recipientId}") ]

 public async Task<IActionResult> GetMessageThread(int userId , int recipientId )
 {
if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var messagefromRepo = await _repo.GetMessageThread(userId ,recipientId ) ;

         var msgtoreturn =_mapper.Map<IEnumerable<MessageToReturnDto>>(messagefromRepo) ;

         return Ok(msgtoreturn)  ;
      
 }

[HttpPost("{id}/read")]

public async Task<IActionResult> MarkMessageAsRead(int userId , int id) {
if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var message = await _repo.GetMessage(id) ;

         if (message.RecipientId !=userId)
         return Unauthorized();
         message.IsRead=true ;
         message.DateRead=System.DateTime.Now;

         await _repo.SaveAll();
         return NoContent();

}

 [HttpPost("{id}")]

 public async Task<IActionResult> DeleteMessage(int id , int userId) {
if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

         var msg = await _repo.GetMessage(id) ;

         if (msg.SenderId == userId)         
msg.SenderDeleted = true ;

   if (msg.RecipientId == userId)         
msg.RecipientDeleted = true ;

if (msg.SenderDeleted && msg.RecipientDeleted)
_repo.Delete(msg);

if(await _repo.SaveAll())
return NoContent();

throw new System.Exception("Error Deleting message") ; 

         

 }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId ,MessageForCreationDto  messageForCreationDto)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
         return Unauthorized();

messageForCreationDto.SenderId  = userId ;

var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);
var Mainphoto = await _repo.GetMainPhoto(userId);

if(recipient==null)
    return BadRequest("Could not find user");


var message = _mapper.Map<Message>(messageForCreationDto);

_repo.Add(message);

if (await _repo.SaveAll())
{
    var msg= _mapper.Map<MessageForCreationDto>(message);
    if(Mainphoto!=null)
    msg.senderPhotoUrl = Mainphoto.Url ;
    
    return CreatedAtRoute("Getmessage" , new {id = message.Id} , msg);
}


throw new System.Exception("Creating new message failed") ;

        }

    }
}
