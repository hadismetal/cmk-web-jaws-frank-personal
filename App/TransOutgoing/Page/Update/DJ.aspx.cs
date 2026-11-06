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
        href += "&kw=" + keyword.Text.Trim();
        href += "&startROW=1";

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            startROW.InnerHtml = Request.QueryString["startROW"];

        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;
        int vB = Convert.ToInt32(Request.QueryString["brand"].ToString());
        string kw = Request.QueryString["kw"].ToString();

        int UserID = App.UserID;
        keyword.Text = kw;

        string startRowValue = startROW.InnerHtml;
        int pageStart = int.Parse(startRowValue);

        int pageSize = 30;

        string getDataOutgoingDJ = @"
            DECLARE  @TipeAE int = 9, @IDAE int = 9
            CREATE TABLE #TempLocAsal(
                IDLokasi int, 
                TipeLokasi int, 
                NamaLokasi Varchar(100)
            );

            IF EXISTS(select * from DataAdminSales where IDUser=@IDUser and Disable = 0)
                BEGIN
                    select @TipeAE = TipeLokasi, @IDAE = IDLokasi from DataAdminSales where IDUser=@IDUser and Disable = 0
                    
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
            LEFT JOIN StockOutgoingDJ_Product b on b.IDForm=a.ID
            LEFT JOIN StockProductDJ c on c.ID=b.IDProduct            
            JOIN #TempLocAsal aa on aa.IDLokasi=a.IDAsal and aa.TipeLokasi=a.TipeAsal
            JOIN #TempTujuan ab on ab.IDLokasi = a.IDTujuan and ab.TipeLokasi=a.TipeTujuan
            where A.StatusTransOut = 0 AND (@IDBrand = -1 or a.IDBrand = @IDBrand)
            AND (@TipeAE  = 9 OR a.TipeAsal = @TipeAE) and StatusTransOut = 0
            AND (@IDAE = 9 or a.IDAsal = @IDAE) AND (@Kw = '' OR (c.Nomor like '%'+@Kw+'%' OR a.NamaAsal like '%'+@Kw+'%' OR a.NamaTujuan like '%'+@Kw+'%'))
            GROUP BY a.ID,a.IDAsal,AA.TipeLokasi,AA.NamaLokasi,A.IDTujuan,AB.TipeLokasi,AB.NamaLokasi, A.Keterangan,A.StatusTransOut,
            A.Tgl,A.Operator,a.OperatorTgl 
            ORDER BY a.OperatorTgl ASC
            OFFSET @OFFSET ROWS
            FETCH NEXT @FETCH ROWS ONLY

            select count(*) totalData
            from (
                    select a.ID IDKRM, a.IDAsal, aa.TipeLokasi TipeAsal,aa.NamaLokasi NamaAsal ,
                    a.IDTujuan, ab.TipeLokasi TipeTujuan, ab.NamaLokasi NamaTujuan, a.Keterangan, COUNT(b.IDProduct) TotalItems, a.StatusTransOut
                    , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
                    from StockOutgoingDJ a
                    LEFT JOIN StockOutgoingDJ_Product b on b.IDForm=a.ID
                    LEFT JOIN StockProductDJ c on c.ID=b.IDProduct            
                    JOIN #TempLocAsal aa on aa.IDLokasi=a.IDAsal and aa.TipeLokasi=a.TipeAsal
                    JOIN #TempTujuan ab on ab.IDLokasi = a.IDTujuan and ab.TipeLokasi=a.TipeTujuan
                    where A.StatusTransOut = 0 AND (@IDBrand = -1 or a.IDBrand = @IDBrand)
                    AND (@TipeAE  = 9 OR a.TipeAsal = @TipeAE) 
                    AND (@IDAE = 9 or a.IDAsal = @IDAE) AND (@Kw = '' OR (c.Nomor like '%'+@Kw+'%' OR a.NamaAsal like '%'+@Kw+'%' OR a.NamaTujuan like '%'+@Kw+'%'))
                    GROUP BY a.ID,a.IDAsal,AA.TipeLokasi,AA.NamaLokasi,A.IDTujuan,AB.TipeLokasi,AB.NamaLokasi, A.Keterangan,A.StatusTransOut,
                    A.Tgl,A.Operator,a.OperatorTgl
            ) DS

            DROP TABLE #TempLocAsal,#TempTujuan		        
        ";

        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(getDataOutgoingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@IDUser", App.UserID);
                cmd.Parameters.AddWithValue("@IDBrand", vB);                
                cmd.Parameters.AddWithValue("@Kw", kw);
                cmd.Parameters.AddWithValue("@OFFSET", (pageStart <= 0 ? 1 : pageStart - 1) * pageSize);
                cmd.Parameters.AddWithValue("@FETCH", pageSize);

                conn.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable data = ds1.Tables[0];
                DataTable data2 = ds1.Tables[1];
                conn.Close();

                int totalData = Convert.ToInt32(data2.Rows[0]["totalData"]);

                BindData(pageStart, pageSize, totalData);

                int vF = 0; // id from
                int vT = 0; // id to
                int tF = 0; // value from
                int tT = 0; // value to

                int index = 1;

                pageStart = pageStart <= 0 ? 1 : pageStart;
                int no = ((pageStart - 1) * pageSize) + 1;

                string msg = "";

                if (totalData == 0) { App.TableNoResult(tb); }
                else
                {
                    msg = "Showing "+ pageSize+ " of " + totalData + " data";
                    if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
                    if (msg != "") { lit_msg.Text = msg; }
                    foreach (DataRow item in data.Rows)
                    {
                        if (!Response.IsClientConnected) break;

                        vF = Convert.ToInt32(item["IDAsal"]); // id from
                        vT = Convert.ToInt32(item["IDTujuan"]); // id to
                        tF = Convert.ToInt32(item["TipeAsal"]); // value from
                        tT = Convert.ToInt32(item["TipeTujuan"]); // value to

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(Convert.ToInt32(item["IDKRM"]), vB, tF, tT, vF, vT);
                        tr.Cells.Add(td);


                        td = new TableCell();
                        td.Text = "<p>" + no++ + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(tF) + "<br>" + TD_Nama(tF, vF) + "<br>" + TD_Addr(tF, vF) + "<br>" + TD_Group(tF, vF);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(tT) + "<br>" + TD_Nama(tT, vT) + "<br>" + TD_Addr(tT, vT) + "<br>" + TD_Group(tT, vT);
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




        // if (rs.Count() == 0) { App.TableNoResult(tb); }
        // else
        // {
        //     string TotalData = rs.Count().ToString();
        //     string msg = "";

        //     if (rs.Count() > 100) { rs = rs.Take(100); msg = "Showing 100 from " + TotalData + " data"; } 
        //         else { msg = "Showing " + TotalData + " data"; }
        //     if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
        //     if (msg != "") { lit_msg.Text = msg; }
        //     rs = rs.OrderByDescending(p => p.OperatorTgl);

        //     foreach (var item in rs)
        //     {
        //         var rsI = from p in db.StockOutgoingDJ_Product where p.IDForm == item.ID select p;

        //         if (!Response.IsClientConnected) break;

        //         vF = item.IDAsal; // id from
        //         vT = item.IDTujuan; // id to
        //         tF = item.TipeAsal; // value from
        //         tT = item.TipeTujuan; // value to

        //         //Sub-Header
        //         //if (prev != r1.IDBrand)
        //         //{
        //         //    Sub_Header(vT, r1.ID);
        //         //    prev = r1.IDBrand;
        //         //}

        //         TableRow tr = new TableRow();
        //         TableCell td;

        //         td = new TableCell();
        //         td.Text = TD_HyperLink(item);
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = "<p>" + index++ + ".</p>";
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF) + TD_Group(tF, vF) + TD_Addr(tF, vF) + "<br>";
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + TD_Group(tT, vT) + TD_Addr(tT, vT) + "<br>";
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = rsI.Count().ToString();
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = item.Operator + "<br>" + Str.FullDate(item.Tgl);
        //         tr.Cells.Add(td);

        //         tb.Rows.Add(tr);
        //     }
        // }

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
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     tb.Visible = true;

    //     //FILTER
    //     int vB = Convert.ToInt32(Request.QueryString["brand"].ToString());
    //     string kw = Request.QueryString["kw"].ToString();
    //     db.CommandTimeout = 300;
    //     keyword.Text = kw;

    //     //SELECT
    //     var rs = (from p in db.StockOutgoingDJ
    //               where ((vB > 0) ? p.IDBrand == vB : 1 == 1) && p.StatusTransOut == 0 // 0 : Draft
    //                     && (p.StockOutgoingDJ_Product.Any(z => z.StockProductDJ.Nomor.Contains(kw))
    //                         || p.NamaAsal.Contains(kw) || p.NamaTujuan.Contains(kw) || p.Operator.Contains(kw))
    //               select p);

    //     int UserID = App.UserID;
    //     var d = from p in db.DataAdminSales
    //             where p.IDUser == UserID
    //             select p;
    //     if (d.Count() > 0)
    //     {
    //         rs = from p in rs
    //              where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
    //              select p;
    //     }

    //     var f = from p in db.DataSK
    //             where p.IDUser == UserID
    //             select p;
    //     if (f.Count() > 0)
    //     {
    //         rs = from p in rs
    //              where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
    //              select p;
    //     }

    //     int vF = 0; // id from
    //     int vT = 0; // id to
    //     int tF = 0; // value from
    //     int tT = 0; // value to

    //     int index = 1;

    //     if (rs.Count() == 0) { App.TableNoResult(tb); }
    //     else
    //     {
    //         string TotalData = rs.Count().ToString();
    //         string msg = "";

    //         if (rs.Count() > 100) { rs = rs.Take(100); msg = "Showing 100 from " + TotalData + " data"; } 
    //             else { msg = "Showing " + TotalData + " data"; }
    //         if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
    //         if (msg != "") { lit_msg.Text = msg; }
    //         rs = rs.OrderByDescending(p => p.OperatorTgl);

    //         foreach (var item in rs)
    //         {
    //             var rsI = from p in db.StockOutgoingDJ_Product where p.IDForm == item.ID select p;

    //             if (!Response.IsClientConnected) break;

    //             vF = item.IDAsal; // id from
    //             vT = item.IDTujuan; // id to
    //             tF = item.TipeAsal; // value from
    //             tT = item.TipeTujuan; // value to

    //             //Sub-Header
    //             //if (prev != r1.IDBrand)
    //             //{
    //             //    Sub_Header(vT, r1.ID);
    //             //    prev = r1.IDBrand;
    //             //}

    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = TD_HyperLink(item);
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<p>" + index++ + ".</p>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF) + TD_Group(tF, vF) + TD_Addr(tF, vF) + "<br>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + TD_Group(tT, vT) + TD_Addr(tT, vT) + "<br>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = rsI.Count().ToString();
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = item.Operator + "<br>" + Str.FullDate(item.Tgl);
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);
    //         }
    //     }

    //     //COUNT FROM RESULT
    //     //int iFW = rsW.Count();
    //     //int iFO = rsO.Count();
    //     //int iFE = rsE.Count();

    //     bool isValidFromTo = true;

    //     //NO-RESULT FROM RESULT
    //     //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
    //     //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
    //     //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

    //     //NO-RESULT FROM-TO (SAME LOCATION) RESULT
    //     //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
    //     //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
    //     //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

    //     //NO-RESULT
    //     //if (rs.Count() == 0) App.TableNoResult(tb);

    //     //RESULT
    //     int prev = 0;


    // }

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
    protected string TD_HyperLink(int IDKRM, int IDBrand, int TipeAsal, int TipeTujuan, int IDAsal, int IDTujuan)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + IDKRM + "&idb=" + IDBrand + "&tf=" + TipeAsal + "&tt=" + TipeTujuan + "&idf=" + IDAsal + "&idt=" + IDTujuan + "\">";
        x += "  <i class=\"icon-search\"></i> ";
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
    
    private void BindData(int page,int pageSize,int totalRecord)
    {     
        int totalRecords = totalRecord; // Fetch total records from DB
        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);        

        GeneratePagination(page, totalPages);
    }  

    private void GeneratePagination(int currentPage, int totalPages)
    {        
        string paginationHtml = "";

        // Previous Button            
        // paginationHtml += $"<li class='{(currentPage == 1 ? "disabled" : "")}'> <a href='{(currentPage > 1 ? url + (currentPage - 1) : "#")}'>&laquo;</a> </li>";
        paginationHtml += "<a href='DJ.aspx?refreshPage=1&kw="+keyword.Text+"&brand="+brand.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)-1)+"' class='btn btn-default' "+(currentPage == 1 ? "disabled" : "")+" runat='server'>Prev</a>";

        // Page Numbers
        int maxDisplay = 10;
        int startPage = Math.Max(1, currentPage - maxDisplay / 2);
        int endPage = Math.Min(totalPages, startPage + maxDisplay - 1);

        if (startPage > 1)
        {
            paginationHtml += "<a href='DJ.aspx?refreshPage=1&kw="+keyword.Text+"&brand="+brand.SelectedValue+"&startROW=1' class='btn btn-default' runat='server'>1</a>";
            if (startPage > 2)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
        }

        for (int i = startPage; i <= endPage; i++)
        {
            paginationHtml += "<a href='DJ.aspx?refreshPage=1&kw="+keyword.Text+"&brand="+brand.SelectedValue+"&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
        }

        if (endPage < totalPages)
        {
            if (endPage < totalPages - 1)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
            paginationHtml += "<a href='DJ.aspx?refreshPage=1&kw="+keyword.Text+"&brand="+brand.SelectedValue+"&startROW=" + totalPages + "' class='btn btn-default' runat='server'>" + totalPages + "</a>";
        }

        // Next Button
        paginationHtml += "<a href='DJ.aspx?refreshPage=1&kw="+keyword.Text+"&brand="+brand.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)+1)+"' class='btn btn-default' "+((currentPage == totalPages || currentPage >totalPages) ? "disabled" : "")+" runat='server'>Next</a>";

        paginationGRP.InnerHtml= paginationHtml;
    }
}