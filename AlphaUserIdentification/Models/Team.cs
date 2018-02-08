using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models
{
    public class Team
    {
        public int TeamId { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<Member> Administrators { get; set; } = new List<Member>();
    }
}
