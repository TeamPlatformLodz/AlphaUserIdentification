using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models.PublicationsViewModels
{
    public class CreatePostViewModel
    {
        public Publication Publication { get; set; }
        public List<int> TeamIds { get; set; }
    }
}
