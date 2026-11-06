using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Print : System.Web.UI.Page
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
	protected int id { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		
        if (!IsPostBack) {
			Fill();
		}
	}
    protected void Error(string Message)
    {

    }
	protected void Fill() 
    {
        div_Print.Visible = false;
        div_Error.Visible = false;

        try
        {
            var rs = db.StockOutgoingPG.SingleOrDefault(p => p.ID == id);

            div_Print.Visible = true;
            div_Error.Visible = false;

            FillHeader();
            FillTable();
            FillItem();
            FillSubStorage();
        }
        catch (Exception ex)
        {
            div_Print.Visible = false;
            div_Error.Visible = true;

            lit_ErrMsg.Text = "<span style='color:red;'>Error !</span>";
        }

	}
	protected void FillHeader()
    {
        var rs = db.StockOutgoingPG.SingleOrDefault(p => p.ID == id);

        string A = "";
        string T = "";

        // Asal
        if (rs.TipeAsal == 0) 
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")"; 
        }
        else if (rs.TipeAsal == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        else if (rs.TipeAsal == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal);
            A = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }

        // Tujuan
        if (rs.TipeTujuan == 0)
        {
            var rsA = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocWarehouseGroup.Nama + ")";
        }
        else if (rs.TipeTujuan == 1)
        {
            var rsA = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocOutletGroup.Nama + ")";
        }
        else if (rs.TipeTujuan == 2)
        {
            var rsA = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDTujuan);
            T = rsA.Nama + " (" + rsA.LocExhibitionGroup.Nama + ")";
        }

        nodokumen.InnerText = rs.Nomor;
        tanggalkirim.InnerText = Str.Tgl(rs.Tgl);
        asal.InnerText = A;
        tujuan.InnerText = T;
    }
    protected void FillTable()
    {
        var rs = from p in db.StockOutgoingPG_Product where p.IDForm == id select p;

        TableHeaderRow htr;
        TableHeaderCell htd;

        TableRow tr;
        TableCell td;

        // Header

        htr = new TableHeaderRow();

        htd = new TableHeaderCell();
        htd.Text = "No";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "No PLU";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Kode Supplier";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Gross Weight";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        //htd = new TableHeaderCell();
        //htd.Text = "Total Butir";
        //htd.Attributes.Add("style", "border:solid 1px black");
        //htr.Cells.Add(htd);

        //htd = new TableHeaderCell();
        //htd.Text = "Total Carat";
        //htd.Attributes.Add("style", "border:solid 1px black");
        //htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Item";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Status";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Harga Jual";
        htd.Attributes.Add("style", "border:solid 1px black");
        htr.Cells.Add(htd);

        tb.Rows.Add(htr);

        // List

        int index = 1;

        decimal total_gross = 0m;
        decimal total_butir = 0m;
        decimal total_carat = 0m;
        decimal total_harga = 0m;
        
        foreach (var item in rs)
        {
            tr = new TableRow();

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = item.StockProductPG.Nomor; // PLU
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = item.StockProductPG.SupplierSJ; // Kode Supplier
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(item.StockProductPG.GrossWeight); // Gross Weight
            total_gross += Convert.ToDecimal(item.StockProductPG.GrossWeight);
            td.HorizontalAlign = HorizontalAlign.Right;
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = ""; // Total Butir
            //total_butir += 0m;
            //td.HorizontalAlign = HorizontalAlign.Right;
            //td.Attributes.Add("style", "border:solid 1px black");
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = ""; // Total Carat
            //total_carat += item.StockProductPG.StockProductPG_CharProduct.StoneCarat;
            //td.HorizontalAlign = HorizontalAlign.Right;
            //td.Attributes.Add("style", "border:solid 1px black");
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = item.StockProductPG.CharProductItem.Nama; // Item
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.SubStorage(item.SubStorage); // SubStorage
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(item.StockProductPG.TotalHarga); // Harga Product
            total_harga += Convert.ToDecimal(item.StockProductPG.TotalHarga);
            td.HorizontalAlign = HorizontalAlign.Right;
            td.Attributes.Add("style", "border:solid 1px black");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        // Grand Total
        tr = new TableRow();

        td = new TableCell();
        td.Text = "";
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "TOTAL"; // PLU
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = ""; // Kode Supplier
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number2(total_gross); // Gross Weight
        td.HorizontalAlign = HorizontalAlign.Right;
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        //td = new TableCell();
        //td.Text = Str.Number0(total_carat); // Total Butir
        //td.HorizontalAlign = HorizontalAlign.Right;
        //td.Attributes.Add("style", "border:solid 1px black");
        //tr.Cells.Add(td);

        //td = new TableCell();
        //td.Text = Str.Number3(total_carat); // Total Carat
        //td.HorizontalAlign = HorizontalAlign.Right;
        //td.Attributes.Add("style", "border:solid 1px black");
        //tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number0(index-1) + " " + (index > 1 ? "PCS" : "PC"); // Item
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = ""; // SubStorage
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number0(total_harga); // Harga Product
        td.HorizontalAlign = HorizontalAlign.Right;
        td.Attributes.Add("style", "border:solid 1px black");
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void FillItem()
    {
        var item = from p in db.StockOutgoingPG_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.StockProductPG.CharProductItem.Nama } into s
                                 select new
                                 {
                                     ProductItem = s.Key.Nama,
                                     Qty = s.Count()
                                 };

        TableRow tr;
        TableCell td;

        decimal total_item = 0m;

        // Summary
        foreach (var rsItem in group_product_item)
        {
            //product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")";

            tr = new TableRow();

            td = new TableCell();
            td.Text = rsItem.ProductItem;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = " = ";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(rsItem.Qty);
            total_item += rsItem.Qty;
            tr.Cells.Add(td);

            tb_item.Rows.Add(tr);
        }

        // Summary Total
        tr = new TableRow();

        td = new TableCell();
        td.Text = "<br>" + "TOTAL";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<br>" + " = ";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<br>" + Str.Number0(total_item);
        tr.Cells.Add(td);

        tb_item.Rows.Add(tr);
    }
    protected void FillSubStorage()
    {
        var item = from p in db.StockOutgoingPG_Product where p.IDForm == id select p;

        var group_product_item = from p in item
                                 group p by new { p.SubStorage } into s
                                 select new
                                 {
                                     ProductSubStorage = s.Key.SubStorage,
                                     Qty = s.Count()
                                 };

        TableRow tr;
        TableCell td;

        decimal total_item = 0m;

        // Summary
        foreach (var rsItem in group_product_item)
        {
            //product_item_summary.InnerHtml += rsItem.ProductItem + " (" + rsItem.Qty.ToString("0") + ")";

            tr = new TableRow();

            td = new TableCell();
            td.Text = DL.SubStorage(rsItem.ProductSubStorage);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = " = ";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(rsItem.Qty);
            total_item += rsItem.Qty;
            tr.Cells.Add(td);

            tb_substorage.Rows.Add(tr);
        }

        // Summary Total
        tr = new TableRow();

        td = new TableCell();
        td.Text = "<br>" + "TOTAL";
        td.HorizontalAlign = HorizontalAlign.Right;
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<br>" + " = ";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<br>" + Str.Number0(total_item);
        tr.Cells.Add(td);

        tb_substorage.Rows.Add(tr);
    }
}