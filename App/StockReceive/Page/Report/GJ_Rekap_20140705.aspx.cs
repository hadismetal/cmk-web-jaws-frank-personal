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

            DLC.CharProductItem(productitem);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            DLC.LocationList(cabang, tipe);

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

        var gj = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        gj = gj.OrderByDescending(p => p.ApprovalTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            gj = gj.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
        {
            gj = gj.Where(p => p.IDWarehouse == Cabang);
            
        }
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            gj = gj.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
            
        }

        if (gj != null)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "Gold Jewellery";
            tr.Cells.Add(td);

            int stock1 = gj.Where(p => p.StockActualGJ.Substorage == 1).Count();
            int stock2 = gj.Where(p => p.StockActualGJ.Substorage == 2).Count();
            int stock3 = gj.Where(p => p.StockActualGJ.Substorage == 3).Count();
            int stock4 = gj.Where(p => p.StockActualGJ.Substorage == 4).Count();
            int stock5 = gj.Where(p => p.StockActualGJ.Substorage == 5).Count();
            int stock6 = gj.Where(p => p.StockActualGJ.Substorage == 6).Count();
            int stock96 = gj.Where(p => p.StockActualGJ.Substorage == 96).Count();
            int stock97 = gj.Where(p => p.StockActualGJ.Substorage == 97).Count();
            int stock98 = gj.Where(p => p.StockActualGJ.Substorage == 98).Count();
            int stock99 = gj.Where(p => p.StockActualGJ.Substorage == 99).Count();
            int stockAll = gj.Count();

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
                netweightAll = Convert.ToDecimal(gj.Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
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
                grossweightAll = Convert.ToDecimal(gj.Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
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

                decimal x = gj.Sum(p=>p.StockProductGJ_CostingProduct.Total);
                decimal z = gj.Sum(p=>p.StockProductGJ_Costing.TotalBiaya);

                AllM = (x+z);

                decimal AllMRupiah = gj.Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value);

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
                HargaRupiahAll = Convert.ToDecimal(gj.Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex) { };

            try
            {

                netweight1 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight1 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal1 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah1 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 1).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweight2 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight2 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal2 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah2 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 2).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight3 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p=>p.StockProductGJ_CharProduct.NettoWeight));
                grossweight3 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal3 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah3 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 3).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight4 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight4 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal4 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah4 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 4).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight5 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight5 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal5 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah5 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 5).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight6 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight6 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal6 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah6 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 6).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight96 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight96 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal96 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah96 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 96).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight97 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight97 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal97 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah97 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 97).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight98 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight98 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal98 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah98 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 98).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight99 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.NettoWeight));
                grossweight99 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CharProduct.GrossWeight));
                HargaModal99 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_CostingProduct.Total * p.StockProductGJ_Costing.TotalRate.Value));
                HargaRupiah99 = Convert.ToDecimal(gj.Where(p => p.StockActualGJ.Substorage == 99).Sum(p => p.StockProductGJ_PricingMU.ModalRupiah));
            }
            catch (Exception ex)
            {
            }

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
                td2.Text = "Gold Jewellery";
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
                td3.Text = "Gold Jewellery";
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
                td4.Text = "Gold Jewellery";
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
                td5.Text = "Gold Jewellery";
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
                td6.Text = "Gold Jewellery";
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
                td7.Text = "Gold Jewellery";
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
                td8.Text = "Gold Jewellery";
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
                td9.Text = "Gold Jewellery";
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
                td10.Text = "Gold Jewellery";
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
            td11.Text = "<b>" + HargaModalAll.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaRupiahAll.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            tb.Rows.Add(tr11);
        }

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e) {

        string href = "GJ_Rekap_Print.aspx?id=1";

        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}