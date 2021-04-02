using DurSake.AspNetCore.ApiVersioning.Swashbuckle.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Web.Http.Description;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Web.Http.Description;
using Xunit;

namespace DurSake.AspNetCore.ApiVersioning.Swashbuckle.Tests
{
    public class SwaggerExtensionsTests
    {
        [Fact]
        public void SwaggerExtensions_AddApiSwaggerGen_WithVersioning_ShouldConfigureSwaggerGen()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection
                .AddApiVersioning(o =>
                {
                    o.ReportApiVersions = true;
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.ApiVersionSelector = new CurrentImplementationApiVersionSelector(o);
                })
                .AddApiSwaggerGen("TestAPI");

            // Assert
            Assert.True(serviceCollection.HasSingletonBinding<IApiVersionRoutePolicy, DefaultApiVersionRoutePolicy>());
            Assert.True(serviceCollection.HasSingletonBinding<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>());
            Assert.True(serviceCollection.HasTransientBinding<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>());
        }
    }
}
