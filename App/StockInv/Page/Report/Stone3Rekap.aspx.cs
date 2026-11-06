using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Account_Stone : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //DLC
            int Tipe = 1;
            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);
            Ddl.Filter(parcel01,"parcel301");
            Ddl.Filter(parcel02,"parcel302");
            Ddl.Filter(parcel03,"parcel303");
            Ddl.Filter(parcel04,"parcel304");
            Ddl.Filter(parcel05,"parcel305");
            Ddl.Filter(parcel06,"parcel306");
            Ddl.Filter(parcel07,"parcel307");
            Ddl.Filter(parcel08,"parcel308");

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
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&parcel08=" + parcel08.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&startROW=1";
        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone3Rekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&parcel08=" + parcel08.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;        
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone3Rekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&parcel08=" + parcel08.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        tipelokasi.Items.Clear();
        tipelokasi.Items.Add(new ListItem("All", "-1"));
        tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
        tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        tipelokasi.Items.Add(new ListItem("Exhibition", "2"));
        

        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedValue = Request.QueryString["parcel01"];
            parcel02.SelectedValue = Request.QueryString["parcel02"];
            parcel03.SelectedValue = Request.QueryString["parcel03"];
            parcel04.SelectedValue = Request.QueryString["parcel04"];
            parcel05.SelectedValue = Request.QueryString["parcel05"];
            parcel06.SelectedValue = Request.QueryString["parcel06"];
            parcel07.SelectedValue = Request.QueryString["parcel07"];
            parcel08.SelectedValue = Request.QueryString["parcel08"];
            order.SelectedValue = Request.QueryString["order"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
            startROW.InnerHtml = Request.QueryString["startROW"];                        
        }
        else
        {            
            lokasi.Items.Clear();                        
            lokasi.Items.Add(new ListItem("All", "-1"));
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        //ORDER BY
        string o = order.SelectedValue + " " + orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone3.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockProductDJ_Stone3
                  where !p.StockProductDJ.Draft.Value
                  select new
                  {
                      ID = p.Stone3.ID,
                      Nama = p.Stone3.Nama,
                      KodeParcel = p.Stone3.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone3.Parcel01,
                      Parcel02 = p.Stone3.Parcel02,
                      Parcel03 = p.Stone3.Parcel03,
                      Parcel04 = p.Stone3.Parcel04,
                      Parcel05 = p.Stone3.Parcel05,
                      Parcel06 = p.Stone3.Parcel06,
                      Parcel07 = p.Stone3.Parcel07,
                      Parcel08 = p.Stone3.Parcel08,
                      Nama01 = p.Stone3.Parcel301.Nama,
                      Nama02 = p.Stone3.Parcel302.Nama,
                      Nama03 = p.Stone3.Parcel303.Nama,
                      Nama04 = p.Stone3.Parcel304.Nama,
                      Nama05 = p.Stone3.Parcel305.Nama,
                      Nama06 = p.Stone3.Parcel306.Nama,
                      Nama07 = p.Stone3.Parcel307.Nama,
                      Nama08 = p.Stone3.Parcel308.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      Namalokasi = p.StockProductDJ.StockActualDJ.NamaLokasi
                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        if (Tipe >= 0)
        {
            rs = rs.Where(p => p.StockActualDJ.TipeLokasi == Tipe);


            if (IDLokasi >= 0)
            {
                rs = rs.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);

            }
        }
        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == parcel07.SelectedIndex);
        if (parcel08.SelectedIndex > 0) rs = rs.Where(p => p.Parcel08 == parcel08.SelectedIndex);


        var stone = rs.GroupBy(p => p.KodeParcel);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            //RESULT
            int index = 1;
            foreach (var r in stone)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Namalokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama01;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama02;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama03;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama04;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama05;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama06;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama07;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama08;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 11;



            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalButir.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalCarat.ToString("N3");
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillTable1(object sender, EventArgs e)    
    {        
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        int parcel1 = Convert.ToInt32(parcel01.SelectedValue);
        int parcel2 = Convert.ToInt32(parcel02.SelectedValue);
        int parcel3 = Convert.ToInt32(parcel03.SelectedValue);
        int parcel4 = Convert.ToInt32(parcel04.SelectedValue);
        int parcel5 = Convert.ToInt32(parcel05.SelectedValue);
        int parcel6 = Convert.ToInt32(parcel06.SelectedValue);
        int parcel7 = Convert.ToInt32(parcel07.SelectedValue);
        int parcel8 = Convert.ToInt32(parcel08.SelectedValue);
        string o1 = order.SelectedValue;
        string o2 = orderby.SelectedValue;
        string startRowValue = startROW.InnerHtml;        
        int pageStart = int.Parse(startRowValue);                

        int pageSize=30;   
        int OFFSET = (pageStart <= 0?1:pageStart -1) * pageSize;        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"
                            

                            select NamaLokasi,Nama as Parcel,KodeParcel, SUM(TotalButir) TotalButir, SUM(TotalCarat) TotalCarat,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin
                            from (
                            	select c.ID,d.NamaLokasi,c.Nama,c.KodeParcel,a.TotalButir,a.TotalCarat
                                ,ca.nama nmItem,cb.nama nmShape,cc.nama nmSize,cd.nama nmColor,ce.nama colorGrade,
                            	cf.nama clarity,cg.nama nmCategory,ch.Nama as countryOForigin
                                from StockProductDJ_Stone3 a
                                JOIN StockProductDJ b on b.id=a.IDForm
                                JOIN Stone3 c on c.ID=a.IDStone
                                JOIN StockActualDJ d on d.IDProduct=b.ID
                                JOIN Parcel301 ca on ca.id=c.Parcel01
                                JOIN Parcel302 cb on cb.id=c.Parcel02
                                JOIN Parcel303 cc on cc.id=c.Parcel03
                                JOIN Parcel304 cd on cd.id=c.Parcel04
                                JOIN Parcel305 ce on ce.id=c.Parcel05
                                JOIN Parcel306 cf on cf.id=c.Parcel06
                                JOIN Parcel307 cg on cg.id=c.Parcel07
                                JOIN Parcel308 ch on ch.id=c.Parcel08
                                where b.Draft=0
                                and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0 and d.IDLokasi <> -1	
                                and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                                and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                                and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                                and (@parcel7 = 0 or cg.id=@parcel7) and (@parcel8 = 0 or ch.id=@parcel8)
                                and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                            )ax
                            GROUP BY  Nama ,KodeParcel,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin,NamaLokasi,ID
                            order by " + o1 + " " + o2 + @"
                            OFFSET @offset ROWS
                            FETCH NEXT @pagesize ROWS ONLY 


                            select count(*) totalPages 
                            from (
                                select NamaLokasi,Nama as Parcel,KodeParcel, SUM(TotalButir) TotalButir, SUM(TotalCarat) TotalCarat,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin
                                from (
                                	select d.NamaLokasi,c.Nama,c.KodeParcel,a.TotalButir,a.TotalCarat
                                    ,ca.nama nmItem,cb.nama nmShape,cc.nama nmSize,cd.nama nmColor,ce.nama colorGrade,
                                	cf.nama clarity,cg.nama nmCategory,ch.Nama as countryOForigin
                                    from StockProductDJ_Stone3 a
                                    JOIN StockProductDJ b on b.id=a.IDForm
                                    JOIN Stone3 c on c.ID=a.IDStone
                                    JOIN StockActualDJ d on d.IDProduct=b.ID
                                    JOIN Parcel301 ca on ca.id=c.Parcel01
                                    JOIN Parcel302 cb on cb.id=c.Parcel02
                                    JOIN Parcel303 cc on cc.id=c.Parcel03
                                    JOIN Parcel304 cd on cd.id=c.Parcel04
                                    JOIN Parcel305 ce on ce.id=c.Parcel05
                                    JOIN Parcel306 cf on cf.id=c.Parcel06
                                    JOIN Parcel307 cg on cg.id=c.Parcel07
                                    JOIN Parcel308 ch on ch.id=c.Parcel08
                                    where b.Draft=0
                                    and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0 and d.IDLokasi <> -1	
                                    and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                                    and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                                    and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                                    and (@parcel7 = 0 or cg.id=@parcel7) and (@parcel8 = 0 or ch.id=@parcel8)
                                    and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                                )ax
                                GROUP BY  Nama ,KodeParcel,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin,NamaLokasi
                            )ax1
                            ";
                            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });
                command.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });
                command.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });
                command.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });
                command.Parameters.Add(new SqlParameter("@parcel8", SqlDbType.Int) { Value = parcel8 });                
                command.Parameters.Add(new SqlParameter("@offset", SqlDbType.Int) { Value = OFFSET });                
                command.Parameters.Add(new SqlParameter("@pagesize", SqlDbType.Int) { Value = pageSize });                

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int totalData = Convert.ToInt32(ds.Tables[1].Rows[0]["totalPages"].ToString());                

                BindData(pageStart,pageSize,totalData);             


                int no = 0;
                pageStart = pageStart <= 0 ? 1 : pageStart; 
                var counter = ((pageStart-1) * pageSize) + 1;        
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;
                        td = new TableCell();
                        td.Text = "No data available";
                        td.ColumnSpan = 13;
                        td.Style.Add("text-align", "center");
                        tr.Cells.Add(td);
                        tb.Rows.Add(tr);
                        return;
                    }                    
                    foreach (DataRow row in dt.Rows)
                    {                                                

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = counter.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Parcel"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        // Ensure the column names match those in the DataTable
                        td.Text = row["colorGrade"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["clarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmCategory"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["countryOForigin"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                        counter++;
                    }

                }
                else
                {
                    TableRow tr = new TableRow();
                    TableCell td;
                    td = new TableCell();
                    td.Text = "No data available";
                    td.ColumnSpan = 9;
                    td.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(td);
                    tb.Rows.Add(tr);
                }

            }
        }
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        paginationGRP.Attributes.Add("style", "display:none;");
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
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
        paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw="+kw.Text+"&parcel01="+parcel01.SelectedValue+"&parcel02="+parcel02.SelectedValue+"&parcel03="+parcel03.SelectedValue+"&parcel04="+parcel04.SelectedValue+"&parcel05="+parcel05.SelectedValue+"&parcel06="+parcel06.SelectedValue+"&parcel07="+parcel07.SelectedValue+"&parcel08="+parcel08.SelectedValue+"&order=" + Request.QueryString["order"]+"&orderby=" + Request.QueryString["orderby"]+"&tipelokasi=" + tipelokasi.SelectedValue+"&lokasi=" + lokasi.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)-1)+"' class='btn btn-default' "+(currentPage == 1 ? "disabled" : "")+" runat='server'>Prev</a>";

        // Page Numbers
        int maxDisplay = 10;
        int startPage = Math.Max(1, currentPage - maxDisplay / 2);
        int endPage = Math.Min(totalPages, startPage + maxDisplay - 1);

        if (startPage > 1)
        {        
            paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&parcel05=" + parcel05.SelectedValue + "&parcel06=" + parcel06.SelectedValue + "&parcel07=" + parcel07.SelectedValue + "&parcel08=" + parcel08.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=1' class='btn btn-default' runat='server'>1</a>";
            if (startPage > 2)
            {
            paginationHtml += "<span readonly disabled class='btn btn-default'>...</span>";
            }
        }

        for (int i = startPage; i <= endPage; i++)
        {
             paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&parcel05=" + parcel05.SelectedValue + "&parcel06=" + parcel06.SelectedValue + "&parcel07=" + parcel07.SelectedValue + "&parcel08=" + parcel08.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
        }

        if (endPage < totalPages)
        {
            if (endPage < totalPages - 1)
            {
            paginationHtml += "<span readonly class='btn btn-default'>...</span>";
            }
            paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&parcel05=" + parcel05.SelectedValue + "&parcel06=" + parcel06.SelectedValue + "&parcel07=" + parcel07.SelectedValue + "&parcel08=" + parcel08.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + totalPages + "' class='btn btn-default' runat='server'>" + totalPages + "</a>";
        }

        // Next Button
        paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&parcel05=" + parcel05.SelectedValue + "&parcel06=" + parcel06.SelectedValue + "&parcel07=" + parcel07.SelectedValue + "&parcel08=" + parcel08.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW="+(int.Parse(startROW.InnerHtml)+1)+"' class='btn btn-default' "+((currentPage == totalPages || currentPage >totalPages) ? "disabled" : "")+" runat='server'>Next</a>";

        paginationGRP.InnerHtml= paginationHtml;
    }

    private void GeneratePagination1(int currentPage, int totalPages)
    {        
        string paginationHtml = "";

        // Previous Button            
        // paginationHtml += $"<li class='{(currentPage == 1 ? "disabled" : "")}'> <a href='{(currentPage > 1 ? url + (currentPage - 1) : "#")}'>&laquo;</a> </li>";

                        
        paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw="+kw.Text+"&parcel01="+parcel01.SelectedValue+"&parcel02="+parcel02.SelectedValue+"&parcel03="+parcel03.SelectedValue+"&parcel04="+parcel04.SelectedValue+"&parcel05="+parcel05.SelectedValue+"&parcel06="+parcel06.SelectedValue+"&parcel07="+parcel07.SelectedValue+"&parcel08="+parcel08.SelectedValue+"&order=" + Request.QueryString["order"]+"&orderby=" + Request.QueryString["orderby"]+"&tipelokasi=" + tipelokasi.SelectedValue+"&lokasi=" + lokasi.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)-1)+"' class='btn btn-default' "+(currentPage == 1 ? "disabled" : "")+" runat='server'>Prev</a>";

        int maxVisible = 10;
        // Page Numbers
        for (int i = 1; i <= totalPages; i++)
        {
            if (i > maxVisible)
            {
            paginationHtml += "<a runat='server'>...</a>";
            break;
            }
            else
            {
            paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw=" + kw.Text + "&parcel01=" + parcel01.SelectedValue + "&parcel02=" + parcel02.SelectedValue + "&parcel03=" + parcel03.SelectedValue + "&parcel04=" + parcel04.SelectedValue + "&parcel05=" + parcel05.SelectedValue + "&parcel06=" + parcel06.SelectedValue + "&parcel07=" + parcel07.SelectedValue + "&parcel08=" + parcel08.SelectedValue + "&order=" + Request.QueryString["order"] + "&orderby=" + Request.QueryString["orderby"] + "&tipelokasi=" + tipelokasi.SelectedValue + "&lokasi=" + lokasi.SelectedValue + "&startROW=" + i + "' class='btn btn-default' " + (currentPage == i ? "disabled" : "") + " runat='server'>" + i + "</a>";
            }
        }
        if (totalPages > maxVisible)
        {
            paginationHtml += "<a runat='server'>...</a>";
        }

        // Next Button
        paginationHtml += "<a href='Stone3Rekap.aspx?refreshPage=1&kw="+kw.Text+"&parcel01="+parcel01.SelectedValue+"&parcel02="+parcel02.SelectedValue+"&parcel03="+parcel03.SelectedValue+"&parcel04="+parcel05.SelectedValue+"&parcel05="+parcel04.SelectedValue+"&parcel06="+parcel06.SelectedValue+"&parcel07="+parcel07.SelectedValue+"&parcel08="+parcel08.SelectedValue+"&order=" + Request.QueryString["order"]+"&orderby=" + Request.QueryString["orderby"]+"&tipelokasi=" + tipelokasi.SelectedValue+"&lokasi=" + lokasi.SelectedValue+"&startROW="+(int.Parse(startROW.InnerHtml)+1)+"' class='btn btn-default' "+((currentPage == totalPages || currentPage >totalPages) ? "disabled" : "")+" runat='server'>Next</a>";

        paginationGRP.InnerHtml= paginationHtml;
    }
}