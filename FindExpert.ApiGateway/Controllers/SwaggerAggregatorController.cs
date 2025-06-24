using FindExpert.ApiGateway.GatewayConfig.Advertisement;
using FindExpert.ApiGateway.GatewayConfig.Authorization;
using FindExpert.ApiGateway.GatewayConfig.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;

namespace FindExpert.ApiGateway.Controllers;

[ApiController]
[Route("api/docs")]
public class SwaggerAggregatorController(IHttpClientFactory httpClientFactory):ControllerBase
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    [HttpGet("aggregated")]
    public async Task<IActionResult> GetAggregatedSwagger()
    {
        var serviceUrls = new[]
                          {
                              new
                              {
                                  Url = AuthorizationConfiguration.SwaggerPath,
                                  AuthorizationConfiguration.Tag
                              },
                              new { Url = AdvertisementConfiguration.SwaggerPath, AdvertisementConfiguration.Tag },
                              new { Url = UsersConfiguration.SwaggerPath, UsersConfiguration.Tag }
                          };

        var mergedDoc = new OpenApiDocument
                        {
                            Info = new() { Title = "Aggregated API", Version = "1.0" },
                            Paths = new(),
                            Components = new()
                                         {
                                             SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                                                               {
                                                                   ["Bearer"] = new()
                                                                                {
                                                                                    Type = SecuritySchemeType.Http,
                                                                                    Scheme = "bearer",
                                                                                    BearerFormat = "JWT",
                                                                                    Description = "JWT Authorization header using the Bearer scheme."
                                                                                }
                                                               }
                                         },
                            SecurityRequirements = new List<OpenApiSecurityRequirement>
                                                   {
                                                       new()
                                                       {
                                                           [new()
                                                            {
                                                                Reference = new()
                                                                            { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                                                            }] = []
                                                       }
                                                   },
                            Tags = new List<OpenApiTag>()
                        };

        foreach (var service in serviceUrls)
        {
            var json = await _httpClient.GetStringAsync(service.Url);
            var doc = new OpenApiStringReader().Read(json, out _);

            if (mergedDoc.Tags.All(t => t.Name != service.Tag))
                mergedDoc.Tags.Add(new() { Name = service.Tag });

            foreach (var path in doc.Paths)
            {
                var formattedPath = path.Key.Replace("/api", "");
                mergedDoc.Paths.Add(formattedPath, path.Value);

                foreach (var operation in path.Value.Operations)
                {
                    operation.Value.Tags ??= new List<OpenApiTag>();

                    operation.Value.Tags.Add(new()
                                             { Name = service.Tag });

                    if (operation.Value.Security != null)
                    {
                        operation.Value.Security = doc.SecurityRequirements;
                    }
                }
            }

            foreach (var schema in doc.Components.Schemas)
            {
                mergedDoc.Components.Schemas.TryAdd(schema.Key, schema.Value);
            }
        }

        var outputString = new StringWriter();
        var writer = new OpenApiJsonWriter(outputString);
        mergedDoc.SerializeAsV3(writer);
        return Content(outputString.ToString(), "application/json");
    }
}