using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace DurSake.AspNetCore.ApiVersioning.Swashbuckle.Options
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly string apiName;
        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;
        private readonly Action<SwaggerGenOptions> additionalOptions;

        public ConfigureSwaggerGenOptions(
            string apiName,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            Action<SwaggerGenOptions> additionalOptions = null)
        {
            if (string.IsNullOrEmpty(apiName))
            {
                throw new ArgumentNullException(nameof(apiName));
            }

            this.apiName = apiName;
            this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
            this.additionalOptions = additionalOptions;
        }

        public void Configure(SwaggerGenOptions swaggerGenOptions)
        {
            if (apiVersionDescriptionProvider?.ApiVersionDescriptions == null)
            {
                swaggerGenOptions.SwaggerDoc(
                    "v1.0",
                    new OpenApiInfo
                    {
                        Title = $"{apiName} API",
                        Version = "1.0"
                    });
            }
            else
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    swaggerGenOptions.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo
                        {
                            Title = $"{apiName} API {description.GroupName.ToUpperInvariant()}",
                            Version = description.ApiVersion.ToString()
                        });
                }
            }

            swaggerGenOptions.CustomSchemaIds(type => $"{type.DeclaringType?.Name}{type.Name}");

            additionalOptions?.Invoke(swaggerGenOptions);
        }
    }
}
