using Server.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Server.Models.Response
{
    public class BookScore
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] MainActors { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; }
        public bool IsComic { get; set; }
        public double Score { get; set; }
    }
    public class FetchedBooks
    {
        public IQueryable<BookScore> Books { get; set; }
    }
}
