using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_GJ : System.Web.UI.Page {
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
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            DLC.LocationList(cabang, tipe);
            DLC.DDLSupplier(supplier, 0);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

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

        var rs = from p in db.StockReceiveGJ
                 where !p.Draft
                 //p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 //&& !(from s in db.SalesOrderGJ
                 //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                 //     select s.IDProduct).Contains(p.ID)
                 select p;

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        if(kw.Text!="")rs = rs.Where(p=>p.Nomor.Contains(kw.Text) || p.ProtoNomor.Contains(kw.Text) || p.SupplierNomor.Contains(kw.Text));
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
        if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
        //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);    
        if (ProductItem > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);

        string Supplier = supplier.SelectedValue;
        if(Supplier!="")
            rs = rs.Where(p=>p.SupplierNama.Equals(Supplier));

        if(orderby.SelectedValue=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
			decimal TotalNetWeight = 0m;
			decimal TotalGrossWeight = 0m;
			decimal TotalFramePriceM = 0m;
			decimal TotalFramePriceR = 0m;
			decimal TotalButirAll = 0m;
			decimal TotalKaratAll = 0m;
			decimal TotalHargaParcelMAll = 0m;
			decimal TotalHargaParcelRAll = 0m;
			decimal TotalNOMAll = 0m;
			decimal TotalNOMLotAll = 0m;
			decimal TotalNORAll = 0m;
			decimal TotalROMAll = 0m;
			decimal TotalRORAll = 0m;
			decimal TotalFinishingM = 0m;
			decimal TotalFinishingR = 0m;
			decimal TotalStoneSettingM = 0m;
			decimal TotalStoneSettingR = 0m;
			decimal TotalOngkosLainM = 0m;
			decimal TotalOngkosLainR = 0m;
			decimal TotalUnitPrice = 0m;

			decimal TotalHargaM = 0m;
			decimal TotalHargaRp = 0m;
			decimal TotalHargaR = 0m;
            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockReceiveGJ_CostingProduct;
                var z = r.StockReceiveGJ_Costing;

                AllM = (x.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor!=null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = KodeKaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceiveGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceiveGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceiveGJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = DL.SubStorage(1);
                tr.Cells.Add(td);

                string Lokasi = "";
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDWarehouse, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockReceiveGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockReceiveGJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                        td.Text = r.StockReceiveGJ_CostingProduct.TotalProduct.ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_PricingProduct.TotalProduct.ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_Costing.MountingNO).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_Costing.MountingNO * r.StockReceiveGJ_Costing.Pembagi.Value).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_PricingBiaya.MountingNO.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_Costing.MountingRO.Value.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_PricingBiaya.MountingRO.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_Finishing.Sum(y => y.BiayaModal)).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_Finishing.Sum(y => y.Biaya)).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_Costing.OngkosLainCOGS.Value.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_Costing.OngkosLain.Value.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_Costing.Setting).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_PricingBiaya.Setting).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = AllM.ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_PricingMU.ModalUSD.ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveGJ_PricingMU.MU.ToString("N0") + "%";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        //td.Text = Convert.ToDecimal(r.StockReceiveGJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockReceiveGJ_PricingMU.HargaRupiahRound).ToString("N2");
                        td.Text = Convert.ToDecimal(r.StockReceiveGJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.SupplierNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.ProductOrderNomor;
                        tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalNetWeight += r.StockReceiveGJ_CharProduct.NettoWeight;
				TotalGrossWeight += r.StockReceiveGJ_CharProduct.GrossWeight;
				TotalFramePriceM += r.StockReceiveGJ_CostingProduct.TotalProduct;
				TotalFramePriceR += r.StockReceiveGJ_PricingProduct.TotalProduct;
				TotalNOMAll += r.StockReceiveGJ_Costing.MountingNO;
				TotalNOMLotAll += r.StockReceiveGJ_Costing.MountingNO * r.StockReceiveGJ_Costing.Pembagi.Value;
				TotalNORAll += r.StockReceiveGJ_PricingBiaya.MountingNO;
				TotalROMAll += r.StockReceiveGJ_Costing.MountingRO.Value;
				TotalRORAll += r.StockReceiveGJ_PricingBiaya.MountingRO;
				TotalOngkosLainM += r.StockReceiveGJ_Costing.OngkosLainCOGS.Value;
				TotalOngkosLainR += r.StockReceiveGJ_Costing.OngkosLain.Value;
				TotalUnitPrice += r.StockReceiveGJ_Costing.UnitPrice.Value;
				try
				{
					TotalFinishingM += r.StockReceiveGJ_Finishing.Sum(y => y.BiayaModal.Value);
				}
				catch (Exception ex) { }
				try
				{
					TotalFinishingR += r.StockReceiveGJ_Finishing.Sum(y => y.Biaya.Value);
				}
				catch (Exception ex) { }

				TotalStoneSettingM += r.StockReceiveGJ_Costing.Setting;
				TotalStoneSettingR += r.StockReceiveGJ_PricingBiaya.Setting;
				TotalHargaM += (AllM);
				TotalHargaRp += (r.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
				TotalHargaR += r.StockReceiveGJ_PricingMU.ModalUSD;
                index++;

            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 8;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = "<b>" + TotalFramePriceM.ToString("0.00") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalFramePriceR.ToString("0.00") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalNOMAll.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalNOMLotAll.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalNORAll.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalROMAll.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalRORAll.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalFinishingM.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalFinishingR.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalOngkosLainM.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalOngkosLainR.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalStoneSettingM.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalStoneSettingR.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalHargaM.ToString("N2") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalHargaR.ToString("N2") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<b>" + TotalHargaRp.ToString("N0") + "</b>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "";
				tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            supplier.SelectedValue = Request.QueryString["supplier"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "GJ_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "GJ_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;

        Response.Redirect(href);
    }
}