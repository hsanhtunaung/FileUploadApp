using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace IDAL
{
    public interface IFile
    {
        bool Insert(FileModel model);

        bool InsertRecords(DataTable dt);       

        List<APIModel> GetByCurrnecy(string currency);

        List<APIModel> GetByStatus(string status);

        List<APIModel> GetBydate(string date);
    }
}
