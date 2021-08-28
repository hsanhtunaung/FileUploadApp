using System;
using IDAL;
using Model;

namespace DAL
{
    public class FileDAL:IFile
    {

        public bool Insert(FileModel model)
        {
            FileRecordModel filerecord = new FileRecordModel();
            filerecord.fileModel.FileName = "";
            return true;
        }
    }
}
