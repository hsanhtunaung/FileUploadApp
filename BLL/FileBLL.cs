using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using IDAL;
using Model;

namespace BLL
{
    public class FileBLL
    {
        IFile ifile = new FileDAL();

        public bool Insert(FileModel model)
        {
            return ifile.Insert(model);
        }


        public bool InsertRecords(DataTable dt)
        {
            return ifile.InsertRecords(dt);
        }

       
        public List<APIModel> GetByCurrnecy(string currency)
        {
            return ifile.GetByCurrnecy(currency);
        }

        public List<APIModel> GetByStatus(string status)
        {
            return ifile.GetByStatus(status);
        }


        public List<APIModel> GetBydate(string date)
        {
            return ifile.GetBydate(date);
        }

    }
}
