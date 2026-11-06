using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;

public partial class Page_Summary : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //Generate
            fillTable(sender, e);
        }

        Uri referrer = HttpContext.Current.Request.UrlReferrer;
        if (referrer != null)
        {
            string original = referrer.OriginalString.ToLower();
            go.HRef=original;
        }
        else
            go.HRef="Souvenir.aspx";
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var so = db.BundleItem.SingleOrDefault(p => p.ID == myID);

        if (so != null)
        {
           
            var ps = from p in db.Bundle_ItemPasangan
                     join q in db.BundleItem on p.IDForm equals q.ID
                     where p.IDForm == so.ID
                     orderby q.DraftDate ascending
                     select p;           
            
            int ind = 1;
            foreach (var r in ps)
            {
                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = ind.ToString();
                tr2.Cells.Add(td2);

                string Tipe_Product = "";
                if (r.TipeProduct == 1) Tipe_Product = "Diamond Jewelry";
                else if (r.TipeProduct == 2) Tipe_Product = "Gold Jewelry";
                else if (r.TipeProduct == 3) Tipe_Product = "Plain Gold";
                else if (r.TipeProduct == 4) Tipe_Product = "Loose Diamond";

                td2 = new TableCell();
                td2.Text = Tipe_Product;
                tr2.Cells.Add(td2);

                string PLU = "";
                if (r.TipeProduct == 1)
                {
                    PLU = Db.SingleString("SELECT Nomor FROM StockProductDJ WHERE ID=" + r.IDProduct, Db.Cnn_DB);
                }
                else if (r.TipeProduct == 2)
                {
                    PLU = Db.SingleString("SELECT Nomor FROM StockProductGJ WHERE ID=" + r.IDProduct, Db.Cnn_DB);
                }
                else if (r.TipeProduct == 3)
                {
                    PLU = Db.SingleString("SELECT Nomor FROM StockProductPG WHERE ID=" + r.IDProduct, Db.Cnn_DB);
                }
                else if (r.TipeProduct == 4)
                {
                    PLU = Db.SingleString("SELECT Nomor FROM StockProductLD_Stone1B WHERE ID=" + r.IDProduct, Db.Cnn_DB);
                }

                td2 = new TableCell();
                td2.Text = PLU;
                tr2.Cells.Add(td2);

                tb2.Rows.Add(tr2);

                ind++;
            }

        }
    }
}