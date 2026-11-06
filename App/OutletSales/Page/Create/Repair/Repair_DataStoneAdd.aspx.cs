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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

			//Form
			fillForm();

            stone1a.Visible = true;

            //DLC
            DLC.StoneIndex(stoneindex);
            stoneindex.SelectedIndex = 0;

            DLC.Parcel101(parcel101a);
            DLC.Parcel102(parcel102a, false);
            //DLC.Parcel103(parcel103a);
            DLC.Parcel104(parcel104a);

            DLC.Parcel101(parcel101b);
            DLC.Parcel102(parcel102b, true);
            DLC.Parcel103(parcel103b);
            DLC.Parcel104(parcel104b);
            DLC.Parcel105(parcel105b);
            DLC.Parcel106(parcel106b);
            DLC.Parcel107(parcel107b);

            DLC.Parcel201(parcel201);
            DLC.Parcel202(parcel202);
            DLC.Parcel203(parcel203);
            DLC.Parcel204(parcel204);

            DLC.Parcel301(parcel301);
            DLC.Parcel302(parcel302);
            DLC.Parcel303(parcel303);
            DLC.Parcel304(parcel304);

            DLC.Parcel401(parcel401);
            DLC.Parcel402(parcel402);
            DLC.Parcel403(parcel403);

            DLC.Parcel501(parcel501);
            DLC.Parcel502(parcel502);
            DLC.Parcel503(parcel503);
            DLC.Parcel504(parcel504);
            DLC.Parcel505(parcel505);
            DLC.Parcel506(parcel506);
            DLC.Parcel507(parcel507);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

    protected void StoneChange(object sender, EventArgs e)
    {
        stone1a.Visible = false;
        stone1b.Visible = false;
        stone2.Visible = false;
        stone3.Visible = false;
        stone4.Visible = false;
        stone5.Visible = false;
        if (stoneindex.SelectedValue == "0")
            stone1a.Visible = true;
        if (stoneindex.SelectedIndex == 1)
            stone1b.Visible = true;
        if (stoneindex.SelectedIndex == 2)
            stone2.Visible = true;
        if (stoneindex.SelectedIndex == 3)
            stone3.Visible = true;
        if (stoneindex.SelectedIndex == 4)
            stone4.Visible = true;
        if (stoneindex.SelectedIndex == 5)
            stone5.Visible = true;

        fillTable(sender, e);
    }

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

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
        href += "&stoneindex=" + stoneindex.SelectedValue;
        href += "&parcel101a=" + parcel101a.SelectedValue;
        href += "&parcel102a=" + parcel102a.SelectedValue;
        href += "&parcel103a=" + parcel103a.SelectedValue;
        href += "&parcel104a=" + parcel104a.SelectedValue;
        href += "&parcel101b=" + parcel101b.SelectedValue;
        href += "&parcel102b=" + parcel102b.SelectedValue;
        href += "&parcel103b=" + parcel103b.SelectedValue;
        href += "&parcel104b=" + parcel104b.SelectedValue;
        href += "&parcel105b=" + parcel105b.SelectedValue;
        href += "&parcel106b=" + parcel106b.SelectedValue;
        href += "&parcel107b=" + parcel107b.SelectedValue;
        href += "&parcel201=" + parcel201.SelectedValue;
        href += "&parcel202=" + parcel202.SelectedValue;
        href += "&parcel203=" + parcel203.SelectedValue;
        href += "&parcel204=" + parcel204.SelectedValue;
        href += "&parcel301=" + parcel301.SelectedValue;
        href += "&parcel302=" + parcel302.SelectedValue;
        href += "&parcel303=" + parcel303.SelectedValue;
        href += "&parcel304=" + parcel304.SelectedValue;
        href += "&parcel401=" + parcel401.SelectedValue;
        href += "&parcel402=" + parcel402.SelectedValue;
        href += "&parcel403=" + parcel403.SelectedValue;
        href += "&parcel501=" + parcel501.SelectedValue;
        href += "&parcel502=" + parcel502.SelectedValue;
        href += "&parcel503=" + parcel503.SelectedValue;
        href += "&parcel504=" + parcel504.SelectedValue;
        href += "&parcel505=" + parcel505.SelectedValue;
        href += "&parcel506=" + parcel506.SelectedValue;
        href += "&parcel507=" + parcel507.SelectedValue;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
            //stoneindex.SelectedIndex = App.SessionInt("StoneIndex");
            stoneindex.SelectedIndex = Convert.ToInt32(Request.QueryString["stoneindex"]);
            parcel101a.SelectedValue = Request.QueryString["parcel101a"];
            parcel102a.SelectedValue = Request.QueryString["parcel102a"];
            parcel103a.SelectedValue = Request.QueryString["parcel103a"];
            parcel104a.SelectedValue = Request.QueryString["parcel104a"];
            parcel101b.SelectedValue = Request.QueryString["parcel101b"];
            parcel102b.SelectedValue = Request.QueryString["parcel102b"];
            parcel103b.SelectedValue = Request.QueryString["parcel103b"];
            parcel104b.SelectedValue = Request.QueryString["parcel104b"];
            parcel105b.SelectedValue = Request.QueryString["parcel105b"];
            parcel106b.SelectedValue = Request.QueryString["parcel106b"];
            parcel107b.SelectedValue = Request.QueryString["parcel107b"];
            parcel201.SelectedValue = Request.QueryString["parcel201"];
            parcel202.SelectedValue = Request.QueryString["parcel202"];
            parcel203.SelectedValue = Request.QueryString["parcel203"];
            parcel204.SelectedValue = Request.QueryString["parcel204"];
            parcel301.SelectedValue = Request.QueryString["parcel301"];
            parcel302.SelectedValue = Request.QueryString["parcel302"];
            parcel303.SelectedValue = Request.QueryString["parcel303"];
            parcel304.SelectedValue = Request.QueryString["parcel304"];
            parcel401.SelectedValue = Request.QueryString["parcel401"];
            parcel402.SelectedValue = Request.QueryString["parcel402"];
            parcel403.SelectedValue = Request.QueryString["parcel403"];
            parcel501.SelectedValue = Request.QueryString["parcel501"];
            parcel502.SelectedValue = Request.QueryString["parcel502"];
            parcel503.SelectedValue = Request.QueryString["parcel503"];
            parcel504.SelectedValue = Request.QueryString["parcel504"];
            parcel505.SelectedValue = Request.QueryString["parcel505"];
            parcel506.SelectedValue = Request.QueryString["parcel506"];
            parcel507.SelectedValue = Request.QueryString["parcel507"];

            stone1a.Visible = false;
            stone1b.Visible = false;
            stone2.Visible = false;
            stone3.Visible = false;
            stone4.Visible = false;
            stone5.Visible = false;
            if (stoneindex.SelectedValue == "0")
                stone1a.Visible = true;
            if (stoneindex.SelectedIndex == 1)
                stone1b.Visible = true;
            if (stoneindex.SelectedIndex == 2)
                stone2.Visible = true;
            if (stoneindex.SelectedIndex == 3)
                stone3.Visible = true;
            if (stoneindex.SelectedIndex == 4)
                stone4.Visible = true;
            if (stoneindex.SelectedIndex == 5)
                stone5.Visible = true;
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		fill();
	}
    protected void fill()
    {
        if (stoneindex.SelectedIndex == 0) fill_1A();
        if (stoneindex.SelectedIndex == 1) fill_1B();
        if (stoneindex.SelectedIndex == 2) fill_2();
        if (stoneindex.SelectedIndex == 3) fill_3();
        if (stoneindex.SelectedIndex == 4) fill_4();
        if (stoneindex.SelectedIndex == 5) fill_5();
    }

    protected void fill_1A()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone1A
                  join q in db.StonePricing1A on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel101a.SelectedIndex > 0 ? parcel101a.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel102a.SelectedIndex > 0 ? parcel102a.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel103a.SelectedIndex > 0 ? parcel103a.SelectedValue.ToString() : p.Parcel03.ToString())
                     && p.Parcel04.ToString() == (parcel104a.SelectedIndex > 0 ? parcel104a.SelectedValue.ToString() : p.Parcel04.ToString())
                  select new
                  {
                     ID = p.ID,
                     Nama = p.Nama,
                     KodeParcel = p.KodeParcel,
                     Parcel01 = p.Parcel101.Nama,
                     Parcel02 = p.Parcel102.Nama,
                     Parcel03 = p.Parcel103.Nama,
                     Parcel04 = p.Parcel104.Nama,
                     Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01
                        + " " + r.Parcel02
                        + " " + r.Parcel03
                        + " " + r.Parcel04
                        ;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }

    }
    protected void fill_1B()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone1B
                  join q in db.StonePricing1B on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel101b.SelectedIndex > 0 ? parcel101b.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel102b.SelectedIndex > 0 ? parcel102b.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel103b.SelectedIndex > 0 ? parcel103b.SelectedValue.ToString() : p.Parcel03.ToString())
                     && p.Parcel04.ToString() == (parcel104b.SelectedIndex > 0 ? parcel104b.SelectedValue.ToString() : p.Parcel04.ToString())
                     && p.Parcel05.ToString() == (parcel105b.SelectedIndex > 0 ? parcel105b.SelectedValue.ToString() : p.Parcel05.ToString())
                     && p.Parcel06.ToString() == (parcel106b.SelectedIndex > 0 ? parcel106b.SelectedValue.ToString() : p.Parcel06.ToString())
                     && p.Parcel07.ToString() == (parcel107b.SelectedIndex > 0 ? parcel107b.SelectedValue.ToString() : p.Parcel07.ToString())
                  select new
                  {
                      ID = p.ID,
                      Nama = p.Nama,
                      KodeParcel = p.KodeParcel,
                      Parcel01 = p.Parcel101.Nama,
                      Parcel02 = p.Parcel102.Nama,
                      Parcel03 = p.Parcel103.Nama,
                      Parcel04 = p.Parcel104.Nama,
                      Parcel05 = p.Parcel105.Nama,
                      Parcel06 = p.Parcel106.Nama,
                      Parcel07 = p.Parcel107.Nama,
                      Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01 
                        + " " + r.Parcel02 
                        + " " + r.Parcel03 
                        + " " + r.Parcel04
                        + " " + r.Parcel05
                        + " " + r.Parcel06
                        + " " + r.Parcel07
                        ;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }

    }
    protected void fill_2()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone2
                  join q in db.StonePricing2 on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel201.SelectedIndex > 0 ? parcel201.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel202.SelectedIndex > 0 ? parcel202.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel203.SelectedIndex > 0 ? parcel203.SelectedValue.ToString() : p.Parcel03.ToString())
                     && p.Parcel04.ToString() == (parcel204.SelectedIndex > 0 ? parcel204.SelectedValue.ToString() : p.Parcel04.ToString())
                  select new
                  {
                      ID = p.ID,
                      Nama = p.Nama,
                      KodeParcel = p.KodeParcel,
                      Parcel01 = p.Parcel201.Nama,
                      Parcel02 = p.Parcel202.Nama,
                      Parcel03 = p.Parcel203.Nama,
                      Parcel04 = p.Parcel204.Nama,
                      Parcel05 = p.Parcel205.Nama,
                      Parcel06 = p.Parcel206.Nama,
                      Parcel07 = p.Parcel207.Nama,
                      Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01
                        + " " + r.Parcel02
                        + " " + r.Parcel03
                        + " " + r.Parcel04
                        + " " + r.Parcel05
                        + " " + r.Parcel06
                        + " " + r.Parcel07
                        ;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }

    }
    protected void fill_3()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone3
                  join q in db.StonePricing3 on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel301.SelectedIndex > 0 ? parcel301.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel302.SelectedIndex > 0 ? parcel302.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel303.SelectedIndex > 0 ? parcel303.SelectedValue.ToString() : p.Parcel03.ToString())
                     && p.Parcel04.ToString() == (parcel304.SelectedIndex > 0 ? parcel304.SelectedValue.ToString() : p.Parcel04.ToString())
                  select new
                  {
                      ID = p.ID,
                      Nama = p.Nama,
                      KodeParcel = p.KodeParcel,
                      Parcel01 = p.Parcel301.Nama,
                      Parcel02 = p.Parcel302.Nama,
                      Parcel03 = p.Parcel303.Nama,
                      Parcel04 = p.Parcel304.Nama,
                      Parcel05 = p.Parcel305.Nama,
                      Parcel06 = p.Parcel306.Nama,
                      Parcel07 = p.Parcel307.Nama,
                      Parcel08 = p.Parcel308.Nama,
                      Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01
                        + " " + r.Parcel02
                        + " " + r.Parcel03
                        + " " + r.Parcel04
                        + " " + r.Parcel05
                        + " " + r.Parcel06
                        + " " + r.Parcel07
                        + " " + r.Parcel08
                        ;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_4()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone4
                  join q in db.StonePricing4 on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel401.SelectedIndex > 0 ? parcel401.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel402.SelectedIndex > 0 ? parcel402.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel403.SelectedIndex > 0 ? parcel403.SelectedValue.ToString() : p.Parcel03.ToString())
                  select new
                  {
                      ID = p.ID,
                      Nama = p.Nama,
                      KodeParcel = p.KodeParcel,
                      Parcel01 = p.Parcel401.Nama,
                      Parcel02 = p.Parcel402.Nama,
                      Parcel03 = p.Parcel403.Nama,
                      Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01
                        + " " + r.Parcel02
                        + " " + r.Parcel03
                        ;
            fill(index, r.ID, r.Nama, r.KodeParcel);
        }
    }
    protected void fill_5()
    {
        // langsung select batu yang termahal

        //SELECT
        var rs = (from p in db.Stone5
                  join q in db.StonePricing5 on p.ID equals q.ID
                  where !p.Disable
                     && p.Parcel01.ToString() == (parcel501.SelectedIndex > 0 ? parcel501.SelectedValue.ToString() : p.Parcel01.ToString())
                     && p.Parcel02.ToString() == (parcel502.SelectedIndex > 0 ? parcel502.SelectedValue.ToString() : p.Parcel02.ToString())
                     && p.Parcel03.ToString() == (parcel503.SelectedIndex > 0 ? parcel503.SelectedValue.ToString() : p.Parcel03.ToString())
                     && p.Parcel04.ToString() == (parcel504.SelectedIndex > 0 ? parcel504.SelectedValue.ToString() : p.Parcel04.ToString())
                     && p.Parcel05.ToString() == (parcel505.SelectedIndex > 0 ? parcel505.SelectedValue.ToString() : p.Parcel05.ToString())
                     && p.Parcel06.ToString() == (parcel506.SelectedIndex > 0 ? parcel506.SelectedValue.ToString() : p.Parcel06.ToString())
                     && p.Parcel07.ToString() == (parcel507.SelectedIndex > 0 ? parcel507.SelectedValue.ToString() : p.Parcel07.ToString())
                  select new
                  {
                      ID = p.ID,
                      Nama = p.Nama,
                      KodeParcel = p.KodeParcel,
                      Parcel01 = p.Parcel501.Nama,
                      Parcel02 = p.Parcel502.Nama,
                      Parcel03 = p.Parcel503.Nama,
                      Parcel04 = p.Parcel504.Nama,
                      Parcel05 = p.Parcel505.Nama,
                      Parcel06 = p.Parcel506.Nama,
                      Parcel07 = p.Parcel507.Nama,
                      Parcel08 = p.Parcel508.Nama,
                      Parcel09 = p.Parcel509.Nama,
                      Harga = q.Harga,
                  }).OrderByDescending(q => q.Harga).FirstOrDefault();

        //NO-RESULT
        if (rs == null) { fillnodata(); /*App.TableNoResult(tb);*/ }
        else
        {
            //RESULT
            int index = 1;
            var r = rs;
            string nama = r.Parcel01
                        + " " + r.Parcel02
                        + " " + r.Parcel03
                        + " " + r.Parcel04
                        + " " + r.Parcel05
                        + " " + r.Parcel06
                        + " " + r.Parcel07
                        + " " + r.Parcel08
                        + " " + r.Parcel09
                        ;
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
	
	protected void fillnodata()
	{
		TableRow tr = new TableRow();
		TableCell td;

		td = new TableCell();
		td.ColumnSpan = 4;
		td.Text = "No Data";
		td.Style.Add("text-align","center");
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