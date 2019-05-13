using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Face API generated class for representing a person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Unique identifier of the person.
        /// </summary>
        public string personId { get; set; }

        /// <summary>
        /// Faces of the person.
        /// <para>
        /// This application works with only one face but the deserialization 
        /// process requires a list.
        /// </para>
        /// </summary>
        public IList<string> persistedFaceIds { get; set; }

        /// <summary>
        /// Name of the person.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Additional user data.
        /// <para>
        /// This application will not use this field but the deserialization 
        /// process requires it.
        /// </para>
        /// </summary>
        public string userData { get; set; }
    }
}
