using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FaceRecognition
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            CameraButton.Clicked += CameraButton_Clicked;
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Camera required!", "This feature requires your phones camera.", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    //var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
                    if (photo != null)
                    {
                        PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Camera Denied!", "You have to allow the use of the camera.", "OK");

                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "OK");
            }
        }
    }
}
