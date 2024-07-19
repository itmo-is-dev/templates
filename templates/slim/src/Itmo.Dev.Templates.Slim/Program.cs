using Itmo.Dev.Platform.Common.Extensions;
using Itmo.Dev.Platform.Logging.Extensions;
using Itmo.Dev.Templates.Slim.Application;
using Itmo.Dev.Templates.Slim.Persistence;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
#if YandexCloudEnabled
using Itmo.Dev.Platform.YandexCloud.Extensions;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
#if YandexCloudEnabled
await builder.AddYandexCloudConfigurationAsync();
#endif

builder.Services.AddOptions<JsonSerializerSettings>();
builder.Services.AddSingleton(p => p.GetRequiredService<IOptions<JsonSerializerSettings>>().Value);

builder.Services.AddPlatform();
builder.Services.AddUtcDateTimeProvider();
builder.Host.AddPlatformSerilog(builder.Configuration);

builder.Services.AddPersistence();
builder.Services.AddApplication(builder.Configuration);

#if SentryEnabled
builder.AddPlatformSentry();
#endif

WebApplication app = builder.Build();

app.UseRouting();
app.UseApplication();

#if SentryEnabled
app.UsePlatformSentryTracing(app.Configuration);
#endif

app.Run();