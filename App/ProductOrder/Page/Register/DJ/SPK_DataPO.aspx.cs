using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_SPK_DataPO : System.Web.UI.Page
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
    protected int myTipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw_po.Focus(); //Keyword

            //Form
            fillForm();

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw_po=" + kw_po.Text;
        href += "&kw_customer=" + kw_customer.Text;
        href += "&kw_store=" + kw_store.Text;
        href += "&kw_exh=" + kw_exh.Text;
        href += "&tipe=" + myTipe;

        Response.Redirect(href);
    }

    protected void DataError() { Response.Redirect("SPK.aspx"); }

    protected void fillForm()
    {
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            string qstr = "?id=" + myID + "&tipe=" + myTipe;
            cancel.HRef += qstr;
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw_po.Text = Request.QueryString["kw_po"];
            kw_customer.Text = Request.QueryString["kw_customer"];
            kw_store.Text = Request.QueryString["kw_store"];
            kw_exh.Text = Request.QueryString["kw_exh"];
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {

        //var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID);

        if (kw_po.Text.Trim().Length >= 3 || kw_customer.Text.Trim().Length >=3 || kw_store.Text.Trim().Length >= 3 || kw_exh.Text.Trim().Length >= 3)
        {
            var rs =( from p in db.SalesReceiptDPPO
                      join p1 in db.DataCustomer on p.IDCustomer equals p1.ID
                      join p2 in db.LocOutlet on p.IDLokasi equals p2.ID
                      into temp1
                      from p3 in temp1.Where(x => x.Disable == false
                                                    && x.Draft == false
                                                    && p.TipeLokasi == 1
                                                    ).DefaultIfEmpty()
                      join p4 in db.LocExhibition on p.IDLokasi equals p4.ID
                      into temp2
                      from p5 in temp2.Where(x => x.Disable == false
                                                    && x.Draft == false
                                                    && p.TipeLokasi == 2
                                                    ).DefaultIfEmpty()
                      where  p.Draft == false 
                            && ( p.NomorDP != null || p.NomorDP != "")
                            && p.IDSalesOrder == null
                            //&& p.IDLokasi == 1
                            //&& ((p.NomorDP.Contains(kw_po.Text.Trim()))
                            //    || (p1.NoCustomer.Contains(kw_customer.Text.Trim()))
                            //    || (p2.Nama.Contains(kw_store.Text.Trim()))                    
                            //    )
                      select new {p.ID, p.NomorDP, p.Tgl, p1.NoCustomer, p.CustomerNama, nm_out = p3.Nama, nm_ex =p5.Nama, p.TipeLokasi});

            if (kw_po.Text.Trim() != "") { rs = rs.Where(p => p.NomorDP != null && p.NomorDP.Contains(kw_po.Text.Trim())); }
            if (kw_customer.Text.Trim() != "") { rs = rs.Where(p => p.NoCustomer != null && p.NoCustomer.Contains(kw_customer.Text.Trim())); }
            if (kw_store.Text.Trim() != "") { rs = rs.Where(p => p.nm_out != null && p.nm_out.Contains(kw_store.Text.Trim())); }
            if (kw_exh.Text.Trim() != "") { rs = rs.Where(p => p.nm_ex != null && p.nm_ex.Contains(kw_exh.Text.Trim())); }

            if (rs.Count() == 0)
                App.TableNoResult(tb);

            int index = 1;

            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class='text-muted pull-left' style='text-align:left;'>" + r.NomorDP + "</span>"
                           + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                           + "<span class='pull-left' style='text-align:left;'><b>" + Str.Tgl(r.Tgl) + "</b></span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class='text-muted pull-left' style='text-align:left;'>" + r.NoCustomer + "</span>"
                           + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                           + "<span class='pull-left' style='text-align:left;'><b>" + r.CustomerNama + "</b></span>";
                tr.Cells.Add(td);

                td = new TableCell();
                string lok = "";
                if (r.TipeLokasi == 1) { lok = r.nm_out; }
                else if (r.TipeLokasi == 2) { lok = r.nm_ex; }
                td.Text = "<p>" + lok + ".</p>";
                tr.Cells.Add(td);


                tb.Rows.Add(tr);
            }
        }
        else
        {
            lit_countcustomer.Text = "<span style='color:red;'>Minimal 3 karakter No. PO / Kode Customer  / Nama Store / Exhibition !</span>";
        }

    }

    protected string TD_HyperLink(int idpo)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"SPK_DataPOSave.aspx?id=" + myID + "&idpo=" + idpo + "&tipe=" + myTipe + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
}