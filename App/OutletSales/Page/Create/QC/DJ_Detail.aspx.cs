using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_QC_DJ_Detail : System.Web.UI.Page
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
	protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
        App.ProtectRole(Role.Manager);
		
		var data = db.DocQCDJ.SingleOrDefault(p => p.ID == myID).StockProductDJ.StockActualDJ;
        var locationbrand_outlet = db.LocOutlet;
        var locationbrand_exhibition = db.LocExhibition;

        string CB = ""; // mondial, franknco, missmondial, thepalace
		string Alamat = "";
        if (data.TipeLokasi == 1) 
		{ 
			CB = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama; 
			Alamat = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).AddrAlamat;
		}
        else if (data.TipeLokasi == 2) 
		{ 
			CB = locationbrand_exhibition.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama; 
			Alamat = locationbrand_exhibition.SingleOrDefault(p => p.ID ==data.IDLokasi).AddrAlamat;
		}

        CB = CB.ToLower();

        if (CB == "mondial") { img_logo_web.Src = "/Images/web/logo_web_mondial.jpg"; }
        else if (CB == "miss mondial") { img_logo_web.Src = "/Images/web/logo_web_missmondial.jpg"; }
        else if (CB == "the palace") { img_logo_web.Src = "/Images/web/logo_web_thepalace.jpg"; }
        else if (CB == "frank & co") { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
        else { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
		
		cabang.Text = Alamat;
		lit_nomor_repair.Text = db.DocQCDJ.SingleOrDefault(p => p.ID == myID).Nomor;

        fillForm();
		
		fillStone();
    }
	
	protected void fillForm()
    {
		var q = db.DocQCDJ.SingleOrDefault(p => p.ID == myID);
        if (q.StockProductDJ == null) App.DataNull(this);
        else
        {
			try
			{
                nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
				Upload.Image(imgpicture, q.StockProductDJ.ImgPicture);
                plu.InnerText = q.StockProductDJ.Nomor;
                petugasqc.InnerText = q.QCNama.ToUpper();
                //blemish.InnerText = q.Blemish;
                //jlhbutir.InnerText = q.JumlahButir;
                //sertifikat.InnerText = q.Sertifikat;
                //noncz.InnerText = q.NonCZ;
                grossweightoriginal.InnerText = Convert.ToDecimal(q.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("N2") + " gram";
                grossweight.InnerText = Convert.ToDecimal(q.GrossWeight).ToString("N2") + " gram";
                //cz.InnerText = q.CZ;
                logo.InnerText = q.Logo;
                kadaremas.InnerText = q.KadarEmas;
                framecolor.InnerText = q.WarnaEmas;
                //merk.InnerText = q.Merk;
                fisik.InnerText = q.FisikKeseluruhan;
				checkedby.InnerText = q.QCNama.ToUpper();
				keterangan.InnerText = q.Keterangan;
            }
			catch
			{
				nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
				Upload.Image(imgpicture, q.StockProductDJ.ImgPicture);
                plu.InnerText = q.StockProductDJ.Nomor;
                petugasqc.InnerText = q.QCNama.ToUpper();
                //blemish.InnerText = q.Blemish;
                //jlhbutir.InnerText = q.JumlahButir;
                //sertifikat.InnerText = q.Sertifikat;
                //noncz.InnerText = q.NonCZ;
                grossweightoriginal.InnerText = Convert.ToDecimal(q.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("N2") + " gram";
                grossweight.InnerText = Convert.ToDecimal(q.GrossWeight).ToString("N2") + " gram";
                //cz.InnerText = q.CZ;
                logo.InnerText = q.Logo;
                kadaremas.InnerText = q.KadarEmas;
                framecolor.InnerText = q.WarnaEmas;
                //merk.InnerText = q.Merk;
                fisik.InnerText = q.FisikKeseluruhan;
				checkedby.InnerText = q.QCNama.ToUpper();
				keterangan.InnerText = q.Keterangan;
			}
			
			//fillStone();
		}
	}

	protected void fillStone()
    {
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
        var rs = from p in db.DocQCDJ_Stone1A
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone1A.Stone1A.Nama
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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') [GIA] FROM StockProductDJ_Stone1A WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone1A.Stone1A.Parcel101.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone1A.Stone1A.Parcel102.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone1A.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone1A.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone1A.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone1A WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone1A WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone1A WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone1A.TotalButir;
            ST_Carat += r.StockProductDJ_Stone1A.TotalCarat;
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
        var rs = from p in db.DocQCDJ_Stone1B
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone1B.Stone1B.Nama
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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone1B WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone1B.Stone1B.Parcel101.Nama + " " + appendcert ;//+ " " + r.StockProductDJ_Stone1B.Stone1B.Parcel102.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone1B.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone1B.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone1B.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone1B WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone1B WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone1B WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone1B.TotalButir;
            ST_Carat += r.StockProductDJ_Stone1B.TotalCarat;
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
        var rs = from p in db.DocQCDJ_Stone2
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone2.Stone2.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone2 WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone2.Stone2.Parcel201.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone2.Stone2.Parcel202.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone2.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone2.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone2.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone2 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone2 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone2 WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone2.TotalButir;
            ST_Carat += r.StockProductDJ_Stone2.TotalCarat;
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
        var rs = from p in db.DocQCDJ_Stone3
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone3.Stone3.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone3 WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone3.Stone3.Parcel301.Nama + " " + appendcert ;//+ " " + r.StockProductDJ_Stone3.Stone3.Parcel302.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone3.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone3.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone3.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone3 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone3 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone3 WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone3.TotalButir;
            ST_Carat += r.StockProductDJ_Stone3.TotalCarat;
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
        var rs = from p in db.DocQCDJ_Stone4
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone4.Stone4.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone4 WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone4.Stone4.Parcel401.Nama + " " + appendcert ;//+ " " + r.StockProductDJ_Stone4.Stone4.Parcel402.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone4.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone4.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone4.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone4 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone4 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone4 WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone4.TotalButir;
            ST_Carat += r.StockProductDJ_Stone4.TotalCarat;
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
        var rs = from p in db.DocQCDJ_Stone5
                 where p.IDForm == myID
                 orderby p.StockProductDJ_Stone5.Stone5.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0m;

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
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);
			
			string sertifikat = Db.SingleString("SELECT TOP 1 ISNULL(GIA,'') [GIA] FROM StockProductDJ_Stone5 WHERE ID = " + r.IDStone, Db.Cnn_DB);
			string appendcert = (!string.IsNullOrEmpty(sertifikat)) ? "(GIA : " + sertifikat + ")" : "";

            td = new TableCell();
            td.Text = r.StockProductDJ_Stone5.Stone5.Parcel501.Nama + " " + appendcert ;//+ " " + r.StockProductDJ_Stone5.Stone5.Parcel502.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.StockProductDJ_Stone5.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.StockProductDJ_Stone5.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.StockProductDJ_Stone5.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalButirActual,0) [TotalButirActual] FROM DocQCDJ_Stone5 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N0");
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalCaratActual,0) [TotalCaratActual] FROM DocQCDJ_Stone5 WHERE ID = " + r.ID, Db.Cnn_DB).ToString("N3");
            td.CssClass = "text-center";
            tr.Cells.Add(td);
			
			td = new TableCell();
            bool sesuai = Db.SingleBool("SELECT TOP 1 ISNULL(SesuaiSertifikat,1) [SesuaiSertifikat] FROM DocQCDJ_Stone5 WHERE ID = " + r.ID, Db.Cnn_DB);
			td.Text = (sesuai == true) ? "Sesuai" : " Tidak Sesuai";
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.StockProductDJ_Stone5.TotalButir;
            ST_Carat += r.StockProductDJ_Stone5.TotalCarat;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void Sub_Header(int StoneIndex)
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
        td.ColumnSpan = 9;
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(TotalCarat);
        td.CssClass = "text-center";
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

        tb.Rows.Add(tr);
    }
}