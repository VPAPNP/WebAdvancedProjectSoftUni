using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace EShopWebApp.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Register all services from the project that should be created as follow (I"NameofInterface"Service)
        /// and the implementation of the service ("NameofService"Service)
        /// </summary>
        /// <param name="services">Type of random service</param>
        /// <param name="serviceType"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AddApplicationServices( this IServiceCollection services,Type serviceType)
        {
            Assembly? assembly = Assembly.GetAssembly(serviceType);

            if (assembly == null)
            {
                throw new InvalidOperationException($"Invalid service type provided");
            }
            Type[] serviceTypes = assembly.GetTypes()
                .Where(t=>t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();
            foreach (var service in serviceTypes)
            {
                Type? interfaceType = service.GetInterface($"I{service.Name}");

                if (interfaceType == null)
                {
                    throw new InvalidOperationException($"Invalid interface service type provided {service.Name}");
                }
                services.AddScoped(interfaceType, service);
            }
        }
    }
}
