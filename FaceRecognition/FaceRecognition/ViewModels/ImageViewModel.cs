using FaceRecognition.Models;
using FaceRecognition.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognition.ViewModels
{
    public class ImageViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public ImageProvider ImageProvider { get; } = new ImageProvider();
        public FaceAPIWrapper FaceAPIWrapper { get; } = new FaceAPIWrapper();
        private Stream imageStream;
        public bool IsImageSet { get; private set; } = false;
        private ImageSource image = ImageSource.FromResource("FaceRecognition.baseline_photo.png");
        public ImageSource Image
        {
            get { return image; }
            private set
            {
                if (value != null)
                {
                    SetProperty(ref image, value);
                    IsImageSet = true;
                }
            }
        }

        public ImageViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }

        public async Task PickImageAsync(Page page)
        {
            var image = await ImageProvider.PickImageAsync(page);
            if (image != null)
            {
                await SetImageStreamAsync(image);
            }
        }

        public async Task TakePhotoAsync(Page page)
        {
            var image = await ImageProvider.TakePhotoAsync(page);
            if (image != null)
            {
                await SetImageStreamAsync(image);
            }
        }

        public async Task<Stream> GetImageStreamAsync()
        {
            // TODO test if only ImageSource requires copy
            // Copy required beause it seems like
            // ImageSource.FromStream calls Dispose
            // on the Stream.
            var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            // Reset the streams.
            imageStream.Position = 0;
            ms.Position = 0;
            return ms;
        }

        private async Task SetImageStreamAsync(Stream stream)
        {
            imageStream = stream;
            var tmpStream = await GetImageStreamAsync();
            Image = ImageSource.FromStream(() => { return tmpStream; });
        }

    }
}
