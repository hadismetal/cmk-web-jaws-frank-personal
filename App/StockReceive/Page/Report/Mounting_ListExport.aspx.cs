using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Mounting : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Mounting_List_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductMounting
                 where !p.Draft.Value
                 //&& !(from s in db.SalesOrderMounting
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
        string sk = Request.QueryString["sk"];
        int Brand = Convert.ToInt32(Request.QueryString["brand"]);
        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "")
            rs = rs.Where(p => p.SupplierNama.Equals(Supplier));

        if (kw!="")
        {
            rs = from p in rs
                 where p.Nomor.Contains(kw) || p.ProtoNomor.Contains(kw) || p.SupplierNomor.Contains(kw)
                 select p;
        }

        //if (Cabang > 0)  rs = rs.Where(p => p.StockActualMounting.TipeLokasi == tipe && p.StockActualMounting.IDLokasi == Cabang);
        if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
        //else rs = rs.Where(p => p.StockActualMounting.TipeLokasi == tipe);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductItem == ProductItem);
        
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductCategory == ProductCategory);
        if (sk!="0") rs = rs.Where(p => p.HumanStockNama == "" + sk + "");
        if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        if (orderby == "1")
            rs.OrderBy(p => p.StockActualMounting.NamaLokasi);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int Qty = 0;
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
            decimal TotalOngkosLainM = 0m;
            decimal TotalOngkosLainR = 0m;
            decimal TotalStoneSettingM = 0m;
            decimal TotalStoneSettingR = 0m;

            decimal TotalHargaM = 0m;
            decimal TotalHargaRp = 0m;
            decimal TotalHargaR = 0m;
            decimal TotalHargaAll = 0m;

            int PrevID = 0;
            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockProductMounting_CostingProduct;
                var z = r.StockProductMounting_Costing;

                AllM = (x.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                /*td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = Str.Tgl(r.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HumanStockNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductOrderNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.NettoWeight.ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.GrossWeight.ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CostingProduct.TotalProduct.ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingProduct.TotalProduct.ToString("N2").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                decimal TotalButir = 0m;
                try
                {

                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {

                }
                catch (Exception ex) { }

                decimal TotalHargaParcelM = 0m;
                decimal TotalHargaParcelR = 0m;
                try
                {

                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Costing.MountingNO * r.StockProductMounting_Costing.Pembagi).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Costing.MountingNO).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingBiaya.MountingNO.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_Costing.MountingRO.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingBiaya.MountingRO.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Finishing.Sum(y => y.BiayaModal)).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Finishing.Sum(y => y.Biaya)).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Costing.OngkosLainCOGS).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_Costing.OngkosLain).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllM.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingMU.ModalUSD.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingMU.MU.ToString("N0").Replace(".","").Replace(",",".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductMounting_PricingMU.ModalRupiah).ToString("N2").Replace(".","").Replace(",",".") + " / " + Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaRupiahRound).ToString("N2").Replace(".","").Replace(",",".");
                td.Text = Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaRupiahRound.Value).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualMounting.Substorage == 1)
                    Jual = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 3)
                    Exchange = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 2)
                    Resell = r.StockActualMounting.InHand;


                TotalNetWeight += r.StockProductMounting_CharProduct.NettoWeight;
                TotalGrossWeight += r.StockProductMounting_CharProduct.GrossWeight;
                TotalFramePriceM += r.StockProductMounting_CostingProduct.TotalProduct;
                TotalFramePriceR += r.StockProductMounting_PricingProduct.TotalProduct;
                TotalButirAll += TotalButir;
                TotalKaratAll += TotalCarat;
                TotalHargaParcelMAll += TotalHargaParcelM;
                TotalHargaParcelRAll += TotalHargaParcelR;
                TotalNOMAll += (r.StockProductMounting_Costing.MountingNO.Value * r.StockProductMounting_Costing.Pembagi.Value);
                TotalNOMLotAll += r.StockProductMounting_Costing.MountingNO.Value;
                TotalNORAll += r.StockProductMounting_PricingBiaya.MountingNO;
                TotalROMAll += r.StockProductMounting_Costing.MountingRO;
                TotalRORAll += r.StockProductMounting_PricingBiaya.MountingRO;
                try
                {
                    TotalFinishingM += r.StockProductMounting_Finishing.Sum(y => y.BiayaModal.Value);
                }
                catch (Exception ex) { }
                try
                {
                    TotalFinishingR += r.StockProductMounting_Finishing.Sum(y => y.Biaya.Value);
                }
                catch (Exception ex) { }

                TotalStoneSettingM += r.StockProductMounting_Costing.Setting;
                TotalStoneSettingR += r.StockProductMounting_PricingBiaya.Setting;
                TotalHargaM += (AllM);
                TotalHargaRp += (r.StockProductMounting_PricingMU.HargaRupiahRound.Value);
                TotalHargaR += r.StockProductMounting_PricingMU.ModalUSD;
                TotalHargaAll += r.StockProductMounting_PricingMU.ModalRupiah;
                TotalOngkosLainM += r.StockProductMounting_Costing.OngkosLainCOGS.Value;
                TotalOngkosLainR += r.StockProductMounting_Costing.OngkosLain.Value;
                Qty++;

                tb.Rows.Add(tr);
            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
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

                td = new TableCell();
                td.Text = "";
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

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Qty.ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFramePriceM.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFramePriceR.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNOMAll.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNOMLotAll.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNORAll.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalROMAll.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalRORAll.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFinishingM.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFinishingR.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkosLainM.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkosLainR.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaM.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaR.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaRp.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

        }
    }
}