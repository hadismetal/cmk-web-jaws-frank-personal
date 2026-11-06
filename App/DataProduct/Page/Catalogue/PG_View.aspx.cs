using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class Page_Register_PG_Submit : System.Web.UI.Page
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
    protected decimal myMin { get { return Convert.ToDecimal(Request.QueryString["min"]); } }
    protected decimal myMax { get { return Convert.ToDecimal(Request.QueryString["max"]); } }
    protected decimal myWeightMin { get { return decimal.Parse(Request.QueryString["weightmin"], System.Globalization.CultureInfo.InvariantCulture); } }
    protected decimal myWeightMax { get { return decimal.Parse(Request.QueryString["weightmax"], System.Globalization.CultureInfo.InvariantCulture); } }
    protected decimal mySizeMin { get { return decimal.Parse(Request.QueryString["sizemin"], System.Globalization.CultureInfo.InvariantCulture); } }
    protected decimal mySizeMax { get { return decimal.Parse(Request.QueryString["sizemax"], System.Globalization.CultureInfo.InvariantCulture); } }
    protected string myKwddl { get { return Request.QueryString["kwddl"]; } }
    protected string myKdplu { get { return Request.QueryString["kdplu"]; } }
    //protected decimal myWeightMax { get { return Convert.ToDecimal(Request.QueryString["weightmax"]); } }



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

        string query1 = "EXEC sp_katalogdj '"+myProto+"', 'PG','1'";
		DataSet ds = QuerySPDataSet(query1, Db.Cnn_DB);  
			

        string Lokasi = "";
        string table = "";
        string ProtoNomor = myProto;
        string KodeProduksi = "";
        decimal dPriceFrom = 0m;
        decimal dPriceUpTo = 0m;
        decimal dGrossWeightFrom = 0m;
        decimal dGrossWeightTo = 0m;
        decimal dKadarFrom = 0m;
        decimal dKadarTo = 0m;
        decimal dRingsizeFrom = 0m;
        decimal dRingsizeTo = 0m;
        int index = 0;
        DataTable header;
        DataTable product;


        table = "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\">";
        table += "<tr><td width=\"50\"><b>No.</b></td><td width=\"100\"><b>Location Type</b></td><td width=\"210\"><b>Location</b></td><td width=\"150\"><b>Kode PLU</b></td><td width=\"150\"><b>Price</b></td><td width=\"100\"><b>Weight</b></td><td width=\"100\"><b>Size</b></td><td width=\"100\"><b>Level</b></td><td width=\"100\"><b>Colour</b></td><td width=\"100\"><b>Kadar</b></td><td width=\"100\"><b>Status</b></td><td width=\"150\"><b>DP Stock</b></td><td width=\"150\"><b>Harga Promo/One Price</b></td><td width=\"150\"><b>Harga PWP</b></td><td width=\"210\"><b>Location To</b></td></tr>";

        if (ds.Tables.Count>0)
        {
             header = ds.Tables[0];
            product = ds.Tables[1]; 
        foreach (DataRow r in header.Rows)
        {
            if (Convert.ToDecimal(r["TotalHargaJual"]) < dPriceFrom || dPriceFrom == 0m) { dPriceFrom = Convert.ToDecimal(r["TotalHargaJual"]); }
            if (Convert.ToDecimal(r["TotalHargaJual"]) > dPriceUpTo) { dPriceUpTo = Convert.ToDecimal(r["TotalHargaJual"]); }

            if (Convert.ToDecimal(r["GrossWeight"]) < dGrossWeightFrom || dGrossWeightFrom == 0m) { dGrossWeightFrom = Convert.ToDecimal(r["GrossWeight"]); }
            if (Convert.ToDecimal(r["GrossWeight"]) > dGrossWeightTo) { dGrossWeightTo = Convert.ToDecimal(r["GrossWeight"]); }

            if (Convert.ToDecimal(r["KadarLogam"]) < dKadarFrom || dKadarFrom == 0m) { dKadarFrom = Convert.ToDecimal(r["KadarLogam"]); }
            if (Convert.ToDecimal(r["KadarLogam"]) > dKadarTo) { dKadarTo = Convert.ToDecimal(r["KadarLogam"]); }

            if (Convert.ToDecimal(r["DimensiR"]) < dRingsizeFrom || dRingsizeFrom == 0m) { dRingsizeFrom = Convert.ToDecimal(r["DimensiR"]); }
            if (Convert.ToDecimal(r["DimensiR"]) > dRingsizeTo) { dRingsizeTo = Convert.ToDecimal(r["DimensiR"]); }

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
                string color = "black";
                string color2 = "black";
                string color3 = "black";
                string color4 = "black";
                string colorPromo = "black";
                string colorPWP = "black";

                if ((myMin <= Convert.ToDecimal(r["TotalHargaJual"]) && myMax >= Convert.ToDecimal(r["TotalHargaJual"])))
                {
                    color = "blue";

                    if ((myWeightMin <= Convert.ToDecimal(r["GrossWeight"]) && myWeightMax >= Convert.ToDecimal(r["GrossWeight"])))
                    {
                        color3 = "blue";

                        if ((mySizeMin <= Convert.ToDecimal(r["DimensiR"]) && mySizeMax >= Convert.ToDecimal(r["DimensiR"])))
                        {
                            color4 = "blue";
                        }
                        else
                        {
                            color = "black";
                            color3 = "black";
                            color4 = "black";
                        }

                    }
                    else
                    {
                        color = "black";
                        color3 = "black";
                    }

                }
                else
                {
                    if ((myWeightMin <= Convert.ToDecimal(r["GrossWeight"]) && myWeightMax >= Convert.ToDecimal(r["GrossWeight"])))
                    {
                        color3 = "blue";

                        if ((mySizeMin <= Convert.ToDecimal(r["DimensiR"]) && mySizeMax >= Convert.ToDecimal(r["DimensiR"])))
                        {
                            color4 = "blue";
                        }
                        else
                        {
                            color = "black";
                            color4 = "black";
                        }

                    }
                    else
                    {
                        if ((mySizeMin <= Convert.ToDecimal(r["DimensiR"]) && mySizeMax >= Convert.ToDecimal(r["DimensiR"])))
                        {
                            color4 = "blue";
                        }
                        else
                        {
                            color = "black";
                            color3 = "black";
                            color4 = "black";
                        }
                    }
                }

                
                if (promoHarga > 0 &&
                    myMin <= promoHarga &&
                    myMax >= promoHarga)
                {
                    colorPromo = "blue";
                }

                
                if (hargaPWP > 0 &&
                    myMin <= hargaPWP &&
                    myMax >= hargaPWP)
                {
                    colorPWP = "blue";
                }

                table += "<tr>";
                table += "<td>" + index + ".</td>";
                table += "<td>" + tipeLokasi + "</td>";
                table += "<td>" + r["NamaLokasi"] + "</td>";
                table += "<td style='color:" + color2 + ";'>" + r["Nomor"].ToString() + "</td>";                
                table += "<td style='text-align:center; color:" + color + ";'>" + Str.Number0(Convert.ToDecimal(r["TotalHargaJual"])) + "</td>";
                table += "<td style='text-align:center; color:" + color3 + ";'>" + Str.Number2(Convert.ToDecimal(r["GrossWeight"])) + "</td>";
                table += "<td style='text-align:center; color:" + color4 + ";'>" + Str.Number2(Convert.ToDecimal(r["DimensiR"])) + "</td>";
                table += "<td>" + r["GoldLevel"].ToString() + "</td>";
                table += "<td>" + r["Color"].ToString() + "</td>";
                table += "<td style='text-align:center;'>" + Str.Number2(Convert.ToDecimal(r["KadarLogam"])) + "%</td>";
                table += "<td>" + r["StatusBarang"].ToString() + "</td>";
                table += "<td>" + (r.IsNull("NomorDP") ? "-" : r["NomorDP"].ToString()) + "</td>";
                table += "<td style='text-align:right; color:" + colorPromo  + ";'>" + (promoHarga == 0m ? "-" : Str.Number0(promoHarga)) + "</td>";
                table += "<td style='text-align:right; color:" + colorPWP + ";'>" + (hargaPWP == 0m ? "-" : Str.Number0(hargaPWP)) + "</td>";
                table += "<td>" + r["NamaTujuan"] + "</td>";
                table += "</tr>";

            
        }

       

        table += "</table><br />";

        lokasi.Text = (index > 0) ? "<div style=\"float: left;\">" + table + "</div>" : "";

       
            if (product.Rows.Count>0){
                //Header
                header_nomor.InnerHtml = myProto;

                //Trans. Form
                brand.InnerHtml = product.Rows[0]["Brand"].ToString();
                tgl.InnerHtml = product.Rows[0]["Tgl"].ToString();
                               
                ringsizefrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dRingsizeFrom).ToString("N2") + "</p>";
                ringsizeto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dRingsizeTo).ToString("N2") + "</p>";


                kadarfrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dKadarFrom).ToString("N2") + "</p>";
                kadarto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dKadarTo).ToString("N2") + "</p>";


                weightfrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dGrossWeightFrom).ToString("N2") + "</p>";
                weightto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dGrossWeightTo).ToString("N2") + "</p>";

                pricefrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceFrom).ToString("N0") + "</p>";
                priceupto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceUpTo).ToString("N0") + "</p>";

                //Picture
                Upload.Image(imgpicture, product.Rows[0]["ImgPicture"].ToString());
            }  
            else
            {
                //Header
                header_nomor.InnerHtml = myProto;

                //Trans. Form
                brand.InnerHtml = product.Rows[0]["Brand"].ToString();
                tgl.InnerHtml = product.Rows[0]["Tgl"].ToString();
                               
                ringsizefrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dRingsizeFrom).ToString("N2") + "</p>";
                ringsizeto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dRingsizeTo).ToString("N2") + "</p>";


                kadarfrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dKadarFrom).ToString("N2") + "</p>";
                kadarto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dKadarTo).ToString("N2") + "</p>";


                weightfrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dGrossWeightFrom).ToString("N2") + "</p>";
                weightto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dGrossWeightTo).ToString("N2") + "</p>";

                pricefrom.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceFrom).ToString("N0") + "</p>";
                priceupto.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(dPriceUpTo).ToString("N0") + "</p>";

                //Picture
                Upload.Image(imgpicture, product.Rows[0]["ImgPicture"].ToString());
            }  
        }
            
    }


    //DJ Info
    protected string designKet(StockProductDJ_CharDesign d)
    {
        string x = string.Empty;

        if (d.SetItem)
        {
            if (x != "") x += ", ";
            x += "Set Item";
        }
        if (d.Special)
        {
            if (x != "") x += ", ";
            x += "Special DJ";
        }

        return x;
    }
    protected string designFinishing(StockProductDJ d)
    {
        string x = string.Empty;

        var rs = from p in d.StockProductDJ_Finishing select p;
        foreach (var r in rs)
        {
            if (x != "") x += "<br />";
            x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
        }

        return x;
    }
    //Stone
    protected void fillStone(StockProductDJ d)
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;


        string plunumber = d.Nomor;
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

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(ShapeName, ((GIA.Trim() != "") ? GIA.Trim() : ""), StoneBrand);
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

                //tb.Rows.Add(tr);

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

        //    tb.Rows.Add(tr);
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

           // tb.Rows.Add(tr);
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

        //    tb.Rows.Add(tr);
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

          //  tb.Rows.Add(tr);
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

          //  tb.Rows.Add(tr);
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

          //  tb.Rows.Add(tr);
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
       // td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

       // tb.Rows.Add(tr);
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

        //tb.Rows.Add(tr);
    }

    //Display Column
    //protected string TD_Nama(string Nama)
    //{
    //    string x = string.Empty;

    //    x += "<p><strong>" + Nama + "</strong></p>";

    //    return x;
    //}
    protected string TD_Nama(string Nama, string GIA, string BrandStone = "")
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        if (BrandStone != null && BrandStone.Trim() != "")
            x += "<p class=\"text-sm\" style=\"color:purple; font-weight:bold;\">" + BrandStone + "</p>";

        if (GIA != null && GIA.Trim() != "")
            x += "<p class=\"text-muted text-sm\">GIA : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";

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
			catch(Exception ex)
			{
				string ErrorMessage = ex.Message.ToString();
				trans.Rollback();
				conn.Close();
			}
		}
		return ds;
	}
}