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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        public static string username { get; set; }
        public static string connstring = "Data Source=localhost;Initial Catalog=hospital;Integrated Security=True";
        public static string Fname { get; set; }
        public static BitmapImage Image { get; set; }
        public static string Lname { get; set; }
        public static string Department { get; set; }
        public static string Job { get; set; }

        public static Frame frame { get; set; }
        private void navView_Loaded(object sender, RoutedEventArgs e)
        {
            frame = ContentFrame;
            ContentFrame.Navigate(typeof(dashboard));
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(Settings));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag.ToString())
                {
                    case "dashboard":
                        ContentFrame.Navigate(typeof(dashboard));
                        break;
                    case "pacientVisit":
                        ContentFrame.Navigate(typeof(CustomVisit));
                        break;
                    case "PacientHistory":
                        ContentFrame.Navigate(typeof(PacientHistory));
                        break;
                    case "ContactList":
                        ContentFrame.Navigate(typeof(ContactList));
                        break;
                }
            }
        }
    }
    
}
