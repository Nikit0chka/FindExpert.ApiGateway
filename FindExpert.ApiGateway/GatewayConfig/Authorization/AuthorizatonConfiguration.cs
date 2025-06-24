using Yarp.ReverseProxy.Configuration;

namespace FindExpert.ApiGateway.GatewayConfig.Authorization;

internal static class AuthorizationConfiguration
{
    private const string Host = "http://localhost:5001";
    public const string SwaggerPath = $"{Host}/swagger/v1/swagger.json";
    public const string Tag = "Микросервис авторизации";
    public const string ClusterId = "authorization-cluster";
    public const string RouteId = "authorization-route";

    public readonly static RouteMatch Match = new()
                                              {
                                                  Path = "/authorization/{**catch-all}"
                                              };

    public readonly static IReadOnlyList<Dictionary<string, string>> Transforms =
    [
        new()
        { { ConfigurationConstants.PathPrefix, "/api" } }
    ];

    public readonly static Dictionary<string, DestinationConfig> Destinations = new() { { "authorization-destination", new() { Address = Host } } };
}