using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_Submit : System.Web.UI.Page {
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
	protected int idstock{ get { return Convert.ToInt32(Request.QueryString["idstock"]); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

        var repairdata = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        var locationbrand_outlet = db.LocOutlet;
        var locationbrand_exhibition = db.LocExhibition;

        string CB = ""; // mondial, franknco, missmondial, thepalace
        if (repairdata.TipeLokasi == 1) { CB = locationbrand_outlet.SingleOrDefault(p => p.ID == repairdata.IDLokasi).CompanyBrand.Nama; }
        else if (repairdata.TipeLokasi == 2) { CB = locationbrand_exhibition.SingleOrDefault(p => p.ID == repairdata.IDLokasi).CompanyBrand.Nama; }

        CB = CB.ToLower();

        if (CB == "mondial") { img_logo_web.Src = "/Images/web/logo_web_mondial.jpg"; }
        else if (CB == "miss mondial") { img_logo_web.Src = "/Images/web/logo_web_missmondial.jpg"; }
        else if (CB == "the palace") { img_logo_web.Src = "/Images/web/logo_web_thepalace.jpg"; }
        else if (CB == "frank & co") { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
        else { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() 
    {
        bool isRepairStock = false; // true : repair stock, false : repair toko

		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml = d.Nomor;
            lit_nomor_repair.Text = d.Nomor;

            string Cabang = "";
			string kodeCabang = string.Empty;
            //if (d.TipeLokasi == 1)
            //    Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi,Db.Cnn_DB);
            //else
            //    Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);

            string AlamatCabang = "";
            if (d.TipeLokasi == 0 && isRepairStock)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = kodeCabang + "\n" + Db.SingleString("SELECT AddrAlamat FROM LocWarehouse WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else if (d.TipeLokasi == 1)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                kodeCabang = "(" + Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB) + ")";
                AlamatCabang = kodeCabang + "\n" + Db.SingleString("SELECT AddrAlamat FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else if (d.TipeLokasi == 2)
            {
				Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
				kodeCabang = "(" + Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB) + ")";
                AlamatCabang = kodeCabang + "\n" + Db.SingleString("SELECT AddrAlamat FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }

            cabang.Text = Str.HtmlBreak(AlamatCabang);

            tgl.InnerText = Str.Tgl(d.Tgl);
            tglselesai.InnerText = Str.Tgl(d.TglSelesai);
            ket.InnerText = d.Keterangan;

            customer.InnerText = (!isRepairStock) ? d.CustomerNama : "-";
            jpjr.InnerText = (!isRepairStock) ? d.SalesNama : "-";

            if (d.IDProduct != null)
            {
                noplu.InnerText = d.StockProductDJ.Nomor;
                productitem.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
				material.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.CharFrameMaterial.Nama;
                kodeKaret.InnerText = d.StockProductDJ.ProtoNomor;
            }
            else if (d.IDProductTitipan != null)
            {
                noplu.InnerText = d.StockProductDJCustomer.Nomor;
                productitem.InnerText = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
            }

            //Upload.Image(imgpicture, d.ImgPicture);
            string Img = "";

            Img = d.ImgPicture;
            string str_image = "";
            //imgpicture.ImageUrl = Img;

            str_image = "<img src='/Images.aspx?path=" + Img + "' style='width: 100px;' border='0' /></div>";

            //lit_image_product.Text = str_image;

			////Product Char.
            var product = d.DocRepair_CharProduct;
            if (product == null) { }
            else
            {
                //Picture Repair
                Upload.Image(imgpicturerepair, product.DocRepair.ImgPicture);

                //Weight
                if (d.IDProduct != null)
                {
                    var product_ori = d.StockProductDJ.StockProductDJ_CharProduct;

                    td_grossweight.InnerHtml = Str.Number2(product_ori.GrossWeight) + " &#8594; " + Str.Number2(product.GrossWeight);
                    totalbutir.InnerHtml = Str.Number2(product.StoneQty);

                    dimensid.InnerHtml = Str.Number2(product_ori.DimensiD) + " &#8594; " + Str.Number2(product.DimensiD);
                    dimensir.InnerHtml = Str.Number2(product_ori.DimensiR) + " &#8594; " + Str.Number2(product.DimensiR);

                    //Picture Before
                    Upload.Image(imgpicturebefore, product_ori.StockProductDJ.ImgPicture);
                }
                else if (d.IDProductTitipan != null)
                {
                    var product_ori = d.StockProductDJCustomer.StockProductDJCustomer_CharProduct;

                    td_grossweight.InnerHtml = Str.Number2(product_ori.GrossWeight) + " &#8594; " + Str.Number2(product.GrossWeight);
                    totalbutir.InnerHtml = Str.Number2(product.StoneQty);

                    dimensid.InnerHtml = Str.Number2(product_ori.DimensiD) + " &#8594; " + Str.Number2(product.DimensiD);
                    dimensir.InnerHtml = Str.Number2(product_ori.DimensiR) + " &#8594; " + Str.Number2(product.DimensiR);

                    //Picture Before
                    Upload.Image(imgpicturebefore, product_ori.StockProductDJCustomer.ImgPicture);
                }

                
                
                customer2.Text = (!isRepairStock) ? d.CustomerNama : "-";
                jp.Text = (!isRepairStock) ? d.SalesNama : "-";

            }

			//Stone
			fillStone();
			
			fillStone_Stock();

            //Repair Item
            fillRepairItem();

		}
	}

    //Repair Item
    protected void fillRepairItem()
    {
        decimal GT = 0;

        var rs = from p in db.DocRepair_Repair
                 where p.IDForm == myID
                 orderby p.CharProcessRepair.Nama
                 select p;

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
            td.Text = TD_Nama(r.CharProcessRepair.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Biaya.Value).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += rs.Sum(p=>p.Biaya.Value);
        }
        Sub_Total(GT);
    }

	//Stone
	protected void fillStone() {
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
	protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone1A.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
	protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
	protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone2.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
	protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone3.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
	protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone4.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
	protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
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
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Nama(r.Stone5.Nama);
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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
            td.Text = r.Keterangan;
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
		td.CssClass = "text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 2;
		td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		td.Text += "</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-sm\">" + Str.Integer(TotalButir) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-sm\">" + Str.Number3(TotalCarat) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

    protected void Sub_Total(decimal Total)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "text-grandtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += "Grand Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Number0(Total) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
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

		x += "<p class=\"text-sm text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-sm text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}
	
	//Stone
    protected void fillStone_Stock()
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        //Stone
        fillStone1A_Stock(0, ref GT_Butir, ref GT_Carat);
        fillStone1B_Stock(1, ref GT_Butir, ref GT_Carat);
        fillStone2_Stock(2, ref GT_Butir, ref GT_Carat);
        fillStone3_Stock(3, ref GT_Butir, ref GT_Carat);
        fillStone4_Stock(4, ref GT_Butir, ref GT_Carat);
        fillStone5_Stock(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total_Stock(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1A
                 where p.IDForm == idstock
                 orderby p.Stone1A.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            //td = new TableCell();
            //td.Text = "-";
            //tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone1B_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone1B
                 where p.IDForm == idstock
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            // td = new TableCell();
            // td.Text = "-";
            // tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone2_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone2
                 where p.IDForm == idstock
                 orderby p.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            // td = new TableCell();
            // td.Text = "-";
            // tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone3_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone3
                 where p.IDForm == idstock
                 orderby p.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            // td = new TableCell();
            // td.Text = "-";
            // tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone4_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone4
                 where p.IDForm == idstock
                 orderby p.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            // td = new TableCell();
            // td.Text = "-";
            // tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone5_Stock(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var rs = from p in db.StockProductDJ_Stone5
                 where p.IDForm == idstock
                 orderby p.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header_Stock(StoneIndex);

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

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiP);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiL);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = TD_Dimensi(r.DimensiT);
            //td.CssClass = "text-center";
            //tr.Cells.Add(td);

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

            // td = new TableCell();
            // td.Text = "-";
            // tr.Cells.Add(td);

            tb3.Rows.Add(tr);
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total_Stock(ST_Butir, ST_Carat, false);
        }
    }
    //Custom Row
    protected void Sub_Header_Stock(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }
    protected void Sub_Total_Stock(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-sm\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }
}