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
    App gp = new App();
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();


        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;

        if (d.Count() > 0)
        {
            string Lokasi = "";
            var x = d.First();

            if (x.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            cabang.Items.Add(Lokasi);
        }
        else
        {
            cabang.Items.Add("ALL");
        }

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);
            DLC.CompanyBrand(brand);
            Ddl.Substorage(substorage);

            //DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(Request.QueryString["locationtype"]);

        int Lokasi = 0;
        try
        {
            Lokasi = Convert.ToInt32(Request.QueryString["location"]);
        }
        catch (Exception ex) { }

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        int Brand = Convert.ToInt32(brand.SelectedValue);
        int GroupBy = 0;
        string PLU = "";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_StockInventorySummary", connection))
            {
                command.CommandTimeout = 1000;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = PLU });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Lokasi });
                command.Parameters.Add(new SqlParameter("@ProductItem", SqlDbType.Int) { Value = ProductItem });
                command.Parameters.Add(new SqlParameter("@ProductCategory", SqlDbType.Int) { Value = ProductCategory });
                command.Parameters.Add(new SqlParameter("@Substorage", SqlDbType.Int) { Value = Substorage });
                command.Parameters.Add(new SqlParameter("@GroupBy", SqlDbType.Int) { Value = GroupBy });
                

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();

                    td = new TableCell();
                    td.Text = row["Keterangan"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Qty"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalHarga"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["GrossWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["NettoWeight"]).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);
                    tb3.Rows.Add(tr);
                }



                DataTable dt2 = ds.Tables[1];

                foreach (DataRow row in dt2.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();

                    td = new TableCell();
                    td.Text = row["Keterangan"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Jual"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Resell"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["TradeIn"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["TitipanCustomer"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Reparasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Lebur"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Hadiah"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["OngoingLebur"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Broken"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "0";
                    // td.Text = row["OngoingDraft"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Ongoing"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Lost"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Total"].ToString();
                    tr.Cells.Add(td);

                    tb4.Rows.Add(tr);
                }

                tb4.Visible = true;
            }
        }

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            brand.SelectedValue = Request.QueryString["brand"];
        }
    }

    protected object userLocation()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;

        string Lokasi = "";
        int IDLokasi = 0;
        int TipeLokasi = 9;
        if (d.Count() > 0)
        {
            var x = d.First();

            IDLokasi = x.IDLokasi;
            TipeLokasi = x.TipeLokasi;

            if (x.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + x.IDLokasi, Db.Cnn_DB);
            else
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + x.IDLokasi, Db.Cnn_DB);

            cabang.Items.Add(Lokasi);
        }
        else
        {
            Lokasi = "ALL";
            cabang.Items.Add(Lokasi);
        }

        return new
        {
            IDLokasi = IDLokasi,
            TipeLokasi = TipeLokasi
        };
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        dynamic user = userLocation();


        //Customize-Qstr
        href += "&location=" + user.IDLokasi;
        href += "&locationtype=" + user.TipeLokasi;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&brand=" + brand.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "SummaryStock_3_Print.aspx?id=1";

        //Customize-Qstr
        //href += "&cabang=" + cabang.Text;
        dynamic user = userLocation();


        //Customize-Qstr
        href += "&location=" + user.IDLokasi;
        href += "&locationtype=" + user.TipeLokasi;
        href += "&refreshPage=1";
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}