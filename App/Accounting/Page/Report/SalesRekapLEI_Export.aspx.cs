using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Summary : System.Web.UI.Page
{
    protected string kw { get { return Request.QueryString["kw"]; } }
    protected string productitem { get { return Request.QueryString["productitem"]; } }
    protected string goldlevel { get { return Request.QueryString["goldlevel"]; } }
    protected string goldmodel { get { return Request.QueryString["goldmodel"]; } }
    protected string filter { get { return Request.QueryString["filter"]; } }
    protected string status { get { return Request.QueryString["status"]; } }
    protected string dari { get { return Request.QueryString["dari"]; } }
    protected string sampai { get { return Request.QueryString["sampai"]; } }
    protected string tipelokasi { get { return Request.QueryString["tipelokasi"]; } }
    protected string lokasi { get { return Request.QueryString["lokasi"]; } }
    protected string payment { get { return Request.QueryString["payment"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=SalesRekapLEI_Export.xls");
            fillTable(sender, e);
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string startdate = Convert.ToDateTime(dari).ToString("yyyy-MM-dd");
        string enddate = Convert.ToDateTime(sampai).ToString("yyyy-MM-dd");


        string query = "EXEC sp_salesdetail_lei '" + kw + "'," +
                       " '" + startdate + "', " +
                       " '" + enddate + "', " +
                       " '" + status + "', " +
                       " '" + payment + "', " +
                       " '" + productitem + "', " +
                       " '" + goldlevel + "', " +
                       " '" + goldmodel + "', " +
                       " '" + filter + "', " +
                       " '" + tipelokasi + "', " +
                       " '" + lokasi + "', " +
                       " '1' "
                       ;
        dt = Db.Rs(query, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("es-ES", false).NumberFormat;
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
                td.Text = "<p style='font-family:consolas'>" + Convert.ToDateTime(dr["OperatorTgl"]).ToString("dd-MM-yyyy HH:mm tt") + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["NoSO"].ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = "<p style='font-family:consolas'>" + dr["NoReferensi"].ToString() + "</p>";
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
                td.Text = "<p style='font-family:consolas'>" + dr["ProductItem"].ToString() + "</p>";
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
                td.Text = "<p style='font-family:consolas'>" + Convert.ToInt32(dr["Qty"]).ToString() + "</p>";
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
            tr2.Style.Add("background-color", "powderblue");
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
}