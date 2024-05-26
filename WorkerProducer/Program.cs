using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    UserName = "vp",
    Password = "vp123456"
};
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

bool next = true;
while (next)
{
    Console.Write("Digite uma mensagem ou X para sair:");
    string message = Console.ReadLine();

    if (message.Equals("x"))
    {
        next = false;
    }

    var body = Encoding.UTF8.GetBytes(message);

    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "task_queue",
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent {message}");

}


static string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}