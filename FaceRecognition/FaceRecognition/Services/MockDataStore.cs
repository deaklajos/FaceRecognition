using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceRecognition.Models;
using Xamarin.Forms;

namespace FaceRecognition.Services
{
    public class MockDataStore
    {
        IList<Person> people;
        private FaceAPIWrapper FaceAPIWrapper = new FaceAPIWrapper();

        private async Task InitPeopleAsync()
        {
            if (people != null) return;

            people = await FaceAPIWrapper.ListAllPersonAsync();
        }

        public async Task<bool> AddItemAsync(Person person, Stream image)
        {
            await InitPeopleAsync();
            await FaceAPIWrapper.AddPersonAsync(person, image);
            people.Add(person);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await InitPeopleAsync();
            var oldItem = people.Where((Person arg) => arg.personId == id).FirstOrDefault();
            await FaceAPIWrapper.DeletePersonAsync(oldItem);
            people.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Person> GetItemAsync(string id)
        {
            await InitPeopleAsync();
            return await Task.FromResult(people.FirstOrDefault(s => s.personId == id));
        }

        public async Task<IEnumerable<Person>> GetItemsAsync(bool forceRefresh = false)
        {
            var isRefreshRequired = false;
            if (forceRefresh)
                if (people != null)
                    isRefreshRequired = true;

            await InitPeopleAsync();

            if(isRefreshRequired)
                people = await FaceAPIWrapper.ListAllPersonAsync();

            return await Task.FromResult(people);
        }
    }
}