using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI.WebControls;

public partial class Page_Report_QCDJ_Approve : System.Web.UI.Page
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
    protected DateTime TglCutOff = Convert.ToDateTime("2026-03-05");

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            RedirectToHome();

            //var data = db.DocQCDJ.SingleOrDefault(p => p.ID == myID).StockProductDJ.StockActualDJ;

            DataTable dt = Rs("IF EXISTS (SELECT 1 FROM DocQCDJ WHERE ID = @ID AND TipeLokasi IS NOT NULL AND IDLokasi IS NOT NULL) " +
                "BEGIN " +
                "SELECT TOP 1 A.ID, A.TipeLokasi, A.IDLokasi, C.Nama, C.Brand, C.AddrAlamat FROM DocQCDJ A " +
                "JOIN (SELECT A.ID, 1 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocOutlet A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID " +
                "UNION ALL " +
                "SELECT A.ID, 2 Tipe, A.Nama, A.Kode, B.Nama [Brand], A. AddrAlamat FROM LocExhibition A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID) C ON A.IDLokasi = C.ID AND A.TipeLokasi = C.Tipe " +
                "WHERE A.ID = @ID " +
                "END " +
                "ELSE " +
                "BEGIN " +
                "SELECT TOP 1 A.ID, A.TipeLokasi, A.IDLokasi, C.Nama, C.Brand, C.AddrAlamat FROM DocQCDJ A " +
                "JOIN StockActualDJ B ON A.IDProduct = B.IDProduct " +
                "JOIN (SELECT A.ID, 1 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocOutlet A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID " +
                "UNION ALL " +
                "SELECT A.ID, 2 Tipe, A.Nama, A.Kode, B.Nama [Brand], A.AddrAlamat FROM LocExhibition A " +
                "JOIN CompanyBrand B ON A.IDBrand = B.ID) C ON B.IDLokasi = C.ID AND B.TipeLokasi = C.Tipe " +
                "WHERE A.ID = @ID " +
                "END", Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@ID", SqlDbType.Int) { Value = myID } }, CommandType.Text);
            //int TipeLokasi = Convert.ToInt32(dt.Rows[0]["TipeLokasi"]);
            //int IDLokasi = Convert.ToInt32(dt.Rows[0]["IDLokasi"]);
            string brand = dt.Rows[0]["Brand"].ToString();
            string alamat = dt.Rows[0]["AddrAlamat"].ToString();

            string CB = brand; // mondial, franknco, missmondial, thepalace
            string Alamat = alamat;

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
    }

    protected void fillForm()
    {
        var q = db.DocQCDJ.SingleOrDefault(p => p.ID == myID);
        int tipelokasi = Convert.ToInt32(Db.Rs("SELECT TOP 1 ISNULL(TipeLokasi,-1) TipeLokasi FROM DocQCDJ WHERE ID = " + myID, Db.Cnn_DB).Rows[0]["TipeLokasi"]);
        int idlokasi = Convert.ToInt32(Db.Rs("SELECT TOP 1 ISNULL(IDLokasi,-1) IDLokasi FROM DocQCDJ WHERE ID = " + myID, Db.Cnn_DB).Rows[0]["IDLokasi"]);
        if (q.StockProductDJ == null) App.DataNull(this);
        else
        {
            try
            {
                nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
                //Upload.Image(imgpicture, q.StockProductDJ.ImgPicture);
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
                keterangan.InnerText = q.Keterangan;
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductDJ.ImgPicture;
            }
            catch
            {
                nodocqc.InnerText = q.Nomor;
                tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
                //Upload.Image(imgpicture, q.StockProductDJ.ImgPicture);
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
                keterangan.InnerText = q.Keterangan;
                string url = ConfigurationManager.AppSettings["ImageAPI"];
                imgpicture.ImageUrl = url + q.StockProductDJ.ImgPicture;
            }

            //fillStone();

            string warning = string.Empty;
            if (IsStoreManager(idlokasi, tipelokasi, ref warning))
            {
                reject.Visible = true;
                approve.Visible = true;
                lbl_warning.Visible = false;
                lbl_warning.InnerText = string.Empty;
            }
            else
            {
                reject.Visible = false;
                approve.Visible = false;
                lbl_warning.Visible = true;
                lbl_warning.InnerText = warning;
            }
        }
    }

    protected bool IsStoreManager(int idlokasi, int tipelokasi, ref string warning)
    {
        bool IsSM = false;
        using (Map_AC ac = new Map_AC())
        {
            var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if (Role.Manager)
            {
                if (idlokasi >= 0 && tipelokasi > -1)
                {
                    string kodelokasi = string.Empty;
                    if (tipelokasi == 1)
                        kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == idlokasi).Kode;
                    else if(tipelokasi == 2)
                        kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == idlokasi).Kode;
                    string jabatan = user.Keterangan.ToString();
                    if (jabatan.Contains("JEWELLERY REPRESENTATIVE") || jabatan.Contains("GOLD REPRESENTATIVE") || jabatan.Contains("ADMINISTRATIVE EXECUTIVE"))
                    {
                        IsSM = false;
                        warning = "Only SM / AM Who Can Approve / Reject.";
                    }
                    else if(jabatan.Contains("STORE MANAGER") || jabatan.Contains("AREA MANAGER") || jabatan.Contains("JEWELLERY COORDINATOR"))
                    {
                        if (tipelokasi != 2)
                        {
                            IsSM = getLocationUser(kodelokasi);
                            if (!IsSM)
                                warning = "SM / AM Not In This Store.";
                        }
                        else
                        {
                            IsSM = true;
                        }
                    }
                    else
                    {
                        IsSM = true;
                    }
                }
                else
                {
                    IsSM = true;
                }
            }
        }
        return IsSM;
    }

    protected bool getLocationUser(string kodelokasi)
    {
        bool isSM = false;
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        if (result.Contains(kodelokasi))
            isSM = true;

        return isSM;
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
            td.Text = r.StockProductDJ_Stone1B.Stone1B.Parcel101.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone1B.Stone1B.Parcel102.Nama;
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
            td.Text = r.StockProductDJ_Stone3.Stone3.Parcel301.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone3.Stone3.Parcel302.Nama;
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
            td.Text = r.StockProductDJ_Stone4.Stone4.Parcel401.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone4.Stone4.Parcel402.Nama;
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
            td.Text = r.StockProductDJ_Stone5.Stone5.Parcel501.Nama + " " + appendcert;//+ " " + r.StockProductDJ_Stone5.Stone5.Parcel502.Nama;
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

    protected void Approve_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, RejectNama, RejectTgl, OperatorTgl FROM DocQCDJ WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsRejected = dt.Rows[0]["RejectNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsRejected)
            {
                // Alert + Redirect
                string url = "QCDJ.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Rejected! Tolong Buat QC Ulang'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", script, true);
            }
            else
            {
                string appnama = App.UserNama;
                string queryupadte = "UPDATE DocQCDJ SET ApprovalNama = @ApprovalNama, ApprovalTgl = @ApprovalTgl WHERE ID = @ID";
                List<SqlParameter> parameters2 = new List<SqlParameter>()
                {
                    new SqlParameter("@ApprovalNama", SqlDbType.VarChar){ Value = appnama },
                    new SqlParameter("@ApprovalTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                    new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
                };
                Execute(queryupadte, Db.Cnn_DB, parameters2, CommandType.Text);

                // Alert + Redirect
                string url = "QCDJ_Detail.aspx?id=" + myID;
                string script = "alert('Data success approved!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert2", script, true);
            }
        }
        else
        {
            string appnama = App.UserNama;
            string queryupadte = "UPDATE DocQCDJ SET ApprovalNama = @ApprovalNama, ApprovalTgl = @ApprovalTgl WHERE ID = @ID";
            List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@ApprovalNama", SqlDbType.VarChar){ Value = appnama },
                new SqlParameter("@ApprovalTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
            };
            Execute(queryupadte, Db.Cnn_DB, parameters2, CommandType.Text);

            // Alert + Redirect
            string url = "QCDJ_Detail.aspx?id=" + myID;
            string script = "alert('Data success approved!'); window.location='" + url + "';";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert2", script, true);
        }
    }

    protected void Rejected_Click(object sender, EventArgs e)
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, ApprovalNama, ApprovalTgl, OperatorTgl FROM DocQCDJ WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsApproved = dt.Rows[0]["ApprovalNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsApproved)
            {
                // Alert + Redirect
                string url = "QCDJ.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Approved! Data Tidak Bisa Direject.'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert3", script, true);
            }
            else
            {
                string appnama = App.UserNama;
                string queryreject = "UPDATE DocQCDJ SET RejectNama = @RejectNama, RejectTgl = @RejectTgl, RejectKeterangan = @RejectKeterangan WHERE ID = @ID";
                List<SqlParameter> parameters2 = new List<SqlParameter>()
                {
                    new SqlParameter("@RejectNama", SqlDbType.VarChar){ Value = appnama },
                    new SqlParameter("@RejectTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                    new SqlParameter("@RejectKeterangan", SqlDbType.VarChar){ Value = keterangan_reject.Value },
                    new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
                };
                Execute(queryreject, Db.Cnn_DB, parameters2, CommandType.Text);

                // Alert + Redirect
                string url = "QCDJ.aspx";
                string script = "alert('Data success rejected!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert4", script, true);
            }
        }
        else
        {
            string appnama = App.UserNama;
            string queryreject = "UPDATE DocQCDJ SET RejectNama = @RejectNama, RejectTgl = @RejectTgl, RejectKeterangan = @RejectKeterangan WHERE ID = @ID";
            List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@RejectNama", SqlDbType.VarChar){ Value = appnama },
                new SqlParameter("@RejectTgl", SqlDbType.DateTime){ Value = DateTime.Now },
                new SqlParameter("@RejectKeterangan", SqlDbType.VarChar){ Value = keterangan_reject.Value },
                new SqlParameter("@ID", SqlDbType.VarChar){ Value = myID }
            };
            Execute(queryreject, Db.Cnn_DB, parameters2, CommandType.Text);

            // Alert + Redirect
            string url = "QCDJ.aspx";
            string script = "alert('Data success rejected!'); window.location='" + url + "';";
            ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert4", script, true);
        }
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = cmdType;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            sqlCnn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    protected void RedirectToHome()
    {
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs("SELECT TOP 1 Nomor, RejectNama, RejectTgl, ApprovalNama, ApprovalTgl, OperatorTgl FROM DocQCDJ WHERE ID = @ID", Db.Cnn_DB, parameters, CommandType.Text);
        bool IsRejected = dt.Rows[0]["RejectNama"] == DBNull.Value ? false : true;
        bool IsApprove = dt.Rows[0]["ApprovalNama"] == DBNull.Value ? false : true;
        DateTime TglQC = Convert.ToDateTime(dt.Rows[0]["OperatorTgl"]);
        string NomorQC = dt.Rows[0]["Nomor"].ToString();

        if (TglCutOff <= TglQC)
        {
            if (IsRejected)
            {
                // Alert + Redirect
                string url = "QCDJ.aspx";
                string script = "alert('QC " + NomorQC + " Sudah Rejected! Tolong Buat QC Ulang'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert6", script, true);
            }
            else if (IsApprove)
            {
                // Alert + Redirect
                string url = "QCDJ_Detail.aspx?id=" + myID;
                string script = "alert('Data has already approved!'); window.location='" + url + "';";
                ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert5", script, true);
            }
        }
    }
}