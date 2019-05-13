using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Face API generated class for representing a group of people.
    /// </summary>
    public class PersonGroup
    {
        /// <summary>
        /// Unique identifier of group.
        /// </summary>
        public string personGroupId { get; set; }

        /// <summary>
        /// Name of the group.
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

        /// <summary>
        /// Version of the recognition algorithm used.
        /// <para>
        /// Can be "recognition_01" or "recognition_02".
        /// </para>
        /// </summary>
        public string recognitionModel { get; set; }
    }
}
