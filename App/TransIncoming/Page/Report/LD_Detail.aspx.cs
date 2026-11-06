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
        var rs = (from p in db.StockIncomingLD_Product where p.StockIncomingLD.ID == myID select p); // Outgoing

        
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

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDAsal) ? _locWarehouseDict[item.StockIncomingLD.StockOutgoingLD.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDAsal) ? _locOutletDict[item.StockIncomingLD.StockOutgoingLD.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDAsal) ? _locExhibitionDict[item.StockIncomingLD.StockOutgoingLD.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDTujuan) ? _locWarehouseDict[item.StockIncomingLD.StockOutgoingLD.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDTujuan) ? _locOutletDict[item.StockIncomingLD.StockOutgoingLD.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockIncomingLD.StockOutgoingLD.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockIncomingLD.StockOutgoingLD.IDTujuan) ? _locExhibitionDict[item.StockIncomingLD.StockOutgoingLD.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.StockIncomingLD.ApprovalTgl);
                tr.Cells.Add(td);

                string Status = "";
                if (item.SubStorageResult == 1)
                    Status = DL.IncomingApprovalStatus(item.StockIncomingLD.StatusTransIn);
                else if (item.SubStorageResult == 96)
                    Status = "Broken";
                else if (item.SubStorageResult == 99)
                    Status = "Lost";

                td = new TableCell();
                td.Text = Status;
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
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockIncomingLD.StockOutgoingLD.NamaKurir;
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
                td.Text = ((item.SubStorageResult.Value != 96) ? 1 : 0).ToString(); // Str.Number0(item.Qty);
                GT_QtyBroken += ((item.SubStorageResult.Value != 96) ? 1 : 0);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ((item.SubStorageResult.Value != 99) ? 1 : 0).ToString(); // Str.Number0(item.Qty);
                GT_QtyLost += ((item.SubStorageResult.Value != 99) ? 1 : 0);
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0((item.SubStorageResult.Value != 99) ? item.StockProductLD_Stone1B.TotalButir : 0);
                GT_Butir += Convert.ToDecimal((item.SubStorageResult.Value != 99) ? item.StockProductLD_Stone1B.TotalButir : 0);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3((item.SubStorageResult.Value != 99) ? item.StockProductLD_Stone1B.TotalCarat : 0);
                GT_Carat += (item.SubStorageResult.Value != 99) ? item.StockProductLD_Stone1B.TotalCarat : 0;
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
        Response.Redirect("LD.aspx");
    }

    //Print
    protected void print_Click(object sender, EventArgs e)
    {

        //Redirect
        Response.Redirect("LD_Print.aspx?id=" + myID
            );
    }
}