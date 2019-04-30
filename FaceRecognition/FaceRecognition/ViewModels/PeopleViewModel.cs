using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FaceRecognition.Models;
using FaceRecognition.Views;
using FaceRecognition.Services;
using System.IO;

namespace FaceRecognition.ViewModels
{
    public class PeopleViewModel : BaseViewModel
    {
        public ObservableCollection<Person> People { get; set; }
        public Command LoadItemsCommand { get; set; }
        public MockDataStore DataStore = new MockDataStore();

        public PeopleViewModel()
        {
            Title = "Browse";
            People = new ObservableCollection<Person>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public async Task AddPersonAsync(Person person, Stream image)
        {
            await DataStore.AddItemAsync(person, image);
            People.Add(person);
        }

        public async Task DeletePersonAsync(Person person)
        {
            await DataStore.DeleteItemAsync(person.personId);
            People.Remove(person);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                People.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    People.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}