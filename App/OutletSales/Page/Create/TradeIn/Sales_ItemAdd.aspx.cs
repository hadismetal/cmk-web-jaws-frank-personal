using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page {
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
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this,"so"); } }
    protected int myIDRO { get { return App.QueryInt(this,"ro"); } }
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("TradeIn.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

		//Customize-Qstr
		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        fill_DJ();
        fill_PG();
        fill_GJ();
        fill_LD();
	}
    protected void fill_DJ()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockProductDJ
                     where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false &&
                     p.StockActualDJ.IDLokasi == d.IDLokasi && p.StockActualDJ.TipeLokasi == d.TipeLokasi && p.StockActualDJ.Substorage == 1
                     && !(from z in db.SalesOrderDJ
                          where z.SalesOrder.StatusVoid == false
                          select z.IDProduct).Contains(p.ID)
                     select p;

            int tipe = 1; // 1 = DJ, 2 = GJ, 3 = PG, 4 = LD
            int tipe_svr = 1; // 1 = DJ, 2 = GJ, 3 = PG, 4 = LD

            string mode = "info";
            var rssv = (from z in db.Bundle_ItemPasangan
                        where z.TipeProduct == tipe_svr
                          && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                        select z.IDProduct);

            // promo
            var rspr = (from z in db.PromoDJ_Detail
                        where z.PromoDJ.TglExpire > System.DateTime.Now && z.PromoDJ.Draft == false
                        select z.IDProduct);

            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor.Contains(kw.Text));

            int index = 1;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    if (rspr.Contains(r.ID)) { mode = "success"; }
                    else if (rssv.Contains(r.ID)) { mode = "warning"; }
                    else { mode = "info"; }

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID, tipe, mode);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor + "<br />" + r.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                    tr.Cells.Add(td);

                    tbdj.Rows.Add(tr);

                    index++;
                }
            }
            else
            {
                App.TableNoResult(tbdj);
            }
        }
    }
    protected void fill_LD()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockProductLD_Stone1B
                     where p.StatusPenjualan == 0 && p.ReturnProduct == false &&
                     p.StockActualLD_Stone1B.IDLokasi == d.IDLokasi && p.StockActualLD_Stone1B.TipeLokasi == d.TipeLokasi && p.StockActualLD_Stone1B.Substorage == 1
                     && !(from z in db.SalesOrderLD
                          where z.SalesOrder.StatusVoid == false
                          select z.IDProduct).Contains(p.ID)
                     //&& !(from z in db.Bundle_ItemPasangan
                     //     where z.TipeProduct == 4
                     //       && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                     //     select z.IDProduct).Contains(p.ID)
                     select p;

            int tipe = 4; // 1 = DJ, 2 = PG, 3 = GJ, 4 = LD
            int tipe_svr = 4; // 1 = DJ, 2 = GJ, 3 = PG, 4 = LD

            string mode = "info";
            var rssv = (from z in db.Bundle_ItemPasangan
                        where z.TipeProduct == tipe_svr
                          && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                        select z.IDProduct);

            // promo
            //var rspr = (from z in db.PromoLD_Detail
            //            where z.PromoDJ.TglExpire > System.DateTime.Now && z.PromoDJ.Draft == false
            //            select z.IDProduct); 

            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor.Contains(kw.Text));

            int index = 1;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    //if (rspr.Contains(r.ID)) { mode = "success"; }
                    //else 
                    if (rssv.Contains(r.ID)) { mode = "warning"; }
                    else { mode = "info"; }

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID, tipe, mode);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor + "<br />" + "Loose Diamond";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.HargaTotal.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tbld.Rows.Add(tr);

                    index++;
                }
            }
            else
            {
                App.TableNoResult(tbld);
            }
        }
    }
    protected void fill_PG()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockProductPG
                     where p.Draft == false && p.StatusPenjualan == 0 && p.StockActualPG.TipeLokasi == d.TipeLokasi
                        && p.StockActualPG.IDLokasi == d.IDLokasi && p.StockActualPG.TipeLokasi == d.TipeLokasi && p.StockActualPG.Substorage == 1
                        && !(from z in db.SalesOrderPG
                             where z.SalesOrder.StatusVoid == false
                             select z.IDProduct).Contains(p.ID)
                     //&& !(from z in db.Bundle_ItemPasangan
                     //     where z.TipeProduct == 3
                     //       && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                     //     select z.IDProduct).Contains(p.ID)
                     select p;


            int tipe = 2; // 1 = DJ, 2 = PG, 3 = GJ, 4 = LD
            int tipe_svr = 3; // 1 = DJ, 2 = GJ, 3 = PG, 4 = LD

            string mode = "info";
            var rssv = (from z in db.Bundle_ItemPasangan
                        where z.TipeProduct == tipe_svr
                          && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                        select z.IDProduct);

            // promo
            var rspr = (from z in db.PromoPG_Detail
                        where z.PromoPG.TglExpire > System.DateTime.Now && z.PromoPG.Draft == false
                        select z.IDProduct);

            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor.Contains(kw.Text));

            int index = 1;
            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    if (rspr.Contains(r.ID)) { mode = "success"; }
                    else if (rssv.Contains(r.ID)) { mode = "warning"; }
                    else { mode = "info"; }

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID, tipe, mode);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor + "<br />" + r.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    decimal GoldPrice = gp.GetPGPricing(r, false);

                    if (r.FixRate == true)
                    {
                        GoldPrice = r.TotalHargaJual.Value;
                    }

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(GoldPrice).ToString("N0");
                    tr.Cells.Add(td);

                    tbpg.Rows.Add(tr);

                    index++;
                }
            }
            else
            {
                App.TableNoResult(tbpg);
            }
        }
    }
    protected void fill_GJ()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            var rs = from p in db.StockProductGJ
                     where p.Draft == false && p.StatusPenjualan == 0 && p.ReturnProduct == false &&
                     p.StockActualGJ.IDLokasi == d.IDLokasi && p.StockActualGJ.TipeLokasi == d.TipeLokasi && p.StockActualGJ.Substorage == 1
                     && !(from z in db.SalesOrderGJ
                          where z.SalesOrder.StatusVoid == false
                          select z.IDProduct).Contains(p.ID)
                     //&& !(from z in db.Bundle_ItemPasangan
                     //     where z.TipeProduct == 2
                     //       && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                     //     select z.IDProduct).Contains(p.ID)
                     select p;

            int tipe = 3;   // 1 = DJ, 2 = PG, 3 = GJ, 4 = LD
            int tipe_svr = 2; // 1 = DJ, 2 = GJ, 3 = PG, 4 = LD

            string mode = "info";
            var rssv = (from z in db.Bundle_ItemPasangan
                        where z.TipeProduct == tipe_svr
                          && z.BundleItem.TglExpire > System.DateTime.Now && z.BundleItem.Draft == false && z.BundleItem.StatusActive == 1
                        select z.IDProduct);

            // promo
            var rspr = (from z in db.PromoGJ_Detail
                        where z.PromoGJ.TglExpire > System.DateTime.Now && z.PromoGJ.Draft == false
                        select z.IDProduct);

            if (kw.Text != "")
                rs = rs.Where(p => p.Nomor.Contains(kw.Text));

            int index = 1;

            if (rs.Count() > 0)
            {
                foreach (var r in rs)
                {
                    if (rspr.Contains(r.ID)) { mode = "success"; }
                    else if (rssv.Contains(r.ID)) { mode = "warning"; }
                    else { mode = "info"; }

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID, tipe, mode);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor + "<br />" + r.StockProductGJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                    tr.Cells.Add(td);

                    tbgj.Rows.Add(tr);

                    index++;
                }
            }
            else
            {
                App.TableNoResult(tbgj);
            }
        }
    }

	//Display Column
    protected string TD_HyperLink(int ID, int itemindex, string mode = "info")
    {
		string x = string.Empty;
		string href = string.Empty;

		href += "Sales_ItemAddSave.aspx";
		href += "?id=" + myID;
		href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&item=" + itemindex;
		href += "&iditem=" + ID;

        x += "<a class=\"btn btn-" + mode + "\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  ";
		x += "</a>";

		return x;
	}
}