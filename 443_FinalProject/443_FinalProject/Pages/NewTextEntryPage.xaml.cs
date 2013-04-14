using _443_FinalProject.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _443_FinalProject.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class NewTextEntryPage : _443_FinalProject.Common.LayoutAwarePage
    {
        public StorageFile videoFile;
        public StorageFile imageFile;
        public BitmapImage videoImage;
        private readonly String PHOTO_FILE_NAME = "Archive Temp Photo.jpg";

        public NewTextEntryPage()
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

        private void pageTitle_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            pageTitle.Visibility = Visibility.Collapsed;
            textTitleTxtBox.Visibility = Visibility.Visible;
            
        }

        private void newvideoBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void timelineComboBox_DropDownClosed_1(object sender, object e)
        {
            foreach (SampleDataGroup group in App._sampleDataSource.AllGroups)
            {
                if (group.Title == timelineComboBox.SelectedItem.ToString())
                    App.currentGroup = group;
            }
        }

        private async void Add_Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.currentGroup == null)           //  The user didn't pick anything 
            {
                selectTimelinePopup.IsOpen = true;
                // Add all of the available timelines to the combobox


                foreach (SampleDataGroup group in App._sampleDataSource.AllGroups)
                {
                    timelineComboBox.Items.Insert(0, group.Title.ToString());
                }
                timelineComboBox.Items.Insert(0, "New timeline");
                timelineComboBox.SelectedIndex = 0;
                return; 
            }

            selectTimelinePopup.IsOpen = false;
            var title = textTitleTxtBox.Text;
            var description = descriptionTxtBox.Text;
            var newVideoItem = new SampleDataItem("", title, "", "Assets/DarkGray.png",description,"", App.currentGroup);
            App.currentGroup.Items.Add(newVideoItem);

            this.Frame.Navigate(typeof(GroupDetailPage), App.currentGroup.UniqueId);


        }

        private void Cancel_Button_Click_1(object sender, RoutedEventArgs e)
        {
            selectTimelinePopup.IsOpen = false;
        }

        private void saveTextEntry_Click_1(object sender, RoutedEventArgs e)
        {
            if (App.currentGroup == null)           //  The user didn't pick anything 
            {
                selectTimelinePopup.IsOpen = true;
                // Add all of the available timelines to the combobox


                foreach (SampleDataGroup group in App._sampleDataSource.AllGroups)
                {
                    timelineComboBox.Items.Insert(0, group.Title.ToString());
                }
                timelineComboBox.Items.Insert(0, "New timeline");
                timelineComboBox.SelectedIndex = 0;
                return;
            }

            selectTimelinePopup.IsOpen = false;
            var title = textTitleTxtBox.Text;
            var description = descriptionTxtBox.Text;
            var newVideoItem = new SampleDataItem("", title, "", "Assets/DarkGray.png", description, "", App.currentGroup);
            App.currentGroup.Items.Add(newVideoItem);

            this.Frame.Navigate(typeof(GroupDetailPage), App.currentGroup.UniqueId);
        }


    }
}
