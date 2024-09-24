using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using CleanArchitecture.Sql.Queries;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class TenantRepository : ITenantRepository
    {
        #region "private"

        private List<Tenant> tenants;

        #endregion

        #region "constructor"

        public TenantRepository(IConfiguration configuration)
        {
            var json = System.IO.File.ReadAllText("tenants.json");
            tenants = JsonSerializer.Deserialize<List<Tenant>>(json);
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<Tenant>> GetAllAsync()
        {
            return tenants.ToList();
        }

        public async Task<Tenant> GetByIdAsync(long id)
        {
            return tenants.Where(c => c.id == id).FirstOrDefault();
        }

        public async Task<string> AddAsync(Tenant entity)
        {
            tenants.Add(entity);

            return entity.name;
        }

        public async Task<string> UpdateAsync(Tenant entity)
        {
            var school = tenants.Where(c => c.id == entity.id).FirstOrDefault();

            school.name = entity.name;

            return entity.name;
        }

        public async Task<string> DeleteAsync(long id)
        {
            return null;
        }

        #endregion
    }
}
