﻿using _443_FinalProject.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using _443_FinalProject.Pages;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace _443_FinalProject
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : _443_FinalProject.Common.LayoutAwarePage
    {
        int eventCounter;       // This is a hack to fix this weird bug... 
        StorageFile videoFile, imageFile;
        BitmapImage videoImage;
        String PHOTO_FILE_NAME = "Archive Temp Photo.jpg";

        public GroupDetailPage()
        {
            this.InitializeComponent();
            eventCounter = 0; 
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
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var group = SampleDataSource.GetGroup((String)navigationParameter);
            App.currentGroup = group;
            this.DefaultViewModel["Group"] = group;
            this.DefaultViewModel["Items"] = group.Items;
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void addContentComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                switch (addContentComboBox.SelectedIndex)
                {
                    case 0: break;
                    case 1:
                        addPhotoPopUp.IsOpen = true; 
                        break;
                    case 2:
                        addVideoPopUp.IsOpen = true;
                        break;
                    case 3:
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }

        private async void uploadVideoButton_Click_1(object sender, RoutedEventArgs e)
        {
            addVideoPopUp.IsOpen = false;
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            filePicker.SettingsIdentifier = "picker1";
            filePicker.CommitButtonText = "Import video";

            var file = await filePicker.PickSingleFileAsync(); 

            

             // If the video file isn't null: 
                if (file != null)
                {
                    
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
                    imageFile = await folder.CreateFileAsync(PHOTO_FILE_NAME, CreationCollisionOption.GenerateUniqueName);

                    // Write picture data to the file 
                    await FileIO.WriteBufferAsync(imageFile, buffer);

                    // Preview the image
                    var bmpimg = new BitmapImage();
                    bmpimg.SetSource(thumb);
                    videoImage = bmpimg;

                    var newVideoItem = new SampleDataItem("", videoFile.Name, "", imageFile.Path, "", "", App.currentGroup);

                    App.currentGroup.Items.Add(newVideoItem);

                    this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)App.currentGroup).UniqueId); 

                }
        }

        private void captureVideoButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(VideoCapturePage)); 
            addVideoPopUp.IsOpen = false; 
        }

        private void addVideoPopUp_Closed_1(object sender, object e)
        {
           
        }

        private void addContentComboBox_DropDownClosed_1(object sender, object e)
        {
            try
            {


                switch (addContentComboBox.SelectedIndex)
                {
                    case 0: break;
                    case 1:
                        addPhotoPopUp.IsOpen = true;
                        break;
                    case 2:
                        addVideoPopUp.IsOpen = true;
                        break;
                    case 3:
                        this.Frame.Navigate(typeof(NewTextEntryPage)); 
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }

        private void cancelVideoButton_Click_1(object sender, RoutedEventArgs e)
        {
            addPhotoPopUp.IsOpen = false;
            addVideoPopUp.IsOpen = false; 
        }

        private void addPhotoPopUp_Closed_1(object sender, object e)
        {

        }

        private async void uploadPhotoButton_Click_1(object sender, RoutedEventArgs e)
        {
            addPhotoPopUp.IsOpen = false; 
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png"); 
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picker1";
            filePicker.CommitButtonText = "Import picture";


            var file = await filePicker.PickSingleFileAsync();


            // If the video file isn't null: 
            if (file != null)
            {

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
                imageFile = await folder.CreateFileAsync(PHOTO_FILE_NAME, CreationCollisionOption.GenerateUniqueName);

                // Write picture data to the file 
                await FileIO.WriteBufferAsync(imageFile, buffer);

                // Preview the image
                var bmpimg = new BitmapImage();
                bmpimg.SetSource(thumb);
                videoImage = bmpimg;

                var newVideoItem = new SampleDataItem(videoFile.Name, videoFile.Name, "", imageFile.Path, "", "", App.currentGroup);

                App.currentGroup.Items.Add(newVideoItem);

                this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)App.currentGroup).UniqueId);

            }
        }

        private void capturePhoto_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PictureCapturePage));
            addPhotoPopUp.IsOpen = false; 
        }

        private void shareBtn_Click_1(object sender, RoutedEventArgs e)
        {
            shareTimelinePopup.IsOpen = true; 
        }

        private async void shareButton_Click_1(object sender, RoutedEventArgs e)
        {
            var output = string.Format("Your timeline has been shared.");
            Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(output);
            await dialog.ShowAsync();

            shareTimelinePopup.IsOpen = false;
        }

        private void cancelButton_Click_1(object sender, RoutedEventArgs e)
        {
            shareTimelinePopup.IsOpen = false; 

        }

        private void recipientEmailTxtBox_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                
                if(eventCounter % 2 == 0)
                {
                    eventCounter++;
                    var newRecipient = recipientEmailTxtBox.Text;
                    var newListBoxItem = new ListBoxItem();
                    newListBoxItem.Content = newRecipient;
                    recipientsListBox.Items.Add(newListBoxItem);
                    recipientEmailTxtBox.Text = "";
                    
                }
                else
                    eventCounter++;

            }

            
        }

        private void addRecipientButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (eventCounter % 2 == 0)
            {
                eventCounter++;
                var newRecipient = recipientEmailTxtBox.Text;
                var newListBoxItem = new ListBoxItem();
                newListBoxItem.Content = newRecipient;
                recipientsListBox.Items.Add(newListBoxItem);
                recipientEmailTxtBox.Text = "";

            }
            else
                eventCounter++;
        }

        private void homeBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupedItemsPage), "AllGroups"); 
        }

        private void privateRadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void publicRadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void submitTimelineButton_Click_1(object sender, RoutedEventArgs e)
        {
            newTimelinePopup.IsOpen = false;
            var timelineName = timelineNameTxtBox.Text;
            var new_group = new SampleDataGroup(timelineName, timelineName, "", "", "");

            App._sampleDataSource.AllGroups.Insert(0, new_group);
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)new_group).UniqueId);
        }

        private void cancelTimelineButton_Click_1(object sender, RoutedEventArgs e)
        {
            newTimelinePopup.IsOpen = false;
        }

        private void timelineNameTxtBox_KeyUp_1(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                submitTimelineButton_Click_1(sender, e);
            }
        }
    }
}
