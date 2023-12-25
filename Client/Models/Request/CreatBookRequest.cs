using Microsoft.AspNetCore.Http;
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
        public string[] MainActors { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [RegularExpression(RegexPattern.Base64String, ErrorMessage = "The provided string is not valid base64 string")]
        public string CoverImage { get; set; }
        public bool IsComic { get; set; }
    }
}
