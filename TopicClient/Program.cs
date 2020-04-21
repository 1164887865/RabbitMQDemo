using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace TopicClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TopicClient接收客户端启动...");
            //1.创建连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    var queue = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue, "topicExchange", "Topic.#");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        //接收消息
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine($"Topic.#接收消息：{body.ToString()}");
                    };
                    channel.BasicConsume(queue, true, consumer);

                    Console.ReadKey();
                }
            }
        }
    }
}
