using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Finish : System.Web.UI.Page
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

        var rsH = d.StockOutgoingPG.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.Nomor;
        brand.InnerText = rsH.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.TipeTujuan);
        from.InnerText = rsH.NamaAsal;
        to.InnerText = rsH.NamaTujuan;

        print.Visible = (rsH.StatusTransOut == 2) ? true : false;

        tgl.InnerText = Str.Tgl(rsH.Tgl);
        kurir.InnerText = rsH.NamaKurir;
        keterangan.InnerText = rsH.Keterangan;
        eta.InnerText = Str.Tgl(rsH.TglETA);

        var item = from p in d.StockOutgoingPG_Product where p.IDForm == id select p;

        //var group_product_item = from p in item
        //                         group p by new { p.StockProductPG.StockProductPG_CharProduct.CharProductItem.Nama } into s
        //                         select new
        //                         {
        //                             ProductItem = s.Key.Nama,
        //                             Qty = s.Count()
        //                         };

        totalqty.InnerText = item.Count().ToString();

        totalgross.InnerText = item.Sum(p => p.StockProductPG.GrossWeight).ToString();
        totalnetto.InnerText = item.Sum(p => p.StockProductPG.NettoWeight).ToString();
        //totalbutir.InnerText = "";// CalculateTotalButir(id).ToString();
        //totalcarat.InnerText = item.Sum(p => p.StockProductPG.StockProductPG_CharProduct.StoneCarat).ToString();
        product_item_summary.InnerText = "";

        //foreach (var rsItem in group_product_item)
        //{
        //    product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")";
        //}

        foreach (var rs in item)
        {
            string v = rs.StockProductPG.ID.ToString();
            string t = "<div>"
                    + "<div style='float:left; padding:5px;'>"
                    + "<div style='border-bottom:solid 1px black;'>" + rs.StockProductPG.Nomor + "</div>"
                    + "<div><div style='float:left; width:15px;'>N</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductPG.NettoWeight.ToString() + "</div><div style='clear:both;'></div></div>"
                    + "<div><div style='float:left; width:15px;'>G</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductPG.GrossWeight.ToString() + "</div><div style='clear:both;'></div></div>"
                    //+ "<div><div style='float:left; width:15px;'>B</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + "" + "</div><div style='clear:both;'></div></div>"
                    //+ "<div><div style='float:left; width:15px;'>C</div><div style='float:left; width:10px;'>&nbsp;:&nbsp;</div><div style='float:left; width:60px; text-align:right;'>" + rs.StockProductPG.StockProductPG_CharProduct.StoneCarat.ToString() + "</div><div style='clear:both;'></div></div>"
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
    

    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("PG.aspx");
        
    }

    protected void print_Click(object sender, EventArgs e)
    {
        
        //Redirect
        Response.Redirect("PG_Print.aspx?id=" + id);
    }
}