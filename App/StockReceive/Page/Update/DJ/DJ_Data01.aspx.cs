using System;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Data01 : System.Web.UI.Page {
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
    protected int myIDKodeKaret { get { return App.QueryInt(this,"idkodekaret"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			search_proto.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
		if (d == null) DataError();
		else {

            deleteProcess(d);

			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_proto.HRef += qstr;
			search_productorder.HRef += qstr;
			search_supplier.HRef += qstr;
			// search_outlet.HRef += qstr;
			search_customer.HRef += qstr;

			//Disabled-State
            if (d.Proto > 0)
            {
                protonomor.Enabled = false;
                protonomor.Text = d.ProtoNomor;

                var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (proto == null) DataError();
                else
                {
                    requestcustomernama.Text = proto.ProductOrder.RequestCustomerNama;
                    // requestoutletnama.Text = proto.ProductOrder.RequestOutletNama;
                    productordernomor.Enabled = false;
                    productordernomor.Text = proto.NoSPK;
                }
            }
            else
            {
                protonomor.Text = d.ProtoNomor;
            }
			if (d.ProductOrder > 0) {
				productordernomor.Enabled = false;
				productordernomor.Text = d.ProductOrderNomor;
			}
			if (d.Supplier > 0) {
				suppliernama.Enabled = false;
				suppliernama.Text = d.SupplierNama;
			}
			// if (d.RequestOutlet > 0) {
			// 	requestoutletnama.Enabled = false;
			// 	requestoutletnama.Text = d.RequestOutletNama;
			// }
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
			}

            suppliernama.Text = d.SupplierNama;
            suppliernomor.Text = d.SupplierNomor;
            productordernomor.Text = d.ProductOrderNomor;
            // requestoutletnama.Text = d.RequestOutletNama;
            requestcustomernama.Text = d.RequestCustomerNama;
            string typeLocation = "";
            string locationId = "";

            // ambil dari DB (RAW SQL)
            DataTable dtLoc = Db.Rs(
                "SELECT TOP 1 LocationType, RequestOutlet FROM StockReceiveDJ WHERE ID = '" + myID + "'",
                Db.Cnn_DB
            );

            if (dtLoc != null && dtLoc.Rows.Count > 0)
            {
                typeLocation = dtLoc.Rows[0]["LocationType"].ToString();
                locationId = dtLoc.Rows[0]["RequestOutlet"].ToString();
            }

            // set location type dulu
            if (!string.IsNullOrEmpty(typeLocation))
            {
                ddl_location_type.SelectedValue = typeLocation;

                // trigger isi dropdown location
                ddl_location_type_SelectedIndexChanged(null, null);
            }

            // set location
            if (!string.IsNullOrEmpty(locationId))
            {
                if (ddl_location.Items.FindByValue(locationId) != null)
                {
                    ddl_location.SelectedValue = locationId;
                }

                // optional: disable kalau sudah ada
                // ddl_location.Enabled = false;
                // ddl_location_type.Enabled = false;
            }

		}
	}

    protected void ddl_location_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = ddl_location_type.SelectedValue;
        string selected = ddl_location.SelectedValue;
        DataTable dt = new DataTable();

        if (type == "0")
            dt = Db.Rs("SELECT ID, Nama FROM locwarehouse", Db.Cnn_DB);
        else if (type == "1")
            dt = Db.Rs("SELECT ID, Nama FROM locoutlet", Db.Cnn_DB);
        else if (type == "2")
            dt = Db.Rs("SELECT ID, Nama FROM locexhibition", Db.Cnn_DB);

        ddl_location.DataSource = dt;
        ddl_location.DataTextField = "Nama";
        ddl_location.DataValueField = "ID";
        ddl_location.DataBind();
        if (!string.IsNullOrEmpty(selected) && ddl_location.Items.FindByValue(selected) != null)
        {
            ddl_location.SelectedValue = selected;
        }

        ddl_location.Items.Insert(0, new ListItem("-- Select Location --", ""));
    }
    // protected void ddl_location_type_SelectedIndexChanged(object sender, EventArgs e)
    // {
    //     ddl_location.Items.Clear();

    //     if (ddl_location_type.SelectedValue == "1")
    //     {
    //         // Outlet
    //         var dt = Db.Rs("SELECT ID, Nama FROM locoutlet ORDER BY Nama", Db.Cnn_DB);

    //         ddl_location.DataSource = dt;
    //         ddl_location.DataTextField = "Nama";
    //         ddl_location.DataValueField = "ID";
    //         ddl_location.DataBind();
    //     }
    //     else if (ddl_location_type.SelectedValue == "2")
    //     {
    //         // Exhibition
    //         var dt = Db.Rs("SELECT ID, Nama FROM locexhibition ORDER BY Nama", Db.Cnn_DB);

    //         ddl_location.DataSource = dt;
    //         ddl_location.DataTextField = "Nama";
    //         ddl_location.DataValueField = "ID";
    //         ddl_location.DataBind();
    //     }
    //     if (ddl_location_type.SelectedValue == "0")
    //     {
    //         // Warehouse
    //         var dt = Db.Rs("SELECT ID, Nama FROM locwarehouse ORDER BY Nama", Db.Cnn_DB);

    //         ddl_location.DataSource = dt;
    //         ddl_location.DataTextField = "Nama";
    //         ddl_location.DataValueField = "ID";
    //         ddl_location.DataBind();
    //     }
    //     else
    //     {
    //         // Warehouse (optional kalau ada table)
    //         ddl_location.Items.Clear();
    //     }
    // }

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			d.ProtoNomor = Str.CapUpper(protonomor.Text.Replace("/","-"));
			d.ProductOrderNomor = Str.CapUpper(productordernomor.Text);

            var sp = (from p in db.DataSupplier
                      where p.Nama.Equals(suppliernama.Text)
                      select p);
            if (sp.Count() > 0)
            {
                var spl = sp.First();
                d.Supplier = spl.ID;
            }

            int UserID = App.UserID;
            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;
            if (f.Count() > 0)
            {
                var sk = f.First();
                d.HumanStock = sk.ID;
                d.HumanStockNama = sk.Nama;
            }

			d.SupplierNama = Str.CapTitle(suppliernama.Text);
			d.SupplierNomor = Str.CapUpper(suppliernomor.Text);

			// d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);
            
            int locationType = 0;
            int locationId = 0;

            int.TryParse(ddl_location_type.SelectedValue, out locationType);
            int.TryParse(ddl_location.SelectedValue, out locationId);

            // save ke DB
            // d.LocationType = locationType;
            // d.RequestOutlet = locationId;
            Db.Execute(
                "UPDATE StockReceiveDJ SET LocationType = " + locationType + ", RequestOutlet = " + locationId + " WHERE ID = " + myID,
                Db.Cnn_DB
            );

			//Commit

            if (locationType == 0) // Warehouse
            {
                var warehouse = db.LocWarehouse.SingleOrDefault(p => p.ID == locationId);
                if (warehouse != null)
                {
                    d.RequestOutletNama = warehouse.Nama;
                }
            }
            else if (locationType == 1) // Outlet
            {
                var outlet = db.LocOutlet.SingleOrDefault(p => p.ID == locationId);
                if (outlet != null)
                {
                    d.RequestOutletNama = outlet.Nama;
                }
            }
            else if (locationType == 2) // Exhibition
            {
                var exhibition = db.LocExhibition.SingleOrDefault(p => p.ID == locationId);
                if (exhibition != null)
                {
                    d.RequestOutletNama = exhibition.Nama;
                }
            }

			db.SubmitChanges();

			//Redirect
			Response.Redirect("DJ_Data02.aspx?id=" + myID);
		}
	}

    protected void deleteProcess(StockReceiveDJ d)
    {
        if (d.StockReceiveDJ_PricingProduct != null)
            db.StockReceiveDJ_PricingProduct.DeleteOnSubmit(
                d.StockReceiveDJ_PricingProduct);

        if (d.StockReceiveDJ_CostingProduct != null)
            db.StockReceiveDJ_CostingProduct.DeleteOnSubmit(
                d.StockReceiveDJ_CostingProduct);

        if (d.StockReceiveDJ_PricingBiaya != null)
            db.StockReceiveDJ_PricingBiaya.DeleteOnSubmit(
                d.StockReceiveDJ_PricingBiaya);

        if (d.StockReceiveDJ_PricingMU != null)
            db.StockReceiveDJ_PricingMU.DeleteOnSubmit(
                d.StockReceiveDJ_PricingMU);

        if (d.StockReceiveDJ_PricingSegmen != null)
            db.StockReceiveDJ_PricingSegmen.DeleteOnSubmit(
                d.StockReceiveDJ_PricingSegmen);

        //Commit
        db.SubmitChanges();
    }
}