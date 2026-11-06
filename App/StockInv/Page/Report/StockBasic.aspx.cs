using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductSegmen(productsegmen);
            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }


    protected void getLokasi(object sender, EventArgs e)
    {
        location.Items.Clear();
        if (locationtype.SelectedValue == "0")
        {
            DLC.LocationList(location, 0);
        }
        else if (locationtype.SelectedValue == "1")
        {
            DLC.LocationList(location, 1);
        }
        else if (locationtype.SelectedValue == "2")
        {
            DLC.LocationList(location, 2);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {    
        DateTime start;
        DateTime end;
	int index = 1;
        int Lokasi = 0;
        try
        {
            Lokasi = Convert.ToInt32(Request.QueryString["location"]);
        }
        catch { }

        string query = "exec sp_Basic_Item "
            + Request.QueryString["locationtype"] + ", "
            + Lokasi.ToString() + ", "
            + productitem.SelectedValue + ", "
            + productcategory.SelectedValue + ", "
            + productlevel.SelectedValue + ", "
            + productsegmen.SelectedValue;

        DataTable dt = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
        
        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell td;

            foreach (DataRow dr in dt.Rows)
            {
		tr = new TableRow();

                td = new TableCell();
                td.Text = TD_Hyperlink(Convert.ToInt32(dr["ID"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index++.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<img src=\"/Images.aspx?path=" + dr["ImgPicture"] + "\" border=\"0\" width=\"100\" />";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_HyperlinkKaret(dr["ProtoNomor"].ToString());
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Qty"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["ProductItem"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["ProductCategory"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["ProductLevel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["ProductSegmen"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["Min"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["Max"]).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        location.Items.Clear();
        if (Request.QueryString["locationtype"] == "0")
        {
            DLC.LocationList(location, 0);
        }
        else if (Request.QueryString["locationtype"] == "1")
        {
            DLC.LocationList(location, 1);
        }
        else if (Request.QueryString["locationtype"] == "2")
        {
            DLC.LocationList(location, 2);
        }
        if (App.Refresh(this))
        {
            locationtype.SelectedValue = Request.QueryString["locationtype"];
            location.SelectedValue = Request.QueryString["location"];
        }
        productitem.SelectedValue = Request.QueryString["productitem"];
        productcategory.SelectedValue = Request.QueryString["productcategory"];
        productlevel.SelectedValue = Request.QueryString["productlevel"];
        productsegmen.SelectedValue = Request.QueryString["productsegmen"];
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&locationtype=" + locationtype.Text;
        href += "&location=" + location.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productsegmen=" + productsegmen.Text;

        Response.Redirect(href);
    }

    protected string TD_Hyperlink(int ID)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";

        return x;
    }

    protected string TD_HyperlinkKaret(string KodeKaret)
    {
        string x = string.Empty;

        x += "  <a target=\"_blank\" href=\"DJ_View.aspx?proto="  + KodeKaret +  "\">";
        x += KodeKaret;
        x += "  </a>";

        return x;
    }

    private DataTable customExec(string strSql, string CnnString) {
 	SqlConnection sqlCnn = new SqlConnection(CnnString);
	SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
	sqlAdapter.SelectCommand.CommandTimeout = 3000;

	DataSet objDS = new DataSet();
	sqlAdapter.Fill(objDS, "data");
	sqlCnn.Close();

	return objDS.Tables["data"];
   }
}