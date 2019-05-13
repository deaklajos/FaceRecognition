using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    /// <summary>
    /// Face API generated class for receiving Identification data.
    /// </summary>
    public class IdentifyData
    {
        /// <summary>
        /// Id of the face stored on the server.
        /// </summary>
        public string faceId { get; set; }
        public Candidate[] candidates { get; set; }

        /// <summary>
        /// Face API generated class for receiving Identification candidates.
        /// </summary>
        public class Candidate
        {
            /// <summary>
            /// Id of the person.
            /// </summary>
            public string personId { get; set; }

            /// <summary>
            /// Describes, how confident is the algorithm about the person-face match. 
            /// </summary>
            public float confidence { get; set; }
        }

    }
}
