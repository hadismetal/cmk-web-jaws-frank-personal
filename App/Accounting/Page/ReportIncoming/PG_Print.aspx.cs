using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Print : System.Web.UI.Page
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
	protected int id { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		
        if (!IsPostBack) {
			Fill();
		}
	}
    protected void Error(string Message)
    {

    }
	protected void Fill() 
    {
        //Form-Default
        FillHeader();
        FillTable();
	}
    //Fill
    protected void FillHeader()
    {
        var rsB = db.CompanyBrand.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["idb"]));
        var rsM = from p in db.CharGoldModel select p;
        var rsI = from p in db.CharProductItem select p;

        if (Convert.ToInt32(Request.QueryString["model"]) > 0) { var v_mod = rsM.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["model"])); model.InnerText = v_mod.Nama; } else { model.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["item"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["item"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }

        periode.InnerHtml = Request.QueryString["pf"];
        periode.InnerHtml += " s/d " + Request.QueryString["pt"];
        brand.InnerText = rsB.Nama;
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));

        string From = ""; string To = "";
        if (Convert.ToInt32(Request.QueryString["ta"]) == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["ida"])); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["ta"]) == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["ida"])); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["ta"]) == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["ida"])); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
        if (Convert.ToInt32(Request.QueryString["tt"]) == 0 && Convert.ToInt32(Request.QueryString["idt"]) > 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["idt"])); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tt"]) == 1 && Convert.ToInt32(Request.QueryString["idt"]) > 0) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["idt"])); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tt"]) == 2 && Convert.ToInt32(Request.QueryString["idt"]) > 0) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["idt"])); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

        from.InnerHtml = From;
        to.InnerHtml = To;

        // Label Incoming
        lit_status.Text = "Incoming";
        status.InnerText = Convert.ToInt32(Request.QueryString["status"]) > 0 ? DL.IncomingApprovalStatus(Convert.ToInt32(Request.QueryString["status"])) : "Semua";
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];
        search.InnerText = Request.QueryString["key"] == "" ? "-" : Request.QueryString["key"];

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    protected void FillTable()
    {
        TableRow tr;
        TableCell td;

        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        DateTime d_pf = Convert.ToDateTime(Request.QueryString["pf"]);
        DateTime d_pt = Convert.ToDateTime(Request.QueryString["pt"]).AddDays(1);
        int vB = Convert.ToInt32(Request.QueryString["idb"]); // brand
        int tF = Convert.ToInt32(Request.QueryString["ta"]); ; // type from
        int tT = Convert.ToInt32(Request.QueryString["tt"]); ; // type to
        int idF = Convert.ToInt32(Request.QueryString["ida"]); // id from
        int idT = Convert.ToInt32(Request.QueryString["idt"]); // id to
        int stats = Convert.ToInt32(Request.QueryString["status"]);
        int p_mod = Convert.ToInt32(Request.QueryString["model"]);
        int p_itm = Convert.ToInt32(Request.QueryString["item"]);
        string key = Request.QueryString["key"];

        //SELECT
        var rs = (from p in db.StockIncomingPG_Product select p); // Incoming
        //WHERE
        rs = rs.Where(p => p.StockIncomingPG.StockIncomingPG.IDBrand == vB && p.StockIncomingPG.ApprovalTgl >= d_pf && p.StockIncomingPG.ApprovalTgl < d_pt && p.StockIncomingPG.ApprovalTgl != null && p.StockIncomingPG.ApprovalTgl != null);
        rs = rs.Where(p => p.StockIncomingPG.StockIncomingPG.TipeAsal == tF);
        rs = rs.Where(p => p.StockIncomingPG.StockIncomingPG.TipeTujuan == tT);
        if (stats > 0) { rs = rs.Where(p => p.StockIncomingPG.StatusTransIn == stats); } // Status Incoming
        if (p_mod > 0) { rs = rs.Where(p => p.StockProductPG.Model == p_mod); }
        if (p_itm > 0) { rs = rs.Where(p => p.StockProductPG.ProductItem == p_itm); }
        if (key != "") { rs = rs.Where(p => p.StockProductPG.Nomor.Contains(key)); }
        //ORDERBY
        rs = rs.OrderBy(p => p.StockIncomingPG.Tgl);

        //GROUPBY (utk rekap)
        //var rsg = from p in rs
        //          group p by new
        //          {
        //              p.StockIncomingPG.StockIncomingPG.TipeAsal,
        //              p.StockIncomingPG.StockIncomingPG.TipeTujuan,
        //              p.StockIncomingPG.StockIncomingPG.IDAsal,
        //              p.StockIncomingPG.StockIncomingPG.IDTujuan
        //          } into s
        //          select new
        //          {
        //              TipeAsal = s.Key.TipeAsal,
        //              TipeTujuan = s.Key.TipeTujuan,
        //              IDAsal = s.Key.IDAsal,
        //              IDTujuan = s.Key.IDTujuan,
        //              Qty = s.Count(),
        //              Netto = s.Sum(x => x.StockProductPG.StockProductPG_CharProduct.NettoWeight),
        //              Gross = s.Sum(x => x.StockProductPG.StockProductPG_CharProduct.GrossWeight),
        //              Butir = s.Sum(x => x.StockProductPG.StockProductPG_CharProduct.StoneQty),
        //              Carat = s.Sum(x => x.StockProductPG.StockProductPG_CharProduct.StoneCarat),
        //              PriceR = s.Sum(x => x.StockProductPG.StockProductPG_PricingMU.HargaRupiahRound)
        //          };

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            //decimal GT_Butir = 0m;
            //decimal GT_Carat = 0m;
            decimal GT_Price = 0m;

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                try
                {
                    if (item.StockIncomingPG.StockIncomingPG.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockIncomingPG.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockIncomingPG.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockIncomingPG.StockIncomingPG.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockIncomingPG.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockIncomingPG.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingPG.StockIncomingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingPG.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockIncomingPG.ApprovalTgl);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                string Status = "";
                if (item.SubStorageResult == 1)
                    Status = DL.IncomingApprovalStatus(item.StockIncomingPG.StatusTransIn);
                else if (item.SubStorageResult == 96)
                    Status = "Broken";
                else if (item.SubStorageResult == 99)
                    Status = "Lost";

                td = new TableCell();
                td.Text = Status;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockIncomingPG.StockIncomingPG.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingPG.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingPG.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingPG.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingPG.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingPG.StockIncomingPG.NamaKurir;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldModel.Nama;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharProductItem.Nama;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1"; // Str.Number0(item.Qty);
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.NettoWeight);
                GT_Netto += Convert.ToDecimal(item.StockProductPG.NettoWeight);
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.GrossWeight);
                GT_Gross += Convert.ToDecimal(item.StockProductPG.GrossWeight);
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number0(item.StockProductPG.StockProductPG_CharProduct.StoneQty);
                //GT_Butir += item.StockProductPG.StockProductPG_CharProduct.StoneQty;
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number3(item.StockProductPG.StockProductPG_CharProduct.StoneCarat);
                //GT_Carat += item.StockProductPG.StockProductPG_CharProduct.StoneCarat;
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductPG.TotalHarga);
                GT_Price += Convert.ToDecimal(item.StockProductPG.TotalHarga);
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<b>Total</b>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = "";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number0(GT_Qty) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number2(GT_Netto) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number2(GT_Gross) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number0(GT_Butir);
            //td.HorizontalAlign = HorizontalAlign.Right;
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(GT_Carat);
            //td.HorizontalAlign = HorizontalAlign.Right;
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number0(GT_Price) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
}