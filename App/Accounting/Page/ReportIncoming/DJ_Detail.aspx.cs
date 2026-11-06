using System;
using System.Linq;
using System.Web.UI.WebControls;

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
        nomor.InnerText = d.Nomor;

        // Label Incoming
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
        var rs = (from p in db.StockIncomingDJ_Product where p.StockIncomingDJ.ID == myID select p); // Incoming

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
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

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingDJ.StockOutgoingDJ.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockIncomingDJ.StockOutgoingDJ.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockIncomingDJ.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(item.StockIncomingDJ.StatusTransIn);
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
        Response.Redirect("DJ.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("DJ_Print.aspx?id=" + myID);
    }
}