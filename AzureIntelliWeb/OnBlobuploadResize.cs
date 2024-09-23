using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AzureIntelliFunc
{
    public class OnBlobuploadResize
    {
        [FunctionName("OnBlobuploadResize")]
        public void Run([BlobTrigger("functionsalesrep/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob,
            [Blob("functionsalesrep-sm/{name}",FileAccess.Write)] Stream myBlobFileOutput,
            string name, ILogger log)
        {
            using Image<Rgba32> input = Image.Load<Rgba32>(myBlob,out IImageFormat format);
            input.Mutate(x => x.Resize(300, 200));
            input.Save(myBlobFileOutput, format);

            log.LogInformation($"Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
