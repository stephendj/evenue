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
using Microsoft.WindowsAzure.MobileServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Evenue.ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateEvent : Page
    {
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();

        public CreateEvent()
        {
            this.InitializeComponent();
        }

        private async void InsertEvent(Event _event)
        {
            _event.Id = Guid.NewGuid().ToString();
            await eventTable.InsertAsync(_event);
        }
    }
}
