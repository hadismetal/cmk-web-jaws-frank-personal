using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Sales_VoucherStampsAdd : System.Web.UI.Page
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

    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myItemType { get { return App.QueryInt(this, "itemtype"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        int IDSO = myIDSO;
        int IDTI = myID;
        int IDRS = myIDRO;
        int ITEMTP = myItemType;
        int ITEMID = myItemID;

        if (!IsPostBack)
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO && p.Draft);
            if (d == null) DataError("1");
            else
            {
                kw.Focus();
                fillForm(IDSO, ITEMTP, ITEMID);
                fillFilter();
                if (App.Refresh(this)) fillTable(d, sender, e);

                //Cancel-Link
                cancel.HRef = "TradeIn_Data.aspx";
                cancel.HRef += "?id=" + IDTI;
                cancel.HRef += "&so=" + IDSO;
                cancel.HRef += "&ro=" + IDRS;
            }

        }
    }

    //Exception
    protected void DataError(string err) { Response.Redirect("TradeIn.aspx?err=" + err); }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
        href += "&itemtype=" + myItemType;
        href += "&itemid=" + myItemID;
        href += "&kw=" + kw.Text;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(string NomorVoucher)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_VoucherStampsAddSave.aspx";
        href += "?id=" + myID;
        href += "&itemtype=" + myItemType;
        href += "&itemid=" + myItemID;
        href += "&NomorVoucher=" + NomorVoucher;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected void fillForm(int IDSO, int ITEMTP, int ITEMID)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO && p.Draft);
        if (d == null) DataError("2");
        else
        {
            string xpluimg = "";
            string xplutype = "";
            string xplunumber = "";
            string xpluitem = "";

            if (ITEMTP == 1)
            {
                xplutype = "Diamond Jewellery";

                var x = db.SalesOrderDJ.SingleOrDefault(p => p.ID == ITEMID);
                if (x != null)
                {
                    xpluimg = x.StockProductDJ.ImgPicture;
                    xplunumber = x.StockProductDJ.Nomor;
                    xpluitem = x.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                }

            }
            else if (ITEMTP == 2)
            {
                xplutype = "Plain Gold";

                var x = db.SalesOrderPG.SingleOrDefault(p => p.ID == ITEMID);
                if (x != null)
                {
                    xpluimg = x.StockProductPG.ImgPicture;
                    xplunumber = x.StockProductPG.Nomor;
                    xpluitem = x.StockProductPG.CharProductItem.Nama;
                }
            }
            else if (ITEMTP == 3)
            {
                xplutype = "Gold Jewellery";

                var x = db.SalesOrderGJ.SingleOrDefault(p => p.ID == ITEMID);
                if (x != null)
                {
                    xpluimg = x.StockProductGJ.ImgPicture;
                    xplunumber = x.StockProductGJ.Nomor;
                    xpluitem = x.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                }
            }
            else if (ITEMTP == 4)
            {
                xplutype = "Loose Diamond";

                var x = db.SalesOrderLD.SingleOrDefault(p => p.ID == ITEMID);
                if (x != null)
                {
                    xplunumber = x.StockProductLD_Stone1B.Nomor;
                    xpluitem = "Loose Diamond";
                }
            }

            lit_img.Text = xpluimg != "" ? "<img class='img-thumbnail' src=\"/Images.aspx?path=" + xpluimg + "\" style='max-height:60px;' />" : "";

            span_plu_type.InnerText = Str.CapUpper(xplutype);
            span_plu_number.InnerText = Str.CapUpper(xplunumber);
            span_plu_item.InnerText = Str.CapUpper(xpluitem);

        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
        }
    }
    protected void fillTable(SalesOrder d, object sender, EventArgs e)
    {
        string voucherTypeAllowed = "DISCOUNT"; // DISCOUNT or PAYMENT
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

            string query = "sp_validate_voucher";
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@ID", SqlDbType.Int){ Value = myIDSO },
                new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = keyword },
            };
            DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.StoredProcedure);
            bool success = Convert.ToBoolean(dt.Rows[0]["SUCCESS"]);
            string error = dt.Rows[0]["MESSAGE"].ToString();

            if (success)
            {
                errMsg = error;
            }
            else if (keyword.Length <= 20)//Tambahan by Andri Priyono
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
				catch (Exception ex) { errMsg = ex.Message.ToString();}//"Terjadi kendala teknis. Silahkan ulangi beberapa saat lagi. Jika masalah masih berlanjut silahkan hubungi bagian IT Support"; }
			}
            else
                errMsg = "Kode Voucher Lebih dari 20 digit. Voucher tidak valid.";
			
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

    /*protected bool IsUsed(SalesOrder d)
    {
        bool isused = false;
        isused = d.SalesVoucher.Any(p => p.NomorVoucher == kw.Text.Trim());

        if (!isused)
        {
            isused = db.SalesVoucher.Any(p => p.NomorVoucher == kw.Text.Trim()
            && (p.SalesOrder.StatusPembayaran == true || p.SalesOrder.StatusPembayaran == false)
            && p.SalesOrder.StatusVoid == false
			&& (p.SalesOrder.Draft == false || p.SalesOrder.Draft == true));
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