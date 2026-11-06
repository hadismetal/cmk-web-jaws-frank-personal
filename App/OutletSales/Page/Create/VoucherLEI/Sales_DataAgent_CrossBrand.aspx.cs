using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Sales_Sales_DataAgent_CrossBrand : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Ddl.CompanyBrand1(brand);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    protected void fillTable(DataTable dt)
    {
        //RESULT
        int prev = 0;
        int index = 1;

        string loccode = location.SelectedValue;
        string tipeloc = loccode.Split('-')[0]; //tipelokasion
        string idloc = loccode.Split('-')[1]; //idlokasion
        string codeloc = loccode.Split('-')[2]; //kodeokasion

        int rowcount = dt.Rows.Count;
        if (rowcount == 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 5;
            td.CssClass = "text-center";
            td.Text = "<h1>No Data</h1>";
        }
        else
        {
            foreach (DataRow r in dt.Rows)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                /*Button btn = new Button();
                btn.Attributes["data-id"] = r["ID"].ToString();
                btn.Attributes["data-nama"] = r["Nama"].ToString();
                btn.Attributes["data-brand"] = r["Keterangan"].ToString();
                btn.Attributes["data-store"] = r["NamaKode"].ToString();

                btn.CssClass = "btn btn-primary";
                btn.Text = "+ Add";
                btn.Click += new EventHandler(save_AgentCrossBrand);*/

				td.Text = TD_HyperLink(tipeloc, idloc, r["ID"].ToString(), r["Nama"].ToString(), r["Keterangan"].ToString());
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + "</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r["Nama"].ToString(),r["Keterangan"].ToString());
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Addr(r["NamaKode"].ToString(), r["Nama"].ToString());
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Group(r["NamaKode"].ToString());
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected string TD_HyperLink(string tipeloc, string idloc, string idsalescross, string namasalescross, string niksalescross)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Sales_DataAgent_CrossBrandSave.aspx?id=" + myID + "&kdbc=" + brand.SelectedValue + "&tlc=" + tipeloc + "&idlc=" + idloc + "&idsc=" + idsalescross + "&nsc=" + namasalescross + "&niksc=" + niksalescross + " \">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(string nama, string keterangan)
    {
        string x = string.Empty;

        string Nama = "";
        string styleFontColor = "";
        string nikinfo = "";

        Nama = Str.CapUpper(nama);

        if (keterangan.Trim() != "")
        {
            nikinfo = "<p class=\"text-muted\">" + keterangan.Trim() + "</p>";
        }
        else
        {
            styleFontColor = "style='color:red;'";
        }

        x += "<p><strong " + styleFontColor + ">" + Nama + "</strong></p>";
        x += nikinfo;

        return x;
    }
    protected string TD_Addr(string namakode, string nama)
    {
        string x = string.Empty;

        string Location = "";
        string Location2 = "";

        Location = namakode;
        Location2 = nama;

        x += "<p><strong>" + Location + "</strong></p>";
        x += "<p class=\"text-muted\">" + Location2 + "</p>";

        return x;
    }
    protected string TD_Group(string namakode)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + namakode.ToString() + "</p>";

        return x;
    }

    protected void brand_SelectedIndexChanged(object sender, EventArgs e)
    {
		location.Items.Clear();
        string brandcode = brand.SelectedValue;
        divjr.Visible = false;
        if (!string.IsNullOrEmpty(brandcode))
        {
            divlocation.Visible = true;
            if (brandcode == "M" || brandcode == "S")
            {
                Ddl.StoreMondialMissMondial(location);
            }
            else if (brandcode == "F")
            {
                Ddl.StoreFrank(location);
            }
            else
            {
                Ddl.StorePalace(location);
            }
			location_SelectedIndexChanged(sender, e);
        }
        else
        {
            divlocation.Visible = false;
        }
    }

    protected void location_SelectedIndexChanged(object sender, EventArgs e)
    {
        divjr.Visible = true;
        string loccode = location.SelectedValue;
        string tipeloc = loccode.Split('-')[0]; //tipelokasion
        string idloc = loccode.Split('-')[1]; //idlokasion
        string codeloc = loccode.Split('-')[2]; //kodeokasion

        string brandcode = brand.SelectedValue;
        DataTable dt = new DataTable();

        if (brandcode == "M" || brandcode == "S")
        {
            dt = Ddl.SalesByStoreMondialMissMondial(codeloc);
        }
        else if (brandcode == "F")
        {
            dt = Ddl.SalesByStoreFrank(codeloc);
        }
        else
        {
            dt = Ddl.SalesByStorePalace(codeloc);
        }

        fillTable(dt);
    }

    protected void save_AgentCrossBrand(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
    }
}