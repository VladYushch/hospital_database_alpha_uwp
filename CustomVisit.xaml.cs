using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CustomVisit : Page
    {
        public CustomVisit()
        {
            this.InitializeComponent();
            
        }
        Record rec = new Record();
        Pacient pacient = new Pacient();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sql = new SqlConnection(connectionString: MainPage.connstring))
            {
                string value = string.Empty;
                editor.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out value);
                sql.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.priyom (priyom_data,diagnos,doctor,pacient,record_full) VALUES (@data,@diag,@doc,@id,@rec)", sql))
                {
                    string doc = MainPage.Fname + " " + MainPage.Lname;
                    cmd.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@diag", "plaaceholer");
                    cmd.Parameters.AddWithValue("@doc", doc);
                    cmd.Parameters.AddWithValue("@rec", value);
                    cmd.Parameters.AddWithValue("@id", pacient.ID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            editor.Document.GetText(TextGetOptions.UseCrlf, out string currentRawText);

            // reset colors to correct defaults for Focused state
            ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush background = (SolidColorBrush)App.Current.Resources["TextControlBackgroundFocused"];

            if (background != null)
            {
                documentRange.CharacterFormat.BackgroundColor = background.Color;
            }
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.ForegroundColor = Color.FromArgb(0, 0, 0, 255);
        }
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

        private void findPacient_Click(object sender, RoutedEventArgs e)
        {
            using ( SqlConnection sql = new SqlConnection(connectionString: MainPage.connstring))
            {
                sql.Open();
                using (SqlCommand cmd1 = new SqlCommand("Select * From dbo.pacient1 Where [name_pacient]=@fname and [lastname_pacient]=@lname and [midle_name]=@mname", sql))
                {
                    cmd1.Parameters.AddWithValue("@fname", Fname.Text);
                    cmd1.Parameters.AddWithValue("@lname",Lname.Text);
                    cmd1.Parameters.AddWithValue("@mname", Mname.Text);
                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                pacient.ID = (int)reader.GetValue(0);
                                pacient.FirstName = reader.GetValue(1).ToString();
                                pacient.LastName = reader.GetValue(2).ToString();
                                pacient.MiddleName = reader.GetValue(3).ToString();
                                pacient.birtdate = (DateTime)reader.GetValue(4);
                                if(!reader.IsDBNull(5)) pacient.phone = (string)reader.GetValue(5);
                                if (!reader.IsDBNull(6)) pacient.city = (string)reader.GetValue(6);
                            }
                        }
                    }
                }
            }
        }
    }
}
