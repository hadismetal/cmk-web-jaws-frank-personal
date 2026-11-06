using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_Summary : System.Web.UI.Page 
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        //if (!IsPostBack)
        //{
        //}

        updateApproval();
    }


    protected void updateApproval()
    {
        var d = db.StockProductWhitelist.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {

            int mode = 1;
            string userapp = App.UserNama;
            string tipeplu = d.TipeProduct;
            string nomorplu = d.NomorPLU;
            string keterangan = d.Keterangan;
            string customernama = "";
            string customerkode = "";
            string requestornama = "";
            string requestorlokasi = "";
            string approvalgm = "";
            string requestvia = "";

            Db.Execute("exec sp_stockwhitelist_tradein "
                + mode.ToString()
                + ", '" + userapp + "'"
                + ", '" + tipeplu + "'"
                + ", '" + nomorplu + "'"
                + ", '" + keterangan + "'"
                + ", '" + customernama + "'"
                + ", '" + customerkode + "'"
                + ", '" + requestornama + "'"
                + ", '" + requestorlokasi + "'"
                + ", '" + approvalgm + "'"
                + ", '" + requestvia + "'"
                , Db.Cnn_DB);

        }

        Response.Redirect("WhitelistTradeIn.aspx?d=1");

    }



}