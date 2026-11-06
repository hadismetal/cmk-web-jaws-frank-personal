using CrossBrand;
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
    Modelize cross = new Modelize();
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
        var d = cross.OutgoingPG(id).SingleOrDefault();

        string NamaAsal = "";
        if (d.TipeAsal == 0) { lit_type_from.Text = "Warehouse"; NamaAsal = cross.Warehouses(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }
        else if (d.TipeAsal == 1) { lit_type_from.Text = "Outlet"; NamaAsal = cross.Outlets(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }
        else if (d.TipeAsal == 2) { lit_type_from.Text = "Exhibition"; NamaAsal = cross.Exhibitions(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }

        string NamaTujuan = "";
        if (d.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; NamaTujuan = cross.Warehouses(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; NamaTujuan = cross.Warehouses(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; NamaTujuan = cross.Warehouses(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.IDBrandAsal).Nama;
        nomor.InnerText = d.Nomor;
        keterangan.InnerText = Str.HtmlBreak(d.Keterangan);
        kurir.InnerText = d.NamaKurir;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(d.StatusTransOut);
        tglkirim.InnerText = Str.TglJam(d.ApprovalTgl);

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

        var rs = cross.OutgoingPG(id).SingleOrDefault(); // Outgoing


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

        if (rs.CrossBrand_StockOutgoingPG_Product.Count == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            //decimal GT_Butir = 0m;
            //decimal GT_Carat = 0m;
            decimal GT_Price = 0m;

            foreach (var item in rs.CrossBrand_StockOutgoingPG_Product)
            {
                string From = ""; string To = "";
                try
                {
                    if (rs.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (rs.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (rs.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (rs.TipeTujuan == 0) { var rst = cross.Warehouses(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                    else if (rs.TipeTujuan == 1) { var rst = cross.Outlets(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                    else if (rs.TipeTujuan == 2) { var rst = cross.Exhibitions(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                }
                catch (Exception ex) { }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = index++ + ".";
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
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

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductPG.TotalHargaJual);
                GT_Price += Convert.ToDecimal(item.StockProductPG.TotalHargaJual);
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "Total";
            td.ColumnSpan = 4;
            td.HorizontalAlign = HorizontalAlign.Right;
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