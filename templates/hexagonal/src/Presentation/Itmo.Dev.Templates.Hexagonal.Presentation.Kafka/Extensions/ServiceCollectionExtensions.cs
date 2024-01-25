using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Itmo.Dev.Templates.Hexagonal.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationKafka(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string consumerKey = "Presentation:Kafka:Consumers";
        const string producerKey = "Presentation:Kafka:Producers";

        string group = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        // TODO: add consumers and producers
        // consumer example:
        // .AddConsumer<MessageKey, MessageValue>(selector => selector
        //     .HandleWith<MessageHandler>()
        //     .DeserializeKeyWithProto()
        //     .DeserializeValueWithProto()
        //     .UseNamedOptionsConfiguration(
        //         "MessageName",
        //         configuration.GetSection($"{consumerKey}:MessageName"),
        //         c => c.WithGroup(group)))
        //
        // producer example:
        // .AddProducer<MessageKey, MessageValue>(selector => selector
        //     .SerializeKeyWithProto()
        //     .SerializeValueWithProto()
        //     .UseNamedOptionsConfiguration(
        //         "MessageName",
        //         configuration.GetSection($"{producerKey}:MessageName")))
        collection.AddKafka(builder => builder
            .ConfigureOptions(b => b.BindConfiguration("Presentation:Kafka")));

        return collection;
    }
}