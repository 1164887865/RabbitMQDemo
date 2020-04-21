using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace TopicServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TopicServer发布服务器启动...");

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
                    //创建交换机
                    channel.ExchangeDeclare("topicExchange", "topic");

                    string msg = "";
                    for (int i = 0; i < 20; i++)
                    {
                        msg = $"发布消息{i}";
                        var body = Encoding.UTF8.GetBytes(msg);
                        string Topic = "";
                        if (i % 2 == 0)
                        {
                            Topic = "Topic.add";
                        }
                        else
                        {
                            Topic = "Topic.remove";
                        }
                        channel.BasicPublish("topicExchange", Topic, null, body);
                        Console.WriteLine($"{Topic}-发布消息成功：{msg}");

                        Thread.Sleep(1000);
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
