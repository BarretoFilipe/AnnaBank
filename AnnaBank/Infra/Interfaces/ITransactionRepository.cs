using AnnaBank.Domain;

namespace AnnaBank.Infra.Interfaces
{
    public interface ITransactionRepository : IRepository
    {
        Task SaveTransaction(Client sender, Client receiver, Transaction transaction);
    }
}