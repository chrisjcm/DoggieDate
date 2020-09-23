using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoggieDate.Models
{
	public class ApplicationUser : IdentityUser
	{
		public IEnumerable<Trait> UserHasTrait { get; set; }
		public IEnumerable<Trait> UserDesiredTrait { get; set; }

		public IEnumerable<Contact> Contacts { get; set; }
		public IEnumerable<Message> Messages { get; set; }
        
        public string Avatar { get; set; }

		public string Dogname { get; set; }

		public string Owner { get; set; }

		//Landskap
		public string Region { get; set; }

		public int? Age { get; set; }

		public string Breed { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
		[DataType(DataType.DateTime)]
		[DefaultValue("getutcdate()")]
		public DateTime LastLogin { get; set; }
	}
}