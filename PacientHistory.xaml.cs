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
using System.Data.SqlClient;
using System.Data;
using Windows.UI.Popups;
using System.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PacientHistory : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Record> _records;

        public ObservableCollection<Record> Records { get { return _records; } set { _records = value; OnPropertyChanged(nameof(Records)); } }
        public PacientHistory()
        {
            this.InitializeComponent();
        }
       
     
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            Records = new ObservableCollection<Record>();
            try
            {
                var records = await GetRecordAsync();
                records.ForEach(r=> Records.Add(r));
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
                        string subId= string.Empty;
                        using (SqlCommand cmd1 = sql.CreateCommand())
                        {
                            cmd1.CommandText = "SELECT id_pacient FROM dbo.pacient1 WHERE [name_pacient]=@fname and [lastname_pacient]=@lname";
                            cmd1.Parameters.AddWithValue("@fname", textBox1.Text);
                            cmd1.Parameters.AddWithValue("@lname", textBox.Text);
                            using (SqlDataReader reader = cmd1.ExecuteReader())
                            {
                                while (await reader.ReadAsync())
                                {
                                    if (!await reader.IsDBNullAsync(0)) subId = reader.GetValue(0).ToString();
                                }
                            }
                        }

                        cmd.CommandText = "SELECT* FROM dbo.pacient1 inner join priyom On(dbo.pacient1.id_pacient = dbo.priyom.pacient) WHERE [id_pacient]=@id";
                        cmd.Parameters.AddWithValue("@id", subId);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new Record { Number = reader.GetInt32(7) };
                                if(!await reader.IsDBNullAsync(1)) { record.Name = reader.GetString(1); }

                                if (!await reader.IsDBNullAsync(2))
                                {
                                    record.Fname = reader.GetString(2);
                                }
                                if (!await reader.IsDBNullAsync(10))
                                {
                                    record.Doctor = reader.GetString(10);
                                }
                                if (!await reader.IsDBNullAsync(8))
                                {
                                    record.Date = reader.GetDateTime(8);
                                }
                                if (!await reader.IsDBNullAsync(9))
                                {
                                    record.Diagnos = reader.GetString(9);
                                }
                                if (!await reader.IsDBNullAsync(12)) record.Description = reader.GetString(12);
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

    }
        
}

