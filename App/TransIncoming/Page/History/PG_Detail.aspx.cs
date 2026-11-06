using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Detail : System.Web.UI.Page
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

    protected int id { get { return App.QueryInt(this); } } /// id trans in

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            cbl_Product.Enabled = true;
            cbl_Product_2.Enabled = true;
            cbl_Product_3.Enabled = true;

            cbl_Product.Items.Clear();
            cbl_Product_2.Items.Clear();
            cbl_Product_3.Items.Clear();

            //Form-Default
            Fill();
            Fill1();
            Fill2();
            Fill3();

            Calculate_QC_Summary();

            cbl_Product.Enabled = false;
            cbl_Product_2.Enabled = false;
            cbl_Product_3.Enabled = false;
        }
    }
    //Fill
    protected void Fill()
    {
        
        var rsH = d.StockIncomingPG.SingleOrDefault(p => p.ID == id);
        outgoing.InnerText = rsH.StockOutgoingPG.Nomor;
        brand.InnerText = rsH.StockOutgoingPG.CompanyBrand.Nama;
        lit_type_from.Text = DL.LocationType(rsH.StockOutgoingPG.TipeAsal);
        lit_type_to.Text = DL.LocationType(rsH.StockOutgoingPG.TipeTujuan);
        from.InnerText = rsH.StockOutgoingPG.NamaAsal;
        to.InnerText = rsH.StockOutgoingPG.NamaTujuan;

        tgl.InnerText = Str.Tgl(rsH.StockOutgoingPG.Tgl);
        kurir.InnerText = rsH.StockOutgoingPG.NamaKurir;
        keterangan.InnerText = rsH.StockOutgoingPG.Keterangan;
        eta.InnerText = Str.Tgl(rsH.StockOutgoingPG.TglETA);

        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;

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

    }
    protected void Fill1()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == p.SubStorageTo);

        if (item.Count() > 0)
        {
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
        }
    }
    protected void Fill2()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 96);

        if (item.Count() > 0)
        {
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
                cbl_Product_2.Items.Add(new ListItem(t, v));
                cbl_Product_2.Items[cbl_Product_2.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Fill3()
    {
        var item = from p in d.StockIncomingPG_Product where p.IDForm == id select p;
        item = item.Where(p => p.SubStorageResult == 99);

        if (item.Count() > 0)
        {
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
                cbl_Product_3.Items.Add(new ListItem(t, v));
                cbl_Product_3.Items[cbl_Product_3.Items.Count - 1].Selected = true;
            }
        }
    }
    protected void Calculate_QC_Summary()
    {
        bool flag_cbl1 = cbl_Product.Enabled;
        bool flag_cbl2 = cbl_Product_2.Enabled;
        bool flag_cbl3 = cbl_Product_3.Enabled;

        cbl_Product.Enabled = true;
        cbl_Product_2.Enabled = true;
        cbl_Product_3.Enabled = true;

        int cbl1 = 0;
        int cbl2 = 0;
        int cbl3 = 0;

        for (int i = 0; i < cbl_Product.Items.Count; i++) { if (cbl_Product.Items[i].Selected == true) cbl1++; }
        for (int i = 0; i < cbl_Product_2.Items.Count; i++) { if (cbl_Product_2.Items[i].Selected == true) cbl2++; }
        for (int i = 0; i < cbl_Product_3.Items.Count; i++) { if (cbl_Product_3.Items[i].Selected == true) cbl3++; }
        
        accepted.InnerText = cbl1.ToString();
        broken.InnerText = cbl2.ToString();
        troublelost.InnerText = cbl3.ToString();

        cbl_Product.Enabled = flag_cbl1;
        cbl_Product_2.Enabled = flag_cbl2;
        cbl_Product_3.Enabled = flag_cbl3;
    }

    // back
    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("PG.aspx");
    }

}