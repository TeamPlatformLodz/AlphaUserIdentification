using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models
{
    public class Membership
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
