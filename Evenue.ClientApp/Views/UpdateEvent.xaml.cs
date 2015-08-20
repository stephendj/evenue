using Evenue.ClientApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Evenue.ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateEvent : Page
    {
        private Event _event { get; set; }
        private StorageFile image = null;
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();
        List<string> comboBoxItems = new List<string>()
        {
            "Music", "Food & Drink", "Sport", "Art & Culture", "Party", "Technology", "Other"
        };

        public UpdateEvent()
        {
            this.InitializeComponent();
            categoryComboBox.ItemsSource = comboBoxItems;
        }

        // Parse date to get day, month, and year
        public int[] ParseDate(string date)
        {
            string[] stringdate = date.Split('/');
            return new int[3] { int.Parse(stringdate[0]), int.Parse(stringdate[1]), int.Parse(stringdate[2]) };
        }

        // Validate all Fields
        private bool ValidateForm()
        {
            bool success = true;

            if (titleTextBox.Text == "")
            {
                titleTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                success = false;
            }
            else
            {
                titleTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (locationTextBox.Text == "")
            {
                locationTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                success = false;
            }
            else
            {
                locationTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (categoryComboBox.SelectedValue == null)
            {
                categoryComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                success = false;
            }
            else
            {
                categoryComboBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (descTextBox.Text == "")
            {
                descTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                success = false;
            }
            else
            {
                descTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }

            if (feeTextBox.Text == "")
            {
                feeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                success = false;
            }
            else
            {
                feeTextBox.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            
            if (!success)
            {
                ValidationErrorText.Visibility = Visibility.Visible;
            }

            return success;
        }

        private async void BrowseImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.ViewMode = PickerViewMode.Thumbnail;

            // Set the file extensions
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".bmp");
            picker.FileTypeFilter.Add(".png");

            image = await picker.PickSingleFileAsync();

            if (image != null)
            {
                // Open a stream for the selected file.
                Windows.Storage.Streams.IRandomAccessStream fileStream = await image.OpenAsync(Windows.Storage.FileAccessMode.Read);

                // Set the image source to the selected bitmap.
                Windows.UI.Xaml.Media.Imaging.BitmapImage bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();

                bitmapImage.SetSource(fileStream);
                imagePreview.Source = bitmapImage;
            }
        }

        // Event handler for the update event button
        private void Update_Event(object sender, RoutedEventArgs e)
        {

        }

        // Event handler for the delete event button
        private async void Delete_Event(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Confirm event deletion?");
            
            // Add commands and set their callbacks
            messageDialog.Commands.Add(new UICommand("Yes", (command) =>
            {
                eventTable.DeleteAsync(_event);
                this.Frame.Navigate(typeof(MyEvents));
            }));

            messageDialog.Commands.Add(new UICommand("No", (command) =>
            {
                // Do nothing
            }));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;

            // Show the message dialog
            await messageDialog.ShowAsync();
        }

        // On navigated to event handler
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _event = (Event) e.Parameter;

            int[] startDate = ParseDate(_event.StartDate);
            int[] endDate = ParseDate(_event.EndDate);
            startDatePicker.Date = new DateTimeOffset(new DateTime(startDate[2], startDate[1], startDate[0]));
            endDatePicker.Date = new DateTimeOffset(new DateTime(endDate[2], endDate[1], endDate[0]));

            base.OnNavigatedTo(e);
        }
    }
}
