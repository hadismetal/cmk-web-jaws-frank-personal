using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
			Ddl.Store(lokasi);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";
			reselltype.SelectedValue = "0";
            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {	
		string query = "sp_report_resell_detail_new";
        string[] start = dari.Text.Split('-');
        string[] end = sampai.Text.Split('-');

        string startdate = start[2] + "-" + start[1] + "-" + start[0];
        string enddate = end[2] + "-" + end[1] + "-" + end[0];
		
		DataTable dt = Db.Rs("EXEC " + query + " '"
            + kw.Text + "','"
            + startdate + "','"
            + enddate + "','"
            + lokasi.SelectedValue + "','" 
            + status.SelectedValue + "','" 
            + producttype.SelectedValue + "'," 
			+ reselltype.SelectedValue + "," 
            + productitem.SelectedValue + "," 
            + productcategory.SelectedValue + ", 0" , Db.Cnn_DB);

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
                td.Text = Convert.ToInt64(dr["HargaPembelianAsal"]).ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToInt64(dr["HargaDPPPembelianAsal"]).ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Convert.ToInt64(dr["PPNAsal"]).ToString("N0");
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
                td.Text = Convert.ToInt32(dr["HargaResell"]).ToString("N0");
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

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
            lokasi.Text = Request.QueryString["lokasi"];
            producttype.SelectedValue = Request.QueryString["producttype"];
			reselltype.SelectedValue = Request.QueryString["reselltype"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;
		href += "&reselltype=" + reselltype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ResellRekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;
		href += "&reselltype=" + reselltype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "ResellRekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;
		href += "&reselltype=" + reselltype.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(Resell r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Resell_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        if (r.Draft == true)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Resell_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        if ((r.StatusVoid == null || r.StatusVoid == 0) && r.Draft == false)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Resell_Void.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Void";
            x += "</a>";
        }
        if (r.Draft == false && r.StatusVoid != 1)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-info\" target=\"_blank\" href=\"/OutletSales/Page/Create/Resell/Print_Receipt.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-print\"></i> ";
            x += "  Print";
            x += "</a>";
        }
        return x;
    }
}