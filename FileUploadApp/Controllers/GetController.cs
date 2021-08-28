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
    //[Route("api/[controller]")]
    [ApiController]
    public class GetController : ControllerBase
    {
        FileBLL filebll = new FileBLL();
        // GET: api/<GetController>
        //[Route("api/GET")]
        [Route("api/Get/names")]
        [HttpGet]
        public string Get()
        {
            List<APIModel> lst = new List<APIModel>();
            lst = filebll.GetALL();           
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;
         
        }


        [Route("api/Get/currency")]
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
            lst = filebll.GetByCurrnecy(status);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;
        }

        [Route("api/Get/date")]
        public string GetBydate(string date)
        {
            List<APIModel> lst = new List<APIModel>();
            lst = filebll.GetByCurrnecy(date);
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            return output;
        }

    }
}
