namespace Core.API.Helpers
{
    public class MessageParams
    {
        private const int MAX_PAGESIZE = 50;

        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGESIZE) ? MAX_PAGESIZE : value; }
        }

        public int UserId { get; set; }
        public string MessageContainer { get; set; } = "Unread";
    }
}