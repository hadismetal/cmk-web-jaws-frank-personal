using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class GJ_Print : System.Web.UI.Page
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
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["ta"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tt"]));

        var d = db.StockIncomingGJ.SingleOrDefault(p => p.ID == myID);

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.StockOutgoingGJ.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingGJ.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingGJ.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingGJ.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingGJ.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingGJ.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.StockOutgoingGJ.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingGJ.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingGJ.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingGJ.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingGJ.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingGJ.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.StockOutgoingGJ.CompanyBrand.Nama;
        nomor_kirim.InnerText = d.StockOutgoingGJ .Nomor;
        nomor_terima.InnerText = d.Nomor;
        keterangan_kirim.InnerText = Str.HtmlBreak(d.StockOutgoingGJ.Keterangan);

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
        var rs = (from p in db.StockIncomingGJ_Product where p.StockIncomingGJ.ID == myID select p); // Outgoing

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
                if (item.StockIncomingGJ.StockOutgoingGJ.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDAsal) ? _locWarehouseDict[item.StockIncomingGJ.StockOutgoingGJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingGJ.StockOutgoingGJ.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDAsal) ? _locOutletDict[item.StockIncomingGJ.StockOutgoingGJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingGJ.StockOutgoingGJ.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDAsal) ? _locExhibitionDict[item.StockIncomingGJ.StockOutgoingGJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDTujuan) ? _locWarehouseDict[item.StockIncomingGJ.StockOutgoingGJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDTujuan) ? _locOutletDict[item.StockIncomingGJ.StockOutgoingGJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingGJ.StockOutgoingGJ.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockIncomingGJ.StockOutgoingGJ.IDTujuan) ? _locExhibitionDict[item.StockIncomingGJ.StockOutgoingGJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockIncomingGJ.ApprovalTgl);
                tr.Cells.Add(td);

                string Status = "";
                if (item.SubStorageResult == 1)
                    Status = DL.IncomingApprovalStatus(item.StockIncomingGJ.StatusTransIn);
                else if (item.SubStorageResult == 96)
                    Status = "Broken";
                else if (item.SubStorageResult == 99)
                    Status = "Lost";

                td = new TableCell();
                td.Text = Status;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductGJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingGJ.StockOutgoingGJ.NamaKurir;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductGJ.StockProductGJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1"; // Str.Number0(item.Qty);
                GT_Qty += 1;
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductGJ.StockProductGJ_CharProduct.NettoWeight);
                GT_Netto += item.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductGJ.StockProductGJ_CharProduct.GrossWeight);
                GT_Gross += item.StockProductGJ.StockProductGJ_CharProduct.GrossWeight;
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(item.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
                GT_Price += Convert.ToDecimal(item.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value);
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
            td.Text = Str.Number0(GT_Price);
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

        }
    }
}