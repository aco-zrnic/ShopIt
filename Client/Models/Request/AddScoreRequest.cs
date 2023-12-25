using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Request
{
    public class AddScoreRequest
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10.")]
        public int Score { get; set; }
    }
}
