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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Evenue.ClientApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        // Check if both username and password fields are not empty
        private bool ValidateLoginField(string username, string password)
        {
            bool valid = true;

            if (username == "")
            {
                username_error.Visibility = Visibility.Visible;
                valid = false;
            }
            else
            {
                username_error.Visibility = Visibility.Collapsed;
            }

            if(password == "")
            {
                password_error.Visibility = Visibility.Visible;
                valid = false;
            }
            else
            {
                password_error.Visibility = Visibility.Collapsed;
            }

            return valid;
        }

        private async void Login(string username, string password)
        {
            // Insert login process here
        }

        // Event handler for login button, try to login when clicked
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Visibility = Visibility.Collapsed;
            LoginProgressRing.IsActive = true;

            if(ValidateLoginField(username.Text, password.Text))
            {
                
                /*                              *
                 *      Try to login here       *
                 *                              */

                AppShell shell = Window.Current.Content as AppShell;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (shell == null)
                {
                    // Create a AppShell to act as the navigation context and navigate to the first page
                    shell = new AppShell();

                    // Set the default language
                    shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                }

                // Place our app shell in the current Window
                Window.Current.Content = shell;

                if (shell.AppFrame.Content == null)
                {
                    // When the navigation stack isn't restored, navigate to the first page
                    // suppressing the initial entrance animation.
                    shell.AppFrame.Navigate(typeof(EventList), null, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
                }

                Window.Current.Activate();
            }
            else
            {
                LoginProgressRing.IsActive = false;
                (sender as Button).Visibility = Visibility.Visible;
            }
        }
    }
}
