using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Page_Register_DJ_Submit : System.Web.UI.Page {
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
	protected string myProto{ get { return Request.QueryString["proto"]; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm() {
		//var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID && p.ReturnProduct == false && p.StatusPenjualan == 0);
        var d = (from p in db.StockProductDJ
                where p.ProtoNomor != ""
                && (p.ProtoNomor.Contains("-") ? p.ProtoNomor.Substring(0, p.ProtoNomor.IndexOf('-')) : p.ProtoNomor) == "" + myProto + "" && p.ReturnProduct == false /*&& p.StatusPenjualan == 0*/
                select p).First();

        var outlet = (from p in db.StockProductDJ
                      where p.ProtoNomor != ""
                     && (p.ProtoNomor.Contains("-") ? p.ProtoNomor.Substring(0, p.ProtoNomor.IndexOf('-')) : p.ProtoNomor) == "" + myProto + "" && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StockActualDJ.TipeLokasi == 1
                 select p);

        var exh = (from p in db.StockProductDJ
                   where p.ProtoNomor != ""
                  && (p.ProtoNomor.Contains("-") ? p.ProtoNomor.Substring(0, p.ProtoNomor.IndexOf('-')) : p.ProtoNomor) == "" + myProto + "" && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StockActualDJ.TipeLokasi == 2
                 select p);

        var x = outlet.GroupBy(p => p.StockActualDJ.IDLokasi);
        var y = exh.GroupBy(p => p.StockActualDJ.IDLokasi);

        string Lokasi = "";
        string table = "";

        foreach (var r in x)
        {
            int IDLokasi = r.First().StockActualDJ.IDLokasi;

            var loc = db.LocOutlet.SingleOrDefault(p=>p.ID == IDLokasi);

            Lokasi += "<b>Outlet - " + loc.Nama + " : " + r.Count().ToString() + "</b>";
            int count = 1;

            var u = outlet.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);

            

            table = "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\">";
            table += "<tr><td><b>Kode PLU</b></td><td><b>Kode Karet</b></td></tr>";

            foreach (var s in u)
            {
                /*if (count > 1)
                    Lokasi += ", ( " + s.Nomor + " - <em>" + s.ProtoNomor + "</em> )";
                else
                    Lokasi += " / ( " + s.Nomor + " - <em>" + s.ProtoNomor + "</em> )";
                count++;*/
                table += "<tr><td>" + s.Nomor + "</td><td>" + s.ProtoNomor + "</td></tr>";
            }

            table += "</table><br />";

            Lokasi += "<br />";
            Lokasi += table;
        }

        foreach (var r in y)
        {
            int IDLokasi = r.First().StockActualDJ.IDLokasi;

            var loc = db.LocExhibition.SingleOrDefault(p => p.ID == IDLokasi);

            Lokasi += "<b>Exhibition - " + loc.Nama + " : " + r.Count().ToString() + "</b>";

            var u = outlet.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);

            int count = 1;
            table = "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\">";
            table += "<tr><td><b>Kode PLU</b></td><td><b>Kode Karet</b></td></tr>";

            foreach (var s in u)
            {
                /*if (count > 1)
                    Lokasi += ", ( " + s.Nomor + " - <em>" + s.ProtoNomor + "</em> )";
                else
                    Lokasi += " / ( " + s.Nomor + " - <em>" + s.ProtoNomor + "</em> )";
                count++;*/
                table += "<tr><td>" + s.Nomor + "</td><td>" + s.ProtoNomor + "</td></tr>";
            }
            table += "</table><br />";

            Lokasi += "<br />";
            Lokasi += table;
        }

        lokasi.Text = "<div style=\"float: left;\">" + Lokasi + "</div>";

        if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + (d.ProtoNomor.Contains("-") ? d.ProtoNomor.Substring(0,d.ProtoNomor.IndexOf('-')) : d.ProtoNomor) + "</a>";

			//Trans. Form
            brand.InnerHtml = d.LocWarehouse.CompanyBrand.Nama;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
            designnomor.InnerHtml = (d.ProtoNomor.Contains("-") ? d.ProtoNomor.Substring(0,d.ProtoNomor.IndexOf('-')) : d.ProtoNomor);
			

			////Product Char.
			var product = d.StockProductDJ_CharProduct;
            if (product == null) DataError();
            else
            {
                productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
                productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
                productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
                stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
                framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
                framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
                framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
                processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

                //Weight
                grossweight.InnerHtml = Str.Number3(product.GrossWeight);
                stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
                stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
                nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

                //Dimensi
                dimensid.InnerHtml = Str.Number2(product.DimensiD);
                dimensip.InnerHtml = Str.Number2(product.DimensiP);
                dimensil.InnerHtml = Str.Number2(product.DimensiL);
                dimensir.InnerHtml = Str.Number2(product.DimensiR);
            }

			//DJ Char.
			var design = d.StockProductDJ_CharDesign;
            if (design == null) DataError();
            else
            {
                designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
                designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
                designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
                targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
                targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

                naming.InnerHtml = design.Naming;
                designket.InnerHtml = designKet(design);
                grafir.InnerHtml = design.Grafir;
                finishing.InnerHtml = designFinishing(d);
            }

			//Stone
			fillStone(d);

			//Estimasi Harga
            harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(d.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0") + "</p>";

			//Picture
			Upload.Image(imgpicture, d.ImgPicture);
		}
	}

	//DJ Info
	protected string designKet(StockProductDJ_CharDesign d) {
		string x = string.Empty;

		if (d.SetItem) {
			if (x != "") x += ", ";
			x += "Set Item";
		}
		if (d.Special) {
			if (x != "") x += ", ";
			x += "Special DJ";
		}

		return x;
	}
	protected string designFinishing(StockProductDJ d) {
		string x = string.Empty;

		var rs = from p in d.StockProductDJ_Finishing select p;
		foreach (var r in rs) {
			if (x != "") x += "<br />";
			x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
		}

		return x;
	}

	//Stone
	protected void fillStone(StockProductDJ d) {
		int GT_Butir = 0;
		decimal GT_Carat = 0;

		//Stone
		fillStone1A(0, ref GT_Butir, ref GT_Carat, d);
		fillStone1B(1, ref GT_Butir, ref GT_Carat, d);
		fillStone2(2, ref GT_Butir, ref GT_Carat, d);
		fillStone3(3, ref GT_Butir, ref GT_Carat, d);
		fillStone4(4, ref GT_Butir, ref GT_Carat, d);
		fillStone5(5, ref GT_Butir, ref GT_Carat, d);

		//Grand-Total
		Sub_Total(GT_Butir, GT_Carat, true);
	}

	//Stone
	protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
        var rs = d.StockProductDJ_Stone1A;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            string Parcel103 = "";
            string Parcel104 = "";

            /*if (Regex.Matches(r.Stone1A.Nama, "A4").Count > 0 || Regex.Matches(r.Stone1A.Nama, "S4").Count > 0)
            {
                Parcel103 = "F";
                Parcel104 = "VS";
            }
            else
            {
                Parcel103 = "F";
                Parcel104 = "VVS";
            }*/

            Parcel103 = "F";
            if (r.Stone1A.Parcel104.Nama == "SI")
                Parcel104 = "VS";
            else
                Parcel104 = "VVS";

			td = new TableCell();
			td.Text = r.Stone1A.Parcel101.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Parcel103;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = Parcel104;
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
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
        var rs = d.StockProductDJ_Stone1B;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1B.Parcel101.Nama, r.GIA);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1B.Parcel103.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1B.Parcel104.Nama;
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
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
        var rs = d.StockProductDJ_Stone2;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			

            td = new TableCell();
            td.Text = r.Stone2.Parcel201.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone2.Parcel203.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone2.Parcel204.Nama;
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
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
		var rs = d.StockProductDJ_Stone3;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			

            td = new TableCell();
            td.Text = r.Stone3.Parcel301.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone3.Parcel303.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone3.Parcel304.Nama;
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
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
		var rs = d.StockProductDJ_Stone4;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			

			td = new TableCell();
			td.Text = TD_Dimensi(r.DimensiP);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone4.Parcel401.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone4.Parcel403.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone4.Parcel403.Nama;
            td.CssClass = "text-center";
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
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d) {
		//SELECT
		var rs = d.StockProductDJ_Stone5;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			

            td = new TableCell();
            td.Text = r.Stone5.Parcel501.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone5.Parcel503.Nama;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone5.Parcel504.Nama;
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
		if (rs.Count() > 0) {
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
		td.CssClass = "text-right";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	//Display Column
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> <a target=\"_blank\" class=\"text-muted text-sm\" href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";

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
}