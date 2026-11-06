using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJDO_DJDO_DataValidasi : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
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

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

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
        var rs = from p in cs.T_brj
                 orderby p.NoValidasi, p.NoKodeBarang
                 where p.NoReceive == null && p.NoSPK == null
                    && (p.NoValidasi.Contains("41RT")
                    //|| p.NoKodeBarang.Contains(Request.QueryString["kw"].ToString())
                    )
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            if (rs.Count() > 100)
            {
                string data_total = rs.Count().ToString("N0");
                rs = rs.Take(100);

                lit_data.Text = "<br />Displaying 100 / " + data_total + " data";

                //RESULT
                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_NoValidasi(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_NoKodeBarang(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoKodeBagian;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoDOBRJ;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
    }

    protected string TD_HyperLink(T_brj r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJDO_DataValidasiSave.aspx?id=" + myID + "&link=" + r.NoValidasi + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_NoValidasi(T_brj r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.NoValidasi + "</strong></p>";

        return x;
    }
    protected string TD_NoKodeBarang(T_brj r)
    {

        string x = string.Empty;

        x += "<p><strong>" + r.NoKodeBarang + "</strong></p>";

        return x;
    }

    protected void cancel_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("DJDO_Detail.aspx?id=" + myID + "&nodo=" + myNODO);
    }
}