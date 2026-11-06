using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DJ_Mutasi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        if (!IsPostBack)
        {
            DLC.CharProductItem(productItem);
            DLC.StockLocationList(cabang, Convert.ToInt32(locationType.SelectedValue));
            DLC.CharProductCategory(productCategory);
            DLC.CharProductLevel(productLevel);
            DLC.CharStoneDist(stoneDist);
            DLC.CompanyBrand(brand);


            setCharacter();
            datepicker.Text = Str.Tgl(DateTime.Today);
            datepicker2.Text = Str.Tgl(DateTime.Today);
        }
    }

    protected void locationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        cabang.Items.Clear();
        cabang.DataBind();
        cabang.Items.Add(new ListItem("Cabang :", "0"));
        DLC.StockLocationList(cabang, Convert.ToInt32(locationType.SelectedValue));
        //cabang.Enabled = true;
    }

    protected void closing_ServerClick(object sender, EventArgs e)
    {
        descHeader.Text = groupby.SelectedItem.Text;

        DateTime date = Convert.ToDateTime(datepicker.Text);
        date.AddMonths(1);

        string spname = "sp_mutasi_product";
        string query = " exec " + spname
                    + "'" + reverseDate(datepicker.Text, "from") + "', "
                    + "'" + reverseDate(datepicker2.Text, "to") + "', "
                    + locationType.SelectedValue + ", "
                    + cabang.SelectedValue + ", "
                    + productItem.SelectedValue + ", "
                    + productCategory.SelectedValue + ", "
                    + productLevel.SelectedValue + ", "
                    + stoneDist.SelectedValue + ", "
                    + "'" + plu.Text + "', "
                    + groupby.SelectedValue + ", "
                    + product.SelectedValue + ", "
                    + brand.SelectedValue + ", 1";


        DataSet ds = new DataSet();
        ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
    }
    protected void go_ServerClick(object sender, EventArgs e)
    {
        descHeader.Text = groupby.SelectedItem.Text;

        string spname = "sp_mutasi_product";
        string query = " exec " + spname
                     + "'" + reverseDate(datepicker.Text, "from") + "', "
                     + "'" + reverseDate(datepicker2.Text, "to") + "', "
                     + locationType.SelectedValue + ", "
                     + cabang.SelectedValue + ", "
                     + productItem.SelectedValue + ", "
                     + productCategory.SelectedValue + ", "
                     + productLevel.SelectedValue + ", "
                     + stoneDist.SelectedValue + ", "
                     + "'" + plu.Text + "', "
                     + groupby.SelectedValue + ", "
                     + product.SelectedValue + ", "
                     + brand.SelectedValue;


        DataSet ds = new DataSet();
        ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        DataTable dt = ds.Tables["data"];
        DataTable total = ds.Tables["data1"];

        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = dr["Description"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Awal"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Awal"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Awal"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Awal"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Awal"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Awal"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Receiving"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Receiving"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Receiving"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Incoming"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Incoming"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Incoming"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Resell"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Resell"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Resell"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Resell"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Resell"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Resell"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_TradeIn"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_TradeIn"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_TradeIn"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Outgoing"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Outgoing"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Outgoing"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Sales"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Sales"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Sales"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Sales"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Sales"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Sales"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Gift"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Gift"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Gift"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Gift"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Gift"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Gift"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Return"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Return"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Return"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Return"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Return"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Return"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Lebur"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Lebur"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Lebur"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Mutasi"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Mutasi"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Mutasi"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Qty_Opname"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Nett_Weight_Opname"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Gross_Weight_Opname"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["Butir_Opname"]).ToString("#,##0");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["Carat_Opname"]).ToString("#,##0.000");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaM_Opname"]).ToString("#,##0.00");
				tc.Style.Add("text-align", "right");
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);
            }

            foreach (DataRow last in total.Rows)
            {
                tr = new TableRow();
                tc = new TableCell();
                tc.Text = last["Description"].ToString();
                tc.Style.Add("font-weight", "bold");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Awal"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Awal"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Awal"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Awal"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Awal"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Awal"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Receiving"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Receiving"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Receiving"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Receiving"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Incoming"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Incoming"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Incoming"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Incoming"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Resell"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Resell"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Resell"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Resell"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Resell"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Resell"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_TradeIn"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_TradeIn"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_TradeIn"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_TradeIn"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Outgoing"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Outgoing"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Outgoing"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Outgoing"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Sales"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Sales"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Sales"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Sales"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Sales"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Sales"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Gift"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Gift"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Gift"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Gift"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Gift"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Gift"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Return"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Return"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Return"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Return"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Return"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Return"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Lebur"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Lebur"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Lebur"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Lebur"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Mutasi"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Mutasi"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Mutasi"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Mutasi"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Qty_Opname"]).ToString("#,##0");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Nett_Weight_Opname"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Gross_Weight_Opname"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToInt32(last["Butir_Opname"]).ToString("#,##0");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["Carat_Opname"]).ToString("#,##0.000");
				tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(last["HargaM_Opname"]).ToString("#,##0.00");
                tc.Style.Add("font-weight", "bold");
				tc.Style.Add("text-align", "right");
                tc.BackColor = Color.Aquamarine;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);
            }

        }
    }


    private void setCharacter()
    {
        productLevel.Items.Add(new ListItem("Loose Diamond (LD)", "9999"));
        productCategory.Items.Add(new ListItem("Plain Gold (PG)", "9998"));
        productCategory.Items.Add(new ListItem("Loose Diamond (LD)", "9999"));
        stoneDist.Items.Add(new ListItem("Plain Gold (PG)", "9998"));
        stoneDist.Items.Add(new ListItem("Loose Diamond (LD)", "9999"));
    }

    private string reverseDate(string date, string param)
    {

        string newDate = string.Empty;
        string[] arr = date.Split('-');
        newDate = arr[2] + "-" + arr[1] + "-" + arr[0];
        if (param.Equals("from"))
        {
            newDate = arr[2] + "-" + arr[1] + "-01";
        }
        return newDate;
    }

    private DataSet customExec(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
        sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
        DataSet objDS = new DataSet();
        sqlAdapter.Fill(objDS, "data");
        sqlCnn.Close();

        return objDS;
    }

    protected void excel_ServerClick(object sender, EventArgs e)
    {
        string href = "DJ_Mutasi_Export.aspx?";

        //Customize-Qstr
        href += "from=" + reverseDate(datepicker.Text, "from");
        href += "&to=" + reverseDate(datepicker2.Text, "to");
        href += "&locationType=" + locationType.SelectedValue;
        href += "&location=" + cabang.SelectedValue;
        href += "&productItem=" + productItem.SelectedValue;
        href += "&productCategory=" + productCategory.SelectedValue;
        href += "&productLevel=" + productLevel.SelectedValue;
        href += "&stoneDist=" + stoneDist.SelectedValue;
        href += "&plu=" + plu.Text;
        href += "&groupBy=" + groupby.SelectedValue;
        href += "&brand=" + brand.SelectedValue;
        href += "&product=" + product.SelectedValue;
        href += "&groupByText=" + groupby.SelectedItem.Text;

        Response.Redirect(href);
    }

    private void excelDetail(string href)
    {
        //Customize-Qstr
        href += "from=" + reverseDate(datepicker.Text, "to");
        href += "&to=" + reverseDate(datepicker2.Text, "to");
        href += "&locationType=" + locationType.SelectedValue;
        href += "&location=" + cabang.SelectedValue;
        href += "&productItem=" + productItem.SelectedValue;
        href += "&productCategory=" + productCategory.SelectedValue;
        href += "&productLevel=" + productLevel.SelectedValue;
        href += "&stoneDist=" + stoneDist.SelectedValue;
        href += "&plu=" + plu.Text;
        href += "&groupBy=" + groupby.SelectedValue;
        href += "&brand=" + brand.SelectedValue;
        href += "&product=" + product.SelectedValue;
        href += "&groupByText=" + groupby.SelectedItem.Text;

        Response.Redirect(href);
    }

    protected void btnReceive_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Receive.aspx?");
    }

    protected void btnSales_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Sales.aspx?");
    }

    protected void btnGift_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Gift.aspx?");
    }

    protected void btnResell_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Resell.aspx?");
    }

    protected void btnTradeIn_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_TradeIn.aspx?");
    }

    protected void btnReturn_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Return.aspx?");
    }

    protected void btnLebur_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Lebur.aspx?");
    }

    protected void stockAkhir_ServerClick(object sender, EventArgs e)
    {
        string href = "DJ_Mutasi_ExportDetail.aspx?";

        //Customize-Qstr
        href += "from=" + reverseDate(datepicker.Text, "from");
        href += "&to=" + reverseDate(datepicker2.Text, "to");
        href += "&locationType=" + locationType.SelectedValue;
        href += "&location=" + cabang.SelectedValue;
        href += "&productItem=" + productItem.SelectedValue;
        href += "&productCategory=" + productCategory.SelectedValue;
        href += "&productLevel=" + productLevel.SelectedValue;
        href += "&stoneDist=" + stoneDist.SelectedValue;
        href += "&plu=" + plu.Text;
        href += "&groupBy=" + groupby.SelectedValue;
        href += "&brand=" + brand.SelectedValue;
        href += "&product=" + product.SelectedValue;
        href += "&groupByText=" + groupby.SelectedItem.Text;

        Response.Redirect(href);
    }

    protected void btnTransfer_ServerClick(object sender, EventArgs e)
    {
        excelDetail("Mutasi_Export_Transfer.aspx?");
    }
}