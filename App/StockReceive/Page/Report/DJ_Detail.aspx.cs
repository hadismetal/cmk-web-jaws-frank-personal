using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
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
    protected int uid { get { return App.UserID; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            allc_location_type.Items.Clear();
            allc_location_type.Items.Add(new ListItem("All Location Type", "0"));
            allc_location_type.Items.Add(new ListItem("Warehouse", "1"));
            allc_location_type.Items.Add(new ListItem("Outlet", "2"));
            allc_location_type.Items.Add(new ListItem("Exhibition", "3"));
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.StockKeeperDJ(sk);
            DLC.CompanyBrand(brand);
            DLC.LocationList(cabang, tipe);

            DLC.GroupSupplier(suppliergroup);
            FillSup(sender, e);
            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);
           
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }


    protected void FillSup(object sender, EventArgs e)
    {
        int group= Convert.ToInt32(suppliergroup.SelectedValue);
        if (group > 0)
        {

            supplier.Items.Clear();
            supplier.Items.Add(new ListItem("All Supplier", "0"));
            int sgroup = Convert.ToInt32(suppliergroup.SelectedValue);
            var q1 = from q in db.DataSupplier where q.IDGroup == sgroup select q;

            foreach (var q2 in q1)
            {
                supplier.Items.Add(new ListItem(q2.Nama, q2.ID.ToString()));
            }
            
        }
        else
        {
            supplier.Items.Clear();
            supplier.Items.Add(new ListItem("All Supplier", "0"));
            var q2 = from q in db.DataSupplier select q;

            foreach (var q3 in q2)
            {
                supplier.Items.Add(new ListItem(q3.Nama,q3.ID.ToString()));
            }
        }
        
    }



    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = ""; // tipelokasifrom.SelectedValue; // tipe lokasi from : W, O, E
        String s_kF = ""; // lokasifrom.SelectedValue; // id lokasi from
        String s_tT = ""; // tipelokasito.SelectedValue; // tipe lokasi to : W, O, E
        String s_kT = cabang.SelectedValue; // id lokasi to
        String s_vB = brand.SelectedValue; // brand company
        String s_stats = ""; // status.SelectedValue; // status data
        String s_pf = datepicker.Text.Trim(); // period from
        String s_pt = datepicker2.Text.Trim(); // period to
        String s_kw = kw.Text.Trim(); // keyword

        String s_ob = orderby.SelectedValue; // orderby 
        String s_sup = Request.QueryString["supplier"]; //supplier.SelectedValue; // supplier 
        String s_sk = sk.SelectedValue; // stock keeper 
        String s_sup_group = Request.QueryString["suppliergroup"];//suppliergroup.SelectedValue; // supplier group

        String s_plevel = productlevel.SelectedValue; // product level
        String s_pitem = productitem.SelectedValue; // product item
        String s_pcategory = productcategory.SelectedValue; // product category

        String s_allc_type = Request.QueryString["allc_type"];
        String s_allc_loc = Request.QueryString["allc_loc"];
        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;


        DateTime start;
        DateTime end;

        /// Fill Table
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        start = Convert.ToDateTime(s_pf);
        end = Convert.ToDateTime(s_pt);

        s_sk = s_sk != "0" ? s_sk : "";

        string spname = "detailDJ_Receive ";
        string query = " exec " + spname
                     + s_vB + ", "
                     + s_kT + ", "
                     + s_pitem + ", "
                     + s_pcategory + ", "
                     + s_plevel + ", "
                     + "'" + s_sk + "', "
                     + s_sup + ", "
                     + s_sup_group + ", "
                     + "'" + s_kw + "', "
                     + "'" + start.ToString("yyyy-MM-dd") + "', "
                     + "'" + end.ToString("yyyy-MM-dd") + "', "
                     + s_ob + ", "
                     + (string.IsNullOrEmpty(s_allc_type) || s_allc_type == "0" ? "-1" : (int.Parse(s_allc_type) - 1).ToString()) + ", "
                     + (string.IsNullOrEmpty(s_allc_loc) || s_allc_loc == "0" ? "-1" : s_allc_loc);

        DataSet ds = new DataSet();
        ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        DataTable dt = ds.Tables["data"];
        DataTable total = ds.Tables["data1"];

        if (dt.Rows.Count == 0) { TableNoResult(tbl_Table); }
        else
        {
            /// header table
            TableHeaderRow htr = new TableHeaderRow();
            TableHeaderCell htd;

            htd = new TableHeaderCell();
            htd.Text = "No.";
            htd.Width = 50;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "No. PLU";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Tanggal Approve";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stock Keeper";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Kode Karet";
            htd.Width = 140;
            htr.Cells.Add(htd);
			
			htd = new TableHeaderCell();
            htd.Text = "Kode Karet (From No Supplier)";
            htd.Width = 140;
            htr.Cells.Add(htd);
			
			htd = new TableHeaderCell();
            htd.Text = "Nomor Pro (From No Supplier)";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Nomor Supplier";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Supplier";
            htd.Width = 180;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Location Type";
            htd.Width = 150;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Request Outlet Name";
            htd.Width = 150;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Dist.";
            htd.Width = 140;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Item";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Category";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Segmen";
            htd.Width = 130;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Qty";
            htd.Width = 90;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Net Weight";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Gross Weight";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Frame Price M";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Frame Price R";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO M";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO M Lot";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "NO R";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "RO M";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "RO R";
            htd.Width = 80;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Finishing M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Finishing R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Ongkos Lain M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Ongkos Lain R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Setting M";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone Setting R";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total M <span class='text-muted'>USD</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total R <span class='text-muted'>USD</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Mark Up <span class='text-muted'>%</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "H. Jual <span class='text-muted'>Rp</span>";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "No. Validasi";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Product Order Nomor";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Parcel";
            htd.Width = 130;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "T. Butir";
            htd.Width = 70;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "T. Carat";
            htd.Width = 70;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "GIA";
            htd.Width = 120;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Unit Price";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone M";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Stone R";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Keterangan";
            htd.Width = 150;
            htr.Cells.Add(htd);

            tbl_Table.Rows.Add(htr);

            int PrevID = 0;
            int index = 0;
            string plu = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                if (!response.IsClientConnected) break;

                decimal AllM = 0m;


                TableRow tr = new TableRow();
                TableCell td;
                if (plu == dr["Nomor"].ToString())
                {
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

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);
                }
                else
                {
                    index++;

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Nomor"].ToString();
                    tr.Cells.Add(td);

                    /*td = new TableCell();
                    td.Text = Str.Tgl(r.Tgl);
                    tr.Cells.Add(td);*/

                    td = new TableCell();
                    td.Text = Str.Tgl(Convert.ToDateTime(dr["ApprovalTgl"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["StockKeeper"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProtoNomor"].ToString();
                    tr.Cells.Add(td);
					
					td = new TableCell();
                    td.Text = dr["KodeKaret"].ToString();
                    tr.Cells.Add(td);
					
					td = new TableCell();
                    td.Text = dr["NomorPro"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["SupplierNomor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["SupplierNama"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();

                    string locType = dr["LocationType"].ToString();

                    if (locType == "0")
                        td.Text = "Warehouse";
                    else if (locType == "1")
                        td.Text = "Outlet";
                    else if (locType == "2")
                        td.Text = "Exhibition";
                    else
                        td.Text = "-";

                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["RequestOutletNama"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["StoneDist"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductItem"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductCategory"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Segment"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NettoWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FrameM"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FrameR"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOMLot"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["NOR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["ROM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["ROR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FinishingM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["FinishingR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["OngkosLainCOGS"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["OngkosLain"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["SettingM"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["SettingR"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["HargaMUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["HargaUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["MU"]).ToString("#,##0", nfi) + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.Text = Convert.ToDecimal(r.StockReceiveDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockReceiveDJ_PricingMU.HargaRupiahRound).ToString("N2");
                    td.Text = Convert.ToDecimal(dr["HargaRupiahRound"]).ToString("#,##0", nfi);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["NoValidasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ProductOrderNomor"].ToString();
                    tr.Cells.Add(td);

                    plu = dr["Nomor"].ToString();
                }
                /*td = new TableCell();
                td.Text = rstone.Nama;
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = dr["Parcel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(dr["TotalButir"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalCarat"]).ToString("N4", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + dr["GIA"].ToString() + "\" target=\"_blank\">" + dr["GIA"].ToString() + "</a>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["HargaSatuan"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalHargaM"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["TotalHargaR"]).ToString("N2", nfi);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Keterangan"].ToString();
                tr.Cells.Add(td);

                tbl_Table.Rows.Add(tr);
            }

            if (total.Rows.Count > 0)
            {
                DataRow dr = total.Rows[0];
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Grand Total</b>";
                td.ColumnSpan = 13;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + dr["Qty"].ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NettoWeight"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FrameM"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FrameR"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOMLot"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["NOR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["ROM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["ROR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FinishingM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["FinishingR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["OngkosLainM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["OngkosLainR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["SettingM"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["SettingR"]).ToString("#,##0", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaMUSD"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaUSD"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaRupiahRound"]).ToString("#,##0", nfi) + "</b>";
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
                td.Text = "<b>" + Convert.ToInt32(dr["TotalButir"]).ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalCarat"]).ToString("N4", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["HargaSatuan"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalHargaM"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(dr["TotalHargaR"]).ToString("N2", nfi) + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);


                tbl_Table.Rows.Add(tr);
            }
        }

    }

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
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            sk.Text = Request.QueryString["sk"];
            brand.SelectedValue = Request.QueryString["brand"];
            suppliergroup.SelectedValue = Request.QueryString["suppliergroup"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&brand=" + brand.Text;
        href += "&suppliergroup=" + suppliergroup.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&allc_type=" + allc_location_type.SelectedValue;
        href += "&allc_loc=" + allc_location.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_DetailExportNew.aspx?id=1";
        
        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&brand=" + brand.Text;
        href += "&suppliergroup=" + suppliergroup.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        Response.Redirect(href);
    }


    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_DetailPrint.aspx?id=1";
        
        //Customize-Qstr
        href += "&uid=" + uid;
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&sk=" + sk.Text;
        href += "&brand=" + brand.Text;
        href += "&suppliergroup=" + suppliergroup.SelectedValue;
        href += "&supplier=" + supplier.SelectedValue;
        
        Response.Redirect(href);
    }

    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td.Text = "No Data";
        //td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }

    private DataSet customExec(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
        sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
        DataSet objDS = new DataSet();
        sqlAdapter.Fill(objDS, "data");
        sqlCnn.Close();

        return objDS;
    }

    protected void allc_location_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = allc_location_type.SelectedValue;
        DataTable dt = new DataTable();

        if (type == "1") // Warehouse
            dt = Db.Rs("SELECT ID, Nama FROM locwarehouse", Db.Cnn_DB);
        else if (type == "2") // Outlet
            dt = Db.Rs("SELECT ID, Nama FROM locoutlet", Db.Cnn_DB);
        else if (type == "3") // Exhibition
            dt = Db.Rs("SELECT ID, Nama FROM locexhibition", Db.Cnn_DB);

        allc_location.DataSource = dt;
        allc_location.DataTextField = "Nama";
        allc_location.DataValueField = "ID";
        allc_location.DataBind();

        allc_location.Items.Insert(0, new ListItem("All Location", "0"));
    }
}