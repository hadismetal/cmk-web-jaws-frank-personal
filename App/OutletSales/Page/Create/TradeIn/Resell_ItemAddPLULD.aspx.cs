using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_LD_StoneAdd : System.Web.UI.Page {
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
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

            DLC.Parcel101(parcel101b);
            DLC.Parcel102(parcel102b, true);
            DLC.Parcel103(parcel103b);
            DLC.Parcel104(parcel104b);
            DLC.Parcel105(parcel105b);
            DLC.Parcel106(parcel106b);
            DLC.Parcel107(parcel107b);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.Resell.SingleOrDefault(p => p.ID == myIDRO);
		if (d == null) DataError();
		else {

			//Cancel-Link
			cancel.HRef += "?id=" + myID;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

		//Customize-Qstr
        href += "&parcel101b=" + parcel101b.SelectedValue;
        href += "&parcel102b=" + parcel102b.SelectedValue;
        href += "&parcel103b=" + parcel103b.SelectedValue;
        href += "&parcel104b=" + parcel104b.SelectedValue;
        href += "&parcel105b=" + parcel105b.SelectedValue;
        href += "&parcel106b=" + parcel106b.SelectedValue;
        href += "&parcel107b=" + parcel107b.SelectedValue;
		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
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
	protected void fillTable(object sender, EventArgs e) {
		fill_1B();
	}
	protected void fill_1B() {
        var rs = from p in db.Stone1B
                 where
                    !p.Disable
                    && p.StonePricing1B.Harga != null

                 select p;

        if (parcel101b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel101b.SelectedValue));
        if (parcel102b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel102b.SelectedValue));
        if (parcel103b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel103b.SelectedValue));
        if (parcel104b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel104b.SelectedValue));
        if (parcel105b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(parcel105b.SelectedValue));
        if (parcel106b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(parcel106b.SelectedValue));
        if (parcel107b.SelectedIndex > 0)
            rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(parcel107b.SelectedValue));

        rs = rs.OrderBy(p => p.Nama);

        if (rs.Count() > 0)
        {
            rs = rs.OrderByDescending(p => p.StonePricing1B.Harga.Value);
            string href = string.Empty;

            href += "Resell_LD_StoneAddSave.aspx";
            href += "?id=" + myID;
            href += "&so=" + myIDSO;
            href += "&ro=" + myIDRO;
            href += "&si=1";
            href += "&idstone=" + rs.First().ID;

            Response.Redirect(href);
        }
	}
}