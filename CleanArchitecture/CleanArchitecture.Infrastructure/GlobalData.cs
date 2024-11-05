using CleanArchitecture.Core.Entites;

namespace CleanArchitecture.API.Models
{
    public static class GlobalData
    {
        public static List<Tenant> Tenants { get; set; } = new List<Tenant>();
        public static List<School> Schools { get; set; } = new List<School>();
        public static List<Metadata> Metadatas { get; set; } = new List<Metadata>();
        public static List<Class> Classes { get; set; } = new List<Class>();

        public static List<Class> GetClasses()
        { 
            var classesList = new List<Class>();

            classesList = new List<Class> 
            { 
                new Class()
                { 
                    id = 1,
                    name = "Class1",
                    schoolId = 7286747169
                },
                new Class()
                {
                    id = 2,
                    name = "Class2",
                    schoolId = 7286747169
                },
                new Class()
                {
                    id = 3,
                    name = "Class3",
                    schoolId = 7286747169
                },
                new Class()
                {
                    id = 4,
                    name = "Class4",
                    schoolId = 7286747169
                },
                new Class()
                {
                    id = 1,
                    name = "Class1",
                    schoolId = 7593317597
                },
                new Class()
                {
                    id = 2,
                    name = "Class2",
                    schoolId = 7593317597
                },
                new Class()
                {
                    id = 3,
                    name = "Class3",
                    schoolId = 7621956175
                },
                new Class()
                {
                    id = 4,
                    name = "Class4",
                    schoolId = 7621956175
                }
            };

            return classesList;
        }
    }
}
