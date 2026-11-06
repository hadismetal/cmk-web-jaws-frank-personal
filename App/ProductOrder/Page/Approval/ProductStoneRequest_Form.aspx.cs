using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_ProductStoneRequest_Form : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.ProductStoneRequest.SingleOrDefault(p => p.ID == myID && p.Approval == null);
        if (d == null) App.DataNull(this);
        else
        {
            noproductstonerequest.InnerText = d.NomorDokumen;
            tgl.InnerText = Str.Tgl(d.Tgl);
            brand.InnerText = d.CompanyBrand.Nama;
            tgljatuhtempo.InnerText = Str.Tgl(d.TglJatuhTempo);
            keterangan.InnerText = d.Keterangan;

            fillStone();
        }

    }

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        //Stone
        fillStone1A(0, ref GT_Butir, ref GT_Carat);
        fillStone1B(1, ref GT_Butir, ref GT_Carat);
        fillStone2(2, ref GT_Butir, ref GT_Carat);
        fillStone3(3, ref GT_Butir, ref GT_Carat);
        fillStone4(4, ref GT_Butir, ref GT_Carat);
        fillStone5(5, ref GT_Butir, ref GT_Carat);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
    }

    //Stone
    protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT

        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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



        var rs = (from p in db.ProductOrder_Stone1A
                 where parcel.Contains(p.IDForm)
                 group p by new { p.Stone1A.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                  select new { KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        if (rs != null)
        {
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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

        var rs = (from p in db.ProductOrder_Stone1B
                    where parcel.Contains(p.IDForm)
                    group p by new { p.GIA, p.Stone1B.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                    select new { GIA = z.Key.GIA, KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        //RESULT
        int index = 1;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            //Total
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }
    protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        //SELECT
        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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

        var rs = (from p in db.ProductOrder_Stone2
                  where parcel.Contains(p.IDForm)
                  group p by new { p.Stone2.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                  select new { KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        //RESULT
        int index = 1;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            //Total
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }
    protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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

        var rs = (from p in db.ProductOrder_Stone3
                  where parcel.Contains(p.IDForm)
                  group p by new { p.Stone3.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                  select new { KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        //RESULT
        int index = 1;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            //Total
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }
    protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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

        var rs = (from p in db.ProductOrder_Stone4
                  where parcel.Contains(p.IDForm)
                  group p by new { p.Stone4.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                  select new { KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        //RESULT
        int index = 1;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            //Total
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }
    protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
    {
        //SELECT
        var stone = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID && p.ProductStoneRequest.Approval == null
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

        var rs = (from p in db.ProductOrder_Stone5
                  where parcel.Contains(p.IDForm)
                  group p by new { p.Stone5.KodeParcel, p.DimensiP, p.DimensiL, p.DimensiT } into z
                  select new { KodeParcel = z.Key.KodeParcel, DimensiP = z.Key.DimensiP, DimensiL = z.Key.DimensiL, DimensiT = z.Key.DimensiT, TotalCarat = z.Sum(o => o.TotalCarat), TotalButir = z.Sum(o => o.TotalButir) }
                  ).OrderBy(x => x.KodeParcel);

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Carat = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        //RESULT
        int index = 1;
        if (rs != null)
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(r.KodeParcel);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiP);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiL);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Dimensi(r.DimensiT);
                td.CssClass = "text-center";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }

            //Total
            if (rs.Count() > 0)
            {
                GT_Butir += ST_Butir;
                GT_Carat += ST_Carat;

                Sub_Total(ST_Butir, ST_Carat, false);
            }
        }
    }

    //Custom Row
    protected void Sub_Header(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 5;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected string TD_Dimensi(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number2(Dimensi) + "</p>";

        return x;
    }
    protected string TD_CaratButir(decimal? CaratButir)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

        return x;
    }
    protected string TD_TotalButir(int? TotalButir, ref int ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

        //Sub-Total
        if (TotalButir.HasValue) ST += TotalButir.Value;

        return x;
    }
    protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

        //Sub-Total
        if (TotalCarat.HasValue) ST += TotalCarat.Value;

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.ProductStoneRequest.SingleOrDefault(p => p.ID == myID && p.Approval == null);
        if (d == null) App.DataNull(this);
        else
        {
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;

            db.SubmitChanges();

            //Redirect
            Response.Redirect("ProductStoneRequest.aspx?d=1");
        }
    }
    protected void reject_Click(object sender, EventArgs e)
    {
        var d = db.ProductStoneRequest.SingleOrDefault(p => p.ID == myID && p.Approval == null);
        if (d == null) App.DataNull(this);
        else
        {
            var x = from p in db.ProductStoneRequest_Detail
                    where p.IDProductStoneRequest == myID
                    select p;
            if (x != null)
            {
                foreach (var r in x)
                {
                    var s = db.ProductOrder.SingleOrDefault(p=>p.ID == r.IDProductOrder);
                    if (s != null)
                    {
                        s.StatusFPS = 0;
                        db.SubmitChanges();
                    }

                }
            }
            Db.Execute("DELETE FROM ProductStoneRequest_Detail WHERE IDProductStoneRequest=" + myID, Db.Cnn_DB);
            Db.Execute("DELETE FROM ProductStoneRequest WHERE ID=" + myID, Db.Cnn_DB);

            //Redirect
            Response.Redirect("ProductStoneRequest.aspx?d=1");
        }
    }
}