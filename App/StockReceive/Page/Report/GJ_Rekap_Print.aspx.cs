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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		if (!IsPostBack) {

            //Generate
            fillTable(sender, e);
            fillHeader();
		}
	}

    protected void fillHeader()
    {

        var rs = from p in db.StockReceiveGJ
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                           //     select s.IDProduct).Contains(p.ID)
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualGJ select p;     

        if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rsB.SingleOrDefault(p => p.IDLokasi == Convert.ToInt32(Request.QueryString["cabang"]) && p.TipeLokasi == Convert.ToInt32(Request.QueryString["tipe"])); cabang.InnerText = v_cab.NamaLokasi; } else { cabang.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsL.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }
        if (Request.QueryString["supplier"] != "") { supplier.InnerText = Request.QueryString["supplier"]; } else { supplier.InnerText = "Semua"; }

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

        /*var GJ = from p in db.StockReceiveGJ
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
                var item = from p in db.StockReceiveGJ
                           where p.StockReceiveGJ_CharProduct.ProductItem == r.ID
                           //&& p.ReturnProduct == false && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
                {
                    start = Convert.ToDateTime(Request.QueryString["dari"]);
                    end = Convert.ToDateTime(Request.QueryString["sampai"]);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = Request.QueryString["supplier"];
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach(var s in item)
                    {
                        //TotalButir += s.StockReceiveGJ_Stone1A.Sum(p=>p.TotalButir);
                        //TotalCarat += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total * p.StockReceiveGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveGJ_PricingMU.ModalRupiah);
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
                td.Text = TotalButir.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllRRupiah.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
                TotalNettoWeight += NettoWeight;
                TotalGrossWeight += GrossWeight;
                TotalMAll += AllMRupiah;
                TotalRAll += AllRRupiah;
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
            td1.Text = "<b>" + TotalMAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);

        }
        else if (GroupBy == 2)
        {
            var pl = from p in db.CharProductLevel
                     select p;
            foreach (var r in pl)
            {
                var item = from p in db.StockReceiveGJ
                           where p.StockReceiveGJ_CharProduct.ProductLevel == r.ID
                           //&& p.ReturnProduct == false && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
                {
                    start = Convert.ToDateTime(Request.QueryString["dari"]);
                    end = Convert.ToDateTime(Request.QueryString["sampai"]);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = Request.QueryString["supplier"];
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        //TotalButir += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalButir);
                        //TotalCarat += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total * p.StockReceiveGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveGJ_PricingMU.ModalRupiah);
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
                td.Text = TotalButir.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllRRupiah.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
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

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 3)
        {
            var pc = from p in db.CharProductCategory
                     select p;
            foreach (var r in pc)
            {
                var item = from p in db.StockReceiveGJ
                           where p.StockReceiveGJ_CharProduct.ProductCategory == r.ID
                           //&& p.ReturnProduct == false && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
                {
                    start = Convert.ToDateTime(Request.QueryString["dari"]);
                    end = Convert.ToDateTime(Request.QueryString["sampai"]);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = Request.QueryString["supplier"];
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        //TotalButir += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalButir);
                        //TotalCarat += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total * p.StockReceiveGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveGJ_PricingMU.ModalRupiah);
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
                td.Text = TotalButir.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllRRupiah.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
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

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
        else if (GroupBy == 4)
        {
            var ps = from p in db.CharProductSegmen
                     select p;
            foreach (var r in ps)
            {
                var item = from p in db.StockReceiveGJ
                           where p.StockReceiveGJ_PricingSegmen.ProductSegmen == r.ID
                           //&& p.ReturnProduct == false && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
                {
                    start = Convert.ToDateTime(Request.QueryString["dari"]);
                    end = Convert.ToDateTime(Request.QueryString["sampai"]);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = Request.QueryString["supplier"];
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        //TotalButir += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalButir);
                        //TotalCarat += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total * p.StockReceiveGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveGJ_PricingMU.ModalRupiah);
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
                td.Text = TotalButir.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllRRupiah.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
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

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }

        else if (GroupBy == 5)
        {
            var ps = from p in db.StockReceiveGJ
                     select p;
            var sp = ps.GroupBy(p => p.SupplierNama);
            foreach (var r in sp)
            {
                var item = from p in db.StockReceiveGJ
                           where p.SupplierNama == r.Key
                           //&& p.ReturnProduct == false && p.StatusLebur == false
                           //&& !(from s in db.SalesOrderGJ
                           //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                           //     select s.IDProduct).Contains(p.ID)
                           select p;

                if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
                {
                    start = Convert.ToDateTime(Request.QueryString["dari"]);
                    end = Convert.ToDateTime(Request.QueryString["sampai"]);
                    item = item.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) item = item.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
                if (ProductItem > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
                }
                int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
                if (ProductLevel > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
                }
                int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
                if (ProductCategory > 0)
                {
                    item = item.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);
                }

                string Supplier = Request.QueryString["supplier"];
                if (Supplier != "0")
                    item = item.Where(p => p.SupplierNama.Equals(Supplier));

                //int jlh = item.Count();
                int jlh = Convert.ToInt32(item.Count());
                decimal NettoWeight = 0m;
                try
                {
                    NettoWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.NettoWeight);
                }
                catch (Exception ex) { }
                decimal GrossWeight = 0m;
                try
                {
                    GrossWeight = item.Sum(p => p.StockReceiveGJ_CharProduct.GrossWeight);
                }
                catch (Exception ex) { }
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                try
                {
                    foreach (var s in item)
                    {
                        //TotalButir += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalButir);
                        //TotalCarat += s.StockReceiveGJ_Stone1A.Sum(p => p.TotalCarat);
                    }
                }
                catch (Exception ex) { }

                decimal AllM = 0m;

                decimal x = 0m;
                decimal z = 0m;

                try
                {
                    x = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total);
                    z = item.Sum(p => p.StockReceiveGJ_Costing.TotalBiaya);
                }
                catch (Exception ex) { }

                AllM = (x + z);

                decimal AllMRupiah = 0m;
                try
                {
                    AllMRupiah = item.Sum(p => p.StockReceiveGJ_CostingProduct.Total * p.StockReceiveGJ_Costing.TotalRate.Value);
                }
                catch (Exception ex) { }

                decimal AllRRupiah = 0m;
                try
                {
                    AllRRupiah = item.Sum(p => p.StockReceiveGJ_PricingMU.ModalRupiah);
                }
                catch (Exception ex) { }

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
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllRRupiah.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Qty += jlh;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
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

            td1 = new TableCell();
            td1.Text = "<b>" + TotalButirAll.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalMAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + TotalRAll.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
    }
}