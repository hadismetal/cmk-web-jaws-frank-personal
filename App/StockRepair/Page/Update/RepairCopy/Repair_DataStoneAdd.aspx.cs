using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Repair_StoneAdd : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

			//Form
			fillForm();

            DLC.StoneIndex(stoneindex);
            

            //Form-Default
            stoneindex.SelectedIndex = App.SessionInt("StoneIndex");

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //protected void StoneChange(object sender, EventArgs e)
    //{
    //    stone1a.Visible = false;
    //    stone1b.Visible = false;
    //    stone2.Visible = false;
    //    stone3.Visible = false;
    //    stone4.Visible = false;
    //    stone5.Visible = false;
    //    if (stoneindex.SelectedIndex == 0)
    //        stone1a.Visible = true;
    //    if (stoneindex.SelectedIndex == 1)
    //        stone1b.Visible = true;
    //    if (stoneindex.SelectedIndex == 2)
    //        stone2.Visible = true;
    //    if (stoneindex.SelectedIndex == 3)
    //        stone3.Visible = true;
    //    if (stoneindex.SelectedIndex == 4)
    //        stone4.Visible = true;
    //    if (stoneindex.SelectedIndex == 5)
    //        stone5.Visible = true;
    //}

    //Exception
    protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&stoneindex=" + stoneindex.SelectedIndex;
        href += "&kw=" + kw.Text;

        //Create-Session
        App.Session("StoneIndex", stoneindex.SelectedIndex.ToString());

        Response.Redirect(href);
    }
	protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            stoneindex.SelectedIndex = App.SessionInt("StoneIndex");
            kw.Text = Request.QueryString["kw"];
        }
    }

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		fill();
	}
	protected void fill() {
        if (stoneindex.SelectedIndex == 0) fill_1A();
        if (stoneindex.SelectedIndex == 1) fill_1B();
        if (stoneindex.SelectedIndex == 2) fill_2();
        if (stoneindex.SelectedIndex == 3) fill_3();
        if (stoneindex.SelectedIndex == 4) fill_4();
        if (stoneindex.SelectedIndex == 5) fill_5();
	}

    protected void fill_1A()
    {
        //SELECT
        var rs = from p in db.Stone1A
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        rs = rs.OrderBy(p => p.Nama);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            //string nama = r.Parcel101.Nama + " " + r.Parcel102.Nama + " " + r.Parcel103.Nama + " " + r.Parcel104.Nama;
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
            index++;
        }
    }
    protected void fill_1B()
    {
        //SELECT
        var rs = from p in db.Stone1B
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_2()
    {
        //SELECT
        var rs = from p in db.Stone2
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_3()
    {
        //SELECT
        var rs = from p in db.Stone3
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_4()
    {
        //SELECT
        var rs = from p in db.Stone4
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_5()
    {
        //SELECT
        var rs = from p in db.Stone5
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
                 orderby p.Nama
                 select p;

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }

    //Display Row
    protected void fill(int index, int ID, string Nama, string KodeParcel) {
		TableRow tr = new TableRow();
		TableCell td;

		td = new TableCell();
		td.Text = TD_HyperLink(ID);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p>" + index++.ToString() + ".</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Nama(Nama);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_KodeParcel(KodeParcel);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	//Display Column
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "Repair_DataStoneAddSave.aspx";
		href += "?id=" + myID;
        href += "&si=" + stoneindex.SelectedIndex;
		href += "&idstone=" + ID;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_KodeParcel(string KodeParcel) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + KodeParcel + "</p>";

		return x;
	}
}