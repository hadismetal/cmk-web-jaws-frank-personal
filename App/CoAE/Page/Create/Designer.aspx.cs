using System;
using System.Linq;

public partial class Page_Create_Designer : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			group.Focus();

			//DLC
			DLC.DataHumanGroup(group);
			DLC.CompanyBrand(brand);

			//DLC-Control
			App.DropDown(save, group, group_ERR, "GroupDesigner");
			App.DropDown(save, brand, brand_ERR, "Brand");
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		//Session-Create
		App.Session("GroupDesigner", group.SelectedValue);
		App.Session("Brand", brand.SelectedValue);

		//Redirect
		string href = string.Empty;

		href += "DesignerAccount.aspx";
		href += "?idgroup=" + group.SelectedValue;
		href += "&idbrand=" + brand.SelectedValue;

		Response.Redirect(href);
	}
}