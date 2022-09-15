using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace App3
{
    public class Doctor
    {
        public string Username { get; set; }
        public string Fname { get; set; }
        public BitmapImage Image { get; set; }
        public string Lname { get; set; }   
        public string Department { get; set; }  
        public string Job { get; set; }

        public Doctor (string username, string fname, string lname, string department, string job, BitmapImage image)
        {
            Username = username;
            Fname = fname;
            Image = image;
            Lname = lname;
            Department = department;
            Job = job;
        }

        public Doctor()
        {
        }
    }
}
