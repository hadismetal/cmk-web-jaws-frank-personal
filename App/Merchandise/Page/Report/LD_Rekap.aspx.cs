using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_LD_Rekap : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            DLC.LocationList(cabang, tipe);

            //DLC
            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, false);
            DLC.Parcel103(parcel03);
            DLC.Parcel104(parcel04);
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&order=" + order.SelectedIndex;
        href += "&orderby=" + orderby.SelectedIndex;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
            parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
            parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
            parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
            order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
            orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
            cabang.SelectedValue = Request.QueryString["cabang"];
            substorage.SelectedValue = Request.QueryString["substorage"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        string o = order.SelectedValue + " " + orderby.SelectedValue;

        //SELECT
        /*var rs = from p in db.Stone1B.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(kw.Text) ||
                     p.Keterangan.Contains(kw.Text) ||
                     p.KodeParcel.Contains(kw.Text)
                     )
                 select p;*/

        var rs = (from p in db.StockProductLD_Stone1B
                  where p.ReturnProduct == false
                 && p.StockActualLD_Stone1B.IDLokasi > 0
                 && !(from s in db.SalesOrderLD
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                      select s.IDProduct).Contains(p.ID)
                 && !(from s in db.SalesOrderSouvenir
                      where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                      select s.IDProductSouvenir).Contains(p.ID)
                  select new
                  {
                      ID = p.Stone1B.ID,
                      Nama = p.Stone1B.Nama,
                      KodeParcel = p.Stone1B.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductLD.Nomor,
                      Parcel01 = p.Stone1B.Parcel01,
                      Parcel02 = p.Stone1B.Parcel02,
                      Parcel03 = p.Stone1B.Parcel03,
                      Parcel04 = p.Stone1B.Parcel04,
                      Parcel05 = p.Stone1B.Parcel05,
                      Parcel06 = p.Stone1B.Parcel06,
                      Parcel07 = p.Stone1B.Parcel07,
                      Nama01 = p.Stone1B.Parcel101.Nama,
                      Nama02 = p.Stone1B.Parcel102.Nama,
                      Nama03 = p.Stone1B.Parcel103.Nama,
                      Nama04 = p.Stone1B.Parcel104.Nama,
                      Nama05 = p.Stone1B.Parcel105.Nama,
                      Nama06 = p.Stone1B.Parcel106.Nama,
                      Nama07 = p.Stone1B.Parcel107.Nama,
                      StockProductLD = p.StockProductLD,
                      StockActualLD_Stone1B = p.StockActualLD_Stone1B
                  }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
                     z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

        //rs = rs.Where(p => p.StockProductLD.StatusLebur == false && p.StockProductLD.ReturnProduct == false && p.StockProductLD.StatusPenjualan == 0);

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);
        }

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == parcel07.SelectedIndex);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == Cabang && p.StockActualLD_Stone1B.TipeLokasi == tipe);
        }
        else
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe);
        }

        var stone = rs.GroupBy(p => p.KodeParcel);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            //RESULT
            int index = 1;
            foreach (var r in stone)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama01;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama03;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama04;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama05;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama06;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 7;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalButir.ToString("N0");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalCarat.ToString("N3");
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "LD_RekapExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&order=" + order.Text;
        href += "&orderby=" + orderby.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "LD_RekapPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&order=" + order.Text;
        href += "&orderby=" + orderby.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
}