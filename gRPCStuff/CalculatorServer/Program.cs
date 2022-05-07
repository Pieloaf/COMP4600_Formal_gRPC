using System;
using Grpc.Core;
using GrpcCalculator;

// using CalculatorServer namespace
namespace CalculatorServer
{
    // Main server program class
    class Program
    {
        // setting host and port variables
        const string Host = "localhost";
        const int Port = 50051;

        // Main server function 
        public static void Main(string[] args)
        {
            // Create a gRPC Server object
            var server = new Server
            {
                // define servers service [server only has calculator service with the calculator service implementation]
                Services = { CalculatorService.BindService(new CalculatorServiceImpl()) },
                // setting the server hostname and listening port from the previously set variables
                Ports = { new ServerPort(Host, Port, ServerCredentials.Insecure) } 
            };

            // Start server listening
            server.Start();

            // Print to console...
            Console.WriteLine("CalculatorServer listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey(); // wait for user input

            // shut down server on user input
            server.ShutdownAsync().Wait();
        }
    }
}
