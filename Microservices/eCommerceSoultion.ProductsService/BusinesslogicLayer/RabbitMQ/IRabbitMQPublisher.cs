using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesslogicLayer.RabbitMQ
{
    public interface IRabbitMQPublisher
    {
        void Publish<T>(string routingkey,T message);
    }
}
