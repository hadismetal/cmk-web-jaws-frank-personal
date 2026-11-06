using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Titipan_StoneAdd : System.Web.UI.Page
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
    protected int myIDLokasi { get { return App.QueryInt(this,"idlokasi"); } }
    protected int myTipeLokasi { get { return App.QueryInt(this, "tipe"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {


            if (myTipeLokasi == 0 || myIDLokasi == 0)
                Response.Redirect("LD_LocationType.aspx");

            DLC.Parcel101(parcel101b);
            DLC.Parcel102(parcel102b, true);
            DLC.Parcel103(parcel103b);
            DLC.Parcel104(parcel104b);
            DLC.Parcel105(parcel105b);
            DLC.Parcel106(parcel106b);
            DLC.Parcel107(parcel107b);
            //Form-Default
            //stoneindex.SelectedIndex = App.SessionInt("StoneIndex");

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
        href += "&parcel101b=" + parcel101b.SelectedValue;
        href += "&parcel102b=" + parcel102b.SelectedValue;
        href += "&parcel103b=" + parcel103b.SelectedValue;
        href += "&parcel104b=" + parcel104b.SelectedValue;
        href += "&parcel105b=" + parcel105b.SelectedValue;
        href += "&parcel106b=" + parcel106b.SelectedValue;
        href += "&parcel107b=" + parcel107b.SelectedValue;
        href += "&idlokasi=" + myIDLokasi;
        href += "&tipe=" + myTipeLokasi;

        //Create-Session
        //App.Session("StoneIndex", stoneindex.SelectedIndex.ToString());

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //stoneindex.SelectedIndex = App.SessionInt("StoneIndex");

            parcel101b.SelectedValue = Request.QueryString["parcel101b"];
            parcel102b.SelectedValue = Request.QueryString["parcel102b"];
            parcel103b.SelectedValue = Request.QueryString["parcel103b"];
            parcel104b.SelectedValue = Request.QueryString["parcel104b"];
            parcel105b.SelectedValue = Request.QueryString["parcel105b"];
            parcel106b.SelectedValue = Request.QueryString["parcel106b"];
            parcel107b.SelectedValue = Request.QueryString["parcel107b"];

        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        fill_1B();
    }

    protected void fill_1B()
    {
        //SELECT
        var rs = from p in db.StockActualLD_Stone1B
                 where p.TipeLokasi == 0 &&
                    !p.StockProductLD_Stone1B.Stone1B.Disable &&
                    p.StockProductLD_Stone1B.ReturnProduct == false &&
                    (p.StockProductLD_Stone1B.BookExpire < DateTime.Now || !p.StockProductLD_Stone1B.BookExpire.HasValue)
                    && !(from z in db.StockProductDJLD
                         where !z.Draft.Value
                         select z.IDProductLD).Contains(p.IDProduct)
                 select p;

        if (parcel101b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel01 == Convert.ToInt32(parcel101b.SelectedValue));
        if (parcel102b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel02 == Convert.ToInt32(parcel102b.SelectedValue));
        if (parcel103b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel03 == Convert.ToInt32(parcel103b.SelectedValue));
        if (parcel104b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel04 == Convert.ToInt32(parcel104b.SelectedValue));
        if (parcel105b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel05 == Convert.ToInt32(parcel105b.SelectedValue));
        if (parcel106b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel06 == Convert.ToInt32(parcel106b.SelectedValue));
        if (parcel107b.SelectedIndex > 0)
            rs = rs.Where(p => p.StockProductLD_Stone1B.Stone1B.Parcel07 == Convert.ToInt32(parcel107b.SelectedValue));

        rs = rs.OrderBy(p => p.StockProductLD_Stone1B.Stone1B.Nama);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            string Shape = r.StockProductLD_Stone1B.Stone1B.Parcel101.Nama;
            string Size = r.StockProductLD_Stone1B.Stone1B.Parcel103.Nama;
            string Colour = r.StockProductLD_Stone1B.Stone1B.Parcel102.Nama;
            string Clarity = r.StockProductLD_Stone1B.Stone1B.Parcel104.Nama;
            string PLU = r.StockProductLD_Stone1B.Nomor;
            if (!Response.IsClientConnected) break;
            fill(index, r.IDProduct, PLU, Shape, Size, Colour, Clarity, r.StockProductLD_Stone1B.Stone1B.KodeParcel);
            index++;
        }
    }

    //Display Row
    protected void fill(int index, int ID, string PLU,  string Shape, string Size, string Colour, string Clarity, string KodeParcel)
    {
        TableRow tr = new TableRow();
        TableCell td;

        td = new TableCell();
        td.Text = TD_HyperLink(ID);
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p>" + index++.ToString() + ".</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = TD_Nama(PLU);
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = TD_Nama(Shape);
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = TD_Nama(Colour);
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = TD_Nama(Size);
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = TD_Nama(Clarity);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "LD_ItemAddSave.aspx";
        href += "?id=" + ID;
        href += "&idlokasi=" + myIDLokasi;
        href += "&tipe=" + myTipeLokasi;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_KodeParcel(string KodeParcel)
    {
        string x = string.Empty;

        x += "<p class=\"text-muted\">" + KodeParcel + "</p>";

        return x;
    }
}