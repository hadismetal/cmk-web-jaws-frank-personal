using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected int myReceiptID { get { return App.QueryInt(this, "sid"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Fill();

        }
    }
    protected void Fill()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("Sales.aspx");
        else
        {
            decimal TotalBayar = 0m;
            var py = from p in db.SalesReceipt
                     where p.IDSalesOrder == myID && ((myReceiptID > 0) ? p.ID == myReceiptID : 1==1)
                     select p;

            TotalBayar = py.Sum(p => p.TotalBayar.Value);
            
            if (TotalBayar < 0)
                titledp.InnerText = "Cancel Pembayaran";
            else
                titledp.InnerText = "Titipan Customer";

            string Cabang = "";
            string AlamatCabang = "";
            if (d.TipeLokasi == 1)
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocOutlet WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }
            else
            {
                Cabang = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
                AlamatCabang = Db.SingleString("SELECT AddrAlamat FROM LocExhibition WHERE ID = " + d.IDLokasi, Db.Cnn_DB);
            }

            //cabang.Text = Cabang;
            alamat.Text = Str.HtmlBreak(AlamatCabang);
            //tgl.Text = Str.Tgl(d.Tgl);
            if (myReceiptID > 0)
                tgl.Text = Str.Tgl(py.First().Tgl);
            else
                tgl.Text = Str.Tgl(d.OperatorTgl);
            //terbilang.Text = Str.Terbilang((d.Unpaid.Value + py.Sum(p => p.TotalBayar.Value)));
            //total.Text = Str.Number0(d.Unpaid + py.Sum(p => p.TotalBayar.Value));
            //terbilang.Text = Str.Terbilang((d.Unpaid.Value + py.Sum(p => p.TotalBayar.Value)));
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            //total.Text = Str.Number0(d.Unpaid + py.Sum(p => p.TotalBayar.Value));
            total.Text = Str.Number0(d.TotalBayar.Value);
            //nominal.Text = Str.Number0(py.Sum(p => p.TotalBayar.Value));
            nominal.Text = Str.Number0(d.Paid.Value);
            sisa.Text = Str.Number0(d.Unpaid.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            adminsales.Text = d.SalesNama;

            if (d.DataSales.CompanyBrand.NamaKode == "F")
                frank_term.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "M")
                mondial.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "S")
                missmondial.Visible = true;

            string Item = "";
            int index = 1;
            decimal DP = 0m;
            foreach (var r in d.SalesOrderDJ)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;
                if (index == 1)
                    DP = d.Paid.Value - TotalBayar;
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductDJ.Nomor + "</div>";
                //Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + " BERAT " + r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight + " KADAR " + Math.Floor(r.StockProductDJ.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharFrameColor.Nama) + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + TotalBayar.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            }
            foreach (var r in d.SalesOrderGJ)
            {
                decimal Discount = 0m;
                Discount = (r.DiscountNominal.HasValue ? r.DiscountNominal.Value : 0) + (r.DiscountProgramNominal.HasValue ? r.DiscountProgramNominal.Value : 0) + (r.DiscountGift.HasValue ? r.DiscountGift.Value : 0) + (r.DiscountRound.HasValue ? r.DiscountRound.Value : 0);
                if (index == 1)
                    DP = (d.Paid.HasValue ? d.Paid.Value : 0) -  TotalBayar;
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductGJ.Nomor + "</div>";
                //Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama) + " BERAT " + r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight + " KADAR " + Math.Floor(r.StockProductGJ.StockProductGJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharFrameColor.Nama) + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + TotalBayar.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            }
            foreach (var r in d.SalesOrderPG)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;
                if (index == 1)
                    DP = d.Paid.Value - TotalBayar;
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductPG.Nomor + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductPG.CharProductItem.Nama) + " BERAT " + r.StockProductPG.GrossWeight + " KADAR " + Math.Floor(r.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductPG.CharFrameColor.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + TotalBayar.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            }
            foreach (var r in d.SalesOrderLD)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;
                if (index == 1)
                    DP = d.Paid.Value - TotalBayar;
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductLD_Stone1B.Nomor + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductLD_Stone1B.TotalCarat.ToString("N3"))+ " Carat</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + TotalBayar.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            }
            foreach (var r in d.SalesOrderPackaging)
            {
                decimal Discount = 0m;
                if (index == 1)
                    DP = d.Paid.Value - TotalBayar;
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.Packaging.Kode + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.Packaging.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                //Item += "<div style=\"width:180px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
            }
            foreach (var r in d.SalesOrderSouvenir)
            {
                if (r.TipeProduct == 0 || r.TipeProduct == null)
                {
                    decimal Discount = 0m;
                    if (index == 1)
                        DP = d.Paid.Value - TotalBayar;
                    else
                    {
                        DP = 0m;
                        TotalBayar = 0m;
                    }
                    Item += "<div style=\"padding-top:10px; width:1200px;\">";
                    Item += "<div style=\"width:180px; float:left;\">" + r.Souvenir.Kode + "</div>";
                    Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.Souvenir.Nama) + "</div>";
                    Item += "<div style=\"width:60px; float:left;\">1</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                    Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                    //Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "</div>";
                    Item += "<div style=\"clear: both;\"></div>";
                }

                else if (r.TipeProduct == 1)
                {
                    /*var rs = from p in db.SalesOrderSouvenir
                             join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                             where p.IDForm == myID && p.TipeProduct == 1
                             select new { p, q };*/
                    var bs = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                        DP = d.Paid.Value - TotalBayar;
                    else
                    {
                        DP = 0m;
                        TotalBayar = 0m;
                    }
                    Item += "<div style=\"padding-top:10px; width:1200px;\">";
                    Item += "<div style=\"width:180px; float:left;\">" + bs.Nomor + "</div>";
                    Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(bs.StockProductDJ_CharProduct.CharProductItem.Nama) + "</div>";
                    Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                    Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                    //Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "</div>";
                    Item += "<div style=\"clear: both;\"></div>";
                }

                else if (r.TipeProduct == 2)
                {
                    var bs = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    //decimal DP = 0m;
                    decimal Discount = 0m;
                    if (index == 1)
                        DP = d.Paid.Value - TotalBayar;
                    else
                    {
                        DP = 0m;
                        TotalBayar = 0m;
                    }
                    Item += "<div style=\"padding-top:10px; width:1200px;\">";
                    Item += "<div style=\"width:180px; float:left;\">" + bs.Nomor + "</div>";
                    Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(bs.StockProductGJ_CharProduct.CharProductItem.Nama) + "</div>";
                    Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                    Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                    //Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "</div>";
                    Item += "<div style=\"clear: both;\"></div>";

                }
                else if (r.TipeProduct == 3)
                {
                    var bs = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                        DP = d.Paid.Value - TotalBayar;
                    else
                    {
                        DP = 0m;
                        TotalBayar = 0m;
                    }
                    Item += "<div style=\"padding-top:10px; width:1200px;\">";
                    Item += "<div style=\"width:180px; float:left;\">" + bs.Nomor + "</div>";
                    Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(bs.CharProductItem.Nama) + "</div>";
                    Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                    Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                    //Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "</div>";
                    Item += "<div style=\"clear: both;\"></div>";

                }

                else if (r.TipeProduct == 4)
                {
                    var bs = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    decimal Discount = 0m;
                    if (index == 1)
                        DP = d.Paid.Value - TotalBayar;
                    else
                    {
                        DP = 0m;
                        TotalBayar = 0m;
                    }
                    Item += "<div style=\"padding-top:10px; width:1200px;\">";
                    Item += "<div style=\"width:180px; float:left;\">" + bs.Nomor + "</div>";
                    Item += "<div style=\"width:370px; float:left;\">" + "-" + "</div>";
                    Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                    Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                    //Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                    Item += "</div>";
                    Item += "<div style=\"clear: both;\"></div>";
                }
            }
            itemlist.Text = Item;
        }
            
    }
}