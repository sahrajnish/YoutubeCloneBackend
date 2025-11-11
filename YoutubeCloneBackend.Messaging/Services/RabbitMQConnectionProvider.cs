using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Messaging.Services
{
    public class RabbitMQConnectionProvider
    {
        public IConnection Connection { get; set; }
        private readonly ConnectionFactory _factory;
        public RabbitMQConnectionProvider()
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task InitializeAsync()
        {
            Connection = await _factory.CreateConnectionAsync();
        }
        public async ValueTask DisposeAsync()
        {
            if (Connection != null)
                await Connection.DisposeAsync();
        }
    }
}
