using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class DJ_Product : System.Web.UI.Page
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
            FillProduct();
        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        //inputproduct.Text = "13/11/0018";
        inputproduct.Focus();

    }
    
     protected void FillProduct()
    {
        /// update form jika produk sudah tidak ada di lokasi asal maka dihapus dari daftar
        string getProductOutgoingDJ = @"
                    SELECT c.ID,c.Nomor, c.ImgPicture, d.Substorage, caa.Nama as Item,
                    ca.NettoWeight, ca.GrossWeight, ca.StoneQty, ca.StoneCarat, 
                    cb.HargaRupiahRound
                    FROM StockOutgoingDJ a 
                    JOIN StockOutgoingDJ_Product b on b.IDForm = a.ID
                    JOIN StockProductDJ c on c.ID=b.IDProduct
                    JOIN StockProductDJ_CharProduct ca on ca.ID=c.ID
                    JOIN CharProductItem caa on caa.ID=ca.ProductItem
                    JOIN StockProductDJ_PricingMU cb on cb.ID=c.ID
                    JOIN StockActualDJ d on d.IDProduct = c.ID
                    WHERE a.ID=@ID AND a.StatusTransOut = 0 and d.InHand=1

                    IF EXISTS(SELECT A.IDProduct FROM StockOutgoingDJ_Product A JOIN StockActualDJ B ON B.IDProduct = A.IDProduct WHERE A.IDForm = @ID and b.InHand = 0)
                    BEGIN                                            
                        DELETE A 
                        FROM (
                                SELECT * FROM StockOutgoingDJ_Product 
                                WHERE IDForm = @ID 
                                and IDProduct in (SELECT A.IDProduct FROM StockOutgoingDJ_Product A JOIN StockActualDJ B ON B.IDProduct = A.IDProduct WHERE A.IDForm = @ID and b.InHand = 0)
                            ) A
                    END

        ";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(getProductOutgoingDJ, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(cmd);

                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                DataTable data = ds1.Tables[0];

                foreach (DataRow r in data.Rows)
                {
                    //var rs = d.StockActualDJ.SingleOrDefault(p => p.StockProductDJ.Nomor == r.StockProductDJ.Nomor && p.TipeLokasi == r.StockOutgoingDJ.TipeAsal && p.InHand >= 1 );


                    string v = Convert.ToInt32(r["ID"]).ToString();
                    string t = Logistic.ProductList_DJ(
                            Convert.ToString(r["Item"]),
                            Convert.ToString(r["Nomor"]),
                            Convert.ToString(r["ImgPicture"]),
                            DL.SubStorage(Convert.ToInt32(r["SubStorage"])),
                            Str.Number2(Convert.ToDecimal(r["NettoWeight"])),
                            Str.Number2(Convert.ToDecimal(r["GrossWeight"])),
                            Str.Number0(Convert.ToDecimal(r["StoneQty"])),
                            Str.Number3(Convert.ToDecimal(r["StoneCarat"])),
                            Str.Number0(Convert.ToDecimal(r["HargaRupiahRound"]))
                            );
                    cbl_Product.Items.Add(new ListItem(t, v));
                    cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                }
                
                conn.Close();
            }
        }        

        inputproduct.Text = "";
        inputproduct.Focus();

        save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        
    }

    // protected void FillProduct()
    // {
    //     /// update form jika produk sudah tidak ada di lokasi asal maka dihapus dari daftar
    //     var item = from p in d.StockOutgoingDJ_Product
    //              where p.IDForm == id && p.StockOutgoingDJ.StatusTransOut == 0
    //              select p;

    //     var inhand0 = from p in item where p.StockActualDJ.InHand != 1 select p;
    //     var inhand1 = from p in item where p.StockActualDJ.InHand == 1 select p;

    //     foreach (var s in inhand0) { d.StockOutgoingDJ_Product.DeleteOnSubmit(s); d.SubmitChanges(); }

    //     foreach (var r in inhand1)
    //     {
    //         //var rs = d.StockActualDJ.SingleOrDefault(p => p.StockProductDJ.Nomor == r.StockProductDJ.Nomor && p.TipeLokasi == r.StockOutgoingDJ.TipeAsal && p.InHand >= 1 );

    //         var rs = r.StockActualDJ;

    //         string v = rs.StockProductDJ.ID.ToString();
    //         string t = Logistic.ProductList_DJ(
    //                 rs.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
    //                 rs.StockProductDJ.Nomor,
    //                 rs.StockProductDJ.ImgPicture,
    //                 DL.SubStorage(rs.Substorage),
    //                 Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
    //                 Str.Number2(rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
    //                 Str.Number0(rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
    //                 Str.Number3(rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
    //                 Str.Number0(rs.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
    //                 );
    //         cbl_Product.Items.Add(new ListItem(t, v));
    //         cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

    //     }

    //     inputproduct.Text = "";
    //     inputproduct.Focus();

    //     save.Visible = cbl_Product.Items.Count > 0 ? true : false;

    // }

    //InputProduct

    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        Literal plit_Message = lit_Message;
        TextBox pinputproduct = inputproduct;
        System.Web.UI.HtmlControls.HtmlButton psave = save;
        CheckBoxList pcbl_Product = cbl_Product;
        int pid = id;
        string Mode = "update";

        Lib.FormRegisterUpdateDokumenDraft_DJ(plit_Message, pinputproduct, psave, pcbl_Product, pid, Mode);
    }
    protected decimal CalculateTotalButir(int idproduct)
    {
        decimal result = 0m;

        var rs1a = from p in d.StockProductDJ_Stone1A where p.StockProductDJ.ID == idproduct select p;
        var rs1b = from p in d.StockProductDJ_Stone1B where p.StockProductDJ.ID == idproduct select p;
        var rs2 = from p in d.StockProductDJ_Stone2 where p.StockProductDJ.ID == idproduct select p;
        var rs3 = from p in d.StockProductDJ_Stone3 where p.StockProductDJ.ID == idproduct select p;
        var rs4 = from p in d.StockProductDJ_Stone4 where p.StockProductDJ.ID == idproduct select p;
        var rs5 = from p in d.StockProductDJ_Stone5 where p.StockProductDJ.ID == idproduct select p;

        foreach (var i1a in rs1a) { result += i1a.TotalButir; }
        foreach (var i1b in rs1b) { result += i1b.TotalButir; }
        foreach (var i2 in rs2) { result += i2.TotalButir; }
        foreach (var i3 in rs3) { result += i3.TotalButir; }
        foreach (var i4 in rs4) { result += i4.TotalButir; }
        foreach (var i5 in rs5) { result += i5.TotalButir; }

        return result;
    }
    protected decimal CalculateTotalCarat(int idproduct)
    {
        decimal result = 0m;

        var rs1a = from p in d.StockProductDJ_Stone1A where p.StockProductDJ.ID == idproduct select p;
        var rs1b = from p in d.StockProductDJ_Stone1B where p.StockProductDJ.ID == idproduct select p;
        var rs2 = from p in d.StockProductDJ_Stone2 where p.StockProductDJ.ID == idproduct select p;
        var rs3 = from p in d.StockProductDJ_Stone3 where p.StockProductDJ.ID == idproduct select p;
        var rs4 = from p in d.StockProductDJ_Stone4 where p.StockProductDJ.ID == idproduct select p;
        var rs5 = from p in d.StockProductDJ_Stone5 where p.StockProductDJ.ID == idproduct select p;

        foreach (var i1a in rs1a) { result += i1a.TotalCarat; }
        foreach (var i1b in rs1b) { result += i1b.TotalCarat; }
        foreach (var i2 in rs2) { result += i2.TotalCarat; }
        foreach (var i3 in rs3) { result += i3.TotalCarat; }
        foreach (var i4 in rs4) { result += i4.TotalCarat; }
        foreach (var i5 in rs5) { result += i5.TotalCarat; }

        return result;
    }
    //Save
    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductDJ.SingleOrDefault(p => p.Nomor == input);

        if (rs1 == null) { message = "Invalid Product"; }
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
        Db.Execute("DELETE FROM StockOutgoingDJ_Product WHERE IDForm=" + id,Db.Cnn_DB);

        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var rs = d.StockActualDJ.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(cbl_Product.Items[i].Value));

                StockOutgoingDJ_Product n = new StockOutgoingDJ_Product()
                {
                    //Input
                    IDForm = id,
                    IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value),
                    SubStorage = rs.Substorage

                };
                d.StockOutgoingDJ_Product.InsertOnSubmit(n);
            }
        }

        //Commit
        d.SubmitChanges();

        //Redirect
        Response.Redirect("DJ_Finish.aspx?id=" + id);
        
    }
}