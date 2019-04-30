using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    public class APIErrorResponse
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}
