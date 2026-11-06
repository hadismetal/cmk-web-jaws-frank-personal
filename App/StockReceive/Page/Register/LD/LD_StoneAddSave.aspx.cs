using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

public partial class Page_Register_LD_StoneAddSave : System.Web.UI.Page {
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
	protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
	protected int myIDStone { get { return App.QueryInt(this, "idstone"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			totalcarat.Focus();

			//Number-Format
			App.NumberFormat(totalcarat);
			App.NumberFormat(dimensip);
			App.NumberFormat(dimensil);
			App.NumberFormat(dimensit);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }
	protected void LinkError() { Response.Redirect("LD_Stone.aspx?id=" + myID); }
	protected void ProductError() { Response.Redirect("LD_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

			//Stone Data
			stone_index.InnerHtml = DL.StoneIndex(myStoneIndex);
			fillStone1B();
		}
	}

	//Stone Data
	protected void fillStone1B() {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

			//Size Control
			if (!d.Parcel102.SizeMin.HasValue || !d.Parcel102.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Parcel102.SizeMin,
					d.Parcel102.SizeMax
					);

			//GIA
			div_gia.Visible = true;
		}
	}

	//Size Control
	protected void sizeError() {
		save.Disabled = true;

		//Belum Setup
		sizecontrol.Attributes["class"] = "text-red";
		sizecontrol.InnerHtml = "<i class=\"icon-warning-sign\"></i> Belum Setup";
	}
	protected void sizeDisplay(decimal? SizeMin, decimal? SizeMax) {
		string x = string.Empty;

		x += Str.Number3(SizeMin);
		x += " - ";
		x += Str.Number3(SizeMax);

		sizecontrol.InnerHtml = x;
	}

	//Validasi Carat
	protected bool carat1B(int TotalButir, decimal TotalCarat) {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Parcel102.SizeMin.Value;
			decimal Max = d.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isDecimal(totalcarat, totalcarat_ERR);
			x = x && Fv.isDecimal(dimensip, dimensip_ERR);
			x = x && Fv.isDecimal(dimensil, dimensil_ERR, true);
			x = x && Fv.isDecimal(dimensit, dimensit_ERR, true);

			//Value
			x = x && valid_TotalButir;
			x = x && valid_TotalCarat;

			//Size-Control
			x = x && valid_CaratButir;

			return x;
		}
	}
	protected bool valid_TotalButir {
		get {
			int dv = 1;

			bool x = dv > 0;

			return x;
		}
	}
	protected bool valid_TotalCarat {
		get {
			decimal dv = Convert.ToDecimal(totalcarat.Text);

			bool x = dv > 0;
			totalcarat_VAL.Visible = !x;

			return x;
		}
	}
	protected bool valid_CaratButir {
		get {
			bool x = true;

			int TotalButir = 1;
			decimal TotalCarat = Convert.ToDecimal(totalcarat.Text);

			x = carat1B(TotalButir, TotalCarat);
	
			caratbutir_ERR.Visible = !x;
			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				save1B(d);
				//Redirect
				Response.Redirect("LD_Stone.aspx?id=" + myID);
			}
		}
	}

	//Process
	// protected void save1B(StockReceiveLD ms) {
	// 	using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
	// 	{
	// 		conn.Open();

	// 		string sqlSelect = @"
	// 			SELECT TOP 1 ID, Disable
	// 			FROM Stone1B
	// 			WHERE ID = @ID AND Disable = 0";

	// 		SqlCommand cmdSelect = new SqlCommand(sqlSelect, conn);
	// 		cmdSelect.Parameters.AddWithValue("@ID", myIDStone);

	// 		SqlDataReader reader = cmdSelect.ExecuteReader();

	// 		if (!reader.Read())
	// 		{
	// 			reader.Close();
	// 			LinkError();
	// 			return;
	// 		}

	// 		int stoneID = Convert.ToInt32(reader["ID"]);
	// 		reader.Close();

	// 		decimal totalCarat = Convert.ToDecimal(totalcarat.Text);
	// 		decimal dimP = Convert.ToDecimal(dimensip.Text);
	// 		decimal? dimL = dimensil.Text != "" ? (decimal?)Convert.ToDecimal(dimensil.Text) : null;
	// 		decimal? dimT = dimensit.Text != "" ? (decimal?)Convert.ToDecimal(dimensit.Text) : null;

	// 		string giaVal = Str.CapUpper(gia.Text);
	// 		int productItem = DLC.ProductItem();

	// 		string sql = @"
	// 			INSERT INTO StockReceiveLD_Stone1B
	// 			(
	// 				IDForm, IDStone, TotalButir, TotalCarat, CaratButir,
	// 				DimensiP, DimensiL, DimensiT, GIA, ProductItem, IsConsignment
	// 			)
	// 			VALUES
	// 			(
	// 				@IDForm, @IDStone, 1, @TotalCarat, @CaratButir,
	// 				@DimensiP, @DimensiL, @DimensiT, @GIA, @ProductItem, @IsConsignment
	// 			)";

	// 		SqlCommand cmdInsert = new SqlCommand(sql, conn);

	// 		cmdInsert.Parameters.AddWithValue("@IDForm", ms.ID);
	// 		cmdInsert.Parameters.AddWithValue("@IDStone", stoneID);
	// 		cmdInsert.Parameters.AddWithValue("@TotalCarat", totalCarat);
	// 		cmdInsert.Parameters.AddWithValue("@CaratButir", totalCarat);
	// 		cmdInsert.Parameters.AddWithValue("@DimensiP", dimP);

	// 		cmdInsert.Parameters.AddWithValue("@DimensiL", dimL.HasValue ? (object)dimL.Value : DBNull.Value);
	// 		cmdInsert.Parameters.AddWithValue("@DimensiT", dimT.HasValue ? (object)dimT.Value : DBNull.Value);

	// 		cmdInsert.Parameters.AddWithValue("@GIA", giaVal);
	// 		cmdInsert.Parameters.AddWithValue("@ProductItem", productItem);
	// 		cmdInsert.Parameters.AddWithValue("@IsConsignment", ms.IsConsignment);

	// 		cmdInsert.ExecuteNonQuery();
	// 	}

	// 	conn.Close();
	// }
	protected void save1B(StockReceiveLD ms) {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else
		{
			decimal totalCarat = Convert.ToDecimal(totalcarat.Text);
			decimal dimP = Convert.ToDecimal(dimensip.Text);
			decimal? dimL = dimensil.Text != "" ? (decimal?)Convert.ToDecimal(dimensil.Text) : null;
			decimal? dimT = dimensit.Text != "" ? (decimal?)Convert.ToDecimal(dimensit.Text) : null;

			string giaVal = Str.CapUpper(gia.Text);
			int productItem = DLC.ProductItem();

			using (SqlConnection conn =  new SqlConnection(Db.Cnn_DB))
			{
				conn.Open();
				string sqlConsignment = @"
					SELECT TOP 1 IsConsignment
					FROM StockReceiveLD
					WHERE ID = @ID";

				SqlCommand cmdConsignment = new SqlCommand(sqlConsignment, conn);
				cmdConsignment.Parameters.AddWithValue("@ID", ms.ID);

				object result = cmdConsignment.ExecuteScalar();

				bool isConsignment = false;
				if (result != null && result != DBNull.Value)
				{
					isConsignment = Convert.ToBoolean(result);
				}
				string sql = @"
					INSERT INTO StockReceiveLD_Stone1B
					(
						IDForm, IDStone, TotalButir, TotalCarat, CaratButir,
						DimensiP, DimensiL, DimensiT, GIA, ProductItem, IsConsignment
					)
					VALUES
					(
						@IDForm, @IDStone, 1, @TotalCarat, @CaratButir,
						@DimensiP, @DimensiL, @DimensiT, @GIA, @ProductItem, @IsConsignment
					)";

				SqlCommand cmd = new SqlCommand(sql, conn);

				cmd.Parameters.AddWithValue("@IDForm", ms.ID);
				cmd.Parameters.AddWithValue("@IDStone", d.ID);
				cmd.Parameters.AddWithValue("@TotalCarat", totalCarat);
				cmd.Parameters.AddWithValue("@CaratButir", totalCarat);
				cmd.Parameters.AddWithValue("@DimensiP", dimP);

				cmd.Parameters.AddWithValue("@DimensiL", dimL.HasValue ? (object)dimL.Value : DBNull.Value);
				cmd.Parameters.AddWithValue("@DimensiT", dimT.HasValue ? (object)dimT.Value : DBNull.Value);

				cmd.Parameters.AddWithValue("@GIA", giaVal);
				cmd.Parameters.AddWithValue("@ProductItem", productItem);
            	cmd.Parameters.Add("@IsConsignment", SqlDbType.Bit).Value = 0;

				cmd.ExecuteNonQuery();
			}
		}

	}
}