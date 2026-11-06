using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {

            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
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
        decimal TotalHAll = 0m;

        /*var dj = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var pg = from p in db.StockProductPG
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;*/

        int GroupBy = Convert.ToInt32(groupby.SelectedValue);
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

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
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

                decimal TotalHarga = 0m;
                try
                {
                    TotalHarga = item.Sum(p => p.StockProductGJ_PricingMU.HargaRupiahRound.Value);
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

                td = new TableCell();
                td.Text = TotalHarga.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
                TotalHAll += TotalHarga;
            }

            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>" ;
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
            td1.Text = "<b>" + TotalHAll.ToString("N2") + "</b>";
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

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
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

                decimal TotalHarga = 0m;
                try
                {
                    TotalHarga = item.Sum(p => p.StockProductGJ_PricingMU.HargaRupiahRound.Value);
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

                td = new TableCell();
                td.Text = TotalHarga.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
                TotalHAll += TotalHarga;
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
            td1.Text = "<b>" + TotalHAll.ToString("N2") + "</b>";
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

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
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

                decimal TotalHarga = 0m;
                try
                {
                    TotalHarga = item.Sum(p => p.StockProductGJ_PricingMU.HargaRupiahRound.Value);
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

                td = new TableCell();
                td.Text = TotalHarga.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
                TotalHAll += TotalHarga;
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
            td1.Text = "<b>" + TotalHAll.ToString("N2") + "</b>";
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

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
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

                decimal TotalHarga = 0m;
                try
                {
                    TotalHarga = item.Sum(p => p.StockProductGJ_PricingMU.HargaRupiahRound.Value);
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

                td = new TableCell();
                td.Text = TotalHarga.ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
                TotalHAll += TotalHarga;
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
            td1.Text = "<b>" + TotalHAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }

        

        /*dj = dj.OrderByDescending(p => p.Tgl);
        pg = pg.OrderByDescending(p => p.Tgl);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
        {
            dj = dj.Where(p => p.StockActualGJ.IDLokasi == Cabang && p.StockActualGJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
        }
        else
        {
            dj = dj.Where(p => p.StockActualGJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe);
        }
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            dj = dj.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
            pg = pg.Where(p => p.ProductItem == ProductItem);
        }

        if (dj != null)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "Diamond Jewellery";
            tr.Cells.Add(td);

            int stock1 = dj.Where(p => p.StockActualGJ.Substorage == 1).Count();
            int stock2 = dj.Where(p => p.StockActualGJ.Substorage == 2).Count();
            int stock3 = dj.Where(p => p.StockActualGJ.Substorage == 3).Count();
            int stock4 = dj.Where(p => p.StockActualGJ.Substorage == 4).Count();
            int stock5 = dj.Where(p => p.StockActualGJ.Substorage == 5).Count();
            int stock6 = dj.Where(p => p.StockActualGJ.Substorage == 6).Count();
            int stock96 = dj.Where(p => p.StockActualGJ.Substorage == 96).Count();
            int stock97 = dj.Where(p => p.StockActualGJ.Substorage == 97).Count();
            int stock98 = dj.Where(p => p.StockActualGJ.Substorage == 98).Count();
            int stock99 = dj.Where(p => p.StockActualGJ.Substorage == 99).Count();
            int stockAll = dj.Count();

            decimal netweight1 = 0m;
            decimal netweight2 = 0m;
            decimal netweight3 = 0m;
            decimal netweight4 = 0m;
            decimal netweight5 = 0m;
            decimal netweight6 = 0m;
            decimal netweight96 = 0m;
            decimal netweight97 = 0m;
            decimal netweight98 = 0m;
            decimal netweight99 = 0m;
            decimal netweightAll = 0m;

            try{
                netweightAll = Convert.ToDecimal(dj.Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
            }
            catch (Exception ex) { };

            decimal grossweight1 = 0m;
            decimal grossweight2 = 0m;
            decimal grossweight3 = 0m;
            decimal grossweight4 = 0m;
            decimal grossweight5 = 0m;
            decimal grossweight6 = 0m;
            decimal grossweight96 = 0m;
            decimal grossweight97 = 0m;
            decimal grossweight98 = 0m;
            decimal grossweight99 = 0m;
            decimal grossweightAll = 0m;

            try
            {
                grossweightAll = Convert.ToDecimal(dj.Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
            }
            catch (Exception ex) { };

            decimal HargaModal1 = 0m;
            decimal HargaModal2 = 0m;
            decimal HargaModal3 = 0m;
            decimal HargaModal4 = 0m;
            decimal HargaModal5 = 0m;
            decimal HargaModal6 = 0m;
            decimal HargaModal96 = 0m;
            decimal HargaModal97 = 0m;
            decimal HargaModal98 = 0m;
            decimal HargaModal99 = 0m;
            decimal HargaModalAll = 0m;

            try
            {
                decimal AllM = 0m;

                decimal x = dj.Sum(p => p.StockProductGJ_CostingProduct.Total);
                decimal z = dj.Sum(p => p.StockProductGJ_Costing.TotalBiaya);

                AllM = (x + z);

                decimal AllMRupiah = dj.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);

                HargaModalAll = AllMRupiah;
            }
            catch (Exception ex) { };

            decimal HargaRupiah1 = 0m;
            decimal HargaRupiah2 = 0m;
            decimal HargaRupiah3 = 0m;
            decimal HargaRupiah4 = 0m;
            decimal HargaRupiah5 = 0m;
            decimal HargaRupiah6 = 0m;
            decimal HargaRupiah96 = 0m;
            decimal HargaRupiah97 = 0m;
            decimal HargaRupiah98 = 0m;
            decimal HargaRupiah99 = 0m;
            decimal HargaRupiahAll = 0m;

            try
            {
                HargaRupiahAll = Convert.ToDecimal(dj.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex) { };

            try
            {
                netweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
                HargaRupiah97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }
            decimal TotalButir_1 = 0m;
            decimal TotalCarat_1 = 0m;

            try
            {
                TotalButir_1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_1 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_2 = 0m;
            decimal TotalCarat_2 = 0m;

            try
            {
                TotalButir_2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_2 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_3 = 0m;
            decimal TotalCarat_3 = 0m;

            try
            {
                TotalButir_3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_3 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_4 = 0m;
            decimal TotalCarat_4 = 0m;

            try
            {
                TotalButir_4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_4 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_5 = 0m;
            decimal TotalCarat_5 = 0m;

            try
            {
                TotalButir_5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_5 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_6 = 0m;
            decimal TotalCarat_6 = 0m;

            try
            {
                TotalButir_6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_6 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_96 = 0m;
            decimal TotalCarat_96 = 0m;

            try
            {
                TotalButir_96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_96 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_97 = 0m;
            decimal TotalCarat_97 = 0m;

            try
            {
                TotalButir_97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_97 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_98 = 0m;
            decimal TotalCarat_98 = 0m;

            try
            {
                TotalButir_98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_98 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_99 = 0m;
            decimal TotalCarat_99 = 0m;

            try
            {
                TotalButir_99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
                TotalCarat_99 = Convert.ToDecimal(dj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_All = 0m;
            decimal TotalCarat_All = 0m;

            try
            {
                TotalButir_All = Convert.ToDecimal(dj.Sum(p => p.StockProductGJ_CharProduct.StoneQty.Value));
            }
            catch(Exception ex){}
            try
            {
                TotalCarat_All = Convert.ToDecimal(dj.Sum(p => p.StockProductGJ_CharProduct.StoneCarat));
            }
            catch(Exception ex){}

            if (stock1 > 0)
            {
                td = new TableCell();
                td.Text = DL.SubStorage(1);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = stock1.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = netweight1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = grossweight1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalButir_1.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat_1.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaModal1.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaRupiah1.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            if (stock2 > 0)
            {
                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "Diamond Jewellery";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = DL.SubStorage(2);
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = stock2.ToString();
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = netweight2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = grossweight2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = TotalButir_2.ToString("N0");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = TotalCarat_2.ToString("N3");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = HargaModal2.ToString("N0");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = HargaRupiah2.ToString("N0");
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }

            if (stock3 > 0)
            {
                TableRow tr3 = new TableRow();
                TableCell td3;

                td3 = new TableCell();
                td3.Text = "Diamond Jewellery";
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = DL.SubStorage(3);
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = stock3.ToString();
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = netweight3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = grossweight3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = TotalButir_3.ToString("N0");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = TotalCarat_3.ToString("N3");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = HargaModal3.ToString("N0");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = HargaRupiah3.ToString("N0");
                tr3.Cells.Add(td3);

                tb.Rows.Add(tr3);
            }

            if (stock4 > 0)
            {
                TableRow tr4 = new TableRow();
                TableCell td4;

                td4 = new TableCell();
                td4.Text = "Diamond Jewellery";
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = DL.SubStorage(4);
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = stock4.ToString();
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = netweight4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = grossweight4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = TotalButir_4.ToString("N0");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = TotalCarat_4.ToString("N3");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = HargaModal4.ToString("N0");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = HargaRupiah4.ToString("N0");
                tr4.Cells.Add(td4);

                tb.Rows.Add(tr4);
            }

            if (stock5 > 0)
            {
                TableRow tr5 = new TableRow();
                TableCell td5;

                td5 = new TableCell();
                td5.Text = "Diamond Jewellery";
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = DL.SubStorage(5);
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = stock5.ToString();
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = netweight5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = grossweight5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = TotalButir_5.ToString("N0");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = TotalCarat_5.ToString("N3");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = HargaModal5.ToString("N0");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = HargaRupiah5.ToString("N0");
                tr5.Cells.Add(td5);

                tb.Rows.Add(tr5);
            }

            if (stock6 > 0)
            {
                TableRow tr6 = new TableRow();
                TableCell td6;

                td6 = new TableCell();
                td6.Text = "Diamond Jewellery";
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = DL.SubStorage(6);
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = stock6.ToString();
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = netweight6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = grossweight6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = TotalButir_6.ToString("N0");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = TotalCarat_6.ToString("N3");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = HargaModal6.ToString("N0");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = HargaRupiah6.ToString("N0");
                tr6.Cells.Add(td6);

                tb.Rows.Add(tr6);
            }

            if (stock96 > 0)
            {
                TableRow tr7 = new TableRow();
                TableCell td7;

                td7 = new TableCell();
                td7.Text = "Diamond Jewellery";
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = DL.SubStorage(96);
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = stock96.ToString();
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = netweight96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = grossweight96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = TotalButir_96.ToString("N0");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = TotalCarat_96.ToString("N3");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = HargaModal96.ToString("N0");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = HargaRupiah96.ToString("N0");
                tr7.Cells.Add(td7);

                tb.Rows.Add(tr7);
            }

            if (stock97 > 0)
            {
                TableRow tr8 = new TableRow();
                TableCell td8;

                td8 = new TableCell();
                td8.Text = "Diamond Jewellery";
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = DL.SubStorage(97);
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = stock97.ToString();
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = netweight97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = grossweight97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = TotalButir_97.ToString("N0");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = TotalCarat_97.ToString("N3");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = HargaModal97.ToString("N0");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = HargaRupiah97.ToString("N0");
                tr8.Cells.Add(td8);

                tb.Rows.Add(tr8);
            }

            if (stock98 > 0)
            {

                TableRow tr9 = new TableRow();
                TableCell td9;

                td9 = new TableCell();
                td9.Text = "Diamond Jewellery";
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = DL.SubStorage(98);
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = stock98.ToString();
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = netweight98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = grossweight98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = TotalButir_98.ToString("N0");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = TotalCarat_98.ToString("N3");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = HargaModal98.ToString("N0");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = HargaRupiah98.ToString("N0");
                tr9.Cells.Add(td9);

                tb.Rows.Add(tr9);
            }

            if (stock99 > 0)
            {

                TableRow tr10 = new TableRow();
                TableCell td10;

                td10 = new TableCell();
                td10.Text = "Diamond Jewellery";
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = DL.SubStorage(99);
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = stock99.ToString();
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = netweight99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = grossweight99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = TotalButir_99.ToString("N0");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = TotalCarat_99.ToString("N3");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = HargaModal99.ToString("N0");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = HargaRupiah99.ToString("N0");
                tr10.Cells.Add(td10);

                tb.Rows.Add(tr10);
            }

            TableRow tr11 = new TableRow();
            TableCell td11;

            td11 = new TableCell();
            td11.Text = "<b>Total</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>-</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + stockAll.ToString() + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + netweightAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + grossweightAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + TotalButir_All.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + TotalCarat_All.ToString("N3") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaModalAll.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaRupiahAll.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            tb.Rows.Add(tr11);
        }*/

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            groupby.SelectedValue = Request.QueryString["groupby"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "GJ_Rekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
}