using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class UserPreferences
    {
        public int Id { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string Color { get; set; }
        public string Traits { get; set; }
        public string Activities { get; set; }
    }
}
