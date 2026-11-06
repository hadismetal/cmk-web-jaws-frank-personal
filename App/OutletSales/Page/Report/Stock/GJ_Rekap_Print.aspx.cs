using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_Stock_GJ_Rekap_Print : System.Web.UI.Page
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
    protected int idlokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillTable(sender, e);
            fillHeader();
        }
    }
    protected void fillHeader()
    {

        var rs = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualGJ select p;

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
        decimal TotalMAll = 0;
        decimal TotalRAll = 0m;
        decimal TotalNettoWeight = 0m;
        decimal TotalGrossWeight = 0m;

        /*var dj = from p in db.StockProductGJ
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
                var item = from p in db.StockProductGJ
                           where p.StockProductGJ_CharProduct.ProductItem == r.ID
                           && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                           && p.StockActualGJ.Substorage == 1
                           select p;

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
                }

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah);
                }
                catch (Exception ex) { }

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

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
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

            tb.Rows.Add(tr1);

        }
        else if (GroupBy == 2)
        {
            var pl = from p in db.CharProductLevel
                     select p;
            foreach (var r in pl)
            {
                var item = from p in db.StockProductGJ
                           where p.StockProductGJ_CharProduct.ProductLevel == r.ID
                           && p.ReturnProduct == false && p.StatusPenjualan == 0
                           && p.StockActualGJ.Substorage == 1
                           select p;

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
                }

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah);
                }
                catch (Exception ex) { }

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

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
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

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 3)
        {
            var pc = from p in db.CharProductCategory
                     select p;
            foreach (var r in pc)
            {
                var item = from p in db.StockProductGJ
                           where p.StockProductGJ_CharProduct.ProductCategory == r.ID
                           && p.ReturnProduct == false && p.StatusPenjualan == 0
                           && p.StockActualGJ.Substorage == 1
                           select p;

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
                }

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah);
                }
                catch (Exception ex) { }

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

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
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

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 4)
        {
            var ps = from p in db.CharProductSegmen
                     select p;
            foreach (var r in ps)
            {
                var item = from p in db.StockProductGJ
                           where p.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                           && p.ReturnProduct == false && p.StatusPenjualan == 0
                           && p.StockActualGJ.Substorage == 1
                           select p;

                int UserID = App.UserID;
                var d = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (d.Count() > 0)
                {
                    item = item.Where(p => p.StockActualGJ.TipeLokasi == d.First().TipeLokasi && p.StockActualGJ.IDLokasi == d.First().IDLokasi);
                }

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
                }

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockProductGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockProductGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockProductGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockProductGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah);
                }
                catch (Exception ex) { }

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

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
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

            tb.Rows.Add(tr1);
        }
    }
}