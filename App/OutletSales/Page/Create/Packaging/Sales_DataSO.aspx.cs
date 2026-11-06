using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Packaging_Sales_DataSO : System.Web.UI.Page
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
    protected string no { get { return Request.QueryString["kw"]; } }
	protected string tipe { get { return Request.QueryString["tipe"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //Generate
            fillFilter();
            //if (App.Refresh(this))
            if (!string.IsNullOrEmpty(kw.Text))
                fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {

        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
		
		//Tipe
		href += "&tipe=" + ddltipe.SelectedValue;

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = no;
			ddltipe.SelectedValue = tipe;
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        if (ddltipe.SelectedValue == "SO")
        {
            var so = db.SalesOrder.Where(p => p.Nomor == kw.Text && p.StatusPembayaran == true && p.StatusVoid == false);
            int index = 1;
            if (so.Count() > 0)
            {
                foreach (var d in so)
                {
                    string KodeLokasi = "";
                    if (d.TipeLokasi == 1)
                        KodeLokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;
                    else if (d.TipeLokasi == 2)
                        KodeLokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;

                    TableRow tr = new TableRow();
                    TableCell td;

                    Button btn = new Button();
                    btn.ID = d.ID.ToString();
                    btn.Text = "OK";
                    btn.Attributes["data-noso"] = d.Nomor;
                    btn.Click += new EventHandler(SaveSO);
                    btn.CssClass = "btn btn-info";

                    td = new TableCell();
                    //td.Controls.Add(btn);
					td.Text = TD_HyperLink(myID,d.ID,ddltipe.SelectedValue);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDateTime(d.Tgl).ToString("dd-MM-yyyy");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.CustomerNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";//KodeLokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToInt32(d.TotalBayar).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.SalesNama;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = "No Data.";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
        else if (ddltipe.SelectedValue == "DP")
        {
            var so = db.SalesReceiptDPPO.Where(p => p.NomorDP == kw.Text && p.Draft == false && p.StatusDP == 0);
            int index = 1;
            if (so.Count() > 0)
            {
                foreach (var d in so)
                {
                    string KodeLokasi = "";
                    if (d.TipeLokasi == 1)
                        KodeLokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;
                    else if (d.TipeLokasi == 2)
                        KodeLokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;

                    TableRow tr = new TableRow();
                    TableCell td;

                    Button btn = new Button();
                    btn.ID = d.ID.ToString();
                    btn.Text = "OK";
                    btn.Attributes["data-noso"] = d.NomorDP;
                    btn.Click += new EventHandler(SaveSO);
                    btn.CssClass = "btn btn-info";

                    td = new TableCell();
                    //td.Controls.Add(btn);
					td.Text = TD_HyperLink(myID,d.ID,ddltipe.SelectedValue);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.NomorDP;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDateTime(d.Tgl).ToString("dd-MM-yyyy");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.CustomerNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";//KodeLokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToInt32(d.TotalBayar).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.SalesNama;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = "No Data.";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
        else if (ddltipe.SelectedValue == "RP")
        {
            var so = db.DocRepair.Where(p => p.Nomor == kw.Text && p.Draft == false && p.StatusRepair == 0);
            int index = 1;
            if (so.Count() > 0)
            {
                foreach (var d in so)
                {
                    string KodeLokasi = "";
                    if (d.TipeLokasi == 1)
                        KodeLokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;
                    else if (d.TipeLokasi == 2)
                        KodeLokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Nama;

                    TableRow tr = new TableRow();
                    TableCell td;

                    Button btn = new Button();
                    btn.ID = d.ID.ToString();
                    btn.Text = "OK";
                    btn.Attributes["data-noso"] = d.Nomor;
                    btn.Click += new EventHandler(SaveSO);
                    btn.CssClass = "btn btn-info";

                    td = new TableCell();
                    //td.Controls.Add(btn);
					td.Text = TD_HyperLink(myID,d.ID,ddltipe.SelectedValue);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDateTime(d.Tgl).ToString("dd-MM-yyyy");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.CustomerNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";//KodeLokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToInt32(d.EstimasiHarga).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = d.SalesNama;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else
            {
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = "No Data.";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    //Save SalesOrder To Packaging
    protected void SaveSO(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        Db.Execute("UPDATE SalesOrder SET LinkPackagingSalesOrder = '" + btn.Attributes["data-noso"] + "' WHERE ID = " + btn.ID, Db.Cnn_DB);

        Response.Redirect("Sales_Form.aspx?id=" + myID + "&status=1");
    }

    protected string TD_HyperLink(int id, int iddoc, string tipe)
    {
        string x = string.Empty;
        string noso = "";
        string nodp = "";
        string norep = "";
		
		
        if (tipe == "SO")
        {
            noso = kw.Text.Replace('+','/');

            x += "<a class=\"btn btn-info\" href=\"Sales_DataSOSave.aspx?id=" + id + "&noso=" + noso + "&iddoc=" + iddoc + " \">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (tipe == "DP")
        {
            nodp = kw.Text.Replace('+','/');

            x += "<a class=\"btn btn-info\" href=\"Sales_DataSOSave.aspx?id=" + id + "&nodp=" + nodp + "&iddoc=" + iddoc + " \">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (tipe == "RP")
        {
            norep = kw.Text.Replace('+','/');

            x += "<a class=\"btn btn-info\" href=\"Sales_DataSOSave.aspx?id=" + id + "&norep=" + norep + "&iddoc=" + iddoc + " \">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }

        return x;
    }
}