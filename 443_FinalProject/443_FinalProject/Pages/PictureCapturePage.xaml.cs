using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using _443_FinalProject.Data;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _443_FinalProject.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PictureCapturePage : _443_FinalProject.Common.LayoutAwarePage
    {

        private const String videoKey = "capturedVideo";
        private const String fileKey = "filePath";
        public StorageFile videoFile;
        public StorageFile imageFile;
        public BitmapImage videoImage;
        private readonly String PHOTO_FILE_NAME = "Archive Temp Photo.jpg";
        public PictureCapturePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        #region OnNavigatedTo
        /// <summary>
        /// Instead of using the capture button on this page, 
        /// we're assuming that this page is only navigated to from 
        /// the main page, whereby the user would be expecting to record a video of themselves. 
        /// 
        /// 
        /// Open camera dialog so user is immediately able to record video. 
        /// </summary>
        /// <param name="e"></param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // Open up the camera 
                CameraCaptureUI dialog = new CameraCaptureUI();
                dialog.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg; 

                // Capture a video file 
                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

                // If the video file isn't null: 
                if (file != null)
                {
                    photo_metadataPopup.IsOpen = true;
                    videoFile = file;
                    // Get thumbnail of the video file 
                    var thumb = await videoFile.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView, 1000, Windows.Storage.FileProperties.ThumbnailOptions.UseCurrentScale);

                    // Create a Buffer object to hold raw picture data
                    var buffer = new Windows.Storage.Streams.Buffer(Convert.ToUInt32(thumb.Size));

                    // Read the raw picture data into the Buffer object 
                    await thumb.ReadAsync(buffer, Convert.ToUInt32(thumb.Size), InputStreamOptions.None);

                    // Open LocalFolder
                    var folder = ApplicationData.Current.LocalFolder;

                    // Create (or open if one exists) a folder called temp images
                    folder = await folder.CreateFolderAsync("temp images", CreationCollisionOption.OpenIfExists);

                    // Create a StorageFile 
                    imageFile = await folder.CreateFileAsync(PHOTO_FILE_NAME, CreationCollisionOption.ReplaceExisting);

                    // Write picture data to the file 
                    await FileIO.WriteBufferAsync(imageFile, buffer);

                    // Preview the image
                    var bmpimg = new BitmapImage();
                    bmpimg.SetSource(thumb);
                    videoImage = bmpimg;

                }
            }
            catch (Exception ex)
            {
                // Do something here!!!
            }

        }
        #endregion 

        private void submitPhotoButton_Click_1(object sender, RoutedEventArgs e)
        {
            photo_metadataPopup.IsOpen = false; 
            var photoTitle = titleTxtBox.Text;
            var photoDescription = descriptionTxtBox.Text;
            var newPhoto = new SampleDataItem("", photoTitle, "", imageFile.Path, photoDescription, "", App.currentGroup);

            App.currentGroup.Items.Add(newPhoto);

            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)App.currentGroup).UniqueId); 
        }

        private void cancelUploadButton_Click_1(object sender, RoutedEventArgs e)
        {
            photo_metadataPopup.IsOpen = false; 
        }
    }
}
