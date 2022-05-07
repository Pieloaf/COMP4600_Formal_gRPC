using GrpcCalculator;
using Grpc.Core;
using System.Threading.Tasks;
using System;

// using CalculatorServer namespace
namespace CalculatorServer
{
    // Calculator Service Class Implementation
    public class CalculatorServiceImpl : CalculatorService.CalculatorServiceBase
    {
        // async sine function that returns a Task with MathResponse
        // on function completetion the task returns the MathResponse object
        public override Task<MathResponse> sine(TrigRequest request, ServerCallContext context)
        {
           double val = request.Value; // store request value in val

            // check if request units are degrees
            if (request.Unit == Unit.Degrees)
            {
                //if degrees, convert to radians
                val = ( request.Value * (Math.PI)) / 180;
            }
            // return Task with MathResponse object with Sin of val
            return Task.FromResult(new MathResponse { Answer = Math.Sin(val) });
        }

        // async cosine function that returns a Task with MathResponse
        // on function completetion the task returns the MathResponse object
        public override Task<MathResponse> cosine(TrigRequest request, ServerCallContext context)
        {
            double val = request.Value; // store request value in val

            // check if request units are degrees
            if (request.Unit == Unit.Degrees)
            {
                //if degrees, convert to radians
                val = (request.Value * (Math.PI)) / 180;
            }
            // return Task with MathResponse object with Cosine of val
            return Task.FromResult(new MathResponse { Answer = Math.Cos(val) });
        }

        // async tan function that returns a Task with MathResponse
        // on function completetion the task returns the MathResponse object
        public override Task<MathResponse> tan(TrigRequest request, ServerCallContext context)
        {
            double val = request.Value; // store request in val

            // check if request units are degrees
            if (request.Unit == Unit.Degrees)
            {
                //if degrees, convert to radians
                val = (request.Value * (Math.PI)) / 180;
            }
            // return Task with MathResponse object with Tan of val
            return Task.FromResult(new MathResponse { Answer = Math.Tan(val) });
        }
    }
}
