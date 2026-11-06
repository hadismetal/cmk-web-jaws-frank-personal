using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //Form-Default
            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Today);

            //DLC
            Ddl.Filter(brand,"CompanyBrand");

            int Tipe = 0;
            try
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }
            catch (Exception ex) { }

            DLC.LocationList(lokasi,Tipe);            
            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;

            string TipeLokasi = "9";
            string IDLokasi = "0";
            string Lokasi = string.Empty;


            if (d.Count() > 0)
            {
                var x = d.First();
                tipelokasi.Items.Clear();

                string namaLokasi = "";
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
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
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
        DateTime start = datepicker.Text == "" ? DateTime.Now : Convert.ToDateTime(datepicker.Text);
        DateTime end = datepicker2.Text == "" ? DateTime.Now : Convert.ToDateTime(datepicker2.Text);;

        string getOutgoingDJ = @"
            DECLARE @TipeAE int = 9                                

            CREATE TABLE #TempLocAsal(
                IDLokasi int, 
                TipeLokasi int, 
                NamaLokasi Varchar(100)
            );

            IF EXISTS(select * from DataAdminSales where IDUser=@IDUser and Disable = 0)
                BEGIN
                    select @TipeAE = TipeLokasi from DataAdminSales where IDUser=@IDUser and Disable = 0
                    
                    IF @TipeAE = 0
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select ID, 0 TipeLokasi, Nama from LocWarehouse where Disable=0 
                    END
                    ELSE IF @TipeAE = 1
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select ID, 1 TipeLokasi, Nama from LocOutlet where Disable=0 
                    END
                    ELSE IF @TipeAE = 2
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select ID, 2 TipeLokasi, Nama from LocExhibition where Disable=0 
                    END		
                END
            ELSE
                BEGIN
                    INSERT INTO #TempLocAsal
                    SELECT ID, TipeLokasi, Nama
                    from (
                        select ID, 0 TipeLokasi, Nama from LocWarehouse where Disable=0 
                        UNION
                        select ID, 1 TipeLokasi, Nama from LocOutlet where Disable=0 
                        UNION
                        select ID, 2 TipeLokasi, Nama from LocExhibition where Disable=0 
                    )ax
                END

                    SELECT ID IDLokasi, TipeLokasi, Nama NamaLokasi into #TempTujuan
                    from (
                        select ID, 0 TipeLokasi, Nama from LocWarehouse where Disable=0 
                        UNION
                        select ID, 1 TipeLokasi, Nama from LocOutlet where Disable=0 
                        UNION
                        select ID, 2 TipeLokasi, Nama from LocExhibition where Disable=0 
                    )ax	


            select a.ID IDKRM, a.IDAsal, aa.TipeLokasi TipeAsal,aa.NamaLokasi NamaAsal ,
            a.IDTujuan, ab.TipeLokasi TipeTujuan, ab.NamaLokasi NamaTujuan, a.Keterangan, COUNT(b.IDProduct) TotalItems, a.StatusTransOut
            , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
            from StockOutgoingDJ a
            JOIN StockOutgoingDJ_Product b on b.IDForm=a.ID
            JOIN StockProductDJ c on c.ID=b.IDProduct
            JOIN StockProductDJ_CharProduct ca on ca.ID=c.ID
            JOIN StockActualDJ d on d.IDProduct=c.ID
            JOIN #TempLocAsal aa on aa.IDLokasi=a.IDAsal and aa.TipeLokasi=a.TipeAsal
            JOIN #TempTujuan ab on ab.IDLokasi = a.IDTujuan and ab.TipeLokasi=a.TipeTujuan
            where A.StatusTransOut = 1 AND (@IDBrand = 0 or a.IDBrand = @IDBrand)
            AND (@TipeAE <> 9 and a.TipeAsal = @TipeAE and (@TipeAsal < 0 or a.TipeAsal = @TipeAsal) OR (@TipeAsal < 0 or a.TipeAsal = @TipeAsal) )
            AND (@IDAsal <= 0 or d.IDLokasi = @IDAsal) AND (a.Tgl BETWEEN @StartDate and @EndDate)
            GROUP BY a.ID,a.IDAsal,AA.TipeLokasi,AA.NamaLokasi,A.IDTujuan,AB.TipeLokasi,AB.NamaLokasi, A.Keterangan,A.StatusTransOut,
            A.Tgl,A.Operator,a.OperatorTgl
            ORDER BY a.OperatorTgl ASC

            DROP TABLE #TempLocAsal,#TempTujuan
            ";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(getOutgoingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@IDUser", App.UserID);
                cmd.Parameters.AddWithValue("@IDBrand", vB);
                cmd.Parameters.AddWithValue("@TipeAsal", Tipe);
                cmd.Parameters.AddWithValue("@IDAsal", Lokasi);
                cmd.Parameters.AddWithValue("@StartDate", start);
                cmd.Parameters.AddWithValue("@EndDate", end);
                
                conn.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable data = ds1.Tables[0];
                conn.Close();


                int vF = 0; // id from
                int vT = 0; // id to
                int tF = 0; // value from
                int tT = 0; // value to

                int index = 1;                

                if (data.Rows.Count == 0) { App.TableNoResult(tb); }
                else
                {
                    foreach (DataRow item in data.Rows)
                    {
                        if (!Response.IsClientConnected) break;

                        vF = Convert.ToInt32(item["IDAsal"]); // id from
                        vT = Convert.ToInt32(item["IDTujuan"]); // id to
                        tF = Convert.ToInt32(item["TipeAsal"]); // value from
                        tT = Convert.ToInt32(item["TipeTujuan"]); // value to

                        //Sub-Header
                        //if (prev != r1.IDBrand)
                        //{
                        //    Sub_Header(vT, r1.ID);
                        //    prev = r1.IDBrand;
                        //}

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(Convert.ToInt32(item["IDKRM"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++ + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(tF) + "<br>" + TD_Nama(tF, vF) + "<br>" + TD_Addr(tF, vF) + "<br>" + TD_Group(tF, vF);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(tT) + "<br>" + TD_Nama(tT, vT) + "<br>" + TD_Addr(tT, vT) + "<br>" + TD_Group(tT, vT);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.HtmlBreak(Convert.ToString(item["Keterangan"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToInt32(item["TotalItems"]).ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.OutgoingApprovalStatus(Convert.ToInt32(item["StatusTransOut"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToString(item["Operator"]) + "<br>" + Convert.ToString(item["Tgl"]);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
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

        //SELECT
        var rs = (from p in db.StockOutgoingDJ
                  where ((vB > 0) ? p.IDBrand == Convert.ToInt32(brand.SelectedValue) : 1 == 1) && p.StatusTransOut == 1 // 1 : New
                  select p);

        try
        {
            int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
            int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

            if (Tipe == 1 || Tipe == 2 || Tipe == 0)
            {

                rs = rs.Where(p => p.IDAsal == Lokasi && p.TipeAsal == Tipe);
            }
        }
        catch (Exception ex) { }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && d.Any(q => q.IDLokasi == p.IDAsal))
                 //|| (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 //|| (p.TipeTujuan == f.First().TipeLokasi && p.IDTujuan == f.First().IDLokasi)
                 select p;
        }

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
           
        }
         start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            //rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1));
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));

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
                var rsI = from p in db.StockOutgoingDJ_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.IDAsal; // id from
                vT = item.IDTujuan; // id to
                tF = item.TipeAsal; // value from
                tT = item.TipeTujuan; // value to

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
                td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF) + "<br>" + TD_Addr(tF, vF) + "<br>" + TD_Group(tF, vF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + "<br>" + TD_Addr(tT, vT) + "<br>" + TD_Group(tT, vT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
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
    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
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