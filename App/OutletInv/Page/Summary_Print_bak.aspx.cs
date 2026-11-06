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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
        }
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        fillDJ();
        fillGJ();
        fillPG();
        fillLD();
        fillMonting();
        fillTotalStock();
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

    protected void fillDJ()
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

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
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
            string Lokasi = "";
            var x = d.First();

            if (x.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            rs = rs.Where(p => p.TipeLokasi == x.TipeLokasi && p.IDLokasi == x.IDLokasi);
        }


        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);

        }

        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);

        }

        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        decimal StockDJ = 0m;
        try
        {
            //StockDJ = rs.Sum(p => p.StockActualDJ.InHand);
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

        TotalQty += rs.Count();
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StoneQty;
        TotalCarat += StoneCarat;
        TotalM += HargaM;
        TotalR += HargaR;
    }

    protected void fillGJ()
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

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
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


        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == ProductItem);

        }

        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == ProductLevel);

        }

        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        decimal StockGJ = 0m;
        try
        {
            //StockGJ = rs.Sum(p => p.StockActualGJ.InHand);
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

        TotalQty += rs.Count();
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
        TotalR += HargaR;
    }

    protected void fillPG()
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

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
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


        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        decimal StockPG = 0m;
        try
        {
            //StockPG = rs.Sum(p => p.StockActualPG.InHand);
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

        TotalQty += rs.Count();
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
    }

    protected void fillLD()
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

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
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


        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualLD_Stone1B.IDLokasi) && p.StockActualLD_Stone1B.TipeLokasi == 1);
        }

        decimal StockLD = 0m;
        try
        {
            //StockLD = rs.Sum(p => p.StockActualLD_Stone1B.InHand);
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

        TotalQty += rs.Count();
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StockLD;
        TotalCarat += Carat;
        TotalM += HargaM;
        TotalR += HargaR;
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


        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualMounting.IDLokasi) && p.StockActualMounting.TipeLokasi == 1);
        }

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        //if (Substorage == 0)
            //Substorage = 1;

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
}