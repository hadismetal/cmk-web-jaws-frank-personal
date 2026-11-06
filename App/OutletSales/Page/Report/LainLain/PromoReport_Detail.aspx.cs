using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_LainLain_PromoReport_Detail : System.Web.UI.Page
{
	protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
	protected int tipe { get { return Convert.ToInt32(Request.QueryString["tipe"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
		if(!IsPostBack)
		{
			Fill();
		}
    }
	
	protected void Fill()
	{
		DataTable dt1 = Db.Rs("SELECT TOP 1 ID, Nama, StartDate, EndDate, Draft, Disable, CASE WHEN Draft = 1 THEN 'Draft' WHEN Disable = 1 THEN 'Disable' WHEN StartDate <= GETDATE() AND EndDate >= GETDATE() THEN 'Active' ELSE 'Inactive' END [Status], Keterangan, Tipe FROM Promo WHERE ID = " + myID, Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT ID, IDForm, MinValue, MaxValue, Discount, OrderNumber, Nama FROM PromoDetail WHERE IDForm = " + myID, Db.Cnn_DB);
		
		if(dt1.Rows.Count == 1)
		{
			int draft = Convert.ToInt32(dt1.Rows[0]["Draft"]);
			int disable = Convert.ToInt32(dt1.Rows[0]["Disable"]);
			DateTime started = Convert.ToDateTime(dt1.Rows[0]["StartDate"]);
			DateTime ended = Convert.ToDateTime(dt1.Rows[0]["EndDate"]);
			
			promoname.InnerText = dt1.Rows[0]["Nama"].ToString();
			start.InnerText = Convert.ToDateTime(dt1.Rows[0]["StartDate"]).ToString("dd MMMM yyyy");
			end.InnerText = Convert.ToDateTime(dt1.Rows[0]["EndDate"]).ToString("dd MMMM yyyy");
			status.InnerText = dt1.Rows[0]["Status"].ToString();
			keterangan.InnerText = dt1.Rows[0]["Keterangan"].ToString();
			
			if(Convert.ToInt32(dt1.Rows[0]["Tipe"]) == 1)
			{
				divdetail.Visible = true;
				
				int i = 1;
				foreach(DataRow dr in dt2.Rows)
				{
					TableRow tr = new TableRow();
					TableCell td = new TableCell();
					td.Text = i.ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["Nama"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = Convert.ToInt64(dr["MinValue"]).ToString("N0");
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = Convert.ToInt64(dr["MaxValue"]).ToString("N0");
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = Convert.ToInt64(dr["Discount"]).ToString("N0");
					tr.Cells.Add(td);
					
					tb.Rows.Add(tr);
					
					i++;
				}
			}
			else
			{
				divdetail.Visible = false;
			}
			DataTable dt3 = Db.Rs("EXEC sp_report_promo_lainlain_detail " + myID + "," + tipe, Db.Cnn_DB);
			if(dt3.Rows.Count > 0)
			{
				int j = 1;
				decimal totalNettPrice = 0;
				decimal totalQty = 0;

				foreach(DataRow dr in dt3.Rows)
				{
					TableRow tr = new TableRow();
					TableCell td = new TableCell();
					td.Text = j.ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["NoTransaction"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = Convert.ToDateTime(dr["TglTransaction"]).ToString("dd-MM-yyyy");
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["NoInvoice"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["Store"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["CustomerName"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["SalesName"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["ProductType"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["ProductCode"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["ProductItem"].ToString();
					tr.Cells.Add(td);
					
					td = new TableCell();
					td.Text = dr["Qty"].ToString();
					tr.Cells.Add(td);
					decimal qty = Convert.ToDecimal(dr["Qty"]);
					totalQty += qty;
					
					td = new TableCell();
					td.Text = Convert.ToDecimal(dr["NettPrice"]).ToString("N0");
					tr.Cells.Add(td);
					decimal nettPrice = Convert.ToDecimal(dr["NettPrice"]);
					totalNettPrice += nettPrice;

					tb1.Rows.Add(tr);
					
					j++;
				}
				TableRow totalRow = new TableRow();

				TableCell totalLabel = new TableCell();
				totalLabel.ColumnSpan = 10;
				totalLabel.Text = "<b>TOTAL</b>";
				totalLabel.HorizontalAlign = HorizontalAlign.Right;
				totalRow.Cells.Add(totalLabel);
				
				TableCell totalQtyVal = new TableCell();
				totalQtyVal.Text = "<b>" + totalQty.ToString("N0") + "</b>";
				totalQtyVal.HorizontalAlign = HorizontalAlign.Left;
				totalRow.Cells.Add(totalQtyVal);

				TableCell totalValue = new TableCell();
				totalValue.Text = "<b>" + totalNettPrice.ToString("N0") + "</b>";
				totalValue.HorizontalAlign = HorizontalAlign.Left;
				totalRow.Cells.Add(totalValue);

				tb1.Rows.Add(totalRow);
			}
			else
			{
				TableRow tr = new TableRow();
				TableCell td = new TableCell();
				td.ColumnSpan = 12;
				td.Text = "No Data";
				tr.Cells.Add(td);
				
				tb1.Rows.Add(tr);
			}
		}
	}
}