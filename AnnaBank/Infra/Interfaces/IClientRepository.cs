using AnnaBank.Domain;

namespace AnnaBank.Infra.Interfaces
{
    public interface IClientRepository : IRepository
    {
        Task<Client?> GetById(Guid id);

        Task<Client?> GetByIBAN(string iban);

        Task Create(Client client);
    }
}