using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Summary : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        int Tipe = 0;

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(goldlevel);
            DLC.CharGoldModelAll2(goldmodel);

            var x = from n in db.PaymentType
                    where n.ID == 4 || n.ID == 11 || n.ID == 2
                    select new { n.ID, n.Nama };

            foreach (var r in x)
            {
                payment.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            if (Request.QueryString["lokasi"] != "")
            {
                LocationList(lokasi, Tipe);
                lokasi.SelectedValue = Request.QueryString["lokasi"];
            }
            else
                LocationList(lokasi, 0);
            
            //Generate
            fillFilter();
            
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            productitem.SelectedValue = Request.QueryString["productitem"];
            goldlevel.SelectedValue = Request.QueryString["level"];
            goldmodel.SelectedValue = Request.QueryString["model"];
            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
            payment.SelectedValue = Request.QueryString["payment"];
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string startdate = Convert.ToDateTime(dari.Text).ToString("yyyy-MM-dd");
        string enddate = Convert.ToDateTime(sampai.Text).ToString("yyyy-MM-dd");


        string query = "EXEC sp_salesdetail_lei '"+kw.Text+"'," +
                       " '"+startdate+"', " +
                       " '"+enddate+"', " +
                       " '"+status.SelectedValue+"', " +
                       " '"+payment.SelectedValue+"', " +
                       " '"+productitem.SelectedValue+"', " +
                       " '"+goldlevel.SelectedValue+"', " +
                       " '"+goldmodel.SelectedValue+"', " +
                       " '"+filter.SelectedValue+"', " +
                       " '"+tipelokasi.SelectedValue+"', " +
                       " '"+lokasi.SelectedValue+"', " +
                       " '0' "
                       ;
        dt = Db.Rs(query, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        int RowCount = dt.Rows.Count;

        if (RowCount > 0)
        {
            decimal TotalHargaNet = 0m;
            decimal TotalHargaRupiah = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalHargaR = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
            decimal NettWeight = 0m;
            decimal LmWeight = 0m;
            decimal TotalNominalLakuEmas = 0m;
			decimal TotalNominalLakuEmasTgp = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalBayar1 = 0m;
            decimal TotalBayar2 = 0m;
            decimal TotalBayar3 = 0m;
            decimal TotalBayar4 = 0m;
            decimal TotalBayar5 = 0m;
            decimal TotalBayar6 = 0m;
            decimal TotalBayar7 = 0m;
            decimal TotalBayar8 = 0m;
            decimal TotalBayar9 = 0m;
            decimal TotalBayar10 = 0m;
            decimal TotalMDR = 0m;
            decimal TTerima = 0m;
			decimal TotalPrice_with_vat = 0m;
            decimal TotalPrice_ex_Vat = 0m;
            decimal TotalPPN = 0m;
			decimal TotalPrice_with_vat_tgp = 0m;
            decimal TotalPrice_ex_Vat_tgp = 0m;
            decimal TotalPPN_tgp = 0m;
            decimal TerimaSetelahMDR = 0m;
            int index = 1;
            
            foreach (DataRow dr in dt.Rows)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDateTime(dr["OperatorTgl"]).ToString("dd-MM-yyyy HH:mm tt") + "</p>" ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["NoSO"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["NoReferensi"].ToString() + "</p>" ;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["Lokasi"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["Tipe"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["PLU"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" +  dr["ProductItem"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" +  dr["ItemDesc"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["ProtoNomor"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["GoldLevel"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["GoldModel"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["CustomerNama"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["SalesNama"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" +  Convert.ToInt32(dr["Qty"]).ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["HargaM"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["Harga"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["Discount"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["DiscountProgram"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountGift"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountRound"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountPromo"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["TotalBayar"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["KadarTukaranBeli"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["NettWeight"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["LMWeight"]).ToString("N4", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["RateLM"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["NominalLakuEmas"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PriceWithPPN"]).ToString("N2", nfi) + "</p>";
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PriceExcVAT"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PPN"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["TGPJual"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["TGPBeli"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["NominalLakuEmasTGPJual"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PriceWithPPNTGPJual"]).ToString("N2", nfi) + "</p>";
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PriceExcVATTGPJual"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PPNTGPJual"]).ToString("N2", nfi) + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["JenisTransaksi"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["EDC"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                tb.Rows.Add(tr);

                TotalHargaNet += Convert.ToDecimal(dr["TotalBayar"]);
                TotalPrice_ex_Vat += Convert.ToDecimal(dr["PriceExcVAT"]);
				TotalPrice_ex_Vat_tgp += Convert.ToDecimal(dr["PriceExcVATTGPJual"]);
				TotalPrice_with_vat += Convert.ToDecimal(dr["PriceWithPPN"]);
				TotalPrice_with_vat_tgp += Convert.ToDecimal(dr["PriceWithPPNTGPJual"]);
                TotalPPN += Convert.ToDecimal(dr["PPN"]);
				TotalPPN_tgp += Convert.ToDecimal(dr["PPNTGPJual"]);
                TotalHargaRupiah += Convert.ToDecimal(dr["Harga"]);
                TotalHargaM += Convert.ToDecimal(dr["HargaM"]);
                TotalHargaR += Convert.ToDecimal(dr["Harga"]);
                LmWeight += Convert.ToDecimal(dr["LMWeight"]);
                TotalNominalLakuEmas += Convert.ToDecimal(dr["NominalLakuEmas"]);
				TotalNominalLakuEmasTgp += Convert.ToDecimal(dr["NominalLakuEmasTGPJual"]);
                TotalDiscountGift += Convert.ToDecimal(dr["DiscountGift"]);
                TotalDiscountRound += Convert.ToDecimal(dr["DiscountRound"]);
                TotalNetWeight += Convert.ToDecimal(dr["NettWeight"]);
            }

            TableRow tr2 = new TableRow();
            tr2.Style.Add("background-color","powderblue");
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>Total</b>";
            td2.ColumnSpan = 14;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + RowCount.ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalHargaM).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalHargaRupiah).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalDiscountGift).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalDiscountRound).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalDiscountPromo).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);
            
            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalHargaNet).ToString("N2", nfi) + "</b>";
            td2.Style.Add("color", "blue");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalNetWeight).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(LmWeight).ToString("N4", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalNominalLakuEmas).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPrice_with_vat).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPrice_ex_Vat).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPPN).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);
            
            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalNominalLakuEmasTgp).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPrice_with_vat_tgp).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPrice_ex_Vat_tgp).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b style='font-family:consolas'>" + Convert.ToDecimal(TotalPPN_tgp).ToString("N2", nfi) + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&level=" + goldlevel.Text;
        href += "&model=" + goldmodel.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }

    public void LocationList(DropDownList c1, int tipe, int locid = 0)
    {
        if (tipe == 0)
        {
            c1.Items.Add(new ListItem("Lokasi", "0"));
        }
        else if (tipe == 1)
        {
            DLC.OutletList(c1, locid);
        }
        else if (tipe == 2)
        {
            DLC.ExhibitionList(c1, locid);
        }

    }

    protected void export_ServerClick(object sender, EventArgs e)
    {
        string href = "SalesRekapLEI_Export.aspx?refreshPage=1";

        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&goldlevel=" + goldlevel.Text;
        href += "&goldmodel=" + goldmodel.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
}