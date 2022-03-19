using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nordic.Bal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nordic.Web.Controllers
{
    public class UploadCsvController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private string uploadsFolder;
        private string shippingTrackFilesDir;

        public UploadCsvController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
            this.uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            this.shippingTrackFilesDir = Path.Combine(_hostingEnvironment.WebRootPath, "shipping_track_files");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null)
            {
                try
                {
                    string fileName = SaveUploadedImage(file);
                    if (fileName != null)
                    {
                        ImportCSV importCsv = new ImportCSV();
                        DataTable dt = ReadCsv(Path.Combine(uploadsFolder, fileName));
                        importCsv.BulkInsert(dt);
                        return Redirect("/UploadCsv?status=201");
                    }
                }
                catch(Exception ex)
                {
                    ExceptionLogging.SaveErrorToText(ex);
                    return Redirect("/UploadCsv?status=500");
                }
            }
            return Redirect("/UploadCsv?status=500");
        }

        private string SaveUploadedImage(IFormFile formFile)
        {
            string fileName = null;

            if (formFile != null)
            {
                if(!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                fileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                using (var fileStream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return fileName;
        }


        private DataTable ReadCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add(new DataColumn("RID", typeof(string)));
                tbl.Columns.Add(new DataColumn("TrackingNo", typeof(string)));
                tbl.Columns.Add(new DataColumn("isRead", typeof(string)));
                tbl.Columns.Add(new DataColumn("isMailSent", typeof(bool)));
                tbl.Columns.Add(new DataColumn("IsActive", typeof(bool)));
                tbl.Columns.Add(new DataColumn("ShipVia", typeof(string)));
                tbl.Columns.Add(new DataColumn("CreatedDate", typeof(DateTimeOffset)));
                tbl.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTimeOffset)));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    
                    DataRow dr = tbl.NewRow();
                    dr["RID"] = values[0].Replace("\"", "").Trim();
                    dr["TrackingNo"] = values[1].Replace("\"", "").Trim();
                    dr["isRead"] = values[2].Replace("\"", "").Trim();
                    dr["isMailSent"] = 0;
                    dr["IsActive"] = 1;
                    dr["ShipVia"] = filePath.Contains("fedexport") ? "Fedex" : "UPS";
                    dr["CreatedDate"] = DateTimeOffset.Now;
                    dr["ModifiedDate"] = DateTimeOffset.Now;
                    tbl.Rows.Add(dr);
                }
                return tbl;
            }
        }

        public void BulkInsert(DataTable dt)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "tbl_shippingtrack";

            objbulk.ColumnMappings.Add("RID", "RID");
            objbulk.ColumnMappings.Add("TrackingNo", "TrackingNo");
            objbulk.ColumnMappings.Add("isRead", "isRead");
            objbulk.ColumnMappings.Add("isMailSent", "isMailSent");
            objbulk.ColumnMappings.Add("IsActive", "IsActive");
            objbulk.ColumnMappings.Add("ShipVia", "ShipVia");
            objbulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            objbulk.ColumnMappings.Add("ModifiedDate", "ModifiedDate");

            con.Open();
            objbulk.WriteToServer(dt);
            con.Close();
        }
    }
}
