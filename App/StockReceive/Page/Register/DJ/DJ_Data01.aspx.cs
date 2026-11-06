using System;
using System.Linq;
using System.Data.SqlClient;

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
            search_proto.HRef = "DJ_KodeKaretList.aspx";
            search_proto.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_proto.HRef += qstr;
			search_productorder.HRef += qstr;
			search_supplier.HRef += qstr;
			search_outlet.HRef += qstr;
			search_customer.HRef += qstr;
            search_customer.HRef += qstr;
            //search_validation.HRef += qstr;

			//Disabled-State
			if (d.Proto > 0) {
				protonomor.Enabled = false;
				protonomor.Text = d.ProtoNomor;

                var proto = db.KodeKaret_Detail.SingleOrDefault(p=>p.ID == d.Proto);
                if(proto==null) DataError();
                else
                {
                    requestcustomernama.Text = proto.ProductOrder.RequestCustomerNama;
                    requestoutletnama.Text = proto.ProductOrder.RequestOutletNama;
                    productordernomor.Enabled = false;
                    productordernomor.Text = proto.NoSPK;
                }
			}
            if (d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt == null) DataError();
                else
                {
                    suppliernama.Enabled = false;
                    suppliernama.Text = mt.SupplierNama;
                    suppliernomor.Enabled = false;
                    suppliernomor.Text = mt.SupplierNomor;
                    requestoutletnama.Enabled = false;
                    requestoutletnama.Text = mt.RequestOutletNama;
                    requestcustomernama.Enabled = false;
                    requestcustomernama.Text = mt.RequestCustomerNama;
                    if (mt.Proto != null)
                    {
                        var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == mt.Proto);
                        if (proto != null)
                        {
                            requestcustomernama.Text = proto.ProductOrder.RequestCustomerNama;
                            requestoutletnama.Text = proto.ProductOrder.RequestOutletNama;
                            productordernomor.Enabled = false;
                            productordernomor.Text = proto.NoSPK;
                            protonomor.Text = proto.KodeKaretDetail;
                            protonomor.Enabled = false;
                        }
                    }
                }
            }
			if (d.ProductOrder > 0) {
				productordernomor.Enabled = false;
				productordernomor.Text = d.ProductOrderNomor;
			}
            //if (d.ProductOrder > 0)
            //{
            //    productordernomor.Enabled = false;
            //    productordernomor.Text = d.ProductOrderNomor;
            //}
			if (d.Supplier > 0) {
				suppliernama.Enabled = false;
				suppliernama.Text = d.SupplierNama;
			}
			if (d.RequestOutlet > 0) {
				requestoutletnama.Enabled = false;
				requestoutletnama.Text = d.RequestOutletNama;
			}
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
			}
            //if (d.NoValidasi != null)
            //{
            //    validationnomor.Enabled = false;
            //    validationnomor.Text = d.NoValidasi;
            //}
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        if (string.IsNullOrWhiteSpace(requestnovalidasi.Text))
        {
            lblNoValidError.Text = "No Validasi wajib diisi";
            lblNoValidError.Visible = true;
            return;
        }

        lblNoValidError.Visible = false;

        if (protonomor.Text != "")
        {
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError();
            else
            {
                d.ProtoNomor = Str.CapUpper(protonomor.Text.Replace("/","-"));
                d.ProductOrderNomor = Str.CapUpper(productordernomor.Text);

                if (d.Sumber == 1)
                {
                    var pt = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                    if (pt != null)
                        d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + ".jpg";
                }
                else
                    d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + ".jpg";

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

                d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
                d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

                //check existing novalidasi
                string noValidasi = requestnovalidasi.Text.Trim();

                int exists = Db.SingleInteger(
                    "SELECT COUNT(*) FROM StockReceiveDJ " +
                    "WHERE NoValidasi = '" + noValidasi.Replace("'", "''") + "' " +
                    "AND ID <> " + d.ID,
                    Db.Cnn_DB
                );
                if (exists > 0)
                {
                    lblNoValidError.Text = "No Validasi sudah digunakan";
                    lblNoValidError.Visible = true;
                    return;
                }

                db.SubmitChanges();
				Db.Execute("UPDATE StockReceiveDJ SET NoValidasi = '" + requestnovalidasi.Text + "' WHERE ID = " + d.ID, Db.Cnn_DB);

                //Redirect
                Response.Redirect("DJ_Data02.aspx?id=" + myID);
            }
        }
	}

    // protected void save_Click(object sender, EventArgs e)
    // {
    //     if (!Page.IsValid) return;
    //     if (protonomor.Text == "") return;

    //     var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
    //     if (d == null)
    //     {
    //         DataError();
    //         return;
    //     }

    //     string oldProtoNomor = d.ProtoNomor;
    //     string oldProductOrderNomor = d.ProductOrderNomor;
    //     string oldImg = d.ImgPicture;
    //     int? oldSupplier = d.Supplier;
    //     string oldSupplierNama = d.SupplierNama;
    //     string oldSupplierNomor = d.SupplierNomor;
    //     string oldReqOutletNama = d.RequestOutletNama;
    //     string oldReqCustNama = d.RequestCustomerNama;
    //     int? oldHumanStock = d.HumanStock;
    //     string oldHumanStockNama = d.HumanStockNama;
    //     string oldNoValidasi = d.NoValidasi;

    //     try
    //     {
    //         d.ProtoNomor = Str.CapUpper(protonomor.Text.Replace("/", "-"));
    //         d.ProductOrderNomor = Str.CapUpper(productordernomor.Text);

    //         if (d.Sumber == 1)
    //         {
    //             var pt = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
    //             if (pt != null)
    //                 d.ImgPicture = App.ImagesDir + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + ".jpg";
    //         }
    //         else
    //             d.ImgPicture = App.ImagesDir + "StockReceiveDJ\\" + d.ProtoNomor + ".jpg";

    //         var sp = db.DataSupplier.FirstOrDefault(p => p.Nama == suppliernama.Text);
    //         if (sp != null)
    //             d.Supplier = sp.ID;

    //         int UserID = App.UserID;
    //         var sk = db.DataSK.FirstOrDefault(p => p.IDUser == UserID);
    //         if (sk != null)
    //         {
    //             d.HumanStock = sk.ID;
    //             d.HumanStockNama = sk.Nama;
    //         }

    //         d.SupplierNama = Str.CapTitle(suppliernama.Text);
    //         d.SupplierNomor = Str.CapUpper(suppliernomor.Text);
    //         d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
    //         d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

    //         db.SubmitChanges();  

    //         bool ok = false;

    //         using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
    //         {
    //             conn.Open();

    //             SqlCommand check = new SqlCommand(
    //                 "SELECT COUNT(*) FROM StockReceiveDJ WHERE NoValidasi = @v AND ID <> @id",
    //                 conn);

    //             check.Parameters.AddWithValue("@v", requestnovalidasi.Text);
    //             check.Parameters.AddWithValue("@id", d.ID);

    //             int exists = Convert.ToInt32(check.ExecuteScalar());

    //             if (exists == 0)
    //             {
    //                 SqlCommand upd = new SqlCommand(
    //                     "UPDATE StockReceiveDJ SET NoValidasi = @v WHERE ID = @id", conn);

    //                 upd.Parameters.AddWithValue("@v", requestnovalidasi.Text);
    //                 upd.Parameters.AddWithValue("@id", d.ID);
    //                 upd.ExecuteNonQuery();
    //                 ok = true;
    //             }
    //         }

    //         if (!ok)
    //         {
    //             d.ProtoNomor = oldProtoNomor;
    //             d.ProductOrderNomor = oldProductOrderNomor;
    //             d.ImgPicture = oldImg;
    //             d.Supplier = oldSupplier;
    //             d.SupplierNama = oldSupplierNama;
    //             d.SupplierNomor = oldSupplierNomor;
    //             d.RequestOutletNama = oldReqOutletNama;
    //             d.RequestCustomerNama = oldReqCustNama;
    //             d.HumanStock = oldHumanStock;
    //             d.HumanStockNama = oldHumanStockNama;
    //             d.NoValidasi = oldNoValidasi;

    //             db.SubmitChanges();

    //             ScriptManager.RegisterStartupScript(
    //                 this, GetType(), "dup",
    //                 "toastr.error('No Validasi sudah digunakan');",
    //                 true);
    //             return;
    //         }

    //         Response.Redirect("DJ_Data02.aspx?id=" + myID);
    //     }
    //     catch
    //     {
    //         d.ProtoNomor = oldProtoNomor;
    //         d.ProductOrderNomor = oldProductOrderNomor;
    //         d.ImgPicture = oldImg;
    //         d.Supplier = oldSupplier;
    //         d.SupplierNama = oldSupplierNama;
    //         d.SupplierNomor = oldSupplierNomor;
    //         d.RequestOutletNama = oldReqOutletNama;
    //         d.RequestCustomerNama = oldReqCustNama;
    //         d.HumanStock = oldHumanStock;
    //         d.HumanStockNama = oldHumanStockNama;
    //         d.NoValidasi = oldNoValidasi;
    //         db.SubmitChanges();
    //         throw;
    //     }
    // }



}