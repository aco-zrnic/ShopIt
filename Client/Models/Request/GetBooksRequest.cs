using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Request
{
    public class GetBooksRequest
    {
        public bool OnlyComics { get; set; }
        public string? Title { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set;}
        public DateTime? ReleaseDateAfter { get; set; }
        public DateTime? ReleaseDateBefore { get; set; }
    }
}
