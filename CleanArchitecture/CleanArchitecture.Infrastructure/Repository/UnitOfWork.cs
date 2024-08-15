using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IContactRepository contactRepository, ISchoolRepository schoolRepository)
        {
            Contacts = contactRepository;
            Schools = schoolRepository;
        }

        public IContactRepository Contacts { get; set; }
        public ISchoolRepository Schools { get; set; }
    }
}
