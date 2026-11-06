using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Set();

        }
    }

    protected void Set()
    {
        int Qty = 0;
        decimal netweight = 0m;
        decimal frameprice = 0m;
        decimal totalbutir = 0m;
        decimal totalcarat = 0m;
        decimal totalharga = 0m;
        decimal no = 0m;
        decimal ro = 0m;
        decimal finishing = 0m;
        decimal stonesetting = 0m;
        decimal hargausd = 0m;
        decimal hargarp = 0m;

        var d = db.StockReturnDJ.SingleOrDefault(p => p.ID == ReturnID);
        if (d == null)
            Response.Redirect("DJ.aspx");
        else
        {
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.NoDokumen + "</a>";
            tgl.InnerHtml = Str.Tgl(d.ApprovalTgl);
            ket.Text = d.Keterangan;
            var rs = from p in db.StockReturnDJ_Product
                     where p.IDForm == d.ID
                     select p;
            rs = rs.OrderByDescending(p => p.ID);

            if (rs != null)
            {
                int index = 1;
                foreach (var r in rs)
                {
                    /*TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.ModalRupiah).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.StoneQty).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3");
                    tr.Cells.Add(td);

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.IDProduct.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(hf);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);*/

                    decimal AllM = 0m;
                    decimal Rate = 0m;

                    Rate = r.StockProductDJ.StockProductDJ_PricingBiaya.TotalRate;

                    var y = r.StockProductDJ.StockProductDJ_CostingProduct;
                    var z = r.StockProductDJ.StockProductDJ_Costing;

                    AllM = (y.Total + z.TotalBiaya);

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.StockProductDJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Tgl(r.StockProductDJ.Tgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.Tgl(r.StockProductDJ.ApprovalTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                    tr.Cells.Add(td);

                    netweight += r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct.ToString("N2");
                    tr.Cells.Add(td);

                    frameprice += r.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct;

                    var stone = (from p in r.StockProductDJ.StockProductDJ_Stone1A
                                 select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1A.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                from p in r.StockProductDJ.StockProductDJ_Stone1B
                                where p.StatusTerpasang == 1
                                select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1B.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                from p in r.StockProductDJ.StockProductDJ_Stone2
                                select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone2.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                from p in r.StockProductDJ.StockProductDJ_Stone3
                                select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone3.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                from p in r.StockProductDJ.StockProductDJ_Stone4
                                select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone4.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                from p in r.StockProductDJ.StockProductDJ_Stone5
                                select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone5.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir });
                    if (stone.Count() > 0)
                    {
                        Qty++;
                        int counter = 1;
                        foreach (var rstone in stone)
                        {
                            if (counter > 1)
                            {
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
                            }

                            td = new TableCell();
                            td.Text = rstone.Nama;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = rstone.TotalButir.ToString("N0");
                            tr.Cells.Add(td);

                            totalbutir += rstone.TotalButir;

                            td = new TableCell();
                            td.Text = rstone.TotalCarat.ToString("N3");
                            tr.Cells.Add(td);

                            totalcarat += rstone.TotalCarat;

                            td = new TableCell();
                            td.Text = Convert.ToDecimal(rstone.TotalHargaM).ToString("N2");
                            tr.Cells.Add(td);

                            totalharga += Convert.ToDecimal(rstone.TotalHargaM);

                            if (stone.Count() > 1)
                                tb.Rows.Add(tr);

                            counter++;
                        }
                    }
                    else
                    {
                        td = new TableCell();
                        td.Text = "-";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "-";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "-";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "-";
                        tr.Cells.Add(td);
                    }

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Costing.MountingNO).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_Costing.MountingRO.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Finishing.Sum(x => x.BiayaModal)).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Costing.Setting).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.Text = r.StockProductDJ_PricingMU.ModalUSD.ToString("N2");
                    td.Text = AllM.ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2");
                    td.Text = (AllM * Rate).ToString("N2");
                    tr.Cells.Add(td);

                    no += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Costing.MountingNO);
                    ro += r.StockProductDJ.StockProductDJ_Costing.MountingRO;
                    finishing += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Finishing.Sum(x => x.BiayaModal));
                    stonesetting += Convert.ToDecimal(r.StockProductDJ.StockProductDJ_Costing.Setting);
                    hargausd += AllM;
                    hargarp += (AllM * Rate);

                    tb.Rows.Add(tr);

                    index++;
                }
                TableRow tr1 = new TableRow();
                TableCell td1;

                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);
                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + Qty.ToString() + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + netweight.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + frameprice.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + totalbutir.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + totalcarat.ToString("N4") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + totalharga.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + no.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + ro.ToString("N0") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + finishing.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + stonesetting.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + hargausd.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                td1 = new TableCell();
                td1.Text = "<b>" + hargarp.ToString("N2") + "</b>";
                tr1.Cells.Add(td1);

                tb.Rows.Add(tr1);
            }
        }
    }

}