using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DJ_Form : System.Web.UI.Page
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
            Fill();

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
                        Str.Number0(rs.Qty)
                        );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
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

    protected void save_Click1(object sender, EventArgs e)
    {
        int IDIncoming = 0;

        string InsertHeadIncoming = @"
        			DECLARE @InsertedIDs TABLE (IDIncoming INT, Total INT);

                    IF NOT EXISTS(SELECT TOP 1 * FROM StockIncomingPackaging WHERE IDOutgoing = @IDOutgoing)
                    BEGIN
                        INSERT INTO StockIncomingPackaging (IDOutgoing, Nomor, Tgl, Keterangan, StatusTransIn, Operator, OperatorTgl)
                        OUTPUT INSERTED.ID, 1 INTO @InsertedIDs
                        SELECT @IDOutgoing, NULL, GETDATE(), NULL, 0, @Operator, GETDATE()
            
                        SELECT TOP 1 IDIncoming, Total FROM @InsertedIDs

                        SELECT * FROM StockOutgoingPackaging_Product WHERE IDForm = @IDOutgoing

                    END
                    ELSE
                    BEGIN
                        INSERT INTO @InsertedIDs
                        select ID, 0 as Total FROM StockIncomingPackaging WHERE IDOutgoing = @IDOutgoing
            
                        SELECT TOP 1 IDIncoming, Total FROM @InsertedIDs
                    END
        ";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(InsertHeadIncoming, connection))
            {
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("@IDOutgoing", id);
                command.Parameters.AddWithValue("@Operator", App.UserNama);

                // command.ExecuteNonQuery();
                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);

                DataTable dataHead = ds1.Tables[0];
                DataTable dataProduct = ds1.Tables[1];

                if (dataHead.Rows[0]["Total"].ToString() == "0")
                {
                    IDIncoming = Convert.ToInt32(dataHead.Rows[0]["IDIncoming"]);
                    Response.Redirect("Product_QC.aspx?id=" + IDIncoming);
                }
                else
                {
                    IDIncoming = Convert.ToInt32(dataHead.Rows[0]["IDIncoming"]);
                    string queryInsertDetail = @"
                    INSERT INTO StockIncomingPackaging_Product (IDForm, IDProduct, SubStorageResult, Status, Operator, OperatorTgl, Qty, Received, Damaged, NeverArrived, SubStorageTo)
                    SELECT @IDIncoming, IDProduct, NULL, 0, @Operator, GETDATE(), Qty, 0, 0, 0, 0
                    FROM StockOutgoingPackaging_Product
                    WHERE IDForm = @IDOutgoing
                    ";

                    using (SqlCommand command1 = new SqlCommand(queryInsertDetail, connection))
                    {
                        command1.CommandTimeout = 30;
                        command1.Parameters.AddWithValue("@IDIncoming", IDIncoming);
                        command1.Parameters.AddWithValue("@IDOutgoing", id);
                        command1.Parameters.AddWithValue("@Operator", App.UserNama);

                        // Execute the insert command
                        command1.ExecuteNonQuery();                        
                        Response.Redirect("Product_QC.aspx?id=" + IDIncoming);
                    }
                }
            }
            connection.Close();
        }        
    }
    protected void save_Click(object sender, EventArgs e)
    {
        int IDIncoming = 0;

        var x = d.StockIncomingPackaging.SingleOrDefault(p => p.IDOutgoing == id);

        var rs = from p in d.StockOutgoingPackaging_Product where p.IDForm == id select p;

        if (x != null)
        {
            IDIncoming = x.ID;
        }
        else
        {
            StockIncomingPackaging nH = new StockIncomingPackaging();

            //Auto-Input Header
            nH.IDOutgoing = id;
            nH.Nomor = null;
            nH.Tgl = System.DateTime.Now;
            nH.Keterangan = null;
            nH.StatusTransIn = 0; // Draft

            //User-Info
            nH.Operator = App.UserNama;
            nH.OperatorTgl = DateTime.Now;

            //Commit
            d.StockIncomingPackaging.InsertOnSubmit(nH);
            d.SubmitChanges();

            IDIncoming = nH.ID;

            //Auto-Input List Product
            foreach (var item in rs)
            {
                StockIncomingPackaging_Product n = new StockIncomingPackaging_Product()
                {
                    //Input
                    IDForm = nH.ID,
                    IDProduct = item.IDProduct,
                    //SubStorageTo = item.SubStorage,
                    SubStorageResult = null,
                    Status = 0, // New
                    Operator = App.UserNama,
                    OperatorTgl = DateTime.Now,
                    Qty = item.Qty,
                    Received = 0,
                    Damaged = 0,
                    NeverArrived = 0,
                    SubStorageTo = 0
                };
                d.StockIncomingPackaging_Product.InsertOnSubmit(n);
                d.SubmitChanges();

            }
        }

        //Redirect
        Response.Redirect("Product_QC.aspx?id=" + IDIncoming);

    }

}