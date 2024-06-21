using AnnaBank.Application.Commands;
using AnnaBank.Domain;
using AnnaBank.Infra.Interfaces;
using AnnaBank.Services;
using FluentAssertions;
using NSubstitute;
using System.Net;

namespace AnnaBankTest.Services
{
    public class TransactionServiceTest
    {
        [Fact]
        public async void TransactionMustBeDeniedBecauseDontHaveSenderId()
        {
            var transactionRepository = Substitute.For<ITransactionRepository>();

            Client? client = null;
            var clientRepository = Substitute.For<IClientRepository>();
            clientRepository.GetById(new Guid()).Returns(client);

            TransactionService transactionService = new(clientRepository, transactionRepository);

            CreateTransactionCommand command = new(10m, new Guid(), "");
            var result = await transactionService.GenerateTransaction(command);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Error?.Message.Should().Be("'Sender Id' not found.");
        }

        [Fact]
        public async void TransactionMustBeDeniedBecauseDontHaveBalance()
        {
            var transactionRepository = Substitute.For<ITransactionRepository>();

            Client? client = new("Rick", "PT5012345678912");
            var clientRepository = Substitute.For<IClientRepository>();
            clientRepository.GetById(new Guid()).Returns(client);

            TransactionService transactionService = new(clientRepository, transactionRepository);

            CreateTransactionCommand command = new(10m, new Guid(), "");
            var result = await transactionService.GenerateTransaction(command);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Error?.Message.Should().Be("Sender has no balance.");
        }

        [Fact]
        public async void TransactionMustBeDeniedBecauseDontHaveIban()
        {
            var transactionRepository = Substitute.For<ITransactionRepository>();

            Client? sender = new("Rick", "PT5012345678912", 100);
            Client? receiver = null;
            var clientRepository = Substitute.For<IClientRepository>();
            clientRepository.GetById(new Guid()).Returns(sender);
            clientRepository.GetByIBAN(string.Empty).Returns(receiver);

            TransactionService transactionService = new(clientRepository, transactionRepository);

            CreateTransactionCommand command = new(10m, new Guid(), "");
            var result = await transactionService.GenerateTransaction(command);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error?.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Error?.Message.Should().Be("'IBAN' not found.");
        }

        [Fact]
        public async void TransactionMustBeDeniedBecauseSenderAndReceiverAreTheSame()
        {
            var transactionRepository = Substitute.For<ITransactionRepository>();

            Client? sender = new("Rick", "PT5012345678912", 100);
            var clientRepository = Substitute.For<IClientRepository>();
            clientRepository.GetById(new Guid()).Returns(sender);
            clientRepository.GetByIBAN(string.Empty).Returns(sender);

            TransactionService transactionService = new(clientRepository, transactionRepository);

            CreateTransactionCommand command = new(10m, new Guid(), "");
            var result = await transactionService.GenerateTransaction(command);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Error?.Message.Should().Be("The sender and receiver can't be the same.");
        }

        [Fact]
        public async void TransactionMustHasSuccess()
        {
            Client? sender = new("Rick", "PT5012345678912", 100);
            Client? receiver = new("Deckard", "PT50987321654987", 200);
            var clientRepository = Substitute.For<IClientRepository>();
            clientRepository.GetById(sender.Id).Returns(sender);
            clientRepository.GetByIBAN(receiver.IBAN).Returns(receiver);

            var transactionRepository = Substitute.For<ITransactionRepository>();
            CreateTransactionCommand command = new(10m, sender.Id, receiver.IBAN);

            TransactionService transactionService = new(clientRepository, transactionRepository);
            var result = await transactionService.GenerateTransaction(command);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }
    }
}