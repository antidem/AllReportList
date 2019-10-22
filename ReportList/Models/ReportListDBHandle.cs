using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace ReportList.Models
{
    public class ReportListDBHandle
    {
        private MySqlConnection con;
        private void Connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["LocalMySqlServer"].ToString();
            con = new MySqlConnection(constring);
        }
        public void AddReport(Report report)
        {
            Connection();
            using (MySqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format("INSERT ReportList (Name, Author, About, Year) VALUES ('{0}','{1}','{2}','{3}')", report.Name, report.Author, report.About, report.Year);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<Report> GetReportList()
        {
            List<Report> reportlist = new List<Report>();

            Connection();
            using (MySqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM ReportList";
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    reportlist.Add(
                        new Report
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Author = Convert.ToString(dr["Author"]),
                            About = Convert.ToString(dr["About"]),
                            Year = Convert.ToInt32(dr["Year"])
                        });
                }
                return reportlist;
            }
        }

        public Report FindReport(int id)
        {
            Report myreportlist = new Report();
            Connection();
            using (MySqlCommand cmd = con.CreateCommand())
            {

                cmd.CommandText = string.Format("SELECT * FROM ReportList WHERE id = {0}", id);
                con.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    myreportlist = new Report
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Author = reader.GetString(2),
                        About = reader.GetString(3),
                        Year = reader.GetInt32(4)
                    };
                }
                con.Close();
                return myreportlist;

            }
        }

        public void UpdateReport(Report report)
        {
            Connection();
            using (MySqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format("UPDATE ReportList SET Name = '{0}', Author = '{1}', About = '{2}', Year = {3} WHERE Id = {4}", report.Name, report.Author, report.About, report.Year, report.Id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void RemoveReport(int id)
        {
            Connection();
            using (MySqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format("DELETE FROM ReportList WHERE Id = {0}", id);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}