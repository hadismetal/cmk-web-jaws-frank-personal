using System;
using System.Linq;
using System.Web.UI.WebControls;
using CrossBrand;

public partial class DJ_Print : System.Web.UI.Page
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
        var d = cross.OutgoingDJ(myID).SingleOrDefault();

        string NamaAsal = d.NamaAsal;
        if (d.TipeAsal == 0) { lit_type_from.Text = "Warehouse";  }
        else if (d.TipeAsal == 1) { lit_type_from.Text = "Outlet";}
        else if (d.TipeAsal == 2) { lit_type_from.Text = "Exhibition";}

        string NamaTujuan = d.NamaTujuan;
        if (d.TipeTujuan == 0) { lit_type_to.Text = "Warehouse"; }
        else if (d.TipeTujuan == 1) { lit_type_to.Text = "Outlet"; }
        else if (d.TipeTujuan == 2) { lit_type_to.Text = "Exhibition"; }

        from.InnerHtml = NamaAsal;
        to.InnerHtml = NamaTujuan;
        brand.InnerText = db.CompanyBrand.SingleOrDefault(p => p.ID == d.IDBrandAsal).Nama;
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
        var rs = cross.OutgoingDJ(myID).SingleOrDefault(); // Outgoing

        // = rs.OrderBy(p => p.StockProductDJ.Nomor);

        int index = 1;

        if (rs.CrossBrand_StockOutgoingDJ_Product.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            decimal GT_Qty = 0m;
            decimal GT_Netto = 0m;
            decimal GT_Gross = 0m;
            decimal GT_Butir = 0m;
            decimal GT_Carat = 0m;
            decimal GT_Price = 0m;
            decimal GT_Promo = 0m;
            bool cek = false;


            foreach (var item in rs.CrossBrand_StockOutgoingDJ_Product)
            {
                string From = ""; string To = "";
                if (rs.TipeAsal == 0) { var rsf = db.LocWarehouse.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocWarehouseGroup.Nama + ")"; }
                else if (rs.TipeAsal == 1) { var rsf = db.LocOutlet.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocOutletGroup.Nama + ")"; }
                else if (rs.TipeAsal == 2) { var rsf = db.LocExhibition.SingleOrDefault(p => p.ID == rs.IDAsal); From = rsf.Nama + "<br>(" + rsf.LocExhibitionGroup.Nama + ")"; }
                if (rs.TipeTujuan == 0) { var rst = cross.Warehouses(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 1) { var rst = cross.Outlets(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
                else if (rs.TipeTujuan == 2) { var rst = cross.Exhibitions(rs.IDBrandTujuan, rs.IDTujuan, rs.TipeTujuan).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }

                if (cek == false)
                {
                    var crt = (from o in db.StockProductDJ_Stone1A
                               where o.IDForm == item.StockProductDJ.ID
                               select o.IDStone).Union(from p in db.StockProductDJ_Stone1B
                                       where p.IDForm == item.StockProductDJ.ID
                                       select p.IDStone).Union(from q in db.StockProductDJ_Stone2
                                                               where q.IDForm == item.StockProductDJ.ID
                                                               select q.IDStone).Union(from r in db.StockProductDJ_Stone3
                                                                                       where r.IDForm == item.StockProductDJ.ID
                                                                                       select r.IDStone);
					int TotalStoneCertified = Db.SingleInteger("EXEC sp_check_diamond_certified_or_no "+item.StockProductDJ.ID.ToString()+" ",Db.Cnn_DB); 
                    if (TotalStoneCertified > 0)
                        cek = true;
                }
				//cek = true;
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
            td.Text = Str.Number0(GT_Promo); // Harga Promo
            td.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);


            if (cek == true)
            {

                string PeriodeDari = Request.QueryString["pf"];
                string PeriodeSampai = Request.QueryString["pt"];
                int IDBrand = Convert.ToInt32(Request.QueryString["idb"]); // brand
                int TipeAsal = Convert.ToInt32(Request.QueryString["ta"]); ; // type from
                int TipeTujuan = Convert.ToInt32(Request.QueryString["tt"]); ; // type to
                int IDAsal = Convert.ToInt32(Request.QueryString["ida"]); // id from
                int IDTujuan = Convert.ToInt32(Request.QueryString["idt"]); // id to
                int Status = Convert.ToInt32(Request.QueryString["status"]);
                int Category = Convert.ToInt32(Request.QueryString["category"]);
                int Item = Convert.ToInt32(Request.QueryString["item"]);
                string Keyword = Request.QueryString["key"];

                //Redirect
                string url = "DJCrossBrand_Print_Sertifikat.aspx?id=" + myID;

                Response.Write("<script>window.open('" + url + "')</script>");
            }
        }
    }
}