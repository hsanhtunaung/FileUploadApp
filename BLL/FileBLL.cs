using System;
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
    }
}
