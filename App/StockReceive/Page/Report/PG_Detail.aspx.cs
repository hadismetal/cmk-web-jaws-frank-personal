using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;

public partial class Page_DJ : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            DLC.CharProductItem(productitem);
            DLC.CharGoldModelAllFilter(model);
            DLC.CharGoldLevel(productlevel);
            DLC.DDLSupplier(supplier);
            DLC.LocationList(cabang, tipe);

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
        string w = "1=1";
        
        var rs = from p in db.StockReceivePG
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
        }

        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor == "" +kw.Text+ "");
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductItem == ProductItem);
        int Model = Convert.ToInt32(model.SelectedValue);
        if (Model > 0) rs = rs.Where(p => p.Model == Model);
        int Supplier = Convert.ToInt32(supplier.SelectedValue);
        if (Supplier > 0) rs = rs.Where(p => p.IDSupplier == Supplier);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.CharGoldLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.CharGoldModel.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.CharProductItem.Nama);

        if (rs != null)
        {
            decimal TotalQty = 0m;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalWeight = 0m;
            decimal TotalOngkos = 0m;
            decimal TotalHarga = 0m;
            decimal TotalHargaAll = 0m;
            decimal TotalHPPM = 0m;
            foreach (var r in rs)
            {
                string consignment = Db.SingleString("SELECT CONVERT(varchar, ISNULL(Consignment, 0), 0) FROM StockReceivePG WHERE ID = " + r.ID, Db.Cnn_DB);
                if(ddl_consignment.SelectedValue != "2" && ddl_consignment.SelectedValue != consignment) continue;
                switch (consignment)
                {
                    case "1" :
                    consignment = "Consignment";
                    break;
                    case "0" :
                    consignment = "Non-Consignment";
                    break;
                }
                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KodeBarang;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharProductItem.Nama;
                tr.Cells.Add(td);

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
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(1);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = consignment;
                tr.Cells.Add(td);

                string Lokasi = "";

                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDWarehouse, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.KadarLogam).ToString("N2") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.KadarTukaranBeli).ToString("N2") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CharFrameColor.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DataSupplier.Nama; 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GoldRate).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.MountingM).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHarga24 / r.TotalRate).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHargaJual).ToString("N0");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = r.SupplierNama;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierSJ;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty++;
                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalWeight += Convert.ToDecimal(r.GrossWeight);
                TotalOngkos += Convert.ToDecimal(r.MountingM);
                TotalHPPM += Convert.ToDecimal(r.TotalHarga24 / r.TotalRate);
                TotalHarga += Convert.ToDecimal(r.Harga);
                TotalHargaAll += Convert.ToDecimal(r.TotalHargaJual);
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

                td = new TableCell();
                td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
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

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalOngkos).ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHPPM).ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Convert.ToDecimal(TotalHargaAll).ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            model.SelectedValue = Request.QueryString["model"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            ddl_consignment.SelectedValue = Request.QueryString["consignment"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&model=" + model.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&consignment=" + ddl_consignment.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_DetailExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&model=" + model.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&supplier=" + supplier.SelectedValue;
        href += "&consignment=" + ddl_consignment.SelectedValue;

        Response.Redirect(href);
    }
}