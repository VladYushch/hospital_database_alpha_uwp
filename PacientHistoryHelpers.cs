using System.Collections.Generic;
using System.Data.SqlClient;

namespace App3
{
    internal static class PacientHistoryHelpers
    {
        private static List<Record> Records()
        {
            using (SqlConnection sql = new SqlConnection(MainPage.connstring))
            {

                sql.Open();
                if (sql.State == System.Data.ConnectionState.Open)
                {
                    using (SqlCommand cmd = sql.CreateCommand())
                    {
                        cmd.CommandText = "SELECT* FROM dbo.pacient1 inner join priyom On(dbo.pacient1.name_pacient = dbo.priyom.pacient) WHERE[name_pacient]=@fname and [lastname_pacient]=@lname";
                        cmd.Parameters.AddWithValue("@fname", text);
                        cmd.Parameters.AddWithValue("@lname", text1);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var record = new Record();
                                record.number = reader.GetInt32(4);
                                record.name = reader.GetString(1);
                                record.fname = reader.GetString(2);
                                record.doctor = reader.GetString(7);
                                record.date = reader.GetDateTime(5);
                                record.diagnos = reader.GetString(6);
                                Records.Add(record);
                            }
                        }
                    }
                }
            }
        }
    }
}