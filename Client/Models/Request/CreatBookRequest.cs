using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Request
{
    public class CreatBookRequest
    {

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string MainActors { get; set; }
        public bool IsComic { get; set; }
    }
}
