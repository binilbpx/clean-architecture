using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entites
{
    public class Tenant
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
        public bool isAdminPageAllowed { get; set; }
    }
}
