using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Update_LainLain_PromoDetail : System.Web.UI.Page
{
    protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
        DataTable dt1 = Db.Rs("SELECT TOP 1 ID, Nama, StartDate, EndDate, Draft, Disable, CASE WHEN Draft = 1 THEN 'Draft' WHEN Disable = 1 THEN 'Disable' WHEN StartDate <= GETDATE() AND EndDate >= GETDATE() THEN 'Active' ELSE 'Inactive' END [Status], Keterangan, Tipe FROM Promo WHERE ID = " + myID, Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT ID, IDForm, MinValue, MaxValue, Discount, OrderNumber, Nama FROM PromoDetail WHERE IDForm = " + myID, Db.Cnn_DB);
		
		if(dt1.Rows.Count == 1)
		{
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
		}
		
    }
	
	protected void Update_Click(object sender, EventArgs e)
	{
		Response.Redirect("PromoDetail_Update.aspx?id=" + myID);
	}
	
	protected void Back_Click(object sender, EventArgs e)
	{
		Response.Redirect("Promo.aspx");
	}
}