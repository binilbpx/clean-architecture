using CleanArchitecture.Core.Entites;

namespace CleanArchitecture.API.Models
{
    public static class GlobalData
    {
        public static List<Tenant> Tenants { get; set; } = new List<Tenant>();
        public static List<School> Schools { get; set; } = new List<School>();
        public static List<Metadata> Metadatas { get; set; } = new List<Metadata>();
    }
}
