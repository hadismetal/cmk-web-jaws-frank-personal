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

            fillTable(sender, e);
            fillHeader();
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusLebur == false
                 && !(from s in db.SalesOrderGJ
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                      select s.IDProduct).Contains(p.ID)
                 && !(from s in db.SalesOrderSouvenir
                      where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                      select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualGJ.Substorage == Substorage);
        }

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        /*string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }*/

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];

        if(kw!="")
            rs = rs.Where(p=>p.Nomor==""+kw+"");
        
        if (Cabang > 0) rs = rs.Where(p => p.StockActualGJ.IDLokasi == Cabang && p.StockActualGJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualGJ.TipeLokasi == tipe);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductItem == ProductItem);
        
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductGJ_CharProduct.ProductCategory == ProductCategory);

        if(orderby=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockProductGJ_CostingProduct;
                var z = r.StockProductGJ_Costing;

                AllM = (x.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualGJ.Substorage == 1)
                    Jual = r.StockActualGJ.InHand;
                else if (r.StockActualGJ.Substorage == 3)
                    Exchange = r.StockActualGJ.InHand;
                else if (r.StockActualGJ.Substorage == 2)
                    Resell = r.StockActualGJ.InHand;

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
                td.Text = r.StockActualGJ.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualGJ.Substorage);
                tr.Cells.Add(td);

                /*td = new TableCell();
                td.Text = r.StockActualGJ.InHand.ToString("N0");
                tr.Cells.Add(td);*/

                string Lokasi = "";
                if (r.StockActualGJ.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualGJ.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualGJ.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualGJ.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualGJ.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualGJ.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalRupiah).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllM).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductGJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);
                
                /*td = new TableCell();
                td.Text = TotalButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.StockProductGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductGJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (DateTime.Today - r.Tgl.Value).TotalDays.ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalHarga += Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalRupiah);
                TotalModal += Convert.ToDecimal(r.StockProductGJ_PricingMU.ModalRupiah);
                TotalM += AllM;
                TotalR += r.StockProductGJ_PricingMU.ModalUSD;
                TotalNetWeight += r.StockProductGJ_CharProduct.NettoWeight;
                TotalGrossWeight = r.StockProductGJ_CharProduct.GrossWeight;

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
                td.Text = rs.Count().ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
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

                tb.Rows.Add(tr);
            }
        }


    }

    protected void fillHeader()
    {

        var rs = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusLebur == false
                 && !(from s in db.SalesOrderGJ
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                      select s.IDProduct).Contains(p.ID)
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualGJ select p;

        periode.InnerHtml = Request.QueryString["dari"];
        periode.InnerHtml += " s/d " + Request.QueryString["sampai"];

        if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rsB.SingleOrDefault(p => p.IDLokasi == Convert.ToInt32(Request.QueryString["cabang"]) && p.TipeLokasi == Convert.ToInt32(Request.QueryString["tipe"])); cabang.InnerText = v_cab.NamaLokasi; } else { cabang.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsL.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }


        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }

    
}