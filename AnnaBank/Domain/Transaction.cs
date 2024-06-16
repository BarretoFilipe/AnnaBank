using AnnaBank.Domain.Exceptions;

namespace AnnaBank.Domain
{
    public class Transaction
    {
        //EF needs
        private Transaction()
        {
        }

        public Transaction(decimal amount, Guid senderId, Guid receiverId)
        {
            if (amount <= 0)
            {
                throw new DomainException("The amount can't be less or equal to 0.");
            }
            if (senderId == receiverId)
            {
                throw new DomainException("The sender and receiver can't be the same.");
            }
            if (senderId == default)
            {
                throw new DomainException("The sender is required.");
            }
            if (receiverId == default)
            {
                throw new DomainException("The receiver is required.");
            }
            Id = Guid.NewGuid();
            Amount = amount;
            SenderId = senderId;
            ReceiverId = receiverId;
            Date = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public decimal Amount { get; private set; }
        public Guid SenderId { get; private set; }
        public Guid ReceiverId { get; private set; }
        public DateTime Date { get; private set; }

        public Client Sender { get; private set; }
        public Client Receiver { get; private set; }
    }
}