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
        List<Person> people;

        public MockDataStore()
        {
            people = new List<Person>();
            var mockItems = new List<Person>
            {
                new Person { Id = Guid.NewGuid().ToString(), Name = "First item", Description="This is an item description." },
                new Person { Id = Guid.NewGuid().ToString(), Name = "Second item", Description="This is an item description." },
                new Person { Id = Guid.NewGuid().ToString(), Name = "Third item", Description="This is an item description." },
                new Person { Id = Guid.NewGuid().ToString(), Name = "Fourth item", Description="This is an item description." },
                new Person { Id = Guid.NewGuid().ToString(), Name = "Fifth item", Description="This is an item description." },
                new Person { Id = Guid.NewGuid().ToString(), Name = "Sixth item", Description="This is an item description." },
            };

            foreach (var person in mockItems)
            {
                people.Add(person);
            }
        }

        public async Task<bool> AddItemAsync(Person person)
        {
            people.Add(person);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Person item)
        {
            var oldItem = people.Where((Person arg) => arg.Id == item.Id).FirstOrDefault();
            people.Remove(oldItem);
            people.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = people.Where((Person arg) => arg.Id == id).FirstOrDefault();
            people.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Person> GetItemAsync(string id)
        {
            return await Task.FromResult(people.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Person>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(people);
        }
    }
}