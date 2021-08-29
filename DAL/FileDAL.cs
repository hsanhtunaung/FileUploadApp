using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DBUtilities;
using IDAL;
using Model;

namespace DAL
{
    public class FileDAL : IFile
    {

        DBHelper db = new DBUtilities.DBHelper();
        public static string connectionstring;

        public FileDAL()
        {

            connectionstring = AppSettings.DbConnection;
        }
        
    
        public bool Insert(FileModel model)
        {
            try
            {               
                db = new DBUtilities.DBHelper();
                db.AddParaInput("@FileName", model.FileName, SqlDbType.NVarChar);
                db.AddParaInput("@FileType", model.FileType, SqlDbType.NVarChar);
                if (db.Exec("[SP_Insert]") > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }



        public bool InsertRecords(DataTable dt)
        {
            SqlConnection con = new SqlConnection(connectionstring);
            try
            {

                //create object of SqlBulkCopy which help to insert  
                SqlBulkCopy objbulk = new SqlBulkCopy(con);

                //assign Destination table name  
                objbulk.DestinationTableName = "Tbl_Records";
                objbulk.ColumnMappings.Add("TransactionID", "TransactionID");
                objbulk.ColumnMappings.Add("Amount", "Amount");
                objbulk.ColumnMappings.Add("Currency", "Currency");
                objbulk.ColumnMappings.Add("TransactionDate", "TransactionDate");
                objbulk.ColumnMappings.Add("Status", "Status");
                objbulk.ColumnMappings.Add("FileName", "FileName");
                con.Open();
                //insert bulk Records into DataBase.  
                objbulk.WriteToServer(dt);
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
            finally
            {

                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

               

        public List<APIModel> GetByCurrnecy(string currency)
        {
            List<APIModel> lst = new List<APIModel>();
            db = new DBUtilities.DBHelper();
            DataTable dt = new DataTable();
            db.AddParaInput("@Currency", currency, SqlDbType.NVarChar);
            dt = db.GetTable("[SP_GetByCurrency]");
            foreach (DataRow dr in dt.Rows)
            {
                APIModel model = new APIModel();
                model.id = dr["TransactionID"].ToString();
                model.payment = dr["Amount"].ToString() + dr["Currency"].ToString();
                model.Status = dr["Status"].ToString();
                lst.Add(model);
            }
            return lst;
        }


        public List<APIModel> GetByStatus(string status)
        {
            List<APIModel> lst = new List<APIModel>();
            db = new DBUtilities.DBHelper();
            DataTable dt = new DataTable();
            db.AddParaInput("@Status", status, SqlDbType.NVarChar);
            dt = db.GetTable("[SP_GetByStatus]");
            foreach (DataRow dr in dt.Rows)
            {
                APIModel model = new APIModel();
                model.id = dr["TransactionID"].ToString();
                model.payment = dr["Amount"].ToString() + dr["Currency"].ToString();
                model.Status = dr["Status"].ToString();
                lst.Add(model);
            }
            return lst;
        }


        public List<APIModel> GetBydate(string date)
        {
            List<APIModel> lst = new List<APIModel>();
            db = new DBUtilities.DBHelper();
            DataTable dt = new DataTable();
            db.AddParaInput("@Date", date, SqlDbType.NVarChar);
            dt = db.GetTable("SP_GetByDate");
            foreach (DataRow dr in dt.Rows)
            {
                APIModel model = new APIModel();
                model.id = dr["TransactionID"].ToString();
                model.payment = dr["Amount"].ToString() + dr["Currency"].ToString();
                model.Status = dr["Status"].ToString();
                lst.Add(model);
            }
            return lst;
        }
    }
}
