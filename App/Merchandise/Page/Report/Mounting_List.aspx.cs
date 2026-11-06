using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Mounting : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductMounting
                 join q in db.StockReceiveMounting on p.Nomor equals q.Nomor
                 where q.StatusReceive == 0 && p.Draft == false
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor==""+kw.Text+"");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductMounting_CharProduct.ProductCategory == ProductCategory);

        if(orderby.SelectedValue=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductMounting_CharProduct.CharProductItem.Nama);

        if (Status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusReceive == true);
        else if (Status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusReceive == false);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        if (rs != null)
        {
            int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButirAll = 0m;
            decimal TotalCaratAll = 0m;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor!=null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualMounting.Substorage == 1)
                    Jual = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 3)
                    Exchange = r.StockActualMounting.InHand;
                else if (r.StockActualMounting.Substorage == 2)
                    Resell = r.StockActualMounting.InHand;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.StockActualMounting.InHand.ToString("N0");
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.StockActualMounting.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualMounting.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualMounting.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualMounting.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductMounting_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductMounting_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalHarga += Convert.ToDecimal(r.StockProductMounting_PricingMU.HargaRupiahRound);
                TotalModal += Convert.ToDecimal(r.StockProductMounting_PricingMU.ModalRupiah);
                TotalM += r.StockProductMounting_PricingMU.ModalUSD;
                TotalR += r.StockProductMounting_PricingMU.HargaUSD;
                TotalNetWeight += r.StockProductMounting_CharProduct.NettoWeight;
                TotalGrossWeight = r.StockProductMounting_CharProduct.GrossWeight;
                index++;
            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualMounting.InHand).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N0") + " / "  + TotalHarga.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&status=" + Status.Text;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&productitem=" + productitem.Text;
        href2 += "&productcategory=" + productcategory.Text;
        href2 += "&productlevel=" + productlevel.Text;
        href2 += "&status=" + Status.Text;
        href2 += "&order=" + orderby.Text;
        href2 += "&dari=" + datepicker.Text;
        href2 += "&sampai=" + datepicker2.Text;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            Status.SelectedValue = Request.QueryString["status"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&status=" + Status.Text;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Mounting_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&status=" + Status.Text;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Mounting_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&order=" + orderby.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}