namespace Server.Models.Response
{
    
    public class SuccesfulAddedBook
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
    public class CreatedBooks
    {
        public List<SuccesfulAddedBook> AddedBooks { get; set; } = new List<SuccesfulAddedBook>();
        public List<SuccesfulAddedBook> FailedToAddBooks { get; set; } = new List<SuccesfulAddedBook>();
        public int Succesful { get => AddedBooks.Count; }
        public int Failed { get => FailedToAddBooks.Count; }

    }
}
