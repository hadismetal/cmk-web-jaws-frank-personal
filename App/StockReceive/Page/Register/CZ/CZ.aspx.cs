using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;

public partial class Page_Register_CZ_CZ : System.Web.UI.Page
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

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
     
        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
           
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        Table formtable = tb1;
        string query = " exec sp_getcz";

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
  
          if (dt == null) { App.TableNoResult(formtable); }
        else if (dt.Rows.Count == 0) { App.TableNoResult(formtable); }
        else
        {
            int index = 1;
            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();
                int a = 1;
                int b = 1;
                string r = "";
                foreach (DataColumn dc in dt.Columns)
                {
                   


                    // On all tables' columns
                    if (b == 1)
                    {
                       

                        r = dtRow[dc].ToString();

                        td = new TableCell();
                        td.Text = TD_HyperLink(r);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = index.ToString();
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                    }


                        
                        if (b == 4)
                        {
                            td = new TableCell();
                            td.Text = Str.FullDateJam(Convert.ToDateTime(dtRow[dc]));
                            tr.Cells.Add(td);
                        }

                        if (b == 6)
                        {
                            td = new TableCell();
                            td.Text = Convert.ToDecimal(dtRow[dc]).ToString("N2");
                            tr.Cells.Add(td);
                        }
                        if(b != 1 && b!= 4 && b!=6)
                        {
                            td = new TableCell();
                            td.Text = dtRow[dc].ToString();
                            tr.Cells.Add(td);
                        }

                    b++;
    // batas for each kolom
                }

                formtable.Rows.Add(tr);
                index++;
            }
        }
    }
    protected string TD_HyperLink(string r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"CZ_Form1.aspx?iddo=" + r + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }



}