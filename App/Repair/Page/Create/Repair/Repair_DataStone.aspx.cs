using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_Stone : System.Web.UI.Page {
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
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			//HyperLink
			add.HRef += "?id=" + myID;

			//Form
			fillForm();

			//Generate
			fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }
	protected void ProductError() { Response.Redirect("Repair_Data.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            lit_customer.Text = d.DataCustomer.Nama;
            lit_jpjr.Text = d.DataSales.Nama;
            lit_tgl.Text = Str.FullDate(d.Tgl);
            lit_tgl2.Text = Str.FullDate(d.TglSelesai);
            lit_keterangan.Text = Str.HtmlBreak(d.Keterangan);

            if (d.IDProduct != null) { lit_plu.Text = d.StockProductDJ.Nomor; }
            else if (d.IDProductTitipan != null) { lit_plu.Text = d.StockProductDJCustomer.Nomor; }

            var d_item_list = from p in db.DocRepair_Repair where p.IDForm == myID select p;

            int index = 1;

            foreach (var r in d_item_list)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProcessRepair.Nama;
                tr.Cells.Add(td);

                tbl_repairitem.Rows.Add(tr);

                index++;

            }

            var product = d.DocRepair_CharProduct;
            if (d.IDProduct != null)
            {
                var productold = d.StockProductDJ.StockProductDJ_CharProduct;
                if (product == null) ProductError();
                else
                {
                    grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    //kadarlogam.InnerHtml = (product.KadarLogam != productold.KadarLogam ? Str.Number2(productold.KadarLogam) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.KadarLogam);
                    diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);
                }
            }
            else if (d.IDProductTitipan != null)
            {
                var productold = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct;
                if (product == null) ProductError();
                else
                {
                    grossweight.InnerHtml = (product.GrossWeight != productold.GrossWeight ? Str.Number2(productold.GrossWeight) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md text-danger' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.GrossWeight);
                    //kadarlogam.InnerHtml = (product.KadarLogam != productold.KadarLogam ? Str.Number2(productold.KadarLogam) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.KadarLogam);
                    diameter.InnerHtml = (product.DimensiD != productold.DimensiD ? Str.Number2(productold.DimensiD) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiD);
                    panjang.InnerHtml = (product.DimensiP != productold.DimensiP ? Str.Number2(productold.DimensiP) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiP);
                    lebar.InnerHtml = (product.DimensiL != productold.DimensiL ? Str.Number2(productold.DimensiL) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiL);
                    ringsize.InnerHtml = (product.DimensiR != productold.DimensiR ? Str.Number2(productold.DimensiR) + "&nbsp;<span class='glyphicon glyphicon-arrow-right text-md' aria-hidden='true'></span>&nbsp;" : "") + Str.Number2(product.DimensiR);
                }
            }
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		int GT_Butir = 0;
		decimal GT_Carat = 0;

        int GT_ButirCurr = 0;
        decimal GT_CaratCurr = 0;

		//Stone Current
		fillCurrStone1A(0, ref GT_ButirCurr, ref GT_CaratCurr);
        fillCurrStone1B(1, ref GT_ButirCurr, ref GT_CaratCurr);
        fillCurrStone2(2, ref GT_ButirCurr, ref GT_CaratCurr);
        fillCurrStone3(3, ref GT_ButirCurr, ref GT_CaratCurr);
        fillCurrStone4(4, ref GT_ButirCurr, ref GT_CaratCurr);
        fillCurrStone5(5, ref GT_ButirCurr, ref GT_CaratCurr);

        //Stone Repair
        fillStone1A(0, ref GT_Butir, ref GT_Carat);
        fillStone1B(1, ref GT_Butir, ref GT_Carat);
        fillStone2(2, ref GT_Butir, ref GT_Carat);
        fillStone3(3, ref GT_Butir, ref GT_Carat);
        fillStone4(4, ref GT_Butir, ref GT_Carat);
        fillStone5(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
        Sub_TotalCurr(GT_ButirCurr, GT_CaratCurr, true);
	}

	//Stone
    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone1A
                 where p.IDForm == myID
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1A.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StatusTerpasang == 1
                    ? "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>"
                    : "<div class='text-center'><span class='glyphicon glyphicon-remove' aria-hidden='true' style='color:red; font-size:25px;'></span></div>"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone2
                 where p.IDForm == myID
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone2.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone3
                 where p.IDForm == myID
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone3.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone4
                 where p.IDForm == myID
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone4.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.DocRepair_Stone5
                 where p.IDForm == myID
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(StoneIndex, r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone5.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<div class='text-center'><span class='glyphicon glyphicon-ok' aria-hidden='true' style='color:green; font-size:25px;'></span></div>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    //Stone
    protected void fillCurrStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == d.IDProduct
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1A.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillCurrStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == d.IDProduct && p.StatusTerpasang == 1
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillCurrStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == d.IDProduct
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone2.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillCurrStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone3
                 where p.IDForm == d.IDProduct
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone3.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillCurrStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == d.IDProduct
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone4.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillCurrStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        //SELECT
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == d.IDProduct
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderCurr(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone5.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiL);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiT);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_CaratButir(r.CaratButir);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_TotalCurr(ST_Butir, ST_Carat, false);
        }
    }

	//Custom Row
	protected void Sub_Header(int StoneIndex) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += DL.StoneIndex(StoneIndex);

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
    protected void Sub_HeaderCurr(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }
	protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 5;
		td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		td.Text += "</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
    protected void Sub_TotalCurr(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 6;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }
	//Display Column
	protected string TD_HyperLink(int StoneIndex, int TableID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "Repair_DataStoneUpdate.aspx";
		href += "?id=" + myID;
		href += "&si=" + StoneIndex;
		href += "&tid=" + TableID;

		//x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "      <i class=\"icon-pencil\"></i> ";
		x += "      ";
		x += "  </a>";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + StoneIndex + "', '" + TableID + "')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      ";
		x += "  </a>";
		//x += "</div>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != "")
			x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

		return x;
	}
	protected string TD_Dimensi(decimal? Dimensi) {
		string x = string.Empty;

		x += "<p>" + Str.Number2(Dimensi) + "</p>";

		return x;
	}
	protected string TD_CaratButir(decimal? CaratButir) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

		return x;
	}
	protected string TD_TotalButir(int? TotalButir, ref int ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		Response.Redirect("Repair_Submit.aspx?id=" + myID);
	}
}