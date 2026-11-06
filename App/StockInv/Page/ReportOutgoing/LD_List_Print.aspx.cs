using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class LD : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillTable(sender, e);
            FillHeader();
        }
    }

    
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        string key = Request.QueryString["key"];
        string kF = Request.QueryString["keyfrom"];
        string kT = Request.QueryString["keyto"];
        string s_pf = Request.QueryString["pf"];
        string s_pt = Request.QueryString["pt"];
        int stats = Convert.ToInt32(Request.QueryString["status"]);
        int tF = Convert.ToInt32(Request.QueryString["from"]);
        int tT = Convert.ToInt32(Request.QueryString["to"]);
        int vB = Convert.ToInt32(Request.QueryString["brand"]);

        DateTime d_pf = Convert.ToDateTime(s_pf);
        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

        //SELECT
        var rs = (from p in db.StockOutgoingLD_Product select p);
        //WHERE
        rs = rs.Where(p => p.StockOutgoingLD.IDBrand == vB && p.StockOutgoingLD.ApprovalTgl >= d_pf && p.StockOutgoingLD.ApprovalTgl < d_pt && p.StockOutgoingLD.ApprovalTgl != null && p.StockOutgoingLD.ApprovalTgl != null);
        if (tF > 0) { rs = rs.Where(p => p.StockOutgoingLD.TipeAsal == tF); }
        if (tT > 0) { rs = rs.Where(p => p.StockOutgoingLD.TipeTujuan == tT); }
        if (stats > 0) { rs = rs.Where(p => p.StockOutgoingLD.StatusTransOut == stats); } 
        
        if (key != "") { rs = rs.Where(p => p.StockProductLD_Stone1B.Nomor.Contains(key)); }
        //ORDERBY
        rs = rs.OrderBy(p => p.StockOutgoingLD.Tgl);
        
        //GROUPBY (utk rekap)
        var rsg = (from p in rs
                  group p by new
                  {
                      p.StockOutgoingLD.TipeAsal,
                      p.StockOutgoingLD.TipeTujuan,
                      p.StockOutgoingLD.IDAsal,
                      p.StockOutgoingLD.IDTujuan,
                      p.StockOutgoingLD.NamaAsal,
                      p.StockOutgoingLD.NamaTujuan
                  } into s
                  select new
                  {
                      TipeAsal = s.Key.TipeAsal,
                      TipeTujuan = s.Key.TipeTujuan,
                      IDAsal = s.Key.IDAsal,
                      IDTujuan = s.Key.IDTujuan,
                      NamaAsal = s.Key.NamaAsal,
                      NamaTujuan = s.Key.NamaTujuan,
                      Qty = s.Count(),
                      Butir = s.Sum(x => x.StockProductLD_Stone1B.TotalButir),
                      Carat = s.Sum(x => x.StockProductLD_Stone1B.TotalCarat),
                      PriceR = s.Sum(x => x.StockProductLD_Stone1B.HargaTotal)
                  }).Where(p => p.NamaAsal.Contains(kF) && p.NamaTujuan.Contains(kT));

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {

            decimal qty = 0;
            decimal netto = 0;
            decimal gross = 0;
            decimal butir = 0;
            decimal carat = 0;
            decimal harga = 0;
            foreach (var item in rsg)
            {
                //string From = ""; string To = "";
                //if (item.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                //else if (item.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.IDAsal && p.Nama.Contains(kF)); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                //else if (item.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.IDAsal && p.Nama.Contains(kF)); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                //if (item.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.IDTujuan && p.Nama.Contains(kT)); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                //else if (item.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.IDTujuan && p.Nama.Contains(kT)); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                //else if (item.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.IDTujuan && p.Nama.Contains(kT)); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.NamaAsal;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.NamaTujuan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.Qty);
                td.HorizontalAlign = HorizontalAlign.Right;
                qty += item.Qty;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.Butir);
                td.HorizontalAlign = HorizontalAlign.Right;
                butir += Convert.ToDecimal(item.Butir);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.Carat);
                td.HorizontalAlign = HorizontalAlign.Right;
                carat += item.Carat;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.PriceR);
                td.HorizontalAlign = HorizontalAlign.Right;
                harga += Convert.ToDecimal(item.PriceR);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
            TableRow tr31 = new TableRow();
            TableCell td31;

            td31 = new TableCell();
            td31.Text = "<b>Total</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>-</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
          //  td31.Text = "<b>" + stockpgAll.ToString() + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + qty.ToString("N0") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + butir.ToString("N0") + "</b>";
            td31.HorizontalAlign = HorizontalAlign.Right;
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + carat.ToString("N2") + "</b>";
            td31.HorizontalAlign = HorizontalAlign.Right;
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + harga.ToString("N0") + "</b>";
            td31.HorizontalAlign = HorizontalAlign.Right;
            tr31.Cells.Add(td31);

            tb.Rows.Add(tr31);
        }


    }
    protected void Sub_Header(int t, int r)
    {
        //TableRow tr = new TableRow();
        //TableCell td;

        //tr.CssClass = "sub-header";
        //string x = string.Empty;

        //if (t == 0)
        //{
        //    var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 1)
        //{
        //    var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 2)
        //{
        //    var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}

        //td = new TableCell();
        //td.Text = x;
        //td.CssClass = "text-lg text-warning";
        //td.ColumnSpan = App.TableColspan(tb);
        //tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }
    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
        }

        return x;
    }

    protected void FillHeader()
    {
        var rs = (from p in db.StockOutgoingLD_Product select p);

        var rsB = db.CompanyBrand.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["brand"]));
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;

        periode.InnerHtml = Request.QueryString["pf"];
        periode.InnerHtml += " s/d " + Request.QueryString["pt"];
        brand.InnerText = rsB.Nama;
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["from"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["to"]));

        string From = ""; string To = "";
        string keyFrom = Convert.ToString(Request.QueryString["keyfrom"]);
        string keyTo = Convert.ToString(Request.QueryString["keyto"]);

        if (Convert.ToInt32(Request.QueryString["from"]) == 00) { From = "Semua"; }
        else if (Convert.ToInt32(Request.QueryString["from"]) == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.Nama.Contains(keyFrom)); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["from"]) == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.Nama.Contains(keyFrom)); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["from"]) == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.Nama.Contains(keyFrom)); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
        if (Convert.ToInt32(Request.QueryString["to"]) == 00) { To = "Semua"; }
        else if (Convert.ToInt32(Request.QueryString["to"]) == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.Nama.Contains(keyTo)); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["to"]) == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.Nama.Contains(keyTo)); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["to"]) == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.Nama.Contains(keyTo)); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
      

        from.InnerHtml = From;
        to.InnerHtml = To;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = Convert.ToInt32(Request.QueryString["status"]) > 0 ? DL.OutgoingApprovalStatus(Convert.ToInt32(Request.QueryString["status"])) : "Semua";
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];
        search.InnerText = Request.QueryString["key"] == "" ? "-" : Request.QueryString["key"];

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
}