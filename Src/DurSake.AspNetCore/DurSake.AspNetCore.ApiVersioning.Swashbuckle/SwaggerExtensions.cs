using DurSake.AspNetCore.ApiVersioning.Swashbuckle.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace DurSake.AspNetCore.ApiVersioning.Swashbuckle
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddApiSwaggerGen(
            this IServiceCollection services,
            string apiName,
            Action<SwaggerGenOptions> additionalOptions = null)
        {
            if (string.IsNullOrEmpty(apiName))
            {
                throw new ArgumentNullException(nameof(apiName));
            }

            // Check if ApiVersioning has been setup on the API and add VersionApiExplorer if it has
            if (services.Any(x => x.ServiceType == typeof(IApiVersionRoutePolicy)))
            {
                // Add Versioned Api Explorer to handle in route versioning with swagger
                services.AddVersionedApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'FF";
                        options.SubstituteApiVersionInUrl = true;
                    });
            }

            services.AddOptions();

            // Bind SwaggerGenOptions into IOptions framework
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>(
                serviceProvider =>
                    new ConfigureSwaggerGenOptions(
                        apiName, serviceProvider.GetService<IApiVersionDescriptionProvider>(), additionalOptions));

            // Config for swagger is now coming from the IOptions framework binding above
            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app, string apiName)
        {
            if (string.IsNullOrEmpty(apiName))
            {
                throw new ArgumentNullException(nameof(apiName));
            }

            app.UseSwagger();
            app.UseSwaggerUI(
                swaggerUiOptions =>
                {
                // This verifies whether ApiVersioning has been setup on the API
                if (app.ApplicationServices.GetService<IApiVersionRoutePolicy>() == null)
                    {
                        swaggerUiOptions.SwaggerEndpoint("../swagger/v1.0/swagger.json", $"{apiName} API");
                        swaggerUiOptions.RoutePrefix = "swagger";
                    }
                    else
                    {
                        var apiVersionDescriptionProvider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

                        if (apiVersionDescriptionProvider == null)
                        {
                            throw new ArgumentNullException(
                                $"Unable to find {nameof(IApiVersionDescriptionProvider)}.  Be sure {nameof(AddApiSwaggerGen)} method was called after ApiVersioning was configured on the API");
                        }

                        apiVersionDescriptionProvider.ApiVersionDescriptions
                            .OrderByDescending(x => x.ApiVersion.MajorVersion).ThenByDescending(x => x.ApiVersion.MinorVersion).ToList()
                            .ForEach(description =>
                            {
                                swaggerUiOptions.SwaggerEndpoint(
                                    $"../swagger/{description.GroupName}/swagger.json",
                                    $"{apiName} API {description.GroupName.ToUpperInvariant()} {(description.IsDeprecated ? "-(DEPRECATED)" : "")}");
                                swaggerUiOptions.RoutePrefix = "swagger";
                            });
                    }

                    swaggerUiOptions.DisplayRequestDuration();
                });

            return app;
        }
    }
}
