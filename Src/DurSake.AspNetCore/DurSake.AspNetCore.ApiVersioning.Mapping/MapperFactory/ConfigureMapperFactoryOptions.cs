using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using Microsoft.Extensions.Options;
using System;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory
{
    public class ConfigureMapperFactoryOptions : IConfigureOptions<VMapperOptions>
    {
        private readonly Action<VMapperOptions> setupAction;

        public ConfigureMapperFactoryOptions(Action<VMapperOptions> setupAction)
        {
            this.setupAction = setupAction;
        }

        public void Configure(VMapperOptions mapperFactoryOptions)
        {
            setupAction.Invoke(mapperFactoryOptions);
        }
    }
}