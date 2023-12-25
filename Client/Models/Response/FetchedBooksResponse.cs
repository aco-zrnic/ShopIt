using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Response
{
    public class FetchedBooksResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] MainActors { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImage { get; set; }
        public bool IsComic { get; set; }
        public double Score { get; set; }
    }
}
