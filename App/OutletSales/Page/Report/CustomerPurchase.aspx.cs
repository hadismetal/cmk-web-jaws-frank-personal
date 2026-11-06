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
            kw.Focus();
            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        if (kw.Text.Length > 2)
        {
            /*var rs = (from p in db.SalesOrderDJ
                      where p.StockProductDJ.Legacy.Value == null
                      && p.SalesOrder.StatusVoid == false
                      select new
                      {
                          PLU = p.StockProductDJ.Nomor,
                          Tgl = p.SalesOrder.Tgl.Value,
                          Invoice = p.Invoice,
                          ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                          CustomerNama = p.SalesOrder.CustomerNama,
                          Sales = p.SalesOrder.SalesNama,
                          Qty = 1,
                          TotalHarga = p.HargaRupiah.Value,
                          Discount = p.DiscountNominal,
                          DiscountProgram = p.DiscountProgramNominal,
                          DiscountGift = p.DiscountGift,
                          DiscountRound = p.DiscountRound,
                          TotalButir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                          TotalCarat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                          NettoWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                          Harga = p.TotalBayar.Value
                      }).Union(
                         from p in db.SalesOrderGJ
                         where p.SalesOrder.StatusVoid == false
                         select new
                         {
                             PLU = p.StockProductGJ.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal,
                             DiscountProgram = p.DiscountProgramNominal,
                             DiscountGift = p.DiscountGift,
                             DiscountRound = p.DiscountRound,
                             TotalButir = 0m,
                             TotalCarat = 0m,
                             NettoWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                             Harga = p.TotalBayar.Value
                         }).Union(
                         from p in db.SalesOrderPG
                         where p.StockProductPG.Legacy.Value == null
                            && p.SalesOrder.StatusVoid == false
                         select new
                         {
                             PLU = p.StockProductPG.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = p.StockProductPG.CharProductItem.Nama,
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal,
                             DiscountProgram = p.DiscountProgramNominal,
                             DiscountGift = p.DiscountGift,
                             DiscountRound = p.DiscountRound,
                             TotalButir = 0m,
                             TotalCarat = 0m,
                             NettoWeight = p.StockProductPG.NettoWeight.Value,
                             Harga = p.TotalBayar.Value
                         }).Union(
                         from p in db.SalesOrderLD
                         where p.SalesOrder.StatusVoid == false
                         select new
                         {
                             PLU = p.StockProductLD_Stone1B.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = "N/A",
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal,
                             DiscountProgram = p.DiscountProgramNominal,
                             DiscountGift = p.DiscountGift,
                             DiscountRound = p.DiscountRound,
                             TotalButir = 1m,
                             TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
                             NettoWeight = 0m,
                             Harga = p.TotalBayar.Value
                         })
                     ;*/

            var rs = (from p in db.SalesOrderDJ
                      where p.StockProductDJ.Legacy.Value == null
                      && p.SalesOrder.StatusVoid == false
                      select new
                      {
                          Status = "Penjualan",
                          PLU = p.StockProductDJ.Nomor,
                          Tgl = p.SalesOrder.Tgl.Value,
                          Invoice = p.Invoice,
                          ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                          CustomerNama = p.SalesOrder.CustomerNama,
                          Sales = p.SalesOrder.SalesNama,
                          Qty = 1,
                          TotalHarga = p.HargaRupiah.Value,
                          Discount = p.DiscountNominal.Value,
                          DiscountProgram = p.DiscountProgramNominal.Value,
                          DiscountGift = p.DiscountGift.Value,
                          DiscountRound = p.DiscountRound.Value,
                          TotalButir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                          TotalCarat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                          NettoWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                          Harga = p.TotalBayar.Value,
                          ImgPicture = p.StockProductDJ.ImgPicture,
                          NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                      }).Concat(
                         from p in db.SalesOrderGJ
                         where p.SalesOrder.StatusVoid == false
                         select new
                         {
                             Status = "Penjualan",
                             PLU = p.StockProductGJ.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal.Value,
                             DiscountProgram = p.DiscountProgramNominal.Value,
                             DiscountGift = p.DiscountGift.Value,
                             DiscountRound = p.DiscountRound.Value,
                             TotalButir = 0m,
                             TotalCarat = 0m,
                             NettoWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                             Harga = p.TotalBayar.Value,
                             ImgPicture = p.StockProductGJ.ImgPicture,
                             NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                         }).Concat(
                         from p in db.SalesOrderPG
                         where p.StockProductPG.Legacy.Value == null
                            && p.SalesOrder.StatusVoid == false
                         select new
                         {
                             Status = "Penjualan",
                             PLU = p.StockProductPG.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = p.StockProductPG.CharProductItem.Nama,
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal.Value,
                             DiscountProgram = p.DiscountProgramNominal.Value,
                             DiscountGift = p.DiscountGift.Value,
                             DiscountRound = p.DiscountRound.Value,
                             TotalButir = 0m,
                             TotalCarat = 0m,
                             NettoWeight = p.StockProductPG.NettoWeight.Value,
                             Harga = p.TotalBayar.Value,
                             ImgPicture = p.StockProductPG.ImgPicture,
                             NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                         }).Concat(
                         from p in db.SalesOrderLD
                         where p.SalesOrder.StatusVoid == false
                         select new
                         {
                             Status = "Penjualan",
                             PLU = p.StockProductLD_Stone1B.Nomor,
                             Tgl = p.SalesOrder.Tgl.Value,
                             Invoice = p.Invoice,
                             ProductItem = "N/A",
                             CustomerNama = p.SalesOrder.CustomerNama,
                             Sales = p.SalesOrder.SalesNama,
                             Qty = 1,
                             TotalHarga = p.HargaRupiah.Value,
                             Discount = p.DiscountNominal.Value,
                             DiscountProgram = p.DiscountProgramNominal.Value,
                             DiscountGift = p.DiscountGift.Value,
                             DiscountRound = p.DiscountRound.Value,
                             TotalButir = 1m,
                             TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
                             NettoWeight = 0m,
                             Harga = p.TotalBayar.Value,
                             ImgPicture = p.StockProductLD_Stone1B.ImgPicture,
                             NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                         }).Concat(from p in db.ResellDJ
                                where p.Resell.StatusVoid == null && p.Resell.Draft == false
                                select new
                      {
                          Status = "Resell",
                          PLU = p.StockProductDJ.Nomor,
                          Tgl = p.Resell.Tgl.Value,
                          Invoice = p.Resell.Nomor,
                          ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                          CustomerNama = p.Resell.CustomerNama,
                          Sales = p.SalesOrder.SalesNama,
                          Qty = 1,
                          TotalHarga = 0m,
                          Discount = 0m,
                          DiscountProgram = 0m,
                          DiscountGift = 0m,
                          DiscountRound = 0m,
                          TotalButir = 0m,
                          TotalCarat = 0m,
                          NettoWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                          Harga = -1* p.HargaRupiah.Value,
                          ImgPicture = p.StockProductDJ.ImgPicture,
                          NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                      }).Concat(from p in db.ResellPG
                                where p.Resell.StatusVoid == null && p.Resell.Draft == false
                                select new
                                {
                                    Status = "Resell",
                                    PLU = p.StockProductPG.Nomor,
                                    Tgl = p.Resell.Tgl.Value,
                                    Invoice = p.Resell.Nomor,
                                    ProductItem = p.StockProductPG.CharProductItem.Nama,
                                    CustomerNama = p.Resell.CustomerNama,
                                    Sales = p.SalesOrder.SalesNama,
                                    Qty = 1,
                                    TotalHarga = 0m,
                                    Discount = 0m,
                                    DiscountProgram = 0m,
                                    DiscountGift = 0m,
                                    DiscountRound = 0m,
                                    TotalButir = 0m,
                                    TotalCarat = 0m,
                                    NettoWeight = p.StockProductPG.NettoWeight.Value,
                                    Harga = -1*p.HargaRupiah.Value,
                                    ImgPicture = p.StockProductPG.ImgPicture,
                                    NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                                }).Concat(from p in db.ResellGJ
                                          where p.Resell.StatusVoid == null && p.Resell.Draft == false
                                          select new
                                          {
                                              Status = "Resell",
                                              PLU = p.StockProductGJ.Nomor,
                                              Tgl = p.Resell.Tgl.Value,
                                              Invoice = p.Resell.Nomor,
                                              ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                                              CustomerNama = p.Resell.CustomerNama,
                                              Sales = p.SalesOrder.SalesNama,
                                              Qty = 1,
                                              TotalHarga = 0m,
                                              Discount = 0m,
                                              DiscountProgram = 0m,
                                              DiscountGift = 0m,
                                              DiscountRound = 0m,
                                              TotalButir = 0m,
                                              TotalCarat = 0m,
                                              NettoWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                                              Harga = -1 * p.HargaRupiah.Value,
                                              ImgPicture = p.StockProductGJ.ImgPicture,
                                              NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                                          }).Concat(from p in db.ResellLD
                                                    where p.Resell.StatusVoid == null && p.Resell.Draft == false
                                                    select new
                                                    {
                                                        Status = "Resell",
                                                        PLU = p.StockProductLD_Stone1B.Nomor,
                                                        Tgl = p.Resell.Tgl.Value,
                                                        Invoice = p.Resell.Nomor,
                                                        ProductItem = "N/A",
                                                        CustomerNama = p.Resell.CustomerNama,
                                                        Sales = p.SalesOrder.SalesNama,
                                                        Qty = 1,
                                                        TotalHarga = 0m,
                                                        Discount = 0m,
                                                        DiscountProgram = 0m,
                                                        DiscountGift = 0m,
                                                        DiscountRound = 0m,
                                                        TotalButir = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalButir),
                                                        TotalCarat = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalCarat),
                                                        NettoWeight = 0m,
                                                        Harga = -1 * p.HargaRupiah.Value,
                                                        ImgPicture = p.StockProductLD_Stone1B.ImgPicture,
                                                        NoCustomer = p.SalesOrder.DataCustomer.NoCustomer
                                                    });


            rs = rs.OrderByDescending(p => p.Tgl);

            string keyword = kw.Text;
            if (keyword != "") rs = rs.Where(p => p.PLU == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.NoCustomer.Contains(kw.Text));

            if (dari.Text != "" && sampai.Text != "")
            {
                rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }
            if (rs.Count() > 0)
            {
                decimal TotalHarga = 0m;
                decimal TotalDiscount = 0m;
                decimal TotalDiscountProgram = 0m;
                decimal TotalDiscountGift = 0m;
                decimal TotalDiscountRound = 0m;
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                decimal TotalNetWeight = 0m;
                decimal TotalNet = 0m;
                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    decimal Butir = 0m;
                    decimal Carat = 0m;
                    int Qty = 1;

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(r.Tgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Invoice;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Status;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.PLU;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProductItem;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CustomerNama + " / " + r.NoCustomer ;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sales;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalHarga.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Discount.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.DiscountProgram.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.DiscountGift.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.DiscountRound.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalButir.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Harga.ToString("N0");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Harga;
                    TotalDiscount += r.Discount;
                    TotalDiscountProgram += r.DiscountProgram;
                    TotalDiscountGift += r.DiscountGift;
                    TotalDiscountRound += r.DiscountRound;
                    TotalButir += r.TotalButir;
                    TotalCarat += r.TotalCarat;
                    TotalNetWeight += r.NettoWeight;
                    TotalNet += r.Harga;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 17;
                tr2.Cells.Add(td2);

                /*td2 = new TableCell();
                td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
                tr2.Cells.Add(td2);*/

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalNet).ToString("N2") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
}