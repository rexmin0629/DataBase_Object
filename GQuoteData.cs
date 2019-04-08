using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RiskManagement.Object
{
    //  Rex_Modify
    public class GQuoteData : DB.Base
    {
        #region 建構子
        public GQuoteData()
        {
            GQuoteData_GUID = Guid.NewGuid().ToString();
            Symbol = string.Empty;
            Data = string.Empty;
            Create_Time = DateTime.Now;
        }
        public GQuoteData(DataRow dr)
        {
            GQuoteData_GUID = dr[Columns[Array.IndexOf(Columns, COL_0_GQuoteData_GUID)]].ToString();
            Symbol = dr[Columns[Array.IndexOf(Columns, COL_1_Symbol)]].ToString();
            Data = dr[Columns[Array.IndexOf(Columns, COL_2_Data)]].ToString();
            DateTime timeObject = new DateTime(2001, 1, 1);
            DateTime.TryParse(dr[Columns[Array.IndexOf(Columns, COL_3_Create_Time)]].ToString(), out timeObject);
            Create_Time = timeObject;
        }
        #endregion

        #region 公有變數
        public string GQuoteData_GUID;
        public string Symbol;
        public string Data;
        public DateTime Create_Time;
        #endregion

        #region 公有變數-DB
        public static string _DB_NAME = "GQuoteData";
        public const string COL_0_GQuoteData_GUID = "GQuoteData_GUID";
        public const string COL_1_Symbol = "Symbol";
        public const string COL_2_Data = "Data";
        public const string COL_3_Create_Time = "Create_Time";
        #endregion

        #region 公有屬性-DB(實作父類別)
        public override string DB_NAME
        {
            get
            {
                return GQuoteData._DB_NAME;
            }
        }
        public override string[] Columns
        {
            get
            {
                return new string[]{
                    COL_0_GQuoteData_GUID,
                    COL_1_Symbol,
                    COL_2_Data,
                    COL_3_Create_Time
                };
            }
        }
        public override object[] Values
        {
            get
            {
                return new object[]{
                    GQuoteData_GUID,
                    Symbol,
                    Data,
                    Create_Time
                };
            }
        }
        #endregion

        #region 靜態方法
        public static GQuoteData[] GetGQuoteDataByDate(DateTime appointDate)
        {
            List<GQuoteData> objectList = new List<GQuoteData>();
            DateTime nextDate = appointDate.AddDays(1);
            string command = "select * from " + GQuoteData._DB_NAME + " where Create_Time>='" + appointDate.ToString("yyyy/MM/dd") + "' and Create_Time<'" + nextDate.ToString("yyyy/MM/dd") + "' And IsLive='1' order by Create_Time";
            DataTable dt = DB.Utility.GetDataTable(command);
            foreach (DataRow dr in dt.Rows)
            {
                GQuoteData newGQuoteData = new GQuoteData(dr);
                objectList.Add(newGQuoteData);
            }
            return objectList.ToArray();
        }
        #endregion

        #region 公有方法
        
        #endregion
    }
}
