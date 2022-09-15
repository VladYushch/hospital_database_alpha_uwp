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
using System.Windows;
using System.Data.SqlClient;
//using Xceed.Wpf.Toolkit;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using System.Drawing;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Settings : Page
    {
        Doctor doctor = new Doctor();
        public Settings()
        {
            this.InitializeComponent();
            inmode();
        }
        private const string connString = "SERVER = DESKTOP-JKE55OM; DATABASE = hospital; USER ID = rootadmin; PASSWORD = Zpxoci911 ";
        private string usernametext = string.Empty;
        private string passwordtext = string.Empty;

        private void inmode() {
            if (MainPage.username != null)
            {
                logingrid.Visibility = Visibility.Collapsed;
                userset.Visibility = Visibility.Visible;
                loginbtn.Visibility = Visibility.Collapsed;
                logoutbtn.IsEnabled = true;
            }
            else
            {
                loginbtn.Visibility = Visibility.Visible;
                logingrid.Visibility = Visibility.Visible;
                userset.Visibility = Visibility.Collapsed;
                logoutbtn.IsEnabled = false;
            }
            
        }
        private async void loginbtn_Click(object sender, RoutedEventArgs e)
        {

            if (login.Text != string.Empty)
            {
                usernametext = login.Text;
            }
            else { var d = new MessageDialog("Введіть логін користувача"); await d.ShowAsync(); return; }
            if (passwordBox.Password.ToString() != string.Empty)
            {
                passwordtext = passwordBox.Password.ToString();
            }
            else { await new MessageDialog("Введіть пароль користувача").ShowAsync(); return; }


            using (SqlConnection connection = new SqlConnection(connectionString: "Data Source=localhost;Initial Catalog=hospital;Integrated Security=True")) { 
                
                connection.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.doctor WHERE [id]=@id and[password]=@pas", connection);
                    cmd.Parameters.AddWithValue("id", usernametext);
                    cmd.Parameters.AddWithValue("@pas", passwordtext);
                    SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MainPage.username = reader.GetValue(0).ToString();
                        MainPage.Fname = reader.GetValue(1).ToString();
                        MainPage.Lname= reader.GetValue(2).ToString();
                        MainPage.Department= reader.GetValue(3).ToString();
                        MainPage.Job = reader.GetValue(4).ToString();
                        byte[] ava = (byte[])reader.GetValue(6);

                        BitmapImage image1 = new BitmapImage();
                        if (ava != null)
                        {

                            string b64 = Convert.ToBase64String(ava);


                            image1 = await bi(ava, image1);

                        }
                        MainPage.Image = image1;
                        inmode();
                    }
                }
                else
                {
                    await new MessageDialog("неправильний логін або пароль").ShowAsync();

                    passwordBox.Password = String.Empty;
                }
                
            }
        }
        private async Task<BitmapImage> bi(byte[] avatar, BitmapImage image1)
        {
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(avatar.AsBuffer());
                stream.Seek(0);
                image1.SetSource(stream);
            }
            return image1;
        }
        private void logoutbtn_Click(object sender, RoutedEventArgs e)
        {
            MainPage.username = null;
            inmode();
        }

        private async void iconbtn_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                var imagepath = file.Path;

                using (SqlConnection con = new SqlConnection(connectionString: "Data Source=localhost;Initial Catalog=hospital;Integrated Security=True"))
                {
                    con.Open();
                    using (SqlCommand com = new SqlCommand("UPDATE dbo.doctor SET [avatar] = @IM WHERE [id] = @uname", con))
                    {
                        string result = string.Empty;
                        byte[] array;
                        using (var stream = await file.OpenStreamForReadAsync())
                        {
                            using (var memory = new MemoryStream())
                            {
                                await stream.CopyToAsync(memory);
                                array = memory.ToArray();
                                result = Convert.ToBase64String(array);
                            }
                        }

                        com.Parameters.AddWithValue("@IM", array);
                        com.Parameters.AddWithValue("@uname", MainPage.username);
                        com.ExecuteNonQuery();
                    }
                }
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

    }
}