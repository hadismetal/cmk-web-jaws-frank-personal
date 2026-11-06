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
        var rs = db.StockOutgoingPG.SingleOrDefault(p => p.ID == id);

        var rsB = db.CompanyBrand.SingleOrDefault(p => p.ID == rs.IDBrand);
        var rsM = from p in db.CharGoldModel select p;
        var rsI = from p in db.CharProductItem select p;
        
        brand.InnerText = rsB.Nama;
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(rs.TipeTujuan);

        string From = ""; string To = "";
        if (rs.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama; }
        else if (rs.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama; }
        else if (rs.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama; }
        if (rs.TipeTujuan == 0 && rs.IDTujuan > 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDTujuan); To = rst.Nama; }
        else if (rs.TipeTujuan == 1 && rs.IDTujuan > 0) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDTujuan); To = rst.Nama; }
        else if (rs.TipeTujuan == 2 && rs.IDTujuan > 0) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDTujuan); To = rst.Nama; }

        from.InnerHtml = From;
        to.InnerHtml = To;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(rs.StatusTransOut);
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];
        nodokumen.InnerText = rs.Nomor;
        keterangan.InnerHtml = Str.HtmlBreak(rs.Keterangan);

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    protected void FillTable()
    {
        TableRow tr;
        TableCell td;

        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        /*DateTime d_pf = Convert.ToDateTime(Request.QueryString["pf"]);
        DateTime d_pt = Convert.ToDateTime(Request.QueryString["pt"]).AddDays(1);
        int vB = Convert.ToInt32(Request.QueryString["idb"]); // brand
        int tF = Convert.ToInt32(Request.QueryString["ta"]); ; // type from
        int tT = rs.TipeTujuan; ; // type to
        int idF = Convert.ToInt32(Request.QueryString["ida"]); // id from
        int idT = rs.IDTujuan; // id to
        int stats = Convert.ToInt32(Request.QueryString["status"]);
        int p_mod = Convert.ToInt32(Request.QueryString["model"]);
        int p_itm = Convert.ToInt32(Request.QueryString["item"]);
        string key = Request.QueryString["key"];

        //SELECT
        var rs = (from p in db.StockOutgoingPG_Product select p); // Outgoing
        //WHERE
        rs = rs.Where(p => p.StockOutgoingPG.IDBrand == vB && p.StockOutgoingPG.ApprovalTgl >= d_pf && p.StockOutgoingPG.ApprovalTgl < d_pt && p.StockOutgoingPG.ApprovalTgl != null && p.StockOutgoingPG.ApprovalTgl != null);
        rs = rs.Where(p => p.StockOutgoingPG.TipeAsal == tF);
        rs = rs.Where(p => p.StockOutgoingPG.TipeTujuan == tT);
        if (stats > 0) { rs = rs.Where(p => p.StockOutgoingPG.StatusTransOut == stats); } // Status Outgoing
        if (p_mod > 0) { rs = rs.Where(p => p.StockProductPG.Model == p_mod); }
        if (p_itm > 0) { rs = rs.Where(p => p.StockProductPG.ProductItem == p_itm); }
        if (key != "") { rs = rs.Where(p => p.StockProductPG.Nomor.Contains(key)); }
        //ORDERBY
        rs = rs.OrderBy(p => p.StockOutgoingPG.Tgl);*/

        var rs = (from p in db.StockOutgoingPG_Product where p.IDForm == id select p); // Outgoing

        rs = rs.OrderBy(p => p.StockProductPG.Nomor);

        //GROUPBY (utk rekap)
        //var rsg = from p in rs
        //          group p by new
        //          {
        //              p.StockIncomingPG.StockOutgoingPG.TipeAsal,
        //              p.StockIncomingPG.StockOutgoingPG.TipeTujuan,
        //              p.StockIncomingPG.StockOutgoingPG.IDAsal,
        //              p.StockIncomingPG.StockOutgoingPG.IDTujuan
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
                    if (item.StockOutgoingPG.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockOutgoingPG.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
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
                td.Text = item.StockOutgoingPG.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockOutgoingPG.ApprovalTgl);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StockOutgoingPG.StatusTransOut);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockOutgoingPG.StockIncomingPG.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingPG.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingPG.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingPG.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingPG.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockOutgoingPG.NamaKurir;
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