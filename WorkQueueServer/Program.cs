using RabbitMQ.Client;
using System;
using System.Text;

namespace WorkQueueServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //1.创建连接工厂
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "guest",
                Password = "guest"
            };
            //2.创建连接
            var connection = factory.CreateConnection();
            //3.创建管道
            var channel = connection.CreateModel();

            //4.声明队列
            channel.QueueDeclare("simple", false, false, false, null);

            for (int i = 0; i < 200; i++)
            {
                string msg = $"第{i + 1}条消息";
                //5.发布消息
                channel.BasicPublish("", "simple", null, Encoding.UTF8.GetBytes(msg));
                Console.WriteLine($"已发送消息：{msg}");
            }
            channel.Close();
            connection.Close();

            Console.ReadKey();
        }
    }
}
