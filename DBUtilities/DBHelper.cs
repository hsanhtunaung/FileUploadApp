using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtilities
{
    public class DBHelper
    {
        private static string constring;

        private List<SqlParameter> args = new List<SqlParameter>();

        public void AddParaInput(string parametername, object obj, SqlDbType dbtype)
        {
            SqlParameter arg = new SqlParameter();
            arg.ParameterName = parametername;
            arg.SqlDbType = dbtype;
            arg.SqlValue = obj;
            this.args.Add(arg);
            arg.Direction = ParameterDirection.Input;
        }

        public int Exec(string procedure)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand com = new SqlCommand();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            com.CommandText = procedure;
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = con;
            try
            {

                if (this.args.Count() > 0) com.Parameters.AddRange(args.ToArray());
                int result;
                result = com.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    }
}
