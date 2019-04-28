namespace DatingApp.API.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }

        public int RecipientId { get; set; }

        public string senderPhotoUrl{ get; set; }

        public System.DateTime MessageSent { get; set; }

        public string Content { get; set; }

        public MessageForCreationDto()
        {
           MessageSent =System.DateTime.Now; 
        }
    }
}