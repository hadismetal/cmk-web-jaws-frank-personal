using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            tbl.Visible = false;

            DateDrv.ListBulan(ddlbulan);
            for (int i = DateTime.Today.Year; i >= DateTime.Today.Year -1; i--)
            {
                ddltahun.Items.Add(i.ToString());
                ddltahun.SelectedValue = DateTime.Today.Year.ToString();
            }
        }
    }

    protected void Display_Click(object sender, EventArgs e)
    {
        string Action = "display";
        string User = App.UserNama;
        Table tb = tbl;
        String Periode_Tahun = ddltahun.SelectedValue;
        String Periode_Bulan = ddlbulan.SelectedValue.PadLeft(2,'0');
        String Keterangan = txtketerangan.Text.Trim();
        bool Cutoff = false;

        RPT.Closing_Stock(User, tb, Periode_Tahun, Periode_Bulan, Keterangan, Cutoff, Action);
    }

    protected void Closing_Click(object sender, EventArgs e)
    {
        string Action = "display";
        string User = App.UserNama;
        Table tb = tbl;
        String Periode_Tahun = ddltahun.SelectedValue;
        String Periode_Bulan = ddlbulan.SelectedValue.PadLeft(2, '0');
        String Keterangan = txtketerangan.Text.Trim();
        bool Cutoff = true;

        RPT.Closing_Stock(User, tb, Periode_Tahun, Periode_Bulan, Keterangan, Cutoff, Action);
    }

}