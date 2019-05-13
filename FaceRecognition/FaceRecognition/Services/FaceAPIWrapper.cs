using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using FaceRecognition.Models;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Class wrapping all Face API calls.
    /// <para>
    /// Even different devices will see the same list of people.
    /// </para>
    /// </summary>
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

        // TODO Static?
        private PersonGroup group = null;

        private async Task InitGroupAsync()
        {
            if (group != null) return;

            group = await GetPersonGroupAsync();
        }

        /// <summary>
        /// Constructs the FaceAPIWrapper class.
        /// </summary>
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

            // If no persongroup exists on the server, it must be created.
            var group = await CreatePersonGroupAsync();
            return group;
        }

        private async Task<IList<PersonGroup>> ListPersonGroupsAsync()
        {
            var response = await client.GetAsync(uriBase + personGroupUri);
            // Proper Face API error check
            await CheckResponseAsync(response);

            var contentString = await response.Content.ReadAsStringAsync();

            // TODO async
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PersonGroup>>(contentString);
            return result;
        }

        private async Task<PersonGroup> CreatePersonGroupAsync()
        {
            // Always the same persongroup
            // Works somewhat like a server side singleton
            PersonGroup personGroup = new PersonGroup
            {
                personGroupId = "default_id",
                name = "default_group",
                userData = "Default group, every property is hard-coded.",
                recognitionModel = "recognition_02"
            };

            var requestString =
                $@"{{
                ""name"": ""{personGroup.name}"",
                ""userData"": ""{personGroup.userData}"",
                ""recognitionModel"": ""{personGroup.recognitionModel}""
                }}";

            var byteContent = Encoding.UTF8.GetBytes(requestString);
            using (ByteArrayContent content = new ByteArrayContent(byteContent))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                var response = await client.PutAsync(uriBase + personGroupUri + $"/{personGroup.personGroupId}", content);
                // Proper Face API error check
                await CheckResponseAsync(response);

                return personGroup;
            }
        }

        /// <summary>
        /// Saves a person to the server.
        /// </summary>
        /// <param name="person">The person to be saved.</param>
        /// <param name="image">Image of the person's face as a Stream.</param>
        public async Task AddPersonAsync(Person person, Stream image)
        {
            // Lazy initialization
            await InitGroupAsync();

            // Add person
            var requestString =
                $@"{{
                ""name"": ""{person.name}""
                }}";

            var byteContent = Encoding.UTF8.GetBytes(requestString);
            using (ByteArrayContent content = new ByteArrayContent(byteContent))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                var uri = uriBase + personGroupUri + "/" +
                    group.personGroupId + "/" + personUri;

                var response = await client.PostAsync(uri, content);
                // Proper Face API error check
                await CheckResponseAsync(response);

                string contentString = await response.Content.ReadAsStringAsync();

                // TODO async
                Person dummyPerson = Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(contentString);
                person.personId = dummyPerson.personId;
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
                    person.personId + "/persistedFaces";

                var response = await client.PostAsync(uri, content);

                // If the face add operation fails, the person must be deleted.
                if (!response.IsSuccessStatusCode)
                    await DeletePersonAsync(person);
                // Proper Face API error check
                await CheckResponseAsync(response);

                string contentString = await response.Content.ReadAsStringAsync();

                JToken token = JObject.Parse(contentString);
                string faceId = (string)token.SelectToken("persistedFaceId");
                person.persistedFaceIds = new List<string>{ faceId };
            }
        }

        /// <summary>
        /// Deletes the person from the server.
        /// </summary>
        /// <param name="person">The person to be deleted.</param>
        public async Task DeletePersonAsync(Person person)
        {
            // Lazy initialization
            await InitGroupAsync();

            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri + "/" + person.personId;

            var response = await client.DeleteAsync(uri);
            // Proper Face API error check
            await CheckResponseAsync(response);
        }

        /// <summary>
        /// Lists all person from the server.
        /// </summary>
        /// <returns>A list of all the people stored on the server.</returns>
        public async Task<IList<Person>> ListAllPersonAsync()
        {
            // Lazy initialization
            await InitGroupAsync();

            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/" + personUri;

            var response = await client.GetAsync(uri);
            // Proper Face API error check
            await CheckResponseAsync(response);

            string contentString = await response.Content.ReadAsStringAsync();
            // TODO async
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(contentString);
        }

        /// <summary>
        /// Trains the PersonGroup so it can be used for identification.
        /// </summary>
        public async Task TrainPersonGroup()
        {
            // Lazy initialization
            await InitGroupAsync();

            var uri = uriBase + personGroupUri + "/" +
                group.personGroupId + "/train";

            HttpResponseMessage response;

            byte[] byteData = Encoding.UTF8.GetBytes("");
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                response = await client.PostAsync(uri, content);
                // Proper Face API error check
                await CheckResponseAsync(response);
            }
        }

        /// <summary>
        /// Tries to match(Identifies) all the people on the server with the provided faceIds.
        /// </summary>
        /// <param name="faceIds">The Ids of the faces to be identified.</param>
        /// <returns>
        /// Returns a list with all the data that is required to display the 
        /// result of the identification.
        /// </returns>
        public async Task<IList<IdentifyData>> IdentifyInPersonGroup(IList<string> faceIds)
        {
            // Lazy initialization
            await InitGroupAsync();

            var uri = uriBase + "identify";
            var faceIdString = Newtonsoft.Json.JsonConvert.SerializeObject(faceIds);

            var requestString =
                $@"{{
                ""personGroupId"": ""{group.personGroupId}"",
                ""faceIds"": {faceIdString},
                ""confidenceThreshold"": 0.5
                }}";


            HttpResponseMessage response;

            byte[] byteData = Encoding.UTF8.GetBytes(requestString);
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json");

                response = await client.PostAsync(uri, content);
                // Proper Face API error check
                await CheckResponseAsync(response);
            }

            string contentString = await response.Content.ReadAsStringAsync();
            // TODO async
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IList<IdentifyData>>(contentString);
        }

        /// <summary>
        /// Detects faces on the provided image.
        /// </summary>
        /// <param name="imageStream">Image with faces to be detected as a Stream.</param>
        /// <returns>Returns the detected faces.</returns>
        public async Task<IList<Face>> UploadAndDetectFaces(Stream imageStream)
        {
            string requestParameters = 
                "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender" +
                "&recognitionModel=recognition_02" +
                "&returnRecognitionModel=true";

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
                // Proper Face API error check
                await CheckResponseAsync(response);

                string contentString = await response.Content.ReadAsStringAsync();
                // TODO async
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Face>>(contentString);
            }
        }

        private async Task CheckResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return;

            string contentString = await response.Content.ReadAsStringAsync();

            // Detailed Face API errors returned in the body of the response.
            var errorResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIErrorResponse>(contentString);
            var errorString = $"{errorResponse.error.message}({errorResponse.error.code})";
            throw new HttpRequestException(errorString);
        }
    }
}
