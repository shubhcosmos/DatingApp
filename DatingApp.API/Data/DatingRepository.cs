using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;

        public DatingRepository(DataContext dataContext)
        {
            this.context = dataContext;

        }
        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync( x=>x.Id ==id);

            return user;
        }

        public  async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users =  context.Users.Include(p=>p.Photos).OrderByDescending(u=>u.LastActive).AsQueryable() ;

            users = users.Where(user => user.Id != userParams.UserId).Where(u => u.Gender == userParams.Gender);

           if (userParams.Likers)  {
            var userlikers = await GetUserLikes(userParams.UserId , userParams.Likers) ;
            users = users.Where(u => userlikers.Contains(u.Id));
           }

           if(userParams.Likees) {

                var userlikees = await GetUserLikes(userParams.UserId , userParams.Likers) ;
                 users = users.Where(u => userlikees.Contains(u.Id));
           }
           
            if (userParams.MinAge !=18 || userParams.MaxAge!=99) 
            {

                var minDOB = System.DateTime.Today.AddYears(-userParams.MaxAge-1);

                var maxDOB = System.DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u=>u.DateOfBirth>=minDOB && u.DateOfBirth<=maxDOB);


            }

            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {

                switch(userParams.OrderBy)
                {

                    case  "created" :
                    users = users.OrderByDescending(u=>u.Created) ;
                    break;

                    default : 
                                    users = users.OrderByDescending(u=>u.LastActive) ;
                    break;


                }
            }

            return await PagedList<User>.CreateAsync(users , userParams.pageNumber , userParams.PageSize) ;
        }

private async Task<IEnumerable<int>> GetUserLikes (int id , bool likers) {

    var user = await context.Users.Include(x=>x.Likers).Include(x=>x.Likees).FirstOrDefaultAsync(u=>u.Id ==id);


       if(likers) {

// returns list of users who have liked logged in user
           return  user.Likers.Where(u=>u.LikeeId==id).Select(x=>x.LikerId);
       }
       else {

//returns list of users who wereliked by logged in user
           return user.Likees.Where(u=>u.LikerId==id).Select(x=>x.LikeeId);
       }

}
        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync()  > 0;
        }

        

       public async  Task<Photo> GetPhoto(int id)
        {
            var Photo = await  context.Photos.FirstOrDefaultAsync( p =>p.ID==id); 

            return Photo;
        }


       public  async Task<Photo> GetMainPhoto(int userId){

          return  await context.Photos.Where( p=>p.UserID ==userId).FirstOrDefaultAsync(p=>p.IsMain);


       }


      public async Task<Like> GetLike (int userId , int recipientId) 
      {

return await context.Likes.FirstOrDefaultAsync(u=>u.LikerId==userId && u.LikeeId == recipientId);

      }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages.FirstOrDefaultAsync(m=>m.Id == id) ;
        }

        public async  Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
           var messages = context.Messages.Include(m=>m.Sender).ThenInclude(u=>u.Photos)
           .Include(m=>m.Recipient)
           .ThenInclude(u=>u.Photos)
           .AsQueryable();

           switch(messageParams.MessageContainer)
           {
             case  "Inbox":
                 messages = messages.Where(m=>m.RecipientId == messageParams.UserId && m.RecipientDeleted==false);
                    break;

                case  "Outbox":
                 messages = messages.Where(m=>m.SenderId == messageParams.UserId && m.SenderDeleted==false);
                    break;

                default: 
                  messages = messages.Where(m=>m.RecipientId == messageParams.UserId && m.IsRead==false && m.RecipientDeleted==false);
                  break ;
           }
 messages =messages.OrderByDescending(m=>m.MessageSent) ;

 return await    PagedList<Message>.CreateAsync(messages ,messageParams.pageNumber , messageParams.PageSize) ;
          
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int UserId, int recipientId)
        {
            var messages = await context.Messages.Include(m=>m.Sender).ThenInclude(u=>u.Photos)
           .Include(m=>m.Recipient)
           .ThenInclude(u=>u.Photos)
           .Where(m=>m.RecipientId==UserId && m.RecipientDeleted==false && m.SenderId == recipientId 
           || m.RecipientId == recipientId  && m.SenderId ==UserId &&m.SenderDeleted==false).OrderByDescending(m=>m.MessageSent)
           .ToListAsync();

           return  messages ;
        }
    }
}