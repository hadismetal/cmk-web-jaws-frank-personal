using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Submit : System.Web.UI.Page
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
    protected string myProto { get { return Request.QueryString["proto"]; } }
    protected string mySAP { get { return Request.QueryString["SAP"]; } }
    protected decimal myMin { get { return Convert.ToDecimal(Request.QueryString["min"]); } }
    protected decimal myMax { get { return Convert.ToDecimal(Request.QueryString["max"]); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        // adis turn linq into sp

        string query = "exec sp_collectHiddenWarehouse";
        DataTable dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        var wh = dt.Rows.OfType<DataRow>().Select(k => Convert.ToInt32(k[0])).ToArray();


        string Lokasi = "";
        string table = "";
        string ProtoNomor = myProto;
        string KodeProduksi = "";
        decimal dPriceFrom = 0m;
        decimal dPriceUpTo = 0m;
        int index = 0;

        DataTable header;
        DataTable product;
        DataTable designCoeg;
        DataTable finishHim;
        string KodeFMaterial = "";
        string ImgPicture = "";

        string queryIsUserMerchOrIT = "SELECT COUNT('') [Total] FROM UserAccount A " +
            " JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[mskaryawan] B ON A.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS = B.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS " +
            " JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[msdepartemen] C ON b.m_departemen = c.m_iddept " +
            " JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[msdivisi] D ON c.m_iddept = D.m_departemen " +
            " WHERE (C.m_iddept IN ('22','95') OR D.m_iddivisi = '692') AND A.ID = " + App.UserID;
        int isUserMerchOrIT = Db.SingleInteger(queryIsUserMerchOrIT, Db.Cnn_AC);
        if(isUserMerchOrIT > 0)
                isUserMerchOrIT = 1;
        else
                isUserMerchOrIT = 0;
        string query1 = "EXEC sp_katalogdj '" + myProto + "', 'DJ'," + isUserMerchOrIT + "";
        DataSet ds = QuerySPDataSet(query1, Db.Cnn_DB);


        table = "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\">";
        table += "<tr><td width=\"50\"><b>No.</b></td><td width=\"100\"><b>Location Type</b></td><td width=\"210\"><b>Location</b></td><td width=\"150\"><b>Kode PLU</b></td><td width=\"150\"><b>Kode Barang</b><td width=\"150\"><b>Warna</b></td></td><td width=\"100\"><b>Harga</b></td><td width=\"100\"><b>Status</b></td><td width=\"150\"><b>Ring Size 1</b></td><td width=\"150\"><b>Ring Size 2</b></td><td width=\"150\"><b>DP Stock</b></td><td width=\"150\"><b>Harga Promo/One Price</b></td><td width=\"150\"><b>Harga PWP</b></td><td width=\"210\"><b>Location To</b></td></tr>";
        if (ds.Tables.Count > 0)
        {
            header = ds.Tables[0];
            product = ds.Tables[1];
            designCoeg = ds.Tables[2];
            finishHim = ds.Tables[3];
            foreach (DataRow r in header.Rows)
            {
                if (db.DataAdminSales.Any(p => p.IDUser == App.UserID && p.Disable == false && p.TipeLokasi == 0))
                {
                    if (wh.Any(c => c == Convert.ToInt32(r["idlokasi"]))) continue;
                }

                if (Convert.ToDecimal(r["HargaRupiahRound"]) < dPriceFrom || dPriceFrom == 0m) { dPriceFrom = Convert.ToDecimal(r["HargaRupiahRound"]); }
                if (Convert.ToDecimal(r["HargaRupiahRound"]) > dPriceUpTo) { dPriceUpTo = Convert.ToDecimal(r["HargaRupiahRound"]); }
                
                decimal promoHarga = r.IsNull("PromoHarga") ? 0m : Convert.ToDecimal(r["PromoHarga"]);
                decimal hargaPWP = r.IsNull("HargaPWP") ? 0m : Convert.ToDecimal(r["HargaPWP"]);

                if (promoHarga > 0)
                {
                    if (promoHarga < dPriceFrom || dPriceFrom == 0m)
                        dPriceFrom = promoHarga;

                    if (promoHarga > dPriceUpTo)
                        dPriceUpTo = promoHarga;
                }

                if (hargaPWP > 0)
                {
                    if (hargaPWP < dPriceFrom || dPriceFrom == 0m)
                        dPriceFrom = hargaPWP;

                    if (hargaPWP > dPriceUpTo)
                        dPriceUpTo = hargaPWP;
                }

                index++;
                string tipeLokasi = r["locType"].ToString();
                KodeProduksi = r["KodeBarang"].ToString();

                string color = "black";
                if (myMin <= Convert.ToDecimal(r["HargaRupiahRound"]) && myMax >= Convert.ToDecimal(r["HargaRupiahRound"])) { color = "blue"; }

                if (promoHarga > 0 &&
                    myMin <= promoHarga &&
                    myMax >= promoHarga)
                {
                    color = "blue";
                }

                if (hargaPWP > 0 &&
                    myMin <= hargaPWP &&
                    myMax >= hargaPWP)
                {
                    color = "blue";
                }

                decimal RingSize = 0m;
                if (!r.IsNull("RingSize"))
                {
                    RingSize = Convert.ToDecimal(r["RingSize"]);
                }

                decimal RingSize2 = 0m;
                if (!r.IsNull("RingSize2"))
                {
                    RingSize2 = Convert.ToDecimal(r["RingSize2"]);
                }


                table += "<tr>";
                table += "<td>" + index + ".</td>";
                table += "<td>" + tipeLokasi + "</td>";
                table += "<td>" + r["NamaLokasi"].ToString() + "</td>";
                table += "<td>" + r["Nomor"].ToString() + "</td>";
                table += "<td>" + KodeProduksi + "</td>";
                table += "<td>" + r["nmColor"] + "</td>";
                table += "<td style='text-align:right; color:" + color + ";'>" + Str.Number0(Convert.ToDecimal(r["HargaRupiahRound"])) + "</td>";
                table += "<td>" + r["StatusBarang"].ToString() + "</td>";
                table += "<td>" + Str.Number2(RingSize) + "</td>";
                table += "<td>" + Str.Number2(RingSize2) + "</td>";
                table += "<td>" + (r.IsNull("NomorDP") ? "-" : r["NomorDP"].ToString()) + "</td>";
                table += "<td style='text-align:right; color:" + color + ";'>" + (promoHarga == 0m ? "-" : Str.Number0(promoHarga)) + "</td>";
                table += "<td style='text-align:right; color:" + color + ";'>" + (hargaPWP == 0m ? "-" : Str.Number0(hargaPWP)) + "</td>";
                table += "<td>" + r["NamaTujuan"].ToString() + "</td>";
                table += "</tr>";
            }

            table += "</table><br />";

            lokasi.Text = (index > 0) ? "<div style=\"float: left;\">" + table + "</div>" : "";
            if (product.Rows.Count > 0)
            {
                ImgPicture = product.Rows[0]["ImgPicture"].ToString();
                KodeFMaterial = string.IsNullOrEmpty(product.Rows[0]["KodeFMaterial"].ToString()) ? "" : " / " + product.Rows[0]["KodeFMaterial"].ToString() + "";

                // ===== FIX GAMBAR (SAFE) =====
                string basePath = "";
                try
                {
                    if (!string.IsNullOrEmpty(ImgPicture))
                    {
                        basePath = Path.GetDirectoryName(ImgPicture);
                        if (!string.IsNullOrEmpty(basePath))
                        {
                            basePath += "\\";
                        }
                    }
                }
                catch { }

                if (File.Exists(ImgPicture))
                {
                    Upload.Image(imgpicture, ImgPicture);
                }
                else if (File.Exists(basePath + myProto + product.Rows[0]["KodeFColor"].ToString() + ".jpg"))
                {
                    string path = basePath + myProto + product.Rows[0]["KodeFColor"].ToString() + ".jpg";
                    Upload.Image(imgpicture, path);
                }
                else
                {
                    string path = basePath + myProto + ".jpg";
                    Upload.Image(imgpicture, path);
                }
                // ===== END FIX =====

                brand.InnerHtml = product.Rows[0]["brand"].ToString();
                tgl.InnerHtml = product.Rows[0]["Tgl"].ToString();
                productitem.InnerHtml = product.Rows[0]["nmItem"].ToString() + " (" + product.Rows[0]["KodeItem"].ToString() + ")";
                productcategory.InnerHtml = product.Rows[0]["nmCategory"].ToString() + " (" + product.Rows[0]["nmKodeCat"].ToString() + ")";
                productlevel.InnerHtml = product.Rows[0]["nmLevel"].ToString() + " (" + product.Rows[0]["nmLevelKode"].ToString() + ")";
                stonedist.InnerHtml = product.Rows[0]["nmStoneDist"].ToString() + " (" + product.Rows[0]["KodeStoneDist"].ToString() + ")";
                framematerial.InnerHtml = product.Rows[0]["nmFrameMaterial"].ToString() + " (" + KodeFMaterial + ")";
                framefinishing.InnerHtml = product.Rows[0]["nmFrameFinishing"].ToString() + " (" + product.Rows[0]["kodeFinishing"].ToString() + ")";
                processcons.InnerHtml = product.Rows[0]["nmProsesCons"].ToString() + " (" + product.Rows[0]["KodeProsesCons"].ToString() + ")";
                framecolor.InnerHtml = product.Rows[0]["nmFrameColor"].ToString() + " (" + product.Rows[0]["KodeFColor"].ToString() + ")";

                grossweight.InnerHtml = Str.Number3(Convert.ToDecimal(product.Rows[0]["GrossWeight"]));
                stonecarat.InnerHtml = Str.Number3(Convert.ToDecimal(product.Rows[0]["StoneCarat"]));
                stoneweight.InnerHtml = Str.Number3(Convert.ToDecimal(product.Rows[0]["StoneWeight"]));
                nettoweight.InnerHtml = Str.Number3(Convert.ToDecimal(product.Rows[0]["NettoWeight"]));

                dimensid.InnerHtml = Str.Number2(Convert.ToDecimal(product.Rows[0]["DimensiD"]));
                dimensip.InnerHtml = Str.Number2(Convert.ToDecimal(product.Rows[0]["DimensiP"]));
                dimensil.InnerHtml = Str.Number2(Convert.ToDecimal(product.Rows[0]["DimensiL"]));
                dimensir.InnerHtml = Str.Number2(Convert.ToDecimal(product.Rows[0]["DimensiR"]));
                dimensir2.InnerHtml = Str.Number2(Convert.ToDecimal(product.Rows[0]["DimensiR2"]));

                designcategory.InnerHtml = designCoeg.Rows[0]["nmCat"] + " (" + designCoeg.Rows[0]["KodeCat"] + ")";
                designconcept.InnerHtml = designCoeg.Rows[0]["nmConcept"] + " (" + designCoeg.Rows[0]["KodeConcept"] + ")";
                designprocess.InnerHtml = designCoeg.Rows[0]["nmProcess"] + " (" + designCoeg.Rows[0]["KodeProcess"] + ")";
                targetage.InnerHtml = designCoeg.Rows[0]["nmAge"] + " (" + designCoeg.Rows[0]["KodeAge"] + ")";
                targetgender.InnerHtml = designCoeg.Rows[0]["nmGender"] + " (" + designCoeg.Rows[0]["KodeGender"] + ")";

                naming.InnerHtml = designCoeg.Rows[0]["Naming"].ToString();
                designket.InnerHtml = designKet(Convert.ToInt32(designCoeg.Rows[0]["Setitem"]), Convert.ToInt32(designCoeg.Rows[0]["Special"]));
                grafir.InnerHtml = designCoeg.Rows[0]["Grafir"].ToString();
                finishing.InnerHtml = designFinishing(finishHim);

                fillStone(designCoeg.Rows[0]["Nomor"].ToString());
            }
            else
            {
                brand.InnerHtml = "";
                tgl.InnerHtml = "";
                productitem.InnerHtml = "";
                productcategory.InnerHtml = "";
                productlevel.InnerHtml = "";
                stonedist.InnerHtml = "";
                framematerial.InnerHtml = "";
                framefinishing.InnerHtml = "";
                processcons.InnerHtml = "";
                framecolor.InnerHtml = "";

                grossweight.InnerHtml = "0";
                stonecarat.InnerHtml = "0";
                stoneweight.InnerHtml = "0";
                nettoweight.InnerHtml = "0";

                dimensid.InnerHtml = "0";
                dimensip.InnerHtml = "0";
                dimensil.InnerHtml = "0";
                dimensir.InnerHtml = "0";
                dimensir2.InnerHtml = "0";

                designcategory.InnerHtml = "";
                designconcept.InnerHtml = "";
                designprocess.InnerHtml = "";
                targetage.InnerHtml = "";
                targetgender.InnerHtml = "";

                naming.InnerHtml = "";
                designket.InnerHtml = "";
                grafir.InnerHtml = "";
                finishing.InnerHtml = "";

                fillStone("");
            }

            header_nomor.InnerHtml = myProto;
            designnomor.InnerHtml = myProto;

            pricefrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceFrom).ToString("N0") + "</p>";
            priceupto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceUpTo).ToString("N0") + "</p>";

        }
    }

    //DJ Info
    protected string designKet(int setitem, int setspecial)
    {
        string x = string.Empty;

        if (setitem == 1)
        {
            if (x != "") x += ", ";
            x += "Set Item";
        }
        if (setspecial == 1)
        {
            if (x != "") x += ", ";
            x += "Special DJ";
        }

        return x;
    }
    protected string designFinishing(DataTable finishHim)
    {
        string x = string.Empty;

        foreach (DataRow r in finishHim.Rows)
        {
            if (x != "") x += "<br />";
            x += r["nmFinish"].ToString() + " (" + r["KodeFinish"].ToString() + ")";
        }

        return x;
    }
    //Stone
    protected void fillStone(string Nomor)
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;


        string plunumber = Nomor;
        string stonemodelist = "i"; // b:bandroll / c:certificate
        string plutype = "dj"; // dj or ld

        string spname = "sp_get_stone_information_for_bandroll_certificate";
        string query = " exec " + spname
                     + " '" + stonemodelist + "'"
                     + ",'" + plunumber + "'"
                     + ",'" + plutype + "'"
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt.Rows.Count > 0)
        {
            int TotalButir = 0;
            decimal TotalCarat = 0m;
            decimal CaratButir = 0m;
            //string Keterangan = "";
            string ShapeName = "";
            string Colour = "";
            string Clarity = "";
            string GIA = "";
            string StoneBrand = "";

            int ST_Butir = 0;
            decimal ST_Carat = 0;

            string xStoneTypeCursor = "";
            string xStoneType = "";

            //RESULT
            int index = 1;

            foreach (DataRow dtRow in dt.Rows)
            {
                xStoneType = (dtRow["StoneType"]).ToString();
                if (xStoneType != xStoneTypeCursor)
                {
                    GT_Butir += ST_Butir;
                    GT_Carat += ST_Carat;

                    if (index > 1)
                    {
                        Sub_Total(ST_Butir, ST_Carat, false);
                    }

                    ST_Butir = 0;
                    ST_Carat = 0;

                    xStoneTypeCursor = xStoneType;
                    if (xStoneTypeCursor == "1A") { Sub_Header(0); }
                    if (xStoneTypeCursor == "1B") { Sub_Header(1); }
                    if (xStoneTypeCursor == "2") { Sub_Header(2); }
                    if (xStoneTypeCursor == "3") { Sub_Header(3); }
                    if (xStoneTypeCursor == "4") { Sub_Header(4); }
                    if (xStoneTypeCursor == "5") { Sub_Header(5); }
                }

                TotalButir = Convert.ToInt32(dtRow["TotalButir"]);
                TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"]);
                CaratButir = Convert.ToDecimal(dtRow["CaratButir"]);
                ShapeName = (dtRow["ShapeName"]).ToString();
                Colour = (dtRow["Colour"]).ToString();
                Clarity = (dtRow["Clarity"]).ToString();
                GIA = (dtRow["GIA"]).ToString();
                StoneBrand = (dtRow["BrandName"]).ToString();

                TableRow tr = new TableRow();
                TableCell td;
                string sarine = string.Empty;
                string linkSarine = string.Empty;
                string grade = string.Empty;
                if (GIA != null)
                {
                    try
                    {
                        string q = "SELECT m_report, m_digital, m_grade FROM table_sarin WHERE m_gia = '" + GIA + "'";
                        DataTable dts = Db.Rs(q, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

                        if (dts.Rows.Count > 0)
                        {
                            DataRow row = dts.Rows[0];
                            sarine = row["m_report"].ToString();
                            linkSarine = row["m_digital"].ToString();
                            grade = row["m_grade"].ToString();
                        }
                    }
                    catch { }
                }

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(ShapeName, ((GIA.Trim() != "") ? GIA.Trim() : ""), StoneBrand, sarine, linkSarine, grade);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Colour;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Clarity;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_CaratButir(CaratButir);
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalButir(TotalButir, ref ST_Butir);
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(TotalCarat, ref ST_Carat);
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                if (dt.Rows.Count == index - 1)
                {
                    GT_Butir += ST_Butir;
                    GT_Carat += ST_Carat;

                    Sub_Total(ST_Butir, ST_Carat, false);
                }


            }


        }

        //Stone
        //fillStone1A(0, ref GT_Butir, ref GT_Carat, d);
        //fillStone1B(1, ref GT_Butir, ref GT_Carat, d);
        //fillStone2(2, ref GT_Butir, ref GT_Carat, d);
        //fillStone3(3, ref GT_Butir, ref GT_Carat, d);
        //fillStone4(4, ref GT_Butir, ref GT_Carat, d);
        //fillStone5(5, ref GT_Butir, ref GT_Carat, d);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone1A;

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
            else if (r.Stone1A.Nama.Contains("B4") && r.StockProductDJ.LocWarehouse.CompanyBrand.NamaKode == "P")
                Parcel104 = "RED";
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone1B;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone1B.Parcel101.Nama, (r.GIA != null ? r.GIA : ""), r.Stone1B.Parcel106.Nama);
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone2;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);



            td = new TableCell();
            td.Text = TD_Nama(r.Stone2.Parcel201.Nama, (r.GIA != null ? r.GIA : ""));
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone3;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);



            td = new TableCell();
            td.Text = TD_Nama(r.Stone3.Parcel301.Nama, (r.GIA != null ? r.GIA : ""));
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone4;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);



            td = new TableCell();
            td.Text = TD_Dimensi(r.DimensiP);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Stone4.Parcel401.Nama, (r.GIA != null ? r.GIA : ""));
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, StockProductDJ d)
    {
        //SELECT
        var rs = d.StockProductDJ_Stone5;

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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);



            td = new TableCell();
            td.Text = TD_Nama(r.Stone5.Parcel501.Nama, (r.GIA != null ? r.GIA : ""));
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
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Carat;

            Sub_Total(ST_Butir, ST_Carat, false);
        }
    }

    //Custom Row
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
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
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
    //protected string TD_Nama(string Nama)
    //{
    //    string x = string.Empty;

    //    x += "<p><strong>" + Nama + "</strong></p>";

    //    return x;
    //}
    protected string TD_Nama(string Nama, string GIA, string BrandStone = "", string sarine = "", string linkSarine = "", string grade = "")
    {
        string x = string.Empty;
        string UrlGIA = ConfigurationManager.AppSettings["UrlGIA"];
        UrlGIA = UrlGIA + "locale=en_US&reportno=";

        x += "<p><strong>" + Nama + "</strong></p>";

        if (BrandStone != null && BrandStone.Trim() != "")
            x += "<p class=\"text-sm\" style=\"color:purple; font-weight:bold;\">" + BrandStone + "</p>";

        if (GIA != null && GIA.Trim() != "")
            x += "<p class=\"text-muted text-sm\">GIA : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + UrlGIA + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";

        if (sarine != null && sarine.Trim() != "")
            x += "<p class=\"text-muted text-sm\">Sarine : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + linkSarine + "\">" + sarine + "</a></p>";

        if (grade != null && grade.Trim() != "")
            x += "<p class=\"text-muted text-sm\">Grade : <label>" + grade + "</label></p>";
        return x;
    }
    protected string TD_Dimensi(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number2(Dimensi) + "</p>";

        return x;
    }
    protected string TD_CaratButir(decimal? CaratButir)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

        return x;
    }
    protected string TD_TotalButir(int? TotalButir, ref int ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }
    protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }
    protected DataSet QuerySPDataSet(string query, string connectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlTransaction trans;
            trans = conn.BeginTransaction();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
                sda.SelectCommand.CommandTimeout = 3000;
                sda.Fill(ds);
                sda.Dispose();
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
                trans.Rollback();
                conn.Close();
            }
        }
        return ds;
    }
}