using AnnaBank.Domain;
using AnnaBank.Exceptions;
using FluentAssertions;

namespace AnnaBankTest.Domain
{
    public class ClientTest
    {
        private readonly string _validIBAN = "PT50123456789012345678901";
        private readonly string _validName = "Rick Deckard";
        private readonly string _invalidName = "RD";

        [Fact]
        public void NameMustBeRequired()
        {
            FluentActions.Invoking(() => new Client(null, _validIBAN))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The name must be greater than or equal to 3 characters.");

            FluentActions.Invoking(() => new Client(string.Empty, _validIBAN))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The name must be greater than or equal to 3 characters.");
        }

        [Fact]
        public void NameMustBeGreaterThanOrEqualTo3Carcteres()
        {
            FluentActions.Invoking(() => new Client(_invalidName, _validIBAN))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The name must be greater than or equal to 3 characters.");
        }

        [Fact]
        public void IBANMustBeRequired()
        {
            FluentActions.Invoking(() => new Client(_validName, null))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The IBAN must be between 15 and 33 characters.");

            FluentActions.Invoking(() => new Client(_validName, string.Empty))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The IBAN must be between 15 and 33 characters.");
        }

        [Fact]
        public void NameCantBeLessThan15Caracters()
        {
            FluentActions.Invoking(() => new Client(_validName, "PT501234567890"))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The IBAN must be between 15 and 33 characters.");
        }

        [Fact]
        public void NameCantBeGreaterThan33Caracters()
        {
            FluentActions.Invoking(() => new Client(_validName, "PT50123456789012345678901234567890"))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The IBAN must be between 15 and 33 characters.");
        }

        [Fact]
        public void ClientCreateWithBalanceWithZero()
        {
            Client client = new(_validName, _validIBAN);

            client.Name.Should().Be(_validName);
            client.IBAN.Should().Be(_validIBAN);
            client.Balance.Should().Be(0m);
        }

        [Fact]
        public void ReceiverBalanceMustAcceptPositiveAmount()
        {
            Client client = new(_validName, _validIBAN);
            client.UpdateReceiverBalance(123.45m);

            client.Balance.Should().Be(123.45m);
        }

        [Fact]
        public void SenderBalanceMustAcceptPositiveAmount()
        {
            Client client = new(_validName, _validIBAN);
            client.UpdateReceiverBalance(200.50m);
            client.UpdateSenderBalance(100.50m);

            client.Balance.Should().Be(100.00m);
        }

        [Fact]
        public void ReceiverBalanceCantBeAcceptNegativeAmount()
        {
            Client client = new(_validName, _validIBAN);
            FluentActions.Invoking(() => client.UpdateReceiverBalance(-0.10m))
                .Should()
                .Throw<DomainException>()
                .WithMessage("Amount can't be negative.");
        }

        [Fact]
        public void SenderBalanceCantBeAcceptNegativeAmount()
        {
            Client client = new(_validName, _validIBAN);
            FluentActions.Invoking(() => client.UpdateSenderBalance(-0.10m))
                .Should()
                .Throw<DomainException>()
                .WithMessage("Amount can't be negative.");
        }

        [Fact]
        public void BalanceCantBeNegative()
        {
            Client client = new(_validName, _validIBAN);
            FluentActions.Invoking(() => client.UpdateSenderBalance(0.10m))
                .Should()
                .Throw<DomainException>()
                .WithMessage("The balance can't be negative.");
        }
    }
}