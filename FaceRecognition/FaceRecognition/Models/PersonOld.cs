using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognition.Models
{
    public class PersonOld
    {
        public string Id { get; set; }
        public string Name { get; set; }
        private Stream imageStream;

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

        public async Task SetImageStreamAsync(Stream stream)
        {
            imageStream = stream;
            var tmpStream = await GetImageStreamAsync();
            FaceImage = ImageSource.FromStream(() => { return tmpStream; });
        }

        //public Stream ImageStream {
        //    get
        //    {
        //        // Copy required beause it seems like
        //        // ImageSource.FromStream calls Dispose
        //        // on the Stream.
        //        var ms = new MemoryStream();
        //        await imageStream.CopyToAsync(ms);// TODO ASYNC COPY
        //        // Reset the streams.
        //        imageStream.Position = 0;
        //        ms.Position = 0;
        //        return ms;
        //    }
        //    set
        //    {
        //        imageStream = value;
        //        FaceImage = ImageSource.FromStream(() => { return ImageStream; });
        //    }
        //}

        public ImageSource FaceImage { get; private set; }

        // TODO remove this property.
        public string Description { get; set; }
    }
}