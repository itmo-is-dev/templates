using Itmo.Dev.Platform.Kafka.Configuration;
using Microsoft.Extensions.Configuration;

namespace Itmo.Dev.Templates.Hexagonal.Presentation.Kafka;

public static class ServiceCollectionExtensions
{
    public static IKafkaConfigurationBuilder AddPresentationKafkaConsumers(
        this IKafkaConfigurationBuilder kafka,
        IConfiguration configuration)
    {
        const string consumerKey = "Presentation:Kafka:Consumers";
        configuration = configuration.GetSection(consumerKey);

        // TODO: add consumers
        // .AddConsumer(b => b
        //     .WithKey<MessageKey>()
        //     .WithValue<MessageValue>()
        //     .WithConfiguration(configuration.GetSection("MessageName"))
        //     .DeserializeKeyWithProto()
        //     .DeserializeValueWithProto()
        //     .HandleWith<MessageHandler>())
        return kafka;
    }
}
