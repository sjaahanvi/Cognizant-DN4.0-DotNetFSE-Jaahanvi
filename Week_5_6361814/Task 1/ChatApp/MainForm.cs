using Confluent.Kafka;
using KafkaChat.Shared;
using System.Text.Json;

namespace ChatApp
{
    public partial class MainForm : Form
    {
        private IProducer<Null, string>? _producer;
        private IConsumer<Null, string>? _consumer;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _consumerTask;
        private string _username = string.Empty;
        private bool _isConnected = false;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;
            UpdateUI();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Username Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            _username = txtUsername.Text.Trim();
            
            try
            {
                await ConnectToKafka();
                _isConnected = true;
                UpdateUI();
                AppendChatMessage("System", "Connected to chat successfully!", Color.Green);
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to Kafka:\n{ex.Message}\n\nMake sure Kafka is running on localhost:9092", 
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            await DisconnectFromKafka();
            _isConnected = false;
            UpdateUI();
            AppendChatMessage("System", "Disconnected from chat.", Color.Red);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await SendMessage();
            }
        }

        private async Task ConnectToKafka()
        {
            // Initialize producer
            var producerConfig = KafkaConfig.GetProducerConfig();
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            // Initialize consumer
            var consumerGroup = $"{KafkaConfig.CONSUMER_GROUP}-{_username}-{Guid.NewGuid().ToString("N")[..8]}";
            var consumerConfig = KafkaConfig.GetConsumerConfig(consumerGroup);
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig)
                .SetErrorHandler((_, e) => {
                    this.Invoke(() => AppendChatMessage("Error", e.Reason, Color.Red));
                })
                .Build();

            _consumer.Subscribe(KafkaConfig.CHAT_TOPIC);

            // Send join message
            var joinMessage = new ChatMessage(_username, "joined the chat");
            await _producer.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                new Message<Null, string> { Value = joinMessage.ToJson() });

            // Start consumer task
            _cancellationTokenSource = new CancellationTokenSource();
            _consumerTask = Task.Run(() => ConsumeMessages(_cancellationTokenSource.Token));

            UpdateStatusLabel($"Connected as {_username}");
        }

        private async Task DisconnectFromKafka()
        {
            try
            {
                // Send leave message
                if (_producer != null && _isConnected)
                {
                    var leaveMessage = new ChatMessage(_username, "left the chat");
                    await _producer.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                        new Message<Null, string> { Value = leaveMessage.ToJson() });
                }

                // Stop consumer
                _cancellationTokenSource?.Cancel();
                if (_consumerTask != null)
                {
                    await _consumerTask;
                }

                // Dispose resources
                _consumer?.Close();
                _consumer?.Dispose();
                _producer?.Dispose();

                _consumer = null;
                _producer = null;
                _consumerTask = null;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;

                UpdateStatusLabel("Disconnected");
            }
            catch (Exception ex)
            {
                AppendChatMessage("Error", $"Error during disconnect: {ex.Message}", Color.Red);
            }
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text) || _producer == null)
                return;

            try
            {
                var messageText = txtMessage.Text.Trim();
                var chatMessage = new ChatMessage(_username, messageText);
                
                await _producer.ProduceAsync(KafkaConfig.CHAT_TOPIC, 
                    new Message<Null, string> { Value = chatMessage.ToJson() });

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                AppendChatMessage("Error", $"Failed to send message: {ex.Message}", Color.Red);
            }
        }

        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested && _consumer != null)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(1));
                        
                        if (consumeResult?.Message?.Value != null)
                        {
                            var chatMessage = ChatMessage.FromJson(consumeResult.Message.Value);
                            
                            // Don't display our own messages (optional - you can remove this if you want to see your own messages)
                            if (chatMessage.Username != _username)
                            {
                                var color = GetMessageColor(chatMessage);
                                this.Invoke(() => AppendChatMessage(chatMessage.Username, chatMessage.Message, color));
                            }
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        this.Invoke(() => AppendChatMessage("Error", $"Consume error: {ex.Error.Reason}", Color.Red));
                    }
                    catch (JsonException ex)
                    {
                        this.Invoke(() => AppendChatMessage("Error", $"Message format error: {ex.Message}", Color.Red));
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception ex)
            {
                this.Invoke(() => AppendChatMessage("Error", $"Consumer error: {ex.Message}", Color.Red));
            }
        }

        private Color GetMessageColor(ChatMessage chatMessage)
        {
            if (chatMessage.Message.Contains("joined the chat"))
                return Color.Green;
            else if (chatMessage.Message.Contains("left the chat"))
                return Color.Orange;
            else
                return Color.Black;
        }

        private void AppendChatMessage(string username, string message, Color color)
        {
            if (rtbChatHistory.InvokeRequired)
            {
                rtbChatHistory.Invoke(() => AppendChatMessage(username, message, color));
                return;
            }

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var formattedMessage = $"[{timestamp}] {username}: {message}\n";

            rtbChatHistory.SelectionStart = rtbChatHistory.TextLength;
            rtbChatHistory.SelectionLength = 0;
            rtbChatHistory.SelectionColor = color;
            rtbChatHistory.AppendText(formattedMessage);
            rtbChatHistory.SelectionColor = rtbChatHistory.ForeColor;
            rtbChatHistory.ScrollToCaret();
        }

        private void UpdateUI()
        {
            txtUsername.Enabled = !_isConnected;
            btnConnect.Enabled = !_isConnected;
            btnDisconnect.Enabled = _isConnected;
            txtMessage.Enabled = _isConnected;
            btnSend.Enabled = _isConnected;
        }

        private void UpdateStatusLabel(string status)
        {
            if (statusLabel.Owner?.InvokeRequired == true)
            {
                statusLabel.Owner.Invoke(() => UpdateStatusLabel(status));
                return;
            }
            statusLabel.Text = status;
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isConnected)
            {
                await DisconnectFromKafka();
            }
        }
    }
}
