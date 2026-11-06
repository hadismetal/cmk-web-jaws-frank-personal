using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;

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
            
        }
        fillForm();
        //btn_add_payment_click(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("PaymentRepair.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.SalesOrderRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Trans. Form
            string Brand = "";
            if (d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            else if (d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);

            brand.InnerHtml = Brand;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.CustomerNama;
            tgl.InnerHtml = Str.Tgl(d.Tgl);


            //Stone
            fillItem();

            decimal NilaiSO = 0m;
            decimal Paid = 0m;
            decimal Unpaid = 0m;

            try
            {
                NilaiSO = d.HargaRupiah.Value;
            }
            catch (Exception ex) { }
            try
            {
                Paid = d.Paid.Value;
            }
            catch (Exception ex) { }
            try
            {
                Unpaid = d.Unpaid.Value;
            }
            catch (Exception ex) { }

            nilaiso.Text = NilaiSO.ToString("N0");
            totalbayar.Text = (d.TotalBayar.Value).ToString("N0");
            discount.Text = (d.DiscountNominal.Value).ToString("N0");
            terbayar.Text = Paid.ToString("N0");
            kekurangan.Text = Unpaid.ToString("N0");
            //if (d.TradeIn == false)
            //    resell.Visible = false;
            pembayaran.Text = "0";
        }
    }

    //Stone
    //Stone
    protected void fillItem()
    {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItem(ref GT, ref GT2);

        //Grand-Total
        Sub_Total(GT, GT2, true);
    }

    //Item
    protected void fillItem(ref decimal GT, ref decimal GT2)
    {
        var rs = from p in db.SalesOrderRepair
                 where p.ID == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            

            if (r.IDProduct.HasValue)
            {
                td = new TableCell();
                td.Text = r.StockProductDJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);
            }
            else if (r.IDProductTitipan.HasValue)
            {
                td = new TableCell();
                td.Text = r.StockProductDJCustomer.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);
            }

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal((r.IDProduct.HasValue) ? r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound : 0) ;
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


    protected void btn_add_payment_click(object sender, EventArgs e)
    {
        DataTable dtMember = new DataTable("AdditionalMember");

        decimal SisaPembayaran = 0m;
        decimal NominalPembayaran = 0m;

        var d = db.SalesOrderRepair.SingleOrDefault(p => p.ID == myID);

        SisaPembayaran = d.Unpaid.Value;

        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("tipe"), new DataColumn("cardtipe"), new DataColumn("edc"), new DataColumn("bank"), new DataColumn("nokartu"), new DataColumn("program"), new DataColumn("nominal"), new DataColumn("mdr") });
        var counter = 1;

        string[] tipe = new string[100];
        string[] edc = new string[100];
        string[] bank = new string[100];
        string[] cardtipe = new string[100];
        string[] program = new string[100];

        for (var i = 0; i < GridView.Rows.Count; i++)
        {
           
            var gridViewRowTemp = GridView.Rows[i];

            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["tipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;
            tipe[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;

            drMember["edc"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            edc[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            drMember["bank"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;
            bank[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;

            drMember["cardtipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlCardType")).SelectedValue;
            cardtipe[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlCardType")).SelectedValue;
            drMember["program"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlProgram")).SelectedValue;
            program[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlProgram")).SelectedValue;

            drMember["nokartu"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNoKartu")).Text;
            drMember["nominal"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text).ToString("N0");
            drMember["mdr"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtMDR")).Text).ToString("N0");

            NominalPembayaran = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text);

            SisaPembayaran -= NominalPembayaran;

            dtMember.Rows.Add(drMember);

            counter++;
        }

        drMember = dtMember.NewRow();
        drMember["number"] = counter;
        drMember["tipe"] = "1";
        drMember["cardtipe"] = "1";
        drMember["edc"] = "1";
        drMember["bank"] = "1";
        drMember["program"] = "1";
        drMember["nokartu"] = "";
        drMember["nominal"] = SisaPembayaran.ToString("N0");
        drMember["mdr"] = "0";
        dtMember.Rows.Add(drMember);

        GridView.DataSource = dtMember;
        GridView.DataBind();
        if (GridView.Rows.Count > 0)
            GridView.Rows[(GridView.Rows.Count - 1)].Cells[1].FindControl("txtNominal").Focus();

        for (var i = 0; i < GridView.Rows.Count; i++)
        {
            var gridViewRowTemp = GridView.Rows[i];

            var x = from n in db.PaymentType
                    select new { n.ID, n.Nama };
            DropDownList ddlType = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlType");

            foreach (var r in x)
            {
                ddlType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlType.SelectedValue = tipe[i];

            var c = from n in db.CardType
                    select new { n.ID, n.Nama };
            DropDownList ddlCardType = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlCardType");
            ddlCardType.Items.Add(new ListItem("", "0"));
            foreach (var r in c)
            {
                ddlCardType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlCardType.SelectedValue = cardtipe[i];

            var y = from n in db.EDCList
                    where n.ID != 5
                    select new { n.ID, n.Nama };
            DropDownList ddlEDC = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlEDC");
            ddlEDC.Items.Add(new ListItem("", ""));
            foreach (var r in y)
            {
                ddlEDC.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlEDC.SelectedValue = edc[i];

            var z = from n in db.BankIssuer
                    where n.ID != 5
                    select new { n.ID, n.Nama };
            DropDownList ddlBank = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlBank");
            ddlBank.Items.Add(new ListItem("", ""));
            foreach (var r in z)
            {
                ddlBank.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlBank.SelectedValue = bank[i];

            var f = from n in db.ProgramCicilan
                    select new { n.ID, n.Nama };
            DropDownList ddlProgram = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlProgram");
            ddlProgram.Items.Add(new ListItem("", ""));
            foreach (var r in f)
            {
                ddlProgram.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlProgram.SelectedValue = program[i];

            counter++;
        }
        UpdatePembayaran();
        
    }

    protected void GridView_onRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtMember = new DataTable("AdditionalMember");
        DataTable dtMember2 = new DataTable("AdditionalMember");

        decimal SisaPembayaran = 0m;
        decimal NominalPembayaran = 0m;

        var d = db.SalesOrderRepair.SingleOrDefault(p => p.ID == myID);

        SisaPembayaran = d.Unpaid.Value;

        DataRow drMember, drMember2;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("tipe"), new DataColumn("cardtipe"), new DataColumn("edc"), new DataColumn("bank"), new DataColumn("program"), new DataColumn("nokartu"), new DataColumn("nominal"), new DataColumn("mdr")});
        dtMember2.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("tipe"), new DataColumn("cardtipe"), new DataColumn("edc"), new DataColumn("bank"), new DataColumn("program"), new DataColumn("nokartu"), new DataColumn("nominal"), new DataColumn("mdr")});

        string[] tipe = new string[100];
        string[] edc = new string[100];
        string[] bank = new string[100];
        string[] cardtipe = new string[100];
        string[] program = new string[100];

        var counter = 1;
        int i = 0;
        for (i = 0; i < GridView.Rows.Count; i++)
        {
            var gridViewRowTemp = GridView.Rows[i];

            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["tipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;
            tipe[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;

            drMember["edc"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            edc[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            drMember["bank"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;
            bank[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;

            drMember["cardtipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlCardType")).SelectedValue;
            cardtipe[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlCardType")).SelectedValue;
            drMember["program"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlProgram")).SelectedValue;
            program[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlProgram")).SelectedValue;

            drMember["nokartu"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNoKartu")).Text;
            drMember["nominal"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text).ToString("N0");
            drMember["mdr"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtMDR")).Text).ToString("N0");

            NominalPembayaran = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text);

            SisaPembayaran -= NominalPembayaran;

            dtMember.Rows.Add(drMember);

            counter++;
        }



        dtMember.Rows[e.RowIndex].Delete();

        GridView.DataSource = dtMember;
        GridView.DataBind();

        counter = 1;
        decimal totalbutir = 0;
        decimal totalongkospb = 0;
        decimal totalhargaparcel = 0;


        i = 0;
        for (i = 0; i < GridView.Rows.Count; i++)
        {
            var gridViewRowTemp = GridView.Rows[i];

            drMember2 = dtMember2.NewRow();
            //drMember2["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember2["number"] = (i+1).ToString();
            drMember2["tipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;
            drMember2["cardtipe"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlCardType")).SelectedValue;
            //tipe[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlType")).SelectedValue;

            drMember2["edc"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            //edc[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlEDC")).SelectedValue;
            drMember2["bank"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;
            drMember2["program"] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlProgram")).SelectedValue;
            //bank[i] = ((DropDownList)gridViewRowTemp.Cells[0].FindControl("ddlBank")).SelectedValue;

            drMember2["nokartu"] = ((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNoKartu")).Text;
            drMember2["nominal"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text).ToString("N0");
            drMember2["mdr"] = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtMDR")).Text).ToString("N0");

            NominalPembayaran = Convert.ToDecimal(((TextBox)gridViewRowTemp.Cells[1].FindControl("txtNominal")).Text);

            SisaPembayaran -= NominalPembayaran;

            dtMember2.Rows.Add(drMember2);

            counter++;
        }



        GridView.DataSource = dtMember2;
        GridView.DataBind();
        if (i > 0)
            GridView.Rows[(GridView.Rows.Count - 1)].Cells[1].FindControl("txtNominal").Focus();

        for (i = 0; i < GridView.Rows.Count; i++)
        {
            var gridViewRowTemp = GridView.Rows[i];

            var x = from n in db.PaymentType
                    select new { n.ID, n.Nama };
            DropDownList ddlType = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlType");

            foreach (var r in x)
            {
                ddlType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlType.SelectedValue = tipe[i];

            var c = from n in db.CardType
                    select new { n.ID, n.Nama };
            DropDownList ddlCardType = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlCardType");
            ddlCardType.Items.Add(new ListItem("", "0"));
            foreach (var r in c)
            {
                ddlCardType.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlCardType.SelectedValue = cardtipe[i];

            var y = from n in db.EDCList
                    where n.ID != 5
                    select new { n.ID, n.Nama };
            DropDownList ddlEDC = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlEDC");
            ddlEDC.Items.Add(new ListItem("", "0"));
            foreach (var r in y)
            {
                ddlEDC.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlEDC.SelectedValue = edc[i];

            var z = from n in db.BankIssuer
                    where n.ID != 5
                    select new { n.ID, n.Nama };
            DropDownList ddlBank = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlBank");
            ddlBank.Items.Add(new ListItem("", "0"));
            foreach (var r in z)
            {
                ddlBank.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlBank.SelectedValue = bank[i];

            var f = from n in db.ProgramCicilan
                    select new { n.ID, n.Nama };
            DropDownList ddlProgram = (DropDownList)GridView.Rows[i].Cells[0].FindControl("ddlProgram");
            ddlProgram.Items.Add(new ListItem("", ""));
            foreach (var r in f)
            {
                ddlProgram.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            ddlProgram.SelectedValue = program[i];

            counter++;
        }
        UpdatePembayaran();
    }

    protected void Bind(object sender, EventArgs e)
    {
        //if(DiamondGridView.Rows.Count > 0)

    }
    protected void UpdatePembayaran()
    {
        decimal TotalBayar = 0m;

        for (int i = 0; i < GridView.Rows.Count; i++)
        {
            TotalBayar += Convert.ToDecimal(((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNominal")).Text);
        }
        kekurangan.Text = (Convert.ToDecimal(kekurangan.Text) - TotalBayar).ToString("N0");
        pembayaran.Text = TotalBayar.ToString("N0");
    }

    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrderRepair.SingleOrDefault(p => p.ID == myID && p.Draft == false);
        if (d == null) DataError();
        else
        {
            

            decimal TotalBayar = 0m;

            int IDSalesReceipt = 0;

            string NoInvoice = "";
            string NoDP = "";

            NoInvoice = GenerateInvoice(d);
            NoDP = GenerateBooking(d);

            SalesReceipt s = new SalesReceipt();

            s.IDRepair = d.DocRepairResult.ID;
            s.NoSalesOrder = d.Nomor;
            s.OperatorNama = App.UserNama;
            s.OperatorTgl = DateTime.Now;

            db.SalesReceipt.InsertOnSubmit(s);
            db.SubmitChanges();

            IDSalesReceipt = s.ID;

            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                TotalBayar += Convert.ToDecimal(((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNominal")).Text);

                SalesReceiptDetail sd = new SalesReceiptDetail();

                sd.IDSalesReceipt = IDSalesReceipt;
                try
                {
                    sd.IDPaymentType = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlType")).SelectedValue);
                }
                catch (Exception ex) { }
                try
                {
                    if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlCardType")).SelectedValue) != 0)
                        sd.IDCard = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlCardType")).SelectedValue);
                }
                catch (Exception ex) { }
                try
                {
                    if(Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlEDC")).SelectedValue) != 0)
                        sd.IDEDC = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlEDC")).SelectedValue);
                }
                catch (Exception ex) { }
                try
                {
                    if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlBank")).SelectedValue) != 0)
                        sd.IDBankIssuer = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlBank")).SelectedValue);
                }
                catch (Exception ex) { }
                try
                {
                    if (Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlProgram")).SelectedValue) != 0)
                        sd.IDProgram = Convert.ToInt32(((DropDownList)GridView.Rows[i].Cells[4].FindControl("ddlProgram")).SelectedValue);
                }
                catch (Exception ex) { }
                sd.CCNumber = ((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNoKartu")).Text;
                sd.Nominal = Convert.ToDecimal(((TextBox)GridView.Rows[i].Cells[4].FindControl("txtNominal")).Text);
                sd.MDR = Convert.ToDecimal(((TextBox)GridView.Rows[i].Cells[4].FindControl("txtMDR")).Text);
                sd.OperatorNama = App.UserNama;
                sd.OperatorTgl = DateTime.Now;

                db.SalesReceiptDetail.InsertOnSubmit(sd);
                db.SubmitChanges();
            }
            kekurangan.Text = (Convert.ToDecimal(kekurangan.Text) - TotalBayar).ToString("N0");
            pembayaran.Text = TotalBayar.ToString("N0");

            d.Paid = d.Paid + TotalBayar;
            d.Unpaid = d.TotalBayar - d.Paid;

            db.SubmitChanges();

            var x = db.SalesReceipt.SingleOrDefault(p => p.ID == IDSalesReceipt);

            if (d.Unpaid <= 0m)
            {
                d.StatusPembayaran = true;
                x.TotalBayar = TotalBayar;
                x.NomorInvoice = NoInvoice;

                //Commit
                db.SubmitChanges();

                
                Response.Redirect("/OutletSales/Page/Create/Payment/Repair_Invoice.aspx?id=" + d.ID);
            }
            else
            {
                x.TotalBayar = TotalBayar;
                x.NomorDP = NoDP;

                //Commit
                db.SubmitChanges();

                //Redirect
                Response.Redirect("/OutletSales/Page/Create/Payment/Repair_DP.aspx?id=" + d.ID);
            }


            
        }
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
    }

    protected string GenerateInvoice(SalesOrderRepair j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrderRepair.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.SalesReceipt
                     where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan && p.NomorInvoice != null
                     && p.NomorInvoice.Substring(4, 1) == "" + j.TipeLokasi.ToString() + ""
                     && p.NomorInvoice.Substring(6, 3) == "" + j.IDLokasi.ToString().PadLeft(3, '0') + ""
                     orderby p.ID descending
                     select p).First();

            if (s.NomorInvoice != null)
                Nomor = Convert.ToInt32(s.NomorInvoice.Substring(16, 4)) + 1;
        }
        catch (Exception ex) { }

        return "INV/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3,'0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
    protected string GenerateBooking(SalesOrderRepair j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrderRepair.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.SalesReceipt
                     where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan && p.NomorDP != null
                     && p.NomorDP.Substring(3,1) == "" + j.TipeLokasi.Value.ToString() + ""
                     && p.NomorDP.Substring(5,3) == "" + j.IDLokasi.Value.ToString().PadLeft(3,'0') + ""
                     orderby p.ID descending
                     select p).First();
            
            /*var s = (from p in db.SalesReceipt
                     where p.OperatorTgl.Value.Year == 2014 && p.OperatorTgl.Value.Month == 2 && p.NomorDP != null
                     && p.NomorDP.Substring(3, 1) == "1"
                     && p.NomorDP.Substring(5, 3) == "001"
                     orderby p.ID descending
                     select p).First();*/

            if (s.NomorDP != null)
                Nomor = Convert.ToInt32(s.NomorDP.Substring(15, 4)) + 1;
        }
        catch (Exception ex) { }

        return "DP/" + j.TipeLokasi.Value.ToString() + "/" + j.IDLokasi.Value.ToString().PadLeft(3, '0') + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}