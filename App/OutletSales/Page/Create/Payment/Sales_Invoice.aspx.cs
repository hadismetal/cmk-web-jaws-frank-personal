using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

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

    // Prefetch untuk rantai lazy-load r.StockProductDJ/GJ/PG/LD_Stone1B (+ nested CharProduct/CharFrameColor)
    // yang sebelumnya trigger 1-3 query tambahan PER BARIS item di dalam foreach (N+1). Replikasi dari
    // Palace, TAPI diadaptasi: pakai LINQ-to-SQL nav-property yang SAMA dengan yang sudah dipakai per-baris
    // di kode asli (r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama dkk), bukan raw SQL
    // dengan nama kolom FK yang ditebak seperti versi Palace - supaya query dijamin benar oleh mapping
    // LINQ-to-SQL yang sudah ada (kalau nama property salah, ini tidak akan ke-compile), bukan asumsi nama
    // kolom di database yang tidak bisa diverifikasi dari sini.
    private class StockDisplayInfo
    {
        public string Nomor;
        public string ItemNama;
        public string ColorNama;    // PG saja
        public decimal? KadarLogam; // PG saja
        public decimal? GrossWeight; // PG saja
        public decimal? TotalCarat; // LD saja
    }
    private Dictionary<int, StockDisplayInfo> PrefetchStockDisplayDJ(List<int> ids)
    {
        Dictionary<int, StockDisplayInfo> result = new Dictionary<int, StockDisplayInfo>();
        if (ids == null || ids.Count == 0) return result;
        var rows = db.StockProductDJ.Where(sp => ids.Contains(sp.ID))
            .Select(sp => new { sp.ID, sp.Nomor, ItemNama = sp.StockProductDJ_CharProduct.CharProductItem.Nama })
            .ToList();
        foreach (var row in rows)
        {
            result[row.ID] = new StockDisplayInfo { Nomor = row.Nomor, ItemNama = row.ItemNama };
        }
        return result;
    }
    private Dictionary<int, StockDisplayInfo> PrefetchStockDisplayGJ(List<int> ids)
    {
        Dictionary<int, StockDisplayInfo> result = new Dictionary<int, StockDisplayInfo>();
        if (ids == null || ids.Count == 0) return result;
        var rows = db.StockProductGJ.Where(sp => ids.Contains(sp.ID))
            .Select(sp => new { sp.ID, sp.Nomor, ItemNama = sp.StockProductGJ_CharProduct.CharProductItem.Nama })
            .ToList();
        foreach (var row in rows)
        {
            result[row.ID] = new StockDisplayInfo { Nomor = row.Nomor, ItemNama = row.ItemNama };
        }
        return result;
    }
    // PG beda dari DJ/GJ: CharProductItem/CharFrameColor ada langsung di StockProductPG (bukan lewat
    // tabel _CharProduct terpisah) - dikonfirmasi dari pemakaian r.StockProductPG.CharProductItem/
    // CharFrameColor langsung (tanpa _CharProduct) di kode asli method ini.
    // CATATAN ADAPTASI: TIDAK menyertakan lookup BeratTimbang ("decimal c = Db.SingleDecimal(...)")
    // seperti versi Palace - kode Frank yang sudah ada TIDAK memiliki override BeratTimbang itu sama
    // sekali (langsung pakai GrossWeight apa adanya), jadi menambahkannya akan mengubah tampilan
    // invoice PG yang sudah ada, bukan sekadar optimasi akses data. Prefetch di sini hanya mencakup
    // apa yang benar-benar dipakai Frank saat ini.
    private Dictionary<int, StockDisplayInfo> PrefetchStockDisplayPG(List<int> ids)
    {
        Dictionary<int, StockDisplayInfo> result = new Dictionary<int, StockDisplayInfo>();
        if (ids == null || ids.Count == 0) return result;
        var rows = db.StockProductPG.Where(sp => ids.Contains(sp.ID))
            .Select(sp => new { sp.ID, sp.Nomor, ItemNama = sp.CharProductItem.Nama, ColorNama = sp.CharFrameColor.Nama, sp.KadarLogam, sp.GrossWeight })
            .ToList();
        foreach (var row in rows)
        {
            result[row.ID] = new StockDisplayInfo
            {
                Nomor = row.Nomor,
                ItemNama = row.ItemNama,
                ColorNama = row.ColorNama,
                KadarLogam = row.KadarLogam,
                GrossWeight = row.GrossWeight
            };
        }
        return result;
    }
    private Dictionary<int, StockDisplayInfo> PrefetchStockDisplayLD(List<int> ids)
    {
        Dictionary<int, StockDisplayInfo> result = new Dictionary<int, StockDisplayInfo>();
        if (ids == null || ids.Count == 0) return result;
        var rows = db.StockProductLD_Stone1B.Where(sp => ids.Contains(sp.ID))
            .Select(sp => new { sp.ID, sp.Nomor, sp.TotalCarat })
            .ToList();
        foreach (var row in rows)
        {
            result[row.ID] = new StockDisplayInfo { Nomor = row.Nomor, TotalCarat = row.TotalCarat };
        }
        return result;
    }
    // Prefetch kolom TotalFeeExtraRingSize sekali per tipe produk untuk semua baris di SO ini, bukan
    // 1 query per baris seperti sebelumnya. Query text SAMA PERSIS dengan raw SQL per-baris yang sudah
    // ada di kode asli (cuma "ID = X" jadi "ID IN (...)"), jadi tidak ada tebakan nama kolom/tabel baru.
    private Dictionary<int, decimal> PrefetchFeeExtraRingSize(string table, List<int> ids)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (ids == null || ids.Count == 0) return result;
        string idList = string.Join(",", ids.Distinct());
        string query = "SELECT ID, ISNULL(TotalFeeExtraRingSize,0) AS Val FROM " + table + " WHERE ID IN (" + idList + ")";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["Val"]);
        }
        return result;
    }

    // CountPPNItem() lama membuka SqlConnection baru + query PER BARIS untuk transaksi tanggal
    // >2024-12-30 (yang berarti SEMUA transaksi sekarang, per tanggal sistem berjalan). Karena satu
    // invoice cuma punya 1 SalesOrder.Tgl, cabang tanggal yang diambil PASTI SAMA untuk semua baris
    // tipe produk yang sama - jadi bisa diprefetch sekali per tipe produk untuk semua baris sekaligus.
    // Query text SAMA PERSIS dengan raw SQL per-baris yang sudah ada di CountPPNItem asli Frank (di
    // bawah), cuma predikat ID tunggal diganti IN-list.
    private Dictionary<int, decimal> PrefetchRateppnDJ(DateTime tgl, Dictionary<int, int> idProductByRowId)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (idProductByRowId == null || idProductByRowId.Count == 0) return result;
        if (tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.02m;
        }
        else if (tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.022m;
        }
        else if (tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
        {
            string idpList = string.Join(",", idProductByRowId.Values.Distinct());
            string query = "SELECT A.ID, CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductDJ A LEFT JOIN DataSupplier B ON A.Supplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID IN (" + idpList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            Dictionary<int, bool> isInSourceByProduct = new Dictionary<int, bool>();
            foreach (DataRow row in dt.Rows) isInSourceByProduct[Convert.ToInt32(row["ID"])] = Convert.ToBoolean(row["IsInSource"]);
            foreach (var kv in idProductByRowId)
            {
                bool isInSource;
                isInSourceByProduct.TryGetValue(kv.Value, out isInSource);
                result[kv.Key] = isInSource ? 1.0165m : 1.011m;
            }
        }
        else
        {
            string idList = string.Join(",", idProductByRowId.Keys.Distinct());
            string query = "SELECT a.ID, CAST(a.PPNPembagi AS Numeric(9,4)) AS PPNPembagi FROM SalesOrderDJ a JOIN StockProductDJ B ON a.IDProduct = B.ID WHERE a.ID IN (" + idList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            foreach (DataRow row in dt.Rows) result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["PPNPembagi"]);
        }
        return result;
    }
    private Dictionary<int, decimal> PrefetchRateppnGJ(DateTime tgl, Dictionary<int, int> idProductByRowId)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (idProductByRowId == null || idProductByRowId.Count == 0) return result;
        if (tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.02m;
        }
        else if (tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.022m;
        }
        else if (tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
        {
            string idpList = string.Join(",", idProductByRowId.Values.Distinct());
            string query = "SELECT A.ID, CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductGJ A LEFT JOIN DataSupplier B ON A.Supplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID IN (" + idpList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            Dictionary<int, bool> isInSourceByProduct = new Dictionary<int, bool>();
            foreach (DataRow row in dt.Rows) isInSourceByProduct[Convert.ToInt32(row["ID"])] = Convert.ToBoolean(row["IsInSource"]);
            foreach (var kv in idProductByRowId)
            {
                bool isInSource;
                isInSourceByProduct.TryGetValue(kv.Value, out isInSource);
                result[kv.Key] = isInSource ? 1.0165m : 1.011m;
            }
        }
        else
        {
            string idList = string.Join(",", idProductByRowId.Keys.Distinct());
            string query = "SELECT a.ID, CAST(a.PPNPembagi AS Numeric(9,4)) AS PPNPembagi FROM SalesOrderGJ a JOIN StockProductGJ B ON a.IDProduct = B.ID WHERE a.ID IN (" + idList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            foreach (DataRow row in dt.Rows) result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["PPNPembagi"]);
        }
        return result;
    }
    private Dictionary<int, decimal> PrefetchRateppnPG(DateTime tgl, Dictionary<int, int> idProductByRowId)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (idProductByRowId == null || idProductByRowId.Count == 0) return result;
        if (tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.02m;
        }
        else if (tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
        {
            foreach (int id in idProductByRowId.Keys) result[id] = 1.022m;
        }
        else if (tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
        {
            // PG beda dari DJ/GJ: kolom FK supplier-nya "IDSupplier", bukan "Supplier" - sama seperti
            // raw SQL per-baris yang sudah ada di CountPPNItem asli Frank untuk tipeproduct==2.
            string idpList = string.Join(",", idProductByRowId.Values.Distinct());
            string query = "SELECT A.ID, CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductPG A LEFT JOIN DataSupplier B ON A.IDSupplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID IN (" + idpList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            Dictionary<int, bool> isInSourceByProduct = new Dictionary<int, bool>();
            foreach (DataRow row in dt.Rows) isInSourceByProduct[Convert.ToInt32(row["ID"])] = Convert.ToBoolean(row["IsInSource"]);
            foreach (var kv in idProductByRowId)
            {
                bool isInSource;
                isInSourceByProduct.TryGetValue(kv.Value, out isInSource);
                result[kv.Key] = isInSource ? 1.0165m : 1.011m;
            }
        }
        else
        {
            string idList = string.Join(",", idProductByRowId.Keys.Distinct());
            string query = "SELECT a.ID, CAST(a.PPNPembagi AS Numeric(9,4)) AS PPNPembagi FROM SalesOrderPG a JOIN StockProductPG B ON a.IDProduct = B.ID WHERE a.ID IN (" + idList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            foreach (DataRow row in dt.Rows) result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["PPNPembagi"]);
        }
        return result;
    }
    // ADAPTASI: cabang tanggal terbaru (>2024-12-30) di Frank TIDAK JOIN ke StockProductLD_Stone1b
    // sama sekali (beda dari Palace, yang masih JOIN ke tabel itu meski tidak ada kolomnya yang
    // dipakai) - dikonfirmasi dari raw SQL CountPPNItem asli Frank untuk tipeproduct==4 di bawah
    // ("FROM SalesOrderLD A WHERE a.ID = @ID", tanpa JOIN). Dipertahankan sama, TIDAK menambahkan
    // JOIN yang tidak ada di kode Frank - itu akan mengubah semantik (INNER JOIN bisa membuang baris
    // yang tidak match) bukan sekadar optimasi akses data.
    private Dictionary<int, decimal> PrefetchRateppnLD(DateTime tgl, List<int> ids)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (ids == null || ids.Count == 0) return result;
        if (tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
        {
            foreach (int id in ids) result[id] = 1.02m;
        }
        else if (tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
        {
            foreach (int id in ids) result[id] = 1.011m;
        }
        else
        {
            string idList = string.Join(",", ids.Distinct());
            string query = "SELECT a.ID, CAST(a.PPNPembagi AS Numeric(9,4)) AS PPNPembagi FROM SalesOrderLD a WHERE a.ID IN (" + idList + ")";
            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            foreach (DataRow row in dt.Rows) result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["PPNPembagi"]);
        }
        return result;
    }

    private decimal ComputePPN(bool isUsePPN, decimal rateppn, decimal totalBuyAre)
    {
        if (!isUsePPN) return 0m;
        return totalBuyAre - (totalBuyAre / rateppn);
    }

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
            // Materialize sekali - py cuma dibaca lewat .Count()/.Sum() di banyak titik di bawah,
            // tanpa .ToList() query yang sama re-execute ke DB tiap kali dipanggil (~6-7x per load).
            var py = (from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == myID
                     orderby p.OperatorTgl descending
                     select p).ToList();

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
            //adminsales.Text = Str.CapUpper(d.SalesNama);
			
			string salescrossbrand = Db.SingleString("SELECT TOP 1 ISNULL(NamasalesCross,'') [SalesCross] FROM SalesOrder WHERE ID = " + d.ID, Db.Cnn_DB);
			
            adminsales.Text = (!string.IsNullOrEmpty(salescrossbrand)) ? salescrossbrand : Str.CapUpper(d.SalesNama);

            decimal DPP = 0m;
            decimal PPN = 0m;
            decimal Nominal = 0m;
            decimal TotalDP = 0m;
            decimal Sisa = 0m;

            if (d.TradeIn == true)
            {
                decimal xTotalRupiah = d.HargaRupiah.Value;
                decimal xTotalDiscount = 0m;

                var yDiscDJ = db.SalesOrderDJ.Where(p => p.IDForm == d.ID);
                var yDiscPG = db.SalesOrderPG.Where(p => p.IDForm == d.ID);
                var yDiscGJ = db.SalesOrderGJ.Where(p => p.IDForm == d.ID);
                var yDiscLD = db.SalesOrderLD.Where(p => p.IDForm == d.ID);

                var xDiscDJ = (yDiscDJ.Count() > 0) 
                            ? (yDiscDJ.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscPG = (yDiscPG.Count() > 0)
                            ? (yDiscPG.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscGJ = (yDiscGJ.Count() > 0)
                            ? (yDiscGJ.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscLD = (yDiscLD.Count() > 0)
                            ? (yDiscLD.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                xTotalDiscount = (xDiscDJ != null ? xDiscDJ : 0m)
                                + (xDiscPG != null ? xDiscPG : 0m)
                                + (xDiscGJ != null ? xDiscGJ : 0m)
                                + (xDiscLD != null ? xDiscLD : 0m)
                                ;

                //DPP = (xTotalRupiah - xTotalDiscount) / 1.02m;
				
				//Cetak Invoice Jika ada pembayaran laku emas
                decimal LakuEmasNominal = 0m;
				decimal DebitLakuEmasNominal = 0m;
                /*foreach (var i in py)
                {
                    if(i.IDPaymentType == 11)
                    {
                        LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                    }
					if(((i.IDPaymentType == 4 || i.IDPaymentType == 2) && (i.IDEDC == 24 || i.IDEDC == 26 || i.IDEDC == 27 || i.IDEDC == 28)) || (i.IDPaymentType == 15 &&(i.IDEDC == 31 || i.IDEDC == 32)))
					{
						LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
					}
                }
				if (py.Count(p => p.IDPaymentType == 11) > 0)
				{
					le.Text = LakuEmasNominal.ToString("N0");
					paymentwithlakuemaspanel.Visible = true;
				}
				else if(py.Count(p => ((p.IDPaymentType == 4 || p.IDPaymentType == 2) && (p.IDEDC == 24 || p.IDEDC == 26 || p.IDEDC == 27 || p.IDEDC == 28) || (p.IDPaymentType == 15 && (p.IDEDC == 31 || p.IDEDC == 32)))) > 0)
				{
					try
					{
						decimal weight_pg_total = 0m;
						decimal weight_pg = 0m;
						decimal weight_lm_total = 0m;
						decimal weight_lm = 0m;
						decimal rate_lm = 0m;
						foreach (var j in py)
						{
							if(((j.IDPaymentType == 4 || j.IDPaymentType == 2) && (j.IDEDC == 24 || j.IDEDC == 26 || j.IDEDC == 27 || j.IDEDC == 28)) || (j.IDPaymentType == 15 &&(j.IDEDC == 31 || j.IDEDC == 32)))
							{
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(RateLM,0) FROM SalesReceiptDetailDebitLakuEmas WHERE IDSalesReceiptDetail = "+j.ID+" AND RateLM IS NOT NULL ", Db.Cnn_DB);
							}
						}
						var pg = d.SalesOrderPG;
						
						decimal harga_pg = 0m;
						decimal harga_pg_total = 0m;
						decimal harga_lm = 0m;
						decimal harga_lm_total = 0m;
						decimal ongkos = 0m;
						foreach(var item in pg)
						{
							weight_pg = item.StockProductPG.NettoWeight ?? 0m;
							weight_lm = (3 * weight_pg) / 4;
							int countisanyrate = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm, Db.Cnn_DB);
							if(countisanyrate > 0)
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(TGPJual,0) [TGPJual] FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm ,Db.Cnn_DB);
							
							harga_lm = weight_lm * rate_lm;
							weight_pg_total += item.StockProductPG.NettoWeight ?? 0m;
							weight_lm_total += (item.StockProductPG.NettoWeight ?? 0m) * (0.75m);
							harga_pg_total += item.TotalBayar ?? 0m;
							harga_lm_total += harga_lm;
							
						}
						//weight_lm_total = (3 * weight_pg_total) / 4;
						//harga_lm = weight_lm_total * rate_lm;
						//harga_pg = d.TotalBayar ?? 0m;
						//ongkos = harga_pg - harga_lm;
						LakuEmasNominal = harga_lm_total;
						
						debitlepg.Text = (harga_pg_total).ToString("N0");
						debitlelm.Text = (harga_lm_total).ToString("N0");
						paymentwithdebitlakuemaspanel.Visible = true;
					}
					catch
					{
						LakuEmasNominal = 0m;
						//le.Text = (0m).ToString("N0");
						//paymentwithlakuemaspanel.Visible = false;
						paymentwithdebitlakuemaspanel.Visible = false;
					}
				}
				else
				{
					le.Text = (0m).ToString("N0");
					paymentwithlakuemaspanel.Visible = false;
				}*/
				
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lit_ppn3.Text = "2,2 %";
				lit_ppn4.Text = "2,2 %";
				lit_ppn5.Text = "2,2 %";
				lit_ppn6.Text = "2,2 %";
				lblppn.InnerText = "11%";
				decimal rateppn = 1.022m;
				if(d.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					lit_ppn.Text = "2 %";
					lit_ppn2.Text = "2 %";
					lit_ppn3.Text = "2 %";
					lit_ppn4.Text = "2 %";
					lit_ppn5.Text = "2 %";
					lit_ppn6.Text = "2 %";
					lblppn.InnerText = "10%";
					rateppn = 1.02m;
				}
				
				if(!IsUsePPN)
				{
					lit_ppn.Text = "0 %";
					lit_ppn2.Text = "0 %";
					lit_ppn3.Text = "0 %";
					lit_ppn4.Text = "0 %";
					lit_ppn5.Text = "0 %";
					lit_ppn6.Text = "0 %";
					
					divuseppn.Visible = false;
					lit_ppn_notice.Visible = false;
					lit_ppn_notice2.Visible = false;
					lit_ppn_notice3.Visible = false;
					
					lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
				}
				else
				{
					divuseppn.Visible = true;
					lit_ppn_notice.Visible = true;
					lit_ppn_notice2.Visible = true;
					lit_ppn_notice3.Visible = true;
					
					lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
				}
				
                DPP = (!IsUsePPN) ? (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) : (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) / rateppn;
                PPN = (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) - DPP;
                Nominal = (xTotalRupiah - xTotalDiscount);
            }
            else
            {
                //DPP = d.TotalBayar.Value / 1.02m;
                //DPP = (KodeLokasi == "E46" || KodeLokasi == "P18" || KodeLokasi == "P45") ? d.TotalBayar.Value : d.TotalBayar.Value / 1.02m;
                //PPN = d.TotalBayar.Value - DPP;
                //Nominal = d.TotalBayar.Value;
				
                //Cetak Invoice Jika ada pembayaran laku emas
                decimal LakuEmasNominal = 0m;
				decimal DebitLakuEmasNominal = 0m;
				/*foreach (var i in py)
                {
                    if(i.IDPaymentType == 11)
                    {
                        LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                    }
                }
				if (py.Count(p => p.IDPaymentType == 11) > 0)
				{
					foreach (var i in py)
					{
						if(i.IDPaymentType == 11)
						{
							LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
						}
					}
                    le.Text = LakuEmasNominal.ToString("N0");
					paymentwithlakuemaspanel.Visible = true;
				}
				else if(py.Count(p => ((p.IDPaymentType == 4 || p.IDPaymentType == 2) && (p.IDEDC == 24 || p.IDEDC == 26 || p.IDEDC == 27 || p.IDEDC == 28)) || (p.IDPaymentType == 15 && (p.IDEDC == 31 || p.IDEDC == 32))) > 0)
				{
					try
					{
						decimal weight_pg_total = 0m;
						decimal weight_pg = 0m;
						decimal weight_lm_total = 0m;
						decimal weight_lm = 0m;
						decimal rate_lm = 0m;
						
						var pg = d.SalesOrderPG;
						
						decimal harga_pg = 0m;
						decimal harga_pg_total = 0m;
						decimal harga_lm = 0m;
						decimal harga_lm_total = 0m;
						decimal ongkos = 0m;
						foreach(var item in pg)
						{
							weight_pg = item.StockProductPG.NettoWeight ?? 0m;
							weight_lm = (3 * weight_pg) / 4;
							int countisanyrate = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm, Db.Cnn_DB);
							if(countisanyrate > 0)
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(TGPJual,0) [TGPJual] FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm ,Db.Cnn_DB);
							
							harga_lm = weight_lm * rate_lm;
							weight_pg_total += item.StockProductPG.NettoWeight ?? 0m;
							weight_lm_total += (item.StockProductPG.NettoWeight ?? 0m) * (0.75m);
							harga_pg_total += item.TotalBayar ?? 0m;
							harga_lm_total += harga_lm;
						}
						
						//weight_lm_total = (3 * weight_pg_total) / 4;
						//harga_lm = weight_lm_total * rate_lm;
						//harga_pg = d.TotalBayar ?? 0m;
						//ongkos = harga_pg - harga_lm;
						LakuEmasNominal = LakuEmasNominal + harga_lm_total;
						
						debitlepg.Text = (harga_pg_total).ToString("N0");
						debitlelm.Text = (harga_lm_total).ToString("N0");
						paymentwithdebitlakuemaspanel.Visible = true;
					}
					catch
					{
						LakuEmasNominal = LakuEmasNominal + 0m;
						paymentwithdebitlakuemaspanel.Visible = false;
					}
				}
				else
				{
					le.Text = (0m).ToString("N0");
					paymentwithlakuemaspanel.Visible = false;
				}*/
				
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lit_ppn3.Text = "2,2 %";
				lit_ppn4.Text = "2,2 %";
				lit_ppn5.Text = "2,2 %";
				lit_ppn6.Text = "2,2 %";
				lblppn.InnerText = "11%";
				decimal rateppn = 1.022m;
				if(d.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					lit_ppn.Text = "2 %";
					lit_ppn2.Text = "2 %";
					lit_ppn3.Text = "2 %";
					lit_ppn4.Text = "2 %";
					lit_ppn5.Text = "2 %";
					lit_ppn6.Text = "2 %";
					lblppn.InnerText = "10%";
					rateppn = 1.02m;
				}
				
				if(!IsUsePPN)
				{
					lit_ppn.Text = "0 %";
					lit_ppn2.Text = "0 %";
					lit_ppn3.Text = "0 %";
					lit_ppn4.Text = "0 %";
					lit_ppn5.Text = "0 %";
					lit_ppn6.Text = "0 %";
					
					divuseppn.Visible = false;
					lit_ppn_notice.Visible = false;
					lit_ppn_notice2.Visible = false;
					lit_ppn_notice3.Visible = false;
					
					lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
				}
				else
				{
					divuseppn.Visible = true;
					lit_ppn_notice.Visible = true;
					lit_ppn_notice2.Visible = true;
					lit_ppn_notice3.Visible = true;
					
					lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
				}
				
                DPP = (!IsUsePPN) ? (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) / rateppn;
                PPN = (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) - DPP;
                Nominal = d.TotalBayar.Value ;
            }
			
            //var pym = from p in db.SalesReceiptDetail where p.SalesReceipt.IDSalesOrder.Value == d.ID select p;
            //if (pym.Count() > 0)
            //{
            //    if (d.Unpaid < 0 && pym.Any(p => p.PaymentType.ID == 5 || p.PaymentType.ID == 8)) 
            //    {
            //        LebihBayar_DP_Voucher = true;
            //        LebihBayar_Pure_Voucher = !pym.Any(p => p.PaymentType.ID != 5);

            //    }

            //    string DetailPayment = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td style=\"vertical-align:top;\" colspan=\"4\"><em>Detail of payment</em></td></tr>";

            //    if (LebihBayar_DP_Voucher && LebihBayar_Pure_Voucher)
            //    {
            //        string vKodeVoucher = "";
            //        int count = 0;
            //        foreach (var r in pym)
            //        {
            //            if (count > 0) { vKodeVoucher += ", "; }
            //            vKodeVoucher += r.CCNumber;
            //            count++;
            //        }
            //        vKodeVoucher = "(" + vKodeVoucher + ")";
            //        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">Voucher</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + vKodeVoucher + "</td></tr>";

            //        //terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            //    }
            //    else
            //    {

            //                foreach (var r in pym)
            //                {
            //                // keterangan debit card
            //                if (r.IDPaymentType == 2 && r.IDProgram.HasValue && r.IDBankIssuer.HasValue && r.IDCard.HasValue && !r.IDEDC.HasValue)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " " + r.ProgramCicilan.Nama + ")</td></tr>";
            //                }
            //                // keterangan credit card
            //                else if (r.IDPaymentType == 2 && r.IDProgram.HasValue && r.IDBankIssuer.HasValue && r.IDCard.HasValue && r.IDEDC.HasValue)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " " + r.ProgramCicilan.Nama + " / EDC : " + r.EDCList.Nama + ")</td></tr>";
            //                }
            //                else
            //                {
            //                    if (LebihBayar_DP_Voucher && pym.Count() == 1)
            //                    {
            //                        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + ((r.CCNumber) != null ? "( " + r.CCNumber + " )" : "") + "</td></tr>";
            //                    }
            //                    else if (r.IDPaymentType != 5)
            //                    {

            //                        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td></td></tr>";
            //                    }
            //                }

            //        }

            //        //firman add 27082018 grouping payment voucher pada sales invoice                  
            //        var pymVchr = from pyv in pym where pyv.IDPaymentType == 5 select pyv;


            //        if (pymVchr.Count() > 0)
            //        {
                        
            //            var group_payment_voucher = from gpym in pym                                                   
            //                                        group gpym by new { gpym.IDPaymentType, gpym.Nominal, gpym.CCName } into spym
            //                                        select new
            //                                        {
            //                                            spym.Key.IDPaymentType,
            //                                            spym.Key.CCName,
            //                                            spym.Key.Nominal,
            //                                            Qty = spym.Count(),
            //                                            Total = spym.Key.Nominal * spym.Count()
            //                                        };
            //            foreach (var pymVchrItem in group_payment_voucher)
            //            {
            //                if(pymVchrItem.IDPaymentType == 5)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\"> Voucher </td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + pymVchrItem.Total.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + pymVchrItem.Qty.ToString() + "x " + pymVchrItem.Nominal.Value.ToString("N0") + " " + pymVchrItem.CCName.ToString() + ")</td></tr>";
            //                }
            //            }
            //        }



            //    }
            //    DetailPayment += "</table>";
            //    detail_payment.Text = DetailPayment;
            //}

            // Detail Invoice (Kiri Bawah)
			decimal PPNItem = 0m;
            string DetailInvoice = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"3\"><em>Detail of invoice</em></td></tr>";
            foreach (var r in d.SalesOrderDJ) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceDJ.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderGJ) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceGJ.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderPG) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoicePG.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderLD) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceLD.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            DetailInvoice += "</table>";
            detail_invoice.Text = DetailInvoice;

            if (d.DataSales.CompanyBrand.NamaKode == "F")
                frank_term.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "M")
                mondial.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "S")
                missmondial.Visible = true;

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

            // Prefetch sekali untuk semua baris DJ/GJ/PG/LD di SO ini - lihat komentar di
            // PrefetchStockDisplayDJ/GJ/PG/LD, PrefetchFeeExtraRingSize dan PrefetchRateppnDJ/GJ/PG/LD.
            Dictionary<int, StockDisplayInfo> stockDisplayDJ = PrefetchStockDisplayDJ(d.SalesOrderDJ.Select(p => p.IDProduct).Distinct().ToList());
            Dictionary<int, StockDisplayInfo> stockDisplayGJ = PrefetchStockDisplayGJ(d.SalesOrderGJ.Select(p => p.IDProduct).Distinct().ToList());
            Dictionary<int, StockDisplayInfo> stockDisplayPG = PrefetchStockDisplayPG(d.SalesOrderPG.Select(p => p.IDProduct).Distinct().ToList());
            Dictionary<int, StockDisplayInfo> stockDisplayLD = PrefetchStockDisplayLD(d.SalesOrderLD.Select(p => p.IDProduct).Distinct().ToList());
            Dictionary<int, decimal> feeExtraRingSizeDJ = PrefetchFeeExtraRingSize("SalesOrderDJ", d.SalesOrderDJ.Select(p => p.ID).ToList());
            Dictionary<int, decimal> feeExtraRingSizeGJ = PrefetchFeeExtraRingSize("SalesOrderGJ", d.SalesOrderGJ.Select(p => p.ID).ToList());
            Dictionary<int, decimal> feeExtraRingSizePG = PrefetchFeeExtraRingSize("SalesOrderPG", d.SalesOrderPG.Select(p => p.ID).ToList());
            Dictionary<int, decimal> feeExtraRingSizeLD = PrefetchFeeExtraRingSize("SalesOrderLD", d.SalesOrderLD.Select(p => p.ID).ToList());
            // CountPPNItem() lama - lihat komentar di PrefetchRateppnDJ/GJ/PG/LD.
            DateTime invoiceTgl = Convert.ToDateTime(d.Tgl);
            Dictionary<int, decimal> rateppnDJ = PrefetchRateppnDJ(invoiceTgl, d.SalesOrderDJ.ToDictionary(p => p.ID, p => p.IDProduct));
            Dictionary<int, decimal> rateppnGJ = PrefetchRateppnGJ(invoiceTgl, d.SalesOrderGJ.ToDictionary(p => p.ID, p => p.IDProduct));
            Dictionary<int, decimal> rateppnPG = PrefetchRateppnPG(invoiceTgl, d.SalesOrderPG.ToDictionary(p => p.ID, p => p.IDProduct));
            Dictionary<int, decimal> rateppnLD = PrefetchRateppnLD(invoiceTgl, d.SalesOrderLD.Select(p => p.ID).ToList());

            foreach (var r in d.SalesOrderDJ)
            {
				decimal fee_extra_ring_size;
				feeExtraRingSizeDJ.TryGetValue(r.ID, out fee_extra_ring_size);
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

                StockDisplayInfo sdDJ;
                stockDisplayDJ.TryGetValue(r.IDProduct, out sdDJ);

                Item += "<tr>";
                Item += "<td " + w1 + ">" + (sdDJ != null ? sdDJ.Nomor : null) + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper((sdDJ != null ? sdDJ.ItemNama : null)) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value + fee_extra_ring_size) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";

                decimal rateppnDJRow;
                rateppnDJ.TryGetValue(r.ID, out rateppnDJRow);
                PPNItem += ComputePPN(IsUsePPN, rateppnDJRow, Convert.ToDecimal(r.TotalBayar));

            }
            foreach (var r in d.SalesOrderGJ)
            {
				decimal fee_extra_ring_size;
				feeExtraRingSizeGJ.TryGetValue(r.ID, out fee_extra_ring_size);
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

                StockDisplayInfo sdGJ;
                stockDisplayGJ.TryGetValue(r.IDProduct, out sdGJ);

                Item += "<tr>";
                Item += "<td " + w1 + ">" + (sdGJ != null ? sdGJ.Nomor : null) + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper((sdGJ != null ? sdGJ.ItemNama : null)) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value + fee_extra_ring_size) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";

                decimal rateppnGJRow;
                rateppnGJ.TryGetValue(r.ID, out rateppnGJRow);
                PPNItem += ComputePPN(IsUsePPN, rateppnGJRow, Convert.ToDecimal(r.TotalBayar));
            }
            foreach (var r in d.SalesOrderPG)
            {
				decimal fee_extra_ring_size;
				feeExtraRingSizePG.TryGetValue(r.ID, out fee_extra_ring_size);
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

                StockDisplayInfo sdPG;
                stockDisplayPG.TryGetValue(r.IDProduct, out sdPG);
                decimal kadarLogamPG = (sdPG != null && sdPG.KadarLogam.HasValue) ? sdPG.KadarLogam.Value : 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + (sdPG != null ? sdPG.Nomor : null) + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper((sdPG != null ? sdPG.ItemNama : null)) + " BERAT " + (sdPG != null ? sdPG.GrossWeight : null) + " KADAR " + Math.Floor(kadarLogamPG).ToString("N0") + "% WARNA " + Str.CapUpper((sdPG != null ? sdPG.ColorNama : null)) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value + fee_extra_ring_size) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";

                decimal rateppnPGRow;
                rateppnPG.TryGetValue(r.ID, out rateppnPGRow);
                PPNItem += ComputePPN(IsUsePPN, rateppnPGRow, Convert.ToDecimal(r.TotalBayar));
            }
            foreach (var r in d.SalesOrderLD)
            {
				decimal fee_extra_ring_size;
				feeExtraRingSizeLD.TryGetValue(r.ID, out fee_extra_ring_size);
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

                StockDisplayInfo sdLD;
                stockDisplayLD.TryGetValue(r.IDProduct, out sdLD);

                Item += "<tr>";
                Item += "<td " + w1 + ">" + (sdLD != null ? sdLD.Nomor : null) + "</td>";
                Item += "<td " + w2 + ">" + Str.Number3((sdLD != null && sdLD.TotalCarat.HasValue) ? sdLD.TotalCarat.Value : 0m) + " Carat " + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value + fee_extra_ring_size) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";

                decimal rateppnLDRow;
                rateppnLD.TryGetValue(r.ID, out rateppnLDRow);
                PPNItem += ComputePPN(IsUsePPN, rateppnLDRow, Convert.ToDecimal(r.TotalBayar));
            }
            foreach (var r in d.SalesOrderPackaging)
            {
                decimal Discount = 0m;
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
                Item += "<td " + w1 + ">" + r.Packaging.Kode + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.Packaging.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                Item += "</tr>";

            }
            foreach (var r in d.SalesOrderSouvenir)
            {
                if (r.TipeProduct == 0 || r.TipeProduct == null)
                {
                    decimal Discount = 0m;
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
                    Item += "<td " + w1 + ">" + r.Souvenir.Kode + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(r.Souvenir.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
                else if (r.TipeProduct == 1)
                {
                    /*var rs = from p in db.SalesOrderSouvenir
                             join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                             where p.IDForm == myID && p.TipeProduct == 1
                             select new { p, q };*/
                    var bs = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1 && d.StatusPembayaran == false)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.StockProductDJ_CharProduct.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }

                else if (r.TipeProduct == 2)
                {
                    var bs = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.StockProductGJ_CharProduct.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
                else if (r.TipeProduct == 3)
                {
                    var bs = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }

                else if (r.TipeProduct == 4)
                {
                    var bs = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + " - " + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
            }
            Item += "</table>";
            itemlist.Text = Item;
			
			// reuse py (already materialized above with the same filter) instead of a redundant round-trip
			TotalDP = Convert.ToDecimal(py.Sum(p => p.Nominal));
            Sisa = Nominal - TotalDP;
			
			PPN = PPNItem; 
			DPP = d.TotalBayar.Value - PPNItem;

            dpp.Text = DPP.ToString("N0");
            ppn.Text = PPN.ToString("N0");
            nominal.Text = Nominal.ToString("N0");
            totaldp.Text = TotalDP.ToString("N0");
            sisa.Text = Sisa.ToString("N0");

            //adding firman 280818 
            totaldp.Text = d.TotalBayar.Value.ToString("N0");
            sisa.Text = 0.ToString("N0");
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);

            detailPaymentControl.SetDetailPayment("SO", d.ID, Convert.ToDecimal(d.Unpaid.Value), Convert.ToDecimal(d.TotalBayar.Value));
        }
    }
	
}