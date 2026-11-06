using CrossBrand;
using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class LD_Detail : System.Web.UI.Page
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
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {


            //Form-Default
            FillHeader();
            FillTable();

        }
    }
    //Fill
    protected void FillHeader()
    {
        var d = cross.IncomingLD(myID).SingleOrDefault();

        string NamaAsal = "";
        if (d.CrossBrand_StockOutgoingLD.TipeAsal == 0) { lit_type_from.Text = "Warehouse"; NamaAsal = cross.Warehouses(d.CrossBrand_StockOutgoingLD.IDBrandAsal, d.CrossBrand_StockOutgoingLD.IDAsal, d.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault().Nama; }
        else if (d.CrossBrand_StockOutgoingLD.TipeAsal == 1) { lit_type_from.Text = "Outlet"; NamaAsal = cross.Outlets(d.CrossBrand_StockOutgoingLD.IDBrandAsal, d.CrossBrand_StockOutgoingLD.IDAsal, d.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault().Nama; }
        else if (d.CrossBrand_StockOutgoingLD.TipeAsal == 2) { lit_type_from.Text = "Exhibition"; NamaAsal = cross.Exhibitions(d.CrossBrand_StockOutgoingLD.IDBrandAsal, d.CrossBrand_StockOutgoingLD.IDAsal, d.CrossBrand_StockOutgoingLD.TipeAsal).SingleOrDefault().Nama; }

        string NamaTujuan = "";
        if (d.CrossBrand_StockOutgoingLD.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; NamaTujuan = cross.Warehouses(d.CrossBrand_StockOutgoingLD.IDBrandTujuan, d.CrossBrand_StockOutgoingLD.IDTujuan, d.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.CrossBrand_StockOutgoingLD.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; NamaTujuan = cross.Outlets(d.CrossBrand_StockOutgoingLD.IDBrandTujuan, d.CrossBrand_StockOutgoingLD.IDTujuan, d.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.CrossBrand_StockOutgoingLD.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; NamaTujuan = cross.Exhibitions(d.CrossBrand_StockOutgoingLD.IDBrandTujuan, d.CrossBrand_StockOutgoingLD.IDTujuan, d.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault().Nama; }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brandFrom.InnerText = db.CompanyBrand.FirstOrDefault(p => p.ID == d.CrossBrand_StockOutgoingLD.IDBrandAsal).Nama;
        brandTo.InnerText = db.CompanyBrand.FirstOrDefault(p => p.ID == d.CrossBrand_StockOutgoingLD.IDBrandTujuan).Nama;
        nomor.InnerText = d.Nomor;
        keterangan.InnerText = Str.HtmlBreak(d.Keterangan);
        kurir.InnerText = d.CrossBrand_StockOutgoingLD.NamaKurir;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(d.CrossBrand_StockOutgoingLD.StatusTransOut);
        tglkirim.InnerText = Str.TglJam(d.ApprovalTgl);
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
        var rs = (from p in cross.IncomingLD(myID) select p).FirstOrDefault(); // Outgoing

        int index = 1;

        if (rs.CrossBrand_StockIncomingLD_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            decimal GT_Promo = 0m;

            foreach (var item in rs.CrossBrand_StockIncomingLD_Product)
            {
                string From = ""; string To = "";
                if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (rs.CrossBrand_StockOutgoingLD.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.CrossBrand_StockOutgoingLD.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 0) { var rst = cross.Warehouses(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 1) { var rst = cross.Outlets(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingLD.TipeTujuan == 2) { var rst = cross.Exhibitions(rs.CrossBrand_StockOutgoingLD.IDBrandTujuan, rs.CrossBrand_StockOutgoingLD.IDTujuan, rs.CrossBrand_StockOutgoingLD.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = index++ + ".";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(rs.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(rs.StatusTransIn);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductLD_Stone1B.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.CrossBrand_StockOutgoingLD.NamaKurir;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductLD_Stone1B.TotalButir);
                GT_Butir += Convert.ToDecimal(item.StockProductLD_Stone1B.TotalButir);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(item.StockProductLD_Stone1B.TotalCarat);
                GT_Carat += item.StockProductLD_Stone1B.TotalCarat;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "Total";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 5;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Qty);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Butir);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(GT_Carat);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }


    // back
    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("LD_CrossBrand.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("LDCrossBrand_Print.aspx?id=" + myID);
    }
}