﻿using System;
using System.Collections.Generic;
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
        // Use your own API key.
        private string subscriptionKey = APIKeyProvider.APIKey;

        private const string uriBase =
            "https://northeurope.api.cognitive.microsoft.com/face/v1.0/";

        private const string detectUri = "detect";
        private const string personGroupUri = "persongroups";
        private const string personUri = "persons";

        private HttpClient client = new HttpClient();
        private PersonGroup group = null;

        private async Task InitGroupAsync()
        {
            if (group != null) return;

            group = await GetPersonGroupAsync();
        }

        public FaceAPIWrapper()
        {
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        private async Task<PersonGroup> GetPersonGroupAsync()
        {
            var groups = await ListPersonGroupsAsync();
            if (groups.Count > 0)
                return groups[0];

            var group = await CreatePersonGroupAsync();
            return group;
        }

        private async Task<IList<PersonGroup>> ListPersonGroupsAsync()
        {
            var response = await client.GetAsync(uriBase + personGroupUri);
            await CheckResponseAsync(response);

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
                await CheckResponseAsync(response);

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

        public async Task AddPersonAsync(string name, Stream image)
        {
            await InitGroupAsync();

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
                await CheckResponseAsync(response);

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
                if (!response.IsSuccessStatusCode)
                    await DeletePersonAsync(newPerson);
                await CheckResponseAsync(response);
            }
        }

        public async Task DeletePersonAsync(Person person)
        {
            await InitGroupAsync();

            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri + "/" + person.personId;

            var response = await client.DeleteAsync(uri);
            await CheckResponseAsync(response);
        }

        public async Task<IList<Person>> ListAllPersonAsync()
        {
            await InitGroupAsync();

            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri;

            var response = await client.GetAsync(uri);
            await CheckResponseAsync(response);

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
                await CheckResponseAsync(response);

                string contentString = await response.Content.ReadAsStringAsync();
                // TODO async
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Face>>(contentString);
            }
        }

        public async Task CheckResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            string contentString = await response.Content.ReadAsStringAsync();

            var errorResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIErrorResponse>(contentString);
            var errorString = $"{errorResponse.error.message}({errorResponse.error.code})";
            throw new HttpRequestException(errorString);
        }
    }
}
