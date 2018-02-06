using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models
{
    public class Publication
    {
     
        public ApplicationUser Author { get; set; } 
        [Key]
        public int Id { get; private set; }
        [StringLength(500, ErrorMessage = "Too long.")]
        public string Description { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
    }
}
