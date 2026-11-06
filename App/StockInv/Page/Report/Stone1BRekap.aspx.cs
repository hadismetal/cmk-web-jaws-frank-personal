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
            Ddl.Filter(parcel01,"Parcel101");
            Ddl.Filter(parcel02,"Parcel102");
            Ddl.Filter(parcel03,"Parcel103");
            Ddl.Filter(parcel04,"Parcel104");
            Ddl.Filter(parcel05,"Parcel105");
            Ddl.Filter(parcel06,"Parcel106");
            Ddl.Filter(parcel07,"Parcel107");

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
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
        string href = "Stone1BRekap_Print.aspx?id=1";

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
        string href = "Stone1BRekap_Export.aspx?id=1";

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
        int parcel1 = Convert.ToInt32(parcel01.SelectedValue);
        int parcel2 = Convert.ToInt32(parcel02.SelectedValue);
        int parcel3 = Convert.ToInt32(parcel03.SelectedValue);
        int parcel4 = Convert.ToInt32(parcel04.SelectedValue);
        int parcel5 = Convert.ToInt32(parcel05.SelectedValue);
        int parcel6 = Convert.ToInt32(parcel06.SelectedValue);
        int parcel7 = Convert.ToInt32(parcel07.SelectedValue);

        string o = (Request.QueryString["order"] == "Nomor" ? "b." + Request.QueryString["order"] : "c." + Request.QueryString["order"]) + " " + Request.QueryString["orderby"];

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"select d.NamaLokasi,c.Nama,ca.nama nmShape,cb.nama nmSize,cc.nama nmColor,
                            cd.nama nmClarity,ce.nama nmCutting,cf.nama nmBrand,cg.nama nmCategory
                            ,SUM(a.TotalButir) TotalButir,SUM(a.TotalCarat)TotalCarat
                            from StockProductDJ_Stone1B a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN Stone1B c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel101 ca on ca.id=c.Parcel01
                            JOIN Parcel102 cb on cb.id=c.Parcel02
                            JOIN Parcel103 cc on cc.id=c.Parcel03
                            JOIN Parcel104 cd on cd.id=c.Parcel04
                            JOIN Parcel105 ce on ce.id=c.Parcel05
                            JOIN Parcel106 cf on cf.id=c.Parcel06
                            JOIN Parcel107 cg on cg.id=c.Parcel07
                            where a.StatusTerpasang = 1 and b.Draft=0 and d.IDLokasi <> -1
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                            and (@IDLokasi <= 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                            and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                            and (@parcel7 = 0 or cg.id=@parcel7) and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                            group by c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,ce.nama,cf.nama,cg.nama,d.NamaLokasi
                            order by " + o + @"
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
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmCutting"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmBrand"].ToString();
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

                        TotalButir = TotalButir + Convert.ToDecimal(row["TotalButir"]);
                        TotalCarat = TotalCarat + Convert.ToDecimal(row["TotalCarat"]);


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
                    td.ColumnSpan = 8;
                    td.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(td);
                    tb.Rows.Add(tr);
                }

            }
        }

        //ORDER BY
        // string o = order.SelectedValue + " " + orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone1B.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        // var rs = (from p in db.StockProductDJ_Stone1B
        //           where p.StatusTerpasang == 1
        //           && !p.StockProductDJ.Draft.Value
        //           select new
        //           {
        //               ID = p.Stone1B.ID,
        //               Nama = p.Stone1B.Nama,
        //               KodeParcel = p.Stone1B.KodeParcel,
        //               TotalButir = p.TotalButir,
        //               TotalCarat = p.TotalCarat,
        //               Nomor = p.StockProductDJ.Nomor,
        //               Parcel01 = p.Stone1B.Parcel01,
        //               Parcel02 = p.Stone1B.Parcel02,
        //               Parcel03 = p.Stone1B.Parcel03,
        //               Parcel04 = p.Stone1B.Parcel04,
        //               Parcel05 = p.Stone1B.Parcel05,
        //               Parcel06 = p.Stone1B.Parcel06,
        //               Parcel07 = p.Stone1B.Parcel07,
        //               Nama01 = p.Stone1B.Parcel101.Nama,
        //               Nama02 = p.Stone1B.Parcel102.Nama,
        //               Nama03 = p.Stone1B.Parcel103.Nama,
        //               Nama04 = p.Stone1B.Parcel104.Nama,
        //               Nama05 = p.Stone1B.Parcel105.Nama,
        //               Nama06 = p.Stone1B.Parcel106.Nama,
        //               Nama07 = p.Stone1B.Parcel107.Nama,
        //               StockProductDJ = p.StockProductDJ,
        //               StockActualDJ = p.StockProductDJ.StockActualDJ,
        //               NamaLokasi = p.StockProductDJ.StockActualDJ.NamaLokasi
        //           }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
        //              z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));


        // if (Tipe >= 0)
        // {
        //     rs = rs.Where(p => p.StockActualDJ.TipeLokasi == Tipe);

        //     if (IDLokasi >= 0)
        //     {
        //         rs = rs.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);
        //     }
        // }

        // rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        // if (Convert.ToInt32(parcel01.SelectedValue) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
        // if (Convert.ToInt32(parcel02.SelectedValue) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
        // if (Convert.ToInt32(parcel03.SelectedValue) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
        // if (Convert.ToInt32(parcel04.SelectedValue) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));
        // if (Convert.ToInt32(parcel05.SelectedValue) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(parcel05.SelectedValue));
        // if (Convert.ToInt32(parcel06.SelectedValue) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(parcel06.SelectedValue));
        // if (Convert.ToInt32(parcel07.SelectedValue) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(parcel07.SelectedValue));

        // var stone = rs.GroupBy(p => p.KodeParcel);

        // //NO-RESULT
        // if (rs.Count() == 0) App.TableNoResult(tb);

        // else
        // {
        //     decimal TotalButir = 0m;
        //     decimal TotalCarat = 0m;

        //     //RESULT
        //     int index = 1;
        //     foreach (var r in stone)
        //     {
        //         if (!Response.IsClientConnected) break;

        //         TableRow tr = new TableRow();
        //         TableCell td;

        //         td = new TableCell();
        //         td.Text = "<p>" + index++.ToString() + ".</p>";
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().NamaLokasi;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama01;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama02;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama03;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama04;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama05;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama06;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = r.First().Nama07;
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
        //         tr.Cells.Add(td);

        //         td = new TableCell();
        //         td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
        //         tr.Cells.Add(td);

        //         tb.Rows.Add(tr);

        //         TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
        //         TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
        //     }

        //     TableRow tr2 = new TableRow();
        //     TableCell td2;

        //     td2 = new TableCell();
        //     td2.Text = "<b>Total</b>";
        //     td2.ColumnSpan = 10;
        //     tr2.Cells.Add(td2);

        //     td2 = new TableCell();
        //     td2.Text = TotalButir.ToString("N0");
        //     tr2.Cells.Add(td2);

        //     td2 = new TableCell();
        //     td2.Text = TotalCarat.ToString("N3");
        //     tr2.Cells.Add(td2);

        //     tb.Rows.Add(tr2);
        // }

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