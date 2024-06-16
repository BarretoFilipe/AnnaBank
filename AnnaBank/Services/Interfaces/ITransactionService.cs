using AnnaBank.Abstractions;
using AnnaBank.Application.Commands;

namespace AnnaBank.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Result> GenerateTransaction(CreateTransactionCommand command);
    }
}