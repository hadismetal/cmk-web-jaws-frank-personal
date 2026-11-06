using System;
using System.Linq;
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
    App gp = new App();
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    decimal TotalQty = 0m;
    decimal TotalHargaJual = 0m;
    decimal TotalM = 0m;
    decimal TotalR = 0m;
    decimal TotalGrossWeight = 0m;
    decimal TotalNetWeight = 0m;
    decimal TotalStone = 0m;
    decimal TotalCarat = 0m;

    int TotalDJS1 = 0;
    int TotalDJS2 = 0;
    int TotalDJS3 = 0;
    int TotalDJS4 = 0;
    int TotalDJS5 = 0;
    int TotalDJS6 = 0;
    int TotalDJS7 = 0;
    int TotalDJS95 = 0;
    int TotalDJS96 = 0;
    int TotalDJS97 = 0;
    int TotalDJS98 = 0;
    int TotalDJS99 = 0;

    int TotalGJS1 = 0;
    int TotalGJS2 = 0;
    int TotalGJS3 = 0;
    int TotalGJS4 = 0;
    int TotalGJS5 = 0;
    int TotalGJS6 = 0;
    int TotalGJS7 = 0;
    int TotalGJS95 = 0;
    int TotalGJS96 = 0;
    int TotalGJS97 = 0;
    int TotalGJS98 = 0;
    int TotalGJS99 = 0;

    int TotalPGS1 = 0;
    int TotalPGS2 = 0;
    int TotalPGS3 = 0;
    int TotalPGS4 = 0;
    int TotalPGS5 = 0;
    int TotalPGS6 = 0;
    int TotalPGS7 = 0;
    int TotalPGS95 = 0;
    int TotalPGS96 = 0;
    int TotalPGS97 = 0;
    int TotalPGS98 = 0;
    int TotalPGS99 = 0;

    int TotalLDS1 = 0;
    int TotalLDS2 = 0;
    int TotalLDS3 = 0;
    int TotalLDS4 = 0;
    int TotalLDS5 = 0;
    int TotalLDS6 = 0;
    int TotalLDS7 = 0;
    int TotalLDS95 = 0;
    int TotalLDS96 = 0;
    int TotalLDS97 = 0;
    int TotalLDS98 = 0;
    int TotalLDS99 = 0;

    int TotalMTS1 = 0;
    int TotalMTS2 = 0;
    int TotalMTS3 = 0;
    int TotalMTS4 = 0;
    int TotalMTS5 = 0;
    int TotalMTS6 = 0;
    int TotalMTS7 = 0;
    int TotalMTS95 = 0;
    int TotalMTS96 = 0;
    int TotalMTS97 = 0;
    int TotalMTS98 = 0;
    int TotalMTS99 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            using (Map_AC ac = new Map_AC())
            {
                UserAccount user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);

                if (user != null && user.UserNumber == "260875")
                {
                    Response.Redirect("~/Page/Report/Stock_Ledger.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }

            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);
            DLC.CompanyBrand(brand);
            Ddl.Substorage(substorage);
            //DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void getLokasi(object sender, EventArgs e)
    {
        location.Items.Clear();
        if (locationtype.SelectedValue == "0")
        {
            DLC.LocationList(location, 0);
        }
        else if (locationtype.SelectedValue == "1")
        {
            DLC.LocationList(location, 1);
        }
        else if (locationtype.SelectedValue == "2")
        {
            DLC.LocationList(location, 2);
        }
    }

    //Display Table
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     /*var dj = from p in db.StockProductDJ
    //              where p.ReturnProduct == false && p.StatusLebur == false
    //                        && !(from s in db.SalesOrderDJ
    //                             where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
    //                             select s.IDProduct).Contains(p.ID)
    //              select p;
    //     var pg = from p in db.StockProductPG
    //              where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
    //              select p;

    //     int LocationType = Convert.ToInt32(locationtype.SelectedValue);
    //     if (LocationType >= 0 && LocationType < 3)
    //     {
    //         dj = dj.Where(p => p.StockActualDJ.TipeLokasi == LocationType);
    //         pg = pg.Where(p => p.StockActualPG.TipeLokasi == LocationType);
    //     }

    //     int ProductItem = Convert.ToInt32(productitem.SelectedValue);
    //     if (ProductItem > 0)
    //     {
    //         dj = dj.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
    //         pg = pg.Where(p => p.ProductItem == ProductItem);
    //     }

    //     int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
    //     if (ProductLevel > 0)
    //     {
    //         dj = dj.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
    //         pg = pg.Where(p => p.ProductLevel == ProductLevel);
    //     }

    //     int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
    //     if (ProductCategory > 0)
    //     {
    //         dj = dj.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
    //         //pg = pg.Where(p => p. == ProductItem);
    //     }

    //     int Brand = Convert.ToInt32(brand.SelectedValue);
    //     if (Brand > 0)
    //     {
    //         dj = dj.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);
    //     }

    //     if (dj.Count() == 0) App.TableNoResult(tb);
    //     else
    //     {
    //         decimal TotalStock = 0m;
    //         decimal TotalNetWeight = 0m;
    //         decimal TotalGrossWeight = 0m;
    //         decimal TotalButir = 0m;
    //         decimal TotalCarat = 0m;
    //         decimal TotalM = 0m;
    //         decimal TotalR = 0m;

    //         var rs_dj = dj.GroupBy(p => new { p.StockActualDJ.TipeLokasi, p.StockActualDJ.IDLokasi });

    //         foreach (var r in rs_dj)
    //         {
    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = DL.LocationType(r.First().StockActualDJ.TipeLokasi);
    //             tr.Cells.Add(td);

    //             string Lokasi = "";

    //             if (r.First().StockActualDJ.TipeLokasi == 0)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);
    //             else if (r.First().StockActualDJ.TipeLokasi == 1)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);
    //             else if (r.First().StockActualDJ.TipeLokasi == 2)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);

    //             td = new TableCell();
    //             td.Text = Lokasi;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Count()).ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight)).ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight)).ToString("N2");
    //             tr.Cells.Add(td);

    //             decimal Butir = 0m;
    //             decimal Carat = 0m;

    //             Butir = Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1B.Where(z => z.StatusTerpasang == 1).Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalButir)));
    //             Carat = Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1B.Where(z => z.StatusTerpasang == 1).Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalCarat)));

    //             td = new TableCell();
    //             td.Text = Butir.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Carat.ToString("N3");
    //             tr.Cells.Add(td);

    //             decimal AllM = 0m;
    //             decimal AllMRupiah = 0m;
    //             try
    //             {
    //                 decimal x = r.Sum(p => p.StockProductDJ.StockProductDJ_CostingProduct.Total);
    //                 decimal z = r.Sum(p => p.StockProductDJ.StockProductDJ_Costing.TotalBiaya);

    //                 AllM = (x + z);

    //                 AllMRupiah = r.Sum(p => (p.StockProductDJ.StockProductDJ_CostingProduct.Total * p.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
    //             }
    //             catch (Exception ex) { }

    //             td = new TableCell();
    //             td.Text = AllM.ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD)).ToString("N2");
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);

    //             TotalStock += r.Count();
    //             TotalNetWeight += Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight));
    //             TotalGrossWeight += Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight));
    //             TotalButir += Butir;
    //             TotalCarat += Carat;
    //             TotalM += AllM;
    //             TotalR += Convert.ToDecimal(r.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD));
    //         }

    //         TableRow tr2 = new TableRow();
    //         TableCell td2;

    //         td2 = new TableCell();
    //         td2.ColumnSpan = 2;
    //         td2.Text = "<b>Total</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalStock.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         tb.Rows.Add(tr2);
    //     }

    //     if (pg.Count() == 0) App.TableNoResult(tb2);
    //     else
    //     {
    //         decimal TotalStock = 0m;
    //         decimal TotalNetWeight = 0m;
    //         decimal TotalGrossWeight = 0m;
    //         decimal TotalButir = 0m;
    //         decimal TotalCarat = 0m;
    //         decimal TotalM = 0m;
    //         decimal TotalR = 0m;

    //         //var rs_pg = pg.GroupBy(p => new { p.StockActualPG.TipeLokasi, p.StockActualPG.IDLokasi });
    //         var rs_pg = from p in pg
    //                     group p by new { p.StockActualPG.TipeLokasi, p.StockActualPG.IDLokasi } into g
    //                     select new
    //                     {
    //                         TipeLokasi = g.Key,
    //                         IDLokasi = g.First().StockActualPG.IDLokasi,
    //                         NettoWeight = g.Sum(s => s.NettoWeight),
    //                         GrossWeight = g.Sum(s => s.GrossWeight),
    //                         Harga = g.Sum(s => s.Harga),
    //                         TotalHarga24 = g.Sum(s => s.TotalHarga24),
    //                         TotalItem = g.Count(),
    //                         StockActualPG = g.First().StockActualPG
    //                     };

    //         foreach (var r in rs_pg)
    //         {
    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = DL.LocationType(r.StockActualPG.TipeLokasi);
    //             tr.Cells.Add(td);

    //             string Lokasi = "";

    //             if (r.StockActualPG.TipeLokasi == 0)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);
    //             else if (r.StockActualPG.TipeLokasi == 1)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);
    //             else if (r.StockActualPG.TipeLokasi == 2)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);

    //             td = new TableCell();
    //             td.Text = Lokasi;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.TotalItem).ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.NettoWeight).ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
    //             tr.Cells.Add(td);

    //             decimal Butir = 0m;
    //             decimal Carat = 0m;

    //             td = new TableCell();
    //             td.Text = Butir.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Carat.ToString("N3");
    //             tr.Cells.Add(td);

    //             decimal AllM = 0m;
    //             decimal AllMRupiah = 0m;
    //             try
    //             {
    //                 AllMRupiah = r.Harga.Value;
    //             }
    //             catch (Exception ex) { }

    //             td = new TableCell();
    //             td.Text = AllMRupiah.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.TotalHarga24).ToString("N0");
    //             tr.Cells.Add(td);

    //             tb2.Rows.Add(tr);

    //             TotalStock += r.TotalItem;
    //             TotalNetWeight += Convert.ToDecimal(r.NettoWeight);
    //             TotalGrossWeight += Convert.ToDecimal(r.GrossWeight);
    //             TotalButir += Butir;
    //             TotalCarat += Carat;
    //             TotalM += AllMRupiah;
    //             TotalR += Convert.ToDecimal(r.TotalHarga24);
    //         }

    //         TableRow tr2 = new TableRow();
    //         TableCell td2;

    //         td2 = new TableCell();
    //         td2.ColumnSpan = 2;
    //         td2.Text = "<b>Total</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalStock.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalM.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = "<b>" + TotalR.ToString("N0") + "</b>";
    //         tr2.Cells.Add(td2);

    //         tb2.Rows.Add(tr2);
    //     }*/

    //     int Tipe = Convert.ToInt32(Request.QueryString["locationtype"]);

    //     int Lokasi = 0;
    //     try
    //     {
    //         Lokasi = Convert.ToInt32(Request.QueryString["location"]);
    //     }
    //     catch (Exception ex) { }

    //     fillDJ(Tipe,Lokasi);
    //     fillGJ(Tipe, Lokasi);
    //     fillPG(Tipe, Lokasi);
    //     fillLD(Tipe, Lokasi);
    //     fillMonting(Tipe, Lokasi);
    //     fillTotalStock();

    //     int Substorage = Convert.ToInt32(substorage.SelectedValue);

    //     if (Substorage == 0)
    //     {
    //         fillStockSubstorage();
    //     }
    // }

protected void fillTable(object sender, EventArgs e)    
    {

        int Tipe = Convert.ToInt32(Request.QueryString["locationtype"]);

        int Lokasi = 0;
        try
        {
            Lokasi = Convert.ToInt32(Request.QueryString["location"]);
            if (Lokasi == -1)
            {
                Lokasi =0;
            }
        }
        catch (Exception ex) { }        
        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        int Brand = Convert.ToInt32(brand.SelectedValue);                
        int konsignmen = Convert.ToInt32(consignment.SelectedValue);
        int GroupBy = 0;
        string PLU = "";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_StockInventorySummary_adis", connection))
            {
				command.CommandTimeout = 1000;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = PLU });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Lokasi });
                command.Parameters.Add(new SqlParameter("@ProductItem", SqlDbType.Int) { Value = ProductItem });
                command.Parameters.Add(new SqlParameter("@ProductCategory", SqlDbType.Int) { Value = ProductCategory });
                command.Parameters.Add(new SqlParameter("@Substorage", SqlDbType.Int) { Value = Substorage });
                command.Parameters.Add(new SqlParameter("@GroupBy", SqlDbType.Int) { Value = GroupBy });                
                command.Parameters.Add(new SqlParameter("@Brand", SqlDbType.Int) { Value = Brand });
                command.Parameters.Add(new SqlParameter("@Consignment", SqlDbType.Int) { Value = konsignmen });
                command.Parameters.Add(new SqlParameter("@ProductLevel", SqlDbType.Int) { Value = ProductLevel });


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();

                    td = new TableCell();
                    td.Text = row["Keterangan"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Qty"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["HargaM"]).ToString("N2"); ;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["HargaR"]).ToString("N2"); ;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalHarga"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["GrossWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["NettoWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);
                    tb3.Rows.Add(tr);
                }


                DataTable dt2 = ds.Tables[1];
                
                int i = 0;

                foreach (DataRow row in dt2.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();

                    string Jual = row["Jual"].ToString();
                    string LinkJual = row["LinkJual"].ToString();

                    string Resell = row["Resell"].ToString();
                    string LinkResell = row["LinkResell"].ToString();
                    
                    string TradeIn = row["TradeIn"].ToString();
                    string LinkTradeIn = row["LinkTradeIn"].ToString();
                    
                    string TitipanCustomer = row["TitipanCustomer"].ToString();
                    string LinkTitipanCustomer = row["LinkTitipanCustomer"].ToString();
                    
                    string Reparasi = row["Reparasi"].ToString();
                    string LinkReparasi = row["LinkReparasi"].ToString();
                    
                    string Lebur = row["Lebur"].ToString();
                    string LinkLebur = row["LinkLebur"].ToString();
                    
                    string Hadiah = row["Hadiah"].ToString();
                    string LinkHadiah = row["LinkHadiah"].ToString();
                    
                    string OngoingLebur = row["OngoingLebur"].ToString();
                    string LinkOngoingLebur = row["LinkOngoingLebur"].ToString();
                    
                    string Broken = row["Broken"].ToString();
                    string LinkBroken = row["LinkBroken"].ToString();
                    
                    string Ongoing = row["Ongoing"].ToString();
                    string LinkOngoing = row["LinkOngoing"].ToString();
                    
                    string Lost = row["Lost"].ToString();
                    string LinkLost = row["LinkLost"].ToString();

                    td = new TableCell();
                    td.Text = row["Keterangan"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkJual + "'>" + Jual + "</a>";
                    } else {    
                        td.Text = row["Jual"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkResell + "'>" + Resell + "</a>";
                    } else {    
                        td.Text = row["Resell"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkTradeIn + "'>" + TradeIn + "</a>";
                    } else {    
                        td.Text = row["TradeIn"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkTitipanCustomer + "'>" + TitipanCustomer + "</a>";
                    } else {    
                        td.Text = row["TitipanCustomer"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkReparasi + "'>" + Reparasi + "</a>";
                    } else {    
                        td.Text = row["Reparasi"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkLebur + "'>" + Lebur + "</a>";
                    } else {    
                        td.Text = row["Lebur"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkHadiah + "'>" + Hadiah + "</a>";
                    } else {    
                        td.Text = row["Hadiah"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkOngoingLebur + "'>" + OngoingLebur + "</a>";
                    } else {    
                        td.Text = row["OngoingLebur"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkBroken + "'>" + Broken + "</a>";
                    } else {    
                        td.Text = row["Broken"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    tr.Cells.Add(td);
                    // td = new TableCell();
                    // td.Text = row["OngoingDraft"].ToString();
                    // tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkOngoing + "'>" + Ongoing + "</a>";
                    } else {    
                        td.Text = row["Ongoing"].ToString();    
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    if (i != 4) {
                        td.Text = "<a href='" + LinkLost + "'>" + Lost + "</a>";
                    } else {    
                        td.Text = row["Lost"].ToString();
                    }
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Total"].ToString();
                    tr.Cells.Add(td);

                    tb4.Rows.Add(tr);

                    i++;
                }

                tb4.Visible = true;
            }
        }

    }

    protected void fillTotalStock()
    {
        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Total</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalQty.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalR.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalHargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalGrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalNetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalCarat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\" text-info\">" + TotalStone.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);
    }

    protected void fillStockSubstorage()
    {
        tb4.Visible = true;

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "Diamond Jewellery";
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS1.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=1\">" + TotalDJS1.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS2.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=2\">" + TotalDJS2.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS3.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=3\">" + TotalDJS3.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS4.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=4\">" + TotalDJS4.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS5.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=5\">" + TotalDJS5.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS6.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=6\">" + TotalDJS6.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS7.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=7\">" + TotalDJS7.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS95.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=95\">" + TotalDJS95.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS96.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=96\">" + TotalDJS96.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS97.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=97\">" + TotalDJS97.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS98.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=98\">" + TotalDJS98.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        //td.Text = "<p class=\"text-info\">" + TotalDJS99.ToString("N0") + "</p>";
        td.Text = "<a target=\"_blank\" href=\"Report/DJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=99\">" + TotalDJS99.ToString("N0") + "</a>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-info\">" + (TotalDJS1 + TotalDJS2 + TotalDJS3 + TotalDJS4 + TotalDJS5 + TotalDJS6 + TotalDJS7 + TotalDJS95 + TotalDJS96 + TotalDJS97 + TotalDJS98 + TotalDJS99).ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb4.Rows.Add(tr);

        TableRow tr2 = new TableRow();
        TableCell td2 = new TableCell();

        td2 = new TableCell();
        td2.Text = "Gold Jewellery";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS1.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=1\">" + TotalGJS1.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS2.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=2\">" + TotalGJS2.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS3.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=3\">" + TotalGJS3.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS4.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=4\">" + TotalGJS4.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS5.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=5\">" + TotalGJS5.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS6.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=6\">" + TotalGJS6.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS7.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=7\">" + TotalGJS7.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS95.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=95\">" + TotalGJS95.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS96.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=96\">" + TotalGJS96.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS97.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=97\">" + TotalGJS97.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS98.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=1=98\">" + TotalGJS98.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        //td2.Text = "<p class=\"text-info\">" + TotalGJS99.ToString("N0") + "</p>";
        td2.Text = "<a target=\"_blank\" href=\"Report/GJ_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=99\">" + TotalGJS99.ToString("N0") + "</a>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-info\">" + (TotalGJS1 + TotalGJS2 + TotalGJS3 + TotalGJS4 + TotalGJS5 + TotalGJS6 + TotalGJS7 + TotalGJS95 + TotalGJS96 + TotalGJS97 + TotalGJS98 + TotalGJS99).ToString("N0") + "</p>";
        td2.HorizontalAlign = HorizontalAlign.Right;
        tr2.Cells.Add(td2);

        tb4.Rows.Add(tr2);

        TableRow tr3 = new TableRow();
        TableCell td3 = new TableCell();

        td3 = new TableCell();
        td3.Text = "Plain Gold";
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS1.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=1\">" + TotalPGS1.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS2.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=2\">" + TotalPGS2.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS3.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=3\">" + TotalPGS3.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS4.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=4\">" + TotalPGS4.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS5.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=5\">" + TotalPGS5.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS6.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=6\">" + TotalPGS6.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS7.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=7\">" + TotalPGS7.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS95.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=95\">" + TotalPGS95.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS96.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=96\">" + TotalPGS96.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS97.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=97\">" + TotalPGS97.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS98.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=98\">" + TotalPGS98.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        //td3.Text = "<p class=\"text-info\">" + TotalPGS99.ToString("N0") + "</p>";
        td3.Text = "<a target=\"_blank\" href=\"Report/PG_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=99\">" + TotalPGS99.ToString("N0") + "</a>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        td3 = new TableCell();
        td3.Text = "<p class=\"text-info\">" + (TotalPGS1 + TotalPGS2 + TotalPGS3 + TotalPGS4 + TotalPGS5 + TotalPGS6 + TotalPGS7 + TotalPGS95 + TotalPGS96 + TotalPGS97 + TotalPGS98 + TotalPGS99).ToString("N0") + "</p>";
        td3.HorizontalAlign = HorizontalAlign.Right;
        tr3.Cells.Add(td3);

        tb4.Rows.Add(tr3);

        TableRow tr4 = new TableRow();
        TableCell td4 = new TableCell();

        td4 = new TableCell();
        td4.Text = "Loose Diamond";
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS1.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=1\">" + TotalLDS1.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS2.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=2\">" + TotalLDS2.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS3.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=3\">" + TotalLDS3.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS4.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=4\">" + TotalLDS4.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS5.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=5\">" + TotalLDS5.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS6.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=6\">" + TotalLDS6.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS7.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=7\">" + TotalLDS7.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS95.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=95\">" + TotalLDS95.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS96.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=96\">" + TotalLDS96.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS97.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=97\">" + TotalLDS97.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS98.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=98\">" + TotalLDS98.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        //td4.Text = "<p class=\"text-info\">" + TotalLDS99.ToString("N0") + "</p>";
        td4.Text = "<a target=\"_blank\" href=\"Report/LD_List.aspx?refreshPage=1&productlevel=" + productlevel.SelectedValue + "&productcategory=" + productcategory.SelectedValue + "&productitem=" + productitem.SelectedValue + "&substorage=99\">" + TotalLDS99.ToString("N0") + "</a>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        td4 = new TableCell();
        td4.Text = "<p class=\"text-info\">" + (TotalLDS1 + TotalLDS2 + TotalLDS3 + TotalLDS4 + TotalLDS5 + TotalLDS6 + TotalLDS7 + TotalLDS95 + TotalLDS96 + TotalLDS97 + TotalLDS98 + TotalLDS99).ToString("N0") + "</p>";
        td4.HorizontalAlign = HorizontalAlign.Right;
        tr4.Cells.Add(td4);

        tb4.Rows.Add(tr4);

        TableRow tr6 = new TableRow();
        TableCell td6 = new TableCell();

        td6 = new TableCell();
        td6.Text = "Mounting WIP";
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS1.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS2.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS3.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS4.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS5.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS6.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS7.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS95.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS96.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS97.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS98.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + TotalMTS99.ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        td6 = new TableCell();
        td6.Text = "<p class=\"text-info\">" + (TotalMTS1 + TotalMTS2 + TotalMTS3 + TotalMTS4 + TotalMTS5 + TotalMTS6 + TotalMTS7 + TotalMTS95 + TotalMTS96 + TotalMTS97 + TotalMTS98 + TotalMTS99).ToString("N0") + "</p>";
        td6.HorizontalAlign = HorizontalAlign.Right;
        tr6.Cells.Add(td6);

        tb4.Rows.Add(tr6);

        TableRow tr5 = new TableRow();
        TableCell td5 = new TableCell();

        td5 = new TableCell();
        td5.Text = "All";
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS1 + TotalGJS1 + TotalPGS1 + TotalLDS1 + TotalMTS1).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS2 + TotalGJS2 + TotalPGS2 + TotalLDS2 + TotalMTS2).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS3 + TotalGJS3 + TotalPGS3 + TotalLDS3 + TotalMTS3).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS4 + TotalGJS4 + TotalPGS4 + TotalLDS4 + TotalMTS4).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS5 + TotalGJS5 + TotalPGS5 + TotalLDS5 + TotalMTS5).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS6 + TotalGJS6 + TotalPGS6 + TotalLDS6 + TotalMTS6).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS7 + TotalGJS7 + TotalPGS7 + TotalLDS7 + TotalMTS7).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS95 + TotalGJS95 + TotalPGS95 + TotalLDS95 + TotalMTS95).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS96 + TotalGJS96 + TotalPGS96 + TotalLDS96 + TotalMTS96).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS97 + TotalGJS97 + TotalPGS97 + TotalLDS97 + TotalMTS97).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS98 + TotalGJS98 + TotalPGS98 + TotalLDS98 + TotalMTS98).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + (TotalDJS99 + TotalGJS99 + TotalPGS99 + TotalLDS99 + TotalMTS99).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        td5 = new TableCell();
        td5.Text = "<p class=\"text-info\">" + ((TotalDJS1 + TotalDJS2 + TotalDJS3 + TotalDJS4 + TotalDJS5 + TotalDJS6 + TotalDJS7 + TotalDJS95 + TotalDJS96 + TotalDJS97 + TotalDJS98 + TotalDJS99) + (TotalGJS1 + TotalGJS2 + TotalGJS3 + TotalGJS4 + TotalGJS5 + TotalGJS6 + TotalGJS7 + TotalGJS95 + TotalGJS96 + TotalGJS97 + TotalGJS98 + TotalGJS99) + (TotalPGS1 + TotalPGS2 + TotalPGS3 + TotalPGS4 + TotalPGS5 + TotalPGS6 + TotalPGS7 + TotalPGS95 + TotalPGS96 + TotalPGS97 + TotalPGS98 + TotalPGS99) + (TotalLDS1 + TotalLDS2 + TotalLDS3 + TotalLDS4 + TotalLDS5 + TotalLDS6 + TotalLDS7 + TotalLDS95 + TotalLDS96 + TotalLDS97 + TotalLDS98 + TotalLDS99) + (TotalMTS1 + TotalMTS2 + TotalMTS3 + TotalMTS4 + TotalMTS5 + TotalMTS6 + TotalMTS7 + TotalMTS96 + TotalMTS97 + TotalMTS98 + TotalMTS99)).ToString("N0") + "</p>";
        td5.HorizontalAlign = HorizontalAlign.Right;
        tr5.Cells.Add(td5);

        tb4.Rows.Add(tr5);
    }

    protected void fillDJ(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualDJ
                 where p.IDLokasi > 0 && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false && !p.StockProductDJ.Draft.Value
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi != 0)
                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            rs = rs.Where(p => p.TipeLokasi == Tipe);
            
        }

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
            
        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
            
        }

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.LocWarehouse.CompanyBrand.ID == Brand);
        }

        decimal StockDJ = 0m;
        try
        {
            //StockDJ = rs.Count();
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99)
            //    StockDJ = rs.Sum(p => p.StockActualDJ.InHand);
            //else
                StockDJ = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p=>p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductDJ.StockProductDJ_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductDJ.StockProductDJ_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductDJ.StockProductDJ_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }
        

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        decimal StoneQty = 0m;
        try
        {
            StoneQty = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value);
        }
        catch (Exception ex) { }

        decimal StoneCarat = 0m;
        try
        {
            StoneCarat = rs.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Diamond Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockDJ.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaR.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        

        td = new TableCell();
        td.Text = "<p class=\"\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StoneCarat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StoneQty.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockDJ;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StoneQty;
        TotalCarat += StoneCarat;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            var temp = (from p in rs
                       where p.Substorage == 1
                       select p.IDProduct).ToList();
            TotalDJS1 = temp.Count();
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalDJS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalDJS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalDJS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalDJS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillGJ(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualGJ
                 where p.IDLokasi > 0 && p.StockProductGJ.ReturnProduct == false && p.StockProductGJ.StatusLebur == false
                           && !(from s in db.SalesOrderGJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi != 0)
                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            rs = rs.Where(p => p.TipeLokasi == Tipe);

        }

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == ProductItem);

        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == ProductLevel);

        }

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand > 0)
        {
            rs = rs.Where(p => p.StockProductGJ.LocWarehouse.CompanyBrand.ID == Brand);
        }

        decimal StockGJ = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99)
            //    StockGJ = rs.Sum(p => p.StockActualGJ.InHand);
            //else
                StockGJ = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductGJ.StockProductGJ_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductGJ.StockProductGJ_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductGJ.StockProductGJ_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Gold Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockGJ.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaR.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        

        td = new TableCell();
        td.Text = "<p class=\"\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockGJ;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalGJS1 = Convert.ToInt32(rs.Where(p => p.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalGJS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalGJS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalGJS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalGJS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillPG(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockActualPG
                 where p.IDLokasi > 0 && p.StockProductPG.ReturnProduct == false && p.StockProductPG.StatusLebur == false
                           && !(from s in db.SalesOrderPG
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.Substorage == Substorage);

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi != 0)
                rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            rs = rs.Where(p => p.TipeLokasi == Tipe);

        }

        decimal StockPG = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99)
            //    StockPG = rs.Sum(p => p.StockActualPG.InHand);
            //else
                StockPG = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;

        try
        {
            HargaJual = rs.Sum(p => p.StockProductPG.TotalHargaJual.Value);
        }
        catch { }

        // foreach (var r in rs)
        // {
            // HargaJual += r.StockProductPG.FixRate == true ? r.StockProductPG.TotalHargaJual.Value : gp.GetPGPricing(r.StockProductPG, false);
        // }

        decimal HargaM = 0m;

        try
        {
            HargaM = rs.Sum(p => p.StockProductPG.HargaUSD.Value);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductPG.GrossWeight.Value);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductPG.GrossWeight.Value);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Plain Gold</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockPG.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">-</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockPG;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;

        try
        {
            TotalPGS1 = Convert.ToInt32(rs.Where(p => p.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS2 = Convert.ToInt32(rs.Where(p => p.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS3 = Convert.ToInt32(rs.Where(p => p.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS4 = Convert.ToInt32(rs.Where(p => p.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS5 = Convert.ToInt32(rs.Where(p => p.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS6 = Convert.ToInt32(rs.Where(p => p.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS7 = Convert.ToInt32(rs.Where(p => p.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS95 = Convert.ToInt32(rs.Where(p => p.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalPGS96 = Convert.ToInt32(rs.Where(p => p.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalPGS97 = rs.Where(p => p.Substorage == 97).Count();
        TotalPGS98 = rs.Where(p => p.Substorage == 98).Count();
        TotalPGS99 = rs.Where(p => p.Substorage == 99).Count();
    }

    protected void fillLD(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false
                           && !(from s in db.SalesOrderLD
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        //if (Substorage == 0)
        //    Substorage = 1;

        if (Substorage != 0)
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi > 0)
                rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == Lokasi && p.StockActualLD_Stone1B.TipeLokasi == Tipe);
            rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == Tipe);

        }

        decimal StockLD = 0m;
        try
        {
            //if (Substorage != 97 && Substorage != 98 && Substorage != 99)
            //    StockLD = rs.Sum(p => p.StockActualLD_Stone1B.InHand);
            //else
                StockLD = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;

        var us = from p in db.PricingTableUS
                 where p.ID == CV.US_General_Pricing
                 select p;

        try
        {
            HargaJual = rs.Sum(p => p.Stone1B.StonePricing1B.Harga.Value * us.First().Harga.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;

        try
        {
            HargaM = rs.Sum(p => p.HargaTotalM.Value);
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;

        try
        {
            HargaR = rs.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;

        decimal NetWeight = 0m;

        decimal Carat = 0m;

        try
        {
            Carat = rs.Sum(p=>p.TotalCarat);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Loose Diamond</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockLD.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaR.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        

        td = new TableCell();
        td.Text = "<p class=\"\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + Carat.ToString("N3") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockLD.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockLD;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalStone += StockLD;
        TotalCarat += Carat;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalLDS1 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 1).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS2 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 2).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS3 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 3).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS4 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 4).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS5 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 5).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS6 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 6).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS7 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS95 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 95).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        try
        {
            TotalLDS96 = Convert.ToInt32(rs.Where(p => p.StockActualLD_Stone1B.Substorage == 96).Sum(p => p.StockActualLD_Stone1B.InHand));
        }
        catch (Exception ex) { }
        TotalLDS97 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 97).Count();
        TotalLDS98 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 98).Count();
        TotalLDS99 = rs.Where(p => p.StockActualLD_Stone1B.Substorage == 99).Count();
    }

    protected void fillMonting(int Tipe, int Lokasi)
    {
        var rs = from p in db.StockProductMounting
                 where p.StatusReceive == false
                 select p;

        if (Tipe >= 0 && Tipe < 3)
        {
            if (Lokasi != 0)
                rs = rs.Where(p => p.StockActualMounting.IDLokasi == Lokasi);
            rs = rs.Where(p => p.StockActualMounting.TipeLokasi == Tipe);

        }

        decimal StockMounting = 0m;
        try
        {
            StockMounting = rs.Sum(p => p.StockActualMounting.InHand);
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductMounting_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductMounting_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductMounting_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductMounting_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductMounting_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductMounting_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Mounting</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + StockMounting.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaM.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaR.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + HargaJual.ToString("N0") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + GrossWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"\">" + NetWeight.ToString("N2") + "</p>";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb3.Rows.Add(tr);

        TotalQty += StockMounting;
        TotalHargaJual += HargaJual;
        TotalGrossWeight += GrossWeight;
        TotalNetWeight += NetWeight;
        TotalM += HargaM;
        TotalR += HargaR;

        try
        {
            TotalMTS1 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 1).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS2 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 2).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS3 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 3).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS4 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 4).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS5 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 5).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS6 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 6).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS7 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 7).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS95 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 95).Count());
        }
        catch (Exception ex) { }
        try
        {
            TotalMTS96 = Convert.ToInt32(rs.Where(p => p.StockActualMounting.Substorage == 96).Count());
        }
        catch (Exception ex) { }
        TotalMTS97 = rs.Where(p => p.StockActualMounting.Substorage == 97).Count();
        TotalMTS98 = rs.Where(p => p.StockActualMounting.Substorage == 98).Count();
        TotalMTS99 = rs.Where(p => p.StockActualMounting.Substorage == 99).Count();
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            location.Items.Clear();
            if (Request.QueryString["locationtype"] == "0")
            {
                DLC.LocationList(location, 0);
            }
            else if (Request.QueryString["locationtype"] == "1")
            {
                DLC.LocationList(location, 1);
            }
            else if (Request.QueryString["locationtype"] == "2")
            {
                DLC.LocationList(location, 2);
            }

            //cabang.SelectedValue = Request.QueryString["cabang"];
            locationtype.SelectedValue = Request.QueryString["locationtype"];
            location.SelectedValue = Request.QueryString["location"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            brand.SelectedValue = Request.QueryString["brand"];
            consignment.SelectedValue = Request.QueryString["consignment"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        //href += "&cabang=" + cabang.Text;
        href += "&locationtype=" + locationtype.Text;
        href += "&location=" + location.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&brand=" + brand.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&consignment=" + consignment.SelectedValue;

        Response.Redirect(href);
    }
}