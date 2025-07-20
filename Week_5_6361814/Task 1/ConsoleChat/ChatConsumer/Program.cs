using Confluent.Kafka;
using KafkaChat.Shared;

namespace ChatConsumer
{
    class Program
    {
        private static IConsumer<Null, string>? _consumer;
        private static CancellationTokenSource _cancellationTokenSource = new();
        private static string _consumerName = string.Empty;

        static async Task Main(string[] args)
        {
            Console.Title = "Kafka Chat Consumer";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("=== KAFKA CHAT CONSUMER ===");
            Console.WriteLine("This application receives chat messages from Kafka topic");
            Console.WriteLine();
            Console.ResetColor();

            // Get consumer name for identification
            Console.Write("Enter your name (for identification): ");
            _consumerName = Console.ReadLine() ?? "Anonymous";

            // Setup console cancellation
            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true;
                _cancellationTokenSource.Cancel();
            };

            try
            {
                await InitializeConsumer();
                Console.WriteLine($"\nHello {_consumerName}! Listening for chat messages...");
                Console.WriteLine("Press Ctrl+C to stop listening");
                Console.WriteLine(new string('-', 50));

                await StartListening();
            }
            catch (OperationCanceledException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nStopping chat consumer...");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("\nMake sure Kafka is running on localhost:9092");
                Console.ResetColor();
            }
            finally
            {
                _consumer?.Close();
                _consumer?.Dispose();
                Console.WriteLine("\nConsumer stopped. Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static async Task InitializeConsumer()
        {
            Console.WriteLine("Connecting to Kafka...");
            
            // Create unique consumer group for this instance
            var consumerGroup = $"{KafkaConfig.CONSUMER_GROUP}-{_consumerName}-{Guid.NewGuid().ToString("N")[..8]}";
            var config = KafkaConfig.GetConsumerConfig(consumerGroup);
            
            _consumer = new ConsumerBuilder<Null, string>(config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ Assigned partitions: [{string.Join(", ", partitions)}]");
                    Console.ResetColor();
                })
                .Build();

            _consumer.Subscribe(KafkaConfig.CHAT_TOPIC);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Connected to Kafka successfully!");
            Console.WriteLine($"✓ Subscribed to topic: {KafkaConfig.CHAT_TOPIC}");
            Console.WriteLine($"✓ Consumer group: {consumerGroup}");
            Console.ResetColor();

            // Small delay to ensure subscription is established
            await Task.Delay(1000);
        }

        private static async Task StartListening()
        {
            var messageCount = 0;
            var lastHeartbeat = DateTime.Now;

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    // Show heartbeat every 30 seconds to indicate the consumer is alive
                    if (DateTime.Now - lastHeartbeat > TimeSpan.FromSeconds(30))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Consumer '{_consumerName}' is listening... (Messages received: {messageCount})");
                        Console.ResetColor();
                        lastHeartbeat = DateTime.Now;
                    }

                    var consumeResult = _consumer!.Consume(TimeSpan.FromSeconds(1));
                    
                    if (consumeResult?.Message?.Value != null)
                    {
                        await ProcessMessage(consumeResult);
                        messageCount++;
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Consume error: {ex.Error.Reason}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        private static async Task ProcessMessage(ConsumeResult<Null, string> consumeResult)
        {
            try
            {
                var chatMessage = ChatMessage.FromJson(consumeResult.Message.Value);
                
                // Color-code different types of messages
                if (chatMessage.Message.Contains("joined the chat"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (chatMessage.Message.Contains("left the chat"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // Display the message with additional info
                Console.WriteLine($"{chatMessage} [P:{consumeResult.Partition} O:{consumeResult.Offset}]");
                Console.ResetColor();

                // Show message details in debug mode (optional)
                if (args.Contains("--debug"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"  └─ ID: {chatMessage.MessageId}, Timestamp: {chatMessage.Timestamp}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to parse message: {ex.Message}");
                Console.WriteLine($"Raw message: {consumeResult.Message.Value}");
                Console.ResetColor();
            }

            await Task.CompletedTask; // For async consistency
        }

        private static string[] args = Environment.GetCommandLineArgs();
    }
}
