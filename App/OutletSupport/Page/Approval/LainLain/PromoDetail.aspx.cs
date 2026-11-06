using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_LainLain_PromoDetail : System.Web.UI.Page
{
    protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
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
			
			if(draft == 0)
			{
				btnApprove.Visible = false;
				btnReject.Visible = false;
				lblWarning.Visible = true;
				if(disable == 1)
				{
					lblWarning.Text = "Promo Has Been Rejected";
				}
				else
				{
					lblWarning.Text = "Promo Has Been Approved";
				}
			}
		}
		
    }
	
	protected void Approve_Click(object sender, EventArgs e)
	{
		Db.Execute("UPDATE Promo SET Draft = 0, Disable = 0, ApprovedlBy = '" + App.UserNama + "', ApprovedDate = GETDATE() WHERE ID = " + myID, Db.Cnn_DB);
		Response.Redirect("PromoDetail.aspx?id=" + myID);
	}
	
	protected void Reject_Click(object sender, EventArgs e)
	{
		Db.Execute("UPDATE Promo SET Draft = 0, Disable = 1 WHERE ID = " + myID, Db.Cnn_DB);
		Response.Redirect("PromoDetail.aspx?id=" + myID);
	}
}