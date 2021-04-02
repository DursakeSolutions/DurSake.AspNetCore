using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DurSake.AspNetCore.ApiVersioning.Swashbuckle.Tests
{
    public static class TestAssertionExtensions
    {
        public static bool HasSingletonBinding<TService, TImplementation>(this IServiceCollection serviceCollection)
        {
            return serviceCollection.HasBinding(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        }

        public static bool HasScopedBinding<TService, TImplementation>(this IServiceCollection serviceCollection)
        {
            return serviceCollection.HasBinding(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
        }

        public static bool HasTransientBinding<TService, TImplementation>(this IServiceCollection serviceCollection)
        {
            return serviceCollection.HasBinding(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        }

        public static bool HasBinding<TService, TImplementation>(this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime)
        {
            return serviceCollection.HasBinding(typeof(TService), typeof(TImplementation), serviceLifetime);
        }

        public static bool HasBinding(this IServiceCollection serviceCollection, Type tService, Type tImplementation, ServiceLifetime serviceLifetime)
        {
            return serviceCollection.Any(x => x.ServiceType == tService && x.ImplementationType == tImplementation && x.Lifetime == serviceLifetime);
        }
    }
}
