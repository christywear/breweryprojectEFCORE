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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using MMABooksUWP.Models;
using MMABooksUWP.Services;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Net.Http;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MMABooksUWP
{
    public sealed partial class MainPage : Page
    {
        Frame frame = Window.Current.Content as Frame;


        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private void ShoppingListBtn_Click(object sender, RoutedEventArgs e)
        {
            if(frame == null)
            {
                frame = new Frame();
            }
            this.frame.Navigate(typeof(ShoppingList));
        }

        private void ProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (frame == null)
            {
                frame = new Frame();
            }
            this.frame.Navigate(typeof(ProductPage));
        }
        

    }
}
