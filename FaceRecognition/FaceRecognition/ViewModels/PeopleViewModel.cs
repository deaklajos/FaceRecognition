using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using FaceRecognition.Models;
using FaceRecognition.Services;
using System.IO;

namespace FaceRecognition.ViewModels
{
    /// <summary>
    /// ViewModel for PeoplePage.
    /// </summary>
    public class PeopleViewModel : BaseViewModel
    {
        public ObservableCollection<Person> People { get; set; }
        public Command LoadItemsCommand { get; set; }
        public DataStore DataStore = new DataStore();
        public FaceAPIWrapper FaceAPIWrapper = new FaceAPIWrapper();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PeopleViewModel()
        {
            Title = "Browse";
            People = new ObservableCollection<Person>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        /// <summary>
        /// Adds a person.
        /// </summary>
        /// <param name="person">The person to be added.</param>
        /// <param name="image">Image of the person.</param>
        public async Task AddPersonAsync(Person person, Stream image)
        {
            await DataStore.AddItemAsync(person, image);
            People.Add(person);
        }

        /// <summary>
        /// Deletes a person.
        /// </summary>
        /// <param name="person">The person to be deleted.</param>
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
                MessagingCenter.Send(this, "LoadException",
                    $"Load failed, check your internet connection.\nDetails:\n{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}