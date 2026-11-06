using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.IO;

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

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        int vB = rsH.IDBrand;
        int vF = rsH.TipeAsal;
        int LokasiAsal = rsH.IDAsal;

        //var rs = from p in d.StockActualDJ
        //         where p.IDBrand == vB
        //        && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1 && p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false
        //        && !(from z in d.StockOutgoingDJ_Product
        //             where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1)
        //             && z.StockOutgoingDJ.IDAsal == LokasiAsal && z.StockOutgoingDJ.TipeAsal == vF
        //             select z.IDProduct).Contains(p.IDProduct)
        //         select p;

        var rs = from p in d.StockActualDJ
                 where p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusLebur == false && !p.StockProductDJ.Draft.Value
                           && !(from s in d.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.IDProduct)
                           && !(from s in d.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.IDProduct)
                            && !(from z in d.StockOutgoingDJ_Product
                                 where (z.StockOutgoingDJ.StatusTransOut == 2 || z.StockOutgoingDJ.StatusTransOut == 1)
                                 && z.StockOutgoingDJ.IDAsal == LokasiAsal && z.StockOutgoingDJ.TipeAsal == vF
                                 select z.IDProduct).Contains(p.IDProduct)
                    && p.TipeLokasi == vF && p.IDLokasi == LokasiAsal && p.InHand >= 1
                 select p;

        if (rs.Count() > 0)
        {
            foreach(var r in rs)
            {
            string v = r.StockProductDJ.ID.ToString();
            string t = Logistic.ProductList_DJ(
                    r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                    r.StockProductDJ.Nomor,
                    r.StockProductDJ.ImgPicture,
                    DL.SubStorage(r.Substorage),
                    Str.Number2(r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight),
                    Str.Number2(r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                    Str.Number0(r.StockProductDJ.StockProductDJ_CharProduct.StoneQty),
                    Str.Number3(r.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                    Str.Number0(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound)
                    );
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
            }
        }
        save.Visible = cbl_Product.Items.Count > 0 ? true : false;
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

    protected void save_Click(object sender, EventArgs e)
    {
        Db.Execute("DELETE FROM StockOutgoingDJ_Product WHERE IDForm=" + id, Db.Cnn_DB);

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