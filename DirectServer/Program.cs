using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace DirectServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DirectServer发布服务器启动...");

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
                    channel.ExchangeDeclare("directExchange", "direct");

                    string msg = "";
                    for (int i = 0; i < 40; i++)
                    {
                        msg = $"发布消息{i}";
                        string ROUTE_KEY = "";
                        var body = Encoding.UTF8.GetBytes(msg);
                        if (i % 2 == 0)
                        {
                            ROUTE_KEY = "route1";
                        }
                        else
                        {
                            ROUTE_KEY = "route2";
                        }
                        channel.BasicPublish("directExchange", ROUTE_KEY, null, body);
                        Console.WriteLine($"向{ROUTE_KEY}发布消息成功：{msg}");

                        Thread.Sleep(1000);
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
