using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Globalization;

public partial class Page_RepairDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Ddl.Store1(locationcode);
            Ddl.TipeLokasi(tipelokasi);

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            // status.SelectedValue = "1";
            
            fillFilter();

            if (App.Refresh(this)) fillTableNew(sender, e);
        }
    }

    protected void fillTableNew(object sender, EventArgs e)    
    {
        tb.Rows.Clear();

        // =========================
        // HEADER ROW 1
        // =========================
        TableHeaderRow headerRow1 = new TableHeaderRow();
        headerRow1.CssClass = "header";

        headerRow1.Cells.Add(CreateHeaderCell("No.", 2, 1, "50px"));
        headerRow1.Cells.Add(CreateHeaderCell("Store", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. SalesOrder", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. Repair", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal Reparasi", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal Pembayaran", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("PLU Product", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. Invoice / SPK", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Customer", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Sales", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Reparasi", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Type Repair", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Jenis Reparasi", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Lokasi Reparasi", 2, 1, "100px"));
        headerRow1.Cells.Add(CreateHeaderCell("Jasa", 1, 3, "650px", "jasa-column"));
        headerRow1.Cells.Add(CreateHeaderCell("Gold", 1, 8, "1500px", "gold-column"));
        headerRow1.Cells.Add(CreateHeaderCell("Stone", 1, 9, "1200px", "stone-column"));
        headerRow1.Cells.Add(CreateHeaderCell("Total", 1, 6, "850px"));
        headerRow1.Cells.Add(CreateHeaderCell("Pembayaran", 1, 10, "1500px"));

        tb.Rows.Add(headerRow1);

        // =========================
        // HEADER ROW 2
        // =========================
        TableHeaderRow headerRow2 = new TableHeaderRow();
        headerRow2.CssClass = "header";

        headerRow2.Cells.Add(CreateHeaderCell("Biaya Reparasi M", 1, 1, null, "jasa-column"));        
        headerRow2.Cells.Add(CreateHeaderCell("Biaya Reparasi Jual", 1, 1, null, "jasa-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Total Biaya", 1, 1, null, "jasa-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Gross Weight Sebelum", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Nett Weight Sesudah", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Gross Weight Sesudah", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Selisih Berat", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Batas Toleransi", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Total Charge", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Emas", 1, 1, null, "gold-column"));        
        headerRow2.Cells.Add(CreateHeaderCell("Harga Jual Emas", 1, 1, null, "gold-column"));
        headerRow2.Cells.Add(CreateHeaderCell("PLU Setting", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Parcel", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Total Butir", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Carat / Butir", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Total Carat", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Stone (USD)", 1, 1, null, "stone-column"));        
        headerRow2.Cells.Add(CreateHeaderCell("Rate Dollar", 1, 1, null, "stone-column"));        
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Stone (IDR)", 1, 1, null, "stone-column"));        
        headerRow2.Cells.Add(CreateHeaderCell("Harga Jual Stone", 1, 1, null, "stone-column"));
        headerRow2.Cells.Add(CreateHeaderCell("Total Tagihan", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total HPP", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Diskon Biaya Instruksi", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total harus dibayar", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("DPP", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("PPN", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Cash / Tunai", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Kartu Kredit", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Transfer", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Debit", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("QRIS", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Voucher", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total Bayar", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("MDR", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total diterima", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Bank", 1, 1));

        tb.Rows.Add(headerRow2);        
        DateTime Start = Convert.ToDateTime(dari.Text);
        DateTime End = Convert.ToDateTime(sampai.Text);        
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_report_detail_repair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@NomorRPR", SqlDbType.VarChar) { Value = kw.Text });                
                command.Parameters.Add(new SqlParameter("@TglStart", SqlDbType.DateTime) { Value = Start });                
                command.Parameters.Add(new SqlParameter("@TglEnd", SqlDbType.DateTime) { Value = End });                
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Convert.ToInt32(tipelokasi.SelectedValue) });                
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Convert.ToInt32(locationcode.SelectedValue) });                
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = Convert.ToInt32(filter.SelectedValue) });                
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();                
                adapter.Fill(ds);                
                dt = ds.Tables[0];
                
                int no = 0;
                string Parcel = "";
                string nomorRPR = "";                
                string namaLokasi = "";
                string nomorRepair = "";
                string nomorRepair1 = "";
                string tglReparasi = "";
                string tglPembayaran = "";
                string PLUSetting = "";
                string PLUSetting1 = "";
                string PLU = "";
                string InvoiceSO = "";
                string NamaCustomer = "";
                string nomor = "";
                string NamaSales = "";
                string StatusReparasi = "";
                string TotalBiayaJasaReparasi = "";
                string GrossWeightBefore = "";
                string NettWeightAfter = "";
                string GrossWeightAfter = "";
                string SelisihBerat = "";
                string BatasToleransi = "";
                string TotalCharge = "";
                string BiayaMEmas = "";
                string HargaJualEmas = "";
                string Stone = "";
                string TotalButir = "";
                string CaratButir = "";
                string TotalCarat = "";
                string BiayaMStone = "";
                string HargaJualStone = "";   
                string TotalBayarBeforeDisc = "";
                string DiscountNominal = "";
                string TotalPembayaran = "";             
                string DPP = "";
                string PPN = "";
                string Cash = "";
                string KartuKredit = "";
                string Transfer = "";
                string Debit = "";
                string Qris = "";
                string Voucher = "";
                string TotalBayar = "";
                string MDR = "";
                string TotalDiterima = "";
                string Bank = "";
                decimal HARGAMBATU = 0;
                string tiperepair = "";
                string JenisRepair = "";
                string TypeRepair = "";
                string JenisRepair1 = "";
                string LokasiReparasi = "";
                string HargaM = "";
                string HargaFinal = "";
                string RateDollar = "";
                string HargaMStone = "";
                string TotalHPP = "";
                decimal TotalBiayaMRepair = 0;
                decimal TotalBiayaJualRepair = 0;
                decimal TotalBiayaRepair = 0;
                decimal TotalGrossWeightBefore = 0;
                decimal TotalGrossWeightAfter = 0;
                decimal TotalNetWeightAfter = 0;
                decimal TotalBatasToleransi = 0;
                decimal TotalSelisihBerat = 0;
                decimal TotalChargeAll = 0;
                decimal TotalBiayaMEmas = 0;
                decimal TotalHargaJualEmas = 0;
                decimal TotalButirAll = 0;
                decimal TotalCaratAll = 0;
                decimal TotalHargaMStoneUSD = 0;
                decimal TotalHargaMStoneIDR = 0;
                decimal TotalHargaJualStone = 0;
                decimal TotalTagihanAll = 0;
                decimal TotalDiskonBiayaReparasi = 0;
                decimal TotalDPP = 0;
                decimal TotalPPN = 0;
                decimal TotalCASH = 0;
                decimal TotalCC = 0;
                decimal TotalTF = 0;
                decimal TotalDEBIT = 0;
                decimal TotalQRIS = 0;
                decimal TotalVOUCHER = 0;
                decimal TotalBayarAll = 0;
                decimal TotalMDR = 0;
                decimal TotalDiterimaAll = 0;                
                decimal TotalanHPP = 0;



                foreach (DataRow row in dt.Rows)                
                {          
                    if (PLUSetting1 == row["PLUSetting"])
                    {
                        PLUSetting = "";
                    }
                    else
                    {                        
                        PLUSetting = row["PLUSetting"].ToString();
                    }
                    if (Convert.ToDecimal(row["HargaTotalM"]) == 0) 
                    {                        
                            Stone = "";
                            TotalButir = "";
                            CaratButir = "";
                            TotalCarat = "";
                            BiayaMStone = "";
                            HargaMStone = "";
                            HargaJualStone = "";   
                            RateDollar = "";
                    }else{
                        Stone = row["Stone"].ToString();
                        TotalButir = Convert.ToDecimal(row["TotalButir"]).ToString();
                        CaratButir = Convert.ToDecimal(row["CaratButir"]).ToString("N3");
                        TotalCarat = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        BiayaMStone = Convert.ToDecimal(row["BIAYA_M_STONE"]).ToString("N2");
                        HargaMStone = Convert.ToDecimal(row["HARGA_M_STONE"]).ToString("N2");
                        HargaJualStone = Convert.ToDecimal(row["HARGA_TOTAL_STONE"]).ToString("N2");
                        RateDollar = Convert.ToDecimal(row["RATE_DOLLAR"]).ToString("N2");
                        TotalButirAll += Convert.ToDecimal(row["TotalButir"]);
                        TotalCaratAll += Convert.ToDecimal(row["TotalCarat"]);
                        TotalHargaMStoneUSD += Convert.ToDecimal(row["BIAYA_M_STONE"]);
                        TotalHargaMStoneIDR += Convert.ToDecimal(row["HARGA_M_STONE"]);
                        TotalHargaJualStone += Convert.ToDecimal(row["HARGA_TOTAL_STONE"]);
                    }           
                    
                    if (nomorRPR != row["NomorRPR"].ToString())                    
                    {
                        TotalBiayaMRepair += Convert.ToDecimal(row["HargaM"]);
                        TotalBiayaJualRepair += Convert.ToDecimal(row["HargaFinal"]);
                        TotalBiayaRepair += Convert.ToDecimal(row["TotalBiayaJasaReparasi"]);
                        TotalGrossWeightBefore += Convert.ToDecimal(row["GROSS_WEIGHT_BEFORE"]);
                        TotalGrossWeightAfter += Convert.ToDecimal(row["GROSS_WEIGHT_AFTER"]);
                        TotalNetWeightAfter += Convert.ToDecimal(row["NETT_WEIGHT_AFTER"]);
                        TotalBatasToleransi += Convert.ToDecimal(row["BatasToleransi"]);
                        TotalSelisihBerat += Convert.ToDecimal(row["SelisihBerat"]);
                        TotalChargeAll += Convert.ToDecimal(row["SelisihBerat"]) - Convert.ToDecimal(row["BatasToleransi"]);
                        TotalBiayaMEmas += Convert.ToDecimal(row["BIAYA_M_EMAS"]);
                        TotalHargaJualEmas += Convert.ToDecimal(row["HARGA_JUAL_EMAS"]);                        
                        TotalTagihanAll += Convert.ToDecimal(row["TotalBayarBeforeDisc"]);                        
                        TotalDiskonBiayaReparasi += Convert.ToDecimal(row["DiscountNominal"]);
                        TotalDPP += Convert.ToDecimal(row["DPP"]);
                        TotalPPN += Convert.ToDecimal(row["PPN"]);
                        TotalCASH += Convert.ToDecimal(row["Cash"]);
                        TotalCC += Convert.ToDecimal(row["KartuKredit"]);
                        TotalTF += Convert.ToDecimal(row["Transfer"]);
                        TotalDEBIT += Convert.ToDecimal(row["Debit"]);
                        TotalQRIS += Convert.ToDecimal(row["QRIS"]);
                        TotalVOUCHER += Convert.ToDecimal(row["Voucher"]);
                        TotalBayarAll += Convert.ToDecimal(row["TotalBayar"]);
                        TotalMDR += Convert.ToDecimal(row["MDR"]);
                        TotalDiterimaAll += Convert.ToDecimal(row["TotalDiterima"]);
                        TotalanHPP += Convert.ToDecimal(row["TotalHPP"]);
                        no++;
                        nomorRepair = row["NomorRPR"].ToString();
                        nomorRepair1 = row["NomorRPR1"].ToString();
                        tglPembayaran = row["TglBayar"].ToString();
                        tglReparasi = row["TglRepair"].ToString();
                        namaLokasi = row["NamaLokasi"].ToString();
                        PLU = row["PLU"].ToString();
                        InvoiceSO = row["InvoiceSO"].ToString();
                        NamaCustomer = row["CustomerNama"].ToString();                        
                        nomor = no.ToString();                        
                        NamaSales = row["SalesNama"].ToString();
                        StatusReparasi = row["StatusReparasi"].ToString();                        
                        TotalBiayaJasaReparasi = Convert.ToDecimal(row["TotalBiayaJasaReparasi"]).ToString("N2");
                        GrossWeightBefore = Convert.ToDecimal(row["GROSS_WEIGHT_BEFORE"]).ToString("N2");
                        NettWeightAfter = Convert.ToDecimal(row["NETT_WEIGHT_AFTER"]).ToString("N2");
                        GrossWeightAfter = Convert.ToDecimal(row["GROSS_WEIGHT_AFTER"]).ToString("N2");
                        SelisihBerat = Convert.ToDecimal(row["SelisihBerat"]).ToString("N2");
                        BatasToleransi = Convert.ToDecimal(row["BatasToleransi"]).ToString("N2");
                        TotalCharge = (Convert.ToDecimal(row["SelisihBerat"]) - Convert.ToDecimal(row["BatasToleransi"])).ToString("N2");
                        BiayaMEmas = Convert.ToDecimal(row["BIAYA_M_EMAS"]).ToString("N2");
                        HargaJualEmas = Convert.ToDecimal(row["HARGA_JUAL_EMAS"]).ToString("N2");
                        TotalBayarBeforeDisc = Convert.ToDecimal(row["TotalBayarBeforeDisc"]).ToString("N2");
                        DiscountNominal = Convert.ToDecimal(row["DiscountNominal"]).ToString("N2");
                        TotalPembayaran = Convert.ToDecimal(row["TotalBayar"]).ToString("N2");
                        DPP = Convert.ToDecimal(row["DPP"]).ToString("N2");
                        PPN = Convert.ToDecimal(row["PPN"]).ToString("N2");
                        Cash = Convert.ToDecimal(row["Cash"]).ToString("N2");
                        KartuKredit = Convert.ToDecimal(row["KartuKredit"]).ToString("N2");
                        Transfer = Convert.ToDecimal(row["Transfer"]).ToString("N2");
                        Debit = Convert.ToDecimal(row["Debit"]).ToString("N2");
                        Qris = Convert.ToDecimal(row["Qris"]).ToString("N2");
                        Voucher = Convert.ToDecimal(row["Voucher"]).ToString("N2");
                        TotalBayar = Convert.ToDecimal(row["TotalBayar"]).ToString("N2");
                        MDR = Convert.ToDecimal(row["MDR"]).ToString("N2");
                        TotalDiterima = Convert.ToDecimal(row["TotalDiterima"]).ToString("N2");
                        Bank = row["BankIssuer"].ToString();
                        TotalPembayaran = Convert.ToDecimal(row["TotalBayar"]).ToString("N2");                                                                                                                    
                        TypeRepair = row["NamaTipeRepair"].ToString();
                        JenisRepair1 = row["JenisReparasi"].ToString();
                        LokasiReparasi = row["LokasiReparasi"].ToString();
                        HargaM = Convert.ToDecimal(row["HargaM"]).ToString("N2");
                        HargaFinal = Convert.ToDecimal(row["HargaFinal"]).ToString("N2");    
                        TotalHPP =Convert.ToDecimal(row["TotalHPP"]).ToString("N2");
                    }else
                    {                        
                        if (tiperepair == row["NamaTipeRepair"].ToString() && JenisRepair == row["JenisReparasi"].ToString() || Convert.ToDecimal(row["HargaM"]) == 0)
                        {
                            TypeRepair = "";
                            JenisRepair1 = "";
                            LokasiReparasi = "";
                            HargaM = "";
                            HargaFinal = "";
                        }
                        else
                        {
                            TypeRepair = row["NamaTipeRepair"].ToString();
                            JenisRepair1 = row["JenisReparasi"].ToString();
                            LokasiReparasi = row["LokasiReparasi"].ToString();
                            HargaM = Convert.ToDecimal(row["HargaM"]).ToString("N2");
                            HargaFinal = Convert.ToDecimal(row["HargaFinal"]).ToString("N2");  
                            TotalBiayaMRepair += Convert.ToDecimal(row["HargaM"]);
                            TotalBiayaJualRepair += Convert.ToDecimal(row["HargaFinal"]);    
                        }
                        TotalHPP = "";
                        nomor = "";
                        namaLokasi = "";
                        nomorRepair = "";
                        nomorRepair1 = "";
                        tglReparasi = "";
                        tglPembayaran = "";
                        PLU = "";
                        InvoiceSO = "";
                        NamaCustomer = "";                        
                        NamaSales = "";
                        StatusReparasi = "";
                        TotalBiayaJasaReparasi = "";
                        GrossWeightBefore = "";
                        NettWeightAfter = "";
                        GrossWeightAfter = "";
                        SelisihBerat = "";
                        BatasToleransi = "";
                        TotalCharge = "";
                        BiayaMEmas = "";
                        HargaJualEmas = "";
                        TotalBayarBeforeDisc = "";
                        DiscountNominal = "";
                        TotalPembayaran = "";
                        DPP = "";
                        PPN = "";
                        Cash = "";
                        KartuKredit = "";
                        Transfer = "";
                        Debit = "";
                        Qris = "";
                        Voucher = "";
                        TotalBayar = "";
                        MDR = "";
                        TotalDiterima = "";
                        Bank = "";                        
                    }                                   
                    

                    TableRow tr = new TableRow();
                    TableCell td;                    

                    td = new TableCell();                    
                    td.Text = nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = namaLokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = nomorRepair;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = nomorRepair1;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = tglReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = tglPembayaran;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = PLU;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = InvoiceSO;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NamaCustomer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NamaSales;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = StatusReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TypeRepair;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = JenisRepair1;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = LokasiReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = HargaM;
                    td.CssClass = "jasa-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = HargaFinal;
                    td.CssClass = "jasa-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalBiayaJasaReparasi.ToString();
                    td.CssClass = "jasa-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeightBefore;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = NettWeightAfter;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = GrossWeightAfter;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = SelisihBerat;
                    td.CssClass = "gold-column";
                    if (SelisihBerat != "")
                    {
                        if (Convert.ToDecimal(SelisihBerat) > 0)
                        {                        
                            td.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {                        
                            td.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BatasToleransi;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCharge;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BiayaMEmas;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = HargaJualEmas;
                    td.CssClass = "gold-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = PLUSetting;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Stone;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalButir;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = CaratButir;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = BiayaMStone;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = RateDollar;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaMStone;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = HargaJualStone;
                    td.CssClass = "stone-column";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalBayarBeforeDisc;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalHPP;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DiscountNominal;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalPembayaran;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DPP;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = PPN;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Cash;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = KartuKredit;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Transfer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Debit;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qris;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Voucher;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalBayar;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = MDR;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalDiterima;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    nomorRPR = row["NomorRPR"].ToString();
                    Parcel = row["Stone"].ToString();
                    HARGAMBATU = Convert.ToDecimal(row["HargaTotalM"]);
                    tiperepair = row["NamaTipeRepair"].ToString();
                    JenisRepair = row["JenisReparasi"].ToString();
                    PLUSetting1 = row["PLUSetting"].ToString();


                    tb.Rows.Add(tr);
                }                    
                    
                    TableRow tr1 = new TableRow();
                    TableCell td1;

                    td1 = new TableCell();                    
                    td1.Text = "TOTAL";
                    td1.ColumnSpan = 14;
                    td1.Style["text-align"] = "left";
                    td1.Style["font-weight"] = "bold";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaMRepair).ToString("N2");
                    td1.CssClass = "jasa-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaJualRepair).ToString("N2");
                    td1.CssClass = "jasa-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaRepair).ToString("N2");
                    td1.CssClass = "jasa-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalGrossWeightBefore).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalNetWeightAfter).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalGrossWeightAfter).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalSelisihBerat).ToString("N2");
                    td1.CssClass = "gold-column";
                    if (TotalSelisihBerat != 0)
                    {
                        if (Convert.ToDecimal(TotalSelisihBerat) > 0)
                        {                        
                            td1.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {                        
                            td1.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBatasToleransi).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalChargeAll).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaMEmas).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaJualEmas).ToString("N2");
                    td1.CssClass = "gold-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalButirAll).ToString();
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCaratAll).ToString("N3");
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaMStoneUSD).ToString("N2");
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style["border"] = "1px solid black";
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Style["border"] = "1px solid black";
                    td1.Text = Convert.ToDecimal(TotalHargaMStoneIDR).ToString("N2");
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaJualStone).ToString("N2");
                    td1.CssClass = "stone-column";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalTagihanAll).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalanHPP).ToString("N2");                    
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDiskonBiayaReparasi).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBayarAll).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDPP).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalPPN).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCASH).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCC).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalTF).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDEBIT).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalQRIS).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalVOUCHER).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBayarAll).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalMDR).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDiterimaAll).ToString("N2");
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    tr1.Cells.Add(td1);
                    
                    tb.Rows.Add(tr1);
            }
        }

        // =========================
        // HITUNG TOTAL OTOMATIS
        // =========================
        // int columnCount = allData[0].Length;
        // decimal[] totals = new decimal[columnCount];

        // foreach (var row in allData)
        // {
        //     for (int i = 0; i < columnCount; i++)
        //     {
        //         decimal val;

        //         if (decimal.TryParse(
        //             row[i]
        //                 .Replace(".", "")
        //                 .Replace(",", ".")
        //                 .Replace("$", "")
        //                 .Trim(),
        //             out val))
        //         {
        //             totals[i] += val;
        //         }
        //     }
        // }

        // =========================
        // TOTAL ROW
        // =========================
        // TableRow totalRow = new TableRow();
        // totalRow.CssClass = "total-row";

        // // Kolom pertama sampai 10 (merge)
        // TableCell totalCell = new TableCell();
        // totalCell.Text = "Total";
        // totalCell.ColumnSpan = 11;
        // totalCell.Style["text-align"] = "center";
        // totalRow.Cells.Add(totalCell);

        // // Sisanya isi otomatis
        // for (int i = 11; i < columnCount; i++)
        // {
        //     if (totals[i] != 0)
        //         totalRow.Cells.Add(CreateCell(totals[i].ToString("N2")));
        //     else
        //         totalRow.Cells.Add(CreateCell(""));
        // }

        // tb.Rows.Add(totalRow);
    }

    protected void exportTableNew(object sender, EventArgs e)
    {
        string ijo = "#c8f2b8"; // ijo muda
        string kuning = "#fffacd"; // kuning muda
        string ungu = "#e6e6fa"; // ungu muda
        Table table = new Table();

        // =========================
        // HEADER ROW 1
        // =========================       
        TableHeaderRow headerRow1 = new TableHeaderRow(); 
        headerRow1.CssClass = "header";

        headerRow1.Cells.Add(CreateHeaderCell("No.", 2, 1, "50px"));
        headerRow1.Cells.Add(CreateHeaderCell("Store", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. SalesOrder", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. Repair", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal Reparasi", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Tanggal Pembayaran", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("PLU", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("No. Invoice / SPK", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Customer", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Sales", 2, 1, "200px"));
        headerRow1.Cells.Add(CreateHeaderCell("Reparasi", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Type Repair", 2, 1, "150px"));
        headerRow1.Cells.Add(CreateHeaderCell("Jenis Reparasi", 2, 1, "250px"));
        headerRow1.Cells.Add(CreateHeaderCell("Lokasi Reparasi", 2, 1, "100px"));                
        headerRow1.Cells.Add(CreateHeaderCell("Jasa", 1, 3, "650px", null, ijo));        
        headerRow1.Cells.Add(CreateHeaderCell("Gold", 1, 8, "1500px", null, kuning));        
        headerRow1.Cells.Add(CreateHeaderCell("Stone", 1, 9, "1200px", null, ungu));        
        headerRow1.Cells.Add(CreateHeaderCell("Total", 1, 6, "850px"));
        headerRow1.Cells.Add(CreateHeaderCell("Pembayaran", 1, 10, "1500px"));

        table.Rows.Add(headerRow1);

        // =========================
        // HEADER ROW 2
        // =========================
        TableHeaderRow headerRow2 = new TableHeaderRow();
        headerRow2.CssClass = "header";

        headerRow2.Cells.Add(CreateHeaderCell("Biaya Reparasi M", 1, 1, null, null, ijo));        
        headerRow2.Cells.Add(CreateHeaderCell("Biaya Reparasi Jual", 1, 1, null, null, ijo));
        headerRow2.Cells.Add(CreateHeaderCell("Total Biaya", 1, 1, null, null, ijo));
        headerRow2.Cells.Add(CreateHeaderCell("Gross Weight Sebelum", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Nett Weight Sesudah", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Gross Weight Sesudah", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Selisih Berat", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Batas Toleransi", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Total Charge", 1, 1, null, null, kuning));
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Emas", 1, 1, null, null, kuning));        
        headerRow2.Cells.Add(CreateHeaderCell("Harga Jual Emas", 1, 1, null, null, kuning));        
        headerRow2.Cells.Add(CreateHeaderCell("PLU Setting", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Parcel", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Total Butir", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Carat / Butir", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Total Carat", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Stone (USD)", 1, 1, null, null, ungu));        
        headerRow2.Cells.Add(CreateHeaderCell("Rate Dollar", 1, 1, null, null, ungu));        
        headerRow2.Cells.Add(CreateHeaderCell("Biaya M Stone (IDR)", 1, 1, null, null, ungu));        
        headerRow2.Cells.Add(CreateHeaderCell("Harga Jual Stone", 1, 1, null, null, ungu));
        headerRow2.Cells.Add(CreateHeaderCell("Total Tagihan", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total HPP", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Diskon Biaya Instruksi", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total harus dibayar", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("DPP", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("PPN", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Cash / Tunai", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Kartu Kredit", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Transfer", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Debit", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("QRIS", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Voucher", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total Bayar", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("MDR", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Total diterima", 1, 1));
        headerRow2.Cells.Add(CreateHeaderCell("Bank", 1, 1));

        table.Rows.Add(headerRow2);        
        DateTime Start = Convert.ToDateTime(dari.Text);
        DateTime End = Convert.ToDateTime(sampai.Text);        
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_report_detail_repair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@NomorRPR", SqlDbType.VarChar) { Value = kw.Text });                
                command.Parameters.Add(new SqlParameter("@TglStart", SqlDbType.DateTime) { Value = Start });                
                command.Parameters.Add(new SqlParameter("@TglEnd", SqlDbType.DateTime) { Value = End });                
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Convert.ToInt32(tipelokasi.SelectedValue) });                
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Convert.ToInt32(locationcode.SelectedValue) });                
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = Convert.ToInt32(filter.SelectedValue) });                
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();                
                adapter.Fill(ds);                
                dt = ds.Tables[0];
                
                int no = 0;
                
                string Parcel = "";
                string nomorRPR = "";                
                string namaLokasi = "";
                string nomorRepair = "";
                string nomorRepair1 = "";
                string tglReparasi = "";
                string tglPembayaran = "";
                string PLUSetting = "";
                string PLUSetting1 = "";
                string PLU = "";
                string InvoiceSO = "";
                string NamaCustomer = "";
                string nomor = "";
                string NamaSales = "";
                string StatusReparasi = "";
                string TotalBiayaJasaReparasi = "";
                string GrossWeightBefore = "";
                string NettWeightAfter = "";
                string GrossWeightAfter = "";
                string SelisihBerat = "";
                string BatasToleransi = "";
                string TotalCharge = "";
                string BiayaMEmas = "";
                string HargaJualEmas = "";
                string Stone = "";
                string TotalButir = "";
                string CaratButir = "";
                string TotalCarat = "";
                string BiayaMStone = "";
                string HargaJualStone = "";   
                string TotalBayarBeforeDisc = "";
                string DiscountNominal = "";
                string TotalPembayaran = "";             
                string DPP = "";
                string PPN = "";
                string Cash = "";
                string KartuKredit = "";
                string Transfer = "";
                string Debit = "";
                string Qris = "";
                string Voucher = "";
                string TotalBayar = "";
                string MDR = "";
                string TotalDiterima = "";
                string Bank = "";
                decimal HARGAMBATU = 0;
                string tiperepair = "";
                string JenisRepair = "";
                string TypeRepair = "";
                string JenisRepair1 = "";
                string LokasiReparasi = "";
                string HargaM = "";
                string HargaMStone = "";
                string HargaFinal = "";
                string RateDollar = "";
                string TotalHPP = "";
                decimal TotalBiayaMRepair = 0;
                decimal TotalBiayaJualRepair = 0;
                decimal TotalBiayaRepair = 0;
                decimal TotalGrossWeightBefore = 0;
                decimal TotalGrossWeightAfter = 0;
                decimal TotalNetWeightAfter = 0;
                decimal TotalBatasToleransi = 0;
                decimal TotalSelisihBerat = 0;
                decimal TotalChargeAll = 0;
                decimal TotalBiayaMEmas = 0;
                decimal TotalHargaJualEmas = 0;
                decimal TotalButirAll = 0;
                decimal TotalCaratAll = 0;
                decimal TotalHargaMStoneUSD = 0;
                decimal TotalHargaMStoneIDR = 0;
                decimal TotalHargaJualStone = 0;
                decimal TotalTagihanAll = 0;
                decimal TotalDiskonBiayaReparasi = 0;
                decimal TotalDPP = 0;
                decimal TotalPPN = 0;
                decimal TotalCASH = 0;
                decimal TotalCC = 0;
                decimal TotalTF = 0;
                decimal TotalDEBIT = 0;
                decimal TotalQRIS = 0;
                decimal TotalVOUCHER = 0;
                decimal TotalBayarAll = 0;
                decimal TotalMDR = 0;
                decimal TotalDiterimaAll = 0;  
                decimal TotalanHPP = 0;              

                foreach (DataRow row in dt.Rows)                
                {       
                    if (PLUSetting1 == row["PLUSetting"])
                    {
                        PLUSetting = "";
                    }
                    else
                    {                        
                        PLUSetting = row["PLUSetting"].ToString();
                    }

                    if (Convert.ToDecimal(row["HargaTotalM"]) == 0)
                    {                        
                            Stone = "";
                            TotalButir = "";
                            CaratButir = "";
                            TotalCarat = "";
                            BiayaMStone = "";
                            HargaJualStone = "";   
                            HargaMStone = "";
                            RateDollar = "";
                    }else{
                        Stone = row["Stone"].ToString();
                        TotalButir = Convert.ToDecimal(row["TotalButir"]).ToString();
                        CaratButir = Convert.ToDecimal(row["CaratButir"]).ToString("N3");
                        TotalCarat = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        BiayaMStone = Convert.ToDecimal(row["BIAYA_M_STONE"]).ToString();
                        HargaMStone = Convert.ToDecimal(row["HARGA_M_STONE"]).ToString();
                        HargaJualStone = Convert.ToDecimal(row["HARGA_TOTAL_STONE"]).ToString();
                        RateDollar = Convert.ToDecimal(row["RATE_DOLLAR"]).ToString();
                        TotalButirAll += Convert.ToDecimal(row["TotalButir"]);
                        TotalCaratAll += Convert.ToDecimal(row["TotalCarat"]);
                        TotalHargaMStoneUSD += Convert.ToDecimal(row["BIAYA_M_STONE"]);
                        TotalHargaMStoneIDR += Convert.ToDecimal(row["HARGA_M_STONE"]);
                        TotalHargaJualStone += Convert.ToDecimal(row["HARGA_TOTAL_STONE"]);
                    }                  
                    
                    if (nomorRPR != row["NomorRPR"].ToString())                    
                    {
                        TotalBiayaMRepair += Convert.ToDecimal(row["HargaM"]);
                        TotalBiayaJualRepair += Convert.ToDecimal(row["HargaFinal"]);
                        TotalBiayaRepair += Convert.ToDecimal(row["TotalBiayaJasaReparasi"]);
                        TotalGrossWeightBefore += Convert.ToDecimal(row["GROSS_WEIGHT_BEFORE"]);
                        TotalGrossWeightAfter += Convert.ToDecimal(row["GROSS_WEIGHT_AFTER"]);
                        TotalNetWeightAfter += Convert.ToDecimal(row["NETT_WEIGHT_AFTER"]);
                        TotalBatasToleransi += Convert.ToDecimal(row["BatasToleransi"]);
                        TotalSelisihBerat += Convert.ToDecimal(row["SelisihBerat"]);
                        TotalChargeAll += Convert.ToDecimal(row["SelisihBerat"]) - Convert.ToDecimal(row["BatasToleransi"]);
                        TotalBiayaMEmas += Convert.ToDecimal(row["BIAYA_M_EMAS"]);
                        TotalHargaJualEmas += Convert.ToDecimal(row["HARGA_JUAL_EMAS"]);                        
                        TotalTagihanAll += Convert.ToDecimal(row["TotalBayarBeforeDisc"]);
                        TotalDiskonBiayaReparasi += Convert.ToDecimal(row["DiscountNominal"]);
                        TotalDPP += Convert.ToDecimal(row["DPP"]);
                        TotalPPN += Convert.ToDecimal(row["PPN"]);
                        TotalCASH += Convert.ToDecimal(row["Cash"]);
                        TotalCC += Convert.ToDecimal(row["KartuKredit"]);
                        TotalTF += Convert.ToDecimal(row["Transfer"]);
                        TotalDEBIT += Convert.ToDecimal(row["Debit"]);
                        TotalQRIS += Convert.ToDecimal(row["QRIS"]);
                        TotalVOUCHER += Convert.ToDecimal(row["Voucher"]);
                        TotalBayarAll += Convert.ToDecimal(row["TotalBayar"]);
                        TotalMDR += Convert.ToDecimal(row["MDR"]);
                        TotalDiterimaAll += Convert.ToDecimal(row["TotalDiterima"]);
                        TotalanHPP += Convert.ToDecimal(row["TotalHPP"]);
                        no++;
                        nomorRepair = row["NomorRPR"].ToString();
                        nomorRepair1 = row["NomorRPR1"].ToString();
                        tglPembayaran = row["TglBayar"].ToString();
                        tglReparasi = row["TglRepair"].ToString();
                        namaLokasi = row["NamaLokasi"].ToString();
                        namaLokasi = row["NamaLokasi"].ToString();
                        PLU = row["PLU"].ToString();
                        InvoiceSO = row["InvoiceSO"].ToString();
                        NamaCustomer = row["CustomerNama"].ToString();                        
                        nomor = no.ToString();                        
                        NamaSales = row["SalesNama"].ToString();
                        StatusReparasi = row["StatusReparasi"].ToString();                        
                        TotalBiayaJasaReparasi = Convert.ToDecimal(row["TotalBiayaJasaReparasi"]).ToString();
                        GrossWeightBefore = Convert.ToDecimal(row["GROSS_WEIGHT_BEFORE"]).ToString("N2");
                        NettWeightAfter = Convert.ToDecimal(row["NETT_WEIGHT_AFTER"]).ToString("N2");
                        GrossWeightAfter = Convert.ToDecimal(row["GROSS_WEIGHT_AFTER"]).ToString("N2");
                        SelisihBerat = Convert.ToDecimal(row["SelisihBerat"]).ToString("N2");
                        BatasToleransi = Convert.ToDecimal(row["BatasToleransi"]).ToString("N2");
                        TotalCharge = (Convert.ToDecimal(row["SelisihBerat"]) - Convert.ToDecimal(row["BatasToleransi"])).ToString("N2");
                        BiayaMEmas = Convert.ToDecimal(row["BIAYA_M_EMAS"]).ToString();
                        HargaJualEmas = Convert.ToDecimal(row["HARGA_JUAL_EMAS"]).ToString();
                        TotalBayarBeforeDisc = Convert.ToDecimal(row["TotalBayarBeforeDisc"]).ToString();
                        DiscountNominal = Convert.ToDecimal(row["DiscountNominal"]).ToString();
                        TotalPembayaran = Convert.ToDecimal(row["TotalBayar"]).ToString();
                        DPP = Convert.ToDecimal(row["DPP"]).ToString();
                        PPN = Convert.ToDecimal(row["PPN"]).ToString();
                        Cash = Convert.ToDecimal(row["Cash"]).ToString();
                        KartuKredit = Convert.ToDecimal(row["KartuKredit"]).ToString();
                        Transfer = Convert.ToDecimal(row["Transfer"]).ToString();
                        Debit = Convert.ToDecimal(row["Debit"]).ToString();
                        Qris = Convert.ToDecimal(row["Qris"]).ToString();
                        Voucher = Convert.ToDecimal(row["Voucher"]).ToString();
                        TotalBayar = Convert.ToDecimal(row["TotalBayar"]).ToString();
                        MDR = Convert.ToDecimal(row["MDR"]).ToString();
                        TotalDiterima = Convert.ToDecimal(row["TotalDiterima"]).ToString();
                        Bank = row["BankIssuer"].ToString();
                        TotalPembayaran = Convert.ToDecimal(row["TotalBayar"]).ToString();                                                                                            
                        TypeRepair = row["NamaTipeRepair"].ToString();
                        JenisRepair1 = row["JenisReparasi"].ToString();
                        LokasiReparasi = row["LokasiReparasi"].ToString();
                        HargaM = Convert.ToDecimal(row["HargaM"]).ToString();
                        HargaFinal = Convert.ToDecimal(row["HargaFinal"]).ToString();      
                        TotalHPP =Convert.ToDecimal(row["TotalHPP"]).ToString("N2");
                    }else
                    {          
                        if (tiperepair == row["NamaTipeRepair"].ToString() && JenisRepair == row["JenisReparasi"].ToString() || Convert.ToDecimal(row["HargaM"]) == 0)
                        {
                            TypeRepair = "";
                            JenisRepair1 = "";
                            LokasiReparasi = "";
                            HargaM = "";
                            HargaFinal = "";
                        }
                        else
                        {
                            TypeRepair = row["NamaTipeRepair"].ToString();
                            JenisRepair1 = row["JenisReparasi"].ToString();
                            LokasiReparasi = row["LokasiReparasi"].ToString();
                            HargaM = Convert.ToDecimal(row["HargaM"]).ToString();
                            HargaFinal = Convert.ToDecimal(row["HargaFinal"]).ToString();      
                            TotalBiayaMRepair += Convert.ToDecimal(row["HargaM"]);
                            TotalBiayaJualRepair += Convert.ToDecimal(row["HargaFinal"]);
                        }        
                        TotalHPP = "";      
                        nomor = "";
                        namaLokasi = "";
                        nomorRepair = "";
                        PLU = "";
                        InvoiceSO = "";
                        NamaCustomer = "";                        
                        NamaSales = "";
                        StatusReparasi = "";
                        TotalBiayaJasaReparasi = "";
                        GrossWeightBefore = "";
                        NettWeightAfter = "";
                        GrossWeightAfter = "";
                        SelisihBerat = "";
                        BatasToleransi = "";
                        TotalCharge = "";
                        BiayaMEmas = "";
                        HargaJualEmas = "";
                        TotalBayarBeforeDisc = "";
                        DiscountNominal = "";
                        TotalPembayaran = "";
                        DPP = "";
                        PPN = "";
                        Cash = "";
                        KartuKredit = "";
                        Transfer = "";
                        Debit = "";
                        Qris = "";
                        Voucher = "";
                        TotalBayar = "";
                        MDR = "";
                        TotalDiterima = "";
                        Bank = "";    
                        nomorRepair1 = "";
                        tglPembayaran = "";
                        tglReparasi = "";
                    }                                                      
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = nomor;
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = namaLokasi;
                    td.Style["border"] = "1px solid black";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = nomorRepair;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = nomorRepair1;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = tglReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = tglPembayaran;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = PLU;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = InvoiceSO;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = NamaCustomer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = NamaSales;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = StatusReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TypeRepair;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = JenisRepair1;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = LokasiReparasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaM;
                    td.Style["background-color"] = ijo;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaFinal;
                    td.Style["background-color"] = ijo;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalBiayaJasaReparasi.ToString();
                    td.Style["background-color"] = ijo;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = GrossWeightBefore;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = NettWeightAfter;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = GrossWeightAfter;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = SelisihBerat;
                    td.Style["background-color"] = kuning;
                    if (SelisihBerat != "")
                    {
                        if (Convert.ToDecimal(SelisihBerat) > 0)
                        {                        
                            td.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {                        
                            td.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = BatasToleransi;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalCharge;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = BiayaMEmas;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaJualEmas;
                    td.Style["background-color"] = kuning;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = PLUSetting;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Stone;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalButir;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = CaratButir;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalCarat;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = BiayaMStone;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = RateDollar;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaMStone;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = HargaJualStone;
                    td.Style["background-color"] = ungu;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalBayarBeforeDisc;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalHPP;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = DiscountNominal;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalPembayaran;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = DPP;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = PPN;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Cash;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = KartuKredit;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Transfer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Debit;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Qris;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Voucher;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalBayar;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = MDR;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = TotalDiterima;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Style["border"] = "1px solid black";
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    nomorRPR = row["NomorRPR"].ToString();
                    Parcel = row["Stone"].ToString();
                    HARGAMBATU = Convert.ToDecimal(row["HargaTotalM"]);
                    tiperepair = row["NamaTipeRepair"].ToString();
                    JenisRepair = row["JenisReparasi"].ToString();
                    PLUSetting1 = row["PLUSetting"].ToString();


                    table.Rows.Add(tr);
                }
                    TableRow tr1 = new TableRow();
                    TableCell td1;

                    td1 = new TableCell();                    
                    td1.Text = "TOTAL";
                    td1.ColumnSpan = 14;
                    td1.Style["text-align"] = "left";
                    td1.Style["font-weight"] = "bold";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaMRepair).ToString();
                    td1.CssClass = "jasa-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaJualRepair).ToString();
                    td1.CssClass = "jasa-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaRepair).ToString();
                    td1.CssClass = "jasa-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalGrossWeightBefore).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalNetWeightAfter).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalGrossWeightAfter).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalSelisihBerat).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    if (TotalSelisihBerat != 0)
                    {
                        if (Convert.ToDecimal(TotalSelisihBerat) > 0)
                        {                        
                            td1.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {                        
                            td1.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBatasToleransi).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalChargeAll).ToString("N2");
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBiayaMEmas).ToString();
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaJualEmas).ToString();
                    td1.CssClass = "gold-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalButirAll).ToString();
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCaratAll).ToString("N3");
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaMStoneUSD).ToString("N2");
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaMStoneIDR).ToString();
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalHargaJualStone).ToString();
                    td1.CssClass = "stone-column";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalTagihanAll).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalanHPP).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDiskonBiayaReparasi).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBayarAll).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDPP).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalPPN).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCASH).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalCC).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalTF).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDEBIT).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalQRIS).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalVOUCHER).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalBayarAll).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalMDR).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = Convert.ToDecimal(TotalDiterimaAll).ToString();
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);

                    td1 = new TableCell();
                    td1.Text = "";
                    td1.Style["border"] = "1px solid black";
                    tr1.Cells.Add(td1);
                    
                    table.Rows.Add(tr1);
                        
            }
        }

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=SalesDetailReparasi.xls");
        Response.ContentType = "application/vnd.ms-excel";

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        table.RenderControl(hw);

        Response.Write(sw.ToString());
        Response.End();
        
    }
    private TableHeaderCell CreateHeaderCell(string text, int rowSpan, int colSpan, string width = null, string cssClass = null, string Style = null)
    {
        TableHeaderCell cell = new TableHeaderCell();
        cell.Text = text;
        cell.RowSpan = rowSpan;
        cell.ColumnSpan = colSpan;

        if (!string.IsNullOrEmpty(cssClass))
        {
            cell.CssClass = cssClass;
        }

        if (!string.IsNullOrEmpty(Style))
        {
            cell.Style["background-color"] = Style;
        }

        cell.Style["border"] = "1px solid black"; // Tambahkan border untuk setiap header cell
        if (!string.IsNullOrEmpty(width))
            cell.Width = Unit.Parse(width);   // contoh: "120px" atau "10%"

        return cell;
    }

    private TableRow CreateDataRow(params string[] values)
    {
        TableRow row = new TableRow();

        for (int i = 0; i < values.Length; i++)
        {
            string val = values[i];
            TableCell cell = new TableCell();
            cell.Text = val;

            if (i == 0)
            {
                cell.Style["text-align"] = "center";
            }
            else
            {
                decimal dummy;
                if (decimal.TryParse(
                    val.Replace(".", "")
                    .Replace(",", ".")
                    .Replace("$", "")
                    .Trim(),
                    out dummy))
                {
                    cell.Style["text-align"] = "right";
                }
            }

            row.Cells.Add(cell);
        }

        return row;
    }

    private TableCell CreateCell(string text, string width = null)
    {
        TableCell cell = new TableCell();
        cell.Text = text;

        if (!string.IsNullOrEmpty(width))
            cell.Width = Unit.Parse(width);

        decimal dummy;
        if (decimal.TryParse(
            text.Replace(".", "")
                .Replace(",", ".")
                .Replace("$", "")
                .Trim(),
            out dummy))
        {
            cell.CssClass = "text-right";
        }

        return cell;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {            
            filter.SelectedValue = Request.QueryString["filter"];         
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["locationtype"];            
            locationcode.SelectedValue = Request.QueryString["locationcode"];        
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        // href += "&productitem=" + productitem.Text;
        // href += "&productcategory=" + productcategory.Text;
        // href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        // href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&locationType=" + tipelokasi.SelectedValue;
        href += "&locationcode=" + locationcode.SelectedValue;
        // href += "&payment=" + payment.SelectedValue;
        // href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {            
            Ddl.Store1(locationcode,Tipe);
        }
        else
        {
            Ddl.Store1(locationcode);
        }        
    }
}