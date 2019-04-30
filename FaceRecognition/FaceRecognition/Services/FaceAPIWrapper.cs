using System;
using System.Collections.Generic;
using PCLAppConfig;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using FaceRecognition.Models;
using System.Web;
using System.Text;

namespace FaceRecognition.Services
{
    public class FaceAPIWrapper
    {
        // Use your own API key in the App.config file.
        private string subscriptionKey = ConfigurationManager.AppSettings["APIKey"];

        private const string uriBase =
            "https://northeurope.api.cognitive.microsoft.com/face/v1.0/";

        private const string detectUri = "detect";
        private const string personGroupUri = "persongroups";
        private const string personUri = "persons";

        private HttpClient client = new HttpClient();

        public FaceAPIWrapper()
        {
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        public async Task<PersonGroup> GetPersonGroupAsync()
        {
            var groups = await ListPersonGroupsAsync();
            if (groups.Count > 0)
                return groups[0];

            var group = await CreatePersonGroupAsync();
            return group;
        }

        private async Task<List<PersonGroup>> ListPersonGroupsAsync()
        {
            var response = await client.GetAsync(uriBase + personGroupUri);
            response.EnsureSuccessStatusCode();
            var contentString = await response.Content.ReadAsStringAsync();

            // TODO async
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonGroup>>(contentString);
            return result;
        }

        private async Task<PersonGroup> CreatePersonGroupAsync()
        {
            var requestString = @"{
    ""name"": ""default_group"",
    ""userData"": ""Default group, every property is hard-coded."",
    ""recognitionModel"": ""recognition_02""
}";

            var byteContent = Encoding.UTF8.GetBytes(requestString);
            using (ByteArrayContent content = new ByteArrayContent(byteContent))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync(uriBase + personGroupUri + "/default_id", content);
                response.EnsureSuccessStatusCode();

                string contentString = await response.Content.ReadAsStringAsync();
                return new PersonGroup
                {
                    personGroupId = "defaultId",
                    name = "default_group",
                    userData = "",
                    recognitionModel = "recognition_02"
                };
            }
        }

        public async Task AddPersonAsync(string name, Stream image, PersonGroup group)
        {

            var requestString =
                $@"{{
                ""name"": ""{name}""
                }}";

            Person newPerson;
            var byteContent = Encoding.UTF8.GetBytes(requestString);
            using (ByteArrayContent content = new ByteArrayContent(byteContent))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                var uri = uriBase + personGroupUri + "/" +
                    group.personGroupId + "/" + personUri;

                var response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                string contentString = await response.Content.ReadAsStringAsync();

                // TODO async
                newPerson = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(contentString);
            }

            // Add face.
            MemoryStream ms = new MemoryStream();
            image.CopyTo(ms);
            byte[] byteData = ms.ToArray();
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                var uri = uriBase + personGroupUri + "/" +
                    group.personGroupId + "/" + personUri + "/" +
                    newPerson.personId + "/persistedFaces";

                var response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeletePersonAsync(Person person, PersonGroup group)
        {
            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri + "/" + person.personId;

            var response = await client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IList<Person>> ListAllPersonAsync(PersonGroup group)
        {
            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri;

            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            string contentString = await response.Content.ReadAsStringAsync();
            // TODO async
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(contentString);
        }

        public async Task<IList<Face>> UploadAndDetectFaces(Stream imageStream)
        {
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender";

            string uri = uriBase + detectUri + "?" + requestParameters;

            HttpResponseMessage response;

            MemoryStream ms = new MemoryStream();
            imageStream.CopyTo(ms);
            byte[] byteData = ms.ToArray();

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                string contentString = await response.Content.ReadAsStringAsync();
                // TODO async
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Face>>(contentString);
            }
        }
    }
}
