using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mqtt_broker.model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace mqtt_broker.Services
{
    public class Aggregator
    {
        private IMqttClient _client;

        //initializes a dictionary, that will have the data stored, based on the windowsize
        private Dictionary<string, List<double>> _dataBuffer = new Dictionary<string, List<double>>(); 
        private Config _config;

        /// <summary>
        /// load the configuration from the file config.json
        /// </summary>
        public Aggregator()
        {
            _config = LoadConfig("../../../config/config.json");
        }


        public async Task StartAsync()
        {
            //from the MQTTNet example, the Connect_Client sample method
            var mqttFactory = new MqttFactory();
            Console.WriteLine("Start async");
            

            using (_client = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_config.mqtt.Address, _config.mqtt.Port).Build();
                
                _client.ConnectedAsync += async e =>
                {
                    Console.WriteLine($"Connected to MQTT broker {_config.mqtt.Address} : {_config.mqtt.Port}");

                    foreach (var sub in _config.Subscriptions)
                    {
                        await _client.SubscribeAsync(new MqttTopicFilterBuilder()
                                                                .WithTopic(sub.Topic)
                                                                .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)sub.QoS)
                                                                .Build()
                        );

                        Console.WriteLine($"Subscribed to {sub.Topic}");
 
                    }
                };
                Console.WriteLine("Client created");

                _client.ApplicationMessageReceivedAsync += async e => await HandleIncomingMessage(e);

                await _client.ConnectAsync(mqttClientOptions);
            }
        }

        /// <summary>
        /// method to handle with the messages
        /// </summary>
        /// <param name="e">message received by the broker</param>
        /// <returns>publishes the result of some operation on the data received</returns>
        private async Task HandleIncomingMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            string topic = e.ApplicationMessage.Topic;
            string payloadSegment = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            Console.WriteLine("Received message from topic: " + topic);
            Console.WriteLine("Payload: " + payloadSegment);

            try
            {
                
                MachineData machineData = JsonConvert.DeserializeObject<MachineData>(payloadSegment);

                Recipe recipe = _config.OtherRecipes.FirstOrDefault(r => r.Name == _config.CurrentRecipe);

                if (recipe != null && recipe.Topics.Contains(topic)) 
                {
                    //Handles the parts_produced.
                    if (topic.EndsWith("parts_produced"))
                    {
                        //updates the buffer with the new topic, adding a list
                        if (!_dataBuffer.ContainsKey(topic))
                        {
                            _dataBuffer[topic] = new List<double>();
                        }

                        _dataBuffer[topic].Add(machineData.StatusData.PartsProduced);

                        //if enough window, apply the recipe
                        if (_dataBuffer[topic].Count >= recipe.WindowSize) 
                        {
                            double result = ApplyRecipe(recipe);
                            Console.WriteLine($"Aggregated Result: {result}");

                            //publish the result
                            var resultpayload = new MqttApplicationMessageBuilder()
                                                        .WithTopic(_config.Output.Topic)
                                                           .WithPayload(result.ToString())
                                                           .Build();
                            await _client.PublishAsync(resultpayload);
                        }
                    }
                }

            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to parse JSON: {ex.Message}");
            }
        }

        /// <summary>
        /// Method used for reading the json of the config file
        /// </summary>
        /// <param name="path">path to the config file</param>
        /// <returns>Deserialized config from json, as a Config objetc</returns>
        private Config LoadConfig(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            return JsonConvert.DeserializeObject<Config>(json); 
        }

        /// <summary>
        /// method to apply the recipe
        /// </summary>
        /// <param name="recipe">the recipe to apply</param>
        /// <returns>the value after the calculation, in double</returns>
        private double ApplyRecipe(Recipe recipe) 
        {
            double result = 0;

            foreach (var topic in recipe.Topics) 
            {
               var values = _dataBuffer[topic];

                switch(recipe.Strategy.ToLower())
                {
                    case "sum":
                        result += values.Sum();
                        break;
                    case "average":
                        result = values.Average();
                        break;
                    case "substract":
                        result -= values.Sum();
                        break;
                    default: throw new Exception("strategy not supported");
                }

                
            }

            return result;
        }

    }
}
