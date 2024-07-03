using System.Text;
using RabbitMQ.Client;

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

channel.ExchangeDeclare("ex.fanout","fanout",false,true);

channel.QueueDeclare("queue.one",false,false,false);
channel.QueueDeclare("queue.two",false,false,false);

channel.QueueBind("queue.one","ex.fanout",string.Empty);
channel.QueueBind("queue.two","ex.fanout",string.Empty);

channel.BasicPublish("ex.fanout", string.Empty, false, null, Encoding.UTF8.GetBytes("This is a first fanout message."));
channel.BasicPublish("ex.fanout", string.Empty, false, null, Encoding.UTF8.GetBytes("This is a second fanout message."));

Console.WriteLine("Press any key to exit.");
Console.ReadKey();

channel.QueueDelete("queue.one");
channel.QueueDelete("queue.two");
channel.ExchangeDelete("ex.fanout");

channel.Close();
connection.Close();



