using MediatR;
using Server.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Server.Models.Request
{
    public class CreateBook :IRequest<CreatedBook>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string MainActors { get; set; }
        public DateTimeOffset ReleaseData { get; set; }
        public string CoverImage { get; set; }
        public bool IsComic { get; set; }

    }
}
