using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_ProductAddr : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError()
    {
        Response.Redirect("GJ.aspx");
    }

    //Form
    protected void fillForm()
    {
        var d = db.PromoGJ.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
    }
    protected void fillProduct()
    {

    }

    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = db.PromoGJ.SingleOrDefault(p => p.ID == myID);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        try
        {
            var rs = db.StockProductGJ.SingleOrDefault(p => p.Nomor.Equals(input) && p.StockActualGJ.Substorage == 1);

            string curFile = @"" + rs.ImgPicture;
            string err_msg = valid(input, cbl_Product);
            //if (!File.Exists(curFile))
            //{
            //    err_msg = "Gambar Product tidak ada.";
            //    lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
            //}
            //else
            //{


            if (err_msg == "")
            {
                string Lokasi = "";
                if (rs.StockActualGJ.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rs.StockActualGJ.IDLokasi, Db.Cnn_DB);
                else if (rs.StockActualGJ.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rs.StockActualGJ.IDLokasi, Db.Cnn_DB);
                else if (rs.StockActualGJ.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rs.StockActualGJ.IDLokasi, Db.Cnn_DB);
                string v = rs.ID.ToString();
                string t = Logistic.ProductList_GJ(
                        rs.StockProductGJ_CharProduct.CharProductItem.Nama,
                        rs.Nomor,
                        rs.ImgPicture,
                        DL.SubStorage(rs.StockActualGJ.Substorage),
                        Str.Number2(rs.StockProductGJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductGJ_CharProduct.GrossWeight),
                        Lokasi
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();

                inputproduct.Text = "";
                inputproduct.Focus();
            }
            else
            {
                // error message
                lit_Message.Text = "<span style='color:red;'>" + err_msg + "</span>";
            }
            save.Visible = cbl_Product.Items.Count > 0 ? true : false;
            //}
        }
        catch
        {
            // error message
            lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        }
    }

    protected string valid(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = db.StockProductGJ.SingleOrDefault(p => p.Nomor == input);

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
        for (int i = 0; i < cbl_Product.Items.Count; i++)
        {
            if (cbl_Product.Items[i].Selected)
            {
                var d = db.StockProductGJ.SingleOrDefault(p => p.ID == Convert.ToInt32(cbl_Product.Items[i].Value));

                PromoGJ_Detail n = new PromoGJ_Detail();

                n.IDForm = myID;
                n.IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value);
                n.IDCategory = d.StockProductGJ_CharProduct.ProductCategory;
                n.IDItem = d.StockProductGJ_CharProduct.ProductItem;
                n.IDLevel = d.StockProductGJ_CharProduct.ProductLevel;
                n.IDLokasi = d.StockActualGJ.IDLokasi;
                n.TipeLokasi = d.StockActualGJ.TipeLokasi;

                db.PromoGJ_Detail.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        var rsH = db.PromoGJ.SingleOrDefault(p => p.ID == myID);
        rsH.Draft = false;
        //Commit
        db.SubmitChanges();

        //Redirect
        Response.Redirect("GJ.aspx?d=1");

    }
}

