using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models
{
    public class PublicationLike
    {
        public int PublicationId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
