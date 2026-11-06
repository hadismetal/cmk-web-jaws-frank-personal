using System;
using System.Linq;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Page_Register_LD_Data01 : System.Web.UI.Page {
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

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_supplier.HRef += qstr;
			search_outlet.HRef += qstr;
			search_customer.HRef += qstr;

            if (d.Supplier > 0)
            {
                suppliernama.Enabled = false;
                suppliernama.Text = d.SupplierNama;
            }

		}
	}

	//Save
	// protected void save_Click(object sender, EventArgs e)
	// {
	// 	var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);

	// 	if (d == null)
	// 	{
	// 		DataError();
	// 		return;
	// 	}

	// 	string selectedValue = "";
	// 	if (RadioConsign.Checked)
	// 		selectedValue = "1";
	// 	else if (RadioNonConsign.Checked)
	// 		selectedValue = "0";

	// 	d.IsConsignment = selectedValue == "1"; 

	// 	var sp = (from p in db.DataSupplier
	// 			where p.Nama.Equals(suppliernama.Text)
	// 			select p);

	// 	if (sp.Any())
	// 	{
	// 		var spl = sp.First();
	// 		d.Supplier = spl.ID;
	// 	}

	// 	d.SupplierNama = Str.CapTitle(suppliernama.Text);
	// 	d.SupplierNomor = Str.CapUpper(suppliernomor.Text);

	// 	int UserID = App.UserID;
	// 	var f = from p in db.DataSK
	// 			where p.IDUser == UserID
	// 			select p;

	// 	if (f.Any())
	// 	{
	// 		var sk = f.First();
	// 		d.HumanStock = sk.ID;
	// 		d.HumanStockNama = sk.Nama;
	// 	}

	// 	d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
	// 	d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

	// 	//Commit
	// 	db.SubmitChanges();

	// 	//Redirect
	// 	Response.Redirect("LD_Data02.aspx?id=" + myID);
	// }

	protected void save_Click(object sender, EventArgs e)
	{
        // Map_DB db = new Map_DB();

		using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
		{
			connection.Open();

			string checkQuery = "SELECT TOP 1 * FROM StockReceiveLD WHERE ID = @ID AND Draft = 1";
			SqlCommand cmdCheck = new SqlCommand(checkQuery, connection);
			cmdCheck.Parameters.AddWithValue("@ID", myID);

			DataTable dt = new DataTable();
			using (SqlDataAdapter da = new SqlDataAdapter(cmdCheck))
			{
				da.Fill(dt);
			}

			if (dt.Rows.Count == 0)
			{
				DataError();
				return;
			}

			DataRow d = dt.Rows[0];

			// bool isConsignment = RadioConsign.Checked;
			bool isConsignment = false;

			int? supplierID = 0;
			string supplierQuery = "SELECT TOP 1 ID FROM DataSupplier WHERE Nama = @Nama";
			using (SqlCommand cmdSupp = new SqlCommand(supplierQuery, connection))
			{
				cmdSupp.Parameters.AddWithValue("@Nama", suppliernama.Text);
				var result = cmdSupp.ExecuteScalar();
				if (result != null)
					supplierID = Convert.ToInt32(result);
			}

			int? humanStockID = 0;
			string humanStockNama = null;
			string skQuery = "SELECT TOP 1 ID, Nama FROM DataSK WHERE IDUser = @IDUser";
			using (SqlCommand cmdSK = new SqlCommand(skQuery, connection))
			{
				cmdSK.Parameters.AddWithValue("@IDUser", App.UserID);
				using (SqlDataReader reader = cmdSK.ExecuteReader())
				{
					if (reader.Read())
					{
						humanStockID = reader.GetInt32(0);
						humanStockNama = reader.GetString(1);
					}
				}
			}

			string updateQuery = @"
				UPDATE StockReceiveLD
				SET
					IsConsignment = @IsConsignment,
					Supplier = @Supplier,
					SupplierNama = @SupplierNama,
					SupplierNomor = @SupplierNomor,
					HumanStock = @HumanStock,
					HumanStockNama = @HumanStockNama,
					RequestOutletNama = @RequestOutletNama,
					RequestCustomerNama = @RequestCustomerNama
				WHERE ID = @ID AND Draft = 1";

			using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, connection))
			{
				cmdUpdate.Parameters.AddWithValue("@IsConsignment", isConsignment);
				cmdUpdate.Parameters.AddWithValue("@Supplier", (object)supplierID ?? DBNull.Value);
				cmdUpdate.Parameters.AddWithValue("@SupplierNama", Str.CapTitle(suppliernama.Text));
				cmdUpdate.Parameters.AddWithValue("@SupplierNomor", Str.CapUpper(suppliernomor.Text));
				cmdUpdate.Parameters.AddWithValue("@HumanStock", (object)humanStockID ?? DBNull.Value);
				cmdUpdate.Parameters.AddWithValue("@HumanStockNama", (object)humanStockNama ?? DBNull.Value);
				cmdUpdate.Parameters.AddWithValue("@RequestOutletNama", Str.CapTitle(requestoutletnama.Text));
				cmdUpdate.Parameters.AddWithValue("@RequestCustomerNama", Str.CapTitle(requestcustomernama.Text));
				cmdUpdate.Parameters.AddWithValue("@ID", myID);

				cmdUpdate.ExecuteNonQuery();
			}

			connection.Close();
		}

		// Redirect after update
		Response.Redirect("LD_Data02.aspx?id=" + myID);
	}

}