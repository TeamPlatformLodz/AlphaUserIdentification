﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models.TeamViewModels
{
    public class AddMemberViewModel
    {
        //users
        public List<ApplicationUser> Users { get; set; }

        //teams
        public List<Team> Teams { get; set; }
    }
}
