using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;
using System.Web.UI;

public partial class Page_Register_Payment_ItemList : System.Web.UI.Page
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
            //tglbayar.Text = Str.Tgl(DateTime.Today);  
            fillForm();
        }
        
        //btn_add_payment_click(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Payment.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.SalesReceipt.SalesOrder.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.SalesReceipt.SalesOrder.Nomor + "</a>";

            //Trans. Form
            string Brand = "";
            if (d.SalesReceipt.SalesOrder.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);
            else if (d.SalesReceipt.SalesOrder.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);

            brand.InnerHtml = Brand;
            keterangan.InnerHtml = Str.HtmlBreak(d.SalesReceipt.SalesOrder.Keterangan);
            customer.InnerHtml = d.SalesReceipt.SalesOrder.CustomerNama;
            tgl.InnerHtml = Str.Tgl(d.SalesReceipt.SalesOrder.Tgl);


            //Stone
            fillItem();

            fillPembayaran();
        }
    }

    protected void fillPembayaran()
    {
        var d = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == myID);

        DataTable dtMember = new DataTable("AdditionalMember");

        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("tipe"), new DataColumn("cardtipe"), new DataColumn("edc"), new DataColumn("bank"), new DataColumn("nokartu"), new DataColumn("namakartu"), new DataColumn("program"), new DataColumn("nominal"), new DataColumn("mdr") });

        drMember = dtMember.NewRow();
        drMember["number"] = "1";
        drMember["tipe"] = "1";
        drMember["cardtipe"] = "1";
        drMember["edc"] = "1";
        drMember["bank"] = "1";
        drMember["program"] = "1";
        drMember["nokartu"] = "";
        drMember["namakartu"] = "";
        drMember["nominal"] = Str.Number0(d.Nominal.Value);
        drMember["mdr"] = Str.Number0(d.MDR.Value);
        dtMember.Rows.Add(drMember);

        GridView.DataSource = dtMember;
        GridView.DataBind();


        string[] tipe = new string[100];
        string[] edc = new string[100];
        string[] bank = new string[100];
        string[] cardtipe = new string[100];
        string[] program = new string[100];

        var gridViewRowTemp = GridView.Rows[0];

        var x = from n in db.PaymentType
                select new { n.ID, n.Nama };
        DropDownList ddlType = (DropDownList)GridView.Rows[0].Cells[0].FindControl("ddlType");

        foreach (var r in x)
        {
            ddlType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
        }

        ddlType.SelectedValue = d.IDPaymentType.ToString();

        var c = from n in db.CardType
                select new { n.ID, n.Nama };
        DropDownList ddlCardType = (DropDownList)GridView.Rows[0].Cells[0].FindControl("ddlCardType");
        ddlCardType.Items.Add(new ListItem("", "0"));
        foreach (var r in c)
        {
            ddlCardType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
        }

        ddlCardType.SelectedValue = d.IDCard.ToString();

        var y = from n in db.EDCList
                select new { n.ID, n.Nama };
        DropDownList ddlEDC = (DropDownList)GridView.Rows[0].Cells[0].FindControl("ddlEDC");
        ddlEDC.Items.Add(new ListItem("", ""));
        foreach (var r in y)
        {
            ddlEDC.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
        }

        ddlEDC.SelectedValue = d.IDEDC.ToString();

        var z = from n in db.BankIssuer
                select new { n.ID, n.Nama };
        DropDownList ddlBank = (DropDownList)GridView.Rows[0].Cells[0].FindControl("ddlBank");
        ddlBank.Items.Add(new ListItem("", ""));
        foreach (var r in z)
        {
            ddlBank.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
        }

        ddlBank.SelectedValue = d.IDBankIssuer.ToString();

        var f = from n in db.ProgramCicilan
                select new { n.ID, n.Nama };
        DropDownList ddlProgram = (DropDownList)GridView.Rows[0].Cells[0].FindControl("ddlProgram");
        ddlProgram.Items.Add(new ListItem("", ""));
        foreach (var r in f)
        {
            ddlProgram.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
        }

        ddlProgram.SelectedValue = d.IDProgram.ToString();

    }

    //Stone
    //Stone
    protected void fillItem()
    {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItemDJ(ref GT, ref GT2);
        fillItemPG(ref GT, ref GT2);
        fillItemGJ(ref GT, ref GT2);
        fillItemLD(ref GT, ref GT2);

        //Grand-Total
        Sub_Total(GT, GT2, true);
    }

    //Item
    protected void fillItemDJ(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            decimal DiscountPromo = 0m;
            DiscountPromo = r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0;

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + Convert.ToDecimal(DiscountPromo)).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }

    }
    protected void fillItemPG(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderPG
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + Convert.ToDecimal(r.DiscountPromo.Value)).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemGJ(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderGJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(2);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (Convert.ToDecimal(r.DiscountRound) + Convert.ToDecimal(r.DiscountPromo.Value)).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemLD(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderLD
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(3);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductLD_Stone1B.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    //Custom Row
    protected void Sub_Header(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(decimal Total, decimal TotalBayar, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 4;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalBayar) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);


        tb.Rows.Add(tr);
    }

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

    protected void save_Click(object sender, EventArgs e)
    {

        var d = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {

            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                try
                {
                    try
                    {
                        d.IDPaymentType = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlType")).SelectedValue);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        
                        if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[i].FindControl("ddlCardType")).SelectedValue) != 0)
                            d.IDCard = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlCardType")).SelectedValue);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlEDC")).SelectedValue) != 0)
                            d.IDEDC = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlEDC")).SelectedValue);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlBank")).SelectedValue) != 0)
                            d.IDBankIssuer = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlBank")).SelectedValue);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlProgram")).SelectedValue) != 0)
                            d.IDProgram = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlProgram")).SelectedValue);
                    }
                    catch (Exception ex) { }
                    d.CCNumber = ((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNoKartu")).Text;
                    d.CCName = ((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNamaKartu")).Text;
                    d.MDR = Convert.ToDecimal(((TextBox)GridView.Rows[i].Cells[4].FindControl("txtMDR")).Text);
                    d.OperatorNama = App.UserNama;
                    //d.OperatorTgl = DateTime.Now;

                    db.SubmitChanges();
                }
                catch (Exception ex) { Response.Write(ex.ToString()); }
            }
        }
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
    }
}