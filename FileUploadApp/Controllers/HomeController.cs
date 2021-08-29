using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileUploadApp.Models;
using Model;
using System.Xml;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data;
using BLL;
using Microsoft.Extensions.Configuration;

namespace FileUploadApp.Controllers
{
    public class HomeController : Controller
    {
        #region Declaration
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private FileBLL bll = new FileBLL();
        string id;
        string currency;
        string transactiondate;
        string amount;
        string status;
        bool result;
        public static string sqlConnectionstring;
        string logfileExt;
        #endregion

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            sqlConnectionstring = _configuration.GetValue<string>("ConnectionStrings:FileUploadDBString");
            AppSettings.DbConnection = sqlConnectionstring;           
            logfileExt= _configuration.GetValue<string>("Logformats:_log");
        }


        public IActionResult Index()
        {
          
            return View();
        }

        public IActionResult ErrorPage()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                FileModel filemodel = new FileModel();
                if (file == null || file.Length == 0)
                    return Content("file not selected");

                DateTime datetNow = DateTime.Now;
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.ffffff");
                string FileDateTime = datetNow.ToString("yyyyMMdd_HHmmss");


                // Extract file name from whatever was posted by browser
                var fileName = System.IO.Path.GetFileName(file.FileName);
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                var LogfilePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Logs\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileNamess = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);

                string LogFileName = LogfilePath + FileDateTime + logfileExt;

                if (System.IO.File.Exists(filePath))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                }
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                DataTable tbl = new DataTable();
                tbl.Columns.Add(new DataColumn("TransactionID", typeof(string)));
                tbl.Columns.Add(new DataColumn("Amount", typeof(decimal)));
                tbl.Columns.Add(new DataColumn("Currency", typeof(string)));
                tbl.Columns.Add(new DataColumn("TransactionDate", typeof(string)));
                tbl.Columns.Add(new DataColumn("Status", typeof(string)));
                tbl.Columns.Add(new DataColumn("FileName", typeof(string)));

                var supportedTypes = new[] { "csv", "xml" };

                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                filemodel.FileName = fileName;
                filemodel.FileType = fileExt;

                //Create log for invaid records
                StreamWriter LogWriter = System.IO.File.CreateText(LogFileName);
                LogWriter.WriteLine("[" + datetime + "]" + "########===== Invalid Records " + " ======#########");

                if (!supportedTypes.Contains(fileExt))
                {
                    return RedirectToAction("Error");
                }
                else if (file.Length >1000000)
                {
                    return BadRequest("File Size should not be greater than 1MB.");
                }
                else if (fileExt == "xml")
                {
                    //create XMLDocument object
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(filePath);
                    XmlNodeList nodeList = xmlDoc.SelectNodes("/Transactions/Transaction");

                    foreach (XmlNode node in nodeList)
                    {
                        id = node["id"].InnerText;
                        transactiondate = node["TransactionDate"].InnerText;
                        XmlNodeList nodeLists = xmlDoc.SelectNodes("/Transactions/Transaction/PaymentDetails");
                        foreach (XmlNode nodes in nodeLists)
                        {
                            amount = nodes["Amount"].InnerText;
                            currency = nodes["CurrencyCode"].InnerText;
                        }
                        status = node["Status"].InnerText;
                        DataRow dr = tbl.NewRow();

                        result = id == null || id == string.Empty || amount == null || amount == string.Empty || transactiondate == null || transactiondate == string.Empty || status == null || status == string.Empty;

                        if (result != true)
                        {
                            dr["TransactionID"] = id.Trim().ToString();
                            dr["Amount"] = amount;
                            dr["Currency"] = currency.Trim().ToString();
                            dr["TransactionDate"] = transactiondate;
                            dr["Status"] = status.Trim().ToString();
                            dr["FileName"] = fileName;
                            tbl.Rows.Add(dr);

                        }
                        else
                        {
                            // Create log file for invalid records
                            LogWriter.WriteLine("fileName: " + fileName + ",TransactionID:" + id + ",Amount : " + amount + " ,Currency : " + currency + ",TransactionDate: " + transactiondate + ",Status: " + status);

                        }

                    }
                    if (bll.Insert(filemodel) == true && bll.InsertRecords(tbl) == true)
                    {
                        LogWriter.Close();
                        return this.Content(Microsoft.AspNetCore.Http.StatusCodes.Status200OK.ToString());
                    }           

                }
                else
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        string[] columns = line.Split(',');
                        id = columns[0].ToString();
                        amount = Convert.ToString(columns[1]);
                        currency = columns[2];
                        transactiondate = columns[3].ToString();
                        status = columns[4];

                        result = id == null || id == string.Empty || amount == null || amount == string.Empty || transactiondate == null || transactiondate == string.Empty || status == null || status == string.Empty;
                        if (result != true)
                        {
                            DataRow dr = tbl.NewRow();
                            dr["TransactionID"] = id;
                            dr["Amount"] = Convert.ToDecimal(amount);
                            dr["Currency"] = currency.Trim().ToString();
                            dr["TransactionDate"] = transactiondate;
                            dr["Status"] = status.Trim().ToString();
                            dr["FileName"] = fileName;
                            tbl.Rows.Add(dr);
                        }
                        else
                        {
                            // Create log file for invalid records
                            LogWriter.WriteLine("fileName: " + fileName + ",TransactionID:" + id + ",Amount : " + amount + " ,Currency : " + currency+ ",TransactionDate: " + transactiondate + ",Status: " + status);

                        }

                    }

                    if (bll.Insert(filemodel) == true && bll.InsertRecords(tbl) == true)
                    {
                        LogWriter.Close();
                        return this.Content(Microsoft.AspNetCore.Http.StatusCodes.Status200OK.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("File Upload Fail");
            }
            return this.Content(Microsoft.AspNetCore.Http.StatusCodes.Status200OK.ToString());
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
