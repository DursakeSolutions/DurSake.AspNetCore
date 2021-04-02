using Xunit;
using DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.Mappers;
using DurSake.AspNetCore.ApiVersioning.Mapping.Models;
using Microsoft.Extensions.DependencyInjection;
using DurSake.AspNetCore.ApiVersioning.Mapping.Tests.Mapping.DTOs;
using System;
using System.Reflection;

namespace DurSake.AspNetCore.ApiVersioning.Mapping.Tests
{
    public class VMapperTests
    {
        [Fact]
        public void vMapper_NullConstructorArguments_ShouldThrow()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new VMapper(null));
        }

        [Fact]
        public void vMapper_ValidMapperSetup_ShouldMap()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddVMapper(
                vMapperOptions =>
                    vMapperOptions
                        .RegisterMapper<Mapping.Contracts.v1.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v1.CoinMapper>())
                        .RegisterMapper<Mapping.Contracts.v2.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v2.CoinMapper>()));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var vMapper = serviceProvider.GetService<IVMapper>();

            var coinDTO = new CoinDTO
            {
                Type = CoinTypeDTO.Quarter,
                Country = CountryDTO.UnitedStates,
                Value = .25,
                Description = "Its A Quarter!",
                Year = 2015
            };

            // Act
            var mappedV1Quarter = vMapper.Map<Mapping.Contracts.v1.Coin>(coinDTO);
            var mappedV2Quarter = vMapper.Map<Mapping.Contracts.v2.Coin>(coinDTO);

            // Assert
            Assert.NotNull(mappedV1Quarter);
            Assert.NotNull(mappedV2Quarter);
        }

        [Fact]
        public void vMapper_MissingMapperSetup_ShouldThrow()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddVMapper(
                vMapperOptions =>
                    vMapperOptions
                        .RegisterMapper<Mapping.Contracts.v1.Coin>(MapperRegistry.Create<ICoinMapper, Mapping.Mappers.v1.CoinMapper>()));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var vMapper = serviceProvider.GetService<IVMapper>();

            var coinDTO = new CoinDTO
            {
                Type = CoinTypeDTO.Quarter,
                Country = CountryDTO.UnitedStates,
                Value = .25,
                Description = "Its A Quarter!",
                Year = 2015
            };

            // Act
            // Assert
            Assert.Throws<TargetInvocationException>(() => vMapper.Map<Mapping.Contracts.v2.Coin>(coinDTO));
        }
    }
}
