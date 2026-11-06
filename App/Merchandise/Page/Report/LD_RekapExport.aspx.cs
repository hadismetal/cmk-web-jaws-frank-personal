using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_LD_RekapExport : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

        }
        Response.ContentType = "application/vnd.ms-excel";

        Response.AppendHeader("content-disposition", "attachment; filename=LD_Rekap_ExportExcel.xls");
        fillTable(sender, e);
    }

    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];
        string kw = Request.QueryString["kw"];

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
                  }).OrderBy(o).Where(z => z.Nama.Contains(kw) ||
                     z.KodeParcel.Contains(kw) || z.Nomor.Contains(kw));

        //rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);
        }

        int parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        int parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        int parcel05 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int parcel06 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int parcel07 = Convert.ToInt32(Request.QueryString["parcel07"]);

        if (parcel01 > 0) rs = rs.Where(p => p.Parcel01 == parcel01);
        if (parcel02 > 0) rs = rs.Where(p => p.Parcel02 == parcel02);
        if (parcel03 > 0) rs = rs.Where(p => p.Parcel03 == parcel03);
        if (parcel04 > 0) rs = rs.Where(p => p.Parcel04 == parcel04);
        if (parcel05 > 0) rs = rs.Where(p => p.Parcel05 == parcel05);
        if (parcel06 > 0) rs = rs.Where(p => p.Parcel06 == parcel06);
        if (parcel07 > 0) rs = rs.Where(p => p.Parcel07 == parcel07);

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
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3").Replace(".","").Replace(",",".");
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
            td2.Text = TotalButir.ToString("N0").Replace(".","").Replace(",",".");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = TotalCarat.ToString("N3").Replace(".","").Replace(",",".");
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}