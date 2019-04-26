using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FaceRecognition.Models;
using FaceRecognition.Views;

namespace FaceRecognition.ViewModels
{
    public class PeopleViewModel : BaseViewModel
    {
        public ObservableCollection<Person> People { get; set; }
        public Command LoadItemsCommand { get; set; }

        public PeopleViewModel()
        {
            Title = "Browse";
            People = new ObservableCollection<Person>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewPersonPage, Person>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Person;
                People.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
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