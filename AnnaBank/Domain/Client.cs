using AnnaBank.Exceptions;

namespace AnnaBank.Domain
{
    public class Client
    {
        //EF needs
        private Client()
        {
        }

        public Client(string name, string iban, decimal balance = 0)
        {
            if (string.IsNullOrWhiteSpace(name)
                || name.Length < 3)
            {
                throw new DomainException("The name must be greater than or equal to 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(iban)
                || iban.Length <= 15
                || iban.Length >= 33)
            {
                throw new DomainException("The IBAN must be between 15 and 33 characters.");
            }

            Id = Guid.NewGuid();
            Name = name;
            IBAN = iban;
            InitialBalance(balance);
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string IBAN { get; private set; }
        public decimal Balance { get; private set; }

        private void InitialBalance(decimal balance)
        {
            if (balance < 0)
            {
                throw new DomainException("Balance can't be negative.");
            }

            Balance = balance;
        }

        public void UpdateSenderBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new DomainException("Amount can't be negative.");
            }
            if ((Balance - amount) < 0)
            {
                throw new DomainException("The balance can't be negative.");
            }

            Balance -= amount;
        }

        public void UpdateReceiverBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new DomainException("Amount can't be negative.");
            }

            Balance += amount;
        }
    }
}