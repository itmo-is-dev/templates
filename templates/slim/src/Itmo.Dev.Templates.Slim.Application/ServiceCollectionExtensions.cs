#if HttpEnabled
using FastEndpoints;
#endif
#if KafkaEnabled
using Itmo.Dev.Platform.Kafka.Extensions;
#endif
#if BackgroundTasksEnabled
using Itmo.Dev.Platform.BackgroundTasks.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Hangfire.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Hangfire.Postgres.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Postgres.Extensions;
#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Templates.Slim.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection, IConfiguration configuration)
    {
#if HttpEnabled
        collection.AddFastEndpoints();
        collection.AddControllers().AddNewtonsoftJson();
        collection.AddSwaggerGen().AddEndpointsApiExplorer().AddSwaggerGenNewtonsoftSupport();
#endif
#if KafkaEnabled
        collection.AddPlatformKafka(kafka => kafka
            .ConfigureOptions(configuration.GetSection("Kafka")));
#endif
#if GrpcEnabled
        collection.AddGrpc();
        collection.AddGrpcReflection();
#endif
#if BackgroundTasksEnabled
        collection.AddPlatformBackgroundTasks(configurator => configurator
            .UsePostgresPersistence(postgres => postgres.BindConfiguration("BackgroundTasks:Postgres"))
            .ConfigureScheduling(scheduling => scheduling.BindConfiguration("BackgroundTasks:Scheduling"))
            .UseHangfireScheduling(hangfire => hangfire
                .ConfigureOptions(builder => builder.BindConfiguration("BackgroundTasks:Hangfire"))
                .UsePostgresJobStorage())
            .ConfigureExecution(configuration.GetSection("BackgroundTasks:Execution")));
#endif
        return collection;
    }

    public static WebApplication UseApplication(this WebApplication app)
    {
#if HttpEnabled
        app.UseFastEndpoints();
        app.UseSwagger();
        app.UseSwaggerUI();
#endif
#if GrpcEnabled
        app.UseEndpoints(builder =>
        {
            builder.MapGrpcReflectionService();
        });
#endif
        return app;
    }
}