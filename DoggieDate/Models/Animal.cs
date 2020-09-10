using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoggieDate.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string Color { get; set; }
        public string Traits { get; set; } // Konvertera lista till string med ,
        public string Activities { get; set; } // Konvertera lista till string med ,


        /*
         * ful -
         * tjock -
         * snorig -
         * 
         * "ful, tjock, snorig"
         */
    }
}
