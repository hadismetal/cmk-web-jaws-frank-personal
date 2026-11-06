using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Summary : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            kw.Focus(); //Keyword

            //DLC
            DLC.CompanyBrand(brand);

            DLC.StockLocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable1(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.StockActualPackaging
                 where
                    (
                    p.Packaging.Nama.Contains(kw.Text) ||
                    p.Packaging.Kode.Contains(kw.Text) ||
                    p.Packaging.Keterangan.Contains(kw.Text)
                    )
                 select p;

        rs = rs.OrderByDescending(p => p.Packaging.Tgl);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
            rs = rs.Where(p => p.IDLokasi == Cabang && p.TipeLokasi == tipe);
        else
            rs = rs.Where(p => p.TipeLokasi == tipe);

        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.Packaging.IDBrand == Brand);

        if (rs.Count() > 0)
        {
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                string Lokasi = "";

                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Satuan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.InTransit).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Damaged).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.NeverArrived).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected void fillTable1(object sender, EventArgs e)
    {
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        int Brand = Convert.ToInt32(brand.SelectedValue);
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"create table #tempLoc(IDLokasi int, NamaLokasi varchar(100), TipeLokasi int,  IDBrand int)

                            insert into #tempLoc
                            select ID,Nama,0 tipelokasi,IDBrand from LocWarehouse

                            insert into #tempLoc
                            select ID,Nama,1 tipelokasi,IDBrand from LocOutlet                            

                            select b.Nama,b.Kode,c.NamaLokasi,b.Satuan,a.InHand,a.InTransit,a.Damaged,a.NeverArrived
                            from StockActualPackaging a
                            JOIN Packaging b on b.ID=a.IDProduct
                            JOIN #tempLoc c on c.IDLokasi=a.IDLokasi and c.TipeLokasi=a.TipeLokasi
                            WHERE (ISNULL(@kw,'') = '' or b.nama = @kw or b.Kode = @kw or b.Keterangan = @kw)
                            and (@lokasi = 0  or c.IDLokasi=@lokasi) and (@tipelokasi < 0 or c.TipeLokasi = @tipelokasi )
                            and (@brand = 0  or c.IDBrand = @brand)
                            order by a.Tgl desc
                            drop table #tempLoc                
                            ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 1;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = kw.Text });
                command.Parameters.Add(new SqlParameter("@tipelokasi", SqlDbType.Int) { Value = tipe });
                command.Parameters.Add(new SqlParameter("@lokasi", SqlDbType.Int) { Value = Cabang });
                command.Parameters.Add(new SqlParameter("@brand", SqlDbType.Int) { Value = Brand });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int no = 0;

                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        no++;
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Nama"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Kode"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Satuan"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["InHand"] != DBNull.Value ? Convert.ToDecimal(row["InHand"]).ToString("N0") : "0";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["InTransit"] != DBNull.Value ? Convert.ToDecimal(row["InTransit"]).ToString("N0") : "0";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Damaged"] != DBNull.Value ? Convert.ToDecimal(row["Damaged"]).ToString("N0") : "0";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NeverArrived"] != DBNull.Value ? Convert.ToDecimal(row["NeverArrived"]).ToString("N0") : "0";
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
                else
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    td.Text = "No data available";
                    td.ColumnSpan = 8;
                    td.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(td);
                    tb.Rows.Add(tr);
                }

            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&cabang=" + cabang.Text;
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }
}