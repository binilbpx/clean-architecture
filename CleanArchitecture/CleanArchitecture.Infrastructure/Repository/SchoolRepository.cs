using CleanArchitecture.API.Models;
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
    public class SchoolRepository : ISchoolRepository
    {
        #region "constructor"

        public SchoolRepository(IConfiguration configuration)
        {
            var json = System.IO.File.ReadAllText("schools.json");
            if (!GlobalData.Tenants.Any())
            {
                GlobalData.Schools = JsonSerializer.Deserialize<List<School>>(json);
            }
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<School>> GetAllAsync()
        {
            return GlobalData.Schools.ToList();
        }

        public async Task<School> GetByIdAsync(long id)
        {
            return GlobalData.Schools.Where(c => c.id == id).FirstOrDefault();
        }

        public async Task<string> AddAsync(School entity)
        {
            GlobalData.Schools.Add(entity);

            return entity.name;
        }

        public async Task<string> UpdateAsync(School entity)
        {
            var school = GlobalData.Schools.Where(c => c.id == entity.id).FirstOrDefault();

            school.name = entity.name;
            school.tenantId = entity.tenantId;
            school.tenantName = entity.tenantName;

            return entity.name;
        }

        public async Task<string> DeleteAsync(long id)
        {
            return null;
        }

        #endregion
    }
}
