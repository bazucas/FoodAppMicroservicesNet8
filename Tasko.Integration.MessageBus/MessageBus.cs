using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Tasko.Integration.MessageBus;

public class MessageBus : IMessageBus
{
    private readonly string connectionString =
        "Endpoint=sb://taskoweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[code_here]";

    public async Task PublishMessage(object message, string? topic_queue_Name)
    {
        await using var client = new ServiceBusClient(connectionString);

        ServiceBusSender sender = client.CreateSender(topic_queue_Name);

        var jsonMessage = JsonConvert.SerializeObject(message);

        ServiceBusMessage finalMessage = new(Encoding
            .UTF8.GetBytes(jsonMessage))
        {
            CorrelationId = Guid.NewGuid().ToString(),
        };

        await sender.SendMessageAsync(finalMessage);

        await client.DisposeAsync();
    }
}
