using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Souvenir_Form : System.Web.UI.Page
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

    decimal GT_Qty = 0;
    decimal GT_Harga = 0m;
    decimal GT_Ongkos = 0m;
    decimal GT_OngkosPacking = 0m;
    decimal GT_HPP = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }
        fillForm();
    }

    //Exception
    protected void DataError() { Response.Redirect("ProductAdjustment.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && !p.Draft.Value && p.Approval == null);
        if (d == null) DataError();
        else
        {
            //Header
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            tgl.InnerHtml = Str.Tgl(d.OperatorTgl);

            //Souvenir
            fillSouvenir(d);
        }
    }


    //Souvenir
    protected void fillSouvenir(AdjustmentSouvenir d)
    {
        //SELECT
        var rs = from p in db.AdjustmentSouvenir_Product
                 where p.IDForm == myID
                 orderby p.Souvenir.Nama
                 select p;

        //RESULT
        int index = 1;
        decimal TotalQty = rs.Sum(p => p.Qty.Value);

        foreach (var r in rs)
        {
            decimal Ongkos = 0m;
            decimal HPP = 0m;

            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Souvenir.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.CssClass = "text-center";
            td.Text = TD_Nama(r.Souvenir.Kode);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Qty);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            GT_Qty += r.Qty.Value;

            tb.Rows.Add(tr);
            index++;
        }

        //Grand-Total
        Sub_Total(GT_Qty, true);
    }

    protected void Sub_Total(decimal TotalQty, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalQty) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected string TD_Info(decimal? Value)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number0(Value) + "</p>";

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && !p.Draft.Value && p.ApprovalTgl == null);
        if (d == null) DataError();
        else
        {

            d.Status = 1;
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Nomor = generateDocumentNumber();

            //db.SubmitChanges();

            var rs = from p in db.AdjustmentSouvenir_Product
                     where p.IDForm == myID
                     orderby p.Souvenir.Nama
                     select p;

            //RESULT

            decimal TotalQty = rs.Sum(p => p.Qty.Value);

            int index = 1;
            foreach (var r in rs)
            {
                var st = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == r.AdjustmentSouvenir.TipeLokasi && p.IDLokasi == r.AdjustmentSouvenir.IDLokasi);

                if (st == null)
                { }
                else
                {
                    if (d.StatusAdjustment == 0)
                    {
                        st.InHand = (Convert.ToDecimal(r.Qty));
                    }
                    else if (d.StatusAdjustment == 1)
                    {
                        st.Damaged = (Convert.ToDecimal(r.Qty));
                    }
                }
                index++;
            }
            db.SubmitChanges();

            //Redirect
            Response.Redirect("ProductAdjustment.aspx?d=1");
            App.ReloadClose(this);
        }
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
    }

    //Reject
    protected void reject_Click(object sender, EventArgs e)
    {
        var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && !p.Draft.Value && p.ApprovalTgl == null);
        if (d == null) DataError();
        else
        {

            d.Status = 2;
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Nomor = generateDocumentNumber();

            db.SubmitChanges();

            //Redirect
            Response.Redirect("ProductAdjustment.aspx?d=1");
            App.ReloadClose(this);
        }
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
    }

    protected string generateDocumentNumber()
    {
        string result = "";

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "ADJ/PC";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        string v3 = System.DateTime.Now.Month.ToString("d2").PadLeft(2, '0');
        string v4 = "0001";

        var rs = from p in db.AdjustmentSouvenir
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}