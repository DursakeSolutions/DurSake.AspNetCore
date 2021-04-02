using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory
{
    public class MapperFactory : IMapperFactory
    {
        private readonly VMapperOptions vMapperOptions;
        private readonly IDictionary<Type, object> registeredMappers;

        public MapperFactory(IServiceProvider serviceProvider, VMapperOptions vMapperOptions)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (vMapperOptions == null)
            {
                throw new ArgumentNullException(nameof(vMapperOptions));
            }

            registeredMappers = ExtractRegisterMappers(serviceProvider, vMapperOptions);
        }

        public MapperFactory(IServiceProvider serviceProvider, IOptions<VMapperOptions> vMapperOptions = null)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.vMapperOptions = vMapperOptions?.Value ?? serviceProvider.GetService<IOptions<VMapperOptions>>()?.Value;

            if (this.vMapperOptions == null)
            {
                throw new ArgumentNullException(nameof(vMapperOptions));
            }

            registeredMappers = ExtractRegisterMappers(serviceProvider, this.vMapperOptions);
        }

        public IMapper<TDto> GetMapper<T, TDto>()
        {
            return registeredMappers.TryGetValue(typeof(T), out var registeredMapper) ?
                (IMapper<TDto>)registeredMapper :
                throw new NotImplementedException($"No registered mapper found for {typeof(T).FullName}");
        }

        private static Dictionary<Type, object> ExtractRegisterMappers(IServiceProvider serviceProvider, VMapperOptions vMapperOptions)
        {
            var registeredMappers = new Dictionary<Type, object>();

            if (vMapperOptions.RegisteredMappers != null)
            {
                foreach (var mapper in vMapperOptions.RegisteredMappers)
                {
                    registeredMappers.Add(mapper.Key, serviceProvider.GetRequiredService(mapper.Value.TImplementation));
                }
            }

            return registeredMappers;
        }
    }
}
