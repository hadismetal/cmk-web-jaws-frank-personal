using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Register_Sales_Item : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //HyperLink            
            addpck.HRef += "?id=" + myID;

            //Form
            fillForm();

            //Generate
        }
        fillTable(sender, e);
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
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //Item        
        fillPackaging();
        TextBox txtbox = new TextBox();

        //string txt = Request.QueryString["tb"];
        //if (txt != "")
        //{
        //    Page.RegisterStartupScript("af", "<script>$(function() {$(\"#" + txt + "\").focus();});</script>");
        //}
    }

    //Item Packaging
    protected void fillPackaging()
    {
        var rs = from p in db.SalesOrderPackaging
                 where p.IDForm == myID
                 select p;

        int index = 1;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderPackaging();
		//else if(rs.Count() > 1) save.Disabled = true;
		else save.Disabled = true;

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(9, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Packaging.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Packaging.Kode;
            tr.Cells.Add(td);

            TextBox txt = new TextBox();
            txt.ID = "pc_" + index.ToString();
            txt.Attributes["style"] = "width: 50px;";
			txt.Attributes["type"] = "number";
            txt.Text = r.Qty.HasValue ? r.Qty.Value.ToString("N0") : "1";
            txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            txt.AutoPostBack = true;
            txt.TextChanged += new EventHandler(OnKeyPressQty);

            HiddenField hf = new HiddenField();
            hf.ID = "hfpc" + index.ToString();
            hf.Value = r.ID.ToString();
            hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.Controls.Add(txt);
            td.Controls.Add(hf);
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);

            index++;
        }
    }

    //Custom Row
    protected void Sub_HeaderPackaging()
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "Packaging";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }

    //Display Column
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + ItemIndex + "','" + ID + "')\">";
        x += "      <i class=\"icon-trash\"></i> ";
        x += "      Delete";
        x += "  </a>";
        x += "</div>";

        return x;
    }
	
	protected void OnKeyPressQty(object sender, EventArgs e)
    {
		int qty = Convert.ToInt32(((TextBox)sender).Text);
		if(qty == 0)
		{
			string script = @"<script type='text/javascript'>
				alert('Item Yg Diinput Tidak Boleh 0.')</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);
			((TextBox)sender).Text = "1";
		}
		else
		{
			((TextBox)sender).Text = ((TextBox)sender).Text;
		}
	}

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            savePackaging(d);

            d.ModalRupiah = d.SalesOrderDJ.Sum(p => p.ModalRupiah) + d.SalesOrderPG.Sum(p => p.ModalRupiah) + d.SalesOrderGJ.Sum(p => p.ModalRupiah) + d.SalesOrderLD.Sum(p => p.ModalRupiah);
            d.HargaRupiah = d.SalesOrderDJ.Sum(p => p.HargaRupiah) + d.SalesOrderPG.Sum(p => p.HargaRupiah) + d.SalesOrderGJ.Sum(p => p.HargaRupiah) + d.SalesOrderLD.Sum(p => p.HargaRupiah);
            d.TotalBayarBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalBayar) + d.SalesOrderPG.Sum(p => p.TotalBayar) + d.SalesOrderGJ.Sum(p => p.TotalBayar) + d.SalesOrderLD.Sum(p => p.TotalBayar));
            d.TotalRupiahBeforeDisc = (d.SalesOrderDJ.Sum(p => p.TotalRupiah) + d.SalesOrderPG.Sum(p => p.TotalRupiah) + d.SalesOrderGJ.Sum(p => p.TotalRupiah) + d.SalesOrderLD.Sum(p => p.TotalRupiah));
            d.Discount = 0m;
            d.DiscountNominal = 0m;
            d.DiscountGift = 0m;
            d.DiscountProgram = 0m;
            d.DiscountProgramNominal = 0m;
            d.DiscountRound = 0m;
            d.TotalBayar = d.TotalBayarBeforeDisc;
            d.TotalRupiah = d.TotalRupiahBeforeDisc;
            d.TotalResell = 0m;
            d.Unpaid = d.TotalBayarBeforeDisc;
            d.Paid = 0m;
            d.Draft = false;
            d.DraftDate = DateTime.Now;
            d.Nomor = Document.Generate_Nomor_Packaging(d);
            db.SubmitChanges();

            decimal TotalBayar = 0m;

            int IDSalesReceipt = 0;

            string NoInvoice = "";
            string NoDP = "";

            NoInvoice = Document.Generate_Nomor_Invoice(d);

            SalesReceipt s = new SalesReceipt();

            s.IDSalesOrder = myID;
            s.NoSalesOrder = d.Nomor;
            s.OperatorNama = App.UserNama;
            s.OperatorTgl = DateTime.Now;
            s.TotalBayar = TotalBayar;
            s.NomorInvoice = NoInvoice;

            db.SalesReceipt.InsertOnSubmit(s);
            db.SubmitChanges();

            d.Paid = d.Paid + TotalBayar;
            d.Unpaid = d.TotalBayar - d.Paid;
            d.StatusPembayaran = false;

            db.SubmitChanges();
			
			string msg = saveApprovalPackaging(d.ID);
			
			string script = "<script type='text/javascript'> " +
				"alert('Mohon Approve PK / SV dengan Nomor " + d.Nomor + " untuk Melanjutkan Transaksi.'); " +
				"window.location.href = '../../Summary.aspx';" + 
			"</script>";

			ClientScript.RegisterStartupScript(this.GetType(), "alertRedirect", script);

            //Response.Redirect("/OutletSales/Page/Create/Payment/Sales_Packaging.aspx?&id=" + d.ID + "&sid=" + s.ID + "&msg" + msg);
        }
        //Response.Redirect("Sales_Submit.aspx?id=" + myID);
        //Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?d=1&id=" + d.ID);

    }


    protected void savePackaging(SalesOrder d)
    {
        TextBox txt = new TextBox();
        string hf = "";
        int index = 1;

        foreach (TableRow row in tb2.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hfpc" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("pc_" + index.ToString());

                var s = db.SalesOrderPackaging.SingleOrDefault(p => p.ID == Convert.ToInt32(hf));
                if (s != null)
                {
                    /*var x = db.StockActualPackaging.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == s.IDProduct && p.InHand > 0);
                    if (x == null) DataError();
                    else
                    {
                        x.InHand = x.InHand + s.Qty.Value;
                        db.SubmitChanges();
                    }*/

                    s.Qty = Convert.ToDecimal(txt.Text);
                    s.TotalModalRupiah = s.Qty * s.ModalRupiah;
                    //x.InHand = x.InHand - Convert.ToDecimal(txt.Text);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex) { }
            index++;
        }
    }

    protected string saveApprovalPackaging(int id)
	{
		string result = string.Empty;
		try
		{
			Db.Execute("EXEC sp_insert_approval_pksv '" + id + "','PK'", Db.Cnn_DB);
		}
		catch(Exception ex) 
		{ 
			result = ex.Message;
		}

		return result;
	}
}
