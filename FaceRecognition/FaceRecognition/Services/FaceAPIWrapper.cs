using Microsoft.Azure.CognitiveServices.Vision.Face;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using PCLAppConfig;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace FaceRecognition.Services
{
    public class FaceAPIWrapper
    {
        // Use your own API key in the App.config file.
        private string subscriptionKey = ConfigurationManager.AppSettings["APIKey"];

        private const string faceEndpoint =
    "https://northeurope.api.cognitive.microsoft.com";

        private readonly IFaceClient faceClient;

        public FaceAPIWrapper()
        {
            faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

            if (Uri.IsWellFormedUriString(faceEndpoint, UriKind.Absolute))
                faceClient.Endpoint = faceEndpoint;
            else
                throw new FormatException("Invalid URI: " + faceEndpoint);
        }

        public async Task<IList<DetectedFace>> UploadAndDetectFaces(Stream imageStream)
        {
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                { FaceAttributeType.Gender, FaceAttributeType.Age };

            IList<DetectedFace> faceList =
                await faceClient.Face.DetectWithStreamAsync(
                    imageStream, true, false, faceAttributes);
            return faceList;
        }
    }
}
