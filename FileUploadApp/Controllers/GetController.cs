using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileUploadApp.Controllers
{    
    [ApiController]
    public class GetController : ControllerBase
    {
        FileBLL filebll = new FileBLL();     


        // GET api/Get/GetByCurrency?currency=USD
        [Route("api/Get/GetByCurrency")]
        [HttpGet]
        public string GetByCurrency(string currency)
        {
            List<APIModel> lst = new List<APIModel>();
            lst = filebll.GetByCurrnecy(currency);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;           
        }
     

        // GET api/Get/GetByStatus?Status=Approved
        [Route("api/Get/GetByStatus")]
        public string GetByStatus(string status)
        {
            List<APIModel> lst = new List<APIModel>();
            lst = filebll.GetByStatus(status);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;
        }

        // GET api/Get/GetBydate?date=2019-01-23T13:45:10
        [Route("api/Get/GetBydate")]
        public string GetBydate(string date)
        {
            List<APIModel> lst = new List<APIModel>();
            lst = filebll.GetBydate(date);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;
        }

    }
}
