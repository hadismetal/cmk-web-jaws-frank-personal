public partial class WriteResponse
{
    public static void ResponseApiStampsTransaction(string type, int idstamps, string noso, string tanggal, string statuscode, string statusdesc, string response, string brand) 
    {
        string query = "EXEC SP_INSERT_TRANSACTION_API_RESPONSE ";
        query += " '" + type + "',";
        query += " " + idstamps + ",";
        query += " '" + noso + "',";
        query += " '" + tanggal + "',";
        query += " '" + statuscode + "',";
        query += " '" + statusdesc + "',";
        query += " '" + response + "',";
        query += " '" + brand + "'";
        Db.Execute(query, Db.Cnn_IN);
    }

    public static void ResponseApiStampsVoucher(string type, int idvoucherstamps, string typevoucher, string novoucher, string noso, string plu,string tanggal, string statuscode, string statusdesc, string response, string brand) 
    {
        string query = "EXEC SP_INSERT_VOUCHER_API_RESPONSE ";
        query += " '" + type + "',";
        query += " " + idvoucherstamps + ",";
        query += " '" + typevoucher + "',";
        query += " '" + novoucher + "',";
        query += " '" + noso + "',";
		query += " '" + plu + "',";
        query += " '" + tanggal + "',";
        query += " '" + statuscode + "',";
        query += " '" + statusdesc + "',";
        query += " '" + response + "',";
        query += " '" + brand + "'";
        Db.Execute(query, Db.Cnn_IN);
    }
}