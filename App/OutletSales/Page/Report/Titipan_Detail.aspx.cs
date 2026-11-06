using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Detail : System.Web.UI.Page
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
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            string tglPengembalian = "";
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../Create/Titipan//Print_Receipt.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            //Trans. Form
            string Brand = "";
            if (d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            else if (d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi, Db.Cnn_DB);
            
            tglPengembalian = Db.SingleString("SELECT ISNULL(FORMAT(tglPengembalian, 'dddd, dd MMMM yyyy', 'id-ID'),'-') as TanggalPengembalian FROM DocTitipan WHERE ID=" + myID, Db.Cnn_DB);

            brand.InnerHtml = Brand;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama;            
            tgl.InnerHtml = Str.Tgl(d.Tgl);
            tglBalikin.InnerHtml = tglPengembalian;

            //Stone
            fillItem();
        }
    }

    //Stone
    protected void fillItem()
    {
        var rs = from p in db.DocTitipan_Product
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.DocTitipan.Nomor;
            tr.Cells.Add(td);

            string ProductItem = "";
            string PLU = "";
            string Image = "";

            if (r.IDProduct.HasValue)
            {
                ProductItem = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                PLU = r.StockProductDJ.Nomor;
                Image = r.ImgPicture;
            }
            else if (r.IDProductTitipan.HasValue)
            {
                ProductItem = r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
                PLU = r.StockProductDJCustomer.Nomor;
                Image = r.StockProductDJCustomer.ImgPicture;
            }

            td = new TableCell();
            td.Text = ProductItem;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = PLU;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<img src=\"/Images.aspx?path=" + Image + "\" border=\"0\" width=\"150\" />";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            index++;
        }
    }

    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }

    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
}