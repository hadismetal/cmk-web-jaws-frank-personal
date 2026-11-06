using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Page_Summary : System.Web.UI.Page
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Packaging_Export.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;
        tb.Visible = true;

        string spname = "sp_summary_packaging_excel";
        string query = " exec " + spname
                     + " " + Request.QueryString["locationType"] + ", "
                     + "'" + reverseDate(Request.QueryString["dari"], "from") + "', "
                     + "'" + reverseDate(Request.QueryString["sampai"], "to") + "', "
                     + Request.QueryString["location"] + ", "
                     + Request.QueryString["product"] + ", "
                     + Request.QueryString["groupby"] + ", "
					 + Request.QueryString["brand"];

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);


        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;

            DataRow last = dt.Rows[dt.Rows.Count - 1];

            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                if (dr["Kode"].ToString() == "Total")
                {
                    last = dr;
                    continue;
                }

                tc = new TableCell();
                tc.Text = dr["Location"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Packaging"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Kode"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["StockAwal"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Receiving"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                //tc.Style.Add("vo", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Incoming"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Outgoing"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Sales"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["ONgoing"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Summary"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                
            }

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = last["Location"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Packaging"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tc.BorderWidth = 1;
            tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Kode"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tc.BorderWidth = 1;
            tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["StockAwal"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Receiving"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            //tc.Style.Add("vo", "right");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Incoming"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Outgoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Sales"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Ongoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tc.BorderWidth = 1;
            tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Summary"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
			tc.BorderWidth = 1;
			tc.BorderColor = Color.Gray;
            tr.Cells.Add(tc);

            tb.Rows.Add(tr);
            
        }


    }
    private string reverseDate(string date, string param)
    {

        string newDate = string.Empty;
        try
        {
            string[] arr = date.Split('/');
            newDate = arr[2] + "-" + arr[0] + "-" + arr[1];
            if (param.Equals("from"))
            {
                newDate = arr[2] + "-" + arr[0] + "-01";
            }
        }
        catch
        {
            string[] arr = date.Split('-');
            newDate = arr[2] + "-" + arr[1] + "-" + arr[0];
            if (param.Equals("from"))
            {
                newDate = arr[2] + "-" + arr[1] + "-01";
            }
        }
        return newDate;
    }
}