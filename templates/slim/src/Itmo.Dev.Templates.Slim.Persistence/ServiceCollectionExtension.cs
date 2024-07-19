using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Itmo.Dev.Templates.Slim.Persistence.Implementations;
using Itmo.Dev.Templates.Slim.Persistence.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Templates.Slim.Persistence;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPersistence(persistence => persistence
            .UsePostgres(postgres => postgres
                .WithConnectionOptions(builder => builder.BindConfiguration("Persistence:Postgres"))
                .WithMigrationsFrom(typeof(ServiceCollectionExtension).Assembly)));

        collection.AddScoped<IPersistenceContext, PersistenceContext>();
        return collection;
    }
}