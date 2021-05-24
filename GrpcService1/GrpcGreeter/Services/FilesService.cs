using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcFiles
{
    public class FilesService : Files.FilesBase
    {
        private readonly ILogger<FilesService> _logger;
        public FilesService(ILogger<FilesService> logger)
        {
            _logger = logger;
        }

        public override async Task GetFileStream(FileRequest request,
                IServerStreamWriter<FileResponse> responseStream,
                ServerCallContext context)
        {
            double i = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                try
                {
                    IEnumerable<string> fileXXX = GetFile(request.FileName);
                    foreach (string fileContent in fileXXX)
                    {
                        FileResponse response = new FileResponse
                        {
                            Base64 = fileContent
                        };
                        await responseStream.WriteAsync(response);
                    }

                    break;
                    //string fileContent = GetFile(request.FileName);
                    //_logger.LogInformation("Sending WeatherData response");


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public IEnumerable<string> GetFile(string fileName)
        {
            int bufferSize = 7 * 1024 * 1024; //5mb
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                //var len = (int)fs.Length;
                var bits = new byte[bufferSize];
                while (fs.Read(bits, 0, bufferSize) > 0)
                {
                    string base64String = Convert.ToBase64String(bits, 0, bits.Length);

                    yield return base64String;
                }
            }
        }
    }
}
