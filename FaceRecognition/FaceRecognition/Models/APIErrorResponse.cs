using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Generated class for wrapping Face API errors for JSON deserialization.
    /// </summary>
    public class APIErrorResponse
    {
        /// <summary>
        /// Error object.
        /// </summary>
        public Error error { get; set; }
    }

    /// <summary>
    /// Generated class for receiving Face API errors.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// HTTP error code.
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// Detailed description of the error.
        /// </summary>
        public string message { get; set; }
    }
}
