using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

    protected string kode { get { return Request.QueryString["kode"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            int Tipe = 1;
            if (tipelokasi.SelectedValue != "")
                Tipe = Convert.ToInt32(tipelokasi.SelectedValue);

            DLC.LocationList(lokasi, 1);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderSouvenir
                 where p.Souvenir.Disable == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if (Tipe == 1 || Tipe == 2)
        {

            rs = rs.Where(p => (p.SalesOrder.IDLokasi == Lokasi || (Lokasi < 0 && p.SalesOrder.IDLokasi > 0)) && p.SalesOrder.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (!string.IsNullOrEmpty(kode)) rs = rs.Where(p => p.Souvenir.Kode.Equals(kode));
        
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            int TotalQty = 0;
            string so = String.Empty;
            var _locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);
            var _locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;


                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                if (!so.Equals(r.SalesOrder.Nomor))
                {
                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(r.SalesOrder.Tgl);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.Nomor;
                    tr.Cells.Add(td);
                    
                    td = new TableCell();
                    string Location = r.SalesOrder.TipeLokasi == 1 ? (_locOutletDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? _locOutletDict[r.SalesOrder.IDLokasi.Value] : null).Nama : (_locExhibitionDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? _locExhibitionDict[r.SalesOrder.IDLokasi.Value] : null).Nama;
                    td.Text = Location;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.IDCustomer.HasValue ? r.SalesOrder.CustomerNama : "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.IDSales.HasValue ? r.SalesOrder.DataSales.Nama : "";
                    tr.Cells.Add(td);
					
					td = new TableCell();
                    td.Text = Db.SingleString("SELECT TOP 1 ISNULL(B.Nama,'') [Nama] FROM SalesOrder A LEFT JOIN Promo B ON A.IDPromo = B.ID WHERE A.ID = " + r.SalesOrder.ID,Db.Cnn_DB);
                    tr.Cells.Add(td);

                    so = r.SalesOrder.Nomor;
                }
                else
                {
                    td = new TableCell();
                    td.ColumnSpan = 7;
                    tr.Cells.Add(td);
                }


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Souvenir.Kode;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(r.Qty).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                decimal Harga = Db.SingleDecimal("SELECT ISNULL(TotalModalRupiah, 0) FROM SalesOrderSouvenir WHERE ID = " + r.ID, Db.Cnn_DB);
                td.Text = "Rp. " + Str.Number2(Harga);
                tr.Cells.Add(td);

                TotalHarga += Harga;
                TotalQty += Convert.ToInt32(r.Qty);

                tb.Rows.Add(tr);
            }

            TableRow row = new TableRow();
            TableCell cell;

            cell = new TableCell();
            cell.ColumnSpan = 10;
            cell.Text = "<h4>Total</h4>";
			cell.Style.Add("text-align", "left");
			
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "<h4>" + TotalQty.ToString() + "</h4>";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "<h4>Rp. " + Str.Number2(TotalHarga) + "</h4>";
            row.Cells.Add(cell);

            tb.Rows.Add(row);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Souvenir_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "Souvenir_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&kode=" + kode;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}