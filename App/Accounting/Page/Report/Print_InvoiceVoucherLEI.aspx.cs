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
        var d = db.SalesOrderPG.SingleOrDefault(p => p.ID == myID);
		var c = Db.SingleDecimal("Select top 1 isnull(BeratTimbang,0.00) BeratTimbang from SalesOrderPG WHERE ID = "+ d.ID +" ",Db.Cnn_DB);
        if (d == null)
        {
            Response.Redirect("Sales.aspx");
        }
        else
        {
            try
            {
                ApiVoucherLEI le = new ApiVoucherLEI();
                le.AddTransactionLEI(d.IDForm);
            }
            catch {}
			string resultapi = "";
			int statushit = 0;
			int count = Db.SingleInteger("SELECT COUNT('') [TOTAL] FROM VoucherLeiTransaction WHERE IDSalesOrder = " + d.SalesOrder.ID, Db.Cnn_DB);
			if(count == 1)
			{
				DataTable dt = Db.Rs("SELECT TOP 1 'Status Voucher LEI : ' + Response + ' - ' + TransactionID + ' ' [MESSAGE], ISNULL(StatusHit, 0) as [StatusHit] FROM VoucherLeiTransaction WHERE IDSalesOrder = " + d.SalesOrder.ID, Db.Cnn_DB);
				resultapi = dt.Rows[0]["MESSAGE"].ToString();
				statushit = Convert.ToInt32(dt.Rows[0]["StatusHit"]);
			}
			
            //Picture
            Upload.Image(imgpicture, d.StockProductPG.ImgPicture);

            string BrandText = "CMK";
            string Cabang = "";
            string AlamatCabang = "";
            string KodeLokasi = "";
			bool IsUsePPN = false;

            if (d.SalesOrder.TipeLokasi == 1)
            {
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.SalesOrder.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
				
				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
            }
            else
            {
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.SalesOrder.IDLokasi);

                // corporate exhibition
                if (d.SalesOrder.IDLokasi == 18 && x.Kode == "XPR")
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

            BrandText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(BrandText.ToLower());

            alamat.Text = Str.HtmlBreak(AlamatCabang);
            tgl.Text = Str.Tgl(d.SalesOrder.Tgl);
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            noinvoice.Text = d.Invoice;

            customer.Text = Str.CapUpper(d.SalesOrder.CustomerNama);
            nocustomer.Text = Str.CapUpper(d.SalesOrder.DataCustomer.NoCustomer);
            customer2.Text = Str.CapUpper(d.SalesOrder.CustomerNama);
            customerother.Text = Str.CapUpper(String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama) ? d.SalesOrder.CustomerNama : d.SalesOrder.CustomerReferenceNama);
			string salescrossbrand = Db.SingleString("SELECT TOP 1 ISNULL(NamasalesCross,'') [SalesCross] FROM SalesOrder WHERE ID = " + d.SalesOrder.ID, Db.Cnn_DB);
			
            adminsales.Text = (!string.IsNullOrEmpty(salescrossbrand)) ? salescrossbrand : Str.CapUpper(d.SalesOrder.SalesNama);
			
            decimal DPP = 0m;
            decimal PPN = 0m;
			decimal LakuEmasNominal = 0m;
			decimal DebitLakuEmasNominal = 0m;
			var py = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == d.IDForm
                     orderby p.OperatorTgl descending
                     select p;
            
            DPP = d.TotalBayar.Value;
			
			/*try
			{
				Db.Execute("EXEC sp_update_ppn " + myID + ",2,'0','0'" ,Db.Cnn_DB);
			}
			catch{}*/

            nominal.Text = d.TotalBayar.Value.ToString("N0");
            totaldp.Text = (d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value.ToString("N0") : (d.TotalDP.HasValue ? d.TotalDP.Value.ToString("N0") : "0");
            sisa.Text = d.TotalDP.HasValue ? (d.TotalBayar.Value - ((d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : d.TotalDP.Value)).ToString("N0") : (d.SalesOrder.StatusPembayaran == true) ? "0" : d.TotalBayar.Value.ToString("N0");

            // cetak detail of payment jika hanya 1 item plu
            int TotalItem = 0;
            TotalItem = d.SalesOrder.SalesOrderDJ.Count()
                        + d.SalesOrder.SalesOrderPG.Count()
                        + d.SalesOrder.SalesOrderGJ.Count()
                        + d.SalesOrder.SalesOrderLD.Count()
                        ;

            //adding firman 280818 user control untuk detail payment dari /UC/DetailPaymentControl.aspx
            if (TotalItem == 1)
            {
                detailPaymentControl.SetDetailPayment("PG", d.SalesOrder.ID,  Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));
            }

            string Header = "";
            string Item = "";

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
            Header += "<th " + w3 + ">" + "Qty" + "</th>";
            Header += "<th " + w4 + ">" + "" + "</th>";
            Header += "<th " + w5 + ">" + "" + "</th>";
            Header += "<th " + w6 + ">" + "<em>Price</em> / Harga Jual (Rp)" + "</th>";
            Header += "</tr>";
            Header += "</table>";
			
            headerlist.Text = Header;

            Item += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px;\" >";
            
            Item += "<tr>";
            Item += "<td " + w1 + ">" + d.StockProductPG.Nomor + "</td>";
            Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductPG.CharProductItem.Nama) + "</td>";
            Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
            Item += "<td " + w4 + ">" + "&nbsp;" + "</td>";
            Item += "<td " + w5 + ">" + "&nbsp;" + "</td>";
            Item += "<td " + w6 + ">" + Str.Number0(d.TotalBayar.Value) + "</td>";
            Item += "</tr>";

            Item += "</table>";
            itemlist.Text = Item;
			
			if(statushit == 1)
			{
				Response.Write("<script>alert('Voucher Sudah Aktif dan Dapat Digunakan.');</script>");
			}
			else if(statushit == 0)
			{
				Response.Write("<script>alert('Voucher Belum Aktif.');</script>");
			}
			else
			{
				Response.Write("<script>alert('" + resultapi + "');</script>");
			}
        }
    }
}