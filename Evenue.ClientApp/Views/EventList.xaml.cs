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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Evenue.ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventList : Page
    {
        private MobileServiceCollection<Event, Event> events;
        private IMobileServiceTable<Event> eventTable =
            App.MobileService.GetTable<Event>();

        public EventList()
        {
            this.InitializeComponent();
            RefreshEventList();
        }

        private async void RefreshEventList()
        {
            events = await eventTable.ToCollectionAsync();

            //// TODO #2: More advanced query that filters out completed items.  
            //items = await todoTable
            //   .Where(todoItem => todoItem.Complete == false)
            //   .ToCollectionAsync();

            eventListGridView.ItemsSource = events;
        }
    }
}
