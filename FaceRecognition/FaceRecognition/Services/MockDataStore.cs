using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceRecognition.Models;
using Xamarin.Forms;

namespace FaceRecognition.Services
{
    public class MockDataStore : IDataStore<Person>
    {
        IList<Person> people;
        private FaceAPIWrapper FaceAPIWrapper = new FaceAPIWrapper();

        private async Task InitPeopleAsync()
        {
            if (people != null) return;

            people = await FaceAPIWrapper.ListAllPersonAsync();
        }

        public async Task<bool> AddItemAsync(Person person)
        {
            await InitPeopleAsync();
            people.Add(person);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Person item)
        {
            await InitPeopleAsync();
            var oldItem = people.Where((Person arg) => arg.personId == item.personId).FirstOrDefault();
            people.Remove(oldItem);
            people.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            await InitPeopleAsync();
            var oldItem = people.Where((Person arg) => arg.personId == id).FirstOrDefault();
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
            await InitPeopleAsync();
            return await Task.FromResult(people);
        }
    }
}