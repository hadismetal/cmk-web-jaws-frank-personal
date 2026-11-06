using System;
using System.Linq;

public partial class Page_Register_DJ_ProductUpdate : System.Web.UI.Page
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
	protected int myTableID { get { return App.QueryInt(this, "tid"); } }

	protected void Page_Load(object sender, EventArgs e)
	{
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack)
		{
			qty.Focus();

			//Number-Format
			App.NumberFormat(harga);
			App.NumberFormat(qty);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Product.aspx"); }
	protected void LinkError() { Response.Redirect("ProductInfo.aspx?id=" + myID); }

	//Form
	protected void fillForm()
	{
		var d = db.StockReceivePackaging.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else
		{
			var x = db.StockReceivePackaging_Product.SingleOrDefault(p => p.ID == myTableID);
			if (x == null) LinkError();
			else
			{
				harga.Text = Convert.ToDecimal(x.Harga).ToString("N0");
				qty.Text = Convert.ToDecimal(x.Qty).ToString("N0");
			}
		}
	}

	//Save
	protected bool valid
	{
		get
		{
			bool x = true;

			x = x && Fv.isDecimal(harga, harga_ERR);
			x = x && Fv.isDecimal(qty, qty_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e)
	{
		if (valid)
		{
			var d = db.StockReceivePackaging.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else
			{
				bool andrianus = false;
				if (Convert.ToDecimal(qty.Text) == 0)
				{
					qty_ERR.InnerHtml = "Quantity tidak boleh kosong";
					qty_ERR.Visible = true;

					andrianus = true;
				}

				if (Convert.ToDecimal(harga.Text) < 1)
				{
					harga_ERR.InnerHtml = "Harga tidak boleh kosong, silahkan isi di menu master terlebih dahulu.";
					harga_ERR.Visible = true;

					andrianus = true;
				}

				if (andrianus)
				{
					return;
				}

				//Process
				var x = db.StockReceivePackaging_Product.SingleOrDefault(p => p.ID == myTableID);
				x.Qty = Convert.ToInt32(qty.Text);

				//Redirect
				Response.Redirect("ProductInfo.aspx?id=" + myID);
			}
		}
	}
	protected void cancel_Click(object sender, EventArgs e)
	{
		//Redirect
		Response.Redirect("ProductInfo.aspx?id=" + myID);
	}
}