using MediatR;
using Server.Models.Response;

namespace Server.Models.Request
{
    public class FetchBooks :IRequest<FetchedBooks>
    {
        public string Title { get; set; }
        public bool OnlyComics { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }
        public DateTime? ReleaseDateAfter { get; set; }
        public DateTime? ReleaseDateBefore { get; set; }
    }
}
