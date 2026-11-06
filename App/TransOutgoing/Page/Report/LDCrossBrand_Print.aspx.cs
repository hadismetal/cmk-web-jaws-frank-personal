using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

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
    Modelize cross = new Modelize();
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
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
        var d = cross.OutgoingLD(myID).SingleOrDefault();

        string NamaAsal = d.NamaAsal;
        if (d.TipeAsal == 0) { lit_type_from.Text = "Warehouse"; }
        else if (d.TipeAsal == 1) { lit_type_from.Text = "Outlet"; }
        else if (d.TipeAsal == 2) { lit_type_from.Text = "Exhibition"; }

        string NamaTujuan = d.NamaTujuan;
        if (d.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; }
        else if (d.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; }
        else if (d.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.IDBrandAsal).Nama;
        nomor.InnerText = d.Nomor;
        keterangan.InnerText = Str.HtmlBreak(d.Keterangan);
        kurir.InnerText = d.NamaKurir;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(d.StatusTransOut);
        tglkirim.InnerText = Str.TglJam(d.ApprovalTgl);

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
    protected void FillTable()
    {
        TableRow tr;
        TableCell td;

        tb.Visible = true;

        //SELECT
        var rs = cross.OutgoingLD(myID).SingleOrDefault(); // Outgoing

        // = rs.OrderBy(p => p.StockProductDJ.Nomor);

        int index = 1;

        if (rs.CrossBrand_StockOutgoingLD_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            decimal GT_Promo = 0m;
            bool cek = false;


            foreach (var item in rs.CrossBrand_StockOutgoingLD_Product)
            {
                string From = ""; string To = "";
                if (rs.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (rs.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (rs.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (rs.TipeTujuan == 0) { var rst = cross.Warehouses(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 1) { var rst = cross.Outlets(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 2) { var rst = cross.Exhibitions(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }

                if (cek == false)
                {
                    var crt = from p in db.StockProductLD_Stone1B
                              where p.ID == item.StockProductLD_Stone1B.ID
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
                td.Text = Str.Tgl(rs.ApprovalTgl);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(rs.StatusTransOut);
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockOutgoingLD.StockIncomingLD.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingLD.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingLD.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingLD.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingLD.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductLD_Stone1B.Nomor;
                td.VerticalAlign = VerticalAlign.Top;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.NamaKurir;
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
            td.Text = "";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 4;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "Total";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Qty);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Butir);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(GT_Carat);
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
                string url = "LDCrossBrand_Print_Sertifikat.aspx?id=" + myID;

                Response.Write("<script>window.open('" + url + "')</script>");
            }
        }
    }
}