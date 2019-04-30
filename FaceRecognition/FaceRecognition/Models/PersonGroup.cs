using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition.Models
{
    public class PersonGroup
    {
        public string personGroupId { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
        public string recognitionModel { get; set; }
    }
}
