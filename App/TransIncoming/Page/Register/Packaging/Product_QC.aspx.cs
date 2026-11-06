using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DJ_QC : System.Web.UI.Page
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
            // Fill();
            Fill1New(false);
            //Form-Default

        }

    }
    //Fill
    protected void Fill()
    {

        var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPackaging.Nomor;
        brand.InnerText = rsH.StockOutgoingPackaging.CompanyBrand.Nama;
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
    protected void Fill1New(bool reverse)
    {
        if (!reverse) { cbl_Product.Items.Clear(); }

        // var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;
        string getItem = @"
        select b.Nomor,  c.Nama as nmBrand, b.TipeAsal, b.IDAsal,b.NamaAsal
        , b.TipeTujuan, b.IDTujuan, b.NamaTujuan, b.Keterangan
        , FORMAT(CONVERT(DATETIME, b.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, b.NamaKurir
        , FORMAT(CONVERT(DATETIME, b.TglETA, 113), 'dddd, dd MMMM yyyy', 'id-ID') TglEta
        from StockIncomingPackaging a
        JOIN StockOutgoingPackaging b on b.ID=a.IDOutgoing
        JOIN CompanyBrand c on c.ID=b.IDBrand
        WHERE a.ID = @IDIncoming

        SELECT ba.ID AS IDPackaging, ba.Kode, b.Qty, b.Received, b.Damaged
        FROM StockIncomingPackaging a 
        JOIN StockIncomingPackaging_Product b on b.IDForm=a.ID
        JOIN Packaging ba on ba.ID=b.IDProduct
        where a.ID = @IDIncoming
        
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(getItem, connection))
            {
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@IDIncoming", id);

                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();

                adapter1.Fill(ds1);
                DataTable dataHead = ds1.Tables[0];
                DataTable dataProduct = ds1.Tables[1];
                connection.Close();
                DataRow rsh = dataHead.Rows[0];

                if (!reverse)
                {
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


                    foreach (DataRow rsP in dataProduct.Rows)
                    {
                        string IDPackaging = Convert.ToString(rsP["IDPackaging"]);
                        string t = LibNew.ProductList_Packaging(
                            Convert.ToString(rsP["Kode"]),
                            IDPackaging,
                            Str.Number0(rsP["Qty"] != DBNull.Value ? Convert.ToDecimal(rsP["Qty"]) : 0),
                            Str.Number0(rsP["Received"] != DBNull.Value ? Convert.ToDecimal(rsP["Received"]) : 0),
                            Str.Number0(rsP["Damaged"] != DBNull.Value ? Convert.ToDecimal(rsP["Damaged"]) : 0),
                            true
                        );

                        cbl_Product.Items.Add(new ListItem(t, IDPackaging));
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true; // default harus manual check
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = false;
                        cbl_Product.Items[cbl_Product.Items.Count - 1].Attributes["data-kode-" + IDPackaging] = IDPackaging;
                    }

                }
            }
        }

        // if (!reverse)
        // {
        //     foreach (var rs in item)
        //     {
        //         string v = rs.Packaging.ID.ToString();
        //         string t = LibNew.ProductList_Packaging(
        //                 rs.Packaging.Kode,
        //                 rs.Packaging.ID.ToString(),
        //                 Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
        //                 Str.Number0(rs.Received != null ? rs.Received.Value : 0),
        //                 Str.Number0(rs.Damaged != null ? rs.Damaged.Value : 0),
        //                 true
        //                 );
        //         cbl_Product.Items.Add(new ListItem(t, v));
        //         cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true; // default harus manual check
        //         cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = false;
        //         cbl_Product.Items[cbl_Product.Items.Count - 1].Attributes["data-kode-" + rs.Packaging.ID.ToString()] = rs.Packaging.ID.ToString();
        //     }
        // }
    }
    protected void Fill1(bool reverse)
    {
        if (!reverse) { cbl_Product.Items.Clear(); }

        var item = from p in d.StockIncomingPackaging_Product where p.IDForm == id select p;

        if (!reverse)
        {
            foreach (var rs in item)
            {
                string v = rs.Packaging.ID.ToString();
                string t = LibNew.ProductList_Packaging(
                        rs.Packaging.Kode,
                        rs.Packaging.ID.ToString(),
                        Str.Number0(rs.Qty != null ? rs.Qty.Value : 0),
                        Str.Number0(rs.Received != null ? rs.Received.Value : 0),
                        Str.Number0(rs.Damaged != null ? rs.Damaged.Value : 0),
                        true
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true; // default harus manual check
                cbl_Product.Items[cbl_Product.Items.Count - 1].Enabled = false;
                cbl_Product.Items[cbl_Product.Items.Count - 1].Attributes["data-kode-" + rs.Packaging.ID.ToString()] = rs.Packaging.ID.ToString();
            }
        }
    }

    // save !!!
    protected void nextF_Click1(object sender, EventArgs e)
    {    

        decimal qty_rcv = 0m;
        decimal qty_dmg = 0m;
        decimal qty_lst = 0m;

        bool isNext = true;

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                qty_rcv = Convert.ToDecimal(Request.Form["qty_rcv" + cbl_Product.Items[i].Value]);
                qty_dmg = Convert.ToDecimal(Request.Form["qty_dmg" + cbl_Product.Items[i].Value]);


                // var rs = d.StockIncomingPackaging_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingPackaging.ID == Convert.ToInt32(id));
                string incomingProduct = @"UPDATE A SET SubStorageResult = A.SubStorageTo, Received = @Received, Damaged = @Damaged, NeverArrived = @NeverArrived
                                            FROM StockIncomingPackaging_Product A
                                            WHERE A.IDForm = @IDForm AND A.IDProduct = @IDProduct
                                            
                                            SELECT b.Kode, b.ID, a.Qty, a.Received, a.Damaged 
                                            FROM StockIncomingPackaging_Product a
                                            JOIN Packaging b ON b.ID = a.IDProduct
                                            WHERE IDForm = @IDForm AND IDProduct = @IDProduct";
                using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(incomingProduct, connection))
                    {
                        command.Parameters.AddWithValue("@IDForm", id);
                        command.Parameters.AddWithValue("@IDProduct", Convert.ToInt32(cbl_Product.Items[i].Value));
                        command.Parameters.AddWithValue("@Received", qty_rcv);
                        command.Parameters.AddWithValue("@Damaged", qty_dmg);
                        command.Parameters.AddWithValue("@NeverArrived", qty_lst);

                        SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                        DataSet ds1 = new DataSet();

                        adapter1.Fill(ds1);
                        DataTable dataProduct = ds1.Tables[0];
                        connection.Close();

                        if (qty_rcv + qty_dmg == 0)
                        {
                            isNext = false;
                            error.Text = "Jumlah terima/rusak pada tiap barang harus diisi!";                
                            break;
                        }
                        else if (Convert.ToDecimal(dataProduct.Rows[0]["Qty"]) > (qty_rcv + qty_dmg))
                        {
                            isNext = false;
                            error.Text = "Jumlah terima/rusak kode packaging " + Convert.ToString(dataProduct.Rows[0]["Kode"]) + " tidak boleh kurang dari jumlah yang dikirim!";                
                            break;
                        }
                        else if (Convert.ToDecimal(dataProduct.Rows[0]["Qty"]) < (qty_rcv + qty_dmg))
                        {
                            isNext = false;
                            error.Text = "Jumlah terima/rusak kode packaging " + Convert.ToString(dataProduct.Rows[0]["Kode"]) + " tidak boleh lebih dari jumlah yang dikirim!";                
                            break;
                        }
                    }
                }                
            }
        }
        if (isNext)
        {
            Response.Redirect("Product_Finish.aspx?id=" + id);
        }
        else error.Visible = true;
    }
    protected void nextF_Click(object sender, EventArgs e)
    {
        //StockIncomingPackaging nH = new StockIncomingPackaging();
        StockIncomingPackaging_Product n = new StockIncomingPackaging_Product();

        //var rsH = d.StockIncomingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransIn == 0);
        //rsH.StatusTransIn = 1;

        decimal qty_rcv = 0m;
        decimal qty_dmg = 0m;
        decimal qty_lst = 0m;

        bool isNext = true;

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                qty_rcv = Convert.ToDecimal(Request.Form["qty_rcv" + cbl_Product.Items[i].Value]);
                qty_dmg = Convert.ToDecimal(Request.Form["qty_dmg" + cbl_Product.Items[i].Value]);


                var rs = d.StockIncomingPackaging_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value) && p.StockIncomingPackaging.ID == Convert.ToInt32(id));

                rs.SubStorageResult = rs.SubStorageTo;
                rs.Received = qty_rcv;
                rs.Damaged = qty_dmg;
                rs.NeverArrived = qty_lst;
                if (qty_rcv + qty_dmg == 0)
                {
                    isNext = false;
                    error.Text = "Jumlah terima/rusak pada tiap barang harus diisi!";
                    //cbl_Product.Items[i].Attributes["style"] = "border-style:solid;border-color:red";
                    break;
                }
                else if (Convert.ToDecimal(rs.Qty) > (qty_rcv + qty_dmg))
                {
                    isNext = false;
                    error.Text = "Jumlah terima/rusak kode packaging " + rs.Packaging.Kode + " tidak boleh kurang dari jumlah yang dikirim!";
                    //cbl_Product.Items[i].Attributes["style"] = "border-style:solid;border-color:red";
                    break;
                }
                else if (Convert.ToDecimal(rs.Qty) < (qty_rcv + qty_dmg))
                {
                    isNext = false;
                    error.Text = "Jumlah terima/rusak kode packaging " + rs.Packaging.Kode + " tidak boleh lebih dari jumlah yang dikirim!";
                    //cbl_Product.Items[i].Attributes["style"] = "border-style:solid;border-color:red";
                    break;
                }
            }
        }
        if (isNext)
        {
            //Commit
            d.SubmitChanges();

            //Redirect
            Response.Redirect("Product_Finish.aspx?id=" + id);
        }
        else error.Visible = true;
    }

}