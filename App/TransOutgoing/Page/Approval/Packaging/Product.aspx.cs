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

            if (Request.QueryString["delete"] == "1")
            {
                deleteOutgoing(Request.QueryString["id"]);
            }

            tb.Visible = false;

            //DLC
            DLC.CompanyBrand(brand);

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

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];

        }
    }
    private void deleteOutgoing(string ID)
    {
        string query = "DELETE StockOutgoingPackaging_Product WHERE IDForm = " + ID;
        Db.Execute(query, Db.Cnn_DB);

        query = "DELETE StockOutgoingPackaging WHERE ID = " + ID;
        Db.Execute(query, Db.Cnn_DB);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        //int vB = 1;

        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

        //SELECT
        /*var rs = (from p in db.StockOutgoingPackagingDJ
                  where (p.IDBrand == vB || (p.IDBrand > 0 && vB == 0)) && p.StatusTransOut == 1 // 1 : New
                  select p).OrderBy(p => p.OperatorTgl);*/

        var rs = (from p in db.StockOutgoingPackaging
                  where (p.IDBrand == vB || (p.IDBrand > 0 && vB == 0)) && p.StatusTransOut == 1 // 1 : New
                  select p);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 || (p.TipeTujuan == f.First().TipeLokasi && p.IDTujuan == f.First().IDLokasi)
                 select p;
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
                var rsI = from p in db.StockOutgoingPackaging_Product where p.IDForm == item.ID select p;

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
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE        
        int vB = Convert.ToInt32(brand.SelectedValue);        

        string getOutgoingPK = @"
            DECLARE @TipeAE int = 9 , @IDAsal int = 0                             

            CREATE TABLE #TempLocAsal(
                IDLokasi int, 
                TipeLokasi int, 
                NamaLokasi Varchar(100),
				NamaKode Varchar(100),
				AddrAlamat nvarchar(max)
            );

            IF EXISTS(select * from DataAdminSales where IDUser=@IDUser and Disable = 0)
                BEGIN
                    select @TipeAE = TipeLokasi, @IDAsal = IDLokasi from DataAdminSales where IDUser=@IDUser and Disable = 0
                    
                    IF @TipeAE = 0
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select a.ID, 0 TipeLokasi, a.Nama, NamaKode,AddrAlamat 
						from LocWarehouse a
						JOIN LocWarehouseGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                    END
                    ELSE IF @TipeAE = 1
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select a.ID, 1 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocOutlet a
						JOIN LocOutletGroup b on b.ID=a.IDGroup
						where a.Disable=0
                    END
                    ELSE IF @TipeAE = 2
                    BEGIN
                        INSERT INTO #TempLocAsal
                        select a.ID, 2 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocExhibition a
						JOIN LocExhibitionGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                    END		
                END
            ELSE
                BEGIN
                    INSERT INTO #TempLocAsal
                    SELECT ID, TipeLokasi, Nama,NamaKode,AddrAlamat
                    from (
                        select a.ID, 0 TipeLokasi, a.Nama, NamaKode,AddrAlamat 
						from LocWarehouse a
						JOIN LocWarehouseGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                        UNION
                        select a.ID, 1 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocOutlet a
						JOIN LocOutletGroup b on b.ID=a.IDGroup
						where a.Disable=0
                        UNION
                        select a.ID, 2 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocExhibition a
						JOIN LocExhibitionGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                    )ax
                END

                    SELECT ID IDLokasi, TipeLokasi, Nama NamaLokasi,NamaKode,AddrAlamat into #TempTujuan
                    from (
                        select a.ID, 0 TipeLokasi, a.Nama, NamaKode,AddrAlamat 
						from LocWarehouse a
						JOIN LocWarehouseGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                        UNION
                        select a.ID, 1 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocOutlet a
						JOIN LocOutletGroup b on b.ID=a.IDGroup
						where a.Disable=0
                        UNION
                        select a.ID, 2 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
						from LocExhibition a
						JOIN LocExhibitionGroup b on b.ID=a.IDGroup
						where a.Disable=0 
                    )ax	


            select a.ID IDKRM, a.IDAsal, aa.TipeLokasi TipeAsal,aa.NamaLokasi NamaAsal ,aa.NamaKode KodeAsal,aa.AddrAlamat as AddrAsal
            , a.IDTujuan, ab.TipeLokasi TipeTujuan, ab.NamaLokasi NamaTujuan,ab.NamaKode KodeTujuan,ab.AddrAlamat as AddrTujuan
            , a.Keterangan, b.Total as TotalItems, a.StatusTransOut
            , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
            from StockOutgoingPackaging a
            JOIN (select IDForm, COUNT(IDProduct) Total from StockOutgoingPackaging_Product GROUP BY IDForm) b on b.IDForm=a.ID            
            JOIN #TempLocAsal aa on aa.IDLokasi=a.IDAsal and aa.TipeLokasi=a.TipeAsal
            JOIN #TempTujuan ab on ab.IDLokasi = a.IDTujuan and ab.TipeLokasi=a.TipeTujuan
            where A.StatusTransOut = 1 
            AND (@IDBrand = 0 or a.IDBrand = @IDBrand) AND (@TipeAE = 9 or (@TipeAE <> 9 and a.TipeAsal = @TipeAE and a.IDAsal = @IDAsal))
            ORDER BY a.OperatorTgl ASC

            DROP TABLE #TempLocAsal,#TempTujuan
            ";

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(getOutgoingPK, conn))
            {
                cmd.Parameters.AddWithValue("@IDUser", App.UserID);
                cmd.Parameters.AddWithValue("@IDBrand", vB);
                
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

                        // td = new TableCell();
                        // td.Text = Str.HtmlBreak(Convert.ToString(item["Keterangan"]));
                        // tr.Cells.Add(td);

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

        //WHERE
        string w = "1=1";

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
    
    
    // protected string TD_HyperLink(int ID)
    // {
    //     string x = string.Empty;

    //     x += "<a class=\"btn btn-info\" href=\"Product_Approve.aspx?id=" + ID + "\">";
    //     x += "  <i class=\"icon-ok\"></i> ";
    //     x += "  Select";
    //     x += "</a>";

    //     x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Cancel Transfer, Are you Sure?');\" href=\"Product.aspx?d=1&delete=1&brand=" + brand.SelectedValue + "&id=" + ID + "\">";
    //     x += "  <i class=\"icon-trash\"></i> ";
    //     x += "  ";
    //     x += "</a>";

    //     return x;
    // }

    // protected string TD_Nama(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }
    //     /*else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }*/

    //     return x;
    // }
    // protected string TD_Addr(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //             x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //             x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }
    //     /*else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //         x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }*/

    //     return x;
    // }
    // protected string TD_Group(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
    //     }
    //     /*else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
    //     }*/

    //     return x;
    // }
protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Product_Approve.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Cancel Transfer, Are you Sure?');\" href=\"Product.aspx?d=1&delete=1&brand=" + brand.SelectedValue + "&id=" + ID + "\">";
        x += "  <i class=\"icon-trash\"></i> ";
        x += "  ";
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

}