using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DJ_DetailExportNew : System.Web.UI.Page
{
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int warehouse { get { return Convert.ToInt32(Request.QueryString["cabang"]); } }  // cabang.SelectedValue; // id lokasi to
    protected int brand { get { return Convert.ToInt32(Request.QueryString["brand"]); } } // brand.SelectedValue; // brand company
    protected string dari { get { return Request.QueryString["dari"]; } } // datepicker.Text.Trim(); // period from
    protected string sampai { get { return Request.QueryString["sampai"]; } } // datepicker2.Text.Trim(); // period to
    protected string keywords { get { return Request.QueryString["kw"]; } } // kw.Text.Trim(); // keyword

    protected string orderby { get { return Request.QueryString["orderby"]; } } // orderby.SelectedValue; // orderby 
    protected int supplier { get { return Convert.ToInt32(Request.QueryString["supplier"]); } } // supplier.SelectedValue; // supplier 
    protected string sk { get { return Request.QueryString["sk"]; } } // sk.SelectedValue; // stock keeper 
    protected int suppliergroup { get { return Convert.ToInt32(Request.QueryString["suppliergroup"]); } } // suppliergroup.SelectedValue; // supplier group

    protected int level { get { return Convert.ToInt32(Request.QueryString["productlevel"]); } } // productlevel.SelectedValue; // product level
    protected int item { get { return Convert.ToInt32(Request.QueryString["productitem"]); } }  // productitem.SelectedValue; // product item
    protected int category { get { return Convert.ToInt32(Request.QueryString["productcategory"]); } } // productcategory.SelectedValue; // product category

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_Detail_ExportExcelNew.xls");
            //Generate
            fillTable(sender, e);
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        System.Web.HttpResponse response = Response;
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        DateTime start = Convert.ToDateTime(dari);
        DateTime end = Convert.ToDateTime(sampai);
        string s_sk = sk != "0" ? sk : "";

        string spname = "detailDJ_Receive ";
        string query = " exec " + spname
                        + brand + ", "
                        + warehouse + ", "
                        + item + ", "
                        + category + ", "
                        + level + ", "
                        + "'" + s_sk + "', "
                        + supplier + ", "
                        + suppliergroup + ", "
                        + "'" + keywords + "', "
                        + "'" + start.ToString("yyyy-MM-dd") + "', "
                        + "'" + end.ToString("yyyy-MM-dd")  + "', "
                        + orderby;


        DataSet ds = new DataSet();
        ds = QuerySPDataSet(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        DataTable dt = ds.Tables["data"];
        DataTable total = ds.Tables["data1"];

        if (dt.Rows.Count == 0) { TableNoResult(tb); }
        else
        {
            /// header table
            TableHeaderRow htr = new TableHeaderRow();
            TableHeaderCell htd;

            htd = new TableHeaderCell();
            htd.Text = "No.";
            htd.Width = 50;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "No. PLU";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tanggal Approve";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stock Keeper";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Kode Karet";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Nomor Supplier";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Supplier";
            htd.Width = 180;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tipe Lokasi";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Request Outlet Nama";
            htd.Width = 180;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Dist.";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Item";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Category";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Segmen";
            htd.Width = 130;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Qty";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Net Weight";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Gross Weight";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Frame Price M";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Frame Price R";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO M";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO M Lot";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO R";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "RO M";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "RO R";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Finishing M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Finishing R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Ongkos Lain M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Ongkos Lain R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Setting M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Setting R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total M <span class='text-muted'>USD</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total R <span class='text-muted'>USD</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Mark Up <span class='text-muted'>%</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "H. Jual <span class='text-muted'>Rp</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "No. Validasi";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Product Order Nomor";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Parcel";
            htd.Width = 130;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "T. Butir";
            htd.Width = 70;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "T. Carat";
            htd.Width = 70;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "GIA";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Unit Price";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone M";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone R";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Keterangan";
            htd.Width = 150;
            htr.Cells.Add(htd);

            tb.Rows.Add(htr);

            int PrevID = 0;
            int index = 0;
            string plu = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                if (!response.IsClientConnected) break;

                decimal AllM = 0m;
                

                TableRow tr = new TableRow();
                TableCell td;
                if (plu == dr["Nomor"].ToString())
                {
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

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);
                }
                else 
                {
                    index++;

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Nomor"].ToString();
                    tr.Cells.Add(td);

                    /*td = new TableCell();
                    td.Text = Str.Tgl(r.Tgl);
                    tr.Cells.Add(td);*/

                    td = new TableCell();
                    td.Text = Convert.ToDateTime(dr["ApprovalTgl"]).ToString("D");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["StockKeeper"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProtoNomor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["SupplierNomor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["SupplierNama"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();

                    string locType = dr["LocationType"].ToString();

                    if (locType == "0")
                        td.Text = "Warehouse";
                    else if (locType == "1")
                        td.Text = "Outlet";
                    else if (locType == "2")
                        td.Text = "Exhibition";
                    else
                        td.Text = "-";

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["RequestOutletNama"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["StoneDist"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductItem"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductCategory"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Segment"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NettoWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FrameM"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FrameR"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOMLot"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["ROM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["ROR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FinishingM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FinishingR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["OngkosLainCOGS"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["OngkosLain"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["SettingM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["SettingR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["HargaMUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["HargaUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["MU"]).ToString("#,##0", nfi) + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.Text = Convert.ToDecimal(r.StockReceiveDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockReceiveDJ_PricingMU.HargaRupiahRound).ToString("N2");
                    td.Text = Convert.ToDecimal(dr["HargaRupiahRound"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["NoValidasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductOrderNomor"].ToString();
                    tr.Cells.Add(td);

                    plu = dr["Nomor"].ToString();
                }
                    /*td = new TableCell();
                    td.Text = rstone.Nama;
                    tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = dr["Parcel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(dr["TotalButir"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalCarat"]).ToString("N4", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + dr["GIA"].ToString() + "\" target=\"_blank\">" + dr["GIA"].ToString() + "</a>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaSatuan"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalHargaM"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalHargaR"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Keterangan"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            if (total.Rows.Count > 0)
            {
                DataRow dr = total.Rows[0];
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Grand Total</b>";
                td.ColumnSpan = 11;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + dr["Qty"].ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NettoWeight"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FrameM"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FrameR"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOMLot"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["ROM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["ROR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FinishingM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FinishingR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["OngkosLainM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["OngkosLainR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["SettingM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["SettingR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaMUSD"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaUSD"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaRupiahRound"]).ToString("#,##0", nfi) + "</b>";
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
                td.Text = "<b>" + Convert.ToInt32(dr["TotalButir"]).ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalCarat"]).ToString("N4", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaSatuan"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalHargaM"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalHargaR"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);


                tb.Rows.Add(tr);
            }
        }
    }

    /*protected void ExportToExcel(DataTable dt)
    {
        Microsoft.Office.Interop.Excel.Application excel;
        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        Microsoft.Office.Interop.Excel.Range excelCellrange;

        // Start Excel and get Application object.  
        excel = new Microsoft.Office.Interop.Excel.Application();
        // for making Excel visible  
        excel.Visible = false;
        excel.DisplayAlerts = false;
        // Creation a new Workbook  
        excelworkBook = excel.Workbooks.Add(Type.Missing);
        // Workk sheet  
        excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
        excelSheet.Name = "Test work sheet";

        if(dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Columns.Count;i++)
            {
                excelSheet.Cells[1, dt.Columns[i]] = dt.Columns[i].ColumnName; 
            }

            for(int i = 0; i < dt.Columns.Count;i++)
            {
                for(int j = 0; j < dt.Rows.Count; j++)
                {
                    excelSheet.Cells[j + 2, i + 1] = dt.Rows[j][i];
                }
            }
        }
        else
        {
            excelSheet.Cells[1, 1] = "Data Not Found.";
        }
    }*/
    public DataSet QuerySPDataSet(string query, string connectionString)
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
                sda.Fill(ds, "data");
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
	
    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td.Text = "No Data";
        //td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }
}