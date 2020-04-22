using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace FanoutServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FanoutServer发布服务器启动...");

            //1.创建连接工厂
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
                channel.ExchangeDeclare("exchange", "fanout");
                channel.QueueDeclare("SMSqueue",true,false,false,null);
                channel.QueueDeclare("SMAILqueue", true, false, false, null);
                channel.QueueBind("SMSqueue", "exchange","",null);
                channel.QueueBind("SMAILqueue", "exchange","",null);

                string msg = "";

                for (int i = 0; i < 20; i++)
                {
                    msg = $"发布消息{i}";
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish("exchange", "", null, body);
                    Console.WriteLine($"发布成功：{msg}");
                    Thread.Sleep(1000);
                }
                Console.ReadKey();
            }
        }
    }
}
