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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {

            DLC.CharProductItem(productitem);
            

            DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var dj = from p in db.StockProductDJ
                 select p;
        var pg = from p in db.StockProductPG
                 select p;
        

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
        {
            dj = dj.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
        }
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
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

            decimal netweight1 = 0m;
            decimal netweight2 = 0m;
            decimal netweight3 = 0m;
            decimal grossweight1 = 0m;
            decimal grossweight2 = 0m;
            decimal grossweight3 = 0m;
            decimal HargaModal1 = 0m;
            decimal HargaModal2 = 0m;
            decimal HargaModal3 = 0m;
            decimal HargaRupiah1 = 0m;
            decimal HargaRupiah2 = 0m;
            decimal HargaRupiah3 = 0m;

            try
            {
                netweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p=>p.StockProductDJ_CharProduct.NettoWeight));
                grossweight1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_PricingMU.ModalRupiah));
                HargaRupiah1 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p=>p.StockProductDJ_CharProduct.NettoWeight));
                grossweight2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_PricingMU.ModalRupiah));
                HargaRupiah2 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p=>p.StockProductDJ_CharProduct.NettoWeight));
                grossweight3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                HargaModal3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_PricingMU.ModalRupiah));
                HargaRupiah3 = Convert.ToDecimal(dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }
            catch (Exception ex)
            {
            }

            decimal jlh_butir1A_1 = 0m;
            decimal jlh_butir1B_1 = 0m;
            decimal jlh_butir2_1 = 0m;
            decimal jlh_butir3_1 = 0m;
            decimal jlh_butir4_1 = 0m;
            decimal jlh_butir5_1 = 0m;

            decimal jlh_carat1A_1 = 0m;
            decimal jlh_carat1B_1 = 0m;
            decimal jlh_carat2_1 = 0m;
            decimal jlh_carat3_1 = 0m;
            decimal jlh_carat4_1 = 0m;
            decimal jlh_carat5_1 = 0m;

            try
            {
                jlh_butir1A_1 = dj.Where(p=>p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalButir));
                jlh_carat1A_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir1B_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalButir));
                jlh_carat1B_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir2_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalButir));
                jlh_carat2_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir3_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalButir));
                jlh_carat3_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir4_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalButir));
                jlh_carat4_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir5_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalButir));
                jlh_carat5_1 = dj.Where(p => p.StockActualDJ.Substorage == 1).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_1 = jlh_butir1A_1 + jlh_butir1B_1 + jlh_butir2_1 + jlh_butir3_1 + jlh_butir4_1 + jlh_butir5_1;
            decimal TotalCarat_1 = jlh_carat1A_1 + jlh_carat1B_1 + jlh_carat2_1 + jlh_carat3_1 + jlh_carat4_1 + jlh_carat5_1;

            decimal jlh_butir1A_2 = 0m;
            decimal jlh_butir1B_2 = 0m;
            decimal jlh_butir2_2 = 0m;
            decimal jlh_butir3_2 = 0m;
            decimal jlh_butir4_2 = 0m;
            decimal jlh_butir5_2 = 0m;

            decimal jlh_carat1A_2 = 0m;
            decimal jlh_carat1B_2 = 0m;
            decimal jlh_carat2_2 = 0m;
            decimal jlh_carat3_2 = 0m;
            decimal jlh_carat4_2 = 0m;
            decimal jlh_carat5_2 = 0m;

            try
            {
                jlh_butir1A_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalButir));
                jlh_carat1A_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir1B_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalButir));
                jlh_carat1B_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir2_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalButir));
                jlh_carat2_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir3_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalButir));
                jlh_carat3_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir4_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalButir));
                jlh_carat4_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir5_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalButir));
                jlh_carat5_2 = dj.Where(p => p.StockActualDJ.Substorage == 2).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_2 = jlh_butir1A_2 + jlh_butir1B_2 + jlh_butir2_2 + jlh_butir3_2 + jlh_butir4_2 + jlh_butir5_2;
            decimal TotalCarat_2 = jlh_carat1A_2 + jlh_carat1B_2 + jlh_carat2_2 + jlh_carat3_2 + jlh_carat4_2 + jlh_carat5_2;

            decimal jlh_butir1A_3 = 0m;
            decimal jlh_butir1B_3 = 0m;
            decimal jlh_butir2_3 = 0m;
            decimal jlh_butir3_3 = 0m;
            decimal jlh_butir4_3 = 0m;
            decimal jlh_butir5_3 = 0m;

            decimal jlh_carat1A_3 = 0m;
            decimal jlh_carat1B_3 = 0m;
            decimal jlh_carat2_3 = 0m;
            decimal jlh_carat3_3 = 0m;
            decimal jlh_carat4_3 = 0m;
            decimal jlh_carat5_3 = 0m;

            try
            {
                jlh_butir1A_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalButir));
                jlh_carat1A_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone1A.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir1B_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalButir));
                jlh_carat1B_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone1B.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir2_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalButir));
                jlh_carat2_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone2.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
                jlh_butir2_3 = 0m;
                jlh_carat2_3 = 0m;
            }
            try
            {
                jlh_butir3_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalButir));
                jlh_carat3_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone3.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir4_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalButir));
                jlh_carat4_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone4.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }
            try
            {
                jlh_butir5_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalButir));
                jlh_carat5_3 = dj.Where(p => p.StockActualDJ.Substorage == 3).Sum(p => p.StockProductDJ_Stone5.Sum(x => x.TotalCarat));
            }
            catch (Exception ex)
            {
            }

            decimal TotalButir_3 = jlh_butir1A_3 + jlh_butir1B_3 + jlh_butir2_3 + jlh_butir3_3 + jlh_butir4_3 + jlh_butir5_3;
            decimal TotalCarat_3 = jlh_carat1A_3 + jlh_carat1B_3 + jlh_carat2_3 + jlh_carat3_3 + jlh_carat4_3 + jlh_carat5_3;

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
            td.Text = HargaModal1.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = HargaRupiah1.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = stock2.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = netweight2.ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = grossweight2.ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TotalButir_2.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TotalCarat_2.ToString("N3");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = HargaModal2.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = HargaRupiah2.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = stock3.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = netweight3.ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = grossweight3.ToString("N2");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TotalButir_3.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TotalCarat_3.ToString("N3");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = HargaModal3.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = HargaRupiah3.ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Rekap PG -------------------------------------------------------------------------------------------------------------

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "Plain Gold";
            tr2.Cells.Add(td2);

            int stockpg1 = pg.Where(p => p.StockActualPG.Substorage == 1).Count();
            int stockpg2 = pg.Where(p => p.StockActualPG.Substorage == 2).Count();
            int stockpg3 = pg.Where(p => p.StockActualPG.Substorage == 3).Count();

            decimal netweightpg1 = 0m;
            decimal netweightpg2 = 0m;
            decimal netweightpg3 = 0m;
            decimal grossweightpg1 = 0m;
            decimal grossweightpg2 = 0m;
            decimal grossweightpg3 = 0m;
            decimal HargaModalPG1 = 0m;
            decimal HargaModalPG2 = 0m;
            decimal HargaModalPG3 = 0m;
            decimal HargaRupiahPG1 = 0m;
            decimal HargaRupiahPG2 = 0m;
            decimal HargaRupiahPG3 = 0m;

            try
            {
                netweightpg1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.GrossWeight));
                grossweightpg1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.NettoWeight));
                HargaModalPG1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.TotalHarga));
                HargaRupiahPG1 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 1).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweightpg2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.GrossWeight));
                grossweightpg2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.NettoWeight));
                HargaModalPG2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.TotalHarga));
                HargaRupiahPG2 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 2).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }
            try
            {
                netweightpg3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.GrossWeight));
                grossweightpg3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.NettoWeight));
                HargaModalPG3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.TotalHarga));
                HargaRupiahPG3 = Convert.ToDecimal(pg.Where(p => p.StockActualPG.Substorage == 3).Sum(p => p.TotalHarga24));
            }
            catch (Exception ex)
            {
            }

            td2 = new TableCell();
            td2.Text = stockpg1.ToString();
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = netweightpg1.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = grossweightpg1.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = HargaModalPG1.ToString("N0");
            tr2.Cells.Add(td2);
            
            td2 = new TableCell();
            td2.Text = HargaRupiahPG1.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = stockpg2.ToString();
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = netweightpg2.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = grossweightpg2.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = HargaModalPG2.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = HargaRupiahPG2.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = stockpg3.ToString();
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = netweightpg3.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = grossweightpg3.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "0";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = HargaModalPG3.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = HargaRupiahPG3.ToString("N0");
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);

        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }
}