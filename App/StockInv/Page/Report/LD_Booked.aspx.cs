using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {

            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, true);
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

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false && p.BookExpire > DateTime.Now
                 && !(from s in db.SalesOrderLD
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                      select s.IDProduct).Contains(p.ID)
                 select p;

        rs = rs.OrderByDescending(p => p.StockProductLD.ApprovalTgl);

        
        if(kw.Text!="")
            rs = rs.Where(p=>p.Nomor==""+kw.Text+"");
        
        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel04 == parcel04.SelectedIndex);
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel05 == parcel05.SelectedIndex);
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel06 == parcel06.SelectedIndex);
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel07 == parcel07.SelectedIndex);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        if (rs != null)
        {
            int index = 1;
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
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string NamaParcel = "";
                try
                {
                    NamaParcel = r.Stone1B.Nama;
                }catch{}

                td = new TableCell();
                td.Text = NamaParcel;
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
                td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalCarat).ToString("N3");
                tr.Cells.Add(td);

                string Status = "Available";
                string BookLokasi = "";
                string Href= "";
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

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
                TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
                TotalCarat += Convert.ToDecimal(r.TotalCarat);
                index++;
            }

            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rs.Sum(p => p.StockActualLD_Stone1B.InHand).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
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
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&parcel01=" + parcel01.SelectedIndex;
        href2 += "&parcel02=" + parcel02.SelectedIndex;
        href2 += "&parcel03=" + parcel03.SelectedIndex;
        href2 += "&parcel04=" + parcel04.SelectedIndex;
        href2 += "&parcel05=" + parcel05.SelectedIndex;
        href2 += "&parcel06=" + parcel06.SelectedIndex;
        href2 += "&parcel07=" + parcel07.SelectedIndex;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>";
        paging += "</div>";

        paging_text.Text = paging;
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
        }
    }

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

        Response.Redirect(href);
    }
}