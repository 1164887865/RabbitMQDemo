using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SimpleClient
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
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, e) =>
                    {
                        byte[] message = e.Body.ToArray();
                        
                        //返回消息确认（true/false，自动/手动确认）,没确认就不会消费掉消息
                        if (Encoding.UTF8.GetString(message).Contains("1"))
                        {
                            Console.WriteLine("接收消息:" + Encoding.UTF8.GetString(message));
                            channel.BasicAck(e.DeliveryTag, false);
                        }
                        else 
                        {
                            Console.WriteLine("拒绝消息:" + Encoding.UTF8.GetString(message));
                            //拒绝消息 false：拒绝后丢弃  true：拒绝后重新入队
                            channel.BasicReject(e.DeliveryTag, false);
                        }
                        Thread.Sleep(100);
                    };
                    //消费者开启监听
                    channel.BasicConsume("simple", false, consumer);

                    Console.ReadLine();

                }
            }
        }
    }
}
