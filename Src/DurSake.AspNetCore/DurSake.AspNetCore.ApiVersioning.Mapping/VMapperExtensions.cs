using DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory;
using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace DurSake.AspNetCore.ApiVersioning.Mapping
{
    public static class VMapperExtensions
    {
        public static IServiceCollection AddVMapper(this IServiceCollection services, Action<VMapperOptions> setupAction)
        {
            // Configure DI for VMapper including MapperFactory and Options
            services.AddOptions();
            services.AddSingleton<IConfigureOptions<VMapperOptions>, ConfigureMapperFactoryOptions>(s => new ConfigureMapperFactoryOptions(setupAction));
            services.TryAddScoped<IMapperFactory, MapperFactory.MapperFactory>();
            services.TryAddScoped<IVMapper, VMapper>();
            
            var mapperFactoryOptions = new VMapperOptions();
            setupAction.Invoke(mapperFactoryOptions);

            // Configure DI for all Registered Mappers
            foreach (var mapper in mapperFactoryOptions.RegisteredMappers)
            {
                services.TryAddSingleton(mapper.Value.TImplementation);
            }

            return services;
        }

        public static VMapperOptions RegisterMapper<T>(this VMapperOptions vMapperOptions, MapperRegistry mapperRegistry)
        {
            return vMapperOptions.RegisterMapper(typeof(T), mapperRegistry);
        }

        public static VMapperOptions RegisterMapper(this VMapperOptions vMapperOptions, Type mapTo, MapperRegistry mapperRegistry)
        {
            if (vMapperOptions.RegisteredMappers == null)
            {
                vMapperOptions.RegisteredMappers =
                    new Dictionary<Type, MapperRegistry>() { { mapTo, mapperRegistry } };
            }
            else
            {
                vMapperOptions.RegisteredMappers.Add(new KeyValuePair<Type, MapperRegistry>(mapTo, mapperRegistry));
            }

            return vMapperOptions;
        }
    }
}
