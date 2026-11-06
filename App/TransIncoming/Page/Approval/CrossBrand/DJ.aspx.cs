using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CrossBrand;
using CrossBrand.Model;
using System.Data;
using System.Data.SqlClient;

public partial class DJ : System.Web.UI.Page
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
    Modelize cross = new Modelize();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;
            DateTime now = DateTime.Now;
            DateTime firstDay = new DateTime(now.Year, now.Month, 1);
            datepicker.Text = firstDay.ToString("dd-MM-yyyy");
            datepicker2.Text = DateTime.Now.ToString("dd-MM-yyyy");

            //DLC
            DLC.CompanyBrand(brand);

            string TipeLokasi;
            int Tipe = 0;
            string IDLokasi = "0";
            string namaLokasi = "";

            try
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }
            catch (Exception ex) { }

            DLC.LocationList(lokasi, Tipe);
                      
            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                var x = d.First();
                tipelokasi.Items.Clear();
                if (x.TipeLokasi == 1)
                {
                    TipeLokasi = "1";
                    IDLokasi = Db.SingleString("SELECT Cast(ID as varchar(10)) FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    namaLokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                }
                else if (x.TipeLokasi == 2)
                {
                    TipeLokasi = "2";
                    IDLokasi = Db.SingleString("SELECT Cast(ID as varchar(10)) FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    namaLokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                }
                else
                {
                    TipeLokasi = "0";
                    IDLokasi = Db.SingleString("SELECT Cast(ID as varchar(10)) FROM LocWarehouse WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                    namaLokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
                }

                tipelokasi.Items.Add(new ListItem(TipeLokasi== "0" ?"Warehouse" : TipeLokasi == "1" ? "Outlet" : "Exhibition", TipeLokasi.ToString()));
                tipelokasi.Enabled = false;

                lokasi.Items.Clear();
                lokasi.Items.Add(new ListItem(namaLokasi, IDLokasi));
                lokasi.Enabled = false;
            }

            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;
            if (f.Count() > 0)
            {
                tipelokasi.Items.Clear();
                tipelokasi.Enabled = false;
                lokasi.Items.Clear();
                lokasi.Enabled = false;
            }

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable1(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&brand=" + brand.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);
        DateTime start = Convert.ToDateTime(datepicker.Text);
        DateTime end = Convert.ToDateTime(datepicker2.Text);

        //SELECT    
        string queryGetIncoming = @"
            select a.ID,A.IDOutgoing, c.IDBrandAsal, c.TipeAsal, c.IDAsal
            , c.IDBrandTujuan, c.TipeTujuan, c.IDTujuan, c.Nomor, A.StatusTransIn, a.Keterangan
            , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl
            , COUNT(B.IDProduct) Qty, a.Operator
            FROM CrossBrand_StockIncomingDJ a
            JOIN CrossBrand_StockIncomingDJ_Product b on b.IDForm=a.ID
            JOIN CrossBrand_StockOutgoingDJ c on c.ID = a.IDOutgoing
            WHERE StatusTransIn = 1 
            AND a.OperatorTgl BETWEEN @tglStart AND @tglAkhir
            AND (@IDBrandTujuan <= 0 OR C.IDBrandTujuan = @IDBrandTujuan)
            AND (@TipeTujuan = -1 OR c.TipeTujuan = @TipeTujuan)
            AND (@IDLokasiTujuan = -1 OR c.IDTujuan = @IDLokasiTujuan)
            GROUP BY a.ID,A.IDOutgoing, c.IDBrandAsal, c.TipeAsal, c.IDAsal
            , c.IDBrandTujuan, c.TipeTujuan, c.IDTujuan, c.Nomor, A.StatusTransIn
            , a.Keterangan, a.Tgl, a.Operator
            order by a.id desc";

        DataSet ds = new DataSet();
        

        
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryGetIncoming, conn))
            {
                cmd.Parameters.AddWithValue("@tglStart", start);
                cmd.Parameters.AddWithValue("@tglAkhir", end.AddDays(1));
                cmd.Parameters.AddWithValue("@IDBrandTujuan", vB);
                cmd.Parameters.AddWithValue("@TipeTujuan", Tipe);
                cmd.Parameters.AddWithValue("@IDLokasiTujuan", Lokasi);

                int totalRecord = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
        }

        DataTable dt = ds.Tables[0];
        int vF = 0; // id from
        int vT = 0; // id to
        int tF = 0; // value from
        int tT = 0; // value to
        int bT = 0; // value brand from
        int bF = 0; // value brand to

        int index = 0;
        

        if (ds.Tables.Count == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (DataRow item in dt.Rows)            
            {
                if (!Response.IsClientConnected) break;

                vF = Convert.ToInt32(item["IDAsal"]); // id from
                vT = Convert.ToInt32(item["IDTujuan"]); // id to
                tF = Convert.ToInt32(item["TipeAsal"]); // value from
                tT = Convert.ToInt32(item["TipeTujuan"]); // value to
                bF = Convert.ToInt32(item["IDBrandAsal"]);
                bT = Convert.ToInt32(item["IDBrandTujuan"]);

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                index++;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(item["ID"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + Convert.ToString(item["Nomor"]) + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(tF) + "<br>" + TD_Nama(tF, vF, bF) + "<br>" + TD_Addr(tF, vF, bF) + "<br>" + TD_Group(tF, vF, bF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);                            

                td = new TableCell();
                td.Text = DL.LocationType((int)item["TipeTujuan"]) + "<br>" + TD_Nama(tT, vT, bT) + "<br>" + TD_Addr(tT, vT, bT) + "<br>" + TD_Group(tT, vT, bT);                
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(Convert.ToString(item["Keterangan"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(item["Qty"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(Convert.ToInt32(item["StatusTransIn"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)item["Operator"]+ "<br>" + (string)item["Tgl"];
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }



        //COUNT FROM RESULT
        //int iFW = rsW.Count();
        //int iFO = rsO.Count();
        //int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        

    }
    
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        
        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

        //var rs = (from p in db.StockIncomingDJ
        //          where ((vB > 0) ? p.StockOutgoingDJ.IDBrand == vB : 1 == 1) && p.StatusTransIn == 1 // 1 : New
        //          select p);

        //SELECT
        var rs = cross.IncomingDJ(0,"2025-07-31","2025-08-01").
        Where(p => (p.CrossBrand_StockOutgoingDJ.IDBrandTujuan == vB || (p.CrossBrand_StockOutgoingDJ.IDBrandTujuan > 0 && vB == 0)) && p.StatusTransIn == 1);

        try
        {
            int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
            int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

            if (Tipe == 1 || Tipe == 2 || Tipe == 0)
            {

                rs = rs.Where(p => p.CrossBrand_StockOutgoingDJ.IDTujuan == Lokasi && p.CrossBrand_StockOutgoingDJ.TipeTujuan == Tipe);
            }
        }
        catch (Exception ex) { }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.StockOutgoingPackaging.TipeAsal == d.First().TipeLokasi && p.StockOutgoingPackaging.IDAsal == d.First().IDLokasi)
                 || (p.StockOutgoingPackaging.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingPackaging.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (d.Any(q => q.TipeLokasi == p.CrossBrand_StockOutgoingDJ.TipeTujuan) && d.Any(q => q.IDLokasi == p.CrossBrand_StockOutgoingDJ.IDTujuan))
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {

            rs = from p in rs
                 where (p.CrossBrand_StockOutgoingDJ.TipeTujuan == f.First().TipeLokasi && p.CrossBrand_StockOutgoingDJ.IDTujuan == f.First().IDLokasi)
                 select p;
        }

        rs = rs.OrderBy(p => p.OperatorTgl);

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1));
        }

        rs = rs.OrderBy(p => p.OperatorTgl);

        int vF = 0; // id from
        int vT = 0; // id to
        int tF = 0; // value from
        int tT = 0; // value to

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (var item in rs)
            {
                if (!Response.IsClientConnected) break;

                vF = item.CrossBrand_StockOutgoingDJ.IDAsal; // id from
                vT = item.CrossBrand_StockOutgoingDJ.IDTujuan; // id to
                tF = item.CrossBrand_StockOutgoingDJ.TipeAsal; // value from
                tT = item.CrossBrand_StockOutgoingDJ.TipeTujuan; // value to
                int bT = item.CrossBrand_StockOutgoingDJ.IDBrandTujuan;
                int bF = item.CrossBrand_StockOutgoingDJ.IDBrandAsal;

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(item.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + item.CrossBrand_StockOutgoingDJ.Nomor + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.CrossBrand_StockOutgoingDJ.TipeAsal) + "<br>" + TD_Nama(tF, vF, bF) + "<br>" + TD_Addr(tF, vF, bF) + "<br>" + TD_Group(tF, vF, bF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.CrossBrand_StockOutgoingDJ.TipeTujuan) + "<br>" + TD_Nama(tT, vT, bT) + "<br>" + TD_Addr(tT, vT, bT) + "<br>" + TD_Group(tT, vT, bT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.CrossBrand_StockOutgoingDJ.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.CrossBrand_StockIncomingDJ_Product.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator + "<br>" + Str.FullDate(item.Tgl);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        

        //COUNT FROM RESULT
        //int iFW = rsW.Count();
        //int iFO = rsO.Count();
        //int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        

    }
    

    protected void Sub_Header(int t, int r)
    {
        //TableRow tr = new TableRow();
        //TableCell td;

        //tr.CssClass = "sub-header";
        //string x = string.Empty;

        //if (t == 0)
        //{
        //    var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 1)
        //{
        //    var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 2)
        //{
        //    var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}

        //td = new TableCell();
        //td.Text = x;
        //td.CssClass = "text-lg text-warning";
        //td.ColumnSpan = App.TableColspan(tb);
        //tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Approve.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }

    protected string TD_Nama(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }

        return x;
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2 || Tipe == 0)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}