using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
            DLC.CardTypeList(card);
            DLC.EDCList(edc);
            DLC.BankList(bank);
            DLC.ProgramList(program);

            int Tipe = 1;
            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);

            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                tipelokasi.Items.Clear();
                tipelokasi.Enabled = false;
                lokasi.Items.Clear();
                lokasi.Enabled = false;
            }

            fillFilter();

            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int ID = Convert.ToInt32(Request.QueryString["id"]);

        var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.SalesOrder.StatusVoid == false
                 && !p.SalesReceipt.SalesOrder.Draft
                 && p.IDPaymentType == ID
                 select p;

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesReceipt.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesReceipt.SalesOrder.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        int IDEDC = Convert.ToInt32(Request.QueryString["edc"]);
        int IDBank = Convert.ToInt32(Request.QueryString["bank"]);
        int IDProgram = Convert.ToInt32(Request.QueryString["program"]);
        int IDCard = Convert.ToInt32(Request.QueryString["card"]);

        if (IDEDC > 0)
            rs = rs.Where(p => p.IDEDC == IDEDC);
        if (IDBank > 0)
            rs = rs.Where(p => p.IDBankIssuer == IDBank);
        if (IDProgram > 0)
            rs = rs.Where(p => p.IDProgram == IDProgram);
        if (IDCard > 0)
            rs = rs.Where(p => p.IDCard == IDCard);

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesReceipt.SalesOrder.IDLokasi == c.IDLokasi && p.SalesReceipt.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        try
        {
            int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

            if (Tipe == 1 || Tipe == 2)
            {

                rs = rs.Where(p => p.SalesReceipt.SalesOrder.IDLokasi == Lokasi && p.SalesReceipt.SalesOrder.TipeLokasi == Tipe);
            }
        }
        catch (Exception ex) { }

        rs = rs.OrderBy(p => p.SalesReceipt.SalesOrder.Nomor);

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalMDR = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (r.Nominal.Value != 0)
                {
                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);


                    td = new TableCell();
                    td.Text = r.SalesReceipt.SalesOrder.Nomor;
                    tr.Cells.Add(td);

                    var item = (from p in r.SalesReceipt.SalesOrder.SalesOrderDJ

                                select new { PLU = p.StockProductDJ.Nomor }).Union
                               (from p in r.SalesReceipt.SalesOrder.SalesOrderPG

                                select new { PLU = p.StockProductPG.Nomor }).Union
                                (from p in r.SalesReceipt.SalesOrder.SalesOrderGJ

                                 select new { PLU = p.StockProductGJ.Nomor }).Union
                                 (from p in r.SalesReceipt.SalesOrder.SalesOrderLD

                                  select new { PLU = p.StockProductLD_Stone1B.Nomor });

                    string PLU = "";
                    foreach (var it in item)
                    {
                        PLU = it.PLU + "<br />";
                    }

                    td = new TableCell();
                    td.Text = PLU;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Nominal.Value).ToString("N0");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.SalesReceipt.SalesOrder.TipeLokasi == 0)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);
                    else if (r.SalesReceipt.SalesOrder.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);
                    else if (r.SalesReceipt.SalesOrder.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesReceipt.SalesOrder.CustomerNama;
                    tr.Cells.Add(td);

                    string EDC = "-";
                    try
                    {
                        EDC = r.EDCList.Nama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = EDC;
                    tr.Cells.Add(td);

                    string BankIssuer = "-";
                    try
                    {
                        BankIssuer = r.BankIssuer.Nama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = BankIssuer;
                    tr.Cells.Add(td);

                    string CCNumber = "-";
                    try
                    {
                        CCNumber = r.CCNumber;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = CCNumber;
                    tr.Cells.Add(td);

                    string CCName = "-";
                    try
                    {
                        CCName = r.CCName;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = CCName;
                    tr.Cells.Add(td);

                    string CC = "-";
                    try
                    {
                        CC = r.CardType.Nama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = CC;
                    tr.Cells.Add(td);

                    string Program = "-";
                    try
                    {
                        Program = r.ProgramCicilan.Nama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = Program;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.MDR.Value.ToString("N0");
                    tr.Cells.Add(td);

                    Total += Convert.ToDecimal(r.Nominal);
                    TotalMDR += r.MDR.Value;

                    tb.Rows.Add(tr);
                }
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 3;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            //td2.HorizontalAlign = HorizontalAlign.Right;
            td2.Text = "<b>" + Total.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 8;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalMDR.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        dari.Text = Request.QueryString["dari"];
        sampai.Text = Request.QueryString["sampai"];
        edc.SelectedValue = Request.QueryString["edc"];
        program.SelectedValue = Request.QueryString["program"];
        bank.SelectedValue = Request.QueryString["bank"];
        card.SelectedValue = Request.QueryString["card"];
        tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
        lokasi.SelectedValue = Request.QueryString["lokasi"];
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&id=" + myID;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;
        href += "&bank=" + bank.SelectedValue;
        href += "&card=" + card.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Payment_Detail_Print.aspx";
        href += "?id=" + myID;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;
        href += "&bank=" + bank.SelectedValue;
        href += "&card=" + card.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Payment_Detail_Export.aspx";
        href += "?id=" + myID;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&edc=" + edc.SelectedValue;
        href += "&program=" + program.SelectedValue;
        href += "&bank=" + bank.SelectedValue;
        href += "&card=" + card.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }
}