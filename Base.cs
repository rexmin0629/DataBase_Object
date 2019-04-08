using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DB
{
    abstract public class Base
    {
        #region 建構子
        public Base()
        {
        }
        public Base(DataRow dr)
        {
        }
        #endregion

        #region 公有屬性(virtual)
        public virtual string DB_NAME
        {
            get { throw new NotImplementedException(); }
        }
        public virtual string[] Columns
        {
            get { throw new NotImplementedException(); }
        }
        public virtual object[] Values
        {
            get { throw new NotImplementedException(); }
        }
        public string[] StringValues
        {
            get
            {
                List<string> stringValueList = new List<string>();
                foreach (object obj in Values)
                {
                    stringValueList.Add(Utility.ObjectToString(obj));
                }
                return stringValueList.ToArray();
            }
        }
        public virtual int UPDATE_INDEX
        {
            get { return 0; }
        }
        #endregion

        #region 公有方法
        public bool AddToDB()
        {
            string command = "insert INTO " + DB_NAME + " (" + string.Join(",", Columns) + ") VALUES ('" + string.Join("','", StringValues) + "')";
            int count = DB.Utility.ExecuteQuery(command);
            if (count == 0) return false;
            return true;
        }
        public bool UpdateToDB()
        {
            List<string> conditionList = new List<string>();
            for (int i = 0; i < Columns.Length; i++)
            {
                if (i == UPDATE_INDEX) continue;
                conditionList.Add(Columns[i] + "='" + StringValues[i] + "'");
            }
            string command = "update " + DB_NAME + " SET " + string.Join(",", conditionList) + " WHERE " + Columns[UPDATE_INDEX] + "='" + StringValues[UPDATE_INDEX] + "'";
            int count = DB.Utility.ExecuteQuery(command);
            if (count == 0) return false;
            return true;
        }
        public bool UpdateOrAddToDB(SqlConnection conn = null, bool closeAfterExecute = true)
        {
            List<string> conditionList = new List<string>();
            for (int i = 0; i < Columns.Length; i++)
            {
                if (i == UPDATE_INDEX) continue;
                conditionList.Add(Columns[i] + "='" + StringValues[i] + "'");
            }
            string updateCommand = "update " + DB_NAME + " SET " + string.Join(",", conditionList) + " WHERE " + Columns[UPDATE_INDEX] + "='" + StringValues[UPDATE_INDEX] + "'";
            string addCommand = "insert INTO " + DB_NAME + " (" + string.Join(",", Columns) + ") VALUES ('" + string.Join("','", StringValues) + "')";
            string command = "IF EXISTS (SELECT * FROM " + DB_NAME + " WHERE " + Columns[UPDATE_INDEX] + "='" + StringValues[UPDATE_INDEX] + "') " +
                              updateCommand + " ELSE " + addCommand;
            int count = DB.Utility.ExecuteQuery(command, conn, closeAfterExecute);
            if (count == 0) return false;
            return true;
        }
        public bool DeleteToDB()
        {
            string command = "delete from " + DB_NAME + " WHERE " + Columns[UPDATE_INDEX] + "='" + StringValues[UPDATE_INDEX] + "'";
            int count = DB.Utility.ExecuteQuery(command);
            if (count == 0) return false;
            return true;
        }
        #endregion

        #region 公有靜態方法
        public static T[] GetAll<T>(bool checkIsLive = true) where T : Base
        {
            List<T> objectList = new List<T>();
            string dbPropertyName = "DB_NAME";
            string dbColumnPropertyName = "Columns";
            if (typeof(T).GetProperty(dbPropertyName) != null)
            {
                T newObjectToGetDbName = (T)Activator.CreateInstance(typeof(T));

                string tempDbName = (string)newObjectToGetDbName.GetType().GetProperty(dbPropertyName).GetValue(newObjectToGetDbName, null);
                string[] tempColumnsName = (string[])newObjectToGetDbName.GetType().GetProperty(dbColumnPropertyName).GetValue(newObjectToGetDbName, null);

                bool hasIsLiveColumn = (Array.IndexOf(tempColumnsName, "IsLive") != -1);
                string command = "select * from " + tempDbName;
                if (hasIsLiveColumn && checkIsLive) command += " where IsLive='1'";
                DataTable dt = DB.Utility.GetDataTable(command);
                foreach (DataRow dr in dt.Rows)
                {
                    //object[] args = new object[] { dr };
                    T newObject = (T)Activator.CreateInstance(typeof(T), dr);
                    objectList.Add(newObject);
                }
                return objectList.ToArray();
            }
            return null;
        }
        #endregion
    }

}
