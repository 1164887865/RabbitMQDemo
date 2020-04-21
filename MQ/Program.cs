using RabbitMQ.Client;
using System;
using System.Text;

namespace MQ
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password="guest",
                HostName="127.0.0.1"
            };
            //创建连接
            var connection = factory.CreateConnection();
            //创建管道
            var channel = connection.CreateModel();
            //声明队列
            channel.QueueDeclare("hello", false, false, false, null);

            string input;

            while (true)
            {
                input = Console.ReadLine();
                if (input == "exit")
                { break; }
                var sendByte = Encoding.UTF8.GetBytes(input);
                channel.BasicPublish("", "hello", null, sendByte);
            }
            channel.Close();
            connection.Close();
        }
    }
}
