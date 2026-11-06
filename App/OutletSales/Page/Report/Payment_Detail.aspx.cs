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
            DLC.JenisKartu(jeniskartu);
            DLC.ProgramList(program);
            DLC.BankList(bank);

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

        //var rs = from p in db.SalesReceiptDetail
        //         where p.SalesReceipt.SalesOrder.StatusVoid == false
        //         && !p.SalesReceipt.SalesOrder.Draft
        //         && p.IDPaymentType == ID
        //         select p;

        var rs = (from p in db.SalesReceiptDetail
                  where p.SalesReceipt.SalesOrder.StatusVoid == false
                  && !p.SalesReceipt.SalesOrder.Draft
                  && p.IDPaymentType == ID
                  select new
                  {
                      ID = p.ID,
                      Tipe = 1,
                      IDSalesReceipt = p.IDSalesReceipt,
                      IDSalesReceiptDetail = p.ID,
                      Tgl = p.SalesReceipt.SalesOrder.Tgl.Value,
                      IDEDC = p.IDEDC,
                      IDJenisKartu = p.IDJenisKartuKredit,
                      IDBank = p.IDBankIssuer,
                      IDProgram = p.IDProgram,
                      IDCard = p.IDCard,
                      IDLokasi = p.SalesReceipt.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesReceipt.SalesOrder.TipeLokasi,
                      Nomor = p.SalesReceipt.SalesOrder.Nomor,
                      Nominal = p.Nominal.Value,
                      CustomerNama = p.SalesReceipt.SalesOrder.CustomerNama,
                      EDCNama = p.EDCList.Nama,
                      ProgramNama = p.ProgramCicilan.Nama,
                      JenisKartu = p.JenisKartuKredit.Nama,
                      BankNama = p.BankIssuer.Nama,
                      CardNama = p.CardType.Nama,
                      CCNumber = p.CCNumber,
                      CCName = p.CCName,
                      MDR = p.MDR.Value
                  }).Concat(
                    from p in db.SalesReceiptDPPODetail
                    where p.SalesReceiptDPPO.StatusDP == 0
                    && p.IDPaymentType == ID
                    select new
                    {
                        ID = p.ID,
                        Tipe = 2,
                        IDSalesReceipt = p.IDSalesReceipt,
                        IDSalesReceiptDetail = p.ID,
                        Tgl = p.SalesReceiptDPPO.Tgl.Value,
                        IDEDC = p.IDEDC,
                        IDJenisKartu = p.IDJenisKartuKredit,
                        IDBank = p.IDBankIssuer,
                        IDProgram = p.IDProgram,
                        IDCard = p.IDCard,
                        IDLokasi = p.SalesReceiptDPPO.IDLokasi,
                        TipeLokasi = p.SalesReceiptDPPO.TipeLokasi,
                        Nomor = p.SalesReceiptDPPO.NomorDP,
                        Nominal = p.Nominal.Value,
                        CustomerNama = p.SalesReceiptDPPO.SalesOrder.CustomerNama,
                        EDCNama = p.EDCList.Nama,
                        ProgramNama = p.ProgramCicilan.Nama,
                        JenisKartu = p.JenisKartuKredit.Nama,
                        BankNama = p.BankIssuer.Nama,
                        CardNama = p.CardType.Nama,
                        CCNumber = p.CCNumber,
                        CCName = p.CCName,
                        MDR = p.MDR.Value
                    }
                 );

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        int IDEDC = Convert.ToInt32(Request.QueryString["edc"]);
        int IDBank = Convert.ToInt32(Request.QueryString["bank"]);
        int IDJenisKartu = Convert.ToInt32(Request.QueryString["jeniskartu"]);
        int IDProgram = Convert.ToInt32(Request.QueryString["program"]);
        int IDCard = Convert.ToInt32(Request.QueryString["card"]);

        if (IDEDC > 0)
            rs = rs.Where(p => p.IDEDC == IDEDC);
        if (IDBank > 0)
            rs = rs.Where(p => p.IDBank == IDBank);
        if (IDJenisKartu > 0)
            rs = rs.Where(p => p.IDJenisKartu == IDJenisKartu);
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
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        try
        {
            int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

            if (Tipe == 1 || Tipe == 2)
            {

                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            }
        }
        catch (Exception ex) { }

        rs = rs.OrderBy(p => p.Tgl);

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalMDR = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (r.Nominal != 0)
                {
                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);


                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    string PLU = "-";

                    if (r.Tipe == 1)
                    {
                        PLU = "";
                        var rslt = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == r.ID);

                        var item = (from p in rslt.SalesReceipt.SalesOrder.SalesOrderDJ

                                    select new { PLU = p.StockProductDJ.Nomor }).Union
                                   (from p in rslt.SalesReceipt.SalesOrder.SalesOrderPG

                                    select new { PLU = p.StockProductPG.Nomor }).Union
                                    (from p in rslt.SalesReceipt.SalesOrder.SalesOrderGJ

                                     select new { PLU = p.StockProductGJ.Nomor }).Union
                                     (from p in rslt.SalesReceipt.SalesOrder.SalesOrderLD

                                      select new { PLU = p.StockProductLD_Stone1B.Nomor });

                        foreach (var it in item)
                        {
                            PLU += it.PLU + "<br />";
                        }
                    }

                    td = new TableCell();
                    td.Text = PLU;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.HorizontalAlign = HorizontalAlign.Right;
                    td.Text = Convert.ToDecimal(r.Nominal).ToString("N0");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.TipeLokasi == 0)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                    else if (r.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                    else if (r.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CustomerNama;
                    tr.Cells.Add(td);

                    string EDC = "-";
                    try
                    {
                        EDC = r.EDCNama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = EDC;
                    tr.Cells.Add(td);

                    string BankNama = "-";
                    try
                    {
                        BankNama = r.BankNama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = BankNama;
                    tr.Cells.Add(td);

                    string JenisKartu = "-";
                    try
                    {
                        JenisKartu = r.JenisKartu;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = JenisKartu;
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
                        CC = r.CardNama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = CC;
                    tr.Cells.Add(td);

                    string Program = "-";
                    try
                    {
                        Program = r.ProgramNama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = Program;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.MDR.ToString("N0");
                    tr.Cells.Add(td);

                    Total += Convert.ToDecimal(r.Nominal);
                    TotalMDR += r.MDR;

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
        jeniskartu.SelectedValue = Request.QueryString["jeniskartu"];
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
        href += "&jeniskartu=" + jeniskartu.SelectedValue;
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
        href += "&jeniskartu=" + jeniskartu.SelectedValue;
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
        href += "&jeniskartu=" + jeniskartu.SelectedValue;
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