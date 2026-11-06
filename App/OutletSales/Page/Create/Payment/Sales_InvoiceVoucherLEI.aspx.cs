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

    static string UppercaseWords(string text)
    {
        
        
            return char.ToUpper(text[0]) +
                ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty);
       
    }

    protected void Fill()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.StatusPembayaran == true && p.StatusVoid == false);
        if (d == null)
        {
            Response.Redirect("/OutletSales/Page/Summary.aspx");
        }
        else
        {
            var py = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == myID
                     orderby p.OperatorTgl descending
                     select p;

            string BrandText = "CMK";
            string Cabang = "";
            string AlamatCabang = "";
            string KodeLokasi = "";
			bool IsUsePPN = false;

            if (d.TipeLokasi == 1)
            {
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
				
				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
            }
            else
            {
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);

                // corporate exhibition
                if (d.IDLokasi == 18 && x.Kode == "XPR")
                {
                    BrandText = "CMK";
                }
                else
                {
                    BrandText = x.CompanyBrand.Nama;
                }
                Cabang = x.Nama;
				cabang.Text = Cabang;
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
				
				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocExhibition WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocExhibition WHERE ID = " + x.ID, Db.Cnn_DB);
            }

            alamat.Text = Str.HtmlBreak(AlamatCabang);
            tgl.Text = Str.Tgl(d.Tgl.Value);

            noinvoice.Text = d.Nomor;
            customer.Text = Str.CapUpper(d.DataCustomer.Nama);
            nocustomer.Text = Str.CapUpper(d.DataCustomer.NoCustomer);
            customer2.Text = Str.CapUpper(d.DataCustomer.Nama);
			customerother.Text = Str.CapUpper(String.IsNullOrEmpty(d.CustomerReferenceNama) ? d.DataCustomer.Nama : d.CustomerReferenceNama);
            adminsales.Text = Str.CapUpper(d.SalesNama);

            decimal DPP = 0m;
            decimal PPN = 0m;
            decimal Nominal = 0m;
            decimal TotalDP = 0m;
            decimal Sisa = 0m;

            //adding firman 280818 
            totaldp.Text = d.TotalBayar.Value.ToString("N0");
			nominal.Text = d.TotalBayar.Value.ToString("N0");
            sisa.Text = 0.ToString("N0");
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);

            detailPaymentControl.SetDetailPayment("SO", d.ID, Convert.ToDecimal(d.Unpaid.Value), Convert.ToDecimal(d.TotalBayar.Value));
           
            // Detail Invoice (Kiri Bawah)
            string DetailInvoice = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"3\"><em>Detail of invoice</em></td></tr>";
            foreach (var r in d.SalesOrderDJ) { DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>"; }
            foreach (var r in d.SalesOrderGJ) { DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>"; }
            foreach (var r in d.SalesOrderPG) { DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>"; }
            foreach (var r in d.SalesOrderLD) { DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>"; }
            DetailInvoice += "</table>";
            detail_invoice.Text = DetailInvoice;

            string Header = "";
            string Item = "";
            int index = 1;
            decimal DP = 0m;

            string w1 = " style='border:none; vertical-align:top; text-align:left; width:180px;' ";
            string w2 = " style='border:none; vertical-align:top; text-align:left; ' ";
            string w3 = " style='border:none; vertical-align:top; text-align:center; width:60px;' ";
            string w4 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
            string w5 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
            string w6 = " style='border:none; vertical-align:top; text-align:right; width:180px;' ";

            Header += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px; \" >";
            Header += "<tr>";
            Header += "<th " + w1 + ">" + "<em>Code NO</em> / CODE" + "</th>";
            Header += "<th " + w2 + ">" + "<em>ITEM & Description</em> / Nama Barang & Keterangan" + "</th>";
            Header += "<th " + w3 + ">" + "<em>Qty</em>" + "</th>";
            Header += "<th " + w4 + ">" + "<em>Price</em> / Harga" + "</th>";
            Header += "<th " + w5 + ">" + "<em>Discount</em>" + "</th>";
            Header += "<th " + w6 + ">" + "<em>Price</em> / Harga Jual (Rp)" + "</th>";
            Header += "</tr>";
            Header += "</table>";

            headerlist.Text = Header;

            Item += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px;\" >";

            foreach (var r in d.SalesOrderPG)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value 
                        + r.DiscountProgramNominal.Value 
                        + r.DiscountGift.Value 
                        + r.DiscountRound.Value
                        + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                        + r.DiscountBank
                        + r.DiscountOther
                        ;
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.StockProductPG.Nomor + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.StockProductPG.CharProductItem.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";

            }
            
            Item += "</table>";
            itemlist.Text = Item;
        }
    }
}