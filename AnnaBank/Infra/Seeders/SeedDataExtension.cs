using AnnaBank.Domain;
using GenericController.Persistence;

namespace AnnaBank.Infra.Seeders
{
    public static class SeedDataExtension
    {
        public static void PopulateSeeder(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            AddInitialData(serviceScope.ServiceProvider.GetService<DataBaseContext>()!);
        }

        private static void AddInitialData(DataBaseContext context)
        {
            if (context.Clients.Any())
            {
                return;
            }

            Client rick = new Client("Rick", "PT50111222333444", 100);
            Client deckard = new Client("Deckard", "PT50555666777888", 200);
            Client obiwan = new Client("Obiwan", "PT50112233445566", 300);

            context.Clients.AddRange(new[] { rick, deckard, obiwan });
            context.SaveChanges();
        }
    }
}