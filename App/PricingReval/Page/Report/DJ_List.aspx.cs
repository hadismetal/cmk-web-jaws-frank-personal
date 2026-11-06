using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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
    App price = new App();

    decimal Setting = 0m;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            
		}
        if (App.Refresh(this)) fillTable(sender, e);
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.RevalLogProductDJ
                 
                 select p;

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text).AddDays(1);
            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end);
        }

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor==""+kw.Text+"");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductCategory == ProductCategory);

        else if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                decimal AllM = 0m;
                decimal AllMOld = 0m;

                var x = r.RevalLogProductDJ_CostingProduct;
                var z = r.RevalLogProductDJ_Costing;

                AllMOld = (x.Total + z.TotalBiaya);

                var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor));

                var x2 = d.StockProductDJ_CostingProduct;
                var z2 = d.StockProductDJ_Costing;

                AllMOld = (x.Total + z.TotalBiaya);
                AllM = (x2.Total + z2.TotalBiaya);

                //var x = db.RevalLogProductDJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

                TableRow tr = new TableRow();
                TableCell td;

                /*CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(cb);
                td.Controls.Add(hf);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRoundOld).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRoundOld),Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRound));
                tr.Cells.Add(td);

                //td = new TableCell(); 
                //td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSDOld).ToString("N2");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllMOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllM).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSDOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in db.RevalLogProductDJ_Stone1A
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone1B
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone2
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone3
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone4
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone5
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                decimal TotalButir = 0m;
                try
                {
                    TotalButir = det.Sum(p => p.TotalButir);
                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {
                    TotalCarat = det.Sum(p => p.TotalCarat);
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = TotalButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }

            
        }


    }

    protected string TD_Mutasi(decimal OldPrice, decimal NewPrice)
    {
        string x = string.Empty;
        decimal mutasi = 0m;

        if (OldPrice == 0)
            mutasi = 0;
        else
            mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;

        string sgn = mutasi > 0 ? "+" : "";
        string css = mutasi > 10 || mutasi < -10 ? "text-red" : "";

        x += "<p class=\"" + css + " text-lg\">";
        x += sgn;
        x += Str.Number2(mutasi);
        x += "%";
        x += "</p>";

        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_ListExport.aspx?id=1";
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&order=" + orderby.SelectedValue;

        Response.Redirect(href);
    }
}