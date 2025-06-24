using Yarp.ReverseProxy.Configuration;

namespace FindExpert.ApiGateway.GatewayConfig.Users;

internal static class UsersConfiguration
{
    private const string Host = "http://localhost:5005";
    public const string SwaggerPath = $"{Host}/swagger/v1/swagger.json";
    public const string Tag = "Микросервис пользователей";
    public const string RouteId = "user-route";
    public const string ClusterId = "user-cluster";

    public readonly static Dictionary<string, DestinationConfig> Destinations = new() { { "user-destination", new() { Address = Host } } };

    public readonly static RouteMatch Match = new()
                                              {
                                                  Path = "/user/{**catch-all}"
                                              };

    public readonly static IReadOnlyList<Dictionary<string, string>> Transforms =
    [
        new()
        { { ConfigurationConstants.PathPrefix, "/api" } }
    ];
}