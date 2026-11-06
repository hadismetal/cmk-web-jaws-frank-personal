using System;
using System.Linq;
using System.Web.UI.WebControls;

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
            Fill();
            
        }
    }
    //Fill
    protected void Fill()
    {
        cbl_Product.Items.Clear();

        var rsH = d.StockOutgoingDJ.SingleOrDefault(p => p.ID == id);
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        print.Visible = rsH.StatusTransOut != 2 ? false : true;

        var item = from p in d.StockOutgoingDJ_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                    group p by new { p.StockProductDJ.StockProductDJ_CharProduct.StoneQty } into s
                    select new
                    {
                        TotalButir = s.Key.StoneQty,
                        TotalCarat = s.Sum(y => y.StockProductDJ.StockProductDJ_CharProduct.StoneCarat),
                        TotalGross = s.Sum(y => y.StockProductDJ.StockProductDJ_CharProduct.GrossWeight),
                        TotalNetto = s.Sum(y => y.StockProductDJ.StockProductDJ_CharProduct.NettoWeight)
                    };

        totalqty.InnerText = item.Count().ToString();
        totalbutir.InnerText = Convert.ToDecimal(item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
        totalcarat.InnerText = item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat).ToString("0.000");
        totalgross.InnerText = item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("0.00");
        totalnetto.InnerText = item.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("0.00");
        product_item_summary.InnerText = "";

        foreach (var rsItem in group_product_item) // Ganti Query nya !!!
        {
            product_item_summary.InnerHtml += Convert.ToDecimal(rsItem.TotalButir).ToString("0")
                                            + " <br> " + rsItem.TotalCarat.ToString("0.000")
                                            + " <br> " + rsItem.TotalGross.ToString("0.00")
                                            + " <br> " + rsItem.TotalNetto.ToString("0.00")
                                            ;
        }

        foreach (var rs in item)
        {
            string v = rs.StockProductDJ.ID.ToString();
            string t = "<div>"
                    + "<div style='float:left; padding:5px;'>"
                    + "<div style='border-bottom:solid 1px black;'>" + rs.StockProductDJ.Nomor + "</div>"
                    + "<div><div style='float:left; width:15px;'>N</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductDJ.StockProductDJ_CharProduct.NettoWeight + "</div><div style='clear:both;'></div></div>"
                    + "<div><div style='float:left; width:15px;'>G</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductDJ.StockProductDJ_CharProduct.GrossWeight + "</div><div style='clear:both;'></div></div>"
                    + "<div><div style='float:left; width:15px;'>B</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductDJ.StockProductDJ_CharProduct.StoneQty + "</div><div style='clear:both;'></div></div>"
                    + "<div><div style='float:left; width:15px;'>C</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductDJ.StockProductDJ_CharProduct.StoneCarat + "</div><div style='clear:both;'></div></div>"
                    + "</div>"
                    + "<div style='float:left; padding:5px;'>"
                    + "" // product image
                    + "</div>"
                    + "<div style='clear:both;'></div>"
                    + "</div>"
                    ;
            cbl_Product.Items.Add(new ListItem(t, v));
            cbl_Product.Items[cbl_Product.Items.Count - 1].Selected = true;
        }
        cbl_Product.Enabled = false;
    }
    
    //Save
    protected string valid(string PLU, CheckBoxList cbl)
    {
        string message = "";

        // 1. Check apakah input string merupakan kode product / ada di database
        var rs1 = d.StockProductDJ.SingleOrDefault(p => p.Nomor == PLU);

        if (rs1 == null) { message = "Invalid PLU"; }
        else
        {
            // 2. Check Apakah Product Item sudah pernah terinput
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Value == rs1.ID.ToString()) { message = "Product Code " + PLU + " sudah terinput"; }
            }
        }

        // 3. Check Apakah product valid status substoragenya untuk dikirim ke gudang

        return message;
    }

    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("DJ.aspx");
        
    }

    protected void print_Click(object sender, EventArgs e)
    {
        
        //Redirect
        Response.Redirect("DJ_Print.aspx?id=" + id);
    }
}