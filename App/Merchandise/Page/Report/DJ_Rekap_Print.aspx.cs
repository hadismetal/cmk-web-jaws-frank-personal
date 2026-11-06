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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        if (!IsPostBack)
        {

            //Generate
            fillTable(sender, e);
            fillHeader();
        }
    }

    protected void fillHeader()
    {

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0 && !p.Draft.Value //&& p.Legacy == null
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualDJ select p;     

        int Cabang =  Convert.ToInt32(Request.QueryString["cabang"]);
        int Tipe = Convert.ToInt32(Request.QueryString["tipe"]);

        string Lokasi = "Semua";
        if (Cabang > 0)
        {
            if (Tipe == 0)
                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + Cabang, Db.Cnn_DB);
            else if(Tipe == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + Cabang, Db.Cnn_DB);
            else if(Tipe == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + Cabang, Db.Cnn_DB);
        }

        cabang.InnerText = Lokasi;
        
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsL.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }


        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        int Qty = 0;
        decimal TotalCaratAll = 0m;
        decimal TotalButirAll = 0m;
        decimal TotalMAll = 0;
        decimal TotalRAll = 0m;
        decimal TotalNettoWeight = 0m;
        decimal TotalGrossWeight = 0m;
        decimal TotalJualAll = 0m;

        /*var dj = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var pg = from p in db.StockProductPG
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;*/

        int GroupBy = Convert.ToInt32(Request.QueryString["groupby"]);
        if (GroupBy == 1)
        {
            var pi = from p in db.CharProductItem
                     select p;
            foreach (var r in pi)
            {
                var item = from p in db.StockProductDJ
                           where p.StockProductDJ_CharProduct.ProductItem == r.ID
                           && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                           select p;

                int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
                if (Substorage > 0)
                {
                    item = item.Where(p => p.StockActualDJ.Substorage == Substorage);
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                if (Cabang > 0)
                {
                    item = item.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
                }
                else
                {
                    item = item.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Where(p => p.StockActualDJ.InHand == 1m).Count());

                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockProductDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockProductDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = jlh.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllMRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllRRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllJualRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Qty += jlh;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalNettoWeight += NettoWeight;
                    TotalGrossWeight += GrossWeight;
                    TotalMAll += AllMRupiah;
                    TotalRAll += AllRRupiah;
                    TotalJualAll += AllJualRupiah;
                }
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalNettoWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalJualAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);

        }
        else if (GroupBy == 2)
        {
            var pl = from p in db.CharProductLevel
                     select p;
            foreach (var r in pl)
            {
                var item = from p in db.StockProductDJ
                           where p.StockProductDJ_CharProduct.ProductLevel == r.ID
                           && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           select p;

                int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
                if (Substorage > 0)
                {
                    item = item.Where(p => p.StockActualDJ.Substorage == Substorage);
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                if (Cabang > 0)
                {
                    item = item.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
                }
                else
                {
                    item = item.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Where(p => p.StockActualDJ.InHand == 1m).Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockProductDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockProductDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = jlh.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllMRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllRRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllJualRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Qty += jlh;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalNettoWeight += NettoWeight;
                    TotalGrossWeight += GrossWeight;
                    TotalMAll += AllMRupiah;
                    TotalRAll += AllRRupiah;
                    TotalJualAll += AllJualRupiah;
                }
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalNettoWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalJualAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 3)
        {
            var pc = from p in db.CharProductCategory
                     select p;
            foreach (var r in pc)
            {
                var item = from p in db.StockProductDJ
                           where p.StockProductDJ_CharProduct.ProductCategory == r.ID
                           && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                           select p;

                int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
                if (Substorage > 0)
                {
                    item = item.Where(p => p.StockActualDJ.Substorage == Substorage);
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                if (Cabang > 0)
                {
                    item = item.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
                }
                else
                {
                    item = item.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Where(p => p.StockActualDJ.InHand == 1m).Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockProductDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockProductDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    //AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalRate.Value);
                    AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya) + item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }


                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = jlh.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllMRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllRRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllJualRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Qty += jlh;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalNettoWeight += NettoWeight;
                    TotalGrossWeight += GrossWeight;
                    TotalMAll += AllMRupiah;
                    TotalRAll += AllRRupiah;
                    TotalJualAll += AllJualRupiah;
                }
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalNettoWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalJualAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 4)
        {
            var ps = from p in db.CharProductSegmen
                     select p;
            foreach (var r in ps)
            {
                var item = from p in db.StockProductDJ
                           where p.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                           && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                           select p;

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                if (Cabang > 0)
                {
                    item = item.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
                }
                else
                {
                    item = item.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Where(p => p.StockActualDJ.InHand == 1m).Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockProductDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockProductDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    //AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalRate.Value);
                    AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya) + item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = jlh.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllMRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllRRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllJualRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Qty += jlh;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalNettoWeight += NettoWeight;
                    TotalGrossWeight += GrossWeight;
                    TotalMAll += AllMRupiah;
                    TotalRAll += AllRRupiah;
                    TotalJualAll += AllJualRupiah;
                }
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalNettoWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalJualAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }

        else if (GroupBy == 5)
        {
            var ps = from p in db.StockProductDJ
                     select p;
            var sp = ps.GroupBy(p => p.SupplierNama);
            foreach (var r in sp)
            {
                var item = from p in db.StockProductDJ
                           where p.SupplierNama == r.Key
                           && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                           select p;

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                if (Cabang > 0)
                {
                    item = item.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
                }
                else
                {
                    item = item.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Where(p => p.StockActualDJ.InHand == 1m).Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockProductDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockProductDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    //AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalRate.Value);
                    AllMRupiah = item.Sum(p => p.StockProductDJ_Costing.TotalBiaya) + item.Sum(p => p.StockProductDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }


                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Key;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = jlh.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllMRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllRRupiah.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = AllJualRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Qty += jlh;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalNettoWeight += NettoWeight;
                    TotalGrossWeight += GrossWeight;
                    TotalMAll += AllMRupiah;
                    TotalRAll += AllRRupiah;
                    TotalJualAll += AllJualRupiah;
                }
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + Qty.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalNettoWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalJualAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
    }
}