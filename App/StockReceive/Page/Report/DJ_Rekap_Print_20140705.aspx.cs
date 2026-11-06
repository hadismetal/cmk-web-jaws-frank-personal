using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Generate

            fillTable(sender, e);
            fillHeader();
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var dj = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var pg = from p in db.StockProductPG
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;

        dj = dj.OrderByDescending(p => p.ApprovalTgl);
        pg = pg.OrderByDescending(p => p.ApprovalTgl);

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            start = Convert.ToDateTime(Request.QueryString["dari"]);
            end = Convert.ToDateTime(Request.QueryString["sampai"]);
            dj = dj.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
            pg = pg.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        if (Cabang > 0)
        {
            dj = dj.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
        }
        else
        {
            dj = dj.Where(p => p.StockActualDJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe);
        }
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0)
        {
            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
            pg = pg.Where(p => p.ProductItem == ProductItem);
        }

        if (dj != null)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "Diamond Jewellery";
            tr.Cells.Add(td);

            int stock1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Count();
            int stock2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Count();
            int stock3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Count();
            int stock4 = dj.Where(p => p.StockActualDJ.Substorage == 4).Count();
            int stock5 = dj.Where(p => p.StockActualDJ.Substorage == 5).Count();
            int stock6 = dj.Where(p => p.StockActualDJ.Substorage == 6).Count();
            int stock96 = dj.Where(p => p.StockActualDJ.Substorage == 96).Count();
            int stock97 = dj.Where(p => p.StockActualDJ.Substorage == 97).Count();
            int stock98 = dj.Where(p => p.StockActualDJ.Substorage == 98).Count();
            int stock99 = dj.Where(p => p.StockActualDJ.Substorage == 99).Count();
            int stockAll = dj.Count();

            decimal netweight1 = 0m;
            decimal netweight2 = 0m;
            decimal netweight3 = 0m;
            decimal netweight4 = 0m;
            decimal netweight5 = 0m;
            decimal netweight6 = 0m;
            decimal netweight96 = 0m;
            decimal netweight97 = 0m;
            decimal netweight98 = 0m;
            decimal netweight99 = 0m;
            decimal netweightAll = 0m;

            try
            {
                netweightAll = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
            }
            catch (Exception ex) { };

            decimal grossweight1 = 0m;
            decimal grossweight2 = 0m;
            decimal grossweight3 = 0m;
            decimal grossweight4 = 0m;
            decimal grossweight5 = 0m;
            decimal grossweight6 = 0m;
            decimal grossweight96 = 0m;
            decimal grossweight97 = 0m;
            decimal grossweight98 = 0m;
            decimal grossweight99 = 0m;
            decimal grossweightAll = 0m;

            try
            {
                grossweightAll = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
            }
            catch (Exception ex) { };

            decimal HargaModal1 = 0m;
            decimal HargaModal2 = 0m;
            decimal HargaModal3 = 0m;
            decimal HargaModal4 = 0m;
            decimal HargaModal5 = 0m;
            decimal HargaModal6 = 0m;
            decimal HargaModal96 = 0m;
            decimal HargaModal97 = 0m;
            decimal HargaModal98 = 0m;
            decimal HargaModal99 = 0m;
            decimal HargaModalAll = 0m;

            try
            {
                decimal AllM = 0m;

                decimal x = dj.Sum(p => p.StockProductDJ_CostingProduct.Total);
                decimal z = dj.Sum(p => p.StockProductDJ_Costing.TotalBiaya);

                AllM = (x + z);

                decimal AllMRupiah = dj.Sum(p => p.StockProductDJ_CostingProduct.Total);

                HargaModalAll = AllMRupiah;
            }
            catch (Exception ex) { };

            decimal Harga1 = 0m;
            decimal Harga2 = 0m;
            decimal Harga3 = 0m;
            decimal Harga4 = 0m;
            decimal Harga5 = 0m;
            decimal Harga6 = 0m;
            decimal Harga96 = 0m;
            decimal Harga97 = 0m;
            decimal Harga98 = 0m;
            decimal Harga99 = 0m;
            decimal HargaAll = 0m;

            decimal HargaRupiah1 = 0m;
            decimal HargaRupiah2 = 0m;
            decimal HargaRupiah3 = 0m;
            decimal HargaRupiah4 = 0m;
            decimal HargaRupiah5 = 0m;
            decimal HargaRupiah6 = 0m;
            decimal HargaRupiah96 = 0m;
            decimal HargaRupiah97 = 0m;
            decimal HargaRupiah98 = 0m;
            decimal HargaRupiah99 = 0m;
            decimal HargaRupiahAll = 0m;

            try
            {
                HargaAll = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiahAll = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex) { };

            try
            {

                netweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweight99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                grossweight99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_CostingProduct.Total));
                Harga99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_PricingMU.HargaUSD));
                HargaRupiah99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }
            decimal TotalButir_1 = 0m;
            decimal TotalCarat_1 = 0m;

            try
            {
                TotalButir_1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_2 = 0m;
            decimal TotalCarat_2 = 0m;

            try
            {
                TotalButir_2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_3 = 0m;
            decimal TotalCarat_3 = 0m;

            try
            {
                TotalButir_3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_4 = 0m;
            decimal TotalCarat_4 = 0m;

            try
            {
                TotalButir_4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_4 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 4).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_5 = 0m;
            decimal TotalCarat_5 = 0m;

            try
            {
                TotalButir_5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_5 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 5).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_6 = 0m;
            decimal TotalCarat_6 = 0m;

            try
            {
                TotalButir_6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_6 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 6).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_96 = 0m;
            decimal TotalCarat_96 = 0m;

            try
            {
                TotalButir_96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_96 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 96).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_97 = 0m;
            decimal TotalCarat_97 = 0m;

            try
            {
                TotalButir_97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_97 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 97).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_98 = 0m;
            decimal TotalCarat_98 = 0m;

            try
            {
                TotalButir_98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_98 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 98).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_99 = 0m;
            decimal TotalCarat_99 = 0m;

            try
            {
                TotalButir_99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
                TotalCarat_99 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 99).Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_All = 0m;
            decimal TotalCarat_All = 0m;

            try
            {
                TotalButir_All = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_CharProduct.StoneQty.Value));
            }
            catch (Exception ex) { }
            try
            {
                TotalCarat_All = Convert.ToDecimal(dj.Sum(p => p.StockProductDJ_CharProduct.StoneCarat));
            }
            catch (Exception ex) { }

            if (stock1 > 0)
            {
                td = new TableCell();
                td.Text = DL.SubStorage(1);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = stock1.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = netweight1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = grossweight1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalButir_1.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat_1.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaModal1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Harga1.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaRupiah1.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            if (stock2 > 0)
            {
                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "Diamond Jewellery";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = DL.SubStorage(2);
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = stock2.ToString();
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = netweight2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = grossweight2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = TotalButir_2.ToString("N0");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = TotalCarat_2.ToString("N3");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = HargaModal2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = Harga2.ToString("N2");
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = HargaRupiah2.ToString("N0");
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }

            if (stock3 > 0)
            {
                TableRow tr3 = new TableRow();
                TableCell td3;

                td3 = new TableCell();
                td3.Text = "Diamond Jewellery";
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = DL.SubStorage(3);
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = stock3.ToString();
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = netweight3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = grossweight3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = TotalButir_3.ToString("N0");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = TotalCarat_3.ToString("N3");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = HargaModal3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = Harga3.ToString("N2");
                tr3.Cells.Add(td3);

                td3 = new TableCell();
                td3.Text = HargaRupiah3.ToString("N0");
                tr3.Cells.Add(td3);

                tb.Rows.Add(tr3);
            }

            if (stock4 > 0)
            {
                TableRow tr4 = new TableRow();
                TableCell td4;

                td4 = new TableCell();
                td4.Text = "Diamond Jewellery";
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = DL.SubStorage(4);
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = stock4.ToString();
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = netweight4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = grossweight4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = TotalButir_4.ToString("N0");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = TotalCarat_4.ToString("N3");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = HargaModal4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = Harga4.ToString("N2");
                tr4.Cells.Add(td4);

                td4 = new TableCell();
                td4.Text = HargaRupiah4.ToString("N0");
                tr4.Cells.Add(td4);

                tb.Rows.Add(tr4);
            }

            if (stock5 > 0)
            {
                TableRow tr5 = new TableRow();
                TableCell td5;

                td5 = new TableCell();
                td5.Text = "Diamond Jewellery";
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = DL.SubStorage(5);
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = stock5.ToString();
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = netweight5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = grossweight5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = TotalButir_5.ToString("N0");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = TotalCarat_5.ToString("N3");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = HargaModal5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = Harga5.ToString("N2");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = HargaRupiah5.ToString("N0");
                tr5.Cells.Add(td5);

                tb.Rows.Add(tr5);
            }

            if (stock6 > 0)
            {
                TableRow tr6 = new TableRow();
                TableCell td6;

                td6 = new TableCell();
                td6.Text = "Diamond Jewellery";
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = DL.SubStorage(6);
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = stock6.ToString();
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = netweight6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = grossweight6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = TotalButir_6.ToString("N0");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = TotalCarat_6.ToString("N3");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = HargaModal6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = Harga6.ToString("N2");
                tr6.Cells.Add(td6);

                td6 = new TableCell();
                td6.Text = HargaRupiah6.ToString("N0");
                tr6.Cells.Add(td6);

                tb.Rows.Add(tr6);
            }

            if (stock96 > 0)
            {
                TableRow tr7 = new TableRow();
                TableCell td7;

                td7 = new TableCell();
                td7.Text = "Diamond Jewellery";
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = DL.SubStorage(96);
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = stock96.ToString();
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = netweight96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = grossweight96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = TotalButir_96.ToString("N0");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = TotalCarat_96.ToString("N3");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = HargaModal96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = Harga96.ToString("N2");
                tr7.Cells.Add(td7);

                td7 = new TableCell();
                td7.Text = HargaRupiah96.ToString("N0");
                tr7.Cells.Add(td7);

                tb.Rows.Add(tr7);
            }

            if (stock97 > 0)
            {
                TableRow tr8 = new TableRow();
                TableCell td8;

                td8 = new TableCell();
                td8.Text = "Diamond Jewellery";
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = DL.SubStorage(97);
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = stock97.ToString();
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = netweight97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = grossweight97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = TotalButir_97.ToString("N0");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = TotalCarat_97.ToString("N3");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = HargaModal97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = Harga97.ToString("N2");
                tr8.Cells.Add(td8);

                td8 = new TableCell();
                td8.Text = HargaRupiah97.ToString("N0");
                tr8.Cells.Add(td8);

                tb.Rows.Add(tr8);
            }

            if (stock98 > 0)
            {

                TableRow tr9 = new TableRow();
                TableCell td9;

                td9 = new TableCell();
                td9.Text = "Diamond Jewellery";
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = DL.SubStorage(98);
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = stock98.ToString();
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = netweight98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = grossweight98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = TotalButir_98.ToString("N0");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = TotalCarat_98.ToString("N3");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = HargaModal98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = Harga98.ToString("N2");
                tr9.Cells.Add(td9);

                td9 = new TableCell();
                td9.Text = HargaRupiah98.ToString("N0");
                tr9.Cells.Add(td9);

                tb.Rows.Add(tr9);
            }

            if (stock99 > 0)
            {

                TableRow tr10 = new TableRow();
                TableCell td10;

                td10 = new TableCell();
                td10.Text = "Diamond Jewellery";
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = DL.SubStorage(99);
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = stock99.ToString();
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = netweight99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = grossweight99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = TotalButir_99.ToString("N0");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = TotalCarat_99.ToString("N3");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = HargaModal99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = Harga99.ToString("N2");
                tr10.Cells.Add(td10);

                td10 = new TableCell();
                td10.Text = HargaRupiah99.ToString("N0");
                tr10.Cells.Add(td10);

                tb.Rows.Add(tr10);
            }

            TableRow tr11 = new TableRow();
            TableCell td11;

            td11 = new TableCell();
            td11.Text = "<b>Total</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>-</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + stockAll.ToString() + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + netweightAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + grossweightAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + TotalButir_All.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + TotalCarat_All.ToString("N3") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaModalAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaAll.ToString("N2") + "</b>";
            tr11.Cells.Add(td11);

            td11 = new TableCell();
            td11.Text = "<b>" + HargaRupiahAll.ToString("N0") + "</b>";
            tr11.Cells.Add(td11);

            tb.Rows.Add(tr11);
        }


        //PG 
        if (pg != null)
        {

            int stockpg1 = pg.Where(p => p.StockActualPG.Substorage == 1).Count();
            int stockpg2 = pg.Where(p => p.StockActualPG.Substorage == 2).Count();
            int stockpg3 = pg.Where(p => p.StockActualPG.Substorage == 3).Count();
            int stockpg4 = pg.Where(p => p.StockActualPG.Substorage == 4).Count();
            int stockpg5 = pg.Where(p => p.StockActualPG.Substorage == 5).Count();
            int stockpg6 = pg.Where(p => p.StockActualPG.Substorage == 6).Count();
            int stockpg96 = pg.Where(p => p.StockActualPG.Substorage == 96).Count();
            int stockpg97 = pg.Where(p => p.StockActualPG.Substorage == 97).Count();
            int stockpg98 = pg.Where(p => p.StockActualPG.Substorage == 98).Count();
            int stockpg99 = pg.Where(p => p.StockActualPG.Substorage == 99).Count();
            int stockpgAll = pg.Count();

            decimal netweightpg1 = 0m;
            decimal netweightpg2 = 0m;
            decimal netweightpg3 = 0m;
            decimal netweightpg4 = 0m;
            decimal netweightpg5 = 0m;
            decimal netweightpg6 = 0m;
            decimal netweightpg96 = 0m;
            decimal netweightpg97 = 0m;
            decimal netweightpg98 = 0m;
            decimal netweightpg99 = 0m;
            decimal netweightpgAll = 0m;

            decimal grossweightpg1 = 0m;
            decimal grossweightpg2 = 0m;
            decimal grossweightpg3 = 0m;
            decimal grossweightpg4 = 0m;
            decimal grossweightpg5 = 0m;
            decimal grossweightpg6 = 0m;
            decimal grossweightpg96 = 0m;
            decimal grossweightpg97 = 0m;
            decimal grossweightpg98 = 0m;
            decimal grossweightpg99 = 0m;
            decimal grossweightpgAll = 0m;

            decimal HargaModalPG1 = 0m;
            decimal HargaModalPG2 = 0m;
            decimal HargaModalPG3 = 0m;
            decimal HargaModalPG4 = 0m;
            decimal HargaModalPG5 = 0m;
            decimal HargaModalPG6 = 0m;
            decimal HargaModalPG96 = 0m;
            decimal HargaModalPG97 = 0m;
            decimal HargaModalPG98 = 0m;
            decimal HargaModalPG99 = 0m;
            decimal HargaModalPGAll = 0m;

            decimal HargaRupiahPG1 = 0m;
            decimal HargaRupiahPG2 = 0m;
            decimal HargaRupiahPG3 = 0m;
            decimal HargaRupiahPG4 = 0m;
            decimal HargaRupiahPG5 = 0m;
            decimal HargaRupiahPG6 = 0m;
            decimal HargaRupiahPG96 = 0m;
            decimal HargaRupiahPG97 = 0m;
            decimal HargaRupiahPG98 = 0m;
            decimal HargaRupiahPG99 = 0m;
            decimal HargaRupiahPGAll = 0m;

            decimal TotalButirPG_All = 0m;
            decimal TotalCaratPG_All = 0m;

            try
            {
                netweightpg1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.GrossWeight));
                grossweightpg1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.NettoWeight));
                HargaModalPG1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.Harga));
                HargaRupiahPG1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweightpg2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.GrossWeight));
                grossweightpg2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.NettoWeight));
                HargaModalPG2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.Harga));
                HargaRupiahPG2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweightpg3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.GrossWeight));
                grossweightpg3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.NettoWeight));
                HargaModalPG3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.Harga));
                HargaRupiahPG3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg4 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 4).Sum(p => p.GrossWeight));
                grossweightpg4 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 4).Sum(p => p.NettoWeight));
                HargaModalPG4 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 4).Sum(p => p.Harga));
                HargaRupiahPG4 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 4).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg5 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 5).Sum(p => p.GrossWeight));
                grossweightpg5 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 5).Sum(p => p.NettoWeight));
                HargaModalPG5 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 5).Sum(p => p.Harga));
                HargaRupiahPG5 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 5).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg6 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 6).Sum(p => p.GrossWeight));
                grossweightpg6 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 6).Sum(p => p.NettoWeight));
                HargaModalPG6 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 6).Sum(p => p.Harga));
                HargaRupiahPG6 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 6).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg96 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 96).Sum(p => p.GrossWeight));
                grossweightpg96 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 96).Sum(p => p.NettoWeight));
                HargaModalPG96 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 96).Sum(p => p.Harga));
                HargaRupiahPG96 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 96).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg97 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 97).Sum(p => p.GrossWeight));
                grossweightpg97 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 97).Sum(p => p.NettoWeight));
                HargaModalPG97 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 97).Sum(p => p.Harga));
                HargaRupiahPG97 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 97).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg98 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 98).Sum(p => p.GrossWeight));
                grossweightpg98 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 98).Sum(p => p.NettoWeight));
                HargaModalPG98 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 98).Sum(p => p.Harga));
                HargaRupiahPG98 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 98).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            try
            {
                netweightpg99 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 99).Sum(p => p.GrossWeight));
                grossweightpg99 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 99).Sum(p => p.NettoWeight));
                HargaModalPG99 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 99).Sum(p => p.Harga));
                HargaRupiahPG99 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 99).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            netweightpgAll = Convert.ToDecimal(pg.Sum(p => p.GrossWeight));
            grossweightpgAll = Convert.ToDecimal(pg.Sum(p => p.NettoWeight));
            HargaModalPGAll = Convert.ToDecimal(pg.Sum(p => p.Harga));
            HargaRupiahPGAll = Convert.ToDecimal(pg.Sum(p => p.TotalHarga24));

            if (stockpg1 > 0)
            {
                TableRow tr21 = new TableRow();
                TableCell td21;

                td21 = new TableCell();
                td21.Text = "Plain Gold";
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = DL.SubStorage(1);
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = stockpg1.ToString();
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = netweightpg1.ToString("N2");
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = grossweightpg1.ToString("N2");
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = "0";
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = "0";
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = HargaModalPG1.ToString("N0");
                tr21.Cells.Add(td21);

                td21 = new TableCell();
                td21.Text = HargaRupiahPG1.ToString("N0");
                tr21.Cells.Add(td21);

                tb2.Rows.Add(tr21);
            }

            if (stockpg2 > 0)
            {

                TableRow tr22 = new TableRow();
                TableCell td22;

                td22 = new TableCell();
                td22.Text = "Plain Gold";
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = DL.SubStorage(2);
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = stockpg2.ToString();
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = netweightpg2.ToString("N2");
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = grossweightpg2.ToString("N2");
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = "0";
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = "0";
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = HargaModalPG2.ToString("N0");
                tr22.Cells.Add(td22);

                td22 = new TableCell();
                td22.Text = HargaRupiahPG2.ToString("N0");
                tr22.Cells.Add(td22);

                tb2.Rows.Add(tr22);
            }

            if (stockpg3 > 0)
            {
                TableRow tr23 = new TableRow();
                TableCell td23;

                td23 = new TableCell();
                td23.Text = "Plain Gold";
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = DL.SubStorage(3);
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = stockpg3.ToString();
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = netweightpg3.ToString("N2");
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = grossweightpg3.ToString("N2");
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = "0";
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = "0";
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = HargaModalPG3.ToString("N0");
                tr23.Cells.Add(td23);

                td23 = new TableCell();
                td23.Text = HargaRupiahPG3.ToString("N0");
                tr23.Cells.Add(td23);

                tb2.Rows.Add(tr23);
            }

            if (stockpg4 > 0)
            {

                TableRow tr24 = new TableRow();
                TableCell td24;

                td24 = new TableCell();
                td24.Text = "Plain Gold";
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = DL.SubStorage(4);
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = stockpg4.ToString();
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = netweightpg4.ToString("N2");
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = grossweightpg4.ToString("N2");
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = "0";
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = "0";
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = HargaModalPG4.ToString("N0");
                tr24.Cells.Add(td24);

                td24 = new TableCell();
                td24.Text = HargaRupiahPG4.ToString("N0");
                tr24.Cells.Add(td24);

                tb2.Rows.Add(tr24);
            }

            if (stockpg5 > 0)
            {

                TableRow tr25 = new TableRow();
                TableCell td25;

                td25 = new TableCell();
                td25.Text = "Plain Gold";
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = DL.SubStorage(5);
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = stockpg5.ToString();
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = netweightpg5.ToString("N2");
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = grossweightpg5.ToString("N2");
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = "0";
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = "0";
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = HargaModalPG5.ToString("N0");
                tr25.Cells.Add(td25);

                td25 = new TableCell();
                td25.Text = HargaRupiahPG5.ToString("N0");
                tr25.Cells.Add(td25);

                tb2.Rows.Add(tr25);
            }

            if (stockpg6 > 0)
            {

                TableRow tr26 = new TableRow();
                TableCell td26;

                td26 = new TableCell();
                td26.Text = "Plain Gold";
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = DL.SubStorage(6);
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = stockpg6.ToString();
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = netweightpg6.ToString("N2");
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = grossweightpg6.ToString("N2");
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = "0";
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = "0";
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = HargaModalPG6.ToString("N0");
                tr26.Cells.Add(td26);

                td26 = new TableCell();
                td26.Text = HargaRupiahPG6.ToString("N0");
                tr26.Cells.Add(td26);

                tb2.Rows.Add(tr26);
            }

            if (stockpg96 > 0)
            {

                TableRow tr27 = new TableRow();
                TableCell td27;

                td27 = new TableCell();
                td27.Text = "Plain Gold";
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = DL.SubStorage(96);
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = stockpg96.ToString();
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = netweightpg96.ToString("N2");
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = grossweightpg96.ToString("N2");
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = "0";
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = "0";
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = HargaModalPG96.ToString("N0");
                tr27.Cells.Add(td27);

                td27 = new TableCell();
                td27.Text = HargaRupiahPG96.ToString("N0");
                tr27.Cells.Add(td27);

                tb2.Rows.Add(tr27);
            }

            if (stockpg97 > 0)
            {

                TableRow tr28 = new TableRow();
                TableCell td28;

                td28 = new TableCell();
                td28.Text = "Plain Gold";
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = DL.SubStorage(97);
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = stockpg97.ToString();
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = netweightpg97.ToString("N2");
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = grossweightpg97.ToString("N2");
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = "0";
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = "0";
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = HargaModalPG97.ToString("N0");
                tr28.Cells.Add(td28);

                td28 = new TableCell();
                td28.Text = HargaRupiahPG97.ToString("N0");
                tr28.Cells.Add(td28);

                tb2.Rows.Add(tr28);
            }

            if (stockpg98 > 0)
            {

                TableRow tr29 = new TableRow();
                TableCell td29;

                td29 = new TableCell();
                td29.Text = "Plain Gold";
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = DL.SubStorage(98);
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = stockpg98.ToString();
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = netweightpg98.ToString("N2");
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = grossweightpg98.ToString("N2");
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = "0";
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = "0";
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = HargaModalPG98.ToString("N0");
                tr29.Cells.Add(td29);

                td29 = new TableCell();
                td29.Text = HargaRupiahPG98.ToString("N0");
                tr29.Cells.Add(td29);

                tb2.Rows.Add(tr29);
            }

            if (stockpg99 > 0)
            {


                TableRow tr30 = new TableRow();
                TableCell td30;

                td30 = new TableCell();
                td30.Text = "Plain Gold";
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = DL.SubStorage(99);
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = stockpg99.ToString();
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = netweightpg99.ToString("N2");
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = grossweightpg99.ToString("N2");
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = "0";
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = "0";
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = HargaModalPG99.ToString("N0");
                tr30.Cells.Add(td30);

                td30 = new TableCell();
                td30.Text = HargaRupiahPG99.ToString("N0");
                tr30.Cells.Add(td30);

                tb2.Rows.Add(tr30);
            }


            TableRow tr31 = new TableRow();
            TableCell td31;

            td31 = new TableCell();
            td31.Text = "<b>Total</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>-</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + stockpgAll.ToString() + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + netweightpgAll.ToString("N2") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + grossweightpgAll.ToString("N2") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + TotalButirPG_All.ToString("N0") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + TotalCaratPG_All.ToString("N3") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + HargaModalPGAll.ToString("N0") + "</b>";
            tr31.Cells.Add(td31);

            td31 = new TableCell();
            td31.Text = "<b>" + HargaRupiahPGAll.ToString("N0") + "</b>";
            tr31.Cells.Add(td31);

            tb2.Rows.Add(tr31);

            /*DateTime start;
            DateTime end;

            var rs = from p in db.StockProductDJ
                     where p.ReturnProduct == false && p.StatusPenjualan == 0
                     select p;

            rs = rs.OrderByDescending(p => p.ApprovalTgl);

            string datepicker = Request.QueryString["dari"];
            string datepicker2 = Request.QueryString["sampai"];

            if (datepicker != "" && datepicker2 != "")
            {
                start = Convert.ToDateTime(datepicker);
                end = Convert.ToDateTime(datepicker2);
                rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end);
            }

            int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
            string kw = Request.QueryString["kw"];
            int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
            int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
            int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
            string orderby = Request.QueryString["orderby"];

            if (kw != "")
                rs = rs.Where(p => p.Nomor == "" + kw + "");

            if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
            else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
            if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);

            if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);

            if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

            if (orderby == "1")
                rs.OrderBy(p => p.LocWarehouse.Nama);
            else if (orderby == "2")
                rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
            else if (orderby == "3")
                rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
            else if (orderby == "4")
                rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

            if (rs != null)
            {
                decimal TotalStock = 0m;
                decimal TotalResell = 0m;
                decimal TotalExchange = 0m;
                decimal TotalHarga = 0m;
                decimal TotalModal = 0m;
                decimal TotalM = 0m;
                decimal TotalR = 0m;
                decimal TotalNetWeight = 0m;
                decimal TotalGrossWeight = 0m;
                decimal TotalButirAll = 0m;
                decimal TotalCaratAll = 0m;
                foreach (var r in rs)
                {
                    decimal AllM = 0m;

                    var x = r.StockProductDJ_CostingProduct;
                    var z = r.StockProductDJ_Costing;

                    AllM = (x.Total + z.TotalBiaya);

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    string KodeKaret;
                    KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                    td = new TableCell();
                    td.Text = "<small>" + KodeKaret + "</small>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    decimal Jual = 0m;
                    decimal Exchange = 0m;
                    decimal Resell = 0m;

                    if (r.StockActualDJ.Substorage == 1)
                        Jual = r.StockActualDJ.InHand;
                    else if (r.StockActualDJ.Substorage == 3)
                        Exchange = r.StockActualDJ.InHand;
                    else if (r.StockActualDJ.Substorage == 2)
                        Resell = r.StockActualDJ.InHand;

                    /-*td = new TableCell();
                    td.Text = Jual.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Exchange.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Resell.ToString("N0");
                    tr.Cells.Add(td);*-/

                    td = new TableCell();
                    td.Text = r.StockActualDJ.InHand.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.SubStorage(r.StockActualDJ.Substorage);
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.StockActualDJ.TipeLokasi == 0)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                    else if (r.StockActualDJ.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                    else if (r.StockActualDJ.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(AllM * z.TotalRate).ToString("N0") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(AllM).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                    tr.Cells.Add(td);

                    var det = (from p in db.StockProductDJ_Stone1A
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                               from p in db.StockProductDJ_Stone1B
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                               from p in db.StockProductDJ_Stone2
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                               from p in db.StockProductDJ_Stone3
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                               from p in db.StockProductDJ_Stone4
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                               from p in db.StockProductDJ_Stone5
                               where p.IDForm == r.ID
                               select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                    decimal TotalButir = 0m;
                    try
                    {
                        TotalButir = det.Sum(p => p.TotalButir);
                    }
                    catch (Exception ex) { }
                    decimal TotalCarat = 0m;
                    try
                    {
                        TotalButir = det.Sum(p => p.TotalCarat);
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = TotalButir.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TotalCarat.ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ_CharProduct.CharStoneDist.Nama;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    TotalStock += Jual;
                    TotalResell += Resell;
                    TotalExchange += Exchange;
                    TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                    TotalModal += Convert.ToDecimal(AllM * z.TotalRate.Value);
                    TotalM += (AllM);
                    TotalR += r.StockProductDJ_PricingMU.ModalUSD;
                    TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                    TotalGrossWeight = r.StockProductDJ_CharProduct.GrossWeight;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;

                }

                if (rs.Count() > 0)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<b>Total</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rs.Sum(p => p.StockActualDJ.InHand).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalModal.ToString("N0") + " / " + TotalHarga.ToString("N0") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
                    tr.Cells.Add(td);

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
                    td.Text = "";
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            */
        }
    }

    protected void fillHeader()
    {

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualDJ select p;

        periode.InnerHtml = Request.QueryString["dari"];
        periode.InnerHtml += " s/d " + Request.QueryString["sampai"];

        if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rsB.SingleOrDefault(p => p.IDLokasi == Convert.ToInt32(Request.QueryString["cabang"]) && p.TipeLokasi == Convert.ToInt32(Request.QueryString["tipe"])); cabang.InnerText = v_cab.NamaLokasi; } else { cabang.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }


        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }


    }
}