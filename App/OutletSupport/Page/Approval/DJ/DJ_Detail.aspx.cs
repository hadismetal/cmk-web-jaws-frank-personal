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
            go.HRef="Product.aspx";
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var so = db.PromoDJ.SingleOrDefault(p => p.ID == myID);

        if (so != null)
        {
            var rs = from p in db.PromoDJ_Detail
                     join q in db.PromoDJ on p.IDForm equals q.ID
                     where p.IDForm == so.ID
                     orderby q.DraftDate ascending
                     select p;
            
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString();
                tr.Cells.Add(td);

                string Category= "";
                Category = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.IDCategory, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Category;
                tr.Cells.Add(td);

                string Brand = "";
                Brand = Db.SingleString("SELECT Nama FROM CharProductLevel WHERE ID=" + r.IDLevel, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Brand;
                tr.Cells.Add(td);

                string Product = "";
                Product = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.IDItem, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Product;
                tr.Cells.Add(td);

                string PLU = "";
                
                td = new TableCell();
                td.Text = r.StockProductDJ.Nomor;
                tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                index++;
            }
        }
    }
}