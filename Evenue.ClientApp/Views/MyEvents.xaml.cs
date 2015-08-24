using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Evenue.ClientApp.Models;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Evenue.ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyEvents : Page
    {
        private MobileServiceCollection<Event, Event> events;
        private IMobileServiceTable<Event> eventTable =
            App.MobileService.GetTable<Event>();

        public MyEvents()
        {
            this.InitializeComponent();
            RefreshEventList(null);
        }

        // Refresh the item list each time we navigate to this page
        private async void RefreshEventList(string query)
        {
            if (query == null)
            {
                try
                {
                    events = await eventTable.ToCollectionAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace.ToString());
                    ErrorText.Visibility = Visibility.Visible;
                }
                finally
                {
                    ProgressRing.IsActive = false;
                    SearchBox.Visibility = Visibility.Visible;
                }
                eventListGridView.ItemsSource = events;
            }
            else
            {
                eventListGridView.ItemsSource = GetMatchingEvents(query).ToList<Event>();
            }
        }

        // Navigate to Event information page and send over the Event object
        private void eventListGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(
                 typeof(EventInfo),
                 e.ClickedItem,
                 new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        // returns an ordered list of location that matches the search query
        public IEnumerable<Event> GetMatchingEvents(string query)
        {
            return events
                .Where(c => c.Title.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                            c.Location.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                            c.Category.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.Title.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.Location.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.Category.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

        // Event handler when user is typing a search query, show a list of suggestions
        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //We only want to get results when it was a user typing
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matchingEvents = GetMatchingEvents(sender.Text);

                RefreshEventList(sender.Text);
                sender.ItemsSource = matchingEvents.ToList();
            }
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as Event).ToString();
        }

        // Remove the ability of the back button to go back to login page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                PageStackEntry lastPage = Frame.BackStack[Frame.BackStackDepth - 1];
                if (lastPage.SourcePageType == typeof(LoginPage))
                {
                    Frame.BackStack.Remove(lastPage);
                }
            }
        }
    }
}
