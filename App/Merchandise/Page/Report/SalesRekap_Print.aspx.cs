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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            FillHeader();
            fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = (from p in db.SalesOrderDJ
                  where p.StockProductDJ.Legacy.Value == null && !p.SalesOrder.Draft
                  && !(from z in db.SalesOrderDJ
                       where z.StockProductDJ.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(p.IDForm)
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Diamond Jewellery",
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
                      CategoryID = p.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.ID,
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = p.StockProductDJ.ProtoNomor,
                      SupplierNomor = p.StockProductDJ.SupplierNomor,
                  }).Concat(
                  from p in db.SalesOrderGJ
                  where p.SalesOrder.StatusPembayaran == true
                  where !p.SalesOrder.Draft
                  select new
                  {
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
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
                      CategoryID = -3,
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = p.StockProductGJ.ProtoNomor,
                      SupplierNomor = p.StockProductGJ.SupplierNomor,
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
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Plain Gold",
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
                      CategoryID = -2,
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = "",
                      SupplierNomor = ""
                  })
                  .Concat(
                  from p in db.SalesOrderLD
                  where p.SalesOrder.StatusPembayaran == true
                  where !p.SalesOrder.Draft
                  select new
                  {
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
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
                      CategoryID = -4,
                      Harga = p.HargaRupiah.Value,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
                      KodeKaret = "",
                      SupplierNomor = ""
                  })
                  ;

        int TipeProduct = 0;
        TipeProduct = Convert.ToInt32(Request.QueryString["tipeproduct"]);
        if (TipeProduct > 0)
        {
            if (TipeProduct == 1)
                rs = rs.Where(p => p.Tipe == "Diamond Jewellery");
            else if (TipeProduct == 2)
                rs = rs.Where(p => p.Tipe == "Gold Jewellery");
            else if (TipeProduct == 3)
                rs = rs.Where(p => p.Tipe == "Plain Gold");
            else if (TipeProduct == 4)
                rs = rs.Where(p => p.Tipe == "Loose Diamond");
        }

        int ProductCategory = 0;
        ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.CategoryID == ProductCategory);
        }
        else
        {
            if (ProductCategory <= -2 && ProductCategory >= -4)
                rs = rs.Where(p => p.CategoryID == ProductCategory);
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

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(Request.QueryString["kw"]) || p.Invoice.Contains(Request.QueryString["kw"]) || p.Nomor.Contains(Request.QueryString["kw"]));
        
        if(Request.QueryString["status"]=="1")
            rs = rs.Where(p=> p.StatusVoid == true);
        else if (Request.QueryString["status"] == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalHargaNet = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
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
            foreach (var r in rs)
            {


                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.Butir;
                Carat = r.Carat;

                if (!Response.IsClientConnected) break;

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
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

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
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Harga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Discount.ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountProgram.ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountPromo.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalDiscountPersen = 0m;

                TotalDiscountPersen = (r.DiscountNominal + r.DiscountProgramNominal + r.DiscountGift + r.DiscountRound) / (r.Harga > 0 ? r.Harga : 1) * 100;

                td = new TableCell();
                td.Text = TotalDiscountPersen.ToString("N2") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NetWeight.ToString("N2");
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
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))).ToString("N0");
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
                TotalHargaNet += r.TotalBayar;
                TotalDiscount += r.Discount;
                TotalDiscountProgram += r.DiscountProgram;
                TotalDiscountGift += r.DiscountGift;
                TotalDiscountRound += r.DiscountRound;
                TotalDiscountPromo += r.DiscountPromo;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.NetWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 13;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPromo).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaNet).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar5).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar6).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar7).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar8).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
    protected void FillHeader()
    {

        periode.InnerHtml = Request.QueryString["dari"];
        periode.InnerHtml += " s/d " + Request.QueryString["sampai"];

        int TipeProduct = 0;
        TipeProduct = Convert.ToInt32(Request.QueryString["tipeproduct"]);
        if (TipeProduct > 0)
        {
            if (TipeProduct == 1)
                tipeproduct.InnerText = "Diamond Jewellery";
            else if (TipeProduct == 2)
                tipeproduct.InnerText = "Gold Jewellery";
            else if (TipeProduct == 3)
                tipeproduct.InnerText = "Plain Gold";
            else if (TipeProduct == 4)
                tipeproduct.InnerText = "Loose Diamond";
        }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
}