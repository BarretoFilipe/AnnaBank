using AnnaBank.Domain;
using AnnaBank.Exceptions;
using FluentAssertions;

namespace AnnaBankTest.Domain
{
    public class TransactionTest
    {
        private readonly Guid _validSender = Guid.NewGuid();
        private readonly Guid _validReceiver = Guid.NewGuid();

        [Fact]
        public void AmountCantBeLessOrEqualToZero()
        {
            FluentActions.Invoking(() => new Transaction(0, _validSender, _validReceiver))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The amount can't be less or equal to 0.");

            FluentActions.Invoking(() => new Transaction(-0.10m, _validSender, _validReceiver))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The amount can't be less or equal to 0.");
        }

        [Fact]
        public void AmountMustAcceptPositiveValues()
        {
            Transaction transaction = new(19.68m, _validSender, _validReceiver);

            transaction.Amount.Should().Be(19.68m);
            transaction.SenderId.Should().Be(_validSender);
            transaction.ReceiverId.Should().Be(_validReceiver);
            transaction.Date.Should().BeSameDateAs(DateTime.Now);
        }

        [Fact]
        public void SenderIsRequired()
        {
            FluentActions.Invoking(() => new Transaction(100m, new Guid(), _validReceiver))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The sender is required.");
        }

        [Fact]
        public void ReceiverIsRequired()
        {
            FluentActions.Invoking(() => new Transaction(100m, _validSender, new Guid()))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The receiver is required.");
        }
        
        [Fact]
        public void SenderAndReceiverCantBeTheSame()
        {
            FluentActions.Invoking(() => new Transaction(100m, _validSender, _validSender))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The sender and receiver can't be the same.");
        }
    }
}