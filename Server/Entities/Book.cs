namespace Server.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] MainActors { get; set; }
        public virtual ICollection<Review> Review { get; set; }
    }
}
