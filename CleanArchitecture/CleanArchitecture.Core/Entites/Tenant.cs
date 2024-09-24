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
        public bool isDeleteAllowed { get; set; }
        public bool isEditAllowed { get; set; }
        public bool multiSafepayEnabled { get; set; }
        public bool exactOnlineEnabled { get; set; }
        public bool impressEnabled { get; set; }
        public bool bolEnabled { get; set; }
        public bool licensePaymentsEnabled { get; set; }
        public bool activationCodesEnabled { get; set; }
        public bool gamificationEnabled { get; set; }
        public bool textToSpeechEnabled { get; set; }
        public bool virtualSkillsLabEnabled { get; set; }
        public bool eckEnabled { get; set; }
        public bool entreeEnabled { get; set; }
        public bool webshopEnabled { get; set; }
    }
}
