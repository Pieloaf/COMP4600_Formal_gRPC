using System.Threading.Tasks;
using Grpc.Core;
using GrpcCalculator;

// using the CalculatorClient namespace
namespace CalculatorClient
{ 
    // Defining the Response Struct
    public struct Response {
        // struct constructor definition
        public Response(bool v, string a)
        {
            valid = v;  //indicates if result is valid
            ans = a;    // result 
        }

        public bool valid { get; }  // defining valid type
        public string ans { get; }  // defining answer type

        // used in testing for easier printing of result
        public override string ToString() => $"({ans}, {valid})"; 
    }

    // TrigCalculator class definition and implementation
    public class TrigCalculator
    {
        // Attribute Initialisation
        private string Host;    // server address
        private int Port;       // server listening port
        private CalculatorService.CalculatorServiceClient client; // gRPC client object
        private Channel channel;    // gRPC channel object

        // Constructor
        public TrigCalculator()
        {
            // Set Host and Port Variables
            Host = "localhost";
            Port = 50051;
        }

        // Overloaded Constructor
        public TrigCalculator(string host, int port)
        {
            // Allows user to define an alternate host and port
            Host = host;
            Port = port;
        }

        // create channel function
        public void CreateChannel()
        {
            // Create an insecure gRPC channel to a given host and port
            // these channels can have multiple underlying connections
            channel = new Channel($"{Host}:{Port}", ChannelCredentials.Insecure);

            // Create a Service Client using the previously made channel
            // Function calls are made through this client
            client = new CalculatorService.CalculatorServiceClient(channel);
        }

        // Async Close channel function
        public async Task CloseChannel()
        {
            // When this function is called the gRPC channel will shutdown
            await channel.ShutdownAsync();
        }

        // helper function to create a request object
        private TrigRequest createReq(double val, bool deg)
        {
            return new TrigRequest
            {
                Value = val,
                Unit = (Unit)(deg ? 0 : 1)
            };
        }

        // Sine function which returns an instance of response struct
        public Response Sine(double val, bool deg)
        {
            // Create a request object containing value and angle unit
            TrigRequest request = createReq(val, deg);

            // try communicate with the server
            try {
                // send request to grpc server
                MathResponse response = client.sine(request);
                // return a valid response object with the server response
                return new Response(true, response.Answer.ToString());
            }
            catch (RpcException e) { // if there is an RpcException
                // return an invalid response object with the error message
                return new Response(false, e.ToString());
            }
        }

        // Cosine function which returns an instance of response struct
        public Response Cos(double val, bool deg)
        {
            // Create a request object containing value and angle unit
            TrigRequest request = createReq(val, deg);

            // try communicate with the server
            try {
                // send request to grpc server
                MathResponse response = client.cosine(request); 
                // return a valid response object with the server response
                return new Response(true, response.Answer.ToString());
            }
            catch (RpcException e) { // if there is an RpcException
                // return an invalid response object with the error message
                return new Response(false, e.ToString());
            }
        }

        // Tan function which returns an instance of response struct
        public Response Tan(double val, bool deg)
        {
            // Create a request object containing value and angle unit
            TrigRequest request = createReq(val, deg);

            // try communicate with the server
            try {
                // send request to grpc server
                MathResponse response = client.tan(request);
                // return a valid response object with the server response
                return new Response(true, response.Answer.ToString());
            } catch (RpcException e) { // if there is an RpcException
                // return an invalid response object with the error message
                return new Response(false, e.ToString());
            }
        }

    }
}
