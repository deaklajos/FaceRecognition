using System;

using FaceRecognition.Models;
using FaceRecognition.Services;

namespace FaceRecognition.ViewModels
{
    public class NewPersonViewModel : BaseViewModel
    {
        public PersonOld Person { get; set; }
        public ImageProvider imageProvider = new ImageProvider();

        public NewPersonViewModel(PersonOld item = null)
        {
            Title = item?.Name;
            Person = item;
        }
    }
}
