using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Packaging_Export.xls");
            fillTable(sender, e);
        }
    }
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderPackaging
				 where !(p.SalesOrder.Draft == true || p.SalesOrder.StatusVoid == true)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(Request.QueryString["kw"]) || p.Invoice.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);
            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);
            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;


                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.SalesOrder.Tgl);
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.SalesOrder.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);
				
				string norefpk = "";
				
				string pkso = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingSalesOrder,'') [LPSO] FROM SalesOrder WHERE ID = " + r.SalesOrder.ID, Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkso))
				{
					norefpk = pkso;
				}
				string pkdp = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingDownPayment,'') [LPDP] FROM SalesOrder WHERE ID = " + r.SalesOrder.ID,Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkdp))
				{
					norefpk = pkdp;
				}
				string pkrep = Db.SingleString("SELECT TOP 1 ISNULL(LinkPackagingRepair,'') [LPRP] FROM SalesOrder WHERE ID = " + r.SalesOrder.ID, Db.Cnn_DB);
				if(!string.IsNullOrEmpty(pkrep))
				{
					norefpk = pkrep;
				}
				
				td = new TableCell();
				td.Text = norefpk;
				tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDCustomer.HasValue ? r.SalesOrder.CustomerNama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDSales.HasValue ? r.SalesOrder.DataSales.Nama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(r.Qty).ToString();
                tr.Cells.Add(td);

                string NamaLokasi = "";
                string GrupLokasi = "";
                if (r.SalesOrder.TipeLokasi == 0) //WareHouse
                {
                    var lw = (_locWarehouseDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? _locWarehouseDict[r.SalesOrder.IDLokasi.Value] : null);
                    NamaLokasi = lw.Nama ;
                    GrupLokasi = "WareHouse";
                }
                else if (r.SalesOrder.TipeLokasi == 1) //Outlet
                {
                    var lo = (_locOutletDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? _locOutletDict[r.SalesOrder.IDLokasi.Value] : null);
                    NamaLokasi = lo.Nama;
                    GrupLokasi = "Outlet";
                }
                else if (r.SalesOrder.TipeLokasi == 2) //Exhibition
                {
                    var lx = (_locExhibitionDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? _locExhibitionDict[r.SalesOrder.IDLokasi.Value] : null);
                    NamaLokasi = lx.Nama ;
                    GrupLokasi = "Exhibition";
                }

                td = new TableCell();
                td.Text = GrupLokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                tb4.Rows.Add(tr);
            }
        }
    }
}