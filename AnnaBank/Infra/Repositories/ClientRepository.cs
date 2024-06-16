using AnnaBank.Domain;
using AnnaBank.Infra.Interfaces;
using GenericController.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AnnaBank.Infra.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DatabaseContext _context;

        public ClientRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        public async Task<Client?> GetById(Guid id)
        {
            return await _context.Clients
                .AsQueryable()
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Client?> GetByIBAN(string iban)
        {
            return await _context.Clients
                .AsQueryable()
               .FirstOrDefaultAsync(x => x.IBAN == iban);
        }

        public async Task Create(Client client)
        {
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }
    }
}