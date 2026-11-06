using System;
using System.Linq;

public partial class MasterPage : System.Web.UI.MasterPage
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
        lit_userstatus.Text = "";
        lit_userstatus_info.Text = "";

        string x_us_h = "";
        string x_us_i = "";

        App.Init(this);

		if (!IsPostBack)
			module.InnerHtml = App.NamaModule;

		//Sistem Callout
		if (Request.QueryString["d"] != null)
			callout.Visible = callout_script.Visible = !IsPostBack;

        x_us_h = "<span class='label label-danger'>ALL</span>";
        x_us_i = "<div class='alert alert-danger' role='alert' style='margin:0 5px; padding:5px; min-width:200px;'>Anda memiliki akses ke semua lokasi !!<hr style='padding:0; margin:5px 0;' /> Jika anda <b>bukan</b> Admin IT atau Coordinator AE, harap segera lapor ke bagian IT Support.</div>";

        //check area manager
        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;

        if (am.Count() > 0)
        {
            string xinfo = "";
            string[] OutletName = DLC.AMOutletGroup(App.UserID);
            //rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);

            var m = am.First();

            x_us_h = "<span class='label label-info'>" + m.Nama + "</span>";
            xinfo += "<div class='alert alert-info' role='alert' style='margin:0 5px; padding:5px; min-width:200px;'>";
            xinfo += "Anda memiliki akses sebagai Area Manager !!";
            xinfo += "<hr />";

            foreach(var l in OutletName)
            {
                xinfo += (l != null) ? l.ToString() + "<br />" : "";
            }
            xinfo += "</div>";

            x_us_i = xinfo;

        }
        else
        {
            //check admin sales
            var ad = from p in db.DataAdminSales
                     where p.IDUser == App.UserID
                     select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();

                //rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);

                string KodeLokasi = "";
                string NamaLokasi = "";

                if (c.TipeLokasi == 0) { var l = db.LocWarehouse.Single(p => p.ID == c.IDLokasi); KodeLokasi = "Warehouse"; NamaLokasi = l.Nama; }
                if (c.TipeLokasi == 1) { var l = db.LocOutlet.Single(p => p.ID == c.IDLokasi); KodeLokasi = l.Kode; NamaLokasi = l.Nama; }
                if (c.TipeLokasi == 2) { var l = db.LocExhibition.Single(p => p.ID == c.IDLokasi); KodeLokasi = l.Kode; NamaLokasi = l.Nama; }

                x_us_h = "<span class='label label-success'>" + KodeLokasi + "</span>";
                x_us_i = "<div class='alert alert-success' role='alert' style='margin:0 5px; padding:5px; min-width:200px;'>" + NamaLokasi + "</div>";
            }
        }

        lit_userstatus.Text = x_us_h;
        lit_userstatus_info.Text = x_us_i;


    }
}
