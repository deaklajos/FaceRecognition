using System;
using Xamarin.Forms;

namespace FaceRecognition.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ImageSource FaceImage { get; set; }
        // TODO remove this property.
        public string Description { get; set; }
    }
}