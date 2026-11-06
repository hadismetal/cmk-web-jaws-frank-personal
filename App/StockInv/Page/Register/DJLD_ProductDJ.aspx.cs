using System;
using System.Linq;
using System.Web.UI.WebControls;

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

        }
    }
    //Fill
    protected void Fill()
    {
        save.Visible = false;

        cbl_Product.Items.Clear();

        var rsH = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        //inputproduct.Text = "13/11/0018";
        inputproduct.Focus();

    }
    //InputProduct
    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        //try
        //{
        var rs = d.StockProductDJ.SingleOrDefault(p => p.Nomor == input && p.ReturnProduct == false && p.StatusLebur == false && p.StatusPenjualan == 0);


        string err_msg = valid(input, cbl_Product);
        if (err_msg == "")
        {
            var dj = from p in d.StockProductDJLD
                     where p.Draft.Value == false
                     && p.IDProductDJ == rs.ID
                     select p;
            if (dj.Count() == 0)
            {
                string v = rs.ID.ToString();
                string t = Logistic.ProductList_DJ(
                        rs.StockProductDJ_CharProduct.CharProductItem.Nama,
                        rs.Nomor,
                        rs.ImgPicture,
                        "",
                        Str.Number2(rs.StockProductDJ_CharProduct.NettoWeight),
                        Str.Number2(rs.StockProductDJ_CharProduct.GrossWeight),
                        Str.Number0(rs.StockProductDJ_CharProduct.StoneQty),
                        Str.Number3(rs.StockProductDJ_CharProduct.StoneCarat),
                        Str.Number0(rs.StockProductDJ_PricingMU.HargaRupiahRound)
                        );
                cbl_Product.Items.Add(new ListItem(t, v));
                cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;

                //upl.Update();

                if (cbl_Product.Items.Count > 0)
                    btn_inputproduct.Visible = false;
            }
            else
            {
                lit_Message.Text = "<span style='color:red;'>PLU already exists</span>";
            }

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
        //catch
        //{
        // error message
        //   lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        //}
    }

    protected void btn_inputproductld_Click(object sender, EventArgs e)
    {
        var rsH = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        lit_Message_ld.Text = "";
        string input = inputproduct_ld.Text.Trim();

        //try
        //{
        var dj = rsH.StockProductDJ;
        var rs = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input && p.ReturnProduct == false && p.StatusPenjualan == 0
            && (p.BookExpire.HasValue ? p.BookExpire < DateTime.Now : 1 == 1)
            //&& ((dj!=null) ? p.StockActualLD_Stone1B.TipeLokasi == dj.StockActualDJ.TipeLokasi && p.StockActualLD_Stone1B.IDLokasi == dj.StockActualDJ.IDLokasi : 1==1));
            );

        string err_msg = validLD(input, cbl_Product);
        if (err_msg == "")
        {
            var ld = from p in d.StockProductDJLD
                     where p.Draft.Value == false
                     && p.IDProductLD == rs.ID
                     select p;
            if (ld.Count() == 0)
            {
                string v = rs.ID.ToString();
                string t = Logistic.ProductList_LD(
                        rs.Nomor,
                        rs.ImgPicture,
                        "",
                        rs.TotalCarat.ToString("N3")
                        );
                cbl_Product_ld.Items.Add(new ListItem(t, v));
                cbl_Product_ld.Items[cbl_Product_ld.Items.Count - 1].Selected = true;

                //upl.Update();
            }
            else
            {
                lit_Message_ld.Text = "<span style='color:red;'>PLU already exists</span>";
            }

            inputproduct.Text = "";
            inputproduct.Focus();
        }
        else
        {
            // error message
            lit_Message_ld.Text = "<span style='color:red;'>" + err_msg + "</span>";
        }

        save.Visible = cbl_Product.Items.Count > 0 ? true : false;
        //}
        //catch
        //{
        // error message
        //   lit_Message.Text = "<span style='color:red;'>Invalid Product Code !!!</span>";
        //}
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

    //Save
    protected string validLD(string input, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == input);

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
        int index = 1;
        var rs = d.StockProductDJLD.SingleOrDefault(p => p.ID == id);

        if (cbl_Product.Items.Count > 0 && cbl_Product_ld.Items.Count > 0)
        {


            for (int i = 0; i < cbl_Product.Items.Count; i++)
            {
                if (cbl_Product.Items[i].Selected)
                {

                    for (int j = 0; j < cbl_Product_ld.Items.Count; j++)
                    {
                        if (cbl_Product_ld.Items[j].Selected)
                        {
                            if (index == 1)
                            {
                                rs.IDProductDJ = Convert.ToInt32(cbl_Product.Items[i].Value);
                                rs.IDProductLD = Convert.ToInt32(cbl_Product_ld.Items[j].Value);
                                rs.Draft = false;
                                rs.DraftDate = null;
                                d.SubmitChanges();
                            }
                            else
                            {
                                StockProductDJLD s = new StockProductDJLD();

                                s.IDProductDJ = Convert.ToInt32(cbl_Product.Items[i].Value);
                                s.IDProductLD = Convert.ToInt32(cbl_Product_ld.Items[j].Value);
                                s.Draft = false;
                                s.DraftDate = null;
                                s.Keterangan = rs.Keterangan;

                                d.StockProductDJLD.InsertOnSubmit(s);
                                d.SubmitChanges();
                            }
                            index++;
                        }
                    }


            //Redirect
                }
            }

            Response.Redirect("DJLD.aspx?d=1");
        }
        else
            lit_Message.Text = "<span style='color:red;'>Please select Diamond Jewellery and Loose Diamond</span>";

    }
}