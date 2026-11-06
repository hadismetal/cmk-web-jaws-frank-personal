using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Titipan_Stone : System.Web.UI.Page {
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
    protected int myTitipanID { get { return App.QueryInt(this,"idtitipan"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			//HyperLink
			add.HRef += "?id=" + myID + "&idtitipan=" + myTitipanID;

			//Form
			fillForm();

			//Generate
			fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Titipan_Data.aspx?id=" + myTitipanID); }
	protected void ProductError() { Response.Redirect("Titipan_CustomerCharProduct.aspx?id=" + myID + "&idtitipan=" + myTitipanID); }

	//Form
	protected void fillForm() {
		var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			var product = d.StockProductDJCustomer_CharProduct;
			if (product == null) ProductError();
			else {
				grossweight.InnerHtml = Str.Number3(product.GrossWeight);
				
			}
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		int GT_Butir = 0;
		decimal GT_Carat = 0;

		//Stone
		fillStone1A(0, ref GT_Butir, ref GT_Carat);
		fillStone1B(1, ref GT_Butir, ref GT_Carat);
		fillStone2(2, ref GT_Butir, ref GT_Carat);
		fillStone3(3, ref GT_Butir, ref GT_Carat);
		fillStone4(4, ref GT_Butir, ref GT_Carat);
		fillStone5(5, ref GT_Butir, ref GT_Carat);

		//Grand-Total
		Sub_Total(GT_Butir, GT_Carat, true);
	}

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJCustomer_Stone1A
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

            string nama = "";
            nama = r.Stone1A.Parcel101.Nama + " " + r.Stone1A.Parcel102.Nama + " " + r.Stone1A.Parcel103.Nama + " " + r.Stone1A.Parcel104.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
        var rs = from p in db.StockProductDJCustomer_Stone1B
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

            string nama = "";
            nama = r.Stone1B.Parcel101.Nama + " " + r.Stone1B.Parcel102.Nama + " " + r.Stone1B.Parcel103.Nama + " " + r.Stone1B.Parcel104.Nama
                + " " + r.Stone1B.Parcel105.Nama + " " + r.Stone1B.Parcel106.Nama + " " + r.Stone1B.Parcel107.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
        var rs = from p in db.StockProductDJCustomer_Stone2
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

            string nama = "";
            nama = r.Stone2.Parcel201.Nama + " " + r.Stone2.Parcel202.Nama + " " + r.Stone2.Parcel203.Nama + " " + r.Stone2.Parcel204.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
        var rs = from p in db.StockProductDJCustomer_Stone3
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

            string nama = "";
            nama = r.Stone3.Parcel301.Nama + " " + r.Stone3.Parcel302.Nama + " " + r.Stone3.Parcel303.Nama + " " + r.Stone3.Parcel304.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
        var rs = from p in db.StockProductDJCustomer_Stone4
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

            string nama = "";
            nama = r.Stone4.Parcel401.Nama + " " + r.Stone4.Parcel402.Nama + " " + r.Stone4.Parcel403.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
        var rs = from p in db.StockProductDJCustomer_Stone5
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

            string nama = "";
            nama = r.Stone5.Parcel501.Nama + " " + r.Stone5.Parcel502.Nama + " " + r.Stone5.Parcel503.Nama + " " + r.Stone5.Parcel504.Nama
                + " " + r.Stone5.Parcel505.Nama + " " + r.Stone5.Parcel506.Nama + " " + r.Stone5.Parcel507.Nama;

            td = new TableCell();
            td.Text = TD_Nama(nama);
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
            td.CssClass = "text-right";
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
	protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 7;
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
		td.CssClass = "text-right";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	//Display Column
	protected string TD_HyperLink(int StoneIndex, int TableID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "Titipan_CustomerStoneUpdate.aspx";
		href += "?id=" + myID;
		href += "&si=" + StoneIndex;
		href += "&tid=" + TableID;
        href += "&idtitipan=" + myTitipanID;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "      <i class=\"icon-pencil\"></i> ";
		x += "      Update";
		x += "  </a>";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + StoneIndex + "', '" + TableID + "','"+myTitipanID+"')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      Delete";
		x += "  </a>";
		x += "</div>";

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
		Response.Redirect("Titipan_CustomerPicture.aspx?id=" + myID + "&idtitipan=" + myTitipanID);
	}
}