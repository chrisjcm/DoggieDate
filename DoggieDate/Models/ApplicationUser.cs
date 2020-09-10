using Microsoft.AspNetCore.Identity;
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
        [PersonalData]
        //[Required]
        [Column(TypeName="varchar(100)")]
        public string FirstName { get; set; }
    }
}
