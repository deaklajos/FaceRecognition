using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognition.Services
{
    /// <summary>
    /// Class for cross platform image taking and picking.
    /// </summary>
    public class ImageProvider
    {
        /// <summary>
        /// Requests the user to take a photo. 
        /// <para>
        /// Checks and requests permissions if required.
        /// </para>
        /// </summary>
        /// <param name="page">Caller Page object for showing messages.</param>
        /// <returns>Returns the image taken as a Stream.</returns>
        public async Task<Stream> TakePhotoAsync(Page page)
        {
            Stream image = null;

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await page.DisplayAlert("Camera required!", "This feature requires your phones camera.", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
                    if (photo != null)
                    {
                        // Copy can make this operation better, because the Stream could be a filestream.
                        var ms = new MemoryStream();
                        await photo.GetStream().CopyToAsync(ms);
                        ms.Position = 0;
                        image = ms;
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await page.DisplayAlert("Camera Denied!", "You have to allow the use of the camera.", "OK");

                }
            }

            catch (Exception ex)
            {
                await page.DisplayAlert("Error!", ex.Message, "OK");
            }

            return image;
        }

        /// <summary>
        /// Requests the user to pic a photo. 
        /// <para>
        /// Checks and requests permissions if required.
        /// </para>
        /// </summary>
        /// <param name="page">Caller Page object for showing messages.</param>
        /// <returns>Returns the image picked as a Stream.</returns>
        public async Task<Stream> PickImageAsync(Page page)
        {

            Stream image = null;

            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await page.DisplayAlert("Storage required!", "This feature requires your phones storage.", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
                    if (photo != null)
                    {
                        // Copy can make this operation better, because the Stream could be a filestream.
                        var ms = new MemoryStream();
                        await photo.GetStream().CopyToAsync(ms);
                        ms.Position = 0;
                        image = ms;
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await page.DisplayAlert("Storage Denied!", "You have to allow the use of the storage.", "OK");

                }
            }

            catch (Exception ex)
            {
                await page.DisplayAlert("Error!", ex.Message, "OK");
            }

            return image;
        }
    }
}
