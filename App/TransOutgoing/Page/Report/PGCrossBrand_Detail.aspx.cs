using CrossBrand;
using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class PG_Detail : System.Web.UI.Page
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
        var d = cross.OutgoingPG(myID).SingleOrDefault();

        string NamaAsal = "";
        if (d.TipeAsal == 0) { lit_type_from.Text = "Warehouse"; NamaAsal = cross.Warehouses(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }
        else if (d.TipeAsal == 1) { lit_type_from.Text = "Outlet"; NamaAsal = cross.Outlets(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }
        else if (d.TipeAsal == 2) { lit_type_from.Text = "Exhibition"; NamaAsal = cross.Exhibitions(d.IDBrandAsal, d.IDAsal, d.TipeAsal).SingleOrDefault().Nama; }

        string NamaTujuan = "";
        if (d.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; NamaTujuan = cross.Warehouses(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; NamaTujuan = cross.Outlets(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }
        else if (d.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; NamaTujuan = cross.Exhibitions(d.IDBrandTujuan, d.IDTujuan, d.TipeTujuan).SingleOrDefault().Nama; }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brandFrom.InnerText = db.CompanyBrand.FirstOrDefault(p => p.ID == d.IDBrandAsal).Nama;
        brandTo.InnerText = db.CompanyBrand.FirstOrDefault(p => p.ID == d.IDBrandTujuan).Nama;
        nomor.InnerText = d.Nomor;
        keterangan.InnerText = Str.HtmlBreak(d.Keterangan);
        kurir.InnerText = d.NamaKurir;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(d.StatusTransOut);
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
        var rs = (from p in cross.OutgoingPG(myID) select p).FirstOrDefault(); // Outgoing

        int index = 1;

        if (rs.CrossBrand_StockOutgoingPG_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            decimal GT_Promo = 0m;

            foreach (var item in rs.CrossBrand_StockOutgoingPG_Product)
            {
                string From = ""; string To = "";
                if (rs.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (rs.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (rs.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (rs.TipeTujuan == 0) { var rst = cross.Warehouses(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 1) { var rst = cross.Outlets(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 2) { var rst = cross.Exhibitions(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = index++ + ".";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.NettoWeight);
                GT_Netto += (decimal) item.StockProductPG.NettoWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.GrossWeight);
                GT_Gross += (decimal) item.StockProductPG.GrossWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductPG.TotalHargaJual);
                GT_Price += Convert.ToDecimal(item.StockProductPG.TotalHargaJual);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            // Grand Total
            tr = new TableRow();

            td = new TableCell();
            td.Text = "Total";
            td.HorizontalAlign = HorizontalAlign.Right;
            td.ColumnSpan = 4;
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
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_Price);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }


    // back
    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("PG_CrossBrand.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("PGCrossBrand_Print.aspx?id=" + myID);
    }
}