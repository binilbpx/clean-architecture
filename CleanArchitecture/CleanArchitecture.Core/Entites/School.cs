using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Entites
{
    public class School
    {
        public long id { get; set; }
        public string name { get; set; }
        public bool isActive { get; set; }
        public int tenantId { get; set; }
        public string numberOfActiveClasses { get; set; }
        public Tenant tenant { get; set; }
        public bool isTextToSpeechEnabled { get; set; }
        public bool isGamificationEnabled { get; set; }
        public bool isAvatarShopEnabled { get; set; }
        public bool isActivationAllowed { get; set; }
    }
}
