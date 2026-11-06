using System;
using System.Linq;
using System.Web.UI.WebControls;

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

        var d = db.StockIncomingDJ.SingleOrDefault(p => p.ID == myID);

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.StockOutgoingDJ.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingDJ.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingDJ.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingDJ.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingDJ.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingDJ.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.StockOutgoingDJ.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingDJ.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingDJ.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingDJ.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingDJ.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.StockOutgoingDJ.CompanyBrand.Nama;
        nomor_kirim.InnerText = d.StockOutgoingDJ.Nomor;
        nomor_terima.InnerText = d.Nomor;
        keterangan_kirim.InnerText = Str.HtmlBreak(d.StockOutgoingDJ.Keterangan);

        // Label Outgoing
        lit_status.Text = "Incoming";
        status.InnerText = DL.IncomingApprovalStatus(d.StatusTransIn);
        tglapprove.InnerText = d.ApprovalTgl != null ? Str.TglJam(d.ApprovalTgl) : "-"; 
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
        var rs = (from p in db.StockIncomingDJ_Product where p.StockIncomingDJ.ID == myID select p); // Outgoing

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            bool cek = false;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDAsal) ? _locWarehouseDict[item.StockIncomingDJ.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDAsal) ? _locOutletDict[item.StockIncomingDJ.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDAsal) ? _locExhibitionDict[item.StockIncomingDJ.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDTujuan) ? _locWarehouseDict[item.StockIncomingDJ.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDTujuan) ? _locOutletDict[item.StockIncomingDJ.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockIncomingDJ.StockOutgoingDJ.IDTujuan) ? _locExhibitionDict[item.StockIncomingDJ.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

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
                td.Text = Str.Tgl(item.StockIncomingDJ.ApprovalTgl);
                tr.Cells.Add(td);

                string Status = "";
                if (item.SubStorageResult == 1)
                    Status = DL.IncomingApprovalStatus(item.StockIncomingDJ.StatusTransIn);
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
                td.Text = item.StockIncomingDJ.StockOutgoingDJ.NamaKurir;
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
                string url = "DJ_Print_Sertifikat.aspx?id=" + myID;

                Response.Write("<script>window.open('" + url + "')</script>");
            }
        }
    }
}