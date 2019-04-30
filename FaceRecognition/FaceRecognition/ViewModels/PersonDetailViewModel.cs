using System;

using FaceRecognition.Models;

namespace FaceRecognition.ViewModels
{
    public class PersonDetailViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public PersonDetailViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }
    }
}
