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

channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

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
    channel.BasicPublish(exchange: "logs",
                         routingKey: string.Empty,
                         basicProperties: null,
                         body: body);
    Console.WriteLine($" [x] Sent {message}");
}