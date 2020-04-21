using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace TopicClient1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TopicClient接收客户端启动...");
            //创建连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //创建连接和通道
            using (var conn = factory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    //创建队列
                    var queue = channel.QueueDeclare().QueueName;
                    //绑定交换机 匹配路由Topic.add
                    channel.QueueBind(queue, "topicExchange", "Topic.add");
                    //创建消费者 只能接受到Topic.add路由消息
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        //接收消息
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine($"Topic.add接收消息：{body.ToString()}");
                    };
                    channel.BasicConsume(queue, true, consumer);

                    Console.ReadKey();
                }
            }
        }
    }
}
