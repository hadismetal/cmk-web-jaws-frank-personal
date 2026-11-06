using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class LD_Print : System.Web.UI.Page
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
        //Form-Default
        FillHeader();
        FillTable();
	}
    //Fill
    protected void FillHeader()
    {
        var d = db.StockIncomingLD.SingleOrDefault(p => p.ID == myID);

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.StockOutgoingLD.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingLD.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingLD.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingLD.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingLD.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingLD.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.StockOutgoingLD.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingLD.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingLD.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingLD.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingLD.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingLD.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.StockOutgoingLD.CompanyBrand.Nama;
        nomor.InnerText = d.Nomor;
        keterangan.InnerText = Str.HtmlBreak(d.Keterangan);

        // Label Incoming
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
        var rs = (from p in db.StockIncomingLD_Product where p.StockIncomingLD.ID == myID select p); // Incoming

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

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingLD.StockOutgoingLD.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if(cek == false)
                {
                    var crt = from p in db.StockProductLD_Stone1B
                              where p.IDForm == item.StockProductLD_Stone1B.ID
                              select p;

                    if (crt.Count() > 0)
                        cek = true;
                }
                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockIncomingLD.ApprovalTgl);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(item.StockIncomingLD.StatusTransIn);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockIncomingLD.StockIncomingLD.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingLD.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingLD.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingLD.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingLD.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductLD_Stone1B.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingLD.StockOutgoingLD.NamaKurir;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1"; // Str.Number0(item.Qty);
                GT_Qty += 1;
                
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductLD_Stone1B.TotalButir);
                GT_Butir += Convert.ToDecimal(item.StockProductLD_Stone1B.TotalButir);
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(item.StockProductLD_Stone1B.TotalCarat);
                GT_Carat += item.StockProductLD_Stone1B.TotalCarat;
                td.HorizontalAlign = HorizontalAlign.Left;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

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
            td.Text = "";
            tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = "";
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = "";
            //tr.Cells.Add(td);

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
            td.Text = "";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number0(GT_Qty) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number0(GT_Butir) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<b>" + Str.Number3(GT_Carat) + "</b>";
            td.HorizontalAlign = HorizontalAlign.Left;
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
                string Keyword = Request.QueryString["key"];

                //Redirect
                string url = "LD_Print_Sertifikat.aspx?id=" + myID;

                Response.Write("<script>window.open('" + url + "')</script>");
            }
        }
    }
}