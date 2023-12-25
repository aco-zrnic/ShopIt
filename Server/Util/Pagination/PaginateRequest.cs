namespace Server.Util.Pagination
{
    public class PaginateRequest
    {
        const int maxPageSize = 100;
        private int _pageNumber = 1;
        public bool PaginationRequired { get; set; }
        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                PaginationRequired = true;
                _pageNumber = (value < 1) ? _pageNumber : value;
            }
        }

        private int _pageSize = 100;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
