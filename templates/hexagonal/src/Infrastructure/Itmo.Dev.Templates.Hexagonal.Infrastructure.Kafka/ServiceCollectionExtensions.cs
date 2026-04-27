using Itmo.Dev.Platform.Kafka.Configuration;
using Microsoft.Extensions.Configuration;

namespace Itmo.Dev.Templates.Hexagonal.Infrastructure.Kafka;

public static class ServiceCollectionExtensions
{
    public static IKafkaConfigurationBuilder AddInfrastructureKafkaProducers(
        this IKafkaConfigurationBuilder kafka,
        IConfiguration configuration)
    {
        const string producerKey = "Presentation:Kafka:Producers";
        configuration = configuration.GetSection(producerKey);

        // TODO: add producers
        // .AddProducer(b => b
        //     .WithKey<MessageKey>()
        //     .WithValue<MessageValue>()
        //     .WithConfiguration(configuration.GetSection($"{producerKey}:MessageName"))
        //     .SerializeKeyWithProto()
        //     .SerializeValueWithProto())
        return kafka;
    }
}
