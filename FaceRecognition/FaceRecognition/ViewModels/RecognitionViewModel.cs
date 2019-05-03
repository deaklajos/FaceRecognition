using FaceRecognition.Models;
using FaceRecognition.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    public class RecognitionViewModel : ImageViewModel
    {
        public RecognitionViewModel()
        {
            Title = "Recognition";
        }

        public async Task<IEnumerable<RectangleData>> IndentifyAsync()
        {
            var imageStream = await GetImageStreamAsync();
            var faceList = await FaceAPIWrapper.UploadAndDetectFaces(imageStream);
            var faceIds = new List<string>();
            foreach (var item in faceList)
                faceIds.Add(item.faceId);

            var IndentifyDataList = await FaceAPIWrapper.IndentifyInPersonGroup(faceIds);

            var personList = await FaceAPIWrapper.ListAllPersonAsync();

            var rectangleData = from face in faceList
                        join indentifyData in IndentifyDataList
                        on face.faceId equals indentifyData.faceId
                        join person in personList
                        on indentifyData.candidates.FirstOrDefault().personId equals person.personId
                        select new RectangleData
                        {
                            Name = person.name,
                            Faceattributes = face.faceAttributes,
                            Facerectangle = face.faceRectangle
                        };

            return rectangleData;
        }
    }
}