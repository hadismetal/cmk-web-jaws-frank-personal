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

        var d = db.StockIncomingPG.SingleOrDefault(p => p.ID == myID);

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.StockOutgoingPG.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingPG.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.StockOutgoingPG.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);
        else if (d.StockOutgoingPG.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockOutgoingPG.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.StockOutgoingPG.CompanyBrand.Nama;
        nomor.InnerText = d.Nomor;

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
        var rs = (from p in db.StockIncomingPG_Product where p.StockIncomingPG.ID == myID select p); // Outgoing

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
            //decimal GT_Butir = 0m;
            //decimal GT_Carat = 0m;
            decimal GT_Price = 0m;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                try
                {
                    if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locWarehouseDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locOutletDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDAsal) ? _locExhibitionDict[item.StockIncomingPG.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locWarehouseDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locOutletDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockIncomingPG.StockOutgoingPG.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockIncomingPG.StockOutgoingPG.IDTujuan) ? _locExhibitionDict[item.StockIncomingPG.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }

                string Consignment = "";
                Consignment = Db.SingleString("select CASE WHEN c.Consignment IS NULL OR c.Consignment = 0 THEN 'Non Consignment' else 'Consignment' end Consignment from StockOutgoingPG a JOIN (SELECT IDForm,IDProduct FROM StockOutgoingPG_Product) b on b.IDForm=a.ID JOIN StockProductPG c on c.ID=b.IDProduct where c.ID=" + item.IDProduct, Db.Cnn_DB);

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockIncomingPG.StockIncomingPG.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingPG.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingPG.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingPG.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockIncomingPG.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingPG.StockOutgoingPG.NamaKurir;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)Consignment;
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
                GT_Netto += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductPG.NettoWeight : 0);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2((item.SubStorageResult.Value != 99) ? item.StockProductPG.GrossWeight : 0);
                GT_Gross += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductPG.GrossWeight : 0);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number0(item.StockProductPG.StockProductPG_CharProduct.StoneQty);
                //GT_Butir += item.StockProductPG.StockProductPG_CharProduct.StoneQty;
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number3(item.StockProductPG.StockProductPG_CharProduct.StoneCarat);
                //GT_Carat += item.StockProductPG.StockProductPG_CharProduct.StoneCarat;
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0((item.SubStorageResult.Value != 99) ? item.StockProductPG.TotalHargaJual : 0);
                GT_Price += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductPG.TotalHargaJual : 0);
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

            //td = new TableCell();
            //td.Text = Str.Number0(GT_Butir);
            //td.HorizontalAlign = HorizontalAlign.Right;
            //tr.Cells.Add(td);

            //td = new TableCell();
            //td.Text = Str.Number3(GT_Carat);
            //td.HorizontalAlign = HorizontalAlign.Right;
            //tr.Cells.Add(td);

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
        Response.Redirect("PG.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {
        //Redirect
        Response.Redirect("PG_Print.aspx?id=" + myID);
    }
}