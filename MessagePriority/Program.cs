using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MessagePriority
{
    class Program
    {
        static void Main(string[] args)
        {
            //初始化工厂
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //2.创建连接
            using (var connection = factory.CreateConnection())
            //3.创建管道
            using (var channel = connection.CreateModel())
            {
                //4.创建交换器
                channel.ExchangeDeclare("exchange", "fanout", true);
                //定义队列，设置优先级
                channel.QueueDeclare("Priqueue", true, false, false, new Dictionary<string, object>() { { "x-max-priority",10} });
                //交换器绑定队列
                channel.QueueBind("Priqueue", "exchange", "", null);

                //消息持久化
                IBasicProperties basicProperties = channel.CreateBasicProperties();
                basicProperties.Persistent = true;

                string msg = "";

                for (int i = 0; i < 10; i++)
                {
                    msg = $"发布消息{i}";
                    if (i % 2 == 0)
                    {
                        msg += "——vip用户消息(优先处理)";
                        basicProperties.Priority = 9;
                    }
                    else
                    {
                        msg += "——普通用户消息";
                        basicProperties.Priority = 1;
                    }
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish("exchange", "", basicProperties, body);
                    Console.WriteLine($"发布成功：{msg}");
                    Thread.Sleep(1000);
                }
                Console.ReadKey();
            }
        }
    }
}
