using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models.TeamViewModels
{
    public class DetailsViewModel
    {
        public bool IsAdmin { get; set; }

        public List<ApplicationUser> NotTeamUsers { get; set; }
        public List<Member> AlreadyTeamUsers { get; set; }
        public Team Team { get; set; }
    }
}
