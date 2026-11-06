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
        href += "&brand=" + brand.SelectedValue;
        href += "&from=" + ddl_from.SelectedValue;
        href += "&to=" + ddl_to.SelectedValue;
        href += "&kw=" + kw.Text;
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
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
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
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Warehouse", "0"));
            }
            else if (x.TipeLokasi == 1)
            {
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
                ddl_to.Items.Clear();
                ddl_to.Items.Add(new ListItem("Exhibition", "2"));
            }
        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;

        
        //WHERE
        string w = "1=1";        
        //if (ddl_from.SelectedIndex > 0) w += " AND TipeAsal = " + ddl_from.SelectedValue;
        //if (ddl_to.SelectedIndex > 0) w += " AND TipeTujuan = " + ddl_to.SelectedValue;

        int vB = Convert.ToInt32(brand.SelectedValue);
        int vF = Convert.ToInt32(ddl_from.SelectedValue);
        int vT = Convert.ToInt32(ddl_to.SelectedValue);
        string kF = txt_from.Text.Trim();
        string kT = txt_to.Text.Trim();

        //SELECT
        string queryGetIncomingDJ = @"
                DECLARE @TipeAE int = 9,@IDTujuan int = 9


                CREATE TABLE #TempLocTujuan(
                    IDLokasi int, 
                    TipeLokasi int, 
                    NamaLokasi Varchar(100),
                    NamaKode Varchar(100),
                    AddrAlamat nvarchar(max)
                );

                IF EXISTS(select * from DataAdminSales where IDUser=@IDUser and Disable = 0)
                    BEGIN
                        select @TipeAE = TipeLokasi, @IDTujuan = IDLokasi from DataAdminSales where IDUser=@IDUser and Disable = 0
                        
                        IF @TipeAE = 0
                        BEGIN
                            INSERT INTO #TempLocTujuan
                            select a.ID, 0 TipeLokasi, a.Nama, NamaKode,AddrAlamat 
                            from LocWarehouse a
                            JOIN LocWarehouseGroup b on b.ID=a.IDGroup
                            where a.Disable=0 
                        END
                        ELSE IF @TipeAE = 1
                        BEGIN
                            INSERT INTO #TempLocTujuan
                            select a.ID, 1 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
                            from LocOutlet a
                            JOIN LocOutletGroup b on b.ID=a.IDGroup
                            where a.Disable=0
                        END
                        ELSE IF @TipeAE = 2
                        BEGIN
                            INSERT INTO #TempLocTujuan
                            select a.ID, 2 TipeLokasi, a.Nama ,b.NamaKode, a.AddrAlamat
                            from LocExhibition a
                            JOIN LocExhibitionGroup b on b.ID=a.IDGroup
                            where a.Disable=0 
                        END		
                    END
                ELSE
                    BEGIN
                        INSERT INTO #TempLocTujuan
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

                        SELECT ID IDLokasi, TipeLokasi, Nama NamaLokasi,NamaKode,AddrAlamat into #TempLocAsal
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

               select a.ID AS IDKrm, a.Nomor as NomorKRM, a.Keterangan, b.Total, a.IDBrand, a.TipeAsal, a.IDAsal, aa.NamaKode as KodeAsal
               , AA.NamaLokasi as NamaAsal, aa.AddrAlamat as AddrAsal
			   , a.TipeTujuan, a.IDTujuan, ab.NamaKode as KodeTujuan, ab.NamaLokasi as NamaTujuan, ab.AddrAlamat as AddrTujuan
			   , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
			   FROM StockOutgoingDJ a
			   JOIN (select IDForm, COUNT(IDProduct) Total from StockOutgoingDJ_Product GROUP BY IDForm) b on b.IDForm=a.ID            
			   JOIN #TempLocAsal aa on aa.IDLokasi =a.IDAsal and aa.TipeLokasi = a.TipeAsal
			   JOIN #TempLocTujuan ab on ab.IDLokasi=a.IDTujuan and ab.TipeLokasi=a.TipeTujuan
			   LEFT JOIN StockIncomingDJ c on c.IDOutgoing = a.ID
			   where StatusTransOut=2 AND (c.ID IS NULL OR c.StatusTransIn NOT IN (3, 1)) 
               AND (@IDBrand = 0 or a.IDBrand = @IDBrand) 
			    AND (@TipeAsal = -1 or a.TipeAsal = @TipeAsal) AND (@keyword = '' or a.Nomor like '%'+@keyword+'%')
			    AND (@LocAsal = '' or (aa.NamaLokasi like '%'+@LocAsal+'%')) AND (@LocTujuan = '' or (ab.NamaLokasi like '%'+@LocTujuan+'%'))
                AND ((@TipeAE = 9  and (@TipeTujuan = -1 or a.TipeTujuan=@TipeTujuan))or (@TipeAE <> 9 and a.TipeTujuan = @TipeAE and a.IDTujuan=@IDTujuan))				
				--and c.ID IS NULL
				order by a.OperatorTgl desc
				

                DROP TABLE #TempLocTujuan,#TempLocAsal
        ";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(queryGetIncomingDJ, connection))
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDBrand", SqlDbType.Int) { Value = vB });
                command.Parameters.Add(new SqlParameter("@TipeAsal", SqlDbType.Int) { Value = vF });
                command.Parameters.Add(new SqlParameter("@TipeTujuan", SqlDbType.Int) { Value = vT });
                command.Parameters.Add(new SqlParameter("@keyword", SqlDbType.VarChar) { Value = kw.Text.Trim() });
                command.Parameters.Add(new SqlParameter("@LocAsal", SqlDbType.VarChar) { Value = kF });
                command.Parameters.Add(new SqlParameter("@LocTujuan", SqlDbType.VarChar) { Value = kT });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.VarChar) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable data = ds.Tables[0];
                connection.Close();
                int prev = 0;
                int index = 1;
                int TipeAsal = 0;
                int TipeTujuan = 0;
                int IDAsal = 0;
                int IDTujuan = 0;
                int IDBrand = 0;


                //NO-RESULT FROM RESULT
                if (data.Rows.Count == 0) { App.TableNoResult(tb); }
                else
                {
                    foreach (DataRow item in data.Rows)                    
                    {

                        TipeAsal = Convert.ToInt32(item["TipeAsal"]);
                        TipeTujuan = Convert.ToInt32(item["TipeTujuan"]);
                        IDAsal = Convert.ToInt32(item["IDAsal"]);
                        IDTujuan = Convert.ToInt32(item["IDTujuan"]);
                        IDBrand = Convert.ToInt32(item["IDBrand"]);

                        if (!Response.IsClientConnected) break;

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(Convert.ToInt32(item["IDKrm"]), Convert.ToString(item["NomorKRM"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToString(item["NomorKRM"]);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.HtmlBreak(Convert.ToString(item["Keterangan"]));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToInt32(item["Total"]).ToString(); // Qty
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(TipeAsal) + TD_Nama(TipeAsal, IDAsal) + TD_Addr(TipeAsal, IDAsal) + TD_Group(TipeAsal, IDAsal);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.LocationType(TipeTujuan) + TD_Nama(TipeTujuan, IDTujuan) + TD_Addr(TipeTujuan, IDTujuan) + TD_Group(TipeTujuan, IDTujuan);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToString(item["Tgl"]);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                        index++;
                    }

                }
            }
        }        

    }
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        
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

        //SELECT
        var rs = from p in db.StockOutgoingDJ.Where(w)
                 where p.StatusTransOut == 2
                 && !(from z in db.StockIncomingDJ
                      where z.StatusTransIn == 3 || z.StatusTransIn == 1
                      select z.IDOutgoing).Contains(p.ID)// Approved
                 select p;

        string keyword = kw.Text.Trim();
        if(keyword!="")
            rs = rs.Where(p=>p.Nomor.Contains(keyword));

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

            rs = from p in rs
                 where (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {

            rs = from p in rs
                 where (p.TipeTujuan == f.First().TipeLokasi && p.IDTujuan == f.First().IDLokasi)
                 select p;
        }

        if (vF == 0) 
        {
            rs = from p in rs
                 join q in db.LocWarehouse on p.IDAsal equals q.ID
                 where p.TipeAsal == 0
                 select p;

            if (kF != "")
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                     where p.TipeAsal == 0 && q.Nama.Contains(kF)
                     select p;
            }
        }
        else if (vF == 1)
        {
            rs = from p in rs
                 join q in db.LocOutlet on p.IDAsal equals q.ID
                 where p.TipeAsal == 1
                 select p;

            if (kF != "")
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDAsal equals q.ID
                     where p.TipeAsal == 1 && q.Nama.Contains(kF)
                     select p;
            }
        }
        else if (vF == 2)
        {
            rs = from p in rs
                 join q in db.LocExhibition on p.IDAsal equals q.ID
                 where p.TipeAsal == 2
                 select p;

            if (kF != "")
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDAsal equals q.ID
                     where p.TipeAsal == 2 && q.Nama.Contains(kF)
                     select p;
            }
        }

        if (vT == 0)
        {
            rs = from p in rs
                 join q in db.LocWarehouse on p.IDTujuan equals q.ID
                 where p.TipeTujuan == 0
                 select p;

            if (kT != "")
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                     where p.TipeTujuan == 0 && q.Nama.Contains(kT)
                     select p;
            }
        }
        else if (vT == 1)
        {
            rs = from p in rs
                 join q in db.LocOutlet on p.IDTujuan equals q.ID
                 where p.TipeTujuan == 1
                 select p;

            if (kT != "")
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                     where p.TipeTujuan == 1 && q.Nama.Contains(kT)
                     select p;
            }
        }
        else if (vT == 2)
        {
            rs = from p in rs
                 join q in db.LocExhibition on p.IDTujuan equals q.ID
                 where p.TipeTujuan == 2
                 select p;

            if (kT != "")
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                     where p.TipeTujuan == 2 && q.Nama.Contains(kT)
                     select p;
            }
        }

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
                var rsI = from p in db.StockOutgoingDJ_Product
                          where p.IDForm == item.ID
                          select p;

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
                td.Text = TD_HyperLink(item.ID, item.Nomor);
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
                td.Text = rsI.Count().ToString(); // Qty
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeAsal) + TD_Nama(item.TipeAsal, item.IDAsal) + TD_Addr(item.TipeAsal, item.IDAsal) + TD_Group(item.TipeAsal, item.IDAsal);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + TD_Nama(item.TipeTujuan, item.IDTujuan) + TD_Addr(item.TipeTujuan, item.IDTujuan) + TD_Group(item.TipeTujuan, item.IDTujuan);
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
    protected string TD_HyperLink(int id, string nomor)
    {
        string x = string.Empty;
		string cek = Lib.CheckDoKRM(nomor);
		
		if(cek == "")
		{
			x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + id + "\">";
			x += "  <i class=\"icon-ok\"></i> ";
			x += "  Select";
			x += "</a>";
		}
		else
		{
			x += "<label class=\"text-danger text-sm\">";
			x += cek;
			x += "</label>";
		}

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