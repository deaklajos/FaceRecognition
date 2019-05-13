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
    /// <summary>
    /// ViewModel for Pages that contain an image.
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        public Person Person { get; set; }
        public ImageProvider ImageProvider { get; } = new ImageProvider();
        public FaceAPIWrapper FaceAPIWrapper { get; } = new FaceAPIWrapper();
        private Stream imageStream;

        /// <summary>
        /// True after the image is set.
        /// </summary>
        public bool IsImageSet { get; private set; } = false;

        /// <summary>
        /// True if the PickImageAsync or TakePhotoAsync operation completed succesfully.
        /// </summary>
        public bool IsImageSetCompleted { get; private set; } = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="item">Person for the ViewModel.</param>
        public ImageViewModel(Person item = null)
        {
            Title = item?.name;
            Person = item;
        }

        /// <summary>
        /// Requests the user to pick an image.
        /// </summary>
        /// <param name="page">Caller page for displaying messages.</param>
        public async Task PickImageAsync(Page page)
        {
            IsImageSetCompleted = false;
            var image = await ImageProvider.PickImageAsync(page);
            if (image != null)
            {
                await SetImageStreamAsync(image);
                IsImageSetCompleted = true;
            }
        }

        /// <summary>
        /// Requests the user to take an image.
        /// </summary>
        /// <param name="page">Caller page for displaying messages.</param>
        public async Task TakePhotoAsync(Page page)
        {
            IsImageSetCompleted = false;
            var image = await ImageProvider.TakePhotoAsync(page);
            if (image != null)
            {
                await SetImageStreamAsync(image);
                IsImageSetCompleted = true;
            }
        }

        /// <summary>
        /// Getter for the image.
        /// </summary>
        /// <returns>Returns the image as a Stream.</returns>
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

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="stream">Image as a Stream.</param>
        /// <returns></returns>
        private async Task SetImageStreamAsync(Stream stream)
        {
            imageStream = stream;
            var tmpStream = await GetImageStreamAsync();
            IsImageSet = true;
        }

    }
}
