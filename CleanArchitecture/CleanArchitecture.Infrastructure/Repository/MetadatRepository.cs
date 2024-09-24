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
    public class MetadataRepository : IMetadataRepository
    {
        #region "constructor"

        public MetadataRepository(IConfiguration configuration)
        {
            var json = System.IO.File.ReadAllText("metadatas.json");
            if (!GlobalData.Metadatas.Any())
            {
                GlobalData.Metadatas = JsonSerializer.Deserialize<List<Metadata>>(json);
            }
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<Metadata>> GetAllAsync()
        {
            return GlobalData.Metadatas.ToList();
        }

        public async Task<Metadata> GetByIdAsync(long id)
        {
            return GlobalData.Metadatas.Where(c => c.id == id).FirstOrDefault();
        }

        public async Task<string> AddAsync(Metadata entity)
        {
            GlobalData.Metadatas.Add(entity);

            return entity.name;
        }

        public async Task<string> UpdateAsync(Metadata entity)
        {
            var metaData = GlobalData.Metadatas.Where(c => c.id == entity.id).FirstOrDefault();

            metaData.name = entity.name;

            return entity.name;
        }

        public async Task<string> DeleteAsync(long id)
        {
            return null;
        }

        #endregion
    }
}
