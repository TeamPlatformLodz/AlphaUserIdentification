using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AlphaUserIdentification.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public List<Member> Teams { get; set; } = new List<Member>();
        public List<Administrator> Administrators { get; set; } = new List<Administrator>();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }


        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
