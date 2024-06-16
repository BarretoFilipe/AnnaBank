using AnnaBank.Domain;

namespace AnnaBank.Infra.Interfaces
{
    public interface ITransactionRepository
    {
        Task SaveTransaction(Client sender, Client receiver, Transaction transaction);
    }
}