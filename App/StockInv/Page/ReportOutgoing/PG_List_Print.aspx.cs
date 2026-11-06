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
        var rs = (from p in db.StockOutgoingPG_Product select p);

        var rsB = db.CompanyBrand.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["brand"]));
        
        var rsI = from p in db.CharProductItem select p;

        
        if (Convert.ToInt32(Request.QueryString["item"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["item"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }

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
    protected void FillTable()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tb.Visible = true;

        string key = Request.QueryString["keyword"];
        string kF = Request.QueryString["keyfrom"];
        string kT = Request.QueryString["keyto"];
        string s_pf = Request.QueryString["pf"];
        string s_pt = Request.QueryString["pt"];
        int stats = Convert.ToInt32(Request.QueryString["status"]);
        int tF = Convert.ToInt32(Request.QueryString["from"]);
        int tT = Convert.ToInt32(Request.QueryString["to"]);
        int p_cat = Convert.ToInt32(Request.QueryString["category"]);
        int p_itm = Convert.ToInt32(Request.QueryString["item"]);
        int vB = Convert.ToInt32(Request.QueryString["brand"]);

        DateTime d_pf = Convert.ToDateTime(s_pf);
        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

        //SELECT
        var rs = (from p in db.StockOutgoingPG_Product select p);
        //WHERE
        rs = rs.Where(p => p.StockOutgoingPG.IDBrand == vB && p.StockOutgoingPG.ApprovalTgl >= d_pf && p.StockOutgoingPG.ApprovalTgl < d_pt && p.StockOutgoingPG.ApprovalTgl != null && p.StockOutgoingPG.ApprovalTgl != null);
        if (tF > 0) { rs = rs.Where(p => p.StockOutgoingPG.TipeAsal == tF); }
        if (tT > 0) { rs = rs.Where(p => p.StockOutgoingPG.TipeTujuan == tT); }
        if (p_itm > 0) { rs = rs.Where(p => p.StockProductPG.ProductItem == p_itm); }
        if (stats > 0) { rs = rs.Where(p => p.StockOutgoingPG.StatusTransOut == stats); }

        if (key != "") { rs = rs.Where(p => p.StockOutgoingPG.Nomor.Contains(key)); }

        if (kF != "")
        {
            if (tF == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.StockOutgoingPG.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
            if (tF == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.StockOutgoingPG.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
            if (tF == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.StockOutgoingPG.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
        }

        if (kT != "")
        {
            if (tT == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.StockOutgoingPG.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
                     select p;
            }
            if (tT == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.StockOutgoingPG.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
                     select p;
            }
            if (tT == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.StockOutgoingPG.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
                     select p;
            }
        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.StockOutgoingPG.TipeAsal == d.First().TipeLokasi && p.StockOutgoingPG.IDAsal == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.StockOutgoingPG.TipeAsal == f.First().TipeLokasi && p.StockOutgoingPG.IDAsal == f.First().IDLokasi)
                 select p;
        }

        //ORDERBY
        rs = rs.OrderBy(p => p.StockOutgoingPG.Tgl);

        //var rs = (from p in db.StockOutgoingPG_Product where p.IDForm == id select p); // Outgoing

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

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                try
                {
                    if (item.StockOutgoingPG.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locWarehouseDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locOutletDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locExhibitionDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockOutgoingPG.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locWarehouseDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locOutletDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locExhibitionDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
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