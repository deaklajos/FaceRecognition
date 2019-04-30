using System;

using FaceRecognition.Models;
using FaceRecognition.Services;

namespace FaceRecognition.ViewModels
{
    public class NewPersonViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public ImageProvider imageProvider = new ImageProvider();

        public NewPersonViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }
    }
}
