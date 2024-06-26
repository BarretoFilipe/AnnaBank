﻿using AnnaBank.Domain;
using AnnaBank.Infra.Interfaces;
using GenericController.Persistence;

namespace AnnaBank.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataBaseContext _context;

        public TransactionRepository(DataBaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task SaveTransaction(Client sender, Client receiver, Transaction transaction)
        {
            _context.Clients.Update(sender);
            _context.Clients.Update(receiver);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}