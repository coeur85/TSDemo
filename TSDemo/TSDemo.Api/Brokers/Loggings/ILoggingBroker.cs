using System;

namespace TSDemo.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
    }
}
