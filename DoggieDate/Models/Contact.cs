using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DefaultValue(false)] // vet ej om behövs
        public bool Accepted { get; set; } // True = friends - False = pending invite

        [DefaultValue(false)]
        public bool Pending { get; set; }

        [DefaultValue(false)] // vet ej om behövs
        public bool Blocked { get; set; }
    }
}
