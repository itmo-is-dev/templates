using Microsoft.AspNetCore.Builder;

namespace Itmo.Dev.Templates.Hexagonal.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePresentationGrpc(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(routeBuilder =>
        {
            // TODO: add gRPC services implementation
            // routeBuilder.MapGrpcService<Sample>();
            routeBuilder.MapGrpcReflectionService();
        });

        return builder;
    }
}