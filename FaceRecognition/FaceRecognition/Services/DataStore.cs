using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceRecognition.Models;

namespace FaceRecognition.Services
{
    public class DataStore
    {
        private IList<Person> people;
        private FaceAPIWrapper FaceAPIWrapper = new FaceAPIWrapper();

        private async Task InitPeopleAsync()
        {
            if (people != null) return;

            people = await FaceAPIWrapper.ListAllPersonAsync();
        }

        /// <summary>
        /// Adds a person to the stored list and to the server.
        /// </summary>
        /// <param name="person">The person to be added.</param>
        /// <param name="image">Image of the person.</param>
        /// <returns>Returns true.</returns>
        public async Task<bool> AddItemAsync(Person person, Stream image)
        {
            // Lazy initialization
            await InitPeopleAsync();
            await FaceAPIWrapper.AddPersonAsync(person, image);
            people.Add(person);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Deletse a person from the stored list and from the server.
        /// </summary>
        /// <param name="id">Id of the person.</param>
        /// <returns>Returns true.</returns>
        public async Task<bool> DeleteItemAsync(string id)
        {
            // Lazy initialization
            await InitPeopleAsync();
            var oldItem = people.Where((Person arg) => arg.personId == id).FirstOrDefault();
            await FaceAPIWrapper.DeletePersonAsync(oldItem);
            people.Remove(oldItem);

            return await Task.FromResult(true);
        }

        /// <summary>
        /// Single element getter for the person list.
        /// </summary>
        /// <param name="id">Id of the person.</param>
        /// <returns>Returns true.</returns>
        public async Task<Person> GetItemAsync(string id)
        {
            // Lazy initialization
            await InitPeopleAsync();
            return await Task.FromResult(people.FirstOrDefault(s => s.personId == id));
        }

        /// <summary>
        /// Person list getter.
        /// </summary>
        /// <param name="forceRefresh">If set to true, the data is synced with the server.</param>
        /// <returns>Returns the list of people.</returns>
        public async Task<IEnumerable<Person>> GetItemsAsync(bool forceRefresh = false)
        {
            var isRefreshRequired = false;
            if (forceRefresh)
                if (people != null)
                    isRefreshRequired = true;

            // Lazy initialization
            await InitPeopleAsync();

            if(isRefreshRequired)
                people = await FaceAPIWrapper.ListAllPersonAsync();

            return await Task.FromResult(people);
        }
    }
}