// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServiceExample;

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7000"); 

        var client = new Calculator.CalculatorClient(channel);

        var reply = await client.AddAsync(new AddRequest { Num1 = 10, Num2 = 20 });

        Console.WriteLine($"Result: {reply.Result}");
    }
}
