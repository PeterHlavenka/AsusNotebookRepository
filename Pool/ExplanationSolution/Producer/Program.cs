using RabbitMQMessageSender;


// producer uz nepotrebuje mit nainstalovany balik RabbitMQ.Client, protoze ho ma MessageSender
var messageSender = new MessageSender();
await messageSender.SendMessage("Hello World!");

Console.WriteLine("Prerequisites: Make sure RabbitMQ is running on localhost:5672");
Console.WriteLine("You can show messages in the queue on http://localhost:8080/#/queues");
Console.ReadLine();