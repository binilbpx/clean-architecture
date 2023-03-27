namespace CleanArchitecture.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IContactRepository Contacts { get; }
    }
}
