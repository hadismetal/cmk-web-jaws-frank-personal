using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_ProductStoneRequest : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("ProductStoneRequest.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.ProductStoneRequest.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            lit_NomorFPS1.Text = lit_NomorFPS2.Text = d.NomorDokumen;
            lit_TanggalFPS1.Text = lit_TanggalFPS2.Text = Str.Tgl(d.Tgl);
            lit_Brand1.Text = lit_Brand2.Text = d.CompanyBrand.Nama;
            lit_TanggalJatuhTempo1.Text = lit_TanggalJatuhTempo2.Text = Str.Tgl(d.TglJatuhTempo);
            lit_Keterangan1.Text = lit_Keterangan2.Text = d.Keterangan;

            fillStone();
        }
    }

    protected void fillStone()
    {
        TableHeaderRow hr = new TableHeaderRow();
        TableHeaderCell hc = new TableHeaderCell();

        TableRow r = new TableRow();
        TableCell c = new TableCell();

        #region rekap
        /// start header

        hr = new TableHeaderRow();

        hc = new TableHeaderCell();
        hc.Text = "PARCEL";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "DIMENSI";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "TOTAL CARAT";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "BUTIR";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "NO CERT";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        tbl_FPS_rekap.Rows.Add(hr);

        #endregion

        decimal Jumlah_Butir = 0m;
        decimal Jumlah_Carat = 0m;

        decimal GrandTotal_Butir = 0m;
        decimal GrandTotal_Carat = 0m;

        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval != null
                    select p;

        int[] parcel = new int[1000];
        int i = 0;
        if (stone.Count() > 0)
        {
            foreach (var st in stone)
            {
                parcel[i] = st.IDProductOrder;
                i++;
            }
        }

        decimal SubTotal_Butir = 0m;
        decimal SubTotal_Carat = 0m;

        string flag_parcel = "";

        var rs_idx = 0;
        var rs_count = 0;



        /// start parcel 1
        /// 
        var rs1A = (from p in db.ProductOrder_Stone1A
                    where parcel.Contains(p.IDForm)
                    group p by new { p.Stone1A.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                    select new { Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs1A.Count();
        if (rs1A.Count() > 0)
        {
            foreach (var rsp in rs1A)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = "";
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }


        /// start parcel 1B
        /// 

        SubTotal_Butir = 0;
        SubTotal_Carat = 0;

        var rs1B = (from p in db.ProductOrder_Stone1B
                    where parcel.Contains(p.IDForm)
                    group p by new { p.GIA, p.Stone1B.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                    select new { GIA = z.Key.GIA, Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs1B.Count();
        if (rs1B.Count() > 0)
        {
            foreach (var rsp in rs1B)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.GIA;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }

        /// start parcel 2
        /// 

        SubTotal_Butir = 0;
        SubTotal_Carat = 0;

        var rs2 = (from p in db.ProductOrder_Stone2
                   where parcel.Contains(p.IDForm)
                   group p by new { p.Stone2.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                   select new { Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs2.Count();
        if (rs2.Count() > 0)
        {
            foreach (var rsp in rs2)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = "";
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }

        /// start parcel 3
        /// 

        SubTotal_Butir = 0;
        SubTotal_Carat = 0;

        var rs3 = (from p in db.ProductOrder_Stone3
                   where parcel.Contains(p.IDForm)
                   group p by new { p.Stone3.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                   select new { Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs3.Count();
        if (rs3.Count() > 0)
        {
            foreach (var rsp in rs3)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = "";
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }

        /// start parcel 4
        /// 

        SubTotal_Butir = 0;
        SubTotal_Carat = 0;

        var rs4 = (from p in db.ProductOrder_Stone4
                   where parcel.Contains(p.IDForm)
                   group p by new { p.Stone4.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                   select new { Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs4.Count();
        if (rs4.Count() > 0)
        {
            foreach (var rsp in rs4)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = "";
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }

        /// start parcel 5
        /// 

        SubTotal_Butir = 0;
        SubTotal_Carat = 0;

        var rs5 = (from p in db.ProductOrder_Stone5
                   where parcel.Contains(p.IDForm)
                   group p by new { p.Stone5.Nama, p.DimensiP, p.DimensiL, p.DimensiT } into z
                   select new { Nama = z.Key.Nama, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.Nama);

        rs_idx = 0;
        rs_count = rs5.Count();
        if (rs5.Count() > 0)
        {
            foreach (var rsp in rs5)
            {
                Jumlah_Butir = rsp.TotalButir;
                Jumlah_Carat = rsp.TotalCarat;

                if (rs_idx != 0 && flag_parcel != rsp.Nama) /// for subtotal
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);

                    SubTotal_Butir = 0m;
                    SubTotal_Carat = 0m;
                }

                SubTotal_Butir += rsp.TotalButir;
                SubTotal_Carat += rsp.TotalCarat;

                r = new TableRow();

                c = new TableCell();
                c.Text = rsp.Nama; flag_parcel = rsp.Nama;
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalCarat.ToString("0.000");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = rsp.TotalButir.ToString("0");
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                c = new TableCell();
                c.Text = "";
                c.HorizontalAlign = HorizontalAlign.Center;
                r.Cells.Add(c);

                tbl_FPS_rekap.Rows.Add(r);

                GrandTotal_Butir += Jumlah_Butir;
                GrandTotal_Carat += Jumlah_Carat;

                rs_idx++;

                if (rs_idx == rs_count) /// this is the last item
                {
                    r = new TableRow();

                    c = new TableCell();
                    c.Text = "Sub Total";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Carat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = SubTotal_Butir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_rekap.Rows.Add(r);
                }
            }

        }


        #region grand total
        /// start last row data total

        r = new TableRow();

        c = new TableCell();
        c.Text = "<b>GRAND TOTAL</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "<b>" + GrandTotal_Carat.ToString("0.000") + "</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "<b>" + GrandTotal_Butir.ToString("0") + "</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        tbl_FPS_rekap.Rows.Add(r);

        /// end last row data total

        #endregion


        #region detail

        GrandTotal_Butir = 0m;
        GrandTotal_Carat = 0m;

        string flag_NomorSPK = "";

        /// start header

        hr = new TableHeaderRow();

        hc = new TableHeaderCell();
        hc.Text = "No. SPK";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "Tanggal SPK";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "PARCEL";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "DIMENSI";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "CARAT";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "BUTIR";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        hc = new TableHeaderCell();
        hc.Text = "NO CERT";
        hc.HorizontalAlign = HorizontalAlign.Center;
        hr.Cells.Add(hc);

        tbl_FPS_detail.Rows.Add(hr);

        /// end header , start data row
        /// 
        #endregion

        foreach (var st in stone)
        {
            var det1A = from p in db.ProductOrder_Stone1A
                        where p.IDForm == st.IDProductOrder
                        select p;
            if (det1A.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det1A)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone1A.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }

            var det1B = from p in db.ProductOrder_Stone1B
                        where p.IDForm == st.IDProductOrder
                        select p;
            if (det1B.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det1B)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone1B.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.GIA;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }

            var det2 = from p in db.ProductOrder_Stone2
                       where p.IDForm == st.IDProductOrder
                       select p;
            if (det2.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det2)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone2.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }

            var det3 = from p in db.ProductOrder_Stone3
                       where p.IDForm == st.IDProductOrder
                       select p;
            if (det3.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det3)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone3.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }

            var det4 = from p in db.ProductOrder_Stone4
                       where p.IDForm == st.IDProductOrder
                       select p;
            if (det4.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det4)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone4.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }

            var det5 = from p in db.ProductOrder_Stone5
                       where p.IDForm == st.IDProductOrder
                       select p;
            if (det5.Count() > 0)
            {
                Jumlah_Butir = 0m;
                Jumlah_Carat = 0m;

                int idx = 0;

                foreach (var rsp in det5)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    idx++;

                    r = new TableRow();

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? rsp.ProductOrder.NoSPK : ""; flag_NomorSPK = rsp.ProductOrder.NoSPK;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = flag_NomorSPK != rsp.ProductOrder.NoSPK || idx == 1 ? Str.Tgl(rsp.ProductOrder.Tgl) : "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.Stone5.Nama;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = (rsp.DimensiP != 0 ? rsp.DimensiP.ToString("N2") : "") + (rsp.DimensiL != 0 ? " x " + Convert.ToDecimal(rsp.DimensiL).ToString("N2") : "") + (rsp.DimensiT != 0 ? " x " + Convert.ToDecimal(rsp.DimensiT).ToString("N2") : "");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalCarat.ToString("0.000");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = rsp.TotalButir.ToString("0");
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    c = new TableCell();
                    c.Text = "";
                    c.HorizontalAlign = HorizontalAlign.Center;
                    r.Cells.Add(c);

                    tbl_FPS_detail.Rows.Add(r);

                    GrandTotal_Butir += Jumlah_Butir;
                    GrandTotal_Carat += Jumlah_Carat;
                }
            }
        }

        #region Grand Total Stone per SPK

        r = new TableRow();

        c = new TableCell();
        c.Text = "<b>GRAND TOTAL</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "<b>" + GrandTotal_Carat.ToString("0.000") + "</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "<b>" + GrandTotal_Butir.ToString("0") + "</b>";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        c = new TableCell();
        c.Text = "";
        c.HorizontalAlign = HorizontalAlign.Center;
        r.Cells.Add(c);

        tbl_FPS_detail.Rows.Add(r);

        /// end last row data total
        #endregion
    }
}