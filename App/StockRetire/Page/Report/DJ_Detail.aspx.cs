using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ_Detail : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 0;

        System.Globalization.NumberFormatInfo nfi1 = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        System.Globalization.NumberFormatInfo nfi2 = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 3;

        System.Globalization.NumberFormatInfo nfi3 = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 4;

        string kode = kw.Text;

        //SELECT
        var rs = from p in db.StockRetireDJ
                 where p.Draft == false
                 select p;

        if (datepicker.Text != "" && datepicker2.Text != "" && kode == "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }
        else if (datepicker.Text != "" && datepicker2.Text != "" && kode != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1) && (p.StockProductDJ.SupplierNomor == kode || p.StockProductDJ.Nomor == kode));
        }


        int Status = Convert.ToInt32(status.SelectedValue);
        if (Status == 1)//approval
            rs = rs.Where(p => p.Status == 1);
        else if (Status == 2)//pending approval
            rs = rs.Where(p => p.Status == 4 || p.Status == 0);
        else if (Status == 3)//rejected
            rs = rs.Where(p => p.Status == 2);
        else if (Status == 4)// outstanding
            rs = rs.Where(p => p.Status == 3);
        rs = rs.OrderBy(p => p.StockProductDJ.LocWarehouse.Nama).ThenBy(p => p.ID);

        //NO-RESULT
        //zero.Visible = rs.Count() == 0;
        //form_area.Visible = !zero.Visible;

        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        //int prev = 0;
        int index = 1;
        //int qty;

        decimal ST_BBruto = 0m;
        decimal ST_BBongkar = 0m;
        decimal ST_SusutBongkar = 0m;
        decimal ST_SusutLebur = 0m;
        decimal ST_Bakhir = 0m;
        decimal ST_DAwalSK = 0;
        decimal ST_SetorStone = 0;
        decimal ST_SetorPecah = 0;
        decimal ST_SelisihButir = 0;
        decimal ST_CaratAwalSK = 0m;
        decimal ST_CaratTerima = 0m;
        decimal ST_CaratRusak = 0m;
        decimal ST_SelisihCarat = 0m;
        decimal ST_Bnetto = 0m;
        decimal ST_hm_tot_stone = 0m;
        decimal ST_hm_emas = 0m;
        decimal ST_hm_ongkos = 0m;
        decimal ST_hm = 0m;
        decimal ST_bpawal = 0m;
        decimal ST_cpawal = 0m;
        decimal ST_hm_tot_per_stone = 0m;
        decimal ST_bpterima = 0m;
        decimal ST_cpterima = 0m;
        decimal ST_bprusak = 0m;
        decimal ST_cprusak = 0m;
        decimal ST_bpselisih = 0m;
        decimal ST_cpselisih = 0m;

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;


            decimal BBruto = 0m;
            decimal BBongkar = 0m;
            decimal SusutBongkar = 0m;
            decimal SusutLebur = 0m;
            decimal Bakhir = 0m;
            decimal DAwalSK = 0;
            decimal SetorStone = 0;
            decimal SetorPecah = 0;
            decimal SelisihButir = 0;
            decimal CaratAwalSK = 0m;
            decimal CaratTerima = 0m;
            decimal CaratRusak = 0m;
            decimal SelisihCarat = 0m;
            decimal BBnetto = 0m;
            decimal bpawal = 0m;
            decimal cpawal = 0m;
            decimal bpterima = 0m;
            decimal cpterima = 0m;
            decimal bprusak = 0m;
            decimal cprusak = 0m;
            decimal bpselisih = 0m;
            decimal cpselisih = 0m;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nomor_Dokumen(r.Nomor);
            tr.Cells.Add(td);

            td = new TableCell();
            string sta = "";
            if (r.Status == 0 || r.Status == 4)
            {
                sta = "Pending Approval";
            }
            else if (r.Status == 2)
            {
                sta = "Rejected";
            }
            else if (r.Status == 1)
            {
                sta = "Approved";
            }
            else if (r.Status == 3)
            {
                sta = "Outstanding";
            }
            td.Text = sta;
            tr.Cells.Add(td);

            //plu
            td = new TableCell();
            td.Text = TD_Nomor(r.StockProductDJ);
            tr.Cells.Add(td);

            //kode barang
            td = new TableCell();
            td.Text = r.StockProductDJ.SupplierNomor;
            tr.Cells.Add(td);

            //product category
            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
            tr.Cells.Add(td);
            //item
            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            //qty
            td = new TableCell();
            td.Text = "1";
            tr.Cells.Add(td);

            //berat bruto awal
            td = new TableCell();
            if (r.NettoWeight != null && r.NettoWeight != 0)
            {
                td.Text = r.NettoWeight.Value.ToString("N2");
                BBruto = r.NettoWeight.Value;
            }
            else
            {
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight.ToString("N2");
                BBruto = r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight;
            }
            //BBruto = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);

            //berat netto awal
            td = new TableCell();
            if (r.NettoWeight != null && r.NettoWeight != 0)
            {
                td.Text = r.NettoWeight.Value.ToString("N2");
                BBnetto = r.NettoWeight.Value;
            }
            else
            {
                td.Text = r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                BBnetto = r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
            }
            //BBnetto = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);

            decimal TotalButirSK = 0m;
            decimal TotalButirSetor = 0m;
            decimal TotalButirPecah = 0m;
            decimal TotalButirSelisih = 0m;
            decimal TotalCaratSK = 0m;
            decimal TotalCaratSetor = 0m;
            decimal TotalCaratRusak = 0m;
            decimal TotalCaratSelisih = 0m;

            TotalButirSK += r.StockRetireDJ_Stone1A.Sum(p => p.TotalButir);
            TotalButirSK += r.StockRetireDJ_Stone1B.Sum(p => p.TotalButir);
            TotalButirSK += r.StockRetireDJ_Stone2.Sum(p => p.TotalButir);
            TotalButirSK += r.StockRetireDJ_Stone3.Sum(p => p.TotalButir);
            TotalButirSK += r.StockRetireDJ_Stone4.Sum(p => p.TotalButir);
            TotalButirSK += r.StockRetireDJ_Stone5.Sum(p => p.TotalButir);

            TotalButirSetor += r.StockRetireDJ_Stone1A.Sum(p => p.TotalTerima);
            TotalButirSetor += r.StockRetireDJ_Stone1B.Sum(p => p.TotalTerima);
            TotalButirSetor += r.StockRetireDJ_Stone2.Sum(p => p.TotalTerima);
            TotalButirSetor += r.StockRetireDJ_Stone3.Sum(p => p.TotalTerima);
            TotalButirSetor += r.StockRetireDJ_Stone4.Sum(p => p.TotalTerima);
            TotalButirSetor += r.StockRetireDJ_Stone5.Sum(p => p.TotalTerima);

            TotalButirPecah += r.StockRetireDJ_Stone1A.Sum(p => p.TotalRusak);
            TotalButirPecah += r.StockRetireDJ_Stone1B.Sum(p => p.TotalRusak);
            TotalButirPecah += r.StockRetireDJ_Stone2.Sum(p => p.TotalRusak);
            TotalButirPecah += r.StockRetireDJ_Stone3.Sum(p => p.TotalRusak);
            TotalButirPecah += r.StockRetireDJ_Stone4.Sum(p => p.TotalRusak);
            TotalButirPecah += r.StockRetireDJ_Stone5.Sum(p => p.TotalRusak);

            TotalButirSelisih += r.StockRetireDJ_Stone1A.Sum(p => p.TotalSelisih);
            TotalButirSelisih += r.StockRetireDJ_Stone1B.Sum(p => p.TotalSelisih);
            TotalButirSelisih += r.StockRetireDJ_Stone2.Sum(p => p.TotalSelisih);
            TotalButirSelisih += r.StockRetireDJ_Stone3.Sum(p => p.TotalSelisih);
            TotalButirSelisih += r.StockRetireDJ_Stone4.Sum(p => p.TotalSelisih);
            TotalButirSelisih += r.StockRetireDJ_Stone5.Sum(p => p.TotalSelisih);

            TotalCaratSK = Convert.ToDecimal((from p in db.StockProductDJ_CharProduct where p.ID == r.IDProduct select p.StoneCarat).SingleOrDefault());

            TotalCaratSetor += r.StockRetireDJ_Stone1A.Sum(p => p.CaratTerima);
            TotalCaratSetor += r.StockRetireDJ_Stone1B.Sum(p => p.CaratTerima);
            TotalCaratSetor += r.StockRetireDJ_Stone2.Sum(p => p.CaratTerima);
            TotalCaratSetor += r.StockRetireDJ_Stone3.Sum(p => p.CaratTerima);
            TotalCaratSetor += r.StockRetireDJ_Stone4.Sum(p => p.CaratTerima);
            TotalCaratSetor += r.StockRetireDJ_Stone5.Sum(p => p.CaratTerima);

            TotalCaratRusak += r.StockRetireDJ_Stone1A.Sum(p => p.CaratRusak);
            TotalCaratRusak += r.StockRetireDJ_Stone1B.Sum(p => p.CaratRusak);
            TotalCaratRusak += r.StockRetireDJ_Stone2.Sum(p => p.CaratRusak);
            TotalCaratRusak += r.StockRetireDJ_Stone3.Sum(p => p.CaratRusak);
            TotalCaratRusak += r.StockRetireDJ_Stone4.Sum(p => p.CaratRusak);
            TotalCaratRusak += r.StockRetireDJ_Stone5.Sum(p => p.CaratRusak);

            TotalCaratSelisih += r.StockRetireDJ_Stone1A.Sum(p => p.CaratSelisih);
            TotalCaratSelisih += r.StockRetireDJ_Stone1B.Sum(p => p.CaratSelisih);
            TotalCaratSelisih += r.StockRetireDJ_Stone2.Sum(p => p.CaratSelisih);
            TotalCaratSelisih += r.StockRetireDJ_Stone3.Sum(p => p.CaratSelisih);
            TotalCaratSelisih += r.StockRetireDJ_Stone4.Sum(p => p.CaratSelisih);
            TotalCaratSelisih += r.StockRetireDJ_Stone5.Sum(p => p.CaratSelisih);

            //carat awal
            td = new TableCell();
            td.Text = TotalCaratSK.ToString("N3");
            CaratAwalSK = TotalCaratSK;
            tr.Cells.Add(td);

            //berat setelah bongkar
            td = new TableCell();
            if (r.NettoSetelahBongkar != null)
            {
                td.Text = r.NettoSetelahBongkar.Value.ToString("N2");
                BBongkar = r.NettoSetelahBongkar.Value;
            }
            else
            {
                td.Text = "0";
                BBongkar = 0;
            }
            //BBongkar = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);
            //susut bongkar
            td = new TableCell();
            if (r.SusutBongkar != null)
            {
                td.Text = r.SusutBongkar.Value.ToString("N2");
                SusutBongkar = r.SusutBongkar.Value;
            }
            else
            {
                td.Text = "0";
                SusutBongkar = 0;
            }
            //SusutBongkar = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);
            //susut lebur
            td = new TableCell();
            if (r.SusutLebur != null)
            {
                td.Text = r.SusutLebur.Value.ToString("N2");
                SusutLebur = r.SusutLebur.Value;
            }
            else
            {
                td.Text = "0";
                SusutLebur = 0;
            }
            //SusutLebur = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);
            //berat akhir
            td = new TableCell();
            if (r.BeratAkhir != null)
            {
                td.Text = r.BeratAkhir.Value.ToString("N2");
                Bakhir = r.BeratAkhir.Value;
            }
            else
            {
                td.Text = "0";
                Bakhir = 0;
            }
            //Bakhir = Convert.ToDecimal(td.Text);
            tr.Cells.Add(td);

            //butir awal
            td = new TableCell();
            if (TotalButirSK == 0)
            {
                td.Text = (Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
                DAwalSK = Convert.ToDecimal(td.Text);
            }
            else
            {
                td.Text = TotalButirSK.ToString("N0");
                DAwalSK = Convert.ToDecimal(td.Text);
            }
            tr.Cells.Add(td);

            //butir setor
            td = new TableCell();
            td.Text = TotalButirSetor.ToString("N0");
            SetorStone = TotalButirSetor;
            tr.Cells.Add(td);

            //butir setor pecah
            td = new TableCell();
            td.Text = TotalButirPecah.ToString("N0");
            SetorPecah = TotalButirPecah;
            tr.Cells.Add(td);

            //selisih butir
            td = new TableCell();
            td.Text = TotalButirSelisih.ToString("N0");
            SelisihButir = TotalButirSelisih;
            tr.Cells.Add(td);

            //carat terima
            td = new TableCell();
            td.Text = TotalCaratSetor.ToString("N3");
            CaratTerima = TotalCaratSetor;
            tr.Cells.Add(td);

            //carat rusak
            td = new TableCell();
            td.Text = TotalCaratRusak.ToString("N3");
            CaratRusak = TotalCaratRusak;
            tr.Cells.Add(td);

            //selisih carat
            td = new TableCell();
            td.Text = TotalCaratSelisih.ToString("N3");
            SelisihCarat = TotalCaratSelisih;
            tr.Cells.Add(td);

            //keterangan
            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            //alasan
            td = new TableCell();
            td.Text = r.Alasan;
            tr.Cells.Add(td);

            //harga m batu
            td = new TableCell();
            decimal hm_tot_stone = r.StockProductDJ.StockProductDJ_CostingProduct.Total - r.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct;
            td.Text = hm_tot_stone.ToString("N2");
            tr.Cells.Add(td);

            //harga m emas
            td = new TableCell();
            decimal hm_emas = r.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct;
            td.Text = hm_emas.ToString("N2");
            tr.Cells.Add(td);

            //harga m ongkos
            td = new TableCell();
            decimal hm_ongkos = r.StockProductDJ.StockProductDJ_Costing.TotalBiaya;
            td.Text = hm_ongkos.ToString("N2"); ;
            tr.Cells.Add(td);

            //harga m
            td = new TableCell();
            decimal hm = r.StockProductDJ.StockProductDJ_Costing.TotalBiaya + r.StockProductDJ.StockProductDJ_CostingProduct.Total;
            td.Text = hm.ToString("N2");
            tr.Cells.Add(td);

            string namaparcel = "";
            decimal butirterima = 0m;
            decimal caratterima = 0m;
            decimal butirawal = 0m;
            decimal caratawal = 0m;
            decimal butirrusak = 0m;
            decimal caratrusak = 0m;
            decimal butirselisih = 0m;
            decimal caratselisih = 0m;
            decimal hargam_per_stone = 0m;

            var stone = (from p1 in db.StockRetireDJ_Stone1A
                         join p2 in db.StockProductDJ_Stone1A on p1.IDStone equals p2.ID
                         join p3 in db.Stone1A on p2.IDStone equals p3.ID
                         where p1.IDForm == r.ID
                         select new
                         {
                             namaparcel = p3.Nama,
                             butirterima = p1.TotalTerima,
                             caratterima = p1.CaratTerima,
                             butirawal = p1.TotalButir,
                             caratawal = p1.CaratButir,
                             butirrusak = p1.TotalRusak,
                             caratrusak = p1.CaratRusak,
                             butirselisih = p1.TotalSelisih,
                             caratselisih = p1.CaratSelisih,
                             nogia = "",
                             idstockproddj_stone = p2.ID,
                             tipe = "1A",
                             hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                         }).Concat(
                            from p1 in db.StockRetireDJ_Stone1B
                            join p2 in db.StockProductDJ_Stone1B on p1.IDStone equals p2.ID
                            join p3 in db.Stone1B on p2.IDStone equals p3.ID
                            where p1.IDForm == r.ID
                            select new
                            {
                                namaparcel = p3.Nama,
                                butirterima = p1.TotalTerima,
                                caratterima = p1.CaratTerima,
                                butirawal = p1.TotalButir,
                                caratawal = p1.CaratButir,
                                butirrusak = p1.TotalRusak,
                                caratrusak = p1.CaratRusak,
                                butirselisih = p1.TotalSelisih,
                                caratselisih = p1.CaratSelisih,
                                nogia = p2.GIA,
                                idstockproddj_stone = p2.ID,
                                tipe = "1B",
                                hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                            }).Concat(
                            from p1 in db.StockRetireDJ_Stone2
                            join p2 in db.StockProductDJ_Stone2 on p1.IDStone equals p2.ID
                            join p3 in db.Stone2 on p2.IDStone equals p3.ID
                            where p1.IDForm == r.ID
                            select new
                            {
                                namaparcel = p3.Nama,
                                butirterima = p1.TotalTerima,
                                caratterima = p1.CaratTerima,
                                butirawal = p1.TotalButir,
                                caratawal = p1.CaratButir,
                                butirrusak = p1.TotalRusak,
                                caratrusak = p1.CaratRusak,
                                butirselisih = p1.TotalSelisih,
                                caratselisih = p1.CaratSelisih,
                                nogia = p2.GIA,
                                idstockproddj_stone = p2.ID,
                                tipe = "2",
                                hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                            }).Concat(
                            from p1 in db.StockRetireDJ_Stone3
                            join p2 in db.StockProductDJ_Stone3 on p1.IDStone equals p2.ID
                            join p3 in db.Stone3 on p2.IDStone equals p3.ID
                            where p1.IDForm == r.ID
                            select new
                            {
                                namaparcel = p3.Nama,
                                butirterima = p1.TotalTerima,
                                caratterima = p1.CaratTerima,
                                butirawal = p1.TotalButir,
                                caratawal = p1.CaratButir,
                                butirrusak = p1.TotalRusak,
                                caratrusak = p1.CaratRusak,
                                butirselisih = p1.TotalSelisih,
                                caratselisih = p1.CaratSelisih,
                                nogia = p2.GIA,
                                idstockproddj_stone = p2.ID,
                                tipe = "3",
                                hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                            }).Concat(
                            from p1 in db.StockRetireDJ_Stone4
                            join p2 in db.StockProductDJ_Stone4 on p1.IDStone equals p2.ID
                            join p3 in db.Stone4 on p2.IDStone equals p3.ID
                            where p1.IDForm == r.ID
                            select new
                            {
                                namaparcel = p3.Nama,
                                butirterima = p1.TotalTerima,
                                caratterima = p1.CaratTerima,
                                butirawal = p1.TotalButir,
                                caratawal = p1.CaratButir,
                                butirrusak = p1.TotalRusak,
                                caratrusak = p1.CaratRusak,
                                butirselisih = p1.TotalSelisih,
                                caratselisih = p1.CaratSelisih,
                                nogia = p2.GIA,
                                idstockproddj_stone = p2.ID,
                                tipe = "4",
                                hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                            }).Concat(
                            from p1 in db.StockRetireDJ_Stone5
                            join p2 in db.StockProductDJ_Stone5 on p1.IDStone equals p2.ID
                            join p3 in db.Stone5 on p2.IDStone equals p3.ID
                            where p1.IDForm == r.ID
                            select new
                            {
                                namaparcel = p3.Nama,
                                butirterima = p1.TotalTerima,
                                caratterima = p1.CaratTerima,
                                butirawal = p1.TotalButir,
                                caratawal = p1.CaratButir,
                                butirrusak = p1.TotalRusak,
                                caratrusak = p1.CaratRusak,
                                butirselisih = p1.TotalSelisih,
                                caratselisih = p1.CaratSelisih,
                                nogia = p2.GIA,
                                idstockproddj_stone = p2.ID,
                                tipe = "5",
                                hargambatu = Convert.ToDecimal(p2.HargaTotalM)
                            });

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
                        td.ColumnSpan = 28;
                        tr.Cells.Add(td);
                    }

                    //nama parcel
                    td = new TableCell();
                    td.Text = rstone.namaparcel;
                    tr.Cells.Add(td);

                    //butir awal parcel
                    td = new TableCell();
                    td.Text = rstone.butirawal.ToString("N0");
                    bpawal = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // carat awal parcel
                    td = new TableCell();
                    td.Text = rstone.caratawal.ToString("N3");
                    cpawal = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // harga m per parcel
                    td = new TableCell();
                    td.Text = rstone.hargambatu.ToString("N2");
                    hargam_per_stone = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    //butir terima parcel
                    td = new TableCell();
                    td.Text = rstone.butirterima.ToString("N0");
                    bpterima = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // carat terima parcel
                    td = new TableCell();
                    td.Text = rstone.caratterima.ToString("N3");
                    cpterima = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    //butir rusak parcel
                    td = new TableCell();
                    td.Text = rstone.butirrusak.ToString("N0");
                    bprusak = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // carat rusak parcel
                    td = new TableCell();
                    td.Text = rstone.caratrusak.ToString("N3");
                    cprusak = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    //butir selisih parcel
                    td = new TableCell();
                    td.Text = rstone.butirselisih.ToString("N0");
                    bpselisih = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // carat selisih parcel
                    td = new TableCell();
                    td.Text = rstone.caratselisih.ToString("N3");
                    cpselisih = Convert.ToDecimal(td.Text);
                    tr.Cells.Add(td);

                    // no gia
                    td = new TableCell();
                    td.Text = (rstone.nogia != null) ? rstone.nogia.ToString() : "";
                    tr.Cells.Add(td);
                    tb.Rows.Add(tr);
                    counter++;

                    ST_bpawal += bpawal;
                    ST_bprusak += bprusak;
                    ST_bpselisih += bpselisih;
                    ST_bpterima += bpterima;

                    ST_cpawal += cpawal;
                    ST_cprusak += cprusak;
                    ST_cpselisih += cpselisih;
                    ST_cpterima += cpterima;
                    ST_hm_tot_per_stone += hargam_per_stone;
                }
            }
            else
            {
                var stone1 = (from p in db.StockProductDJ_Stone1A
                              join p1 in db.Stone1A on p.IDStone equals p1.ID
                              where p.IDForm == r.IDProduct
                              select new
                              {
                                  namaparcel = p1.Nama,
                                  butirawal = p.TotalButir,
                                  caratawal = p.TotalCarat,
                                  nogia = "",
                                  tipe = "1A",
                                  idstockproddj_stone = p.ID,
                                  hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                              }).Concat(
                               from p in db.StockProductDJ_Stone1B
                               join p1 in db.Stone1B on p.IDStone equals p1.ID
                               where p.IDForm == r.IDProduct
                               select new
                               {
                                   namaparcel = p1.Nama,
                                   butirawal = p.TotalButir,
                                   caratawal = p.TotalCarat,
                                   nogia = p.GIA,
                                   tipe = "1B",
                                   idstockproddj_stone = p.ID,
                                   hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                               }).Concat(
                                from p in db.StockProductDJ_Stone2
                                join p1 in db.Stone2 on p.IDStone equals p1.ID
                                where p.IDForm == r.IDProduct
                                select new
                                {
                                    namaparcel = p1.Nama,
                                    butirawal = p.TotalButir,
                                    caratawal = p.TotalCarat,
                                    nogia = p.GIA,
                                    tipe = "2",
                                    idstockproddj_stone = p.ID,
                                    hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                                }).Concat(
                                from p in db.StockProductDJ_Stone3
                                join p1 in db.Stone3 on p.IDStone equals p1.ID
                                where p.IDForm == r.IDProduct
                                select new
                                {
                                    namaparcel = p1.Nama,
                                    butirawal = p.TotalButir,
                                    caratawal = p.TotalCarat,
                                    nogia = p.GIA,
                                    tipe = "3",
                                    idstockproddj_stone = p.ID,
                                    hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                                }).Concat(
                                from p in db.StockProductDJ_Stone4
                                join p1 in db.Stone4 on p.IDStone equals p1.ID
                                where p.IDForm == r.IDProduct
                                select new
                                {
                                    namaparcel = p1.Nama,
                                    butirawal = p.TotalButir,
                                    caratawal = p.TotalCarat,
                                    nogia = p.GIA,
                                    tipe = "4",
                                    idstockproddj_stone = p.ID,
                                    hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                                }).Concat(
                                from p in db.StockProductDJ_Stone5
                                join p1 in db.Stone5 on p.IDStone equals p1.ID
                                where p.IDForm == r.IDProduct
                                select new
                                {
                                    namaparcel = p1.Nama,
                                    butirawal = p.TotalButir,
                                    caratawal = p.TotalCarat,
                                    nogia = p.GIA,
                                    tipe = "5",
                                    idstockproddj_stone = p.ID,
                                    hargambatu = Convert.ToDecimal(p.HargaTotalM ?? 0m)
                                });
                int ct = 1;
                foreach (var rstone in stone1)
                {
                    if (ct > 1)
                    {
                        tr = new TableRow();

                        td = new TableCell();
                        td.Text = "";
                        td.ColumnSpan = 28;
                        tr.Cells.Add(td);
                    }

                    //nama parcel
                    td = new TableCell();
                    td.Text = rstone.namaparcel;
                    tr.Cells.Add(td);

                    //butir awal parcel
                    td = new TableCell();
                    td.Text = rstone.butirawal.ToString("N0");
                    bpawal = rstone.butirawal;
                    tr.Cells.Add(td);

                    //carat awal parcel
                    td = new TableCell();
                    td.Text = rstone.caratawal.ToString("N3");
                    cpawal = rstone.caratawal;
                    tr.Cells.Add(td);

                    // harga m per parcel
                    td = new TableCell();
                    td.Text = rstone.hargambatu.ToString("N2");
                    hargam_per_stone = rstone.hargambatu;
                    tr.Cells.Add(td);

                    //space
                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 6;
                    tr.Cells.Add(td);

                    // no gia
                    td = new TableCell();
                    td.Text = (rstone.nogia != null) ? rstone.nogia.ToString() : "";
                    tr.Cells.Add(td);
                    tb.Rows.Add(tr);
                    ct++;

                    ST_bpawal += bpawal;
                    ST_cpawal += cpawal;
                    ST_hm_tot_per_stone += hargam_per_stone;
                }
            }

            tb.Rows.Add(tr);


            var query = "SELECT p3.Nama [parcel], p2.Nomor [plu], p2.TotalCarat [totalcaratawal], p1.Carat [totalcarat], 1 [totalbutir], p2.HargaTotalM [hargam], p1.GIA [GIA], p4.SupplierNomor [supplier] " +
            "FROM StockRetireDJLD p1 " + 
            "JOIN StockProductLD_Stone1B p2 ON p1.IDProduct = p2.ID " +
            "JOIN Stone1B p3 ON p2.IDStone = p3.ID " +
            "JOIN StockProductLD p4 ON p2.IDForm = p4.ID WHERE p1.IDForm = " + r.ID;

            DataTable dt = Db.Rs(query, Db.Cnn_DB);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow x in dt.Rows)
                {
                    tr = new TableRow();
                    index++;

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
                    td.Text = x["plu"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = x["supplier"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "Loose Diamond";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "Loose Diamond";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(x["totalcarat"]) * (decimal)0.2).ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(x["totalcarat"]) * (decimal)0.2).ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = x["totalcarat"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal("0").ToString("N3", nfi2);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal("0").ToString("N3", nfi2);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (Convert.ToDecimal(x["totalcaratawal"]) - Convert.ToDecimal(x["totalcarat"])).ToString("N3", nfi2);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Keterangan.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Alasan.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(x["hargam"]).ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = 0.ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = 0.ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(x["hargam"]).ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = x["parcel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(x["totalcarat"]).ToString("N3", nfi2);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(x["hargam"]).ToString("N2", nfi1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 6;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = x["GIA"].ToString();
                    tr.Cells.Add(td);


                    ST_BBruto += Convert.ToDecimal(Convert.ToDecimal(x["totalcarat"]) * (decimal)0.2);
                    ST_Bnetto += Convert.ToDecimal(Convert.ToDecimal(x["totalcarat"]) * (decimal)0.2);
                    ST_CaratAwalSK += Convert.ToDecimal(Convert.ToDecimal(x["totalcaratawal"]) * (decimal)0.2);
                    ST_DAwalSK += 1;
                    ST_hm_tot_stone += Convert.ToDecimal(x["hargam"]);
                    ST_hm += Convert.ToDecimal(x["hargam"]);
                    tb.Rows.Add(tr);
                }
            }

            ST_BBruto += BBruto;
            ST_BBongkar += BBongkar;
            ST_SusutBongkar += SusutBongkar;
            ST_SusutLebur += SusutLebur;
            ST_Bakhir += Bakhir;
            ST_DAwalSK += DAwalSK;
            ST_SetorStone += SetorStone;
            ST_SetorPecah += SetorPecah;
            ST_SelisihButir += SelisihButir;
            ST_CaratAwalSK += CaratAwalSK;
            ST_CaratTerima += CaratTerima;
            ST_CaratRusak += CaratRusak;
            ST_SelisihCarat += SelisihCarat;
            ST_Bnetto += BBnetto;
            ST_hm_tot_stone += hm_tot_stone;
            ST_hm_emas += hm_emas;
            ST_hm_ongkos += hm_ongkos;
            ST_hm += hm;
        }

        TableRow tr1 = new TableRow();
        TableCell td1;


        td1 = new TableCell();
        td1.ColumnSpan = 7;
        td1.Text = "<b>Grand Total : </b> ";
        tr1.Cells.Add(td1);

        // total qty  - 1
        td1 = new TableCell();
        td1.Text = "<b>" + (index - 1) + "</b> ";
        tr1.Cells.Add(td1);

        //total bruto awal - 2
        td1 = new TableCell();
        td1.Text = "<b>" + ST_BBruto.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total netto awal - 3
        td1 = new TableCell();
        td1.Text = "<b>" + ST_Bnetto.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total carat awal - 4
        td1 = new TableCell();
        td1.Text = "<b>" + ST_CaratAwalSK.ToString("N3") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //berat setelah bongkar - 5
        td1 = new TableCell();
        td1.Text = "<b>" + ST_BBongkar.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total susut bongkar - 6
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SusutBongkar.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total susut lebur - 7
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SusutLebur.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total berat akhir - 8
        td1 = new TableCell();
        td1.Text = "<b>" + ST_Bakhir.ToString("N2") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total butir awal - 9
        td1 = new TableCell();
        td1.Text = "<b>" + ST_DAwalSK.ToString("N0") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total butir setor - 10
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SetorStone.ToString("N0") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total butir setor pecah - 11
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SetorPecah.ToString("N0") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total selisih butir - 12
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SelisihButir.ToString("N0") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total carat terima - 13
        td1 = new TableCell();
        td1.Text = "<b>" + ST_CaratTerima.ToString("N3") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total carat rusak - 14
        td1 = new TableCell();
        td1.Text = "<b>" + ST_CaratRusak.ToString("N3") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        //total selisih carat - 15
        td1 = new TableCell();
        td1.Text = "<b>" + ST_SelisihCarat.ToString("N3") + "</b>";
        td1.CssClass = "text-center";
        tr1.Cells.Add(td1);

        td1 = new TableCell();
        td1.ColumnSpan = 2;
        td1.Text = "";
        tr1.Cells.Add(td1);

        // total hm stone 
        td1 = new TableCell();
        td1.Text = "<b>" + ST_hm_tot_stone.ToString("N2") + "</b>";
        tr1.Cells.Add(td1);

        // total hm emas
        td1 = new TableCell();
        td1.Text = "<b>" + ST_hm_emas.ToString("N2") + "</b>";
        tr1.Cells.Add(td1);

        // total hm ongkos
        td1 = new TableCell();
        td1.Text = "<b>" + ST_hm_ongkos.ToString("N2") + "</b>";
        tr1.Cells.Add(td1);

        // total hm 
        td1 = new TableCell();
        td1.Text = "<b>" + ST_hm.ToString("N2") + "</b>";
        tr1.Cells.Add(td1);

        //space
        td1 = new TableCell();
        td1.Text = "";
        tr1.Cells.Add(td1);

        //butir awal parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_bpawal.ToString("N0") + "</b>";
        tr1.Cells.Add(td1);

        //carat awal parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_cpawal.ToString("N3") + "</b>";
        tr1.Cells.Add(td1);

        //harga m parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_hm_tot_per_stone.ToString("N2") + "</b>";
        tr1.Cells.Add(td1);

        //butir terima parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_bpterima.ToString("N0") + "</b>";
        tr1.Cells.Add(td1);

        //carat terima parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_cpterima.ToString("N3") + "</b>"; ;
        tr1.Cells.Add(td1);

        //butir rusak parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_bprusak.ToString("N0") + "</b>";
        tr1.Cells.Add(td1);

        //carat rusak parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_cprusak.ToString("N3") + "</b>";
        tr1.Cells.Add(td1);

        //butir selisih parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_bpselisih.ToString("N0") + "</b>";
        tr1.Cells.Add(td1);

        //carat selisih parcel
        td1 = new TableCell();
        td1.Text = "<b>" + ST_cpselisih.ToString("N3") + "</b>";
        tr1.Cells.Add(td1);


        //qty = index;
        tb.Rows.Add(tr1);
    }
    protected void Sub_Header(StockProductDJ r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(StockRetireDJ r)
    {
        string x = string.Empty;


        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Form.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Nomor_Dokumen(string Nomor)
    {
        string x = string.Empty;

        x += Nomor;

        return x;
    }
    protected string TD_Nomor(StockProductDJ r)
    {
        string x = string.Empty;

        x += r.Nomor;

        return x;
    }
    protected string TD_Operator(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
    protected string TD_Data(StockProductDJ r)
    {
        string x = string.Empty;

        if (r.ProtoNomor != "" && r.ProtoNomor != null)
            x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

        if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
            x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

        if (r.SupplierNomor != "" && r.SupplierNomor != null)
            x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

        return x;
    }
    protected string TD_Product01(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Image(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {

            kw.Text = Request.QueryString["kw"];
            status.SelectedValue = Request.QueryString["status"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_Detail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}