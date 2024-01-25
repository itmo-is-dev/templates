#pragma warning disable CA1506

#if BackgroundTasksEnabled
using Itmo.Dev.Platform.BackgroundTasks.Extensions;
#endif
using Itmo.Dev.Platform.Common.Extensions;
#if KafkaEnabled
using Itmo.Dev.Platform.Events;
#endif
using Itmo.Dev.Platform.Logging.Extensions;
#if YandexCloudEnabled
using Itmo.Dev.Platform.YandexCloud.Extensions;
#endif
using Itmo.Dev.Templates.Hexagonal.Application.Extensions;
using Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Extensions;
using Itmo.Dev.Templates.Hexagonal.Presentation.Http.Extensions;
#if GrpcEnabled
using Itmo.Dev.Templates.Hexagonal.Presentation.Grpc.Extensions;
#endif
#if KafkaEnabled
using Itmo.Dev.Templates.Hexagonal.Presentation.Kafka.Extensions;
#endif
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
#if YandexCloudEnabled
await builder.AddYandexCloudConfigurationAsync();
#endif

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddApplication();
builder.Services.AddInfrastructurePersistence();
#if GrpcEnabled
builder.Services.AddPresentationGrpc();
#endif
#if KafkaEnabled
builder.Services.AddPresentationKafka(builder.Configuration);
#endif

builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddPresentationHttp();

#if BackgroundTasksEnabled
builder.Services.AddPlatformBackgroundTasks(configurator => configurator
    .ConfigurePersistence(builder.Configuration.GetSection("Infrastructure:BackgroundTasks:Persistence"))
    .ConfigureScheduling(builder.Configuration.GetSection("Infrastructure:BackgroundTasks:Scheduling"))
    .ConfigureExecution(builder.Configuration.GetSection("Infrastructure:BackgroundTasks:Execution"))
    .AddApplicationBackgroundTasks());
#endif

#if KafkaEnabled
builder.Services.AddPlatformEvents(b => b.AddPresentationKafkaHandlers());
#endif

builder.Host.AddPlatformSerilog(builder.Configuration);
builder.Services.AddUtcDateTimeProvider();
#if SentryEnabled
builder.AddPlatformSentry();
#endif

WebApplication app = builder.Build();

#if BackgroundTasksEnabled
await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UsePlatformBackgroundTasksAsync(default);
}
#endif

app.UseRouting();
#if SentryEnabled
app.UsePlatformSentryTracing(app.Configuration);
#endif

#if GrpcEnabled
app.UsePresentationGrpc();
#endif

await app.RunAsync();