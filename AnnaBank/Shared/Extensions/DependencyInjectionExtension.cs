using AnnaBank.Infra.Interfaces;
using System.Reflection;

namespace AnnaBank.Shared.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            var respositoryInterfaces = typeof(IRepository).Assembly.GetTypes()
                .Where(type =>
                    type.IsInterface
                    && type.GetInterfaces().Any(inter => inter.Name == nameof(IRepository)
                    )
                )
                .ToList();

            var types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var repositoryInterface in respositoryInterfaces)
            {
                //var repositoryImplementation = types.Where(type => type.IsAssignableFrom(repositoryInterface)).SingleOrDefault();
                var repositoryImplementation = types.Where(type => type.IsClass && repositoryInterface.IsAssignableFrom(type)).SingleOrDefault();
                if (repositoryImplementation != null)
                {
                    services.AddScoped(repositoryInterface, repositoryImplementation);
                }
            }
        }
    }
}