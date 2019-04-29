using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Receiving
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //Bu kısımda okuyacağımız Queue yi belirtiyoruz.
                    channel.QueueDeclare(queue: "MyFirstQueue" ,
                            durable: false ,
                            exclusive: false ,
                            autoDelete: false ,
                            arguments: null);


                    var consumer = new EventingBasicConsumer(channel);
                    //Received sayesinde sürekli bir listening yapılması sağlanıyor
                    consumer.Received += (model , ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}" , message);
                    };
                    //Mesajları çekme işlemleri
                    //autoAck : true olmsaı 
                    channel.BasicConsume(queue: "MyFirstQueue" ,
                        autoAck: true ,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }
            }
        }
    }
}
