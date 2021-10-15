using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RestaurantService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration configuration;
        private readonly IConnection connection;
        private readonly IModel channel;

        public MessageBusClient(
            IConfiguration configuration
        ) {
            this.configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = this.configuration["RabbitMQHost"],
                Port = int.Parse( this.configuration["RabbitMQPort"] )
            };

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(
                    exchange: "trigger",
                    type: ExchangeType.Fanout
                );

                connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void PublishNewRestaurant(RestaurantPublishedDTO restaurantPublishedDTO)
        {
            var message = JsonSerializer.Serialize(restaurantPublishedDTO);

            if( connection.IsOpen )
            {
                SendMessage(message);
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"--> sent {message} to MessageBus");
        }

        public void Dispose()
        {
            Console.WriteLine("--> MessageBus Disposed");

            if( channel.IsOpen )
            {
                channel.Close();
                connection.Close();
            }
        }

        private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
