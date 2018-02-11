using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaUserIdentification.Models;
using AlphaUserIdentification.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlphaUserIdentification.Extensions
{
    public static class UserHelper
    {
        public static async Task<ApplicationUser> GetCurrentUserById(ApplicationDbContext context, string id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
