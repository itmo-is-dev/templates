using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.Plugins;
using Itmo.Dev.Templates.Hexagonal.Application.Abstractions.Persistence;
using Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Migrations;
using Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPostgres(builder => builder.BindConfiguration("Infrastructure:Persistence:Postgres"));
        collection.AddSingleton<IDataSourcePlugin, MappingPlugin>();

        collection.AddPlatformMigrations(typeof(IAssemblyMarker).Assembly);
        collection.AddHostedService<MigrationRunnerService>();

        // TODO: add repositories
        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        return collection;
    }
}