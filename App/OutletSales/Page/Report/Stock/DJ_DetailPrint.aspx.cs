using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualDJ select p;

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
        decimal netweight = 0m;
        decimal frameprice = 0m;
        decimal totalbutir = 0m;
        decimal totalcarat = 0m;
        decimal totalharga = 0m;

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.TipeLokasi == d.First().TipeLokasi && p.StockActualDJ.IDLokasi == d.First().IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        /*if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }*/

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];

        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);

        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);

        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        if (orderby == "1")
            rs.OrderBy(p => p.StockActualDJ.NamaLokasi);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int PrevID = 0;
            foreach (var r in rs)
            {
                decimal AllM = 0m;
                decimal Rate = 0m;

                Rate = r.StockProductDJ_PricingBiaya.TotalRate;

                var y = r.StockProductDJ_CostingProduct;
                var z = r.StockProductDJ_Costing;

                AllM = (y.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                tr.Cells.Add(td);

                netweight += r.StockProductDJ_CharProduct.NettoWeight;

                var stone = (from p in r.StockProductDJ_Stone1A
                             select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1A.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone1B
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1B.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone2
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone2.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone3
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone3.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone4
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone4.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone5
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone5.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir });
                if (stone.Count() > 0)
                {
                    int counter = 1;
                    foreach (var rstone in stone)
                    {
                        if (counter > 1)
                        {
                            tr = new TableRow();

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
                        }

                        td = new TableCell();
                        td.Text = rstone.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rstone.TotalButir.ToString("N0");
                        tr.Cells.Add(td);

                        totalbutir += rstone.TotalButir;

                        td = new TableCell();
                        td.Text = rstone.TotalCarat.ToString("N3");
                        tr.Cells.Add(td);

                        totalcarat += rstone.TotalCarat;

                        if (stone.Count() > 1)
                            tb.Rows.Add(tr);

                        counter++;
                    }
                }
                else
                {
                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);
                }

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
                tr.Cells.Add(td);

                totalharga += Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value);

                tb.Rows.Add(tr);
            }
            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);
            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + netweight.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + totalbutir.ToString("N0") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + totalcarat.ToString("N4") + "</b>";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + totalharga.ToString("N2") + "</b>";
            tr1.Cells.Add(td1);

            tb.Rows.Add(tr1);
        }
    }
}