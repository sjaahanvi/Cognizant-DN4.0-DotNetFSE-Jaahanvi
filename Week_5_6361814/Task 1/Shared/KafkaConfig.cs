using Confluent.Kafka;

namespace KafkaChat.Shared
{
    public static class KafkaConfig
    {
        public const string BOOTSTRAP_SERVERS = "localhost:9092";
        public const string CHAT_TOPIC = "chat-topic";
        public const string CONSUMER_GROUP = "chat-consumer-group";

        public static ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = BOOTSTRAP_SERVERS,
                MessageTimeoutMs = 10000,
                RequestTimeoutMs = 10000
            };
        }

        public static ConsumerConfig GetConsumerConfig(string groupId = null)
        {
            return new ConsumerConfig
            {
                BootstrapServers = BOOTSTRAP_SERVERS,
                GroupId = groupId ?? CONSUMER_GROUP,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 1000
            };
        }
    }
}
