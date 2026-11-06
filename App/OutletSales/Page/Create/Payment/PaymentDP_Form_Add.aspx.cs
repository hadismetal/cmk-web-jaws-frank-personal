using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Payment_PaymentDP_Form_Add : System.Web.UI.Page
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
    protected string myModule = "PaymentDP";


    App gp = new App();
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
            //if (App.Refresh(this)) fillTable(sender, e);

            fillTable(sender, e);
        }
    }


    protected void DataError() { Response.Redirect(myModule + ".aspx"); }
    protected void fillForm()
    {
        var d = (from p in db.SalesReceiptDPPO
                 where p.ID == myID && p.Draft == true && p.IDSalesOrder == null
                 select p).SingleOrDefault();

        if (d == null) DataError();
        else
        {
            //Cancel-Link
            cancel.HRef += "?id=" + d.ID.ToString();
            cancel.HRef += "&status=1";

        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

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
        //var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft);
        var d = (from p in db.SalesReceiptDPPO where p.ID == myID && p.Draft == true  && p.IDSalesOrder == null select p).SingleOrDefault();
        if (d == null) DataError();

        var rs = from p in db.SalesReceiptDPPO
                 where p.StatusDP == 0 && p.Draft == false && p.IDCustomer == d.IDCustomer && p.IDSalesOrder == null && p.IDDPPOReference == null
                 select p;

        if (rs.Count() > 0)
        {

            int index = 1;

            if (rs.Count() == 0)
            {
                //Response.Redirect("Payment_DPAddSave.aspx?id=" + myID + "&iditem=" + rs.First().ID);
                Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1");
            }
            else
            {
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.ID);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NomorDP;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(r.Tgl);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0");
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.IDProduct != null && r.TipeProduct != null) ? "Stock" : "Purchase Order";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.EstimasiHarga.ToString("N0");
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
    }

    //Display Column
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "PaymentDP_Form_AddSave.aspx";
        href += "?id=" + myID;
        href += "&idref=" + ID;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }

    protected void cancel_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1");
    }
}