using FaceRecognition.Models;
using FaceRecognition.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using System.Net.Http;

namespace FaceRecognition.ViewModels
{
    public class RecognitionViewModel : ImageViewModel
    {
        public RecognitionViewModel()
        {
            Title = "Recognition";
        }

        public async Task<IEnumerable<RectangleData>> IdentifyAsync()
        {
            var imageStream = await GetImageStreamAsync();
            var faceList = await FaceAPIWrapper.UploadAndDetectFaces(imageStream);
            if(faceList.Count == 0) throw new HttpRequestException("No face detected.");

            var faceIds = new List<string>();
            foreach (var item in faceList)
                faceIds.Add(item.faceId);

            var IdentifyDataList = await FaceAPIWrapper.IdentifyInPersonGroup(faceIds);

            var personList = await FaceAPIWrapper.ListAllPersonAsync();

            // Left outer join
            var rectangleData = from face in faceList
                        join identifyData in IdentifyDataList
                        on face.faceId equals identifyData.faceId
                        join person in personList
                        on identifyData.candidates.FirstOrDefault()?.personId equals person.personId 
                        into result
                        from person in result.DefaultIfEmpty()
                        select new RectangleData
                        {
                            Name = person == null ? "Unknown" : person.name,
                            Faceattributes = face.faceAttributes,
                            Facerectangle = face.faceRectangle
                        };

            return rectangleData;
        }
    }
}