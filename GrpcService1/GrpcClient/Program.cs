﻿using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var headers = new Metadata();
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGl0eSI6IntcIlVzZXJOYW1lXCI6XCJLTDEzNTZcIixcIlVzZXJJZFwiOjMxMDI3NzYyOSxcIlVzZXJJZFR5cGVcIjowLFwiVXNlckZpcnN0TmFtZVwiOlwi16jXldee159cIixcIlVzZXJMYXN0TmFtZVwiOlwi15LXqNeY15XXpNeh16fXmVwiLFwiU2hpeXVjaElyZ3VuaVwiOlt7XCJDb2RlTWFjaG96XCI6OTksXCJDb2RlU2VjdG9yXCI6MCxcIkNvZGV5UGVpbHV0XCI6W119XSxcIk1hY2hvek1lbmFoZWxcIjo5OSxcIk9yZ1JvbGVzXCI6bnVsbCxcIkFjdGl2ZU9yZ1JvbGVcIjp7XCJPcmdSb2xlQ29kZVwiOm51bGwsXCJTdWdZZXNodXRcIjowLFwiU2VtZWxZZXNodXRcIjpudWxsLFwiR3JvdXBzXCI6W3tcIkdyb3VwRGVzY3JpcHRpb25cIjpcIteU15LXk9eo16og15TXqNep15DXldeqINec15fXodeZ157XldeqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfQ09ORklHX0hBUlNIQU9UX0hBU0lNT1RcIixcIkdyb3VwSWRcIjoxMjM0NTYsXCJTdWdZZXNodXRcIjpudWxsLFwiU2VtZWxZZXNodXRcIjpudWxsfSx7XCJHcm91cERlc2NyaXB0aW9uXCI6XCLXnteg15TXnCDXntei16jXm9eqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfT0hNX01FTkFIRUxfTUFBUkVDSEVUXCIsXCJHcm91cElkXCI6MTAwMCxcIlN1Z1llc2h1dFwiOm51bGwsXCJTZW1lbFllc2h1dFwiOm51bGx9XX0sXCJKd3RUb2tlblwiOm51bGx9IiwibmJmIjoxNjIxNTAxNTgxLCJleHAiOjE2MjE1Mzc1ODEsImlhdCI6MTYyMTUwMTU4MX0.vLqZTWZeJQb55ZxIafGh7eM_1Rpej0AZfgJhS0VAhsQ";
            headers.Add("Authorization", $"Bearer {token}");

            var client = new Greeter.GreeterClient(channel);
            var request = new HelloRequest { Name = "GreeterClient" };
            var reply = await client.SayHelloAsync(request, headers);
            Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static GrpcChannel CreateAuthenticatedChannel(string address)
        {
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZGVudGl0eSI6IntcIlVzZXJOYW1lXCI6XCJLTDEzNTZcIixcIlVzZXJJZFwiOjMxMDI3NzYyOSxcIlVzZXJJZFR5cGVcIjowLFwiVXNlckZpcnN0TmFtZVwiOlwi16jXldee159cIixcIlVzZXJMYXN0TmFtZVwiOlwi15LXqNeY15XXpNeh16fXmVwiLFwiU2hpeXVjaElyZ3VuaVwiOlt7XCJDb2RlTWFjaG96XCI6OTksXCJDb2RlU2VjdG9yXCI6MCxcIkNvZGV5UGVpbHV0XCI6W119XSxcIk1hY2hvek1lbmFoZWxcIjo5OSxcIk9yZ1JvbGVzXCI6bnVsbCxcIkFjdGl2ZU9yZ1JvbGVcIjp7XCJPcmdSb2xlQ29kZVwiOm51bGwsXCJTdWdZZXNodXRcIjowLFwiU2VtZWxZZXNodXRcIjpudWxsLFwiR3JvdXBzXCI6W3tcIkdyb3VwRGVzY3JpcHRpb25cIjpcIteU15LXk9eo16og15TXqNep15DXldeqINec15fXodeZ157XldeqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfQ09ORklHX0hBUlNIQU9UX0hBU0lNT1RcIixcIkdyb3VwSWRcIjoxMjM0NTYsXCJTdWdZZXNodXRcIjpudWxsLFwiU2VtZWxZZXNodXRcIjpudWxsfSx7XCJHcm91cERlc2NyaXB0aW9uXCI6XCLXnteg15TXnCDXntei16jXm9eqXCIsXCJHcm91cE5hbWVcIjpcIk5URE9NQUlOXFxcXEdSX0RfT0hNX01FTkFIRUxfTUFBUkVDSEVUXCIsXCJHcm91cElkXCI6MTAwMCxcIlN1Z1llc2h1dFwiOm51bGwsXCJTZW1lbFllc2h1dFwiOm51bGx9XX0sXCJKd3RUb2tlblwiOm51bGx9IiwibmJmIjoxNjIxNTAxNTgxLCJleHAiOjE2MjE1Mzc1ODEsImlhdCI6MTYyMTUwMTU4MX0.vLqZTWZeJQb55ZxIafGh7eM_1Rpej0AZfgJhS0VAhsQ";
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
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
    }
}