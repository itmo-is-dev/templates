using Itmo.Dev.Platform.Postgres.Plugins;
using Npgsql;

namespace Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Plugins;

/// <summary>
///     Plugin for configuring NpgsqlDataSource's mappings
///     ie: enums, composite types
/// </summary>
public class MappingPlugin : IDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder builder) { }
}