using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Design_Design : System.Web.UI.Page {
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
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Summary.aspx"); }

	//Form
    protected void fillForm()
    {
        decimal TotalBatu = 0m;
        decimal TotalKarat = 0m;

        var rs = db.ProductDesign.SingleOrDefault(x => x.ID == Convert.ToInt32(myID));

        if (rs != null)
        {
            tgl.Text = Str.FullDate(rs.Tgl);

            estimasi_harga.Text = (Convert.ToDecimal(rs.ProductDesign_PricingMU.HargaRupiahRound).ToString("N0"));
            Kode.Text = rs.Nomor;
            NamaDesigner.Text = rs.DataHuman.Nama;
            ApprovedBy.Text = rs.Approval;
            Toko.Text = rs.DataHuman.CompanyBrand.Nama;
            customernama.Text = rs.RequestCustomerNama;

            Upload.Image(imgpicture, rs.ImgPicture);

            TableHeaderRow hr;
            TableHeaderCell hc;

            TableRow r;
            TableCell c;

            hr = new TableHeaderRow();

            hc = new TableHeaderCell(); hc.Text = "Parcel"; hr.Cells.Add(hc);
            hc = new TableHeaderCell(); hc.Text = "Butir"; hr.Cells.Add(hc);
            hc = new TableHeaderCell(); hc.Text = "Total Carat"; hr.Cells.Add(hc);
            hc = new TableHeaderCell(); hc.Text = "Color"; hr.Cells.Add(hc);
            hc = new TableHeaderCell(); hc.Text = "Clarity"; hr.Cells.Add(hc);
            hc = new TableHeaderCell(); hc.Text = "GIA"; hr.Cells.Add(hc);

            tbl_List_Parcel.Rows.Add(hr);

            var rsParcel1A = from p in db.ProductDesign_Stone1A
                             where p.IDForm == Convert.ToInt32(myID)
                             select p;
            if (rsParcel1A != null)
            {

                foreach (var rslt in rsParcel1A)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone1A.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone1A.Parcel103.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone1A.Parcel104.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            var rsParcel1B = from p in db.ProductDesign_Stone1B
                             where p.IDForm == Convert.ToInt32(myID)
                             select p;
            if (rsParcel1B != null)
            {

                foreach (var rslt in rsParcel1B)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone1B.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone1B.Parcel103.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone1B.Parcel104.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            var rsParcel2 = from p in db.ProductDesign_Stone2
                            where p.IDForm == Convert.ToInt32(myID)
                            select p;
            if (rsParcel2 != null)
            {

                foreach (var rslt in rsParcel2)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone2.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone2.Parcel203.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone2.Parcel204.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            var rsParcel3 = from p in db.ProductDesign_Stone3
                            where p.IDForm == Convert.ToInt32(myID)
                            select p;
            if (rsParcel3 != null)
            {

                foreach (var rslt in rsParcel3)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone3.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone3.Parcel303.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone3.Parcel304.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            var rsParcel4 = from p in db.ProductDesign_Stone4
                            where p.IDForm == Convert.ToInt32(myID)
                            select p;
            if (rsParcel4 != null)
            {

                foreach (var rslt in rsParcel4)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone4.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone4.Parcel403.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            var rsParcel5 = from p in db.ProductDesign_Stone5
                            where p.IDForm == Convert.ToInt32(myID)
                            select p;
            if (rsParcel5 != null)
            {

                foreach (var rslt in rsParcel5)
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "<b>" + rslt.Stone5.KodeParcel + "</b>";
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalButir.ToString("N0");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.TotalCarat.ToString("N3");
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone5.Parcel503.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rslt.Stone5.Parcel504.Nama;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    r.Cells.Add(c);

                    tbl_List_Parcel.Rows.Add(r);

                    TotalBatu += rslt.TotalButir;
                    TotalKarat += rslt.TotalCarat;
                }
            }

            r = new TableRow();
            c = new TableCell();
            c.Text = "<b>Total</b>";
            r.Cells.Add(c);

            c = new TableCell();
            c.Text = TotalBatu.ToString("N0");
            r.Cells.Add(c);

            c = new TableCell();
            c.Text = TotalKarat.ToString("N3");
            r.Cells.Add(c);

            tbl_List_Parcel.Rows.Add(r);
        }
    }
}