using System;

using FaceRecognition.Models;

namespace FaceRecognition.ViewModels
{
    /// <summary>
    /// ViewModel for PersonDetailPage.
    /// </summary>
    public class PersonDetailViewModel : BaseViewModel
    {
        public Person Person { get; set; }

        /// <summary>
        /// Cpnstructor.
        /// </summary>
        /// <param name="item">Person for the ViewModel.</param>
        public PersonDetailViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }
    }
}
