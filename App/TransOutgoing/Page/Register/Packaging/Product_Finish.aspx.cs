using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Product_Finish : System.Web.UI.Page
{
    private Map_AC _db;

    private Map_AC db
    {
        get
        {
            if (_db == null)
                _db = new Map_AC();
            return _db;
        }
    }

    private Map_DB _d;

    private Map_DB d
    {
        get
        {
            if (_d == null)
                _d = new Map_DB();
            return _d;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (d != null)
            d.Dispose();
        base.Dispose();
    }

    protected int id { get { return App.QueryInt(this); } } /// id trans out

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            //Form-Default
            Fill1();
            
        }
    }
    //Fill
    protected void Fill()
    {
        cbprod.Items.Clear();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.Company.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;

        foreach (var rs in item)
        {
            string v = rs.Packaging.ID.ToString();
            var q1 = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == rs.IDProduct && p.IDLokasi == rsH.IDAsal && p.TipeLokasi == rsH.TipeAsal);
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text= rs.Packaging.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = rs.Packaging.Kode;
            tr.Cells.Add(td);

            td = new TableCell();
            int q2 = Convert.ToInt32(q1.InHand);
            td.Text = q2.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            int q3 = Convert.ToInt32(rs.Qty);
            td.Text = q3.ToString();
            tr.Cells.Add(td);

            tb1.Rows.Add(tr);

        }
        cbprod.Enabled = false;
    }

    protected void Fill1()
    {
        cbprod.Items.Clear();

        string getOutgoingPK = @"select ID,IDBrand
                                ,IIF(TipeAsal = 0,'Warehouse', IIF(TipeAsal = 1,'Outlet', 'Exhibition'))  TipeAsal, NamaAsal
                                ,IIF(TipeTujuan = 0,'Warehouse', IIF(TipeTujuan = 1,'Outlet', 'Exhibition'))  TipeTujuan, NamaTujuan
                                ,NamaKurir, Keterangan
                                ,CONVERT(varchar(10),tgl, 105) Tanggal, CONVERT(varchar(10),TglETA, 105) tglETA                                
                                from StockOutgoingPackaging where id=@ID
                                

                                SELECT a.ID, a.IDForm, a.IDProduct, a.Qty,b.InHand,d.Nama as NamaPK,Kode
                                FROM StockOutgoingPackaging_Product a
                                JOIN StockOutgoingPackaging c ON a.IDForm = c.ID                                
                                JOIN StockActualPackaging b ON a.IDProduct = b.IDProduct and b.IDLokasi = c.IDAsal and b.TipeLokasi = c.TipeAsal                                
								JOIN Packaging d on d.ID=a.IDProduct
                                where IDForm=@ID;
                                
        ";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(getOutgoingPK, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.AddWithValue("@ID", id);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                connection.Close();

                DataTable DataHead = ds.Tables[0];
                DataTable DataDetail = ds.Tables[1];

                lit_type_from.Text = Convert.ToString(DataHead.Rows[0]["TipeAsal"]);
                lit_type_to.Text = Convert.ToString(DataHead.Rows[0]["TipeTujuan"]);
                from.InnerText = Convert.ToString(DataHead.Rows[0]["NamaAsal"]);
                to.InnerText = Convert.ToString(DataHead.Rows[0]["NamaTujuan"]);

                tgl.InnerText = Convert.ToString(DataHead.Rows[0]["Tanggal"]);
                kurir.InnerText = Convert.ToString(DataHead.Rows[0]["NamaKurir"]);
                keterangan.InnerText = Convert.ToString(DataHead.Rows[0]["Keterangan"]);
                eta.InnerText = Convert.ToString(DataHead.Rows[0]["tglETA"]);


                foreach (DataRow rs in DataDetail.Rows)
                {                    
                    TableRow tr = new TableRow();
                    TableCell td;
        
                    td = new TableCell();
                    td.Text= (string)rs["NamaPK"];
                    tr.Cells.Add(td);
        
                    td = new TableCell();
                    td.Text = (string)rs["Kode"];
                    tr.Cells.Add(td);
        
                    td = new TableCell();
                    int q2 = Convert.ToInt32(rs["InHand"]);
                    td.Text = q2.ToString();
                    tr.Cells.Add(td);
        
                    td = new TableCell();
                    int q3 = Convert.ToInt32(rs["Qty"]);
                    td.Text = q3.ToString();
                    tr.Cells.Add(td);
        
                    tb1.Rows.Add(tr);

                }
                cbprod.Enabled = false;
            }
        }                        
    }


    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.Packaging.SingleOrDefault(p => p.Kode == "" + input + "");

        if (rs1 == null) { message = "Invalid Product Code"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + input + " sudah terinput"; }
            }
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void save_Click(object sender, EventArgs e)
    {
        cbprod.Enabled = true;
        StockOutgoingPackaging nH = new StockOutgoingPackaging();
        StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 0);
        rsH.StatusTransOut = 1;

        /*var rs = from p in d.StockOutgoingPackaging_Product where p.IDForm == id && p.StockActual.Substorage == 97 select p;

        foreach (var item in rs)
        {
            item.StockActual.Substorage = 98;
        }*/

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("Product.aspx?d=1");
        
    }
    protected void save_Click1(object sender, EventArgs e)
    {
        cbprod.Enabled = true;
        string query = @"UPDATE StockOutgoingPackaging SET StatusTransOut = 1 WHERE ID = @ID;";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.AddWithValue("@ID", id);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }        
        // StockOutgoingPackaging nH = new StockOutgoingPackaging();
        // StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product();

        // var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 0);
        // rsH.StatusTransOut = 1;

        // /*var rs = from p in d.StockOutgoingPackaging_Product where p.IDForm == id && p.StockActual.Substorage == 97 select p;

        // foreach (var item in rs)
        // {
        //     item.StockActual.Substorage = 98;
        // }*/

        // //Commit
        // d.SubmitChanges();

        //Redirect
        Response.Redirect("Product.aspx?d=1");
        
    }
}