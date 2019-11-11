using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FName { get; set; }
        public String LName { get; set; }
    }
}
