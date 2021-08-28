using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class FileRecordModel
    {
        public string ID { get; set; }
        public string TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string TransactionDate { get; set; }

        public string Payment { get; set; }
        public FileModel fileModel { get; set; }
        public FileRecordModel()
        {
            fileModel = new FileModel();

        }
    }
}
