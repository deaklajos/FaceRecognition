using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    /// <summary>
    /// This class is used for displayig the results of the identification.
    /// </summary>
    public class RectangleData
    {
        /// <summary>
        /// Name of the person.
        /// </summary>
        public string Name;

        /// <summary>
        /// Attributes of the face.
        /// </summary>
        public Faceattributes Faceattributes;

        /// <summary>
        /// Rectange of the face.
        /// </summary>
        public Facerectangle Facerectangle;
    }
}
