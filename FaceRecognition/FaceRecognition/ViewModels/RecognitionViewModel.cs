using FaceRecognition.Models;
using FaceRecognition.Services;
using System;
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

        public async Task<IList<Facerectangle>> IndentifyAsync()
        {
            var imageStream = await GetImageStreamAsync();
            var faceList = await FaceAPIWrapper.UploadAndDetectFaces(imageStream);
            var faceIds = new List<string>();
            foreach (var item in faceList)
                faceIds.Add(item.faceId);

            var IndentifyData = await FaceAPIWrapper.IndentifyInPersonGroup(faceIds);

            // TODO All or one person.
            return new List<Facerectangle>();
        }
    }
}