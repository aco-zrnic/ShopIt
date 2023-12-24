using System.ComponentModel.DataAnnotations;

namespace Server.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10.")]
        public int Score { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
