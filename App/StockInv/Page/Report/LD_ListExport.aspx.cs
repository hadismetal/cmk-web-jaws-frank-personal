using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page
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

        Response.AppendHeader("content-disposition", "attachment; filename=DJ_List_ExportExcel.xls");
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false && !p.StockProductLD.Draft.Value
                 && p.StockActualLD_Stone1B.IDLokasi > 0
                 && !(from s in db.SalesOrderLD
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                      select s.IDProduct).Contains(p.ID)
                 && !(from s in db.SalesOrderSouvenir
                      where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                      select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);
        }

        rs = rs.OrderByDescending(p => p.StockProductLD.ApprovalTgl);

        string kw = Request.QueryString["kw"];

        if (kw != "")
            rs = rs.Where(p => p.Nomor == "" + kw + "" || p.GIA == "" + kw + "");
        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == Cabang && p.StockActualLD_Stone1B.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe);

        int parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        int parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        int parcel05 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int parcel06 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int parcel07 = Convert.ToInt32(Request.QueryString["parcel07"]);

        /*if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel07 == parcel07.SelectedIndex);*/


        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalCarat = 0m;
            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string NamaParcel = "";
                try
                {
                    NamaParcel = r.Stone1B.Nama;
                }
                catch { }

                td = new TableCell();
                td.Text = NamaParcel;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.GIA;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Db.SingleString("select top 1 m_report from table_sarin where m_gia = '" + r.GIA + "' ", Db.Cnn_CS);
                tr.Cells.Add(td);


                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualLD_Stone1B.Substorage == 1)
                    Jual = r.StockActualLD_Stone1B.InHand;
                else if (r.StockActualLD_Stone1B.Substorage == 3)
                    Exchange = r.StockActualLD_Stone1B.InHand;
                else if (r.StockActualLD_Stone1B.Substorage == 2)
                    Resell = r.StockActualLD_Stone1B.InHand;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualLD_Stone1B.Substorage);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.StockActualLD_Stone1B.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualLD_Stone1B.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualLD_Stone1B.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalCarat.ToString("N3").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                string Status = "Available";
                string BookLokasi = "";
                string Href = "";
                int Cancel = 0;
                if (r.BookExpire.HasValue && r.BookExpire > DateTime.Now)
                {
                    Cancel = 1;
                    Status = "Booking oleh ";
                    if (r.TipeLokasi == 1)
                        BookLokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                    else
                        BookLokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                    Status += BookLokasi;
                }

                td = new TableCell();
                td.Text = Status;
                tr.Cells.Add(td);

                /// Shape
                td = new TableCell();
                td.Text = r.Stone1B.Parcel101.Nama;
                tr.Cells.Add(td);

                /// Size
                td = new TableCell();
                td.Text = r.Stone1B.Parcel102.Nama;
                tr.Cells.Add(td);

                /// Brand
                td = new TableCell();
                td.Text = r.Stone1B.Parcel106.Nama;
                tr.Cells.Add(td);

                /// Colour
                td = new TableCell();
                td.Text = Convert.ToString(r.Stone1B.Parcel103.Nama);
                tr.Cells.Add(td);

                /// Clarity
                td = new TableCell();
                td.Text = Convert.ToString(r.Stone1B.Parcel104.Nama);
                tr.Cells.Add(td);



                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                TotalCarat += Convert.ToDecimal(r.TotalCarat);

            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 4;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualLD_Stone1B.InHand).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3").Replace(".", "").Replace(",", ".") + "</b>";
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

                tb.Rows.Add(tr);
            }
        }


    }
}