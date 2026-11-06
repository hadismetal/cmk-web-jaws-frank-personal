using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataAgent : System.Web.UI.Page
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
            kw.Focus(); //Keyword

            //DLC.CompanyBrand(brand);

            //Form
            fillForm();

            //Generate
            fillFilter();
            //if (App.Refresh(this)) 
        }
        fillTable(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Titipan.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Titipan_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Hidden-ID
            //idbrand.Text = d.DataSales.IDBrand.ToString();
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        //href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT

        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
        int IDBrand = 1;
        if (d.TipeLokasi == 1)
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

        if (d.TipeLokasi == 2)
            IDBrand = Db.SingleInteger("SELECT IDBrand FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
		
		string NamaLokasiSO = "";
        if (d.TipeLokasi == 1)
            NamaLokasiSO = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID =" + d.IDLokasi, Db.Cnn_DB);
        else if (d.TipeLokasi == 2)
            NamaLokasiSO = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID =" + d.IDLokasi, Db.Cnn_DB);

        var rs = from p in db.DataSales
                 orderby p.Nama
                 where
                    !p.Disable && p.IDBrand == IDBrand &&
                    (p.Nama.Contains(kw.Text)) &&
					p.DataSalesGroup.NamaKode.Equals(NamaLokasiSO)
                 select p;

        int UserID = App.UserID;
        var asl = from p in db.DataAdminSales
                  where p.IDUser == UserID
                  select p;
        if (asl.Count() > 0)
        {
            var x = asl.First();

            string NamaLokasi = "";
            if (x.TipeLokasi == 1)
                NamaLokasi = Db.SingleString("SELECT Kode FROM LocOutlet WHERE ID =" + x.IDLokasi, Db.Cnn_DB);
            else if (x.TipeLokasi == 2)
                NamaLokasi = Db.SingleString("SELECT Kode FROM LocExhibition WHERE ID =" + x.IDLokasi, Db.Cnn_DB);

            if (NamaLokasi != "")
                rs = rs.Where(p => p.DataSalesGroup.NamaKode.Equals(NamaLokasi));
        }

        //int Brand = Convert.ToInt32(brand.SelectedValue);
        //if (Brand > 0)
        //    rs = rs.Where(p => p.IDBrand == Brand);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.IDBrand)
            {
                Sub_Header(r);
                prev = r.IDBrand;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Addr(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
    protected void Sub_Header(DataSales r)
    {
        /*TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        string Username = Db.SingleString("SELECT TOP 1 Nama FROM UserAccount WHERE ID = " + r.IDUser, Db.Cnn_AC);

        x += "<br />";
        x += r.Nama + " (" + Username + ")";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);*/
    }
    protected string TD_HyperLink(DataSales r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Titipan_DataAgentSave.aspx?id=" + myID + "&link=" + r.ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(DataSales r)
    {
        string x = string.Empty;

        string Nama = "";

        Nama = r.Nama;

        x += "<p class=\"text-sm\">" + Nama + "</p>";

        return x;
    }
    protected string TD_Addr(DataSales r)
    {
        string x = string.Empty;

        string Email = "";

        Email = r.AddrEmail;

        x += "<p class=\"text-sm\">" + Email + "</p>";

        return x;
    }
    protected string TD_Group(DataSales r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.CompanyBrand.Nama + "</p>";

        return x;
    }
}