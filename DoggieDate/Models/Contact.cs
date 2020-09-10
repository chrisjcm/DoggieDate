﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class Contact
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ContactId { get; set; }
        public ApplicationUser UserContact { get; set; }

        public bool Blocked { get; set; }
    }
}
