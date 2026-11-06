using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_TGP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            dari.Text = DateTime.Now.ToString("dd-MM-yyyy");
            sampai.Text = DateTime.Now.ToString("dd-MM-yyyy");
            //fillTable(sender, e);
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        string[] startsdate = dari.Text.Split('-');
        string[] endsdate = sampai.Text.Split('-');
        string startdate = startsdate[2] + "-" + startsdate[1].PadLeft(2, '0') + "-" + startsdate[0].PadLeft(2, '0') + " 00:00:00";
        string enddate = endsdate[2] + "-" + endsdate[1].PadLeft(2, '0') + "-" + endsdate[0].PadLeft(2, '0') + " 23:59:00";
        string query = "EXEC sp_get_tgp_pg_per_tgl '" + startdate + "','" + enddate + "'";

        DataTable dt = Db.Rs(query, Db.Cnn_CS);

        int i = 1;
        foreach (DataRow dr in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.Text = i.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDateTime(dr["m_tanggal"]).ToString("dd-MM-yyyy");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDateTime(dr["m_tanggal"]).ToString("hh:mm");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_375_jual_chain_white"]).ToString("N2",nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_375_beli_chain_white"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_jual_chain_yellow"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_beli_chain_yellow"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_jual_chain_white"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_beli_chain_white"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_jual_non_chain_yellow"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_beli_non_chain_yellow"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_jual_non_chain_white"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_75_beli_non_chain_white"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_99_jual_non_chain"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_99_beli_non_chain"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_755_jual_cz_hala"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_755_jual_cz_hala"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_755_beli_cz_celeb"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_755_beli_cz_celeb"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_375_jual_cz"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_375_beli_cz"]).ToString("N2", nfi);
            tr.Cells.Add(td);
			
			td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_prelei_jual"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(dr["m_prelei_beli"]).ToString("N2", nfi);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            i++;
        }
    }
    protected void fillTableExcel(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.ContentType = "application/vnd.ms-excel";

        Response.AppendHeader("content-disposition", "attachment; filename=TGP_Export.xls");

        if (!string.IsNullOrEmpty(dari.Text) && !string.IsNullOrEmpty(sampai.Text))
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalDigits = 2;
            string[] listtanggal1 = dari.Text.Split('-');
            string[] listtanggal2 = sampai.Text.Split('-');

            string tanggalstart = listtanggal1[2] + "-" + listtanggal1[1].PadLeft(2, '0') + "-" + listtanggal1[0].PadLeft(2, '0') + " 00:00:00.000";
            string tanggalend = listtanggal2[2] + "-" + listtanggal2[1].PadLeft(2, '0') + "-" + listtanggal2[0].PadLeft(2, '0') + " 23:59:00.000";

            DataTable dt = new DataTable();
            string query = "EXEC sp_get_tgp_pg_per_tgl '" + tanggalstart + "','" + tanggalend + "'";

            dt = Db.Rs(query, Db.Cnn_CS);

            dt.Columns["m_375_jual_chain_white"].ColumnName = "37.5 % Chain (White) Jual";
            dt.Columns["m_375_beli_chain_white"].ColumnName = "37.5 % Chain (White) Beli";
            dt.Columns["m_tanggal"].ColumnName = "Tanggal";
            dt.Columns["m_75_jual_chain_yellow"].ColumnName = "75.0% Chain (Yellow) Jual";
            dt.Columns["m_75_beli_chain_yellow"].ColumnName = "75.0% Chain (Yellow) Beli";
            dt.Columns["m_75_jual_chain_white"].ColumnName = "75.0% Chain (White) Jual";
            dt.Columns["m_75_beli_chain_white"].ColumnName = "75.0% Chain (White) Beli";
            dt.Columns["m_75_jual_non_chain_yellow"].ColumnName = "75.0% Non-Chain (Yellow) Jual";
            dt.Columns["m_75_beli_non_chain_yellow"].ColumnName = "75.0% Non-Chain (Yellow) Beli";
            dt.Columns["m_75_jual_non_chain_white"].ColumnName = "75.0% Non-Chain (White) Jual";
            dt.Columns["m_75_beli_non_chain_white"].ColumnName = "75.0% Non-Chain (White) Beli";
            dt.Columns["m_99_jual_non_chain"].ColumnName = "99.9% Non-Chain Jual";
            dt.Columns["m_99_beli_non_chain"].ColumnName = "99.9% Non-Chain Beli";
            dt.Columns["m_755_jual_cz_hala"].ColumnName = "75.5% CZ Jewelry (Hala) Jual";
            dt.Columns["m_755_beli_cz_hala"].ColumnName = "75.5% CZ Jewelry (Hala) Beli";
            dt.Columns["m_755_jual_cz_celeb"].ColumnName = "75.5% CZ Jewelry (Celeb) Jual";
            dt.Columns["m_755_beli_cz_celeb"].ColumnName = "75.5% CZ Jewelry (Celeb) Beli";
            dt.Columns["m_375_jual_cz"].ColumnName = "37.5% CZ Jewelry Jual";
            dt.Columns["m_375_beli_cz"].ColumnName = "37.5% CZ Jewelry Beli";
			dt.Columns["m_prelei_jual"].ColumnName = "PRELEI JUAL";
			dt.Columns["m_prelei_beli"].ColumnName = "PRELEI BELI";

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
                    if (i != 0) //Format Nominal
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
    }
}