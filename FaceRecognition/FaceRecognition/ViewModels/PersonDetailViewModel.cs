using System;

using FaceRecognition.Models;

namespace FaceRecognition.ViewModels
{
    public class PersonDetailViewModel : BaseViewModel
    {
        public PersonOld Person { get; set; }
        public PersonDetailViewModel(PersonOld item = null)
        {
            Title = item?.Name;
            Person = item;
        }
    }
}
