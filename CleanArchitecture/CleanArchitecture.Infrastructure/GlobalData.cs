using CleanArchitecture.Core.Entites;

namespace CleanArchitecture.API.Models
{
    public static class GlobalData
    {
        public static List<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
}
