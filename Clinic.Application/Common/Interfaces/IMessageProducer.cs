using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Application.Common.Interfaces
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message, string queueName);
    }
}