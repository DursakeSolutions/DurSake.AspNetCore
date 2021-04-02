using Xunit;
using DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Mappers;
using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using Microsoft.Extensions.DependencyInjection;
using DurSake.AspNetCore.ApiVersioning.Mapping.MapperFactory;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests
{
    public class VMapperExtensionsTests
    {
        [Fact]
        public void vMapperExtensions_AddVMapper_ShouldConfigureVMapper()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddVMapper(
                vMapperOptions =>
                    vMapperOptions
                        .RegisterMapper<Mapping.Contracts.v1.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v1.CoinMapper>())
                        .RegisterMapper<Mapping.Contracts.v2.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v2.CoinMapper>()));

            // Assert
            Assert.True(serviceCollection.HasScopedBinding<IMapperFactory, ApiVersioning.Mapping.MapperFactory.MapperFactory>());
            Assert.True(serviceCollection.HasScopedBinding<IVMapper, VMapper>());
            Assert.True(serviceCollection.HasSingletonBinding<Mapping.Mappers.v1.CoinMapper, Mapping.Mappers.v1.CoinMapper>());
            Assert.True(serviceCollection.HasSingletonBinding<Mapping.Mappers.v2.CoinMapper, Mapping.Mappers.v2.CoinMapper>());
        }
    }
}
