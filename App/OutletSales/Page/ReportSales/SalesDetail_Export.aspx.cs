using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class SalesDetail_Export : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=SalesRekap_Export.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        db.CommandTimeout = 300;
        var rs = (from p in db.SalesOrderDJ
                  where p.StockProductDJ.Legacy.Value == null && !p.SalesOrder.Draft
                  && !(from z in db.SalesOrderDJ
                       where z.StockProductDJ.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(p.IDForm)
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
                      IDProduct = p.StockProductDJ.ID,
                      Supplier = p.StockProductDJ.Supplier.Value,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoCustomer = p.SalesOrder.DataCustomer.NoCustomer,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Diamond Jewellery" + "<br />" + p.StockProductDJ.StockProductDJ_CharProduct.CharFrameColor.Nama,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalButir) : 0m)),
                      Carat = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalCarat) : 0m)),
                      Nomor = p.StockProductDJ.Nomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      Category = p.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama,
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = p.StockProductDJ.ProtoNomor,
                      SupplierNomor = p.StockProductDJ.SupplierNomor,
                      StatusConsignment = ""
                  }).Concat(
                  from p in db.SalesOrderGJ
                  where p.SalesOrder.StatusPembayaran == true
                  where !p.SalesOrder.Draft
                  select new
                  {
                      IDProduct = p.StockProductGJ.ID,
                      Supplier = p.StockProductGJ.Supplier.Value,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoCustomer = p.SalesOrder.DataCustomer.NoCustomer,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Gold Jewellery",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductGJ.Nomor,
                      ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      Category = "Gold Jewellery",
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = p.StockProductGJ.ProtoNomor,
                      SupplierNomor = p.StockProductGJ.SupplierNomor,
                      StatusConsignment = ""
                  }).Concat(
                  from p in db.SalesOrderPG
                      //where p.SalesOrder.StatusPembayaran == false
                  where !(from z in db.SalesOrderPG
                          where z.StockProductPG.Legacy.Value == 1
                          select z.SalesOrder.ID).Contains(p.IDForm)
                  && !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
                      IDProduct = p.StockProductPG.ID,
                      Supplier = p.StockProductPG.IDSupplier.Value,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoCustomer = p.SalesOrder.DataCustomer.NoCustomer,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Plain Gold" + "<br />" + p.StockProductPG.CharGoldLevel.Nama,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductPG.Nomor,
                      ProductItem = p.StockProductPG.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductPG.NettoWeight.Value,
                      IDSO = p.SalesOrder.ID,
                      Category = "Plain Gold",
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = "",
                      SupplierNomor = "",
                      StatusConsignment = ""
                  })
                  .Concat(
                  from p in db.SalesOrderLD
                  where p.SalesOrder.StatusPembayaran == true
                  where !p.SalesOrder.Draft
                  select new
                  {
                      IDProduct = p.StockProductLD_Stone1B.ID,
                      Supplier = 0,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoCustomer = p.SalesOrder.DataCustomer.NoCustomer,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Loose Diamond",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalButir),
                      Carat = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalCarat),
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      ProductItem = "-",
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = 0m,
                      IDSO = p.SalesOrder.ID,
                      Category = "Loose Diamond",
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = "",
                      SupplierNomor = "",
                      StatusConsignment = ""
                  })
                  ;

        int TipeProduct = 0;
        TipeProduct = Convert.ToInt32(Request.QueryString["tipeproduct"]);
        if (TipeProduct > 0)
        {
            if (TipeProduct == 1)
                rs = rs.Where(p => p.Category == "Diamond Jewellery");
            else if (TipeProduct == 2)
                rs = rs.Where(p => p.Category == "Gold Jewellery");
            else if (TipeProduct == 3)
                rs = rs.Where(p => p.Category == "Plain Gold");
            else if (TipeProduct == 4)
                rs = rs.Where(p => p.Category == "Loose Diamond");
        }

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO); ;

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(Request.QueryString["kw"]) || p.Invoice.Contains(Request.QueryString["kw"]) || p.Nomor.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["status"] == "1")
            rs = rs.Where(p => p.StatusVoid == true);
        else if (Request.QueryString["status"] == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalPrice_ex_Vat = 0m;
            decimal TotalPPN = 0m;
            decimal TotalHargaNet = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
            decimal TotalDiscountPersen = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalBayar1 = 0m;
            decimal TotalBayar2 = 0m;
            decimal TotalBayar3 = 0m;
            decimal TotalBayar4 = 0m;
            decimal TotalBayar5 = 0m;
            decimal TotalBayar6 = 0m;
            decimal TotalBayar7 = 0m;
            decimal TotalBayar8 = 0m;
            int prev = 0;
            int index = 1;
            string SO = "";

            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            foreach (var r in rs)
            {


                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.Butir;
                Carat = r.Carat;

                if (!Response.IsClientConnected) break;

                if (TipeProduct == 3)
                {
                    if (Request.QueryString["consignment"] != "2" && Request.QueryString["consignment"] != Db.SingleString("SELECT CONVERT(varchar, ISNULL(Consignment, 0), 0) FROM StockProductPG WHERE Nomor = '" + r.Nomor + "'", Db.Cnn_DB)) continue;
                }
                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoSO;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                string Lokasi = "";
                string KodeLokasi = "";
                //if (r.TipeLokasi == 1)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                //else if (r.TipeLokasi == 2)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                bool IsPPN = false;

                try
                {
                    if (r.TipeLokasi == 0)
                        Lokasi = rsWarehouse[r.IDLokasi.Value];
                    else if (r.TipeLokasi == 1)
                    {
                        Lokasi = rsOutlet[r.IDLokasi.Value];
                        KodeLokasi = Db.SingleString("SELECT TOP 1 ISNULL(Kode,'-') [Kode] FROM LocOutlet WHERE ID = " + r.IDLokasi.Value, Db.Cnn_DB);
                        IsPPN = Db.SingleBool("SELECT TOP 1 ISNULL(IsUsePPN,0) [IsUsePPN] FROM LocOutlet WHERE ID = " + r.IDLokasi.Value, Db.Cnn_DB);
                    }
                    else if (r.TipeLokasi == 2)
                    {
                        Lokasi = rsExhibition[r.IDLokasi.Value];
                        KodeLokasi = Db.SingleString("SELECT TOP 1 ISNULL(Kode,'-') [Kode] FROM LocExhibition WHERE ID = " + r.IDLokasi.Value, Db.Cnn_DB);
                        IsPPN = Db.SingleBool("SELECT TOP 1 ISNULL(IsUsePPN,0) [IsUsePPN] FROM LocExhibition WHERE ID = " + r.IDLokasi.Value, Db.Cnn_DB);
                    }
                }
                catch (Exception ex) { throw ex; }

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KodeKaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Category;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoCustomer;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.Category == "Plain Gold")
                {
                    td.Text = Db.SingleString("SELECT TOP 1 CASE WHEN Consignment = 1 THEN 'Consignment' ELSE 'Non Consignment' END [Status] FROM StockProductPG WHERE ID = " + r.IDProduct, Db.Cnn_DB);
                }
                else
                {
                    td.Text = r.StatusConsignment.ToString();
                }
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Harga.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Discount.ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountProgram.ToString("N1").Replace(".", "").Replace(",", ".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountPromo.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                decimal Price_ex_Vat = 0m;
                decimal PPN = 0m;

                if (IsPPN)
                {
                    if (r.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
                    {
                        Price_ex_Vat = r.TotalBayar / 1.02m;
                        PPN = r.TotalBayar - Price_ex_Vat;
                    }
                    else if (r.Tgl < Convert.ToDateTime("2023-05-01"))
                    {
                        Price_ex_Vat = r.TotalBayar / 1.022m;
                        PPN = r.TotalBayar - Price_ex_Vat;
                    }
                    else
                    {
                        if (r.Tipe == "Loose Diamond")
                        {
                            Price_ex_Vat = r.TotalBayar / 1.011m;
                            PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if (r.Supplier == null || r.Supplier == 0)
                        {
                            Price_ex_Vat = r.TotalBayar / 1.0165m;
                            PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if (Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("In"))
                        {
                            Price_ex_Vat = r.TotalBayar / 1.0165m;
                            PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if (Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("Out"))
                        {
                            Price_ex_Vat = r.TotalBayar / 1.011m;
                            PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else
                        {
                            Price_ex_Vat = r.TotalBayar / 1.0165m;
                            PPN = r.TotalBayar - Price_ex_Vat;
                        }
                    }
                }
                else
                {
                    Price_ex_Vat = r.TotalBayar;
                }

                //Price_ex_Vat = r.TotalBayar / 1.022m;
                //PPN = r.TotalBayar - Price_ex_Vat;

                td = new TableCell();
                td.Text = Price_ex_Vat.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PPN.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                decimal JumlahDiscountPersen = 0m;

                JumlahDiscountPersen = (r.DiscountNominal + r.DiscountProgramNominal + r.DiscountGift + r.DiscountRound) / (r.Harga > 0 ? r.Harga : 1) * 100;

                td = new TableCell();
                td.Text = JumlahDiscountPersen.ToString("N2").Replace(".", "").Replace(",", ".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                //td.Text = "<b>" + Convert.ToDecimal(Butir).ToString("N0").Replace(".", "").Replace(",", ".").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("0.###");
                //td.Text = "<b>" + Convert.ToDecimal(Carat).ToString("N3").Replace(".", "").Replace(",", ".").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NetWeight.ToString("0.##");
                //td.Text = "<b>" + Convert.ToDecimal(r.NetWeight).ToString("N2").Replace(".", "").Replace(",", ".").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                var so = db.SalesOrder.SingleOrDefault(p => p.ID.Equals(r.IDSO));
                if (SO != r.NoSO)
                {
                    TotalBayar1 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal)));
                    TotalBayar2 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal)));
                    TotalBayar3 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal)));
                    TotalBayar4 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal)));
                    TotalBayar5 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal)));
                    TotalBayar6 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal)));
                    TotalBayar7 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal)));
                    TotalBayar8 += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal)));

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    string Bank = "-";
                    int counter = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDBankIssuer.HasValue)
                            {
                                if (counter == 1)
                                    Bank = t.BankIssuer.Nama;
                                else
                                {
                                    Bank += ", " + t.BankIssuer.Nama;
                                    counter++;
                                }
                            }
                        }
                    }

                    string Program = "-";
                    int counter2 = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDProgram.HasValue)
                            {
                                if (counter2 == 1)
                                    Program = t.ProgramCicilan.Nama;
                                else
                                {
                                    Program += ", " + t.ProgramCicilan.Nama;
                                    counter2++;
                                }
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Program;
                    tr.Cells.Add(td);

                }
                else
                {
                    string Bank = "-";
                    int counter = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDBankIssuer.HasValue)
                            {
                                if (counter == 1)
                                    Bank = t.BankIssuer.Nama;
                                else
                                {
                                    Bank += ", " + t.BankIssuer.Nama;
                                    counter++;
                                }
                            }
                        }
                    }

                    string Program = "-";
                    int counter2 = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDProgram.HasValue)
                            {
                                if (counter2 == 1)
                                    Program = t.ProgramCicilan.Nama;
                                else
                                {
                                    Program += ", " + t.ProgramCicilan.Nama;
                                    counter2++;
                                }
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 8;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Program;
                    tr.Cells.Add(td);
                }
                SO = r.NoSO;



                tb.Rows.Add(tr);

                TotalHarga += r.Harga;
                TotalPrice_ex_Vat += Price_ex_Vat;
                TotalPPN += PPN;
                TotalHargaNet += r.TotalBayar;
                TotalDiscount += r.Discount;
                TotalDiscountProgram += r.DiscountProgram;
                TotalDiscountGift += r.DiscountGift;
                TotalDiscountRound += r.DiscountRound;
                TotalDiscountPromo += r.DiscountPromo;
                TotalDiscountPersen += JumlahDiscountPersen;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.NetWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 14;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N1").Replace(".", "").Replace(",", ".") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPromo).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaNet).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPrice_ex_Vat).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPPN).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);



            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPersen / rs.Count()).ToString("N2").Replace(".", "").Replace(",", ".") + " %</b>";
            tr2.Cells.Add(td2);


            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "<b>";
            //td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("0.###") + "<b>";
            //td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalNetWeight.ToString("0.##") + "<b>";
            //td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar5).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar6).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar7).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar8).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}