namespace CleanArchitecture.Core.Entites
{
    public class Metadata
    {
        public bool isCurrent { get; set; }
        public bool isDeleted { get; set; }
        public int actionId { get; set; }
        public int tenantId { get; set; }
        public List<long> methodNodeIds { get; set; }
        public List<MethodNode> methodNodes { get; set; }
        public bool isForAllMethods { get; set; }
        public bool isForAllMethodsOfTenant { get; set; }
        public bool isPageAllowed { get; set; }
        public bool isEditAllowed { get; set; }
        public bool isDeleteAllowed { get; set; }
        public bool isExportAllowed { get; set; }
        public bool isAllowedToPublishToProduction { get; set; }
        public bool isProductionStatusCheckAllowed { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public long optionId { get; set; }
        public Option option { get; set; }
        public string type { get; set; }
        public bool isMultipleSelectionAllowed { get; set; }
        public bool isResourceCategory { get; set; }
        public bool isActive { get; set; }
        public bool isAdminMetadataPropertyPageAllowed { get; set; }
    }

    public class MethodNode
    {
        public long id { get; set; }
        public string title { get; set; }
        public bool isAdminMethodPageAllowed { get; set; }
    }

    public class Icon
    {
        public string description { get; set; }
        public long id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string tags { get; set; }
        public string originalFileName { get; set; }
        public string extension { get; set; }
        public bool isDownloadable { get; set; }
        public bool hasThumb { get; set; }
        public string filePath { get; set; }
        public int tenantId { get; set; }
        public Tenant tenant { get; set; }
        public User userActor { get; set; }
        public User userCreator { get; set; }
        //public DateTime timestampCreated { get; set; }
        //public DateTime timestampAction { get; set; }
    }

    public class OptionChild
    {
        public long parentId { get; set; }
        public int sequence { get; set; }
        public long iconResourceId { get; set; }
        public Icon icon { get; set; }
        public List<OptionChild> children { get; set; }
        public long id { get; set; }
        public string name { get; set; }
    }

    public class Option
    {
        public int sequence { get; set; }
        public long iconResourceId { get; set; }
        public Icon icon { get; set; }
        public List<OptionChild> children { get; set; }
        public long id { get; set; }
        public string name { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public bool isActive { get; set; }
        public string fullName { get; set; }
        public List<string> roles { get; set; }
        public bool hasValidPrimaryEmailAddress { get; set; }
        public bool isSlaveAccount { get; set; }
        public bool isMasterAccount { get; set; }
        public bool isProfilePageAllowed { get; set; }
        public List<int> publisherIds { get; set; }
    }
}
