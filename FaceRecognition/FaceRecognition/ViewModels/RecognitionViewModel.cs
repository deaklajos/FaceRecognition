using FaceRecognition.Models;
using FaceRecognition.Services;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace FaceRecognition.ViewModels
{
    /// <summary>
    /// ViewModel for RecognitionPage.
    /// </summary>
    public class RecognitionViewModel : ImageViewModel
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RecognitionViewModel()
        {
            Title = "Recognition";
        }

        /// <summary>
        /// Identifies the people on the Page's image.
        /// </summary>
        /// <returns>A list for diplayig the results.</returns>
        public async Task<IEnumerable<RectangleData>> IdentifyAsync()
        {
            // Detect faces
            var imageStream = await GetImageStreamAsync();
            var faceList = await FaceAPIWrapper.UploadAndDetectFaces(imageStream);
            if(faceList.Count == 0) throw new HttpRequestException("No face detected.");

            var faceIds = new List<string>();
            foreach (var item in faceList)
                faceIds.Add(item.faceId);

            // Identify
            var IdentifyDataList = await FaceAPIWrapper.IdentifyInPersonGroup(faceIds);

            var personList = await FaceAPIWrapper.ListAllPersonAsync();

            // Matching the faces with the people.
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