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
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class ContactRepository : IContactRepository
    {
        #region "private"

        private readonly IDbConnection connection;

        #endregion

        #region "constructor"

        public ContactRepository(IConfiguration configuration)
        {
            connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        #endregion

        #region procs

        public async Task<IReadOnlyList<Contact>> GetAllAsync()
        {
            using (connection)
            {
                connection.Open();
                var result = await connection.QueryAsync<Contact>(ContactQueries.AllContact);
                return result.ToList();
            }
        }

        public async Task<Contact> GetByIdAsync(long id)
        {
            using (connection)
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Contact>(ContactQueries.ContactById, new { ContactId = id });
                return result;
            }
        }

        public async Task<string> AddAsync(Contact entity)
        {
            using (connection)
            {
                connection.Open();
                var result = await connection.ExecuteAsync(ContactQueries.AddContact, entity);
                return result.ToString();
            }
        }

        public async Task<string> UpdateAsync(Contact entity)
        {
            using (connection)
            {
                connection.Open();
                var result = await connection.ExecuteAsync(ContactQueries.UpdateContact, entity);
                return result.ToString();
            }
        }

        public async Task<string> DeleteAsync(long id)
        {
            using (connection)
            {
                connection.Open();
                var result = await connection.ExecuteAsync(ContactQueries.DeleteContact, new { ContactId = id });
                return result.ToString();
            }
        }

        #endregion
    }
}
