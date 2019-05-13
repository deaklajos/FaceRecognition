
using FaceRecognition.Models;
using FaceRecognition.Services;

namespace FaceRecognition.ViewModels
{
    /// <summary>
    /// ViewModel for NewPersonPage.
    /// </summary>
    public class NewPersonViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public ImageProvider imageProvider = new ImageProvider();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Person for the ViewModel.</param>
        public NewPersonViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }
    }
}
