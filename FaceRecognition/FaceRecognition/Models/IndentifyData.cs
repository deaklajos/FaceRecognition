using System;
using System.Collections.Generic;
using System.Text;

namespace FaceRecognition.Models
{
    public class IndentifyData
    {
        public string faceId { get; set; }
        public Candidate[] candidates { get; set; }

        public class Candidate
        {
            public string personId { get; set; }
            public float confidence { get; set; }
        }

    }
}
