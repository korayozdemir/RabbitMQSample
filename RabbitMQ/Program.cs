using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            //RabbitMQ ya bağlantı.Sunucu üzerinde ise Ip bilgisi yer alacak.
            var factory = new ConnectionFactory() {HostName = "localhost"};

            using (var connection = factory.CreateConnection())
            {
                //Mesajı iletmek için kanal oluşutuyoruz.
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("MyFirstQueue" ,
                                false , //Fiziksel mi saklanacak memory üzerinde mi ?
                                false , //Farklı bağlantılar ile kullanım
                                false , //Consumer lar kullandıktan sonra otomatik silinmesi
                                arguments: null); //Exchange tipleri

                    string message = JsonConvert.SerializeObject("Hello World");
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "" ,
                        routingKey: "MyFirstQueue" ,
                        basicProperties: null ,
                        body: body);

                    Console.WriteLine(" [x] Sent {0}" , message);
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }

        }
    }
}
