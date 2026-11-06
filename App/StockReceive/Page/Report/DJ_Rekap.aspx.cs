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
    protected int uid { get { return App.UserID; } }
    protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) 
        {
            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);

            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);

            DLC.LocationList(cabang, tipe);
            DLC.DDLSupplier(supplier,0);

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
        decimal TotalCaratAll = 0m;
        decimal TotalButirAll = 0m;
        decimal TotalMAll = 0;
        decimal TotalRAll = 0m;
        decimal TotalNettoWeight = 0m;
        decimal TotalGrossWeight = 0m;
        decimal TotalJualAll = 0m;

        /*var dj = from p in db.StockReceiveDJ
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
                var item = from p in db.StockReceiveDJ
                           where p.StockReceiveDJ_CharProduct.ProductItem == r.ID
                           && /*p.ReturnProduct == false && p.StatusLebur == false &&*/ !p.Draft
                           //&& !(from s in db.SalesOrderDJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //    select s.IDProduct).Contains(p.ID)
                           select p;

                if (datepicker.Text != "" && datepicker2.Text != "")
                {
                    start = Convert.ToDateTime(datepicker.Text);
                    end = Convert.ToDateTime(datepicker2.Text);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(cabang.SelectedValue);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = supplier.SelectedValue;
                if (Supplier != "")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach(var s in item){
                        TotalButir += s.StockReceiveDJ_Stone1A.Sum(p=>p.TotalButir);
                        TotalCarat += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(1, r.ID, r.Nama);
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
                var item = from p in db.StockReceiveDJ
                           where p.StockReceiveDJ_CharProduct.ProductLevel == r.ID
                           && /*p.ReturnProduct == false && p.StatusLebur == false &&*/ !p.Draft
                           //&& !(from s in db.SalesOrderDJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (datepicker.Text != "" && datepicker2.Text != "")
                {
                    start = Convert.ToDateTime(datepicker.Text);
                    end = Convert.ToDateTime(datepicker2.Text);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(cabang.SelectedValue);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = supplier.SelectedValue;
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(2, r.ID, r.Nama);
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
                var item = from p in db.StockReceiveDJ
                           where p.StockReceiveDJ_CharProduct.ProductCategory == r.ID
                           && /*p.ReturnProduct == false && p.StatusLebur == false &&*/ !p.Draft
                           //&& !(from s in db.SalesOrderDJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (datepicker.Text != "" && datepicker2.Text != "")
                {
                    start = Convert.ToDateTime(datepicker.Text);
                    end = Convert.ToDateTime(datepicker2.Text);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(cabang.SelectedValue);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = supplier.SelectedValue;
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveDJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(3, r.ID, r.Nama);
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
                var item = from p in db.StockReceiveDJ
                           where p.StockReceiveDJ_PricingSegmen.ProductSegmen == r.ID
                           && /*p.ReturnProduct == false && p.StatusLebur == false &&*/ !p.Draft
                           //&& !(from s in db.SalesOrderDJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (datepicker.Text != "" && datepicker2.Text != "")
                {
                    start = Convert.ToDateTime(datepicker.Text);
                    end = Convert.ToDateTime(datepicker2.Text);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(cabang.SelectedValue);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = supplier.SelectedValue;
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveDJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(4, r.ID, r.Nama);
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
            var ps = from p in db.StockReceiveDJ
                     select p;
            var sp = ps.GroupBy(p => p.SupplierNama);
            foreach (var r in sp)
            {
                var item = from p in db.StockReceiveDJ
                           where p.SupplierNama == r.Key
                           && /*p.ReturnProduct == false && p.StatusLebur == false &&*/ !p.Draft
                           //&& !(from s in db.SalesOrderDJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (datepicker.Text != "" && datepicker2.Text != "")
                {
                    start = Convert.ToDateTime(datepicker.Text);
                    end = Convert.ToDateTime(datepicker2.Text);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(cabang.SelectedValue);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = supplier.SelectedValue;
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                int jlh = item.Count();
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveDJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        TotalButir += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalButir);
                        TotalCarat += s.StockReceiveDJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveDJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveDJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveDJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.ModalUSD);
                }
                catch (Exception ex) { }

                decimal AllJualRupiah = 0m;
                try
                {
                    AllJualRupiah = item.Sum(p => p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                }
                catch (Exception ex) { }

                if (item.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_Hyperlink(5, 0, r.Key);
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
    protected string TD_Hyperlink(int Group, int ItemGroup, string Nama)
    {
        string x = string.Empty;
        string url = string.Empty;
        string dari = string.Empty;
        string sampai = string.Empty;

        dari = datepicker.Text;
        sampai = datepicker2.Text;

        int ProductItem = 0;
        int ProductLevel = 0;
        int ProductCategory = 0;
        int Cabang = 0;
        string Supplier = "";

        ProductItem = Convert.ToInt32(productitem.SelectedValue);
        ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        Cabang = Convert.ToInt32(cabang.SelectedValue);
        Supplier = supplier.SelectedValue;

        if (Group == 1)
            ProductItem = ItemGroup;
        else if (Group == 2)
            ProductLevel = ItemGroup;
        else if (Group == 3)
            ProductCategory = ItemGroup;
        
        url += "DJ_List.aspx";
        url += "?refreshPage=1";
        url += "&tipe=" + tipe;
        url += "&cabang=" + Cabang;
        url += "&productitem=" + ProductItem;
        url += "&productlevel=" + ProductLevel;
        url += "&productcategory=" + ProductCategory;
        url += "&supplier=" + Supplier;
        url += "&dari=" + dari;
        url += "&sampai=" + sampai;
        x += "<a href='" + url + "' target='_blank'>" + Nama + "</a>";


        return x;
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_RekapExport.aspx?id=1";

        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_Rekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.SelectedValue;
        href += "&productlevel=" + productlevel.SelectedValue;
        href += "&productcategory=" + productcategory.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}