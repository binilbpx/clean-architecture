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
    public class MetadataRepository : IMetadataRepository
    {
        #region "private"

        private List<Metadata> metadatas;

        #endregion

        #region "constructor"

        public MetadataRepository(IConfiguration configuration)
        {
            var json = System.IO.File.ReadAllText("metadatas.json");
            metadatas = JsonSerializer.Deserialize<List<Metadata>>(json);
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<Metadata>> GetAllAsync()
        {
            return metadatas.ToList();
        }

        public async Task<Metadata> GetByIdAsync(long id)
        {
            return metadatas.Where(c => c.id == id).FirstOrDefault();
        }

        public async Task<string> AddAsync(Metadata entity)
        {
            metadatas.Add(entity);

            return entity.name;
        }

        public async Task<string> UpdateAsync(Metadata entity)
        {
            var school = metadatas.Where(c => c.id == entity.id).FirstOrDefault();

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
