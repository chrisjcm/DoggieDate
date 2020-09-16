﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class ApplicationUser : IdentityUser
    {
		public IEnumerable<Trait> UserHasTrait { get; set; }
		public IEnumerable<Trait> UserDesiredTrait { get; set; }

		public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
