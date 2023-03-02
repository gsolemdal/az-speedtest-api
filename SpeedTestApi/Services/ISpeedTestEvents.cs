using SpeedTestApi.Models;
using System.Text;
using System.Text.Json;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;

namespace SpeedTestApi.Services;

public interface ISpeedTestEvents
{
    Task PublishSpeedTest(TestResult speedTest);
}

public class SpeedTestEvents : ISpeedTestEvents, IDisposable
{
    private readonly EventHubProducerClient _client;

    public SpeedTestEvents(string connectionString, string entityPath)
    {
        _client = new EventHubProducerClient(connectionString, entityPath);
    }

    public async Task PublishSpeedTest(TestResult speedTest)
    {
        var message = JsonSerializer.Serialize(speedTest);
        var data = new EventData(Encoding.UTF8.GetBytes(message));

        await _client.SendAsync(new[] { data });
    }

    public void Dispose()
    {
        _client.CloseAsync();
    }
}