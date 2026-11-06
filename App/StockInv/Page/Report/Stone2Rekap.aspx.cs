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
            Ddl.Filter(parcel01,"parcel201");
            Ddl.Filter(parcel02,"parcel202");
            Ddl.Filter(parcel03,"parcel203");
            Ddl.Filter(parcel04,"parcel204");
            Ddl.Filter(parcel05,"parcel205");
            Ddl.Filter(parcel06,"parcel206");
            Ddl.Filter(parcel07,"parcel207");

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
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone2Rekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone2Rekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
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
            order.SelectedValue = Request.QueryString["order"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];

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
        //string o= order.SelectedValue + " " + orderby.SelectedValue;

        string o1 = order.SelectedValue;
        string o2 = orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone2.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockProductDJ_Stone2
                  where !p.StockProductDJ.Draft.Value
                  select new
                  {
                      ID = p.Stone2.ID,
                      Nama = p.Stone2.Nama,
                      KodeParcel = p.Stone2.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone2.Parcel01,
                      Parcel02 = p.Stone2.Parcel02,
                      Parcel03 = p.Stone2.Parcel03,
                      Parcel04 = p.Stone2.Parcel04,
                      Parcel05 = p.Stone2.Parcel05,
                      Parcel06 = p.Stone2.Parcel06,
                      Parcel07 = p.Stone2.Parcel07,
                      Nama01 = p.Stone2.Parcel201.Nama,
                      Nama02 = p.Stone2.Parcel202.Nama,
                      Nama03 = p.Stone2.Parcel203.Nama,
                      Nama04 = p.Stone2.Parcel204.Nama,
                      Nama05 = p.Stone2.Parcel205.Nama,
                      Nama06 = p.Stone2.Parcel206.Nama,
                      Nama07 = p.Stone2.Parcel207.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      NamaLokasi = p.StockProductDJ.StockActualDJ.NamaLokasi
                  }).OrderBy(o1, o2).Where(z => z.Nama.Contains(kw.Text) ||
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
                td.Text = r.First().NamaLokasi;
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
            td2.ColumnSpan = 10;
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
        string o = (Request.QueryString["order"] == "Nomor" ? "AX." + Request.QueryString["order"] : "AX." + Request.QueryString["order"]) + " " + Request.QueryString["orderby"];
        // string o = (Request.QueryString["order"] == "Nomor" ? "b." + Request.QueryString["order"] : "c." + Request.QueryString["order"]) + " " + Request.QueryString["orderby"];


        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"SELECT ID,NamaLokasi,Nama,KodeParcel,SUM(TotalButir) TotalButir,SUM(TotalCarat)TotalCarat
                            ,nmShape,nmColor,nmSize,colorGrade,nmClarity,nmMaterial,nmCategory
                         FROM (
                            select c.ID,d.NamaLokasi,c.Nama,c.KodeParcel,SUM(a.TotalButir) TotalButir,SUM(a.TotalCarat)TotalCarat
                            ,ca.nama nmShape,cb.nama nmColor,cc.nama nmSize,cd.nama colorGrade,ce.nama nmClarity,cf.nama nmMaterial,cg.nama nmCategory                            
                            from StockProductDJ_Stone2 a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN Stone2 c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel201 ca on ca.id=c.Parcel01
                            JOIN Parcel202 cb on cb.id=c.Parcel02
                            JOIN Parcel203 cc on cc.id=c.Parcel03
                            JOIN Parcel204 cd on cd.id=c.Parcel04
                            JOIN Parcel205 ce on ce.id=c.Parcel05
                            JOIN Parcel206 cf on cf.id=c.Parcel06
                            JOIN Parcel207 cg on cg.id=c.Parcel07
                            where b.Draft=0
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0 and d.IDLokasi <> -1
                            and (@IDLokasi <= 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                            and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                            and (@parcel7 = 0 or cg.id=@parcel7) and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                            group by c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,ce.nama,cf.nama,cg.nama,d.NamaLokasi
                            )AX
                            group by AX.id,AX.nama,AX.kodeparcel,nmShape,nmColor,nmSize,colorGrade,nmClarity,nmMaterial,nmCategory,NamaLokasi
                            order by " + o ;
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text.Trim() });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });
                command.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });
                command.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });
                command.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });
                


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int no = 0;
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;
                        td = new TableCell();
                        td.Text = "No data available";
                        td.ColumnSpan = 12;
                        td.Style.Add("text-align", "center");
                        tr.Cells.Add(td);
                        tb.Rows.Add(tr);
                        return;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        no++;                        

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Nama"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);
                        
                        td = new TableCell();
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        // Ensure the column names match those in the DataTable
                        td.Text = row["colorGrade"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmMaterial"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmCategory"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        TotalButir += Convert.ToDecimal(row["TotalButir"]);
                        TotalCarat += Convert.ToDecimal(row["TotalCarat"]);




                        tb.Rows.Add(tr);
                    }

                    TableRow tr2 = new TableRow();
                        TableCell td2;
            
                        td2 = new TableCell();
                        td2.Text = "<b>Total</b>";
                        td2.ColumnSpan = 10;
                        tr2.Cells.Add(td2);
            
                        td2 = new TableCell();
                        td2.Text = TotalButir.ToString("N0");
                        tr2.Cells.Add(td2);
            
                        td2 = new TableCell();
                        td2.Text = TotalCarat.ToString("N3");
                        tr2.Cells.Add(td2);
            
                        tb.Rows.Add(tr2);

                }
                else
                {
                    TableRow tr = new TableRow();
                    TableCell td;
                    td = new TableCell();
                    td.Text = "No data available";
                    td.ColumnSpan = 12;
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
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }
}