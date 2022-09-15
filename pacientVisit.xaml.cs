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
using Windows.UI.Text;
using Windows.UI;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace App3
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class pacientVisit : Page
    {
        public pacientVisit()
        {
            this.InitializeComponent();
            
        }
        Record rec = dashboard.rec;
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using(SqlConnection sql =  new SqlConnection(connectionString: MainPage.connstring))
            {
                string value = string.Empty;
               editor.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out value);
                sql.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE dbo.priyom SET [record_full] = @text WHERE [priyom_number] = @pac", sql))
                {
                    cmd.Parameters.AddWithValue("@text", value);
                    cmd.Parameters.AddWithValue("@pac", rec.Number);
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
            editor.Document.Selection.CharacterFormat.ForegroundColor = Color.FromArgb(0,0,0,255);
        }
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

    }
}
