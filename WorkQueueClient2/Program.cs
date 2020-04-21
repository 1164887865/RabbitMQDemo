using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace WorkQueueClient2
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
            //创建连接
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //声明队列
                    channel.QueueDeclare("simple", false, false, false, null);

                    // 告知 RabbitMQ，在未收到当前 Worker 的消息确认信号时，不再分发给消息，确保公平调度。
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, e) =>
                    {
                        byte[] message = e.Body.ToArray();
                        Console.WriteLine("接收消息:" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(e.DeliveryTag, false);
                    };
                    //消费者开启监听
                    channel.BasicConsume("simple", false, consumer);

                    Console.ReadLine();

                }
            }
        }

    }
}
