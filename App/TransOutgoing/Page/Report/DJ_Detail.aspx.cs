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
        var d = db.StockOutgoingDJ.SingleOrDefault(p => p.ID == myID);

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
        var rs = (from p in db.StockOutgoingDJ_Product where p.StockOutgoingDJ.ID == myID select p); // Outgoing

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
            decimal GT_Promo = 0m;

            var _locWarehouseDict = db.LocWarehouse.ToDictionary(p => p.ID);

            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);

            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

            foreach (var item in rs)
            {
                string From = ""; string To = "";
                if (item.StockOutgoingDJ.TipeAsal == 0) { var rsf = (_locWarehouseDict.ContainsKey(item.StockOutgoingDJ.IDAsal) ? _locWarehouseDict[item.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockOutgoingDJ.TipeAsal == 1) { var rsf = (_locOutletDict.ContainsKey(item.StockOutgoingDJ.IDAsal) ? _locOutletDict[item.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (item.StockOutgoingDJ.TipeAsal == 2) { var rsf = (_locExhibitionDict.ContainsKey(item.StockOutgoingDJ.IDAsal) ? _locExhibitionDict[item.StockOutgoingDJ.IDAsal] : null); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (item.StockOutgoingDJ.TipeTujuan == 0) { var rst = (_locWarehouseDict.ContainsKey(item.StockOutgoingDJ.IDTujuan) ? _locWarehouseDict[item.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocWarehouseGroup.Nama + ")"; }
                else if (item.StockOutgoingDJ.TipeTujuan == 1) { var rst = (_locOutletDict.ContainsKey(item.StockOutgoingDJ.IDTujuan) ? _locOutletDict[item.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocOutletGroup.Nama + ")"; }
                else if (item.StockOutgoingDJ.TipeTujuan == 2) { var rst = (_locExhibitionDict.ContainsKey(item.StockOutgoingDJ.IDTujuan) ? _locExhibitionDict[item.StockOutgoingDJ.IDTujuan] : null); To = rst.Nama + "<br>(" + rst.LocExhibitionGroup.Nama + ")"; }

                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = index++ + ".";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                GT_Qty += 1;
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

                // Promo
                decimal HargaPromo = 0m;
                var promo = (from p in db.PromoDJ_Detail
                            where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
                            && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == item.StockProductDJ.ID
                            select p).OrderByDescending(p => p.PromoDJ.ApprovalTgl).FirstOrDefault();

                if (promo != null)
                {
                    if (promo.PromoDJ.Tipe == 1)
                    {
                        decimal DiscountHarga = 0m;
                        decimal DiscountPersen = 0m;
                        decimal Discount = 0m;
                        decimal TotalBayar = item.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value;

                        DiscountHarga = promo.PromoDJ.DiscountNominal.Value;
                        DiscountPersen = (TotalBayar * (promo.PromoDJ.DiscountPersen.Value / 100));
                        Discount = (DiscountHarga + DiscountPersen);
                        HargaPromo = TotalBayar - Discount;
                    }
                    if (promo.PromoDJ.Tipe == 3)
                    {
                        HargaPromo = promo.PromoDJ.PromoHarga.Value;
                    }
                }

                td = new TableCell();
                td.Text = Str.Number0(HargaPromo); // Harga Promo
                GT_Promo += Convert.ToDecimal(HargaPromo);
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

            td = new TableCell();
            td.Text = Str.Number0(GT_Promo);
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