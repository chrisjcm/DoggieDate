using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class Trait
    {

        public int Id { get; set; }
        public string Name { get; set; }
    }

	//m2m Relationship
	public class wantsTrait
	{
		public int TraitId { get; set; }
		public Trait Trait { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}

	//m2m Relationship
	public class hasTrait
	{
		public int TraitId { get; set; }
		public Trait Trait { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
