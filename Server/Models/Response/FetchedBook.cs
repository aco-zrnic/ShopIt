namespace Server.Models.Response
{
    public class FetchedBook
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] MainActors { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; }
        public bool IsComic { get; set; }
    }
}
