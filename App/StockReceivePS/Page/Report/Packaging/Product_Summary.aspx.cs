using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Page_Summary : System.Web.UI.Page {
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

        private Map_DB _map_DB;

        private Map_DB map_DB
        {
            get
            {
                if (_map_DB == null)
                    _map_DB = new Map_DB();
                return _map_DB;
            }
        }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (map_DB != null)
	        map_DB.Dispose();
	    base.Dispose();
	}
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

            //kw.Focus(); //Keyword
            string[] from = DateTime.Now.ToString("dd-MM-yyyy").Split('-');
            
            //DLC
            DLC.CompanyBrand(brand); 
            StockLocationList(location, tipe);
            fillDropDown();

            dari.Text = dari.Text = "01-" + from[1] + "-" + from[2];
            sampai.Text = Str.Tgl(DateTime.Today);
			
            //Generate
            //fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}
	
	protected void StockLocationList(DropDownList c1, int tipe)
    {
        switch (tipe)
        {
            case 0:
                {
                    IOrderedQueryable<LocWarehouse> orderedQueryable2 = from p in map_DB.LocWarehouse
                                                                        where !p.Disable
                                                                        orderby p.Nama
                                                                        select p;
                    {
                        foreach (LocWarehouse item in orderedQueryable2)
                        {
                            string nama = item.Nama;
                            string value = item.ID.ToString();
                            c1.Items.Add(new ListItem(nama, value));
                        }

                        break;
                    }
                }
            case 1:
                {
                    IOrderedQueryable<LocOutlet> orderedQueryable = from p in map_DB.LocOutlet
                                                                    where !p.Disable
                                                                    orderby p.SN.Value
                                                                    select p;
                    {
                        foreach (LocOutlet item2 in orderedQueryable)
                        {
                            string nama = item2.Nama;
                            string value = item2.ID.ToString();
                            c1.Items.Add(new ListItem(nama, value));
                        }

                        break;
                    }
                }
            case 2:
                {
                    IOrderedQueryable<LocExhibition> orderedQueryable = from p in map_DB.LocExhibition
                                                                    where !p.Disable
                                                                    orderby p.ID
                                                                    select p;
                    {
                        foreach (LocExhibition item2 in orderedQueryable)
                        {
                            string nama = item2.Nama;
                            string value = item2.ID.ToString();
                            c1.Items.Add(new ListItem(nama, value));
                        }

                        break;
                    }
                }
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;
        if(groupBy.SelectedValue == "0")
        {
            headLocaion.Visible = true;
            headProduct.Visible = false;
        }
        else
        {
            headLocaion.Visible = false;
            headProduct.Visible = true;
        }

        string spname = "sp_summary_packaging";
        string query = " exec " + spname
                     + " " + locationType.SelectedValue + ", "
                     + "'" + reverseDate(dari.Text, "from") + "', "
                     + "'" + reverseDate(sampai.Text, "to") + "', "
                     + location.SelectedValue + ", "
                     + product.SelectedValue + ", "
                     + groupBy.SelectedValue + ", "
					 + brand.SelectedValue;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

            
		if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;
            DataRow last = dt.Rows[0];
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();
                if (dr["Description"].ToString() == "Total")
                {
                    last = dr;
                    continue;
                }

                tc = new TableCell();
                tc.Text = dr["Description"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Stock"].ToString();
                tr.Cells.Add(tc);
                
                tc = new TableCell();
                tc.Text = HyperReceiving(dr["Description"].ToString(), dr["Receiving"].ToString());
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Incoming"].ToString();
                //tc.Style.Add("vo", "right");
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Outgoing"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = HyperSales(dr["Description"].ToString(), dr["Sales"].ToString());
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Ongoing"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["StockReal"].ToString();
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

               

            }

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = last["Description"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Stock"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Receiving"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Incoming"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            //tc.Style.Add("vo", "right");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Outgoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Sales"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["Ongoing"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = last["StockReal"].ToString();
            tc.Style.Add("font-weight", "bold");
            tc.BackColor = Color.Aquamarine;
            tr.Cells.Add(tc);

            tb.Rows.Add(tr);


        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //filter.SelectedValue = Request.QueryString["filter"];
            //filterby.SelectedValue = Request.QueryString["filterby"];
            //kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        fillTable(sender, e);
    }

    private string HyperReceiving(string Description, string qty)
    {
        var data = db.Packaging.FirstOrDefault(p => p.Nama.Equals(Description) || p.ID.Equals(Convert.ToInt32(product.SelectedValue)));
        if (data != null)
        {
            return "<a target='blank' href='Product_Rekap.aspx?refreshPage=1&cabang=" + location.SelectedValue + "&kw=&kode=" + data.Kode + "&brand=0&tipe=" + locationType.SelectedValue +"&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
        }
        else
        {
            if (db.LocWarehouse.Any(p => p.Nama.Equals(Description)))
            {
                var loc = db.LocWarehouse.Single(p => p.Nama.Equals(Description) && p.Disable == false);
                return "<a target='blank' href='Product_Rekap.aspx?refreshPage=1&cabang=" + loc.ID + "&kw=&brand=0&tipe=0&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
            }
            else if (db.LocOutlet.Any(p => p.Nama.Equals(Description)))
            {
                var loc = db.LocOutlet.Single(p => p.Nama.Equals(Description) && p.Disable == false);
                return "<a target='blank' href='Product_Rekap.aspx?refreshPage=1&cabang=" + loc.ID + "&kw=&brand=0&tipe=1&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
            }
            return "<a href='#' >" + qty + "</a>";
        }
    }

    private string HyperSales(string Description, string qty)
    {
        var data = db.Packaging.FirstOrDefault(p => p.Nama.Equals(Description) || p.ID.Equals(Convert.ToInt32(product.SelectedValue)));
        if (data != null)
        {
            return "<a target='blank' href='/StockReceivePS/Page/Report/Packaging.aspx?refreshPage=1&lokasi=" + location.SelectedValue + "&kw=&kode=" + data.Kode + "&brand=0&tipelokasi=1&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
        }
        else
        {
            if (db.LocWarehouse.Any(p => p.Nama.Equals(Description)))
            {
                var loc = db.LocWarehouse.Single(p => p.Nama.Equals(Description) && p.Disable == false);
                return "<a target='blank' href='/StockReceivePS/Page/Report/Packaging.aspx?refreshPage=1&lokasi=" + loc.ID + "&kw=&brand=0&tipelokasi=1&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
            }
            else if (db.LocOutlet.Any(p => p.Nama.Equals(Description)))
            {
                var loc = db.LocOutlet.Single(p => p.Nama.Equals(Description) && p.Disable == false);
                return "<a target='blank' href='/StockReceivePS/Page/Report/Packaging.aspx?refreshPage=1&lokasi=" + loc.ID + "&kw=&brand=0&tipelokasi=1&dari=" + reverseDate(dari.Text, "from") + "&sampai=" + reverseDate(sampai.Text, "to") + "' >" + qty + "</a>";
            }
            return "<a href='#' >" + qty + "</a>";
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

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        tb.Visible = false;
        location.Items.Clear();
        location.DataBind();
        location.Items.Add(new ListItem("All Location", "0"));
        StockLocationList(location, Convert.ToInt32(locationType.SelectedValue));
        location.Enabled = true;
    }

    protected void excel_ServerClick(object sender, EventArgs e)
    {
        string href = "PackagingSummary_Export.aspx?";

        //Customize-Qstr
        href += "location=" + location.SelectedValue;
        //href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&locationType=" + locationType.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&product=" + product.SelectedValue;
        href += "&groupby=" + groupBy.SelectedValue;
        Response.Redirect(href);
    }

    private void fillDropDown()
    {
        tb.Visible = false;
        var data = from p in db.Packaging
                   where !p.Disable
                   select p;

        foreach (var d in data)
        {
            string t = d.Nama;
            string v = d.ID.ToString();
            product.Items.Add(new ListItem(t, v));
        }

    }

}