using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace App3.imageclass
{
    public class ImageModel
    {
        private byte[] _Image;
        public byte[] Image
        {
            get { return _Image; }
            set { _Image = value; }
        }
        public ImageModel()
        {
            Image = loaddata();
        }
        private byte[] loaddata()
        {
            SqlConnection connection = new SqlConnection(@"Data Source=(LOCALDB)\MSSQLLOCALDB;Initial Catalog=Testdb;Integrated Security=True");
            string sql = "SELECT Images FROM Test9 WHERE id = @ID";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@ID", 1);
            connection.Open();
            object theImg = cmd.ExecuteScalar();
            try
            {
                return (byte[])theImg;
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
