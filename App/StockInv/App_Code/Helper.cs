using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Reflection;


public class Helper
{
    public static List<T> GetDataFromDataTable<T>(DataTable dataTable) where T : class, new()
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
                    prop.SetValue(data, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                }
            }

            // Add the location object to the list
            listOfData.Add(data);
        }

        return listOfData;
    }

    private static SqlDbType GetSqlDbType(Type type)
    {
        // Map common .NET types to SQL DbTypes
        if (type == typeof(int))
            return SqlDbType.Int;
        if (type == typeof(decimal))
            return SqlDbType.Decimal;
        if (type == typeof(string))
            return SqlDbType.NVarChar;
        if (type == typeof(DateTime))
            return SqlDbType.DateTime;
        if (type == typeof(bool))
            return SqlDbType.Bit;
        if (type == typeof(object))
            return SqlDbType.Structured;

        // Add other type mappings as necessary...

        // Default to SqlDbType.VarChar for unrecognized types
        return SqlDbType.NVarChar;
    }

    public static void AddParametersFromObject<T>(SqlCommand command, T obj) where T : class, new()
    {
        foreach (var property in obj.GetType().GetProperties())
        {
            // Create parameter name
            string paramName = "@" + property.Name;

            // Get property value
            SqlDbType sqlDbType = GetSqlDbType(property.PropertyType);


            // Add parameter to command
            command.Parameters.Add(new SqlParameter(paramName, sqlDbType) { Value = property.GetValue(obj) });
        }
    }

    public static object GetDataTesting()
    {
        return "GEMING";
    }
}
