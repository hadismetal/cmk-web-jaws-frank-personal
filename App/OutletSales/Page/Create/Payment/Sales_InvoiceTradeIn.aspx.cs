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
            var py = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == myID
                     orderby p.OperatorTgl descending
                     select p;

            decimal TotalBayar = py.Sum(p => p.Nominal.Value);

            string BrandText = "CMK";
            string Cabang = "";
            string AlamatCabang = "";

            if (d.TipeLokasi == 1)
            {
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
            }
            else
            {
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
            }

            alamat.Text = Str.HtmlBreak(AlamatCabang);
            tgl.Text = Str.Tgl(py.First().OperatorTgl);

            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            total.Text = Str.Number0(d.TotalBayar.Value);
            nominal.Text = Str.Number0(d.Paid.Value);
            sisa.Text = Str.Number0(d.Unpaid.Value);
            customer.Text = d.DataCustomer.Nama;
            customer2.Text = d.DataCustomer.Nama;
            adminsales.Text = d.SalesNama;

            var pym = from p in db.SalesReceiptDetail
                      where p.SalesReceipt.IDSalesOrder.Value == d.ID
                      select p;

            string DetailPayment = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"3\"><em>Detail of payment</em></td></tr>";
            foreach (var r in pym)
            {
                DetailPayment += "<tr><td width=\"120\">" + r.PaymentType.Nama + "</td><td width=\"20\">:</td><td>" + r.Nominal.Value.ToString("N0") + "</td></tr>";
            }
            DetailPayment += "</table>";
            detail_payment.Text = DetailPayment;

            if (d.DataSales.CompanyBrand.NamaKode == "F")
                frank_term.Visible = true;

            string Item = "";
            int index = 1;
            decimal DP = 0m;
            foreach (var r in d.SalesOrderDJ)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;
                if (index == 1)
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
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
                Item += "<div style=\"width:140px; float:left;\">" + r.HargaRupiah.Value.ToString("N0") + "</div>";
                //Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + TotalBayar.ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
                index++;
            }
            foreach (var r in d.SalesOrderGJ)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value + r.DiscountProgramNominal.Value + r.DiscountGift.Value + r.DiscountRound.Value;
                if (index == 1)
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
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
                //Item += "<div style=\"width:140px; float:left;\">" + r.DiscountGift.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
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
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductPG.Nomor + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductPG.CharProductItem.Nama) + " BERAT " + r.StockProductPG.GrossWeight + " KADAR " + Math.Floor(r.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductPG.CharFrameColor.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                //Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
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
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.StockProductLD_Stone1B.Nomor + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.StockProductLD_Stone1B.TotalCarat.ToString("N3")) + " Carat</div>";
                Item += "<div style=\"width:60px; float:left;\">1</div>";
                //Item += "<div style=\"width:140px; float:left;\">" + r.TotalBayar.Value.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
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
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
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
                //Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">&nbsp;</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
            }
            foreach (var r in d.SalesOrderSouvenir)
            {
                decimal Discount = 0m;
                if (index == 1)
                    DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                else
                {
                    DP = 0m;
                    TotalBayar = 0m;
                }
                Item += "<div style=\"padding-top:10px; width:1200px;\">";
                Item += "<div style=\"width:180px; float:left;\">" + r.Souvenir.Kode + "</div>";
                Item += "<div style=\"width:370px; float:left;\">" + Str.CapUpper(r.Souvenir.Nama) + "</div>";
                Item += "<div style=\"width:60px; float:left;\">" + Str.Number0(r.Qty) + "</div>";
                Item += "<div style=\"width:140px; float:left;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                //Item += "<div style=\"width:140px; float:left;\">" + Discount.ToString("N0") + "</div>";
                Item += "<div style=\"width:140px; float:left;\">&nbsp;</div>";
                Item += "<div style=\"width:180px; float:left;\">" + DP.ToString("N0") + "</div>";
                Item += "<div style=\"width:120px; float: left; text-align: right;\">" + Convert.ToDecimal(r.TotalRupiah.Value).ToString("N0") + "</div>";
                Item += "</div>";
                Item += "<div style=\"clear: both;\"></div>";
            }
            itemlist.Text = Item;
        }
    }
}