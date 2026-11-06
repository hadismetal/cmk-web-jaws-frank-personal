using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

public partial class DJ_Detail : System.Web.UI.Page
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
        var d = cross.IncomingDJ(myID).SingleOrDefault();

        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));


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
        brand.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.CrossBrand_StockOutgoingDJ.IDBrandTujuan).Nama;
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
        var rs = (from p in cross.IncomingDJ(myID) select p).SingleOrDefault(); // Outgoing

        int index = 1;

        if (rs.CrossBrand_StockIncomingDJ_Product.Count() == 0) { App.TableNoResult(tb); }
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

            foreach (var item in rs.CrossBrand_StockIncomingDJ_Product)
            {
                string From = ""; string To = "";
                if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 0) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 1) { var rsf = cross.Outlets(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeAsal == 2) { var rsf = cross.Exhibitions(rs.CrossBrand_StockOutgoingDJ.IDBrandAsal, rs.CrossBrand_StockOutgoingDJ.IDAsal, rs.CrossBrand_StockOutgoingDJ.TipeAsal).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 0) { var rsf = cross.Warehouses(rs.CrossBrand_StockOutgoingDJ.IDBrandTujuan, rs.CrossBrand_StockOutgoingDJ.IDTujuan, rs.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 1) { var rsf = cross.Outlets(rs.CrossBrand_StockOutgoingDJ.IDBrandTujuan, rs.CrossBrand_StockOutgoingDJ.IDTujuan, rs.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
                else if (rs.CrossBrand_StockOutgoingDJ.TipeTujuan == 2) { var rsf = cross.Exhibitions(rs.CrossBrand_StockOutgoingDJ.IDBrandTujuan, rs.CrossBrand_StockOutgoingDJ.IDTujuan, rs.CrossBrand_StockOutgoingDJ.TipeTujuan).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }

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
                td.Text = Str.Number2((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0);
                GT_Netto += (item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.NettoWeight : 0;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0);
                GT_Gross += (item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.GrossWeight : 0;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.StoneQty : 0);
                GT_Butir += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.StoneQty : 0);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0);
                GT_Carat += (item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_CharProduct.StoneCarat : 0;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0);
                GT_Price += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value : 0);
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
        }
    }


    // back
    protected void back_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("DJ_CrossBrand.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("DJCrossBrand_Print.aspx?id=" + myID);
    }
}