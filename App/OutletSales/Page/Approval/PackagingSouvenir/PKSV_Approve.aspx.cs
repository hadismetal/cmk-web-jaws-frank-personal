using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_PKSV_Approve : System.Web.UI.Page
{
    protected int id { get { return App.QueryInt(this); } }
    protected string tipe { get { return Request.QueryString["tipe"]; } }
    protected string status { get { return Request.QueryString["status"]; } }
    protected string operator_nama { get { return Request.QueryString["operator_nama"]; } }
    protected string keterangan_reject { get { return Request.QueryString["keterangan_reject"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        save();
    }

    protected void save()
    {
        string msg = "";

        string tipe_result = ChangeStringWildCardCharacterSQL(tipe);
        string status_result = ChangeStringWildCardCharacterSQL(status);
        string operator_result = ChangeStringWildCardCharacterSQL(operator_nama);
        string keterangan_reject_result = ChangeStringWildCardCharacterSQL(keterangan_reject);

        string query = "EXEC sp_approve_or_reject_pksv " +
            "'" + id + "', " +
            "'" + tipe_result + "', " +
            "'" + operator_result + "', " + 
			"'" + status_result + "', " + 
            "'" + keterangan_reject_result + "'";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        int result = Convert.ToInt32(dt.Rows[0]["RESULT"]);
        msg = dt.Rows[0]["MESSAGE"].ToString();

        Response.Redirect("PKSV.aspx?d=1&msg=" + msg);
    }

    protected string ChangeStringWildCardCharacterSQL(string input = "")
    {
        string x = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].ToString() == "'")
                x += input[i] + "'";
            else
                x += input[i];
        }
        return x;
    }
}