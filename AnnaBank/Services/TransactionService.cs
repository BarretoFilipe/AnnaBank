using AnnaBank.Abstractions;
using AnnaBank.Application.Commands;
using AnnaBank.Domain;
using AnnaBank.Infra.Interfaces;
using AnnaBank.Services.Interfaces;

namespace AnnaBank.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IClientRepository clientRepository,
            ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<Result> GenerateTransaction(CreateTransactionCommand command)
        {
            Client? sender = await _clientRepository.GetById(command.SenderId);
            if (sender == null)
            {
                return Result.Failure(TransactionError.NotFound("Sender Id"));
            }
            if ((sender.Balance - command.Amount) < 0)
            {
                return Result.Failure(TransactionError.SenderNotHaveBalance);
            }
            Client? receiver = await _clientRepository.GetByIBAN(command.Iban);
            if (receiver == null)
            {
                return Result.Failure(TransactionError.NotFound("IBAN"));
            }
            if (sender.Id == receiver.Id)
            {
                return Result.Failure(TransactionError.SameUser);
            }

            sender.UpdateSenderBalance(command.Amount);
            receiver.UpdateReceiverBalance(command.Amount);
            Transaction transaction = new(command.Amount, command.SenderId, receiver.Id);

            await _transactionRepository.SaveTransaction(sender, receiver, transaction);

            return Result.Success();
        }
    }
}