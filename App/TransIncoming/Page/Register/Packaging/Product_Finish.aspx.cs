using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DJ_Finish : System.Web.UI.Page
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
            cbl_Product.Enabled = true;

            cbl_Product.Items.Clear();

            //Form-Default
            // Fill();
            Fill1();

            cbl_Product.Enabled = false;
        }
    }
    //Fill
    protected void Fill()
    {
        
        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPackaging.Nomor;
        //brand.InnerText = rsH.StockOutgoingPackaging.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingPackaging.TipeTujuan);
        from.InnerText = rsH.StockOutgoingPackaging.NamaAsal;
        to.InnerText = rsH.StockOutgoingPackaging.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.Tgl);
        kurir.InnerText = rsH.StockOutgoingPackaging.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingPackaging.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingPackaging.TglETA);

        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;
    }
    protected void Fill1()
    {
        // var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;        
        // item = item.Where(p => p.SubStorageResult == p.SubStorageTo);
        string itemQuery = @"
        select b.Nomor,  c.Nama as nmBrand, b.TipeAsal, b.IDAsal,b.NamaAsal
        , b.TipeTujuan, b.IDTujuan, b.NamaTujuan, b.Keterangan
        , FORMAT(CONVERT(DATETIME, b.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, b.NamaKurir
        , FORMAT(CONVERT(DATETIME, b.TglETA, 113), 'dddd, dd MMMM yyyy', 'id-ID') TglEta
        from StockIncomingPackaging a
        JOIN StockOutgoingPackaging b on b.ID=a.IDOutgoing
        JOIN CompanyBrand c on c.ID=b.IDBrand
        WHERE a.ID = @IDForm

        SELECT ba.ID AS IDPackaging, ba.Nama , ba.Kode, b.Qty, b.Received, b.Damaged
        FROM StockIncomingPackaging a 
        JOIN StockIncomingPackaging_Product b on b.IDForm=a.ID
        JOIN Packaging ba on ba.ID=b.IDProduct        
        WHERE b.IDForm = @IDForm AND b.SubStorageResult = b.SubStorageTo";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(itemQuery, connection))
            {
                command.Parameters.AddWithValue("@IDForm", id);

                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();

                adapter1.Fill(ds1);
                connection.Close();

                DataTable dataHead = ds1.Tables[0];
                DataTable dataProduct = ds1.Tables[1];

                int totalRow = dataProduct.Rows.Count; 

                DataRow rsh = dataHead.Rows[0];

                outgoing.InnerText = rsh["Nomor"].ToString();
                brand.InnerText = rsh["nmBrand"].ToString();
                lit_type_from.Text = DL.LocationType(Convert.ToInt32(rsh["TipeAsal"]));
                lit_type_to.Text = DL.LocationType(Convert.ToInt32(rsh["TipeTujuan"]));
                from.InnerText = rsh["NamaAsal"].ToString();
                to.InnerText = rsh["NamaTujuan"].ToString();

                tgl.InnerText = rsh["Tgl"].ToString();
                kurir.InnerText = rsh["NamaKurir"].ToString();
                if (string.IsNullOrEmpty(rsh["Keterangan"].ToString()))
                {
                    keterangan.InnerText = "-";
                }
                else
                {
                    keterangan.InnerText = rsh["Keterangan"].ToString();
                }
                eta.InnerText = rsh["TglEta"].ToString();               

                if (totalRow > 0)
                {
                    foreach (DataRow rs in dataProduct.Rows)
                    {
                        string IDPackaging = Convert.ToString(rs["IDPackaging"]);
                        string t = Logistic.ProductList_Packaging(
                                Convert.ToString(rs["Nama"]),
                                IDPackaging,
                                Str.Number0(rs["Qty"] != DBNull.Value ? Convert.ToDecimal(rs["Qty"]) : 0),
                                Str.Number0(rs["Received"] != DBNull.Value ? Convert.ToDecimal(rs["Received"]) : 0),
                                Str.Number0(rs["Damaged"] != DBNull.Value ? Convert.ToDecimal(rs["Damaged"]) : 0),
                                Str.Number0(Convert.ToDecimal(rs["Qty"]) - (Convert.ToDecimal(rs["Received"]) + Convert.ToDecimal(rs["Damaged"]))),
                                true,
                                true
                                );
                        cbl_Product.Items.Add(new ListItem(t, IDPackaging));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
                    }

                }
                
            }
        }

        // if (item.Count() > 0)
        // {
        //     foreach (var rs in item)
        //     {
        //         string v = rs.Packaging.ID.ToString();
        //         string t = Logistic.ProductList_Packaging(
        //                 rs.Packaging.Nama,
        //                 rs.Packaging.ID.ToString(),
        //                 Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
        //                 Str.Number0(rs.Received != null ? rs.Received.Value : 0),
        //                 Str.Number0(rs.Damaged != null ? rs.Damaged.Value : 0),
        //                 Str.Number0(rs.Qty - (rs.Received + rs.Damaged)),
        //                 true,
        //                 true
        //                 );
        //         cbl_Product.Items.Add(new ListItem(t, v));
        //         cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        //     }
        // }
    }
    // protected void Fill1()
    // {
    //     var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;
    //     item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

    //     if (item.Count() > 0)
    //     {
    //         foreach (var rs in item)
    //         {
    //             string v = rs.Packaging.ID.ToString();
    //             string t = Logistic.ProductList_Packaging(
    //                     rs.Packaging.Nama,
    //                     rs.Packaging.ID.ToString(),
    //                     Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
    //                     Str.Number0(rs.Received != null ? rs.Received.Value : 0),
    //                     Str.Number0(rs.Damaged != null ? rs.Damaged.Value : 0),
    //                     Str.Number0(rs.Qty - (rs.Received + rs.Damaged)),
    //                     true,
    //                     true
    //                     );
    //             cbl_Product.Items.Add(new ListItem(t, v));
    //             cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
    //         }
    //     }
    // }

    // save !!!
    protected void save_Click1(object sender, EventArgs e)
    {        
        string StockIncoming = @"
                            IF EXISTS (SELECT 1 FROM StockIncomingPackaging WHERE ID = @IDForm AND StatusTransIn = 0)
                            BEGIN
                                UPDATE StockIncomingPackaging
                                SET StatusTransIn = 1
                                WHERE ID = @IDForm 
                            END                                
                            

                            IF EXISTS (SELECT 1 FROM StockIncomingPackaging_Product WHERE IDForm = @IDForm AND Status = 0)
                            BEGIN
                                UPDATE StockIncomingPackaging_Product
                                SET Status = 1, Approval = @Approver, 
                                    ApprovalTgl = GETDATE()
                                WHERE IDForm = @IDForm AND Status = 0
                            END";


        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(StockIncoming, connection))
            {

                command.Parameters.AddWithValue("@IDForm", id);
                command.Parameters.AddWithValue("@Approver", App.UserNama);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }                           

        // var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        // if(rsH!=null)
        //     rsH.StatusTransIn = 1;

        // var rs = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

        // if (rs.Count() > 0)
        // {
        //     foreach (var item in rs)
        //     {
        //         item.Status = 1;
        //     }
        // }

        //Commit
        // d.SubmitChanges();

        //Redirect
        Response.Redirect("Product.aspx?d=1");
    }
    protected void save_Click(object sender, EventArgs e)
    {
        StockIncomingPackaging nH = new StockIncomingPackaging();
        StockIncomingPackaging_Product n = new StockIncomingPackaging_Product();

        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        if(rsH!=null)
            rsH.StatusTransIn = 1;

        var rs = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

        if (rs.Count() > 0)
        {
            foreach (var item in rs)
            {
                item.Status = 1;
            }
        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("Product.aspx?d=1");
    }
}