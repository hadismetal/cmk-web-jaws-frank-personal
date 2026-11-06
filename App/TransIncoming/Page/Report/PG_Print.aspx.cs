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

        var d = db.StockIncomingPG.SingleOrDefault(p => p.ID == id);

        //if (Convert.ToInt32(Request.QueryString["model"]) > 0) { var v_mod = rsM.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["model"])); model.InnerText = v_mod.Nama; } else { model.InnerText = "Semua"; }
        //if (Convert.ToInt32(Request.QueryString["item"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["item"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }

        brand.InnerText = d.StockOutgoingPG.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.StockOutgoingPG.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.StockOutgoingPG.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.StockOutgoingPG.CompanyBrand.Nama;
        nomor_kirim.InnerText = d.StockOutgoingPG.Nomor;
        nomor_terima.InnerText = d.Nomor;
        keterangan_kirim.InnerText = Str.HtmlBreak(d.StockOutgoingPG.Keterangan);

        // Label Outgoing
        lit_status.Text = "Incomong";
        status.InnerText = DL.IncomingApprovalStatus(d.StatusTransIn);
        tglapprove.InnerText = d.ApprovalTgl != null ? Str.TglJam(d.ApprovalTgl) : "-"; 
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];
        //search.InnerText = Request.QueryString["key"] == "" ? "-" : Request.QueryString["key"];

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
        var rs = (from p in db.StockIncomingPG_Product  where p.StockIncomingPG.ID == id select p); // Outgoing
        //var rs = (from p in db.StockIncomingPG_Product select p); // Outgoing
        //WHERE
       // rs = rs.Where(p => p.StockIncomingPG.StockOutgoingPG.IDBrand == vB && p.StockIncomingPG.ApprovalTgl >= d_pf && p.StockIncomingPG.ApprovalTgl < d_pt && p.StockIncomingPG.ApprovalTgl != null && p.StockIncomingPG.ApprovalTgl != null);
       // rs = rs.Where(p => p.StockIncomingPG.StockOutgoingPG.TipeAsal == tF);
       // rs = rs.Where(p => p.StockIncomingPG.StockOutgoingPG.TipeTujuan == tT);
       // if (stats > 0) { rs = rs.Where(p => p.StockIncomingPG.StatusTransIn == stats); } // Status Outgoing
       // if (p_mod > 0) { rs = rs.Where(p => p.StockProductPG.Model == p_mod); }
       // if (p_itm > 0) { rs = rs.Where(p => p.StockProductPG.ProductItem == p_itm); }
       // if (key != "") { rs = rs.Where(p => p.StockProductPG.Nomor.Contains(key)); }
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

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                try
                {
                    if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locWarehouseDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locOutletDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locExhibitionDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locWarehouseDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locOutletDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locExhibitionDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }

                string Consignment = "";
                Consignment = Db.SingleString("select CASE WHEN c.Consignment IS NULL OR c.Consignment = 0 THEN 'Non Consignment' else 'Consignment' end Consignment from StockOutgoingPG a JOIN (SELECT IDForm,IDProduct FROM StockOutgoingPG_Product) b on b.IDForm=a.ID JOIN StockProductPG c on c.ID=b.IDProduct where c.ID=" + item.IDProduct, Db.Cnn_DB);

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockIncomingPG.Nomor;
                //td.VerticalAlign = VerticalAlign.Top;
                //tr.Cells.Add(td);

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
                td.Text = item.StockIncomingPG.StockOutgoingPG.NamaKurir;
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
                td.Text = item.StockProductPG.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)Consignment;
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