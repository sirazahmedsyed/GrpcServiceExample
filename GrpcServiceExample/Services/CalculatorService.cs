using Grpc.Core;

namespace GrpcServiceExample.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        // Implement the Add method
        public override Task<AddReply> Add(AddRequest request, ServerCallContext context)
        {
            var result = request.Num1 + request.Num2;
            return Task.FromResult(new AddReply
            {
                Result = result
            });
        }
    }
}
