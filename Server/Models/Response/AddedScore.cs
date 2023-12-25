using System.ComponentModel.DataAnnotations;

namespace Server.Models.Response
{
    public class AddedScore
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public int BookId { get; set; }
    }
}
