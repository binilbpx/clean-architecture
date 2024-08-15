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
        #region "private"

        private List<School> schools;

        #endregion

        #region "constructor"

        public SchoolRepository(IConfiguration configuration)
        {
            var json = System.IO.File.ReadAllText("schools.json");
            schools = JsonSerializer.Deserialize<List<School>>(json);
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<School>> GetAllAsync()
        {
            return schools.ToList();
        }

        public async Task<School> GetByIdAsync(long id)
        {
            return schools.Where(c => c.id == id).FirstOrDefault();
        }

        public async Task<string> AddAsync(School entity)
        {
            schools.Add(entity);

            return entity.name;
        }

        public async Task<string> UpdateAsync(School entity)
        {
            var school = schools.Where(c => c.id == entity.id).FirstOrDefault();

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
