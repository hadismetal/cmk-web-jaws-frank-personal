using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_DataSupplier : System.Web.UI.Page
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
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myNOBRG { get { return Request.QueryString["nobrg"]; } }
    protected string myNOVAL { get { return Request.QueryString["noval"]; } }
    protected string myNOBAG { get { return Request.QueryString["nobag"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        if (!IsPostBack)
        {
            kw.Focus(); //Keyword
            //Form
            fillForm();
            DLC.DDLSupplier(supplier);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void DataError() { Response.Redirect("DJDO.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        //if (d == null) DataError();
        //else
        //{
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Cancel-Link
            cancel.HRef += "?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG + "#div_pinfo";

            //Hidden-ID
            idbrand.Text = d.LocWarehouse.IDBrand.ToString();
        //}
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&nodo=" + myNODO;
		href += "&nobrg=" + myNOBRG;
		href += "&noval=" + myNOVAL;
		href += "&nobag=" + myNOBAG;
        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&supplier=" + supplier.SelectedValue;
        Response.Redirect(href);
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            supplier.Text = Request.QueryString["supplier"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.DataSupplier
                 orderby p.Nama
                 where
                    !p.Disable && p.IDBrand == Convert.ToInt32(idbrand.Text) &&
                    (p.Nama.Contains(kw.Text))
                 select p;

        int Supplier = Convert.ToInt32(supplier.SelectedValue);
        if (Supplier > 0)
            rs = rs.Where(p => p.IDGroup == Supplier);

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
            td.Text = r.DataSupplierGroup.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
    protected void Sub_Header(DataSupplier r)
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
    protected string TD_HyperLink(DataSupplier r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJDO_DataSupplierSave.aspx?id=" + myID + "&nodo=" + myNODO + "&nobrg=" + myNOBRG + "&noval=" + myNOVAL + "&nobag=" + myNOBAG  +  "&link=" + r.ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(DataSupplier r)
    {
        string x = string.Empty;

        string Username = "";

        if (r.IDUser.HasValue)
            Username = " ( " + Db.SingleString("SELECT TOP 1 Nama FROM UserAccount WHERE ID = " + r.IDUser.Value, Db.Cnn_AC) + " )";
        else
            Username = "";

        x += "<p><strong>" + r.Nama + Username + "</strong></p>";

        return x;
    }
    protected string TD_Addr(DataSupplier r)
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
    protected string TD_Group(DataSupplier r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.CompanyBrand.Nama + "</p>";

        return x;
    }
}