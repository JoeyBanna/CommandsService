using CommandsService.EventProcessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommandsService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        public readonly IConfiguration _configuration;
        public readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMq();
        }

        //Creating our connection
        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                // HostName = _configuration["RabbitMQHost"],
                HostName = "localhost",
                Port = int.Parse(_configuration["RabbitMQPort"])

            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange:"trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
            Console.WriteLine("-->> Listening on the message bus.....");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }
        // waiting and listening to the events
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
            {

                Console.WriteLine("--> Event received");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("-->> Connection Shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen) 
            {
                _channel.Close(); 
                _connection.Close();
            }
        }
    }
}
