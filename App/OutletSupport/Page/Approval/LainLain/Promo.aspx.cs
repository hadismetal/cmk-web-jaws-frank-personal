using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_LainLain_Promo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
		if(!IsPostBack)
		{
			datepicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MM-yyyy");
			datepicker2.Value = DateTime.Now.ToString("dd-MM-yyyy");
		}
    }
    
    protected void searchPromo_Click(object sender, EventArgs e)
    {
        string start = datepicker.Value;
        string[] timestart = start.Split('-');
        string expire = datepicker2.Value;
        string[] timeexpire = expire.Split('-');
		string tipepromo = tipe.SelectedValue;
		
		string timenow = "00:00";
        string timeexpired = "23:59";
		
        string query = "EXEC sp_report_promo_lainlain '" + search.Value + "'," + 
						"'" + tipe.SelectedValue + "'," +
						"'" + timestart[2] + "-" + timestart[1] + "-" + timestart[0] + " " + timenow + "'," +
						"'" + timeexpire[2] + "-"+ timeexpire[1] + "-" + timeexpire[0] + " " + timeexpired + "' ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        if (dt.Rows.Count > 0)
        {
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = i.ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Nama"].ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				td = new TableCell();
                td.Text = dr["TipePromo"].ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDateTime(dr["StartDate"]).ToString("dd MMMM yyyy");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDateTime(dr["EndDate"]).ToString("dd MMMM yyyy");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = dr["Status"].ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Keterangan"].ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["CreatedBy"].ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                Button btndetail = new Button();
                btndetail.CssClass = "btn btn-primary";
                btndetail.Attributes["data-id"] = dr["ID"].ToString();
                btndetail.Text = "Detail";
                btndetail.OnClientClick += "return onclickdetail(this);";
                //btndetail.Click += new EventHandler(btnDetails_Click);

                td = new TableCell();
                td.Controls.Add(btndetail);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
                i++;
            }
        }
        else
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 9;
            td.Text = "No Data";
            td.CssClass = "text-center";
            tr.Cells.Add(td);
            tb.Rows.Add(tr);
        }
    }
    protected void btnDetails_Click(object sender, EventArgs e)
    {
        string idpromo = ((TextBox)sender).Attributes["data-id"];
        string href = "/PromoDetail.aspx?";
        href += "id=" + idpromo;
        Response.Redirect(href);
    }
}