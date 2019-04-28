using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognition.Services
{
    public class ImageProvider
    {
        public async Task<ImageSource> TakePhotoAsync(Page page)
        {
            ImageSource image = null;

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
                        image = ImageSource.FromStream(() => { return photo.GetStream(); });
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

        public async Task<ImageSource> PickImageAsync(Page page)
        {

            ImageSource image = null;

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
                        image = ImageSource.FromStream(() => { return photo.GetStream(); });
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
