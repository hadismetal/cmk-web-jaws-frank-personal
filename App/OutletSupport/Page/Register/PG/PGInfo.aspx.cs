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
			
			fillHeader(sender,e);
            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError()
    {
        Response.Redirect("PG.aspx");
    }

    //Form
    protected void fillForm()
    {
        var d = db.PromoPG.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
    }
    protected void fillProduct()
    {

    }
	
	//Display Header
    protected void fillHeader(object sender, EventArgs e)
    {
        var head = db.PromoPG.SingleOrDefault(p => p.ID == myID);

        DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
        DateTime myDate2 = DateTime.Now;
        TimeSpan timeSpanResult;
        timeSpanResult = myDate1 - myDate2;
        double days = Math.Ceiling(timeSpanResult.TotalDays);
        string result = "";
        if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
        else if (days == 0) { result = "Today is the last day!"; }
        else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

        lit_header1.Text += "<table style='border:none;'>"
                    + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                    + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                    + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                    + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                    + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>"
                    + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                    + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                    + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "")
                    + "</td></tr>" 
                    + "</table>";
        lit_header2.Text += "<table style='border:none;'>"
                    + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                    + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                    + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                    
                    + "</table>";
    }

    protected void btn_inputproduct_Click(object sender, EventArgs e)
    {
        var rsH = db.PromoPG.SingleOrDefault(p => p.ID == myID);

        lit_Message.Text = "";
        string input = inputproduct.Text.Trim();

        try
        {
            var rs = db.StockProductPG.SingleOrDefault(p => p.Nomor.Equals(input));

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
                if (rs.StockActualPG.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rs.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rs.StockActualPG.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rs.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rs.StockActualPG.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rs.StockActualPG.IDLokasi, Db.Cnn_DB);
                string v = rs.ID.ToString();
                string t = Logistic.ProductList_PG(
                            rs.CharProductItem.Nama,
                            rs.Nomor,
                            rs.ImgPicture,
                            DL.SubStorage(rs.StockActualPG.Substorage),
                            Str.Number2(rs.NettoWeight),
                            Str.Number2(rs.GrossWeight),
                            rs.CharGoldModel.Nama,
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
        var rs1 = db.StockProductPG.SingleOrDefault(p => p.Nomor == input);

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
                var d = db.StockProductPG.SingleOrDefault(p => p.ID == Convert.ToInt32(cbl_Product.Items[i].Value));

                PromoPG_Detail n = new PromoPG_Detail();

                n.IDForm = myID;
                n.IDProduct = Convert.ToInt32(cbl_Product.Items[i].Value);
                n.IDModel = d.Model;
                n.IDItem = d.ProductItem;
                n.IDLevel = d.ProductLevel;
                n.IDLokasi = d.StockActualPG.IDLokasi;
                n.TipeLokasi = d.StockActualPG.TipeLokasi;

                db.PromoPG_Detail.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }

        var rsH = db.PromoPG.SingleOrDefault(p => p.ID == myID);
        rsH.Draft = false;
        //Commit
        db.SubmitChanges();

        //Redirect
        Response.Redirect("PG.aspx?d=1");

    }
}

