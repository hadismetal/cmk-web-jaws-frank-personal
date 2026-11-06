using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CrossBrand;
using CrossBrand.Model;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
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

    protected string myV { get { return Request.QueryString["valid"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

           

            //DLC
            DLC.CompanyBrand(brand);
            DLC.LocationType(ddl_from, ddl_to);

            //valid transfer
            if (myV == "0")
            {
                lit_valid.Text = "<h4><p class='alert alert-info'>Mapping location not found !!</p></h4>";
                return;
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
        href += "&brand=" + brand.SelectedValue.Trim();
        href += "&from=" + ddl_from.SelectedValue.Trim();
        href += "&to=" + ddl_to.SelectedValue.Trim();
        href += "&kw=" + kw.Text.Trim();
        href += "&keyfrom=" + txt_from.Text.Trim();
        href += "&keyto=" + txt_to.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            ddl_from.SelectedValue = Request.QueryString["from"];
            ddl_to.SelectedValue = Request.QueryString["to"];
            kw.Text = Request.QueryString["kw"];

            txt_from.Text = Request.QueryString["keyfrom"];
            txt_to.Text = Request.QueryString["keyto"];
        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            txt_to.Enabled = false;

            var x = d.First();
            string href = "";

            if (x.TipeLokasi == 1)
            {
                txt_to.Text = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi).Nama;
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
                txt_to.Text = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi).Nama;
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Exhibition", "2"));
            }
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            txt_to.Enabled = false;

            var x = f.First();
            string href = "";


            if (x.TipeLokasi == 0)
            {
                txt_to.Text = db.LocWarehouse.SingleOrDefault(p => p.ID == x.IDLokasi).Nama;
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Warehouse", "0"));
            }
            else if (x.TipeLokasi == 1)
            {
                txt_to.Text = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi).Nama;
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
                txt_to.Text = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi).Nama;
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Exhibition", "2"));
            }
        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;
        bool isUserTujuan = false;
        DataSet ds = new DataSet();
        int vB = Convert.ToInt32(brand.SelectedValue);
        int vF = Convert.ToInt32(ddl_from.SelectedValue);
        int vT = Convert.ToInt32(ddl_to.SelectedValue);
        string kF = txt_from.Text.Trim();
        string kT = txt_to.Text.Trim();
        string keyword = kw.Text.Trim();

        string queryGetCrossBrandOutgoingDJ = @"            

            IF EXISTS(select * from DataAdminSales where IDUser=@IDUser and Disable = 0)
            BEGIN
                select @TipeAE = TipeLokasi
                from DataAdminSales where IDUser=@IDUser and Disable = 0                        
            end

            SELECT IDOutgoing INTO #Exclude 
            FROM CrossBrand_StockIncomingDJ A
            JOIN CrossBrand_StockIncomingDJ_Product B ON B.IDForm=A.ID
            JOIN CrossBrand_StockOutgoingDJ C ON C.ID=A.IDOutgoing
            WHERE a.StatusTransIn > 0 and C.StatusTransOut = 2 AND (@IDBrandAsal = 0 or c.IDBrandAsal = @IDBrandAsal)
        
            
            SELECT ISNULL(C.ID,A.ID) ID, A.Nomor, a.IDBrandAsal,a.IDBrandTujuan,a.TipeAsal,a.TipeTujuan, a.IDAsal, a.IDTujuan
            , a.NamaAsal, a.NamaTujuan , A.NamaKurir, a.Keterangan
            , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
            , COUNT(B.IDProduct) Qty, ISNULL(C.StatusTransIn,9) StatusIn
            FROM CrossBrand_StockOutgoingDJ a 
            JOIN CrossBrand_StockOutgoingDJ_Product b on b.IDForm = a.ID
            LEFT JOIN CrossBrand_StockIncomingDJ c on c.IDOutgoing = a.ID AND c.StatusTransIn = 0
            WHERE a.StatusTransOut = 2 and (@IDBrandAsal = 0 or a.IDBrandAsal = @IDBrandAsal)
            and NOT EXISTS (SELECT IDOutgoing FROM #Exclude where IDOutgoing=a.ID)
            and (@TipeAsal = -1 or a.TipeAsal = @TipeAsal) and (@NamaAsal = '' OR A.NamaAsal LIKE '%'+ @NamaAsal +'%') /*kondisi untuk lokasi Asal*/
            and (@TipeAE = -1 or a.TipeTujuan = @TipeAE) and (@NamaTujuan='' OR A.NamaTujuan LIKE '%'+ @NamaTujuan +'%') /*kondisi untuk lokasi tujuan*/
            and (@Keyword = '' OR a.Nomor LIKE '%'+ @Keyword +'%') and a.IDBrandTujuan = 7
            GROUP BY a.ID, A.Nomor, a.IDBrandAsal,a.IDBrandTujuan,a.TipeAsal,a.TipeTujuan, a.IDAsal, a.IDTujuan
            , a.NamaAsal, a.NamaTujuan , A.NamaKurir, a.Keterangan,a.Operator,a.OperatorTgl, a.Tgl, c.StatusTransIn, c.ID
            ORDER BY a.OperatorTgl ASC

            DROP TABLE #Exclude		
        ";

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(queryGetCrossBrandOutgoingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@TipeAsal", vF);
                cmd.Parameters.AddWithValue("@NamaAsal", kF);
                cmd.Parameters.AddWithValue("@TipeAE", vT);
                cmd.Parameters.AddWithValue("@NamaTujuan", kT);
                cmd.Parameters.AddWithValue("@IDBrandAsal", vB);
                cmd.Parameters.AddWithValue("@IDUser", App.UserID);
                cmd.Parameters.AddWithValue("@Keyword", keyword);
                
                int totalRecord = 0;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
            }
        }
        

        //COUNT FROM RESULT
        int rsc = ds.Tables.Count;
        DataTable dt = ds.Tables[0];

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        if (rsc == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;

        if (isValidFromTo)
        {
            foreach (DataRow item in dt.Rows)
            {

                if (!Response.IsClientConnected) break;

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkNew(Convert.ToInt32(item["ID"]), Convert.ToInt32(item["StatusIn"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)item["Nomor"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak((string)item["Keterangan"]);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToString(item["Qty"]);
                tr.Cells.Add(td);

                int typeAsal = Convert.ToInt32(item["TipeAsal"]);
                int lokasiAsal = Convert.ToInt32(item["IDAsal"]);
                int brandAsal = Convert.ToInt32(item["IDBrandAsal"]);

                td = new TableCell();
                td.Text = DL.LocationType(typeAsal) + TD_Nama(typeAsal, lokasiAsal, brandAsal) + TD_Addr(typeAsal, lokasiAsal, brandAsal) + TD_Group(typeAsal, lokasiAsal, brandAsal);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                int typeTujuan = Convert.ToInt32(item["TipeTujuan"]);
                int lokasiTujuan = Convert.ToInt32(item["IDTujuan"]);
                int brandTujuan = Convert.ToInt32(item["IDBrandTujuan"]);

                td = new TableCell();
                td.Text = DL.LocationType(typeTujuan) + TD_Nama(typeTujuan, lokasiTujuan, brandTujuan) + TD_Addr(typeTujuan, lokasiTujuan, brandTujuan) + TD_Group(typeTujuan, lokasiTujuan, brandTujuan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToString(item["Tgl"]);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
    protected string TD_HyperLinkNew(int id, int StatusIn)
    {
        string x = string.Empty;

        x = "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + id + "\">";    

        if (StatusIn == 0)
        {
            x = "<a class=\"btn btn-info\" href=\"DJ_QC.aspx?id=" + id + "\">";    
        }

        
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;
        bool isUserTujuan = false;


        //WHERE
        string w = "1=1";
        if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
        //if (ddl_from.SelectedIndex > 0) w += " AND TipeAsal = " + ddl_from.SelectedValue;
        //if (ddl_to.SelectedIndex > 0) w += " AND TipeTujuan = " + ddl_to.SelectedValue;

        int vB = Convert.ToInt32(brand.SelectedValue);
        int vF = Convert.ToInt32(ddl_from.SelectedValue);
        int vT = Convert.ToInt32(ddl_to.SelectedValue);
        string kF = txt_from.Text.Trim();
        string kT = txt_to.Text.Trim();

        var idIncoming = cross.IncomingDJ(0, "2025-08-01", "2025-08-05").Where(p => p.CrossBrand_StockOutgoingDJ.StatusTransOut == 2
                         && (p.CrossBrand_StockOutgoingDJ.IDBrandAsal == vB || (p.CrossBrand_StockOutgoingDJ.IDBrandAsal > 0 && vB == 0)));

        //SELECT
        var rs = cross.OutgoingDJ(0, "2025-08-01", "2025-08-05").Where(p => p.StatusTransOut == 2
                 && (p.IDBrandAsal == vB || (p.IDBrandAsal > 0 && vB == 0))
                 && !idIncoming.Any(r => r.IDOutgoing == p.ID));

        //var rs = from p in db.StockOutgoingDJ.Where(w)
        //         where p.StatusTransOut == 2
        //         && !(from z in db.StockIncomingDJ
        //              where z.StatusTransIn == 3
        //              select z.IDOutgoing).Contains(p.ID)// Approved
        //         select p;

        string keyword = kw.Text.Trim();
        if (keyword != "")
            rs = rs.Where(p => p.Nomor.Contains(keyword));

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/
            isUserTujuan = true;
            rs = rs.Where(p => d.Any(q => q.TipeLokasi == p.TipeTujuan) && d.Any(q => q.IDLokasi == p.IDTujuan));
        }

        if (vF > 0)
        {
            rs = rs.Where(p => p.TipeAsal == vF);
        }

        if (!isUserTujuan)
        {
            if (vT > 0)
            {
                rs = rs.Where(p => p.TipeTujuan == vT);
            }
        }

        rs = rs.Where(p => p.NamaAsal.Contains(kF));
        if (!isUserTujuan) rs = rs.Where(p => p.NamaTujuan.Contains(kF));

        rs = rs.OrderBy(p => p.OperatorTgl);


        //COUNT FROM RESULT
        int rsc = rs.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        if (rsc == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;

        if (isValidFromTo)
        {
            foreach (var item in rs)
            {

                if (!Response.IsClientConnected) break;

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
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.CrossBrand_StockOutgoingDJ_Product.Count.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeAsal) + TD_Nama(item.TipeAsal, item.IDAsal, item.IDBrandAsal) + TD_Addr(item.TipeAsal, item.IDAsal, item.IDBrandAsal) + TD_Group(item.TipeAsal, item.IDAsal, item.IDBrandAsal);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + TD_Nama(item.TipeTujuan, item.IDTujuan, item.IDBrandTujuan) + TD_Addr(item.TipeTujuan, item.IDTujuan, item.IDBrandTujuan) + TD_Group(item.TipeTujuan, item.IDTujuan, item.IDBrandTujuan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.Tgl);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

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
    
    protected string TD_HyperLink(int id)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + id + "\">";
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
}