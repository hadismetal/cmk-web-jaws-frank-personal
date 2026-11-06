using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

public partial class DbAccess
{
	//Data Table Builder
	public static DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
	{
		DataTable rs = new DataTable();
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
			sqlCnn.Open();
			SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
			cmd.CommandType = CommandType.StoredProcedure;
			foreach (var x in param)
			{
				cmd.Parameters.Add(x);
			}
			SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
			sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
			DataSet objDS = new DataSet();
			sqlAdapter.Fill(objDS, "data");
			sqlCnn.Close();
			rs = objDS.Tables["data"];
		}
		return rs;
	}

	//Data Table Builder
	public static DataSet Ds(string strSql, string CnnString, List<SqlParameter> param)
	{
        DataSet objDS = new DataSet();
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            sqlCnn.Open();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
			sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
			sqlAdapter.Fill(objDS);
			sqlCnn.Close();
		}
		return objDS;
	}

	//Execute Command
	public static void Execute(string strSql, string CnnString, List<SqlParameter> param)
	{
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            sqlCnn.Open();
            cmd.ExecuteNonQuery();
			sqlCnn.Close();
		}
	}

	//Scalar Function
	public static string SingleString(string strSql, string CnnString, List<SqlParameter> param)
	{
        string x = "";
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
			SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();			
			x = (string)cmd.ExecuteScalar();
			sqlCnn.Close();
		}
        return x;
    }
	public static int SingleInteger(string strSql, string CnnString, List<SqlParameter> param)
	{
		int x = 0;
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();
			x = (int)cmd.ExecuteScalar();
			sqlCnn.Close();
		}
		return x;
	}
	public static decimal SingleDecimal(string strSql, string CnnString, List<SqlParameter> param)
	{
        decimal x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();
			x = Convert.ToDecimal(cmd.ExecuteScalar());
			sqlCnn.Close();
		}
		return x;
	}
	public static bool SingleBool(string strSql, string CnnString, List<SqlParameter> param)
	{
		bool x = false;
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();
			x = (bool)cmd.ExecuteScalar();
			sqlCnn.Close();
		}
		return x;
	}
	public static byte SingleByte(string strSql, string CnnString, List<SqlParameter> param)
	{
		byte x;
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();
			x = (byte)cmd.ExecuteScalar();
			sqlCnn.Close();
		}
		return x;
	}
	public static DateTime SingleTime(string strSql, string CnnString, List<SqlParameter> param)
	{
		DateTime x;
		using (SqlConnection sqlCnn = new SqlConnection(CnnString))
		{
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var y in param)
            {
                cmd.Parameters.Add(y);
            }
            sqlCnn.Open();
			x = (DateTime)cmd.ExecuteScalar();
			sqlCnn.Close();
		}
		return x;
	}
}