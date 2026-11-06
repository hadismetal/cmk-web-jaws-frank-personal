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

        var d = db.StockOutgoingPG.SingleOrDefault(p => p.ID == myID);

        string From = ""; string To = "";
        string NamaAsal = "";
        if (d.TipeAsal == 0)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDAsal, Db.Cnn_DB);
        else if (d.TipeAsal == 1)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDAsal, Db.Cnn_DB);
        else if (d.TipeAsal == 2)
            NamaAsal = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDAsal, Db.Cnn_DB);

        string NamaTujuan = "";
        if (d.TipeTujuan == 0)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + d.IDTujuan, Db.Cnn_DB);
        else if (d.TipeTujuan == 1)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.IDTujuan, Db.Cnn_DB);
        else if (d.TipeTujuan == 2)
            NamaTujuan = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.IDTujuan, Db.Cnn_DB);

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = d.CompanyBrand.Nama;
        nomor.InnerText = d.Nomor;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = DL.OutgoingApprovalStatus(d.StatusTransOut);
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

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                try
                {
                    if (item.StockOutgoingPG.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }
                try
                {
                    if (item.StockOutgoingPG.TipeTujuan == 0) { var rst = db.LocWarehouse.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 1) { var rst = db.LocOutlet.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                    else if (item.StockOutgoingPG.TipeTujuan == 2) { var rst = db.LocExhibition.SingleOrDefault(p => p.ID == item.StockOutgoingPG.IDTujuan); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }
                }
                catch (Exception ex) { }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = item.StockOutgoingPG.StockIncomingPG.Nomor 
                //    + "<br>" + Str.Tgl(item.StockIncomingPG.ApprovalTgl)
                //    + "<br>(" + DL.IncomingApprovalStatus(item.StockIncomingPG.StatusTransIn) + ")"
                //    ;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingPG.TipeAsal) + "<br>" + From;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = DL.LocationType(item.StockOutgoingPG.TipeTujuan) + "<br>" + To;
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockOutgoingPG.NamaKurir;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductPG.CharGoldModel.Nama;
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
                td.Text = Str.Number2(item.StockProductPG.NettoWeight);
                GT_Netto += Convert.ToDecimal(item.StockProductPG.NettoWeight);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number2(item.StockProductPG.GrossWeight);
                GT_Gross += Convert.ToDecimal(item.StockProductPG.GrossWeight);
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
                td.Text = Str.Number0(item.StockProductPG.TotalHargaJual.Value);
                GT_Price += Convert.ToDecimal(item.StockProductPG.TotalHargaJual.Value);
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