using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WS_CouponPrizedrawApprove : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //fillform();
        }
    }

    protected void gvCouponPrizedraw_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string name = "Status"; int index = 0;
        foreach (DataControlField col in gvCouponPrizedraw.Columns)
        {
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
            {
                index = gvCouponPrizedraw.Columns.IndexOf(col);
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[index].Text.ToLower() == "On Progress".ToLower())
            {
                e.Row.Cells[index].ForeColor = System.Drawing.Color.Blue;
            }
            if (e.Row.Cells[index].Text.ToLower() == "Submitted".ToLower())
            {
                e.Row.Cells[index].ForeColor = System.Drawing.Color.Orange;
            }
            if (e.Row.Cells[index].Text.ToLower() == "Approved".ToLower())
            {
                e.Row.Cells[index].ForeColor = System.Drawing.Color.Green;
            }
            if (e.Row.Cells[index].Text.ToLower() == "Rejected".ToLower())
            {
                e.Row.Cells[index].ForeColor = System.Drawing.Color.Red;
            }
        }

        name = "Start"; index = 0;
        foreach (DataControlField col in gvCouponPrizedraw.Columns)
        {
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
            {
                index = gvCouponPrizedraw.Columns.IndexOf(col);
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }

        name = "Created By"; index = 0;
        foreach (DataControlField col in gvCouponPrizedraw.Columns)
        {
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
            {
                index = gvCouponPrizedraw.Columns.IndexOf(col);
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }

        name = "Modified By"; index = 0;
        foreach (DataControlField col in gvCouponPrizedraw.Columns)
        {
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
            {
                index = gvCouponPrizedraw.Columns.IndexOf(col);
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[index].ColumnSpan = 2;
            e.Row.Cells[index].Text = "<div style='border-bottom:0.5px solid'>" + e.Row.Cells[index].Text + "</div>" + e.Row.Cells[index + 1].Text;
        }
    }

    protected void fillform()
    {
        string filter = "WHERE CONCAT(Title,CONCAT(CAST(DateStart AS NVARCHAR(50)),CONCAT('',CAST(DateEnd AS NVARCHAR(50)),CONCAT('',CreatedBy, ModifiedBy)))) LIKE '%" + txtSearch.Text + "%'";
        if (txtSearch.Text != "")
            CouponPrizedrawDataSource.SelectCommand = "SELECT ID, Title, DateStart,DateEnd, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, CASE WHEN [Status] = 0 THEN 'On Progress' WHEN [Status] = 1 THEN 'Submitted' WHEN [Status] = 2 THEN 'Approved' WHEN [Status] = 3 THEN 'Rejected' END [Status], CASE WHEN [DateEnd] < GETDATE() THEN 'Expired' ELSE 'Not Expired' END [Expired] FROM CouponPrizedraw " + filter;
        else
            CouponPrizedrawDataSource.SelectCommand = "SELECT ID, Title, DateStart,DateEnd, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, CASE WHEN [Status] = 0 THEN 'On Progress' WHEN [Status] = 1 THEN 'Submitted' WHEN [Status] = 2 THEN 'Approved' WHEN [Status] = 3 THEN 'Rejected' END [Status], CASE WHEN [DateEnd] < GETDATE() THEN 'Expired' ELSE 'Not Expired' END [Expired] FROM CouponPrizedraw ";
        gvCouponPrizedraw.DataBind();
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        fillform();
    }
}