using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task SayHelloServerStream(
            HelloRequest request, 
            IServerStreamWriter<HelloReply> responseStream, 
            ServerCallContext context)
        {            
            double i = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500); // Gotta look busy
                
                HelloReply forecast = new HelloReply
                {
                    Message = "Hello " + request.Name + 
                        " Date: " + DateTime.Now +
                        " Response No: " + i++                    
                };

                _logger.LogInformation("Sending WeatherData response");

                await responseStream.WriteAsync(forecast);
            }         
        }
    }
}
