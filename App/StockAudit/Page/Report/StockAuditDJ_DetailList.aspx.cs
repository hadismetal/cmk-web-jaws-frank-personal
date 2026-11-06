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

    protected int id { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int Group { get { return App.QueryInt(this, "group"); } }
    protected int pid { get { return App.QueryInt(this, "pid"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            fillTable();

        }

    }

    //Display Table
    protected void fillTable()
    {
        title.InnerText = "Audit ID : " + id.ToString();



        var d = db.StockAuditDJ.SingleOrDefault(p => p.ID == id && p.StatusFinish.Value == true);

        if (d != null)
        {
            var sai = d.StockAuditItemDJ; //StockAuditItemDJ
            var sadtl = d.StockAuditDJDetail; //StockAuditDJDetail

            string NamaLokasi = "";
            string TipeLokasi = DL.LocationType(d.TipeLokasi);
            if (Convert.ToInt32(d.IDLokasi) == 0)
                NamaLokasi = "ALL";
            else
            {
                if (d.TipeLokasi == 0)
                {
                    var w = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = w.Nama;
                }
                if (d.TipeLokasi == 1)
                {
                    var o = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = o.Nama;
                }
                if (d.TipeLokasi == 2)
                {
                    var exh = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    NamaLokasi = exh.Nama;
                }
            }

            title2.InnerText = TipeLokasi + " " + NamaLokasi;
            title3.InnerText = Str.TglJam(d.OperatorTgl) + " - " + Str.TglJam(d.FinishJam);
            if (tipe == 1)
                title4.InnerText = "Fisik Ada,Data Tidak Ada";
            else if (tipe == 2)
                title4.InnerText = "Fisik Tidak Ada,Data Ada";
            else if (tipe == 3)
                title4.InnerText = "Gambar Tidak Sesuai";
            else if (tipe == 4)
                title4.InnerText = "Bandrol Tidak Sesuai";
            else if (tipe == 8)
                title4.InnerText = "Multiple Input";

            int index = 1;

            if (tipe == 1)
            {
                var err1 = from c in d.StockAuditDJDetail
                           where !(from o in d.StockAuditItemDJ
                                   select o.IDProduct)
                                  .Contains(c.IDProduct)
                                  
                           select c;

                if (Group == 1)
                    err1 = err1.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid);
                else if (Group == 2)
                    err1 = err1.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid);
                else if (Group == 3)
                    err1 = err1.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid);

                foreach (var r in err1)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 2)
            {
                var rs = from p in d.StockAuditItemDJ
                         join q in db.StockProductDJ on p.IDProduct equals q.ID
                         where p.AuditID == id
                         && !(from z in d.StockAuditDJDetail
                              where z.AuditID == id
                              select z.IDProduct).Contains(p.IDProduct)
                         select new 
                         {
                             xCharProduct = p.StockProductDJ.StockProductDJ_CharProduct,

                             xStockProductID = q.ID,
                             xStockProductNomor = q.Nomor,
                             xStockProductProtoNomor = q.ProtoNomor,
                             xStockProductKeterangan = q.Keterangan,
                             xStockProductOperator = q.Operator,
                             xStockProductOperatorTgl = q.OperatorTgl,

                             xStockProductItem = q.StockProductDJ_CharProduct.ProductItem,
                             xStockProductItemNama = q.StockProductDJ_CharProduct.CharProductItem.Nama,
                             xStockProductCategory = q.StockProductDJ_CharProduct.ProductCategory,
                             xStockProductCategoryNama = q.StockProductDJ_CharProduct.CharProductCategory.Nama,
                             xStockProductLevel = q.StockProductDJ_CharProduct.ProductLevel,
                             xStockProductLevelNama = q.StockProductDJ_CharProduct.CharProductLevel.Nama,
                             
                             xPricingMUHargaRupiahRound = q.StockProductDJ_PricingMU.HargaRupiahRound.Value
                         };

                if (Group == 1)
                    rs = rs.Where(p => p.xStockProductItem == pid);
                else if (Group == 2)
                    rs = rs.Where(p => p.xStockProductCategory == pid);
                else if (Group == 3)
                    rs = rs.Where(p => p.xStockProductLevel == pid);

                if (rs != null)
                {
                    foreach (var r in rs)
                    {
                        //var cek = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.AuditID == r.AuditID);

                        //var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
                        var x = r;
                        if (x != null)
                        {
                            TableRow tr = new TableRow();
                            TableCell td;

                            td = new TableCell();
                            td.Text = index++.ToString();
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = "-";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = x.xStockProductNomor;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = x.xStockProductProtoNomor;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = x.xStockProductItemNama;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = x.xStockProductKeterangan;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = TD_HyperLink(x.xStockProductID);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = x.xStockProductOperator;
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.FullDateJam(x.xStockProductOperatorTgl);
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(x.xPricingMUHargaRupiahRound);
                            tr.Cells.Add(td);

                            tb.Rows.Add(tr);
                        }

                    }
                }
            }
            else if (tipe == 3)
            {
                var err3 = from c in d.StockAuditDJDetail
                           where c.ImgCheck == true
                           && ((Group == 1) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid :
                                    ((Group == 2) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid :
                                    ((Group == 3) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid : 1 == 1)))
                           select c;

                foreach (var r in err3)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 4)
            {
                var err4 = from c in d.StockAuditDJDetail
                           where c.BandrolCheck == true
                           && ((Group == 1) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid :
                                    ((Group == 2) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid :
                                    ((Group == 3) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid : 1 == 1)))
                           select c;

                foreach (var r in err4)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 5)
            {
                var rs = from p in db.StockAuditDJDetail
                         where p.AuditID == id
                         select p;

                if (Group == 1)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid);
                else if (Group == 2)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid);
                else if (Group == 3)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid);

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 6)
            {
                var rs = from p in db.StockAuditDJDetail
                         where p.AuditID == id
                         select p;

                if (Group == 1)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid);
                else if (Group == 2)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid);
                else if (Group == 3)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid);

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 7)
            {
                var rs = from c in d.StockAuditDJDetail
                         where c.ImgNone == true
                         && ((Group == 1) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid :
                                  ((Group == 2) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid :
                                  ((Group == 3) ? c.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid : 1 == 1)))
                         select c;

                if (Group == 1)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == pid);
                else if (Group == 2)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == pid);
                else if (Group == 3)
                    rs = rs.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == pid);

                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.NoMeja;
                    tr.Cells.Add(td);

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
                    td.Text = r.Keterangan;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            else if (tipe == 8)
            {
                var rs = from c in d.StockAuditDJDetail
                         where c.AuditID == id
                         select c;

                var src = rs.GroupBy(p => p.IDProduct);
                var ctr = src.Where(p => p.Count() > 1);

                foreach (var r in ctr)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index++.ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    var u = db.StockProductDJ.SingleOrDefault(p => p.ID == r.Key);
                    var cek = rs.Where(p => p.IDProduct == r.Key);

                    td = new TableCell();
                    td.Text = u.Nomor + " (" + cek.Count().ToString() +")";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = u.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = u.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_HyperLink(u);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = u.StockProductDJ_PricingMU.HargaRupiahRound.Value.ToString("N0");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
        else
            Response.Redirect("StockAuditDJ.aspx");

    }

    protected string TD_HyperLink(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }

    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Detail.aspx?id=" + ID.ToString() + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "StockAuditDJ_DetailExcel.aspx?id=" + id + "&tipe=" + tipe;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "StockAuditDJ_DetailPrint.aspx?id=" + id + "&tipe=" + tipe;

        Response.Redirect(href);
    }
}