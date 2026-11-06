using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page
{
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //Generate
            fillFilter();
            if (App.Refresh(this))
                fillTable();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Payment.aspx"); }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        //Page-Qstr
        href += "&id=" + myID;
        href += "&kw=" + kw.Text;
        href += "&coc=" + inputoldcustomercode.Text;
        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            inputoldcustomercode.Text = Request.QueryString["coc"];
        }
    }

    protected void fillTable()
    {
        using (Map_DB db = new Map_DB())
        {
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft);
            if (d == null) DataError();
            else
            {
                //Header
                header_nomor.InnerHtml = d.Nomor;
                //Cancel-Link
                cancel.HRef += "?id=" + myID;

                string query = "sp_get_dp_active_by_customer";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@IDCUSTOMER", SqlDbType.Int){ Value = d.IDCustomer},
                    new SqlParameter("@KODE_CUSTOMER_LAMA", SqlDbType.VarChar){ Value = inputoldcustomercode.Text},
                    new SqlParameter("@KEYWORDS", SqlDbType.VarChar){ Value = kw.Text}
                };
                DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);

                int index = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    int totaldays = 0;
                    string lamadp = "";
                    bool morethanthreemonth = false;
                    DateTime tgl = Convert.ToDateTime(dr["Tgl"]);
                    totaldays = Convert.ToInt32((DateTime.Now - tgl).TotalDays);
                    int sisabulan = 0;
                    int sisahari = 0;
                    sisabulan = totaldays / 30;
                    sisahari = totaldays % 30;
                    lamadp = sisabulan.ToString() + " bulan " + sisahari.ToString() + " hari";

                    morethanthreemonth = (totaldays < 90) ? false : true;

                    td = new TableCell();
                    td.Text = TD_HyperLink(Convert.ToInt32(dr["ID"]), morethanthreemonth);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["NomorDP"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(tgl);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["TotalBayar"]).ToString("N0");
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["OperatorNama"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["NamaLokasi"].ToString();
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = lamadp;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (!morethanthreemonth) ? dr["Keterangan"].ToString() : dr["Keterangan"].ToString() + " <p style='color:red'>(KONFIRMASI AKUNTING KARENA SUDAH LEBIH DARI 3 BULAN TIDAK DIGUNAKAN)</p>";
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
    }

    //Display Column
    protected string TD_HyperLink(int ID, bool ismorethan3months = false)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Payment_DPAddSave.aspx";
        href += "?id=" + myID;
        href += "&iditem=" + ID;

        if (ismorethan3months == false)
        {
            x += "<a class=\"btn btn-info\" href=\"" + href + "\" onclick=\"showOverlay();\" >";
            x += "  <i class=\"icon-ok\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else
        {
            x += "<a class=\"btn btn-warning\" href=\"" + href + "\" onclick=\"showOverlay();\" >";
            x += "  <i class=\"icon-ok\"></i> ";
            x += "  ";
            x += "</a>";
        }

        return x;
    }

    //Data Table Builder
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