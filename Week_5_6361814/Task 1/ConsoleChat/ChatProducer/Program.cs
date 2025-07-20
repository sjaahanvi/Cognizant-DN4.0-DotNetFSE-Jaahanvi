using Confluent.Kafka;
using KafkaChat.Shared;

namespace ChatProducer
{
    class Program
    {
        private static IProducer<Null, string>? _producer;
        private static string _username = string.Empty;

        static async Task Main(string[] args)
        {
            Console.Title = "Kafka Chat Producer";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=== KAFKA CHAT PRODUCER ===");
            Console.WriteLine("This application sends chat messages to Kafka topic");
            Console.WriteLine();
            Console.ResetColor();

            // Get username
            Console.Write("Enter your username: ");
            _username = Console.ReadLine() ?? "Anonymous";

            // Initialize Kafka producer
            try
            {
                await InitializeProducer();
                Console.WriteLine($"\nWelcome {_username}! You can start sending messages.");
                Console.WriteLine("Type 'exit' to quit, 'clear' to clear screen");
                Console.WriteLine(new string('-', 50));

                await StartChatLoop();
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
                _producer?.Dispose();
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        private static async Task InitializeProducer()
        {
            Console.WriteLine("Connecting to Kafka...");
            
            var config = KafkaConfig.GetProducerConfig();
            _producer = new ProducerBuilder<Null, string>(config).Build();

            // Test connection by sending a test message
            var testMessage = new ChatMessage(_username, "joined the chat");
            await _producer.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                new Message<Null, string> { Value = testMessage.ToJson() });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Connected to Kafka successfully!");
            Console.ResetColor();
        }

        private static async Task StartChatLoop()
        {
            while (true)
            {
                Console.Write($"{_username}> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                // Handle special commands
                switch (input.ToLower())
                {
                    case "exit":
                        await SendLeaveMessage();
                        return;
                    case "clear":
                        Console.Clear();
                        Console.WriteLine($"Welcome back {_username}! Continue chatting...");
                        continue;
                    case "help":
                        ShowHelp();
                        continue;
                }

                // Send regular chat message
                await SendChatMessage(input);
            }
        }

        private static async Task SendChatMessage(string messageText)
        {
            try
            {
                var chatMessage = new ChatMessage(_username, messageText);
                var result = await _producer!.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                    new Message<Null, string> { Value = chatMessage.ToJson() });

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"✓ Message sent (Partition: {result.Partition}, Offset: {result.Offset})");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"✗ Failed to send message: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static async Task SendLeaveMessage()
        {
            try
            {
                var leaveMessage = new ChatMessage(_username, "left the chat");
                await _producer!.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                    new Message<Null, string> { Value = leaveMessage.ToJson() });
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("✓ Goodbye message sent!");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Failed to send goodbye message: {ex.Message}");
                Console.ResetColor();
            }
        }

        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== HELP ===");
            Console.WriteLine("Commands:");
            Console.WriteLine("  exit  - Leave the chat and quit");
            Console.WriteLine("  clear - Clear the screen");
            Console.WriteLine("  help  - Show this help message");
            Console.WriteLine("\nJust type any message to send it to the chat!");
            Console.WriteLine("=============\n");
            Console.ResetColor();
        }
    }
}
