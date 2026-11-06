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
        var d = db.StockOutgoingPG.SingleOrDefault(p => p.ID == myID);

        string NamaAsal = "";
        if (d.TipeAsal == 0) { lit_type_from.Text = "Warehouse"; NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDAsal, Db.Cnn_DB); }
        else if (d.TipeAsal == 1) { lit_type_from.Text = "Outlet"; NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDAsal, Db.Cnn_DB); }
        else if (d.TipeAsal == 2) { lit_type_from.Text = "Exhibition"; NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDAsal, Db.Cnn_DB); }

        string NamaTujuan = "";
        if (d.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDTujuan, Db.Cnn_DB); }
        else if (d.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDTujuan, Db.Cnn_DB); }
        else if (d.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDTujuan, Db.Cnn_DB); }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.CompanyBrand.Nama;
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
        var rs = (from p in db.StockOutgoingPG_Product where p.StockOutgoingPG.ID == myID select p); // Outgoing

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
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
                    if (item.StockOutgoingPG.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locWarehouseDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locOutletDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockOutgoingPG.IDAsal) ? _locExhibitionDict[item.StockOutgoingPG.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockOutgoingPG.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locWarehouseDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locOutletDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockOutgoingPG.IDTujuan) ? _locExhibitionDict[item.StockOutgoingPG.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                                
                string Consignment = "";
                Consignment = Db.SingleString("select CASE WHEN c.Consignment IS NULL OR c.Consignment = 0 THEN 'Non Consignment' else 'Consignment' end Consignment from StockOutgoingPG a JOIN (SELECT IDForm,IDProduct FROM StockOutgoingPG_Product) b on b.IDForm=a.ID JOIN StockProductPG c on c.ID=b.IDProduct where c.ID=" + item.IDProduct, Db.Cnn_DB);

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
                td.Text = Str.Number2(item.StockProductPG.NettoWeight);
                GT_Netto += Convert.ToDecimal(item.StockProductPG.NettoWeight);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.GrossWeight);
                GT_Gross += Convert.ToDecimal(item.StockProductPG.GrossWeight);
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
            td.ColumnSpan = 4;
            td.HorizontalAlign = HorizontalAlign.Right;
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