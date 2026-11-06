using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Stamps_Transaction : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        //App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //dari.Text = Str.Tgl(DateTime.Today);
            //sampai.Text = Str.Tgl(DateTime.Today);
            //status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected void fillFilter()
    {
        SONumber.Text = Request.QueryString["noso"];
        tglLunas.Text = Request.QueryString["dl"];
        ddlStatus.SelectedValue = Request.QueryString["se"];
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void gvjrstransaction_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //check if the row is the header row
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //add the thead and tbody section programatically
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
    private void fillTable(object sender, EventArgs e)
    {
        string query = "EXEC sp_stamps_transaction '"+ SONumber.Text.Trim() + "', '"+ tglLunas.Text.Trim() + "','"+ ddlStatus.SelectedValue + "' ";
        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_DB);

        if(dt.Rows.Count != 0)
        {
            gvjrstransaction.DataSource = dt;
            gvjrstransaction.DataBind();
        }
        else
        {
            tblNodata.Visible = true;
        }
    }
}