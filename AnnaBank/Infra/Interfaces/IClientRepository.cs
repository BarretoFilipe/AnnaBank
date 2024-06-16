using AnnaBank.Domain;

namespace AnnaBank.Infra.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetById(Guid id);

        Task<Client?> GetByIBAN(string iban);
    }
}