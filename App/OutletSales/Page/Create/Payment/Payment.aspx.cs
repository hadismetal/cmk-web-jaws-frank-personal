using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;


public partial class Page_Summary : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table New
    protected void fillTable(object sender, EventArgs e)
    {
        string kw_text = kw.Text;
        DateTime start_text = Convert.ToDateTime(dari.Text);
        DateTime end_text = Convert.ToDateTime(sampai.Text);

        TimeSpan ts = end_text.Date - start_text.Date;
        if (ts.TotalDays < 31)
        {
            lit_error.Text = string.Empty;
            string location = getLocationUser();
            string query = "sp_report_sales_order_new";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text },
                new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text },
                new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_text },
                new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = location },
                new SqlParameter("@STATUS", SqlDbType.Int) { Value = 2 },
                new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 },
                new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 },
                new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 1 }
            };

            DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, parameters);
            DataTable data = ds.Tables[0];
            int datacount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            if (datacount > 0)
            {
                string ti_nomor = "";
                string ti_sisa_resell = "";

                int prev = 0;
                int index = 1;
                foreach (DataRow r in data.Rows)
                {
                    ti_nomor = "";
                    ti_sisa_resell = "";

                    decimal nilaiResell = 0m;
                    decimal nilaiHarusDibayar = 0m;

                    string rTradeIn = r["NOMORTI"].ToString();
                    decimal rTotalBayar = Convert.ToDecimal(r["TOTALBAYAR"]);
                    decimal rTotalResell = Convert.ToDecimal(r["TOTALRESELL"]);
                    string rNomor = r["NOMORSO"].ToString();
                    string rPLU = r["PLU"].ToString();
                    bool rStatusVoid = Convert.ToBoolean(r["STATUSVOID"]);
                    bool rStatusPembayaran = Convert.ToBoolean(r["STATUSPEMBAYARAN"]);
                    bool rDraft = Convert.ToBoolean(r["DRAFT"]);
                    DateTime rTgl = Convert.ToDateTime(r["TGLSO"]);
                    string rCustomerNama = r["CUSTNAMA"].ToString();
                    string rCustomerCode = r["CUSTCODE"].ToString();
                    string rSalesNama = r["SALESNAMA"].ToString();
                    string rKeterangan = r["KETERANGAN"].ToString();
                    string rKeteranganVoid = r["KETERANGANVOID"].ToString();

                    int ID = Convert.ToInt32(r["ID"]);
                    decimal Harga = Convert.ToDecimal(r["TOTALBAYAR"]);
                    decimal Paid = Convert.ToDecimal(r["TERBAYAR"]);
                    decimal Unpaid = Convert.ToDecimal(r["SISABAYAR"]);
                    int Qty = Convert.ToInt32(r["QTY"]);
                    string rStatus = r["STATUS"].ToString();

                    if (!string.IsNullOrEmpty(rTradeIn))
                    {
                        ti_nomor = "<br /><span style='color:blue;'>" + rTradeIn + "</span>";
                        nilaiResell = rTotalResell;
                        nilaiHarusDibayar = rTotalBayar;
                        ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
                    }

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLinkNew(ID);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(rTgl);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rNomor + ti_nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rPLU;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Harga.ToString("N0") + ti_sisa_resell;
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Paid.ToString("N0");
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Unpaid.ToString("N0");
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rCustomerNama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rCustomerCode;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    TotalBayar += Harga;
                    TotalUnpaid += Unpaid;
                    TotalPaid += Paid;
                    TotalQty += Qty;
                }
            }
            else
            {
                App.TableNoResult(tb);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 5;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
        else
        {
            lit_error.Text = "<label style='font-size:larger;font-family:Lucida Console;color:red'>Jika periode lebih dari 31 hari, klik tombol Export</label>";
        }
    }

    //Display Table
    protected void fillTableOld(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrder
                 where /*p.StatusPembayaran == false &&*/ p.Draft == false && p.StatusVoid == false

                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {

            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(keyword)
                        || (from q in db.SalesOrderDJ
                            where q.StockProductDJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderGJ
                            where q.StockProductGJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderPG
                            where q.StockProductPG.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderLD
                            where q.StockProductLD_Stone1B.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || p.CustomerNama.Contains(kw.Text)
                 select p;
        }

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusVoid == true);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs != null)
        {
            int prev = 0;
            int index = 1;

            string ti_nomor = "";
            string ti_keterangan = "";

            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header
                ti_nomor = "";
                ti_keterangan = "";

                if (r.TradeIn.HasValue)
                {
                    var ti = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == r.ID);
                    if (ti != null)
                    {
                        ti_nomor = "<br /><span style='color:blue;'>" + ti.Nomor + "</span>";
                        ti_keterangan = ti.Keterangan;
                    }
                }

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor + ti_nomor;
                tr.Cells.Add(td);

                decimal Qty = r.SalesOrderDJ.Count() + r.SalesOrderPG.Count() + r.SalesOrderGJ.Count() + r.SalesOrderLD.Count();

                td = new TableCell();
                td.Text = Qty.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalRupiah = r.SalesOrderDJ.Sum(p => p.TotalBayar.Value) + r.SalesOrderPG.Sum(p => p.TotalBayar.Value) + r.SalesOrderGJ.Sum(p => p.TotalBayar.Value) + +r.SalesOrderLD.Sum(p => p.TotalBayar.Value);

                td = new TableCell();
                td.Text = TotalRupiah.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalBayar = (r.Paid.HasValue) ? r.Paid.Value : 0m;

                td = new TableCell();
                td.Text = TotalBayar.ToString("N0");
                tr.Cells.Add(td);

                decimal TotalUnpaid = r.Unpaid.HasValue ? r.Unpaid.Value : 0m;

                td = new TableCell();
                td.Text = TotalUnpaid.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DataCustomer.NoCustomer;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(SalesOrder r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Payment_ItemList.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }

    protected string TD_HyperLinkNew(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Payment_ItemList.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
}