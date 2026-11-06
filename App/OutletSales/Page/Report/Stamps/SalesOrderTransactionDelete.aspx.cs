using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Stamps_SalesOrderTransactionDelete : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    protected string SONumber { get { return Request.QueryString["SONumber"].Replace('+', '/').Replace(' ','/'); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Send();
        }
    }
    protected void Send()
    {
        string Message = "";
        Literal lit_error_stamps = new Literal();
        if (SONumber != "")
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.Nomor == SONumber.Trim());
            if (d != null)
            {
                try
                {
                    if (STAMPS.xActivate)
                    {
						Message += ((Message == "") ? STAMPSM.prePostSalesOrderIntegration(d) : "");
                        Message += ((Message == "") ? STAMPSM.DeleteTransaction(lit_error_stamps, d) : "");
						Message += ((Message == "") ? "Delete Success." : Message); 
                    }
                    else
                    {
                        Message = "Hubungi IT Support. Delete Gagal. ";
                    }
                }
                catch
                {
					Message = "Terjadi Gangguan Teknis. Harap Hubungi IT Support. Send Gagal.";
                }
                Response.Redirect("/OutletSales/Page/Report/Stamps/SalesOrderTransaction.aspx?msg=" + Message + "");
            }
            else
            {
                Message = "Nomor SO tidak ditemukan pada report penjualan, Send Gagal.";
                Response.Redirect("/OutletSales/Page/Report/Stamps/SalesOrderTransaction.aspx?msg=" + Message + "");
            }
        }
        else
        {
            Message = "Nomor SO kosong, Send Gagal.";
            Response.Redirect("/OutletSales/Page/Report/Stamps/SalesOrderTransaction.aspx?msg=" + Message + "");
        }
    }
}