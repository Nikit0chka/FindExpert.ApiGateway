using FindExpert.ApiGateway.GatewayConfig.Advertisement;
using FindExpert.ApiGateway.GatewayConfig.Authorization;
using FindExpert.ApiGateway.GatewayConfig.Users;
using Yarp.ReverseProxy.Configuration;

namespace FindExpert.ApiGateway.GatewayConfig;

internal static class Configuration
{
    public readonly static List<RouteConfig> Routes =
    [
        new()
        {
            RouteId = AdvertisementConfiguration.AdvertisementRouteId,
            ClusterId = AdvertisementConfiguration.ClusterId,
            Match = AdvertisementConfiguration.AdvertisementMatch,
            Transforms = AdvertisementConfiguration.Transforms
        },

        new()
        {
            RouteId = AdvertisementConfiguration.ResponseRouteId,
            ClusterId = AdvertisementConfiguration.ClusterId,
            Match = AdvertisementConfiguration.ResponseMatch,
            Transforms = AdvertisementConfiguration.Transforms
        },
        
        new()
        {
            RouteId = AdvertisementConfiguration.CategoriesRouteId,
            ClusterId = AdvertisementConfiguration.ClusterId,
            Match = AdvertisementConfiguration.CategoriesMatch,
            Transforms = AdvertisementConfiguration.Transforms
        },

        new()
        {
            RouteId = AuthorizationConfiguration.RouteId,
            ClusterId = AuthorizationConfiguration.ClusterId,
            Match = AuthorizationConfiguration.Match,
            Transforms = AuthorizationConfiguration.Transforms
        },

        new()
        {
            RouteId = UsersConfiguration.RouteId,
            ClusterId = UsersConfiguration.ClusterId,
            Match = UsersConfiguration.Match,
            Transforms = UsersConfiguration.Transforms
        }
    ];

    public readonly static List<ClusterConfig> Clusters =
    [
        new()
        {
            ClusterId = AdvertisementConfiguration.ClusterId,
            Destinations = AdvertisementConfiguration.Destinations
        },

        new()
        {
            ClusterId = AuthorizationConfiguration.ClusterId,
            Destinations = AuthorizationConfiguration.Destinations
        },

        new()
        {
            ClusterId = UsersConfiguration.ClusterId,
            Destinations = UsersConfiguration.Destinations
        }
    ];
}