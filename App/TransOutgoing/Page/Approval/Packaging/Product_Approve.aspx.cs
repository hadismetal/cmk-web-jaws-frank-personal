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
            //Form-Default
            Fill1();
            
        }
    }
    //Fill
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id);
        //brand.InnerText = rsH.CompanyBrand.Nama;
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
            string t = Logistic.ProductList_Packaging(
                        rs.Packaging.Nama,
                        Convert.ToDecimal(rs.Qty).ToString("N0")
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
        cbl_Product.Enabled = false;
    }
    
    protected void Fill1()
    {
        cbl_Product.Items.Clear();
        string getOutgoingPK = @"select ID,IDBrand
                                ,IIF(TipeAsal = 0,'Warehouse', IIF(TipeAsal = 1,'Outlet', 'Exhibition'))  TipeAsal, NamaAsal
                                ,IIF(TipeTujuan = 0,'Warehouse', IIF(TipeTujuan = 1,'Outlet', 'Exhibition'))  TipeTujuan, NamaTujuan
                                ,NamaKurir, Keterangan
                                ,CONVERT(varchar(10),tgl, 105) Tanggal, CONVERT(varchar(10),TglETA, 105) tglETA                                
                                from StockOutgoingPackaging where id=@ID
                                

                                SELECT a.ID, a.IDForm, a.IDProduct, a.Qty,b.InHand,d.Nama as NamaPK,Kode,d.ID AS IDPackaging
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
                    string v = Convert.ToString(rs["IDPackaging"]);
                    string t = Logistic.ProductList_Packaging(
                                rs["NamaPK"].ToString(),
                                Convert.ToDecimal(rs["Qty"]).ToString("N0")
                                );
                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                }                
            }
        }       

        // var item = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;

        // foreach (var rs in item)
        // {
        //     string v = rs.Packaging.ID.ToString();
        //     string t = Logistic.ProductList_Packaging(
        //                 rs.Packaging.Nama,
        //                 Convert.ToDecimal(rs.Qty).ToString("N0")
        //                 );
        //     cbl_Product.Items.Add(new ListItem(t, v));
        //     cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        // }
        cbl_Product.Enabled = false;
    }
    
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.Packaging.SingleOrDefault(p => p.Kode == input);

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
        cbl_Product.Enabled = true;
        StockOutgoingPackaging nH = new StockOutgoingPackaging();
        StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product();

        string DocumentNumber = generateDocumentNumber_TransOut();

        var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1);
        rsH.StatusTransOut = 2; // 2 : Approved
        rsH.Nomor = DocumentNumber;
        rsH.Approval = App.UserNama;
        rsH.ApprovalTgl = DateTime.Now;

        var rs = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;

        foreach (var item in rs)
        {
            var x = d.StockActualPackaging.SingleOrDefault(p => p.IDProduct == item.IDProduct && p.TipeLokasi == item.StockOutgoingPackaging.TipeAsal && p.IDLokasi == item.StockOutgoingPackaging.IDAsal);
            if (x != null)
            {
                x.InHand = x.InHand - item.Qty.Value;
                x.InTransit = (x.InTransit.HasValue ? x.InTransit.Value : 0) + item.Qty.Value;
            }

            //item.StockActualPackagingDJ.Substorage = 98; // Product On The Way !!
            //item.StockActualPackagingDJ.InHand = 0; // InHand out

        }

        //Commit
        d.SubmitChanges();

        //Redirect
        //Response.Redirect("Product.aspx?d=1");
        string pageurl = "/TransOutgoing/Page/Report/Packaging/Product_Print.aspx?id=" + id;
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
        Response.Write("<script>window.location.href='Product.aspx?id=1';</script>");
        //Response.Redirect("/TransOutgoing/Page/Report/Packaging/Product_Print.aspx?id=" + id);
    }
    protected void save_Click1(object sender, EventArgs e)
    {
        cbl_Product.Enabled = true;

        string DocumentNumber = generateDocumentNumber_TransOut();

        string query = @"UPDATE StockOutgoingPackaging 
                        SET StatusTransOut = 2, Nomor=@DocumentNumber,
                        Approval=@Approval, ApprovalTgl=GETDATE()
                        WHERE ID = @ID;
                        
                        UPDATE AX SET AX.InHand = (AX.InHand - AX.Qty), AX.InTransit = (ISNULL(AX.InTransit,0) + AX.Qty)
                        FROM (
                            SELECT b.IDProduct,b.InTransit,b.InHand,a.Qty
                            FROM StockOutgoingPackaging_Product a
                            JOIN StockOutgoingPackaging aa on aa.ID=a.IDForm
                            JOIN StockActualPackaging b on b.IDProduct=a.IDProduct and b.TipeLokasi=aa.TipeAsal and b.IDLokasi=AA.IDAsal
                            WHERE IDForm = @ID
                        )AX
                        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@DocumentNumber", DocumentNumber);
                command.Parameters.AddWithValue("@Approval", App.UserNama);                

                command.ExecuteNonQuery();
                // SqlDataAdapter adapter = new SqlDataAdapter(command);
                // DataSet ds = new DataSet();
                // adapter.Fill(ds);

                // DataTable DataDetail = ds.Tables[1];
                
            }
            connection.Close();
        } 

        //Commit
        // d.SubmitChanges();

        //Redirect
        //Response.Redirect("Product.aspx?d=1");
        string pageurl = "/TransOutgoing/Page/Report/Packaging/Product_Print.aspx?id=" + id;
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
        Response.Write("<script>window.location.href='Product.aspx?id=1';</script>");
        //Response.Redirect("/TransOutgoing/Page/Report/Packaging/Product_Print.aspx?id=" + id);
    }


    //protected void reject_Click(object sender, EventArgs e)
    //{
    //    cbl_Product.Enabled = true;
    //    StockOutgoingPackaging nH = new StockOutgoingPackaging();
    //    StockOutgoingPackaging_Product n = new StockOutgoingPackaging_Product();

    //    var rsH = d.StockOutgoingPackaging.SingleOrDefault(p => p.ID == id && p.StatusTransOut == 1);
    //    rsH.StatusTransOut = 3; // 3 : Reject
    //    rsH.Approval = App.UserNama;
    //    rsH.ApprovalTgl = DateTime.Now;

    //    var rs = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;

    //    //foreach (var item in rs)
    //    //{
    //    //    item.StockActualPackagingDJ.Substorage = item.SubStorage; // return SubStorage Value !!!
    //    //}

    //    //Commit
    //    d.SubmitChanges();

    //    //Redirect
    //    Response.Redirect("Product.aspx?d=1");
    //}

    protected string generateDocumentNumber_TransOut()
    {
        string result = "";

        // ex : KRM/13/11/0001
        //      0123456789

        string v1 = "KRM/PC";
        string v2 = System.DateTime.Now.Year.ToString().Substring(2, 2);
        string v3 = System.DateTime.Now.Month.ToString("d2").PadLeft(2, '0');
        string v4 = "0001";

        var rs = from p in d.StockOutgoingPackaging
                 where p.Nomor != null && p.Nomor.Substring(7, 2) == v2 && p.Nomor.Substring(10, 2) == v3
                 select p;

        result = rs.Count() > 0 ? v1 + "/" + v2 + "/" + v3 + "/" + ((rs.Count() + 1).ToString()).PadLeft(4, '0')
                                : v1 + "/" + v2 + "/" + v3 + "/" + v4
                                ;

        return result;
    }
}