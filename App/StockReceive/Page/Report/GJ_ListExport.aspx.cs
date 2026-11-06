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
            //Generate

            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=GJ_List_ExportExcel.xls");

            fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockReceiveGJ
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 //&& !(from s in db.SalesOrderGJ
                 //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                 //     select s.IDProduct).Contains(p.ID)
                 select p;

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];

        if(kw!="")
            rs = rs.Where(p => p.Nomor.Contains(kw) || p.ProtoNomor.Contains(kw) || p.SupplierNomor.Contains(kw));

        //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
        if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
        //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductItem == ProductItem);
        
        if (ProductCategory > 0) rs = rs.Where(p => p.StockReceiveGJ_CharProduct.ProductCategory == ProductCategory);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.SupplierNama.Equals(Supplier));

        if(orderby=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockReceiveGJ_CharProduct.CharProductItem.Nama);

        int index = 0;

        if (rs != null)
        {
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
				index++;
				
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
				TotalHargaR += r.StockReceiveGJ_PricingMU.ModalUSD;
				TotalHargaRp += (r.StockReceiveGJ_PricingMU.HargaRupiahRound.Value);
                

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
}