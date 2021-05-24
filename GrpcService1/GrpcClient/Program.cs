using Grpc.Core;
using Grpc.Net.Client;
using GrpcFiles;
using GrpcGreeter;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace GrpcClient
{
    class Program
    {
        public static string _token { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGl0eSI6IntcIlVzZXJOYW1lXCI6XCJLTDEzNTZcIixcIlVzZXJJZFwiOjMxMDI3NzYyOSxcIlVzZXJJZFR5cGVcIjowLFwiVXNlckZpcnN0TmFtZVwiOlwi16jXldee159cIixcIlVzZXJMYXN0TmFtZVwiOlwi15LXqNeY15XXpNeh16fXmVwiLFwiU2hpeXVjaElyZ3VuaVwiOlt7XCJDb2RlTWFjaG96XCI6OTksXCJDb2RlU2VjdG9yXCI6MCxcIkNvZGV5UGVpbHV0XCI6W119XSxcIk1hY2hvek1lbmFoZWxcIjo5OSxcIk9yZ1JvbGVzXCI6bnVsbCxcIkFjdGl2ZU9yZ1JvbGVcIjp7XCJPcmdSb2xlQ29kZVwiOm51bGwsXCJTdWdZZXNodXRcIjowLFwiU2VtZWxZZXNodXRcIjpudWxsLFwiR3JvdXBzXCI6W3tcIkdyb3VwRGVzY3JpcHRpb25cIjpcIteU15LXk9eo16og15TXqNep15DXldeqINec15fXodeZ157XldeqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfQ09ORklHX0hBUlNIQU9UX0hBU0lNT1RcIixcIkdyb3VwSWRcIjoxMjM0NTYsXCJTdWdZZXNodXRcIjpudWxsLFwiU2VtZWxZZXNodXRcIjpudWxsfSx7XCJHcm91cERlc2NyaXB0aW9uXCI6XCLXnteg15TXnCDXntei16jXm9eqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfT0hNX01FTkFIRUxfTUFBUkVDSEVUXCIsXCJHcm91cElkXCI6MTAwMCxcIlN1Z1llc2h1dFwiOm51bGwsXCJTZW1lbFllc2h1dFwiOm51bGx9XX0sXCJKd3RUb2tlblwiOm51bGx9IiwibmJmIjoxNjIxNTAxNTgxLCJleHAiOjE2MjE1Mzc1ODEsImlhdCI6MTYyMTUwMTU4MX0.vLqZTWZeJQb55ZxIafGh7eM_1Rpej0AZfgJhS0VAhsQ";

        static async Task Main(string[] args)
        {
            string addressHttps = "https://localhost:5001";
            //string addressHttp = "http://localhost:5000";
            using var channel = CreateAuthenticatedChannel(addressHttps);
            //await InvorkeGrpc(address);
            //await InvorkeGrpcByInterseptor(address);

            Console.WriteLine("gRPC Ticketer");
            Console.WriteLine();
            Console.WriteLine("Press a key:");
            Console.WriteLine("1: InvorkeGrpcByInterseptor");
            Console.WriteLine("2: InvorkeGrpcServerStream");
            Console.WriteLine("3: Authenticate");
            Console.WriteLine("4: GetFile");
            Console.WriteLine("5: Exit");
            Console.WriteLine();

            bool exiting = false;
            while (!exiting)
            {
                var consoleKeyInfo = Console.ReadKey(intercept: true);
                switch (consoleKeyInfo.KeyChar)
                {
                    case '1':
                        await InvorkeGrpcByInterseptor(channel);
                        break;
                    case '2':
                        await InvorkeGrpcServerStream(channel);
                        break;
                    case '3':
                        _token = await Authenticate(addressHttps);
                        break;
                    case '4':
                        await GetFile(channel);
                        break;
                    case '5':
                        exiting = true;
                        break;
                }
            }

            Console.WriteLine("Exiting");
        }

        static async Task GetFile(GrpcChannel channel)
        {
            // The port number(5001) must match the port of the gRPC server.
            var client = new Files.FilesClient(channel);

            var cts = new CancellationTokenSource(TimeSpan.FromDays(2));
            var request = new FileRequest { FileName = @"C:\TfsEduCore\install\dotnet-sdk-5.0.100-win-x64.exe" };
            using AsyncServerStreamingCall<FileResponse> streamingCall =
                client.GetFileStream(request, cancellationToken: cts.Token);
            try
            {
                await foreach (FileResponse responseOnePart in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    Console.WriteLine(responseOnePart.Base64);
                    //Console.WriteLine($"{response.DateTimeStamp.ToDateTime():s} | {response.Summary} | {response.TemperatureC} C");
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }

        }

        static async Task InvorkeGrpcServerStream(GrpcChannel channel)
        {            
            // The port number(5001) must match the port of the gRPC server.
            var client = new Greeter.GreeterClient(channel);

            var cts = new CancellationTokenSource(TimeSpan.FromDays(2));
            var request = new HelloRequest { Name = "Hello stream server" };
            using AsyncServerStreamingCall<HelloReply> streamingCall = 
                client.SayHelloServerStream(request, cancellationToken: cts.Token);
            try
            {
                await foreach (HelloReply response in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    Console.WriteLine(response.Message);
                    //Console.WriteLine($"{response.DateTimeStamp.ToDateTime():s} | {response.Summary} | {response.TemperatureC} C");
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }

        }

        static async Task InvorkeGrpc(string address)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var headers = new Metadata();            
            headers.Add("Authorization", $"Bearer {_token}");
            var client = new Greeter.GreeterClient(channel);
            var request = new HelloRequest { Name = "GreeterClient" };
            var reply = await client.SayHelloAsync(request, headers);
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task InvorkeGrpcByInterseptor(GrpcChannel channel)
        {
            // The port number(5001) must match the port of the gRPC server.            
            //using var channel = CreateAuthenticatedChannel(address);
            var client = new Greeter.GreeterClient(channel);
            var request = new HelloRequest { Name = "GreeterClient" };
            //var reply = await client.SayHelloAsync(request, headers);
            var reply = await client.SayHelloAsync(request);
            Console.WriteLine("Greeting: " + reply.Message);
           // Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();
        }

        private static GrpcChannel CreateAuthenticatedChannel(string address)
        {
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                //string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGl0eSI6IntcIlVzZXJOYW1lXCI6XCJLTDEzNTZcIixcIlVzZXJJZFwiOjMxMDI3NzYyOSxcIlVzZXJJZFR5cGVcIjowLFwiVXNlckZpcnN0TmFtZVwiOlwi16jXldee159cIixcIlVzZXJMYXN0TmFtZVwiOlwi15LXqNeY15XXpNeh16fXmVwiLFwiU2hpeXVjaElyZ3VuaVwiOlt7XCJDb2RlTWFjaG96XCI6OTksXCJDb2RlU2VjdG9yXCI6MCxcIkNvZGV5UGVpbHV0XCI6W119XSxcIk1hY2hvek1lbmFoZWxcIjo5OSxcIk9yZ1JvbGVzXCI6bnVsbCxcIkFjdGl2ZU9yZ1JvbGVcIjp7XCJPcmdSb2xlQ29kZVwiOm51bGwsXCJTdWdZZXNodXRcIjowLFwiU2VtZWxZZXNodXRcIjpudWxsLFwiR3JvdXBzXCI6W3tcIkdyb3VwRGVzY3JpcHRpb25cIjpcIteU15LXk9eo16og15TXqNep15DXldeqINec15fXodeZ157XldeqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfQ09ORklHX0hBUlNIQU9UX0hBU0lNT1RcIixcIkdyb3VwSWRcIjoxMjM0NTYsXCJTdWdZZXNodXRcIjpudWxsLFwiU2VtZWxZZXNodXRcIjpudWxsfSx7XCJHcm91cERlc2NyaXB0aW9uXCI6XCLXnteg15TXnCDXntei16jXm9eqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfT0hNX01FTkFIRUxfTUFBUkVDSEVUXCIsXCJHcm91cElkXCI6MTAwMCxcIlN1Z1llc2h1dFwiOm51bGwsXCJTZW1lbFllc2h1dFwiOm51bGx9XX0sXCJKd3RUb2tlblwiOm51bGx9IiwibmJmIjoxNjIxNTAxNTgxLCJleHAiOjE2MjE1Mzc1ODEsImlhdCI6MTYyMTUwMTU4MX0.vLqZTWZeJQb55ZxIafGh7eM_1Rpej0AZfgJhS0VAhsQ";
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                MaxReceiveMessageSize = 10 * 1024 * 1024, // 5 MB
                MaxSendMessageSize = 10 * 1024 * 1024 // 5 MB
            });
            return channel;
        }

        private static async Task<string> Authenticate(string address)
        {
            Console.WriteLine($"Authenticating as {Environment.UserName}...");
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                //RequestUri = new Uri("https://localhost:5001/generateJwtToken"),
                RequestUri = new Uri($"{address}/generateJwtToken?name={HttpUtility.UrlEncode(Environment.UserName)}"),
                Method = HttpMethod.Get,
                Version = new Version(2, 0)
            };
            //httpClient.DefaultRequestHeaders.Authorization = 
            //    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
           
            var tokenResponse = await httpClient.SendAsync(request);
            tokenResponse.EnsureSuccessStatusCode();

            var token = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Successfully authenticated.");

            return token;
        }
    }
}
