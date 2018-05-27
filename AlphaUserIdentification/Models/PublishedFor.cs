﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaUserIdentification.Models
{
    public class PublishedFor
    {
        public int PublicationId { get; set; }
        public Publication Publication { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
