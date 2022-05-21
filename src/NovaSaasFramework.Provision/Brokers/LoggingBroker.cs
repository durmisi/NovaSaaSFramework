// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using NovaSaasFramework.Provision.Models.Configuration;

namespace NovaSaasFramework.Provision.Brokers
{
    public interface ILoggingBroker
    {
        void LogActivity(string message);
    }

    public class LoggingBroker : ILoggingBroker
    {
        public void LogActivity(string message) =>
            Console.WriteLine(message);
    }
}