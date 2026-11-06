using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

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
        var d = cross.IncomingPG(myID).SingleOrDefault();

        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));


        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.CrossBrand_StockOutgoingPG.TipeAsal == 0)
            NamaAsal = cross.Warehouses(d.CrossBrand_StockOutgoingPG.IDBrandAsal, d.CrossBrand_StockOutgoingPG.IDAsal, d.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingPG.TipeAsal == 1)
            NamaAsal = cross.Outlets(d.CrossBrand_StockOutgoingPG.IDBrandAsal, d.CrossBrand_StockOutgoingPG.IDAsal, d.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingPG.TipeAsal == 2)
            NamaAsal = cross.Exhibitions(d.CrossBrand_StockOutgoingPG.IDBrandAsal, d.CrossBrand_StockOutgoingPG.IDAsal, d.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault().Nama;

        string NamaTujuan = "";
        if (d.CrossBrand_StockOutgoingPG.TipeTujuan == 0)
            NamaTujuan = cross.Warehouses(d.CrossBrand_StockOutgoingPG.IDBrandTujuan, d.CrossBrand_StockOutgoingPG.IDTujuan, d.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingPG.TipeTujuan == 1)
            NamaTujuan = cross.Outlets(d.CrossBrand_StockOutgoingPG.IDBrandTujuan, d.CrossBrand_StockOutgoingPG.IDTujuan, d.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault().Nama;
        else if (d.CrossBrand_StockOutgoingPG.TipeTujuan == 2)
            NamaTujuan = cross.Exhibitions(d.CrossBrand_StockOutgoingPG.IDBrandTujuan, d.CrossBrand_StockOutgoingPG.IDTujuan, d.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault().Nama;

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.CrossBrand_StockOutgoingPG.IDBrandTujuan).Nama;
        nomor.InnerText = d.Nomor;

        // Label Outgoing
        lit_status.Text = "Incomong";
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
        var rs = (from p in cross.IncomingPG(myID) select p).SingleOrDefault(); // Outgoing

        int index = 1;

        if (rs.CrossBrand_StockIncomingPG_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_QtyReceived = 0m;
            decimal GT_QtyBroken = 0m;
            decimal GT_QtyLost = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;

            foreach (var item in rs.CrossBrand_StockIncomingPG_Product)
            {
                string From = ""; string To = "";
                if (rs.CrossBrand_StockOutgoingPG.TipeAsal == 0) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingPG.IDBrandAsal, rs.CrossBrand_StockOutgoingPG.IDAsal, rs.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingPG.TipeAsal == 1) { var rsf = cross.Outlets(rs.CrossBrand_StockOutgoingPG.IDBrandAsal, rs.CrossBrand_StockOutgoingPG.IDAsal, rs.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingPG.TipeAsal == 2) { var rsf = cross.Exhibitions(rs.CrossBrand_StockOutgoingPG.IDBrandAsal, rs.CrossBrand_StockOutgoingPG.IDAsal, rs.CrossBrand_StockOutgoingPG.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                if (rs.CrossBrand_StockOutgoingPG.TipeTujuan == 0) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingPG.IDBrandTujuan, rs.CrossBrand_StockOutgoingPG.IDTujuan, rs.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingPG.TipeTujuan == 1) { var rsf = cross.Outlets(rs.CrossBrand_StockOutgoingPG.IDBrandTujuan, rs.CrossBrand_StockOutgoingPG.IDTujuan, rs.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingPG.TipeTujuan == 2) { var rsf = cross.Exhibitions(rs.CrossBrand_StockOutgoingPG.IDBrandTujuan, rs.CrossBrand_StockOutgoingPG.IDTujuan, rs.CrossBrand_StockOutgoingPG.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(rs.StatusTransIn);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.CrossBrand_StockOutgoingPG.NamaKurir;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1"; // Str.Number0(item.Qty);
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ((item.SubStorageResult.Value != 96 && item.SubStorageResult.Value != 99) ? 1 : 0).ToString(); // Str.Number0(item.Qty);
                GT_QtyReceived += ((item.SubStorageResult.Value != 96 && item.SubStorageResult.Value != 99) ? 1 : 0);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ((item.SubStorageResult.Value == 96) ? 1 : 0).ToString(); // Str.Number0(item.Qty);
                GT_QtyBroken += ((item.SubStorageResult.Value == 96) ? 1 : 0);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ((item.SubStorageResult.Value == 99) ? 1 : 0).ToString(); // Str.Number0(item.Qty);
                GT_QtyLost += ((item.SubStorageResult.Value == 99) ? 1 : 0);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2((item.SubStorageResult.Value != 99) ? item.StockProductPG.NettoWeight : 0);
                GT_Netto += (item.SubStorageResult.Value != 99) ? (decimal)item.StockProductPG.NettoWeight : 0;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2((item.SubStorageResult.Value != 99) ? item.StockProductPG.GrossWeight : 0);
                GT_Gross += (item.SubStorageResult.Value != 99) ? (decimal) item.StockProductPG.GrossWeight : 0;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.Number0((item.SubStorageResult.Value != 99) ? item.StockProductPG.TotalHargaJual.Value : 0);
                GT_Price += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductPG.TotalHargaJual.Value : 0);
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
            td.Text = Str.Number0(GT_QtyReceived);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_QtyBroken);
            td.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number0(GT_QtyLost);
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