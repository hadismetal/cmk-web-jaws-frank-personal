using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
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
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductSegmen(productsegmen);
            //DLC.CompanyBrand(brand);

            DLC.LocationList(cabang, tipe);
            Ddl.Substorage(substorage);

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table

    protected void fillTable(object sender, EventArgs e)
    {
        try
        {


            DateTime start;
            DateTime end;
            int Cabang = Convert.ToInt32(cabang.SelectedValue);
            int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
            int ProductItem = Convert.ToInt32(productitem.SelectedValue);
            int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
            int ProductSegmen = Convert.ToInt32(productsegmen.SelectedValue);
            int Substorage = Convert.ToInt32(substorage.SelectedValue);

            int pageNumber = 0;
            int numberOfObjectsPerPage = 30;

            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 0;
            var query = "EXEC dbo.sp_StockInvDJ_hadis " + Substorage + ",'" + kw.Text + "'," + Cabang + "," + tipe + "," + ProductLevel + "," + ProductItem + "," + ProductCategory + "," + ProductSegmen + "," + orderby.SelectedValue + "," + pageNumber + "," + numberOfObjectsPerPage + ",'list',0,0,0";
            DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);


            DataTable Product = ds.Tables[0];
            DataTable totalData = ds.Tables[1];

            var TotalPage = Math.Ceiling(Convert.ToDecimal(totalData.Rows[0]["totalData"]) / numberOfObjectsPerPage);
            if (Cabang > 0)
            {
                string href = App.RefreshURL(this);

                //Customize-Qstr
                href += "&kw=" + kw.Text;
                href += "&cabang=" + cabang.Text;
                href += "&productitem=" + productitem.Text;
                href += "&productcategory=" + productcategory.Text;
                href += "&productlevel=" + productlevel.Text;
                href += "&tipe=" + tipe;
                href += "&order=" + orderby.Text;
                href += "&substorage=" + substorage.SelectedValue;
                href += "&productsegmen=" + productsegmen.SelectedValue;
                href += "&page=" + (pageNumber + 1).ToString();

                string href2 = App.RefreshURL(this);

                //Customize-Qstr
                href2 += "&kw=" + kw.Text;
                href2 += "&cabang=" + cabang.Text;
                href2 += "&productitem=" + productitem.Text;
                href2 += "&productcategory=" + productcategory.Text;
                href2 += "&productlevel=" + productlevel.Text;
                href2 += "&tipe=" + tipe;
                href2 += "&order=" + orderby.Text;
                href2 += "&substorage=" + substorage.SelectedValue;
                href2 += "&productsegmen=" + productsegmen.SelectedValue;
                href2 += "&page=" + (pageNumber - 1).ToString();

                string href3 = App.RefreshURL(this);

                //Customize-Qstr
                href3 += "&kw=" + kw.Text;
                href3 += "&cabang=" + cabang.Text;
                href3 += "&productitem=" + productitem.Text;
                href3 += "&productcategory=" + productcategory.Text;
                href3 += "&productlevel=" + productlevel.Text;
                href3 += "&tipe=" + tipe;
                href3 += "&order=" + orderby.Text;
                href3 += "&substorage=" + substorage.SelectedValue;
                href3 += "&productsegmen=" + productsegmen.SelectedValue;
                href3 += "&page=0";

                string href4 = App.RefreshURL(this);

                //Customize-Qstr
                href4 += "&kw=" + kw.Text;
                href4 += "&cabang=" + cabang.Text;
                href4 += "&productitem=" + productitem.Text;
                href4 += "&productcategory=" + productcategory.Text;
                href4 += "&productlevel=" + productlevel.Text;
                href4 += "&tipe=" + tipe;
                href4 += "&order=" + orderby.Text;
                href4 += "&substorage=" + substorage.SelectedValue;
                href4 += "&productsegmen=" + productsegmen.SelectedValue;
                href4 += "&page=" + (TotalPage - 1);

                string paging = "";

                paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
                paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
                paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
                paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
                paging += "<a href=\"" + href4 + "\">Last</a>";
                paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

                paging_text.Text = paging;

                if (ds.Tables.Count > 0)
                {
                    int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
                    decimal TotalStock = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalExchange = 0m;
                    decimal TotalHarga = 0m;
                    decimal TotalModal = 0m;
                    decimal TotalMAwal = 0m;
                    decimal TotalM = 0m;
                    decimal TotalR = 0m;
                    decimal TotalHJual = 0m;
                    decimal TotalNetWeight = 0m;
                    decimal TotalGrossWeight = 0m;
                    decimal TotalButirAll = 0m;
                    decimal TotalCaratAll = 0m;
                    decimal TotalBunga = 0m;

                    decimal TotalPromo = 0m;

                    foreach (DataRow r in Product.Rows)
                    {
                        decimal AllM = 0m;

                        AllM = Convert.ToDecimal(r["AllM"]);

                        decimal AllMAwal = Convert.ToDecimal(r["AllMAwal"]);

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Hyperlink(r["PLU"].ToString());
                        tr.Cells.Add(td);

                        string KodeKaret;
                        KodeKaret = r["KodeKaret"].ToString();

                        td = new TableCell();
                        td.Text = "<small>" + KodeKaret + "</small>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmProductCategory"].ToString();
                        tr.Cells.Add(td);

                        decimal Jual = 0m;
                        decimal Exchange = 0m;
                        decimal Resell = 0m;

                        if (r["Substorage"].ToString() == "1")
                            Jual = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "2")
                            Resell = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "3")
                            Exchange = Convert.ToDecimal(r["InHand"]);

                        /*td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Exchange.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Resell.ToString("N0");
                        tr.Cells.Add(td);*/

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["InHand"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.SubStorage(Convert.ToInt32(r["Substorage"]));
                        tr.Cells.Add(td);


                        string Lokasi = r["NamaLokasi"].ToString();


                        td = new TableCell();
                        td.Text = Lokasi;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllM).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["ModalUSD"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["HargaRupiahRound"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Netto"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Gross"]).ToString("N2");
                        tr.Cells.Add(td);

                        // var det = (from p in r.StockProductDJ_Stone1A
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone1B
                        //            where p.StatusTerpasang == 1
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone2
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone3
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone4
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone5
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                        decimal TotalButir = 0m;
                        decimal TotalCarat = 0m;
                        decimal umur = 0m;
                        decimal bunga = 0m;

                        TotalButir = Convert.ToInt32(r["TotalButir"]);
                        TotalCarat = Convert.ToDecimal(r["TotalCarat"]);
                        // try
                        // {
                        //     TotalButir = det.Sum(p => p.TotalButir);
                        // }
                        // catch (Exception ex) { }
                        // decimal TotalCarat = 0m;
                        // try
                        // {
                        //     TotalCarat = det.Sum(p => p.TotalCarat);
                        // }
                        // catch (Exception ex) { }

                        td = new TableCell();
                        // td.Text = "0";
                        td.Text = Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["Segmen"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["StoneDist"].ToString(); ;
                        tr.Cells.Add(td);


                        td = new TableCell();
                        td.Text = Convert.ToString(r["Umur"]); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
                        tr.Cells.Add(td);


                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["hargaBunga"]).ToString("N2");
                        // td.Text = Str.Number2(xbunga);
                        tr.Cells.Add(td);


                        td = new TableCell();
                        //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                        td.Text = Convert.ToDecimal(r["Promo"]).ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalStock += Jual;
                        TotalResell += Resell;
                        TotalExchange += Exchange;
                        TotalHarga += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalModal += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalMAwal += AllMAwal;
                        TotalM += AllM;
                        TotalR += Convert.ToDecimal(r["ModalUSD"]);
                        TotalHJual += Convert.ToDecimal(r["HargaRupiahRound"]);
                        TotalNetWeight += Convert.ToDecimal(r["Netto"]);
                        TotalGrossWeight += Convert.ToDecimal(r["Gross"]);
                        TotalButirAll += TotalButir;
                        TotalCaratAll += TotalCarat;
                        TotalBunga += Convert.ToDecimal(r["hargaBunga"]);

                        TotalPromo += Convert.ToDecimal(r["Promo"]);
                        index++;

                    }

                    /// Totalan
                    if (ds.Tables.Count > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = "<b>Total</b>";
                        td.ColumnSpan = 5;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "1";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
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
                        td.Text = "<b>" + Str.Number2(TotalBunga);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + Str.Number0(TotalPromo);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }

                }
                else
                {

                    int index = 1;
                    int count = 1;
                    decimal TotalStock = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalExchange = 0m;
                    decimal TotalHarga = 0m;
                    decimal TotalModal = 0m;
                    decimal TotalMAwal = 0m;
                    decimal TotalM = 0m;
                    decimal TotalR = 0m;
                    decimal TotalHJual = 0m;
                    decimal TotalNetWeight = 0m;
                    decimal TotalGrossWeight = 0m;
                    decimal TotalButirAll = 0m;
                    decimal TotalCaratAll = 0m;
                    decimal TotalBunga = 0m;
                    decimal TotalPromo = 0m;
                    int Qty = 0;

                    string prev = "";
                    foreach (DataRow r in Product.Rows)
                    {
                        decimal AllM = 0m;

                        AllM = Convert.ToDecimal(r["AllM"]);

                        decimal AllMAwal = AllM;

                        //Sub-Header
                        if (prev != tipe.ToString() + " " + r["IDLokasi"].ToString())
                        {
                            if (count > 1)
                            {
                                TableRow tr2 = new TableRow();
                                TableCell td2;

                                td2 = new TableCell();
                                td2.Text = "<b>Total</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = Qty.ToString("N0");
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + Str.Number2(TotalBunga) + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + Str.Number2(TotalPromo) + "</b>";
                                tr2.Cells.Add(td2);


                                tb.Rows.Add(tr2);
                                Qty = 0;
                            }

                            Sub_Header(tipe, Convert.ToInt32(r["IDLokasi"]));
                            prev = tipe.ToString() + " " + r["IDLokasi"].ToString();

                            index = 1;
                            TotalStock = 0m;
                            TotalResell = 0m;
                            TotalExchange = 0m;
                            TotalHarga = 0m;
                            TotalModal = 0m;
                            TotalM = 0m;
                            TotalR = 0m;
                            TotalHJual = 0m;
                            TotalNetWeight = 0m;
                            TotalGrossWeight = 0m;
                            TotalButirAll = 0m;
                            TotalCaratAll = 0m;
                            TotalBunga = 0m;
                        }

                        count++;

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Hyperlink(r["PLU"].ToString());
                        tr.Cells.Add(td);

                        string KodeKaret;
                        KodeKaret = (r["KodeKaret"].ToString() != null) ? r["KodeKaret"].ToString() : "";

                        td = new TableCell();
                        td.Text = "<small>" + KodeKaret + "</small>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmProductCategory"].ToString();
                        tr.Cells.Add(td);

                        decimal Jual = 0m;
                        decimal Exchange = 0m;
                        decimal Resell = 0m;

                        if (r["Substorage"].ToString() == "1")
                            Jual = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "2")
                            Resell = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "3")
                            Exchange = Convert.ToDecimal(r["InHand"]);

                        /*td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Exchange.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Resell.ToString("N0");
                        tr.Cells.Add(td);*/

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["InHand"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.SubStorage(Convert.ToInt32(r["Substorage"]));
                        tr.Cells.Add(td);

                        /*td = new TableCell();
                        td.Text = r.StockActualDJ.InHand.ToString("N0");
                        tr.Cells.Add(td);*/

                        string Lokasi = r["NamaLokasi"].ToString();

                        td = new TableCell();
                        td.Text = Lokasi;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllM).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["ModalUSD"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["HargaRupiahRound"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Netto"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Gross"]).ToString("N2");
                        tr.Cells.Add(td);

                        // var det = (from p in r.StockProductDJ_Stone1A
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone1B
                        //            where p.StatusTerpasang == 1
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone2
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone3
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone4
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                        //            from p in r.StockProductDJ_Stone5
                        //            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                        decimal TotalButir = 0m;
                        decimal TotalCarat = 0m;
                        decimal umur = 0m;
                        decimal bunga = 0m;
                        // try
                        // {
                        //     TotalButir = det.Sum(p => p.TotalButir);
                        // }
                        // catch (Exception ex) { }
                        // decimal TotalCarat = 0m;
                        // try
                        // {
                        //     TotalCarat = det.Sum(p => p.TotalCarat);
                        // }
                        // catch (Exception ex) { }

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(TotalButir).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalCarat.ToString("N3");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["Segmen"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["StoneDist"].ToString(); ;
                        tr.Cells.Add(td);

                        //umur
                        umur = Convert.ToDecimal(r["Umur"]);
                        td = new TableCell();
                        td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
                        tr.Cells.Add(td);

                        // var bg = from p in db.BungaBank
                        //          select p;

                        // decimal xbunga = 0m;
                        // if (bg.Count() > 0)
                        // {
                        //     xbunga = Convert.ToDecimal(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);

                        //     //bunga
                        //     bunga += xbunga;
                        // }

                        td = new TableCell();
                        td.Text = Str.Number2(Convert.ToDecimal(r["hargaBunga"]));
                        tr.Cells.Add(td);



                        /// Promo 
                        // var pdj = (from p in db.PromoDJ_Detail
                        //            where p.IDProduct == r.ID && p.PromoDJ.TglStart < System.DateTime.Now
                        //            orderby p.PromoDJ.ApprovalTgl ascending
                        //            select p).FirstOrDefault();

                        td = new TableCell();
                        //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                        td.Text = Convert.ToDecimal(r["Promo"]).ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalStock += Jual;
                        TotalResell += Resell;
                        TotalExchange += Exchange;
                        TotalHarga += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalModal += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalMAwal += AllMAwal;
                        TotalM += AllM;
                        TotalR += Convert.ToDecimal(r["ModalUSD"]);
                        TotalHJual += Convert.ToDecimal(r["HargaRupiahRound"]);
                        TotalNetWeight += Convert.ToDecimal(r["Netto"]);
                        TotalGrossWeight += Convert.ToDecimal(r["Gross"]);
                        TotalButirAll += TotalButir;
                        TotalCaratAll += TotalCarat;
                        TotalBunga += Convert.ToDecimal(r["hargaBunga"]);

                        TotalPromo += Convert.ToDecimal(r["PromoHarga"]);
                        Qty++;
                        index++;

                    }
                }

            }
            else //all
            {
                if (ds.Tables.Count > 0)
                {
                    int index = 1;
                    int count = 1;
                    decimal TotalStock = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalExchange = 0m;
                    decimal TotalHarga = 0m;
                    decimal TotalModal = 0m;
                    decimal TotalMAwal = 0m;
                    decimal TotalM = 0m;
                    decimal TotalR = 0m;
                    decimal TotalHJual = 0m;
                    decimal TotalNetWeight = 0m;
                    decimal TotalGrossWeight = 0m;
                    decimal TotalButirAll = 0m;
                    decimal TotalCaratAll = 0m;
                    decimal TotalBunga = 0m;
                    decimal TotalPromo = 0m;
                    int Qty = 0;

                    string prev = "";
                    foreach (DataRow r in Product.Rows)
                    {
                        decimal AllM = 0m;



                        AllM = Convert.ToDecimal(r["AllM"]);

                        decimal AllMAwal = AllM;


                        //Sub-Header
                        if (prev != tipe.ToString() + " " + r["IDLokasi"].ToString())
                        {
                            if (count > 1)
                            {
                                TableRow tr2 = new TableRow();
                                TableCell td2;

                                td2 = new TableCell();
                                td2.Text = "<b>Total</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = Qty.ToString("N0");
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + Str.Number2(TotalBunga) + "</b>";
                                tr2.Cells.Add(td2);

                                td2 = new TableCell();
                                td2.Text = "<b>" + Str.Number2(TotalPromo) + "</b>";
                                tr2.Cells.Add(td2);


                                tb.Rows.Add(tr2);
                                Qty = 0;
                            }

                            Sub_Header(tipe, Convert.ToInt32(r["IDLokasi"]));
                            prev = tipe.ToString() + " " + r["IDLokasi"].ToString();

                            index = 1;
                            TotalStock = 0m;
                            TotalResell = 0m;
                            TotalExchange = 0m;
                            TotalHarga = 0m;
                            TotalModal = 0m;
                            TotalM = 0m;
                            TotalR = 0m;
                            TotalHJual = 0m;
                            TotalNetWeight = 0m;
                            TotalGrossWeight = 0m;
                            TotalButirAll = 0m;
                            TotalCaratAll = 0m;
                            TotalBunga = 0m;
                        }

                        count++;

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Hyperlink(r["PLU"].ToString());
                        tr.Cells.Add(td);

                        string KodeKaret;
                        KodeKaret = (r["KodeKaret"].ToString() != null) ? r["KodeKaret"].ToString() : "";

                        td = new TableCell();
                        td.Text = "<small>" + KodeKaret + "</small>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["nmProductCategory"].ToString();
                        tr.Cells.Add(td);

                        decimal Jual = 0m;
                        decimal Exchange = 0m;
                        decimal Resell = 0m;

                        if (r["Substorage"].ToString() == "1")
                            Jual = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "2")
                            Resell = Convert.ToDecimal(r["InHand"]);
                        else if (r["Substorage"].ToString() == "3")
                            Exchange = Convert.ToDecimal(r["InHand"]);

                        /*td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Exchange.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Resell.ToString("N0");
                        tr.Cells.Add(td);*/

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["InHand"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.SubStorage(Convert.ToInt32(r["Substorage"]));
                        tr.Cells.Add(td);

                        /*td = new TableCell();
                        td.Text = r.StockActualDJ.InHand.ToString("N0");
                        tr.Cells.Add(td);*/

                        string Lokasi = r["NamaLokasi"].ToString();

                        td = new TableCell();
                        td.Text = Lokasi;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllM).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["ModalUSD"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["HargaRupiahRound"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Netto"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["Netto"]).ToString("N2");
                        tr.Cells.Add(td);


                        decimal TotalButir = 0m;
                        decimal TotalCarat = 0m;
                        decimal umur = 0m;
                        decimal bunga = 0m;

                        TotalButir = Convert.ToInt32(r["TotalButir"]);
                        TotalCarat = Convert.ToDecimal(r["TotalCarat"]);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["Segmen"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r["StoneDist"].ToString();
                        tr.Cells.Add(td);

                        //umur
                        umur = Convert.ToDecimal(r["Umur"]);
                        td = new TableCell();
                        td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number2(Convert.ToDecimal(r["hargaBunga"]));
                        tr.Cells.Add(td);


                        td = new TableCell();
                        //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                        td.Text = Convert.ToDecimal(r["Promo"]).ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalStock += Jual;
                        TotalResell += Resell;
                        TotalExchange += Exchange;
                        TotalHarga += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalModal += Convert.ToDecimal(r["ModalRupiah"]);
                        TotalMAwal += AllMAwal;
                        TotalM += AllM;
                        TotalR += Convert.ToDecimal(r["ModalUSD"]);
                        TotalHJual += Convert.ToDecimal(r["HargaRupiahRound"]);
                        TotalNetWeight += Convert.ToDecimal(r["Netto"]);
                        TotalGrossWeight += Convert.ToDecimal(r["Gross"]);
                        TotalButirAll += TotalButir;
                        TotalCaratAll += TotalCarat;
                        TotalBunga += Convert.ToDecimal(r["hargaBunga"]);

                        TotalPromo += Convert.ToDecimal(r["Promo"]);
                        Qty++;
                        index++;

                    }
                }
            }
        }
        catch
        {
            db.Dispose();
        }

    }


    // protected void fillTable(object sender, EventArgs e)        
    // {
    //     DateTime start;
    //     DateTime end;

    //     var rs = from p in db.StockProductDJ
    //              where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value && p.StockActualDJ.IDLokasi > 0  //&& p.Legacy == null 
    //              && p.StockActualDJ.IDLokasi > 0
    //                        && !(from s in db.SalesOrderDJ
    //                             where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
    //                            select s.IDProduct).Contains(p.ID)
    //                        && !(from s in db.SalesOrderSouvenir
    //                             where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
    //                             select s.IDProductSouvenir).Contains(p.ID)
    //              select p;

    //     rs = rs.OrderByDescending(p => p.ApprovalTgl);

    //     /*if (datepicker.Text != "" && datepicker2.Text != "")
    //     {
    //         start = Convert.ToDateTime(datepicker.Text);
    //         end = Convert.ToDateTime(datepicker2.Text);
    //         rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
    //     }*/

    //     int Substorage = Convert.ToInt32(substorage.SelectedValue);
    //     if (Substorage > 0)
    //     {
    //         rs = rs.Where(p => p.StockActualDJ.Substorage == Substorage);
    //     }

    //     if (kw.Text != "")
    //     {
    //         rs = from p in rs
    //              where p.Nomor.Contains(kw.Text) || p.ProtoNomor.Contains(kw.Text)
    //              select p;
    //     }
    //     int Cabang = Convert.ToInt32(cabang.SelectedValue);
    //     if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
    //     else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
    //     int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
    //     if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
    //     int ProductItem = Convert.ToInt32(productitem.SelectedValue);
    //     if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
    //     int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
    //     if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
    //     int ProductSegmen = Convert.ToInt32(productsegmen.SelectedValue);
    //     if (ProductSegmen > 0) rs = rs.Where(p => p.StockProductDJ_PricingSegmen.ProductSegmen == ProductSegmen);
    //     //int Brand = Convert.ToInt32(brand.SelectedValue);
    //     //if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

    //     if (orderby.SelectedValue == "1")
    //     {
    //         if (tipe == 0)
    //         {
    //             rs = from p in rs
    //                  join q in db.LocWarehouse on p.StockActualDJ.IDLokasi equals q.ID
    //                  orderby q.Nama
    //                  select p;
    //         }
    //         else if(tipe == 1)
    //         {
    //             rs = from p in rs
    //                  join q in db.LocOutlet on p.StockActualDJ.IDLokasi equals q.ID
    //                  orderby q.Nama
    //                  select p;
    //         }
    //         else if (tipe == 2)
    //         {
    //             rs = from p in rs
    //                  join q in db.LocExhibition on p.StockActualDJ.IDLokasi equals q.ID
    //                  orderby q.Nama
    //                  select p;
    //         }
    //     }
    //     else if (orderby.SelectedValue == "2")
    //         rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
    //     else if (orderby.SelectedValue == "3")
    //         rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
    //     else if (orderby.SelectedValue == "4")
    //         rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);
    //     else
    //     {

    //     }


    //     /// Master Brand
    //     var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocWarehouse
    //     var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocOutlet
    //     var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

    //     /// Master Lokasi LocExhibition
    //     var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);


    //     if (Cabang > 0)
    //     {
    //         int pageNumber = 0;
    //         int numberOfObjectsPerPage = 30;

    //         if (Request.QueryString["page"] != "")
    //             pageNumber = Convert.ToInt32(Request.QueryString["page"]);
    //         if (pageNumber < 0)
    //             pageNumber = 0;

    //         decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

    //         TotalPage = Math.Ceiling(TotalPage);

    //         rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

    //         string href = App.RefreshURL(this);

    //         //Customize-Qstr
    //         href += "&kw=" + kw.Text;
    //         href += "&cabang=" + cabang.Text;
    //         href += "&productitem=" + productitem.Text;
    //         href += "&productcategory=" + productcategory.Text;
    //         href += "&productlevel=" + productlevel.Text;
    //         href += "&tipe=" + tipe;
    //         href += "&order=" + orderby.Text;
    //         href += "&substorage=" + substorage.SelectedValue;
    //         href += "&productsegmen=" + productsegmen.SelectedValue;
    //         href += "&page=" + (pageNumber + 1).ToString();

    //         string href2 = App.RefreshURL(this);

    //         //Customize-Qstr
    //         href2 += "&kw=" + kw.Text;
    //         href2 += "&cabang=" + cabang.Text;
    //         href2 += "&productitem=" + productitem.Text;
    //         href2 += "&productcategory=" + productcategory.Text;
    //         href2 += "&productlevel=" + productlevel.Text;
    //         href2 += "&tipe=" + tipe;
    //         href2 += "&order=" + orderby.Text;
    //         href2 += "&substorage=" + substorage.SelectedValue;
    //         href2 += "&productsegmen=" + productsegmen.SelectedValue;
    //         href2 += "&page=" + (pageNumber - 1).ToString();

    //         string href3 = App.RefreshURL(this);

    //         //Customize-Qstr
    //         href3 += "&kw=" + kw.Text;
    //         href3 += "&cabang=" + cabang.Text;
    //         href3 += "&productitem=" + productitem.Text;
    //         href3 += "&productcategory=" + productcategory.Text;
    //         href3 += "&productlevel=" + productlevel.Text;
    //         href3 += "&tipe=" + tipe;
    //         href3 += "&order=" + orderby.Text;
    //         href3 += "&substorage=" + substorage.SelectedValue;
    //         href3 += "&productsegmen=" + productsegmen.SelectedValue;
    //         href3 += "&page=0";

    //         string href4 = App.RefreshURL(this);

    //         //Customize-Qstr
    //         href4 += "&kw=" + kw.Text;
    //         href4 += "&cabang=" + cabang.Text;
    //         href4 += "&productitem=" + productitem.Text;
    //         href4 += "&productcategory=" + productcategory.Text;
    //         href4 += "&productlevel=" + productlevel.Text;
    //         href4 += "&tipe=" + tipe;
    //         href4 += "&order=" + orderby.Text;
    //         href4 += "&substorage=" + substorage.SelectedValue;
    //         href4 += "&productsegmen=" + productsegmen.SelectedValue;
    //         href4 += "&page=" + (TotalPage-1);

    //         string paging = "";

    //         paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
    //         paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
    //         paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
    //         paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
    //         paging += "<a href=\"" + href4 + "\">Last</a>";
    //         paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

    //         paging_text.Text = paging;

    //         if (rs != null)
    //         {
    //             int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
    //             decimal TotalStock = 0m;
    //             decimal TotalResell = 0m;
    //             decimal TotalExchange = 0m;
    //             decimal TotalHarga = 0m;
    //             decimal TotalModal = 0m;
    //             decimal TotalMAwal = 0m;
    //             decimal TotalM = 0m;
    //             decimal TotalR = 0m;
    //             decimal TotalHJual = 0m;
    //             decimal TotalNetWeight = 0m;
    //             decimal TotalGrossWeight = 0m;
    //             decimal TotalButirAll = 0m;
    //             decimal TotalCaratAll = 0m;
    //             decimal TotalBunga = 0m;

    //             decimal TotalPromo = 0m;

    //             foreach (var r in rs)
    //             {
    //                 decimal AllM = 0m;

    //                 var x = r.StockProductDJ_CostingProduct;
    //                 var z = r.StockProductDJ_Costing;

    //                 AllM = (x.Total + z.TotalBiaya);

    //                 decimal AllMAwal = AllM;
    //                 try
    //                 {
    //                     var d = db.StockReceiveDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor));
    //                     var s = d.StockReceiveDJ_CostingProduct;
    //                     var t = d.StockReceiveDJ_Costing;
    //                     AllMAwal = (s.Total + t.TotalBiaya);
    //                 }
    //                 catch { }

    //                 TableRow tr = new TableRow();
    //                 TableCell td;

    //                 td = new TableCell();
    //                 td.Text = index.ToString() + ".";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = TD_Hyperlink(r.Nomor);
    //                 tr.Cells.Add(td);

    //                 string KodeKaret;
    //                 KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

    //                 td = new TableCell();
    //                 td.Text = "<small>" + KodeKaret + "</small>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
    //                 tr.Cells.Add(td);

    //                 decimal Jual = 0m;
    //                 decimal Exchange = 0m;
    //                 decimal Resell = 0m;

    //                 if (r.StockActualDJ.Substorage == 1)
    //                     Jual = r.StockActualDJ.InHand;
    //                 else if (r.StockActualDJ.Substorage == 3)
    //                     Exchange = r.StockActualDJ.InHand;
    //                 else if (r.StockActualDJ.Substorage == 2)
    //                     Resell = r.StockActualDJ.InHand;

    //                 /*td = new TableCell();
    //                 td.Text = Jual.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Exchange.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Resell.ToString("N0");
    //                 tr.Cells.Add(td);*/

    //                 td = new TableCell();
    //                 td.Text = r.StockActualDJ.InHand.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = DL.SubStorage(r.StockActualDJ.Substorage);
    //                 tr.Cells.Add(td);

    //                 /*td = new TableCell();
    //                 td.Text = r.StockActualDJ.InHand.ToString("N0");
    //                 tr.Cells.Add(td);*/

    //                 string Lokasi = "";
    //                 //if (r.StockActualDJ.TipeLokasi == 0)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 //else if (r.StockActualDJ.TipeLokasi == 1)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 //else if (r.StockActualDJ.TipeLokasi == 2)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 if (r.StockActualDJ.TipeLokasi == 0)
    //                     Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
    //                 else if (r.StockActualDJ.TipeLokasi == 1)
    //                     Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
    //                 else if (r.StockActualDJ.TipeLokasi == 2)
    //                     Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];
    //                 td = new TableCell();
    //                 td.Text = Lokasi;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(AllM).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 var det = (from p in r.StockProductDJ_Stone1A
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone1B
    //                            where p.StatusTerpasang == 1
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone2
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone3
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone4
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone5
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

    //                 decimal TotalButir = 0m;
    //                 decimal umur = 0m;
    //                 decimal bunga = 0m;
    //                 try
    //                 {
    //                     TotalButir = det.Sum(p => p.TotalButir);
    //                 }
    //                 catch (Exception ex) { }
    //                 decimal TotalCarat = 0m;
    //                 try
    //                 {
    //                     TotalCarat = det.Sum(p => p.TotalCarat);
    //                 }
    //                 catch (Exception ex) { }

    //                 td = new TableCell();
    //                 td.Text = TotalButir.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = TotalCarat.ToString("N3");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharStoneDist.Nama;
    //                 tr.Cells.Add(td);

    //                 //umur
    //                 if(tipe == 0)
    //                     umur += Convert.ToDecimal((DateTime.Today - r.Tgl.Value).TotalDays);
    //                 else
    //                 {
    //                     DateTime tglmasuk;
    //                     var rslt = from p in db.StockIncomingDJ_Product
    //                                where p.IDProduct == r.ID
    //                                && p.StockIncomingDJ.StatusTransIn == 2
    //                                orderby p.ApprovalTgl descending
    //                                select p;
    //                     var t = rslt.First();
    //                     tglmasuk = t.ApprovalTgl.Value;
    //                     umur += Convert.ToDecimal((DateTime.Today - tglmasuk.Date).TotalDays);
    //                 }


    //                 td = new TableCell();
    //                 td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
    //                 tr.Cells.Add(td);



    //                 var bg = from p in db.BungaBank
    //                          select p;

    //                 decimal xbunga = 0m;
    //                 if (bg.Count() > 0)
    //                 {
    //                     xbunga = Convert.ToDecimal(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);

    //                     //bunga
    //                     bunga += xbunga;
    //                 }

    //                 td = new TableCell();
    //                 td.Text = Str.Number2(xbunga);
    //                 tr.Cells.Add(td);


    //                 /// Promo 
    //                 var pdj = (from p in db.PromoDJ_Detail
    //                            where p.IDProduct == r.ID && p.PromoDJ.TglStart < System.DateTime.Now
    //                            orderby p.PromoDJ.ApprovalTgl ascending
    //                            select p).FirstOrDefault();

    //                 td = new TableCell();
    //                 //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
    //                 td.Text = Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 tb.Rows.Add(tr);

    //                 TotalStock += Jual;
    //                 TotalResell += Resell;
    //                 TotalExchange += Exchange;
    //                 TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
    //                 TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
    //                 TotalMAwal += AllMAwal;
    //                 TotalM += AllM;
    //                 TotalR += r.StockProductDJ_PricingMU.ModalUSD;
    //                 TotalHJual += r.StockProductDJ_PricingMU.HargaRupiahRound.Value;
    //                 TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
    //                 TotalGrossWeight += r.StockProductDJ_CharProduct.GrossWeight;
    //                 TotalButirAll += TotalButir;
    //                 TotalCaratAll += TotalCarat;
    //                 TotalBunga += bunga;

    //                 TotalPromo += Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0);
    //                 index++;

    //             }

    //             /// Totalan
    //             if (rs.Count() > 0)
    //             {
    //                 TableRow tr = new TableRow();
    //                 TableCell td;

    //                 td = new TableCell();
    //                 td.Text = "<b>Total</b>";
    //                 td.ColumnSpan = 5;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = rs.Count().ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalM.ToString("N2") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalR.ToString("N2") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + Str.Number2(TotalBunga);
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = "<b>" + Str.Number0(TotalPromo);
    //                 tr.Cells.Add(td);

    //                 tb.Rows.Add(tr);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if (rs != null)
    //         {
    //             int index = 1;
    //             int count = 1;
    //             decimal TotalStock = 0m;
    //             decimal TotalResell = 0m;
    //             decimal TotalExchange = 0m;
    //             decimal TotalHarga = 0m;
    //             decimal TotalModal = 0m;
    //             decimal TotalMAwal = 0m;
    //             decimal TotalM = 0m;
    //             decimal TotalR = 0m;
    //             decimal TotalHJual = 0m;
    //             decimal TotalNetWeight = 0m;
    //             decimal TotalGrossWeight = 0m;
    //             decimal TotalButirAll = 0m;
    //             decimal TotalCaratAll = 0m;
    //             decimal TotalBunga = 0m;
    //             decimal TotalPromo = 0m;
    //             int Qty = 0;

    //             string prev = "";
    //             foreach (var r in rs)
    //             {
    //                 decimal AllM = 0m;

    //                 var x = r.StockProductDJ_CostingProduct;
    //                 var z = r.StockProductDJ_Costing;

    //                 AllM = (x.Total + z.TotalBiaya);

    //                 decimal AllMAwal = AllM;
    //                 try
    //                 {
    //                     var d = db.StockReceiveDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor));
    //                     var s = d.StockReceiveDJ_CostingProduct;
    //                     var t = d.StockReceiveDJ_Costing;
    //                     AllMAwal = (s.Total + t.TotalBiaya);
    //                 }
    //                 catch { }

    //                 //Sub-Header
    //                 if (prev != tipe.ToString() +  " "  + r.StockActualDJ.IDLokasi.ToString())
    //                 {
    //                     if (count > 1)
    //                     {
    //                         TableRow tr2 = new TableRow();
    //                         TableCell td2;

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>Total</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = Qty.ToString("N0");
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + Str.Number2(TotalBunga) + "</b>";
    //                         tr2.Cells.Add(td2);

    //                         td2 = new TableCell();
    //                         td2.Text = "<b>" + Str.Number2(TotalPromo) + "</b>";
    //                         tr2.Cells.Add(td2);


    //                         tb.Rows.Add(tr2);
    //                         Qty = 0;
    //                     }

    //                     Sub_Header(tipe,r.StockActualDJ.IDLokasi);
    //                     prev = tipe.ToString() + " " + r.StockActualDJ.IDLokasi.ToString();

    //                     index = 1;
    //                     TotalStock = 0m;
    //                     TotalResell = 0m;
    //                     TotalExchange = 0m;
    //                     TotalHarga = 0m;
    //                     TotalModal = 0m;
    //                     TotalM = 0m;
    //                     TotalR = 0m;
    //                     TotalHJual = 0m;
    //                     TotalNetWeight = 0m;
    //                     TotalGrossWeight = 0m;
    //                     TotalButirAll = 0m;
    //                     TotalCaratAll = 0m;
    //                     TotalBunga = 0m;
    //                 }

    //                 count++;

    //                 TableRow tr = new TableRow();
    //                 TableCell td;

    //                 td = new TableCell();
    //                 td.Text = index.ToString() + ".";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = TD_Hyperlink(r.Nomor);
    //                 tr.Cells.Add(td);

    //                 string KodeKaret;
    //                 KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

    //                 td = new TableCell();
    //                 td.Text = "<small>" + KodeKaret + "</small>";
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
    //                 tr.Cells.Add(td);

    //                 decimal Jual = 0m;
    //                 decimal Exchange = 0m;
    //                 decimal Resell = 0m;

    //                 if (r.StockActualDJ.Substorage == 1)
    //                     Jual = r.StockActualDJ.InHand;
    //                 else if (r.StockActualDJ.Substorage == 3)
    //                     Exchange = r.StockActualDJ.InHand;
    //                 else if (r.StockActualDJ.Substorage == 2)
    //                     Resell = r.StockActualDJ.InHand;

    //                 /*td = new TableCell();
    //                 td.Text = Jual.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Exchange.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Resell.ToString("N0");
    //                 tr.Cells.Add(td);*/

    //                 td = new TableCell();
    //                 td.Text = r.StockActualDJ.InHand.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = DL.SubStorage(r.StockActualDJ.Substorage);
    //                 tr.Cells.Add(td);

    //                 /*td = new TableCell();
    //                 td.Text = r.StockActualDJ.InHand.ToString("N0");
    //                 tr.Cells.Add(td);*/

    //                 string Lokasi = "";
    //                 //if (r.StockActualDJ.TipeLokasi == 0)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 //else if (r.StockActualDJ.TipeLokasi == 1)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 //else if (r.StockActualDJ.TipeLokasi == 2)
    //                 //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
    //                 if (r.StockActualDJ.TipeLokasi == 0)
    //                     Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
    //                 else if (r.StockActualDJ.TipeLokasi == 1)
    //                     Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
    //                 else if (r.StockActualDJ.TipeLokasi == 2)
    //                     Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];
    //                 td = new TableCell();
    //                 td.Text = Lokasi;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(AllM).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
    //                 tr.Cells.Add(td);

    //                 var det = (from p in r.StockProductDJ_Stone1A
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone1B
    //                            where p.StatusTerpasang == 1
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone2
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone3
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone4
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
    //                            from p in r.StockProductDJ_Stone5
    //                            select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

    //                 decimal TotalButir = 0m;
    //                 decimal umur = 0m;
    //                 decimal bunga = 0m;
    //                 try
    //                 {
    //                     TotalButir = det.Sum(p => p.TotalButir);
    //                 }
    //                 catch (Exception ex) { }
    //                 decimal TotalCarat = 0m;
    //                 try
    //                 {
    //                     TotalCarat = det.Sum(p => p.TotalCarat);
    //                 }
    //                 catch (Exception ex) { }

    //                 td = new TableCell();
    //                 td.Text = TotalButir.ToString("N0");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = TotalCarat.ToString("N3");
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
    //                 tr.Cells.Add(td);

    //                 td = new TableCell();
    //                 td.Text = r.StockProductDJ_CharProduct.CharStoneDist.Nama;
    //                 tr.Cells.Add(td);

    //                 //umur
    //                 umur += Convert.ToDecimal((DateTime.Today - r.Tgl.Value).TotalDays);
    //                 td = new TableCell();
    //                 td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
    //                 tr.Cells.Add(td);

    //                 var bg = from p in db.BungaBank
    //                          select p;

    //                 decimal xbunga = 0m;
    //                 if (bg.Count() > 0)
    //                 {
    //                     xbunga = Convert.ToDecimal(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);

    //                     //bunga
    //                     bunga += xbunga;
    //                 }

    //                 td = new TableCell();
    //                 td.Text = Str.Number2(xbunga);
    //                 tr.Cells.Add(td);



    //                 /// Promo 
    //                 var pdj = (from p in db.PromoDJ_Detail
    //                            where p.IDProduct == r.ID && p.PromoDJ.TglStart < System.DateTime.Now
    //                            orderby p.PromoDJ.ApprovalTgl ascending
    //                            select p).FirstOrDefault();

    //                 td = new TableCell();
    //                 //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
    //                 td.Text = Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0).ToString("N0");
    //                 tr.Cells.Add(td);


    //                 tb.Rows.Add(tr);

    //                 TotalStock += Jual;
    //                 TotalResell += Resell;
    //                 TotalExchange += Exchange;
    //                 TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
    //                 TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
    //                 TotalMAwal += AllMAwal;
    //                 TotalM += AllM;
    //                 TotalR += r.StockProductDJ_PricingMU.ModalUSD;
    //                 TotalHJual += r.StockProductDJ_PricingMU.HargaRupiahRound.Value;
    //                 TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
    //                 TotalGrossWeight += r.StockProductDJ_CharProduct.GrossWeight;
    //                 TotalButirAll += TotalButir;
    //                 TotalCaratAll += TotalCarat;
    //                 TotalBunga += bunga;

    //                 TotalPromo += Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0);
    //                 Qty++;
    //                 index++;

    //             }
    //         }
    //     }

    // }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            substorage.SelectedValue = Request.QueryString["substorage"];
            productsegmen.SelectedValue = Request.QueryString["productsegmen"];
            //brand.SelectedValue = Request.QueryString["brand"];
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void Sub_Header(int TipeLokasi, int IDLokasi)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        string Lokasi = "";
        if (tipe == 0)
            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 1)
            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 2)
            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + IDLokasi, Db.Cnn_DB);

        x += "<br />";
        x += "<span class=\"text-orange\">";
        x += Lokasi;
        x += "</span>";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected string TD_Hyperlink(string PLU)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='DJ_Detail.aspx?refreshPage=1&kw=" + PLU + "&tipe=" + tipe + "'>" + PLU + "</a>";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productsegmen=" + productsegmen.SelectedValue;
        // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&order=" + orderby.Text;
        href += "&productsegmen=" + productsegmen.SelectedValue;
        //href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productsegmen=" + productsegmen.SelectedValue;
        //href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected DataSet QuerySPDataSet(string query, string connectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlTransaction trans;
            trans = conn.BeginTransaction();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
                sda.SelectCommand.CommandTimeout = 3000;
                sda.Fill(ds);
                sda.Dispose();
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
                trans.Rollback();
                conn.Close();
            }
        }
        return ds;
    }
}