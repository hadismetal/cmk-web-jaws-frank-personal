using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DPPO_FormVoid : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    protected void fillForm()
    {
        string msg = "";

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.StatusDP == 0);
        if (d == null) DeleteError();
        else
        {
            po.InnerHtml = d.NomorDP;
            po.Style.Add("color", "red");

            if (d.StatusDP == 2)
            {
                // kondisi sudah tervoid
                div_keterangan_void.Visible = false;
                div_alert.Visible = true;

                msg += "Nomor PO sudah divoid. <br /> Keterangan Void : " + Db.SingleString("SELECT TOP 1 ISNULL(VoidKeterangan,'') [VoidKeterangan] FROM SalesReceiptDPPO WHERE ID = " + myID, Db.Cnn_DB);
                lit_error.Text = "<b>" + msg + "</b>";
            }
            if (d.StatusDP == 1)
            {
                // kondisi sudah terpakay
                div_keterangan_void.Visible = false;
                div_alert.Visible = true;

                msg += "Nomor PO sudah digunakan di Nomor SO " + Db.SingleString("SELECT TOP 1 ISNULL(Nomor,'') [NoSo] FROM SalesOrder WHERE ID = " + d.IDSalesOrder, Db.Cnn_DB);
                lit_error.Text = "<b>" + msg + "</b>";
            }
            else
            {
                div_keterangan_void.Visible = true;
                div_alert.Visible = false;
            }
        }
    }

    //Exception
    protected void DeleteError()
    {
        Response.Redirect("DPPO.aspx");
    }

    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("DPPO.aspx");
    }

    //Delete
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.StatusDP == 0);
        if (d == null) App.DataNull(this);
        else
        {
            d.StatusDP = 2;
            db.SubmitChanges();

            string tgl = DateTime.Now.ToString("yyyy-MM-dd");

            Db.Execute("UPDATE SalesReceiptDPPO SET TglVoid = '" + tgl + "', VoidKeterangan = '" + keterangan.Text + " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")' WHERE ID = " + myID.ToString(), Db.Cnn_DB);

            //Redirect
            Response.Redirect("DPPO.aspx?d=1");
        }
    }
}