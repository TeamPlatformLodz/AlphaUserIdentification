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
        public List<Member> Members { get; set; } = new List<Member>();
        public List<Administrator> Administrators { get; set; } = new List<Administrator>();

        public void AddMember(ApplicationUser user)
        {
            #region checking if user can be added
            if (user == null)
            {
                return; //TODO: throw exception
            }
            if (Members.Exists(m => m.ApplicationUserId == user.Id))
            {
                return; //TODO: throw exception
            }
            #endregion
            var member = new Member
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                Team = this,
                TeamId = this.TeamId
            };
            Members.Add(member);
        }
        public void AddAdministrator(ApplicationUser user)
        {
            #region checking if user can be added
            if (user == null)
            {
                return; //TODO: throw exception
            }
            if (Administrators.Exists(m => m.ApplicationUserId == user.Id))
            {
                return; //TODO: throw exception
            }
            #endregion
            var administrator = new Administrator
            {
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                Team = this,
                TeamId = this.TeamId
            };
            Administrators.Add(administrator);
            this.AddMember(user);
        }
    }
}
