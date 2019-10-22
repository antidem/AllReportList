using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ReportList.Models;
using System.Web.Http.Description;

namespace ReportList.Controllers
{
    public class RepController : ApiController
    {
        private ReportListDBHandle dbHandle = new ReportListDBHandle();

        // GET: api/Rep
        public List<Report> GetReports()
        {
            return dbHandle.GetReportList();
        }

        // GET: api/Rep/5
        public IHttpActionResult GetReport(int id)
        {
            Report report = dbHandle.FindReport(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }


        // POST: api/Rep
        public IHttpActionResult PostReport(Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbHandle.AddReport(report);

            return CreatedAtRoute("DefaultApi", new { id = report.Id }, report);
        }

        // PUT: api/Rep/5
        public IHttpActionResult PutReport(int id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.Id)
            {
                return BadRequest();
            }

            dbHandle.UpdateReport(report);


            return StatusCode(HttpStatusCode.NoContent);
        }


        public IHttpActionResult DeleteReport(int id)
        {
            Report report = dbHandle.FindReport(id);
            if (report == null)
            {
                return NotFound();
            }

            dbHandle.RemoveReport(id);

            return Ok(report);
        }
    }
}
