﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    VirtualHost = "/",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (s, e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine("Message received: {0}", message);
};

channel.BasicConsume("queue.one", false, consumer);
channel.BasicConsume("queue.two", false, consumer);
Console.WriteLine("Waiting for messages... Press any key to exit.");
Console.ReadKey();
