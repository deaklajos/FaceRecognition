using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    public class Person
    {
        public string personId { get; set; }
        public string[] persistedFaceIds { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
    }
}
