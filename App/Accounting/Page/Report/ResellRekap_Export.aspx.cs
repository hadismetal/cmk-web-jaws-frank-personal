using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
	
	protected int item { get { return Convert.ToInt32(Request.QueryString["productitem"]); } }
	protected int kategori { get { return Convert.ToInt32(Request.QueryString["productcategory"]); } }
	protected string keyword { get { return Request.QueryString["kw"]; } }
	protected string start { get { return Request.QueryString["dari"]; } }
	protected string end { get { return Request.QueryString["sampai"]; } }
	protected string tipe { get { return Request.QueryString["producttype"]; } }
	protected string lokasi { get { return Request.QueryString["lokasi"]; } }
	protected string status { get { return Request.QueryString["status"]; } }
	protected string reselltype { get { return Request.QueryString["reselltype"]; } }
	
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=ResellRekap_Export.xls");
            fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
		string query = "sp_report_resell_detail_new";
        string[] start1 = start.Split('-');
        string[] end1 = end.Split('-');

        string startdate = start1[2] + "-" + start1[1] + "-" + start1[0];
        string enddate = end1[2] + "-" + end1[1] + "-" + end1[0];
		
		DataTable dt = Db.Rs("EXEC " + query + " '"
            + keyword + "','"
            + startdate + "','"
            + enddate + "','"
            + lokasi + "','" 
            + status + "','" 
            + tipe + "'," 
			+ reselltype + "," 
            + item + "," 
            + kategori + ", 1" , Db.Cnn_DB);

        int rowcount = dt.Rows.Count;
        if (rowcount > 0)
        {
            decimal Total = 0m;
            decimal TotalQty = 0m;
            decimal TotalBeli = 0m;
			decimal TotalDPPBeli = 0m;
			decimal TotalPPNBeli = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalNetWeight = 0m;
			decimal TotalBeratTimbang = 0m;

            int prev = 0;
            int index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(Convert.ToDateTime(dr["TglPembelian"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["LokasiPembelian"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["InvoiceAsal"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaPembelianAsal"]).ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaDPPPembelianAsal"]).ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(dr["PPNAsal"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(dr["KeteranganAsal"].ToString());
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(Convert.ToDateTime(dr["TglResell"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["LokasiResell"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["NoResell"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["CustomerNama"].ToString();
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = dr["KeteranganVoid"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["PLU"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Group"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Item"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Kategori"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(dr["Qty"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(dr["Butir"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["Carat"]).ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["GrossWeight"]).ToString("N4");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["NettWeight"]).ToString("N2");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TGPBuyback"]).ToString("N2");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToDecimal(dr["BeratTimbang"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaResell"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["KadarTukaranBeli"]) .ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaM"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaR"]).ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty++;
                Total += Convert.ToDecimal(dr["HargaResell"]);
                TotalBeli += Convert.ToDecimal(dr["HargaPembelianAsal"]);
				TotalDPPBeli += Convert.ToDecimal(dr["HargaDPPPembelianAsal"]);
				TotalPPNBeli += Convert.ToDecimal(dr["PPNAsal"]);
                TotalM += Convert.ToDecimal(dr["HargaM"]);
                TotalR += Convert.ToDecimal(dr["HargaR"]);
                TotalButir += Convert.ToInt32(dr["Butir"]);
                TotalCarat += Convert.ToDecimal(dr["Carat"]);
                TotalGrossWeight += Convert.ToDecimal(dr["GrossWeight"]);
                TotalNetWeight += Convert.ToDecimal(dr["NettWeight"]);
				TotalBeratTimbang += Convert.ToDecimal(dr["BeratTimbang"]);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 4;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalBeli.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + TotalDPPBeli.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + TotalPPNBeli.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>-</b>";
            td2.ColumnSpan = 10;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + TotalBeratTimbang.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalM).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalR).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}