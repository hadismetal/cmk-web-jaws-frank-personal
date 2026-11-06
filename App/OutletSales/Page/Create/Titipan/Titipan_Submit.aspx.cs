using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Titipan_Submit : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}
        fillForm();
	}

	//Exception
	protected void DataError() { Response.Redirect("Titipan.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
            string Brand = "";
            if(d.TipeLokasi == 1)
                Brand = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDLokasi,Db.Cnn_DB);
            else if(d.TipeLokasi == 2)
                Brand = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDLokasi,Db.Cnn_DB);

            brand.InnerHtml = Brand;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            customer.InnerHtml = d.DataCustomer.Nama;
            agent.InnerHtml = d.DataSales.Nama;
			tgl.InnerHtml = Str.Tgl(d.Tgl);
			
			//Stone
			fillItem();

		}
	}

	//Stone
	protected void fillItem() {
        decimal GT = 0m;
        //Item
        fillItemDJ(ref GT);

	}

    //Item
    protected void fillItemDJ(ref decimal GT)
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
            td.Text = r.IDProduct.HasValue ? r.StockProductDJ.Nomor : r.StockProductDJCustomer.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.IDProduct.HasValue ? r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama : r.StockProductDJCustomer.StockProductDJCustomer_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            tr.Cells.Add(td);

            string path = "";
            path = r.IDProduct.HasValue ? r.ImgPicture : r.StockProductDJCustomer.ImgPicture;

            td = new TableCell();
            td.Text = "<img src=\"/Images.aspx?path=" + path + "\" border=\"0\" width=\"100\" />";
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

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {

			//Status
			d.Draft = false;
            d.DraftDate = null;
            //d.StatusTitipan = 1;
            d.StatusTitipan = 6;

            //Commit
            db.SubmitChanges();

			//Redirect
			Response.Redirect("Print_Receipt.aspx?d=1&id=" + myID);
		}
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
	}

}