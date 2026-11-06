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
    decimal TotalQty = 0m;
    decimal TotalHargaJual = 0m;
    decimal TotalM = 0m;
    decimal TotalR = 0m;
    decimal TotalGrossWeight = 0m;
    decimal TotalNetWeight = 0m;
    decimal TotalStone = 0m;
    decimal TotalCarat = 0m;

    int TotalDJS1 = 0;
    int TotalDJS2 = 0;
    int TotalDJS3 = 0;
    int TotalDJS4 = 0;
    int TotalDJS5 = 0;
    int TotalDJS6 = 0;
    int TotalDJS7 = 0;
    int TotalDJS95 = 0;
    int TotalDJS96 = 0;
    int TotalDJS97 = 0;
    int TotalDJS98 = 0;
    int TotalDJS99 = 0;

    int TotalGJS1 = 0;
    int TotalGJS2 = 0;
    int TotalGJS3 = 0;
    int TotalGJS4 = 0;
    int TotalGJS5 = 0;
    int TotalGJS6 = 0;
    int TotalGJS7 = 0;
    int TotalGJS95 = 0;
    int TotalGJS96 = 0;
    int TotalGJS97 = 0;
    int TotalGJS98 = 0;
    int TotalGJS99 = 0;

    int TotalPGS1 = 0;
    int TotalPGS2 = 0;
    int TotalPGS3 = 0;
    int TotalPGS4 = 0;
    int TotalPGS5 = 0;
    int TotalPGS6 = 0;
    int TotalPGS7 = 0;
    int TotalPGS95 = 0;
    int TotalPGS96 = 0;
    int TotalPGS97 = 0;
    int TotalPGS98 = 0;
    int TotalPGS99 = 0;

    int TotalLDS1 = 0;
    int TotalLDS2 = 0;
    int TotalLDS3 = 0;
    int TotalLDS4 = 0;
    int TotalLDS5 = 0;
    int TotalLDS6 = 0;
    int TotalLDS7 = 0;
    int TotalLDS95 = 0;
    int TotalLDS96 = 0;
    int TotalLDS97 = 0;
    int TotalLDS98 = 0;
    int TotalLDS99 = 0;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);

            //DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void getLokasi(object sender, EventArgs e)
    {
        location.Items.Clear();
        if (locationtype.SelectedValue == "0")
        {
            DLC.LocationList(location, 0);
        }
        else if (locationtype.SelectedValue == "1")
        {
            DLC.LocationList(location, 1);
        }
        else if (locationtype.SelectedValue == "2")
        {
            DLC.LocationList(location, 2);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        int Tipe = Convert.ToInt32(Request.QueryString["locationtype"]);

        int Lokasi = 0;
        try
        {
            Lokasi = Convert.ToInt32(Request.QueryString["location"]);
        }
        catch (Exception ex) { }

        fillDJ(Tipe, Lokasi);
        fillGJ(Tipe, Lokasi);
        fillPG(Tipe, Lokasi);
        fillLD(Tipe, Lokasi);
        fillMonting();
        fillTotalStock();
        int Substorage = Convert.ToInt32(substorage.SelectedValue);

        if (Substorage == 0)
        {
            fillStockSubstorage();
        }
    }

    protected void fillStockSubstorage()
    {
        tb4.Visible = true;

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "Diamond Jewellery";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS1.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS2.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS3.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS4.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS5.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS6.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS7.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS95.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS96.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS97.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS98.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + TotalDJS99.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + (TotalDJS1 + TotalDJS2 + TotalDJS3 + TotalDJS4 + TotalDJS5 + TotalDJS6 + TotalDJS7 + TotalDJS95 + TotalDJS96 + TotalDJS97 + TotalDJS98 + TotalDJS99).ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb4.Rows.Add(tr);

        TableRow tr2 = new TableRow();
        TableCell td2 = new TableCell();

        td2 = new TableCell();
        td2.Text = "Gold Jewellery";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS1.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS2.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS3.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS4.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS5.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS6.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS7.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS95.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS96.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS97.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS98.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + TotalGJS99.ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + (TotalGJS1 + TotalGJS2 + TotalGJS3 + TotalGJS4 + TotalGJS5 + TotalGJS6 + TotalGJS7 + TotalGJS95 + TotalGJS96 + TotalGJS97 + TotalGJS98 + TotalGJS99).ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        tb4.Rows.Add(tr2);

        TableRow tr3 = new TableRow();
        TableCell td3 = new TableCell();

        td3 = new TableCell();
        td3.Text = "Plain Gold";
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS1.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS2.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS3.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS4.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS5.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS6.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS7.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS95.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS96.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS97.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS98.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + TotalPGS99.ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + (TotalPGS1 + TotalPGS2 + TotalPGS3 + TotalPGS4 + TotalPGS5 + TotalPGS6 + TotalPGS7 + TotalPGS95 + TotalPGS96 + TotalPGS97 + TotalPGS98 + TotalPGS99).ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        tb4.Rows.Add(tr3);

        TableRow tr4 = new TableRow();
        TableCell td4 = new TableCell();

        td4 = new TableCell();
        td4.Text = "Loose Diamond";
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS1.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS2.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS3.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS4.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS5.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS6.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS7.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS95.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS96.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS97.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS98.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + TotalLDS99.ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + (TotalLDS1 + TotalLDS2 + TotalLDS3 + TotalLDS4 + TotalLDS5 + TotalLDS6 + TotalLDS7 + TotalLDS95 + TotalLDS96 + TotalLDS97 + TotalLDS98 + TotalLDS99).ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        tb4.Rows.Add(tr4);

        TableRow tr5 = new TableRow();
        TableCell td5 = new TableCell();

        td5 = new TableCell();
        td5.Text = "All";
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS1+TotalGJS1+TotalPGS1+TotalLDS1).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS2 + TotalGJS2 + TotalPGS2 + TotalLDS2).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS3 + TotalGJS3 + TotalPGS3 + TotalLDS3).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS4 + TotalGJS4 + TotalPGS4 + TotalLDS4).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS5 + TotalGJS5 + TotalPGS5 + TotalLDS5).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS6 + TotalGJS6 + TotalPGS6 + TotalLDS6).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS7 + TotalGJS7 + TotalPGS7 + TotalLDS7).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS95 + TotalGJS95 + TotalPGS95 + TotalLDS95).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS96 + TotalGJS96 + TotalPGS96 + TotalLDS96).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS97 + TotalGJS97 + TotalPGS97 + TotalLDS97).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS98 + TotalGJS98 + TotalPGS98 + TotalLDS98).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS99 + TotalGJS99 + TotalPGS99 + TotalLDS99).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + ((TotalDJS1 + TotalDJS2 + TotalDJS3 + TotalDJS4 + TotalDJS5 + TotalDJS6 + TotalDJS7 + TotalDJS95 + TotalDJS96 + TotalDJS97 + TotalDJS98 + TotalDJS99) + (TotalGJS1 + TotalGJS2 + TotalGJS3 + TotalGJS4 + TotalGJS5 + TotalGJS6 + TotalGJS7 + TotalGJS95 + TotalGJS96 + TotalGJS97 + TotalGJS98 + TotalGJS99) + (TotalPGS1 + TotalPGS2 + TotalPGS3 + TotalPGS4 + TotalPGS5 + TotalPGS6 + TotalPGS7 + TotalPGS95 + TotalPGS96 + TotalPGS97 + TotalPGS98 + TotalPGS99) + (TotalLDS1 + TotalLDS2 + TotalLDS3 + TotalLDS4 + TotalLDS5 + TotalLDS6 + TotalLDS7 + TotalLDS95 + TotalLDS96 + TotalLDS97 + TotalLDS98 + TotalLDS99)).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        tb4.Rows.Add(tr5);
    }

    protected void fillTotalStock()
    {
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Total</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalQty.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalHargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);



        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalGrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalNetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalCarat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-info\">" + TotalStone.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

    }

    protected void fillDJ(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualDJ
                 where p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false && !p.StockProductDJ.Draft.Value
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi != 0)
                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            rs = rs.Where(p => p.TipeLokasi == Tipe);

        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            string NamaLokasi = "";
            var x = d.First();

            if (x.TipeLokasi == 1)
                NamaLokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                NamaLokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            rs = rs.Where(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi);
        }

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);

        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);

        }

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        decimal StockDJ = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99 && Substorage!=7)
            //    StockDJ = rs.Sum(p => p.StockActualDJ.InHand);
            //else
                StockDJ = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductDJ.StockProductDJ_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductDJ.StockProductDJ_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }


        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        decimal StoneQty = 0m;
        try
        {
            StoneQty = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value);
        }
        catch (Exception ex) { }

        decimal StoneCarat = 0m;
        try
        {
            StoneCarat = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Diamond Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockDJ.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);



        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StoneCarat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StoneQty.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockDJ;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StoneQty;
        TotalCarat += StoneCarat;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalDJS1 = Convert.ToInt32(rs.Where(p => p.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalDJS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalDJS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalDJS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillGJ(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualGJ
                 where p.StockProductGJ.ReturnProduct == false && p.StockProductGJ.StatusLebur == false
                           && !(from s in db.SalesOrderGJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            rs = rs.Where(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi);
        }

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == ProductItem);

        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == ProductLevel);

        }

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        decimal StockGJ = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99 && Substorage != 7)
            //    StockGJ = rs.Sum(p => p.InHand);
            //else
                StockGJ = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductGJ.StockProductGJ_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Gold Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockGJ.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);



        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockGJ;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalGJS1 = Convert.ToInt32(rs.Where(p => p.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalGJS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalGJS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalGJS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillPG(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualPG
                 where p.StockProductPG.ReturnProduct == false && p.StockProductPG.StatusLebur == false
                           && !(from s in db.SalesOrderPG
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            rs = rs.Where(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi);
        }

        decimal StockPG = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99 && Substorage != 7)
            //    StockPG = rs.Count();
            //else
                StockPG = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;

        decimal HargaM = 0m;

        try
        {
            HargaM = rs.Sum(p => p.StockProductPG.HargaUSD.Value);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductPG.GrossWeight.Value);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductPG.GrossWeight.Value);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Plain Gold</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockPG.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockPG;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;

        try
        {
            TotalPGS1 = Convert.ToInt32(rs.Where(p => p.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalPGS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalPGS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalPGS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillLD(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false
                           && !(from s in db.SalesOrderLD
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == x.TipeLokasi && p.StockActualLD_Stone1B.IDLokasi == x.IDLokasi);
        }

        decimal StockLD = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99 && Substorage != 7)
            //    StockLD = rs.Sum(p => p.StockActualLD_Stone1B.InHand);
            //else
                StockLD = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;

        decimal HargaM = 0m;

        try
        {
            HargaM = rs.Sum(p => p.HargaTotalM.Value);
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;

        try
        {
            HargaR = rs.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;

        decimal NetWeight = 0m;

        decimal Carat = 0m;

        try
        {
            Carat = rs.Sum(p => p.TotalCarat);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Loose Diamond</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockLD.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);



        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Carat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockLD.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockLD;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StockLD;
        TotalCarat += Carat;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalLDS1 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 1).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS2 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 2).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS3 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 3).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS4 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 4).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS5 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 5).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS6 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 6).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS7 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS95 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 95).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS96 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 96).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        TotalLDS97 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 97).Count();
        TotalLDS98 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 98).Count();
        TotalLDS99 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 99).Count();
    }

    protected void fillMonting()
    {
        var rs = from p in db.StockProductMounting
                 where p.StatusReceive == false
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            rs = rs.Where(p => p.StockActualMounting.TipeLokasi == x.TipeLokasi && p.StockActualMounting.IDLokasi == x.IDLokasi);
        }

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.StockActualMounting.Substorage == Substorage);

        decimal StockMounting = 0m;
        try
        {
            StockMounting = rs.Sum(p => p.StockActualMounting.InHand);
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductMounting_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductMounting_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductMounting_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductMounting_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductMounting_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductMounting_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Mounting</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + StockMounting.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += rs.Count();
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
        TotalR += HargaR;

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            location.Items.Clear();
            if (Request.QueryString["locationtype"] == "0")
            {
                DLC.LocationList(location, 0);
            }
            else if (Request.QueryString["locationtype"] == "1")
            {
                DLC.LocationList(location, 1);
            }
            else if (Request.QueryString["locationtype"] == "2")
            {
                DLC.LocationList(location, 2);
            }

            //cabang.SelectedValue = Request.QueryString["cabang"];
            locationtype.SelectedValue = Request.QueryString["locationtype"];
            location.SelectedValue = Request.QueryString["location"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            substorage.SelectedValue = Request.QueryString["substorage"];
        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            string NamaLokasi = "All";
            var x = d.First();

            if (x.TipeLokasi == 1)
                NamaLokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                NamaLokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            

        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        //href += "&cabang=" + cabang.Text;
        href += "&locationtype=" + locationtype.Text;
        href += "&location=" + location.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Summary_Print.aspx?id=1";

        //Customize-Qstr
        //href += "&cabang=" + cabang.Text;
        href += "&locationtype=" + locationtype.Text;
        href += "&location=" + location.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}