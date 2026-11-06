using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;

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
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //Generate
            fillHeader(sender, e);
            fillTable(sender, e);
        }

    }


    protected void back_Click(object sender, EventArgs e)
    {
        /// PG.aspx?refreshPage=1&kw=&dari=01-11-2015&sampai=09-11-2015&status=0&tipelokasi=-1&lokasi=-1

        string href = "";
        href = "PG.aspx?refreshPage=1";
        href += "&kw=" + Request.QueryString["kw"];
        href += "&dari=" + Request.QueryString["dari"];
        href += "&sampai=" + Request.QueryString["sampai"];
        href += "&status=" + Request.QueryString["status"];
        href += "&tipelokasi=" + Request.QueryString["tipelokasi"];
        href += "&lokasi=" + Request.QueryString["lokasi"];

        Response.Redirect(href);
    }

    
    //Display Header
    protected void fillHeader(object sender, EventArgs e)
    {
        var head = db.PromoPG.SingleOrDefault(p => p.ID == myID);

        DateTime myDate1 = Convert.ToDateTime(head.TglExpire);
        DateTime myDate2 = DateTime.Now;
        TimeSpan timeSpanResult;
        timeSpanResult = myDate1 - myDate2;
        double days = Math.Ceiling(timeSpanResult.TotalDays);
        string result = "";
        if (days > 1) { result = days.ToString("#,##0") + ((days > 1 ? " days" : " days")) + " remaining"; }
        else if (days == 0) { result = "Today is the last day!"; }
        else if (days < 0) { result = "<span style='color:red;'>" + Math.Abs(days).ToString("#,##0") + ((Math.Abs(days) > 1 ? " days" : " days")) + " expired</span>"; }

        lit_header1.Text += "<table style='border:none;'>"
                    + "<tr><td>Nama Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Nama + "</td></tr>"
                    + "<tr><td>Keterangan</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.Keterangan + "</td></tr>"
                    + "<tr><td>Approval Nama</td>" + "<td>&nbsp;:&nbsp;</td><td>" + head.ApprovalNama + "</td></tr>"
                    + "<tr><td>Approval Tgl</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.ApprovalTgl).ToString("dd-MM-yyyy") + "</td></tr>"
                    + "<tr><td>Tgl Start</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglStart).ToString("dd-MM-yyyy") + "</td></tr>"
                    + "<tr><td>Tgl Expire</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDateTime(head.TglExpire).ToString("dd-MM-yyyy") + " (" + result + ")</td></tr>"
                    + "</table>";
        lit_header2.Text += "<table style='border:none;'>"
                    + "<tr><td>Tipe Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + (head.Tipe == 3 ? "Promo 1 Harga" : (head.Tipe == 2 ? "Promo Poin" : "Discount")) + "</td></tr>"
                    + "<tr><td>Harga Promo</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PromoHarga).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Nominal</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountNominal).ToString("#,##0") + "</td></tr>"
                    + "<tr><td>Discount Persen</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.DiscountPersen).ToString("#,##0.00") + "</td></tr>"
                    + "<tr><td>Poin Reward</td>" + "<td>&nbsp;:&nbsp;</td><td>" + Convert.ToDecimal(head.PoinReward).ToString("#,##0") + "</td></tr>"
                    + "<tr><td valign='top'>Modifikasi</td>" + "<td valign='top'>&nbsp;:&nbsp;</td><td>" 
                    + (head.StatusASP == 1 ? "- No Exchange / No Resell (ASP)<br />" : "")
                    + (head.StatusDiscMembership == true ? "- No Discount Membership<br />" : "")
                    + (head.StatusDiscNormal == true ? "- No Discount Normal<br />" : "") 
                    + "</td></tr>"
                    + "</table>";
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var so = db.PromoPG.SingleOrDefault(p => p.ID == myID);

        if (so != null)
        {
            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var rs = from p in db.PromoPG_Detail
                     join q in db.PromoPG on p.IDForm equals q.ID
                     where p.IDForm == so.ID
                     orderby q.DraftDate ascending
                     select p;
            
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.Nomor;
                tr.Cells.Add(td);

                string Category= "";
                Category = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.IDCategory, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Category;
                tr.Cells.Add(td);

                string Level = "";
                Level = Db.SingleString("SELECT Nama FROM CharProductLevel WHERE ID=" + r.IDLevel, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Level;
                tr.Cells.Add(td);

                string Item = "";
                Item = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.IDItem, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                string NamaLokasi = "";
                if (r.StockProductPG.StockActualPG.TipeLokasi == 0) { NamaLokasi = rsWarehouse[r.StockProductPG.StockActualPG.IDLokasi]; }
                else if (r.StockProductPG.StockActualPG.TipeLokasi == 1) { NamaLokasi = rsOutlet[r.StockProductPG.StockActualPG.IDLokasi]; }
                else if (r.StockProductPG.StockActualPG.TipeLokasi == 2) { NamaLokasi = rsExhibition[r.StockProductPG.StockActualPG.IDLokasi]; }

                td = new TableCell();
                td.Text = NamaLokasi;
                tr.Cells.Add(td);

                string StatusBarang = "";
                StatusBarang = DL.SubStorage(r.StockProductPG.StockActualPG.Substorage);

                td = new TableCell();
                td.Text = StatusBarang;
                tr.Cells.Add(td);
               
                tb.Rows.Add(tr);

                index++;
            }
        }
    }
}