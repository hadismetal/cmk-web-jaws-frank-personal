using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_SalesRekapLM : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            dari.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1).ToString("dd-MM-yyyy");
            sampai.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        string[] darix = dari.Text.Split('-');
        string[] sampaix = sampai.Text.Split('-');

        string startdate = darix[2] + "-" + darix[1] + "-" + darix[0] + " 00:00:00.000";
        string enddate = sampaix[2] + "-" + sampaix[1] + "-" + sampaix[0] + " 23:59:00.000";
        string query = "EXEC sp_salesdetail_lm '" + kw.Text + "', '" + startdate + "','" + enddate + "' ," + status.SelectedValue + "," + tipelokasi.SelectedValue + "," + lokasi.SelectedValue + ",0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        int index = 1;
        foreach (DataRow dr in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDateTime(dr["Tgl"]).ToShortDateString() + "</p>";
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
            td.Text = "<p style='font-family:consolas'>" + dr["PLU"].ToString() + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + dr["ProductItem"].ToString() + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + dr["GoldModel"].ToString() + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + dr["GoldLevel"].ToString() + "</p>";
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
            td.Text = "<p style='font-family:consolas'>" + dr["Qty"].ToString() + "</p>";
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
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountNominal"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountPromo"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountProgramNominal"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountGift"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountBank"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountOther"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["DiscountRound"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + dr["Pph22Persen"].ToString() + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["Pph22"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["PriceExcludePPH22"]).ToString("N2", nfi) + "</p>";
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
            td.Text = "<p style='font-family:consolas'>" + Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi) + "</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);
            
            tb.Rows.Add(tr);
        }

    }
    protected void fillTableExcel(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.ContentType = "application/vnd.ms-excel";

        Response.AppendHeader("content-disposition", "attachment; filename=RekapLM_Export.xls");

        string[] darix = dari.Text.Split('-');
        string[] sampaix = sampai.Text.Split('-');

        string startdate = darix[2] + "-" + darix[1] + "-" + darix[0] + " 00:00:00.000";
        string enddate = sampaix[2] + "-" + sampaix[1] + "-" + sampaix[0] + " 23:59:00.000";
        string query = "EXEC sp_salesdetail_lm '" + kw.Text + "', '" + startdate + "','" + enddate + "' ," + status.SelectedValue + "," + tipelokasi.SelectedValue + "," + lokasi.SelectedValue + ",1 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        Response.Write("No");
        string tab = "\t";
        foreach (DataColumn dc in dt.Columns)
        {
            Response.Write(tab + dc.ColumnName);
            tab = "\t";
        }
        Response.Write("\n");

        int i;
        int rowindex = 1;
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write(rowindex);
            tab = "\t";
            for (i = 0; i < dt.Columns.Count; i++)
            {
                if (i >= 10 && i != 20) //Format Nominal
                {
                    Response.Write(tab + Convert.ToDecimal(dr[i]).ToString("N2", nfi));
                }
                else //ID
                {
                    Response.Write(tab + dr[i].ToString());
                }
                tab = "\t";
            }
            Response.Write("\n");
            rowindex++;
        }
        dt.Dispose();
        Response.End();

    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("Lokasi", "0"));
        }
        lokasi.Focus();
    }
}