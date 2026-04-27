#pragma warning disable CA1506

#if BackgroundTasksEnabled
using Itmo.Dev.Platform.BackgroundTasks.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Hangfire.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Hangfire.Postgres.Extensions;
using Itmo.Dev.Platform.BackgroundTasks.Postgres.Extensions;
#endif
using Itmo.Dev.Platform.Common.Extensions;
#if MessagePersistenceEnabled
using Itmo.Dev.Platform.MessagePersistence;
using Itmo.Dev.Platform.MessagePersistence.Postgres.Extensions;
#endif
#if KafkaEnabled
using Itmo.Dev.Platform.Kafka.Extensions;
#endif
using Itmo.Dev.Platform.Observability;
using Itmo.Dev.Templates.Hexagonal.Application.Extensions;
#if KafkaEnabled
using Itmo.Dev.Templates.Hexagonal.Infrastructure.Kafka;
#endif
using Itmo.Dev.Templates.Hexagonal.Infrastructure.Persistence.Extensions;
#if KafkaEnabled
using Itmo.Dev.Templates.Hexagonal.Presentation.Kafka;
#endif
#if HttpEnabled
using Itmo.Dev.Templates.Hexagonal.Presentation.Http.Extensions;
#endif
#if GrpcEnabled
using Itmo.Dev.Templates.Hexagonal.Presentation.Grpc.Extensions;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddPlatform(platform => platform
    .WithNewtonsoftSerialization());

builder.AddPlatformObservability();

builder.Services.AddApplication();
builder.Services.AddInfrastructurePersistence();
#if GrpcEnabled
builder.Services.AddPresentationGrpc();
#endif
#if KafkaEnabled
builder.Services.AddPlatformKafka(kafka => kafka
    .ConfigureOptions(builder.Configuration.GetSection("Presentation:Kafka"))
    .AddInfrastructureKafkaProducers(builder.Configuration)
    .AddPresentationKafkaConsumers(builder.Configuration));
#endif
#if HttpEnabled
builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddPresentationHttp();

builder.Services.AddSwaggerGen().AddEndpointsApiExplorer();
#endif

#if BackgroundTasksEnabled
builder.Services.AddPlatformBackgroundTasks(configurator => configurator
    .UsePostgresPersistence(postgres => postgres.BindConfiguration("Infrastructure:BackgroundTasks:Persistence"))
    .ConfigureScheduling(scheduling => scheduling.BindConfiguration("Infrastructure:BackgroundTasks:Scheduling"))
    .UseHangfireScheduling(hangfire => hangfire
        .ConfigureOptions(o => o.BindConfiguration("Infrastructure:BackgroundTasks:Scheduling:Hangfire"))
        .UsePostgresJobStorage())
    .ConfigureExecution(builder.Configuration.GetSection("Infrastructure:BackgroundTasks:Execution"))
    .AddApplicationBackgroundTasks());
#endif

#if MessagePersistenceEnabled
builder.Services.AddPlatformMessagePersistence(configurator => configurator
    .WithDefaultPublisherOptions("Infrastructure:MessagePersistence:Publishers:Default")
    .UsePostgresPersistence(postgres => postgres
        .ConfigureOptions("Infrastructure:MessagePersistence:Persistence")));
#endif

builder.Services.AddUtcDateTimeProvider();

WebApplication app = builder.Build();

app.UseRouting();
#if HttpEnabled
app.UseSwagger();
app.UseSwaggerUI();
#endif

app.UsePlatformObservability();

#if GrpcEnabled
app.UsePresentationGrpc();
#endif
#if HttpEnabled
app.MapControllers();
#endif

await app.RunAsync();