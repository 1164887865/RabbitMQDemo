using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FantoutClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FanoutClient接收客户端启动...");
            //1.创建连接工厂
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //2.创建连接
            using (var connection = factory.CreateConnection())
            {
                //3.创建通道
                using (var channel = connection.CreateModel())
                {
                    //4.定义交换器  持久化
                    channel.ExchangeDeclare("exchange", "fanout",true);
                    //5.创建匿名队列，绑定交换器  durable:true 持久化 没有被消费不会丢失
                    var queueName = channel.QueueDeclare("Priqueue", true, false, false, new Dictionary<string, object>() { { "x-max-priority", 10 } });
                    channel.QueueBind(queueName, "exchange", "",null);

                    //6.创建消费者
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                      {
                          //接收消息
                          var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                          Console.WriteLine($"接收消息：{body.ToString()}");
                      };
                    //7.消费消息
                    channel.BasicConsume(queueName, true, consumer);

                    Console.ReadKey();

                }
            }
        }
    }
}
