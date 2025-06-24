using Yarp.ReverseProxy.Configuration;

namespace FindExpert.ApiGateway.GatewayConfig.Advertisement;

//TODO: Разделить по папкам или файлам

internal static class AdvertisementConfiguration
{
    private const string Host = "http://localhost:5002";
    public const string SwaggerPath = $"{Host}/swagger/v1/swagger.json";
    public const string Tag = "Микросервис заявок";
    public const string AdvertisementRouteId = "adverisement-route";
    public const string ResponseRouteId = "response-route";
    public const string CategoriesRouteId = "categories-route";
    public const string ClusterId = "advertisement-cluster";

    public readonly static RouteMatch AdvertisementMatch = new()
                                                           {
                                                               Path = "/advertisement/{**catch-all}"
                                                           };

    public readonly static RouteMatch ResponseMatch = new()
                                                      {
                                                          Path = "/response/{**catch-all}"
                                                      };

    public readonly static RouteMatch CategoriesMatch = new()
                                                        {
                                                            Path = "/categories/{**catch-all}"
                                                        };


    public readonly static IReadOnlyList<Dictionary<string, string>> Transforms =
    [
        new()
        { { ConfigurationConstants.PathPrefix, "/api" } }
    ];


    public readonly static Dictionary<string, DestinationConfig> Destinations = new() { { "advertisement-destination", new() { Address = Host } } };

}