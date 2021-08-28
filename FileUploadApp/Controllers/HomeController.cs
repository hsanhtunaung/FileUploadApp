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

namespace FileUploadApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private FileBLL bll = new FileBLL();
        string id;
        string currency;
        string transactiondate;
        string amount;
        string status;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            string ErrorMessage;
            FileModel filemodel = new FileModel();
            if (file == null || file.Length == 0)
                return Content("file not selected");

            // Extract file name from whatever was posted by browser
            var fileName = System.IO.Path.GetFileName(file.FileName);
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists) Directory.CreateDirectory(basePath);
            var fileNamess = Path.GetFileNameWithoutExtension(file.FileName);
            var filePath = Path.Combine(basePath, file.FileName);
            var extension = Path.GetExtension(file.FileName);
            if (!System.IO.File.Exists(filePath))
            {
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            DataTable tbl = new DataTable();
            //tbl.Columns.Add(new DataColumn("ID", typeof(Int32)));
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

            if (!supportedTypes.Contains(fileExt))
            {
                ErrorMessage = "File Extension Is InValid - Only Upload CSV/XML File";                
            }
          else  if (fileExt == "xml")
            {
                //create XMLDocument object
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(@"E:\files\xmlnames.xml");
                xmlDoc.Load(filePath);
                //XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Transactions/Transaction");
                XmlNodeList nodeList = xmlDoc.SelectNodes("/Transactions/Transaction");
                //loop through each node and save it value in NodeStr
              
            
                
                foreach (XmlNode node in nodeList)
                {
                    //id = node.SelectSingleNode("id").InnerText;
                    id = node["id"].InnerText;
                    transactiondate = node["TransactionDate"].InnerText;
                    
                    XmlNodeList nodeLists = xmlDoc.SelectNodes("/Transactions/Transaction/PaymentDetails");
                    foreach(XmlNode nodes in nodeLists)
                    {
                        amount = nodes["Amount"].InnerText;
                        currency = nodes["CurrencyCode"].InnerText;
                    }
                    status = node["Status"].InnerText;
                    DataRow dr = tbl.NewRow();
                    dr["TransactionID"] =id;
                    dr["Amount"] = amount;
                    dr["Currency"] = currency;
                    dr["TransactionDate"] = transactiondate;
                    dr["Status"] = status;
                    dr["FileName"] = fileName;                   
                    tbl.Rows.Add(dr);
                    
                    //To check if any record is valid/invalid
                    

                }
                if (bll.Insert(filemodel)==true && bll.InsertRecords(tbl) == true)
                {
                    ErrorMessage = "Success";
                }
            }
           else
            { 
                string[] lines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] columns = line.Split(',');
                    //string address = data.Replace(" ", "");
                    id = columns[0].Trim().ToString();
                   string rowString = id.Replace('"', ' ').Trim();
                    amount =Convert.ToString(columns[1]);
                    currency = columns[2];
                    transactiondate = columns[3].ToString();
                    status = columns[4];

                    DataRow dr = tbl.NewRow();
                    dr["TransactionID"] = id;
                    dr["Amount"] = Convert.ToDecimal(amount);
                    dr["Currency"] = currency;
                    dr["TransactionDate"] = transactiondate;
                    dr["Status"] = status;
                    dr["FileName"] = fileName;
                    tbl.Rows.Add(dr);
                    
                }

                if (bll.Insert(filemodel) == true && bll.InsertRecords(tbl) == true)
                {
                    ErrorMessage = "Success";
                }
            }

            return RedirectToAction("Files");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
