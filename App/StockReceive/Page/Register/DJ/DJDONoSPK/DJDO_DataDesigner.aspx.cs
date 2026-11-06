using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_DataDesigner : System.Web.UI.Page
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

            //Form
            fillForm();

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJDO_FillForm.aspx?id="+myID + "&err=ondesign"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Cancel-Link
            cancel.HRef += "?id=" + myID;

            //Hidden-ID
            idbrand.Text = d.LocWarehouse.IDBrand.ToString();
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

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.DataHuman
                 orderby p.Nama
                 where
                    !p.Disable && p.IDBrand == Convert.ToInt32(idbrand.Text) &&
                    (p.Nama.Contains(kw.Text))
                 select p;

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
    protected void Sub_Header(DataHuman r)
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
    protected string TD_HyperLink(DataHuman r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJDO_DataDesignerSave.aspx?id=" + myID + "&link=" + r.ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(DataHuman r)
    {
        string x = string.Empty;

        string Username = "";

        if (r.IDUser.HasValue)
            Username = Db.SingleString("SELECT TOP 1 Nama FROM UserAccount WHERE ID = " + r.IDUser.Value, Db.Cnn_AC);
        else
            Username = "";

        x += "<p><strong>" + r.Nama + " (" + Username + ")" + "</strong></p>";

        return x;
    }
    protected string TD_Addr(DataHuman r)
    {
        string x = string.Empty;

        string Email = "";

        if (r.IDUser.HasValue)
            Email = Db.SingleString("SELECT TOP 1 Email FROM UserAccount WHERE ID = " + r.IDUser.Value, Db.Cnn_AC);
        else
            Email = "";

        x += "<p class=\"text-sm\">" + Email + "</p>";

        return x;
    }
    protected string TD_Group(DataHuman r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.CompanyBrand.Nama + "</p>";

        return x;
    }
}