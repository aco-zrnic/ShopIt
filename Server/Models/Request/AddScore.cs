using MediatR;
using Server.Models.Response;

namespace Server.Models.Request
{
    public class AddScore :IRequest<AddedScore>
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
    }
}
