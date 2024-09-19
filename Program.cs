using System.Reflection.Metadata.Ecma335;
using mqtt_broker.model;
using mqtt_broker.Services;


Console.WriteLine("Starting MQTT Data Aggregator...");

// Create an instance of the MqttDataAggregator class
var aggregator = new Aggregator();

Thread threadAggregator = new Thread(async () =>
{
    await aggregator.StartAsync();
});

threadAggregator.Start();

Console.WriteLine("MQTT Data Aggregator started. Press any key to exit...");

// Keep the program running until a key is pressed
Console.ReadKey();

Console.WriteLine("Shutting down...");