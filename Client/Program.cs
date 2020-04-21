using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "127.0.0.1"
            };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //声明队列
                    channel.QueueDeclare("hello", false, false, false, null);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, e) =>
                    {
                        byte[] message = e.Body;
                        Console.WriteLine("接收消息:" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(e.DeliveryTag, true);
                    };
                    //消费者开启监听
                    channel.BasicConsume("hello", false, consumer);

                    Console.ReadLine();

                }
            }
        }
        
    }
}
