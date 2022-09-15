using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
//using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.SqlClient;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
//using System.Windows.Media.Imaging;
//using Windows.UI.Xaml.Media;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class dashboard : Page, INotifyPropertyChanged
    {
        public dashboard()
        {
            this.InitializeComponent();
            if (MainPage.username != null)
            {
                image.Source = MainPage.Image;
                greeting.Text = MainPage.Fname + " " + MainPage.Lname;
                calendar.Date = DateTime.Now;
                Page_Loaded();
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Record> _records;

        public ObservableCollection<Record> Records { get { return _records; } set { _records = value; OnPropertyChanged(nameof(Records)); } }

        

        /*private async void loaddataAsync(string uname) 
        {
            using (SqlConnection sql = new SqlConnection(MainPage.connstring))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.doctor WHERE [id]=@id",sql);
                cmd.Parameters.AddWithValue("@id", uname);
                sql.Open();
                SqlDataReader reader =  cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        fname = reader.GetValue(1).ToString();
                        lname = reader.GetValue(2).ToString();
                        byte[] ava = (byte[])reader.GetValue(6);
                        byte[] bytes;
                        bytes = ava;
                        if (bytes != null)
                        {

                            string b64 = Convert.ToBase64String(ava);

                            BitmapImage image1 = new BitmapImage();
                            
                            image.Source = await bi(bytes,image1);

                        }
                    }
                }
                
                
            }
        }
       */
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

        private async void Page_Loaded()
        {
            await   load();
        }
        public async Task load()
        {

            Records = new ObservableCollection<Record>();
            try
            {
                var records = await GetRecordAsync();
                records.ForEach(r => Records.Add(r));
            }
            catch { }
        }
        private async Task<List<Record>> GetRecordAsync()
        {
            var recordses = new List<Record>();
            using (SqlConnection sql = new SqlConnection(MainPage.connstring))
            {

                await sql.OpenAsync();
                if (sql.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = sql.CreateCommand())
                    {

                        cmd.CommandText = "SELECT* FROM dbo.pacient1 inner join priyom On(dbo.pacient1.name_pacient = dbo.priyom.pacient) WHERE [priyom_data] >= @date and [priyom_data]< @date2 and [doctor] = @id";

                        var dat = calendar.Date.Value.Date.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd.Parameters.AddWithValue("@id", MainPage.username);
                        cmd.Parameters.AddWithValue("@date", dat);
                        DateTime dateTime = calendar.Date.Value.Date;
                        DateTime dateTime1=  dateTime.AddDays(1.0);
                        cmd.Parameters.AddWithValue("@date2", dateTime1.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new Record { Number = reader.GetInt32(4) };
                                if (!await reader.IsDBNullAsync(1)) { record.Name = reader.GetString(1); }

                                if (!await reader.IsDBNullAsync(2))
                                {
                                    record.Fname = reader.GetString(2);
                                }
                                if (!await reader.IsDBNullAsync(7))
                                {
                                    record.Doctor = reader.GetString(7);
                                }
                                if (!await reader.IsDBNullAsync(5))
                                {
                                    record.Date = reader.GetDateTime(5);
                                }
                                if (!await reader.IsDBNullAsync(6))
                                {
                                    record.Diagnos = reader.GetString(6);
                                }
                                if (!await reader.IsDBNullAsync(9)) record.Description = reader.GetString(9);
                                recordses.Add(record);
                            }
                        }

                        //historyGrid.ItemsSource = recordses;
                        //historyGrid.UpdateLayout();
                    }

                }
            }
            return recordses;
        }

        private void calendar_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            Page_Loaded();
        }
        public static Record rec { get; set; }
        private void lview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            rec = (Record)lview.SelectedItem;
            MainPage.frame.Navigate(typeof(pacientVisit));
            

        }

        /*private void calendar_Loaded(object sender, RoutedEventArgs e)
        {
            calendar.Date = DateTime.Now;
            Page_Loaded();
        }*/
    }
}
