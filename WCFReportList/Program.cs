using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using WCFReportList.Models;

namespace WCFReportList
{
    [ServiceContract]
    public interface IReportListService
    {
        [OperationContract]
        List<Report> GetReports();
        
        [OperationContract]
        Report GetReport(int id);

        [OperationContract]
        string PostReport(Report report);

        [OperationContract]
        string PutReport(int id, Report report);

        [OperationContract]
        string DeleteReport(int id);
    }

    public class ReportListService : IReportListService
    {
        private ReportListDBHandle dbHandle = new ReportListDBHandle();
        public List<Report> GetReports()
        {
            return dbHandle.GetReportList();
        }

        public Report GetReport(int id)
        {
            Report report = dbHandle.FindReport(id);
            return report;
        }
        public string PostReport(Report report)
        {
            dbHandle.AddReport(report);
            return "Ok";
        }
        public string PutReport(int id, Report report)
        {
            dbHandle.UpdateReport(report);
            return id.ToString();
        }

        public string DeleteReport(int id)
        {
            Report report = dbHandle.FindReport(id);
            dbHandle.RemoveReport(id);
            return report.Id.ToString();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ReportListService), new Uri("http://localhost:8000"));
            host.AddServiceEndpoint(typeof(IReportListService), new BasicHttpBinding(), "Soap");

            try
            {
                host.Open();

                using (ChannelFactory<IReportListService> scf = new ChannelFactory<IReportListService>(new BasicHttpBinding(), "http://localhost:8000/Soap"))
                {
                    IReportListService channel = scf.CreateChannel();
                    
                    // GetReports
                    Console.WriteLine("Calling GetReports on SOAP endpoint: ");
                    channel.GetReports();
                    Console.WriteLine("");

                    // GetReport
                    Console.WriteLine("Calling GetReport on SOAP endpoint: ");
                    channel.GetReport(3);
                    Console.WriteLine("");

                    // PostReport
                    Console.WriteLine("Calling PostReport on SOAP endpoint: ");
                    channel.PostReport(new Report { Name = "report8755", Author = "Gordon Freeman", About = "BlackMesa unit", Year = 1996 });
                    Console.WriteLine("");

                    // PutReport
                    Console.WriteLine("Calling PutReport on SOAP endpoint: ");
                    channel.PutReport(6, new Report { Id = 5, Name = "report8755", Author = "Gordon Freeman3", About = "BlackMesa unit", Year = 1996 });
                    Console.WriteLine("");

                    // DeleteReport
                    Console.WriteLine("Calling DeleteReport on SOAP endpoint: ");
                    channel.DeleteReport(6);
                    Console.WriteLine("");
                }


                Console.WriteLine("Press [Enter] to terminate");
                Console.ReadLine();
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
        }
    }
}
