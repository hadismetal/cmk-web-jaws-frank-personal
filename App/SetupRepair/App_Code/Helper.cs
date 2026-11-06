using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Reflection;


public class Helper
{
    public static List<T> GetDataFromDataTable<T>(DataTable dataTable, bool modifyValue = false) where T : class, new()
    {
        List<T> listOfData = new List<T>();

        // Loop through each DataRow in the DataTable
        foreach (DataRow dr in dataTable.Rows)
        {
            // Create a new IMapDataLocation object
            T data = new T();

            // Use reflection to set the properties dynamically
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (dataTable.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                {
                    var value = dr[prop.Name];

                    if (prop.Name == "ImgPicture" && value is string && modifyValue == true)
                    {
                        value = RepairLib.GetPathImage(dr[prop.Name].ToString());
                    }

                    prop.SetValue(data, Convert.ChangeType(value, prop.PropertyType), null);
                }
            }

            // Add the location object to the list
            listOfData.Add(data);
        }

        return listOfData;
    }

    public static string GetConnectionByBrand(int IDBrand)
    {
        string dbName = IDBrand == 7 ? "frank_db" : IDBrand == 5 ? "palace_db_real" : "mondial_db_real";
        string connectionString = "Data Source=dbprodjaws.intra.cmk.co.id;Initial Catalog=" + dbName + ";Persist Security Info=True;User ID=ITJAWS;Password=9HPZhTUMNAgipum;Connection Timeout=1290;max pool size=500;MultipleActiveResultSets=True;";
        return connectionString;
    }
}