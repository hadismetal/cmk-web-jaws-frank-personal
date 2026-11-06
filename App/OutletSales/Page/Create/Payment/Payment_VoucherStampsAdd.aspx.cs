using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page {
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

        int IDSO = myID;

        if (!IsPostBack)
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO && !p.Draft);
            if (d == null) DataError("1");
            else
            {
                kw.Focus(); //Keyword

                //Form
                fillForm(d);

                //Generate
                fillFilter();
                if (App.Refresh(this)) fillTable(d, sender, e);
                
            }
                
		}
	}

	//Exception
	protected void DataError(string err) { Response.Redirect("Payment.aspx?err=" + err); }

	//Form
	protected void fillForm(SalesOrder d) {
		// caller (Page_Load) already loaded and null-checked this SalesOrder;
		// it used to be re-fetched here by ID, which was a redundant round-trip
		//Header
		header_nomor.InnerHtml = d.Nomor;

		//Cancel-Link
		cancel.HRef += "?id=" + myID;
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			
			kw.Text = Request.QueryString["kw"];
		}
	}

    protected void fillTable(SalesOrder d, object sender, EventArgs e)
    {
        string voucherTypeAllowed = "PAYMENT"; // DISCOUNT or PAYMENT
        string keyword = kw.Text.Trim();
        string errMsg = "";

        div_panel_voucher.Visible = false;

        if (keyword != "")
        {
            int voucher_id = 0;
            string voucher_name = "";

            string voucher_validity = "";
            string voucher_type = "";
            string voucher_nominal = "";

            string voucher_date_start = "";
            string voucher_date_end = "";
            string voucher_valid_for = "";
            bool voucher_is_redeemable = false;
			
			if (keyword.StartsWith("UV") || keyword.StartsWith("JC") || keyword.StartsWith("JWLCER") || keyword.StartsWith("Y"))
            {
                // combined into a single round-trip - was 3 separate lazy EntitySet.Count queries
                int total = Db.SingleInteger(
                    "SELECT (SELECT COUNT(*) FROM SalesOrderPG WHERE IDForm=" + d.ID + ") + " +
                    "(SELECT COUNT(*) FROM SalesOrderGJ WHERE IDForm=" + d.ID + ") + " +
                    "(SELECT COUNT(*) FROM SalesOrderLD WHERE IDForm=" + d.ID + ")", Db.Cnn_DB);
                if(total > 0)
                {
                    if(keyword.StartsWith("JC500") || 
					   keyword.StartsWith("JWLCER") || 
					   keyword.StartsWith("JC2000") || 
					   keyword.StartsWith("JCCMK") || 
					   keyword.StartsWith("JC10000") || 
					   keyword.StartsWith("JC20000") || 
					   keyword.StartsWith("JCE500") || 
					   keyword.StartsWith("JCE1000") || 
					   keyword.StartsWith("JCE2000") ||
					   keyword.StartsWith("JCE5000") ||
					   keyword.StartsWith("JCE10000") ||
					   keyword.StartsWith("JCE20000") ||
					   keyword.StartsWith("JC200") ||
					   keyword.StartsWith("Y"))
					{
						errMsg = "";
					}
					else
					{
						errMsg = "Jewellery Certificate ini hanya bisa dipakai jika barang hanya Diamond Jewellery. Jika terdapat Plain Gold, Gold Jewellery atau Loose Diamond tidak bisa menggunakan Jewellery Certificate ini.";
					}
                }
            }

            string query = "sp_validate_voucher";
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@ID", SqlDbType.Int){ Value = myID },
                new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = keyword },
            };
            DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.StoredProcedure);
            bool success = Convert.ToBoolean(dt.Rows[0]["SUCCESS"]);
            string error = dt.Rows[0]["MESSAGE"].ToString();

            if (success)
            {
                errMsg = error;
            }
            else if (keyword.Length <= 20 && errMsg == "")
            {
				try
				{
					if (STAMPS.xActivate)
					{
						errMsg += ((errMsg == "") ? STAMPSM.ValidateVoucherCode(
							lit_msg
							, keyword
							, d
							, ref voucher_id
							, ref voucher_name
							, ref voucher_validity
							, ref voucher_type
							, ref voucher_nominal
							, ref voucher_date_start
							, ref voucher_date_end
							, ref voucher_valid_for
							, ref voucher_is_redeemable
							, voucherTypeAllowed
							) : "");
					}

				}
				catch (Exception) { errMsg = "Terjadi kendala teknis. Silahkan ulangi beberapa saat lagi. Jika masalah masih berlanjut silahkan hubungi bagian IT Support"; }
			}
			else if (keyword.Length > 20)
                errMsg = "Kode Voucher Lebih dari 20 digit. Voucher tidak valid.";
			else if(keyword.Length <= 20 && errMsg != "")
				errMsg = errMsg;
			
            if (errMsg != "")
            {
                lit_msg.Text = "<div class='alert alert-warning' role='alert'>" + errMsg + "</div>";
            }
            else
            {
                div_panel_voucher.Visible = true;

                string xinfo_left = "";
                string xinfo_right = "";

                xinfo_left += "<table>";
                xinfo_left += "<tr><td style='vertical-align:top;'>Voucher Type</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + voucher_name.ToUpper() + " # " + voucher_id + "</td></tr>";
                xinfo_left += "<tr><td style='vertical-align:top;'>Voucher Validity Type</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + voucher_validity.ToUpper() + "</td></tr>";
                xinfo_left += "</table>";

                xinfo_right += "<table>";
                xinfo_right += (voucher_validity.ToUpper() == "DYNAMIC") ? "" : "<tr><td style='vertical-align:top;'>Tanggal Terbit</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + voucher_date_start + "</td></tr>";
                xinfo_right += (voucher_validity.ToUpper() == "DYNAMIC") ? "" : "<tr><td style='vertical-align:top;'>Tanggal Expire</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + voucher_date_end + "</td></tr>";
                xinfo_right += (voucher_validity.ToUpper() == "STATIC") ? "" : "<tr><td style='vertical-align:top;'>Voucher Valid</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + voucher_valid_for + " day(s)</td></tr>";
                xinfo_right += "</table>";

                spn_voucher_nomorkode.InnerText = keyword;
                spn_voucher_nominal.InnerText = voucher_nominal;

                lit_voucher_link.Text = TD_HyperLink(keyword);
                lit_voucher_info_left.Text = xinfo_left;
                lit_voucher_info_right.Text = xinfo_right;
            }
        }
    }

    //Display Column
    protected string TD_HyperLink(string ID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "Payment_VoucherStampsAddSave.aspx";
		href += "?id=" + myID;
		href += "&iditem=" + ID;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\" onclick=\"showOverlay();\" >";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  ";
		x += "</a>";

		return x;
	}

    /*protected bool IsUsed(SalesOrder d)
    {
        bool isused = false;
        // d.SalesReceipt.Any(p => p.SalesReceiptDetail.Any(...)) dulu memuat semua SalesReceipt milik SO ini
        // ke memori (1 query) lalu lazy-load SalesReceiptDetail per receipt (N query). Query langsung ke
        // SalesReceiptDetail dengan filter IDSalesOrder menghasilkan predikat yang sama dalam 1 round-trip.
        isused = db.SalesReceiptDetail.Any(q => q.SalesReceipt.IDSalesOrder == d.ID && q.IDPaymentType == 5 && q.CCNumber == kw.Text.Trim());
        if (!isused)
        {
            isused = db.SalesReceipt.Any(p => p.SalesReceiptDetail.Any(q => q.IDPaymentType == 5 
			&& q.CCNumber == kw.Text.Trim() 
			&& (q.SalesReceipt.SalesOrder.StatusPembayaran == true || q.SalesReceipt.SalesOrder.StatusPembayaran == false)
			&& q.SalesReceipt.SalesOrder.StatusVoid == false
			&& q.SalesReceipt.SalesOrder.Draft == false));
        }

        return isused;
    }*/

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}