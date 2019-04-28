namespace DatingApp.API.Helpers
{
    public class MessageParams
    {
          private const int MaxpageSize = 50 ;
        public int pageNumber { get; set; } =1;

        private int pageSize =10;
        public int PageSize
        {
            get { return pageSize ;}
            set { pageSize =  value > MaxpageSize ? MaxpageSize : value;}
        }


        public int UserId { get; set; }

        public string MessageContainer  { get; set; } = "Unread";
    }
}