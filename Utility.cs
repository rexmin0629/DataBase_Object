using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DB
{
    public class Utility
    {
        #region 公有靜態方法
        public static string ServerIp = "";
        public static SqlConnection GetSqlConnection()
        {
            //server=192.168.15.149, 內部
            //server=192.168.4.52, 測試

            SqlConnection conn = new SqlConnection("server="+ServerIp+"; database=InvestmentConsulting;uid=otcuser;pwd=otc2856");
            return conn;
        }
        public static DataTable GetDataTable(string command, SqlConnection conn = null, bool closeAfterExecute = true)
        {
            DataTable resultDataTable = new DataTable();
            if (conn == null) conn = GetSqlConnection();
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlCommand sqlCommand = new SqlCommand(command, conn);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            sqlDataAdapter.Fill(dataSet);
            resultDataTable = dataSet.Tables[0];
            if (conn.State == ConnectionState.Open && closeAfterExecute) conn.Close();
            return resultDataTable;
        }
        public static int ExecuteQuery(string command, SqlConnection conn = null, bool closeAfterExecute = true)
        {
            DataTable resultDataTable = new DataTable();
            if (conn == null) conn = GetSqlConnection();
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlCommand sqlCommand = new SqlCommand(command, conn);
            int result = sqlCommand.ExecuteNonQuery();
            if (conn.State == ConnectionState.Open && closeAfterExecute) conn.Close();
            return result;

        }
        public static int ExecuteScalar(string command, SqlConnection conn = null, bool closeAfterExecute = true)
        {
            if (conn == null) conn = GetSqlConnection();
            if (conn.State != ConnectionState.Open) conn.Open();
            SqlCommand sqlCommand = new SqlCommand(command, conn);
            int count = (Int32)sqlCommand.ExecuteScalar();
            return count;
        }
        public static string ObjectToString(object source)
        {
            string resultString = "";
            if (source is string) resultString = source.ToString();
            else if (source is DateTime) resultString = ((DateTime)source).ToString("yyyy/MM/dd HH:mm:ss");
            else if (source is bool) resultString = ((bool)source == true) ? "1" : "0";
            else if (source != null) resultString = source.ToString();
            return resultString;
        }
        #endregion
    }
}
