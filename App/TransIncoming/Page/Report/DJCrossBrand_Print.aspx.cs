using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

public partial class DJ_Print : System.Web.UI.Page
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
    Modelize cross = new Modelize();
    protected int myID { get { return App.QueryInt(this); } }

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
        //Form-Default
        FillHeader();
        FillTable();
	}
    //Fill
    protected void FillHeader()
    {
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));

        var d = cross.IncomingDJ(myID).SingleOrDefault();

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.CrossBrand_StockOutgoingDJ.TipeAsal == 0)
            NamaAsal = cross.Warehouses(d.CrossBrand_StockOutgoingDJ.IDBrandAsal, d.CrossBrand_StockOutgoingDJ.IDAsal, d.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingDJ.TipeAsal == 1)
            NamaAsal = cross.Outlets(d.CrossBrand_StockOutgoingDJ.IDBrandAsal, d.CrossBrand_StockOutgoingDJ.IDAsal, d.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingDJ.TipeAsal == 2)
            NamaAsal = cross.Exhibitions(d.CrossBrand_StockOutgoingDJ.IDBrandAsal, d.CrossBrand_StockOutgoingDJ.IDAsal, d.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault().Nama;

        string NamaTujuan = "";
        if (d.CrossBrand_StockOutgoingDJ.TipeTujuan == 0)
            NamaTujuan = cross.Warehouses(d.CrossBrand_StockOutgoingDJ.IDBrandTujuan, d.CrossBrand_StockOutgoingDJ.IDTujuan, d.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingDJ.TipeTujuan == 1)
            NamaTujuan = cross.Outlets(d.CrossBrand_StockOutgoingDJ.IDBrandTujuan, d.CrossBrand_StockOutgoingDJ.IDTujuan, d.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingDJ.TipeTujuan == 2)
            NamaTujuan = cross.Exhibitions(d.CrossBrand_StockOutgoingDJ.IDBrandTujuan, d.CrossBrand_StockOutgoingDJ.IDTujuan, d.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault().Nama;

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brandFrom.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.CrossBrand_StockOutgoingDJ.IDBrandAsal).Nama;
        brandTo.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.CrossBrand_StockOutgoingDJ.IDBrandTujuan).Nama;
        nomor_kirim.InnerText = d.CrossBrand_StockOutgoingDJ.Nomor;
        nomor_terima.InnerText = d.Nomor;
        keterangan_kirim.InnerText = Str.HtmlBreak(d.CrossBrand_StockOutgoingDJ.Keterangan);

        // Label Outgoing
        lit_status.Text = "Incoming";
        status.InnerText = DL.IncomingApprovalStatus(d.StatusTransIn);
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    protected void FillTable()
    {
        TableRow tr;
        TableCell td;

        tb.Visible = true;

        //SELECT
        var rs = (from p in cross.IncomingDJ(myID) select p).SingleOrDefault(); // Outgoing

        int index = 1;

        if (rs.CrossBrand_StockIncomingDJ_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            bool cek = false;

            foreach (var item in rs.CrossBrand_StockIncomingDJ_Product)
            {
                string From = ""; string To = "";
                if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 0) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 1) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 2) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if(cek == false)
                {
                    var crt = from p in db.StockProductDJ_Stone1B
                              where p.IDForm == item.StockProductDJ.ID
                              select p;

                    if (crt.Count() > 0)
                        cek = true;
                }
                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(rs.ApprovalTgl);
                tr.Cells.Add(td);

                string Status = "";
                if (item.SubStorageResult == 1)
                    Status = DL.IncomingApprovalStatus(rs.StatusTransIn);
                else if (item.SubStorageResult == 96)
                    Status = "Broken";
                else if (item.SubStorageResult == 99)
                    Status = "Lost";

                td = new TableCell();
                td.Text = Status;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.CrossBrand_StockOutgoingDJ.NamaKurir;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1"; // Str.Number0(item.Qty);
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                GT_Netto += item.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductDJ.StockProductDJ_CharProduct.GrossWeight);
                GT_Gross += item.StockProductDJ.StockProductDJ_CharProduct.GrossWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductDJ.StockProductDJ_CharProduct.StoneQty);
                GT_Butir += Convert.ToDecimal(item.StockProductDJ.StockProductDJ_CharProduct.StoneQty);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(item.StockProductDJ.StockProductDJ_CharProduct.StoneCarat);
                GT_Carat += item.StockProductDJ.StockProductDJ_CharProduct.StoneCarat;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                GT_Price += Convert.ToDecimal(item.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

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

            //td = new TableCell();
            //td.Text = "";
            //tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Qty);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(GT_Netto);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(GT_Gross);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Butir);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(GT_Carat);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Price);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            
            if (cek == true)
            {

                string PeriodeDari = Request.QueryString["pf"];
                string PeriodeSampai = Request.QueryString["pt"];
                int IDBrand = Convert.ToInt32(Request.QueryString["idb"]); // brand
                int TipeAsal = Convert.ToInt32(Request.QueryString["ta"]); ; // type from
                int TipeTujuan = Convert.ToInt32(Request.QueryString["tt"]); ; // type to
                int IDAsal = Convert.ToInt32(Request.QueryString["ida"]); // id from
                int IDTujuan = Convert.ToInt32(Request.QueryString["idt"]); // id to
                int Status = Convert.ToInt32(Request.QueryString["status"]);
                int Category = Convert.ToInt32(Request.QueryString["category"]);
                int Item = Convert.ToInt32(Request.QueryString["item"]);
                string Keyword = Request.QueryString["key"];

                //Redirect
                string url = "DJCrossBrand_Print_Sertifikat.aspx?id=" + myID;

                Response.Write("<script>window.open('" + url + "')</script>");
            }
        }
    }
}