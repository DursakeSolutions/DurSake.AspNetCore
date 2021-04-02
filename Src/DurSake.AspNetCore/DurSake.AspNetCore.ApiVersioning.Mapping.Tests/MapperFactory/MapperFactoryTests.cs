using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Mappers;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests.MapperFactory
{
    public class MapperFactoryTests
    {
        [Fact]
        public void MapperFactory_NullConstructorArguments_ShouldThrow()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(null));
            Assert.Throws<ArgumentNullException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(new Mock<IServiceProvider>().Object, (VMapperOptions)null));
            Assert.Throws<ArgumentNullException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(new Mock<IServiceProvider>().Object, (IOptions<VMapperOptions>)null));
            Assert.Throws<ArgumentNullException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(null, new VMapperOptions()));
            Assert.Throws<ArgumentNullException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(null, new Mock<IOptions<VMapperOptions>>().Object));
        }

        [Fact]
        public void MapperFactory_GetMapper_BoundMapper_ShouldGetMapper()
        {
            // Arrange
            var v1CoinMapper = new Mapping.Mappers.v1.CoinMapper();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(It.Is<Type>(x => x == typeof(Mapping.Mappers.v1.CoinMapper))))
                .Returns(v1CoinMapper);

            var vMapperOptions =
                new VMapperOptions()
                    .RegisterMapper<Mapping.Contracts.v1.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v1.CoinMapper>());

            var mapperFactory = new ApiVersioning.Mapping.MapperFactory.MapperFactory(mockServiceProvider.Object, vMapperOptions);

            // Act
            var mapper = mapperFactory.GetMapper<Mapping.Contracts.v1.Coin, Mapping.DTOs.CoinDTO>();

            // Assert
            Assert.Equal(v1CoinMapper, mapper);
        }

        [Fact]
        public void MapperFactory_GetMapper_NoRegisteredMapper_ShouldThrow()
        {
            // Arrange
            var v1CoinMapper = new Mapping.Mappers.v1.CoinMapper();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(provider => provider.GetService(It.Is<Type>(x => x == typeof(Mapping.Mappers.v1.CoinMapper))))
                .Returns(v1CoinMapper);

            var vMapperOptions = new VMapperOptions();

            var mapperFactory = new ApiVersioning.Mapping.MapperFactory.MapperFactory(mockServiceProvider.Object, vMapperOptions);

            // Act
            // Assert
            Assert.Throws<NotImplementedException>(() => mapperFactory.GetMapper<Mapping.Contracts.v1.Coin, Mapping.DTOs.CoinDTO>());
        }

        [Fact]
        public void MapperFactory_GetMapper_NoBoundMapper_ShouldThrow()
        {
            // Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();

            var vMapperOptions =
                new VMapperOptions()
                    .RegisterMapper<Mapping.Contracts.v1.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v1.CoinMapper>());

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => new ApiVersioning.Mapping.MapperFactory.MapperFactory(mockServiceProvider.Object, vMapperOptions));
        }
    }
}
