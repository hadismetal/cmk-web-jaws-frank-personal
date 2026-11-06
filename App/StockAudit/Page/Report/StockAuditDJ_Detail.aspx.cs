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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            fillFilter();
            fillTable();
            
        }

    }

    //Display Table
    protected void fillTable()
    {
        title.InnerText = "Audit ID : " + id.ToString();
        int Group = Convert.ToInt32(group.SelectedValue);

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

            int index = 1;

            TableRow tr = new TableRow();
            TableCell td;

            int err1 = 0; // Fisik Ada, data tidak ada
            int err2 = 0; // Fisik Tidak Ada, data ada
            int err3 = 0; // Beda Gambar
            int err4 = 0; // Beda Bandrol
            int err5 = 0; // gambar tidak ada

            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=5\"" + ">" + Convert.ToDecimal(sadtl.Count()).ToString("N0") + "</a>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=6\"" + ">" + Convert.ToDecimal(d.TotalItemAudit).ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err1 = (from c in sadtl
                    where !(from o in sai
                            select o.IDProduct)
                           .Contains(c.IDProduct)
                    select c).Count();

            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=1\"" + ">" + err1.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            var rs = from p in sai
                     where p.AuditID == id
                     && !(from z in sadtl
                          where z.AuditID == id
                          select z.IDProduct).Contains(p.IDProduct)
                     select p;

            err2 = rs.Count();

            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=2\"" + ">" + err2.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err5 = (from p in sadtl
                    where p.ImgNone == true
                    select p).Count();


            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=7\"" + ">" + err5.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            err3 = (from p in sadtl
                    where p.ImgCheck == true
                    select p).Count();


            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=3\"" + ">" + err3.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            err4 = (from p in sadtl
                    where p.BandrolCheck == true
                    select p).Count();


            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=4\"" + ">" + err4.ToString("N0") + "</a>";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            var src5 = (from p in sadtl
                    select p);

            var ctr5 = src5.GroupBy(p => p.IDProduct);

            ctr5 = ctr5.Where(p => p.Count() > 1);

            td = new TableCell();
            td.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=8\"" + ">" + ctr5.Count().ToString("N0") + "</a>";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);


            //Detail Item Group By
            if (Group == 1)
            {
                var rsg = sai.GroupBy(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem);

                foreach (var r in rsg)
                {

                    int index2 = 1;

                    TableRow tr2 = new TableRow();
                    TableCell td2;

                    int err1_2 = 0; // Fisik Ada, data tidak ada
                    int err2_2 = 0; // Fisik Tidak Ada, data ada
                    int err3_2 = 0; // Beda Gambar
                    int err4_2 = 0; // Beda Bandrol
                    int err5_2 = 0; // Gambar tidak ada

                    td2 = new TableCell();
                    td2.Text = r.First().StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr2.Cells.Add(td2);


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=5&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sadtl.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=6&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err1_2 = (from c in sadtl
                              where !(from o in sai
                                    select o.IDProduct)
                                   .Contains(c.IDProduct)
                                   && c.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key
                            select c).Count();

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=1&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err1_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    var rst = sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key);

                    foreach (var t in rst)
                    {
                        var cek = from p in sadtl
                                  where p.IDProduct == t.IDProduct && p.AuditID == t.AuditID
                                  && p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key
                                  select p;
                        if (cek.Count() == 0) err2_2++;
                    }

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=2&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err2_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err5_2 = (from p in sadtl
                              where p.ImgNone == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=7&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err5_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err3_2 = (from p in sadtl
                              where p.ImgCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=3&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err3_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err4_2 = (from p in sadtl
                              where p.BandrolCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=4&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err4_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    tb2.Rows.Add(tr2);
                }
            }
            else if (Group == 2)
            {
                var rsg = sai.GroupBy(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory);

                foreach (var r in rsg)
                {

                    int index2 = 1;

                    TableRow tr2 = new TableRow();
                    TableCell td2;

                    int err1_2 = 0; // Fisik Ada, data tidak ada
                    int err2_2 = 0; // Fisik Tidak Ada, data ada
                    int err3_2 = 0; // Beda Gambar
                    int err4_2 = 0; // Beda Bandrol
                    int err5_2 = 0; // Gambar tidak ada

                    td2 = new TableCell();
                    td2.Text = r.First().StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                    tr2.Cells.Add(td2);


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=5&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sadtl.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=6&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err1_2 = (from c in sadtl
                              where !(from o in sai
                                      select o.IDProduct)
                                     .Contains(c.IDProduct)
                                     && c.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key
                              select c).Count();

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=1&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err1_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    var rst = sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key);

                    foreach (var t in rst)
                    {
                        var cek = from p in sadtl
                                  where p.IDProduct == t.IDProduct && p.AuditID == t.AuditID
                                  && p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key
                                  select p;
                        if (cek.Count() == 0) err2_2++;
                    }

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=2&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err2_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err5_2 = (from p in sadtl
                              where p.ImgNone == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=7&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err5_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err3_2 = (from p in sadtl
                              where p.ImgCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=3&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err3_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err4_2 = (from p in sadtl
                              where p.BandrolCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=4&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err4_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    tb2.Rows.Add(tr2);
                }
            }
            else if (Group == 3)
            {
                var rsg = sai.GroupBy(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel);

                foreach (var r in rsg)
                {

                    int index2 = 1;

                    TableRow tr2 = new TableRow();
                    TableCell td2;

                    int err1_2 = 0; // Fisik Ada, data tidak ada
                    int err2_2 = 0; // Fisik Tidak Ada, data ada
                    int err3_2 = 0; // Beda Gambar
                    int err4_2 = 0; // Beda Bandrol
                    int err5_2 = 0; // Gambar tidak ada

                    td2 = new TableCell();
                    td2.Text = r.First().StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.Nama;
                    tr2.Cells.Add(td2);


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=5&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sadtl.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=6&group=" + Group + "&pid=" + r.Key + "\"" + ">" + Convert.ToDecimal(sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key).Count()).ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err1_2 = (from c in sadtl
                              where !(from o in sai
                                      select o.IDProduct)
                                     .Contains(c.IDProduct)
                                     && c.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key
                              select c).Count();

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=1&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err1_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    var rst = sai.Where(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key);

                    foreach (var t in rst)
                    {
                        var cek = from p in sadtl
                                  where p.IDProduct == t.IDProduct && p.AuditID == t.AuditID
                                  && p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key
                                  select p;
                        if (cek.Count() == 0) err2_2++;
                    }

                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=2&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err2_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err5_2 = (from p in sadtl
                              where p.ImgNone == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=7&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err5_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err3_2 = (from p in sadtl
                              where p.ImgCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=3&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err3_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    err4_2 = (from p in sadtl
                              where p.BandrolCheck == true && p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.Key
                              select p).Count();


                    td2 = new TableCell();
                    td2.Text = "<a href=\"StockAuditDJ_DetailList.aspx?id=" + id.ToString() + "&tipe=4&group=" + Group + "&pid=" + r.Key + "\"" + ">" + err4_2.ToString("N0") + "</a>";
                    tr2.Cells.Add(td2);

                    tb2.Rows.Add(tr2);
                }
            }
        }
        else
            Response.Redirect("StockAuditDJ.aspx");

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            group.SelectedValue = Request.QueryString["group"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&id=" + id;
        href += "&group=" + group.Text;

        Response.Redirect(href);
    }
}