using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Page_Report_QCLD : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            if (App.Refresh(this))
			{
				fillFilter();
				fillTable(sender, e);
			}
        }
    }
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
		string mode = "PROC";
		string producttype = "LD";
        string keyword = kw.Text;
        string[] dari1 = dari.Text.Split('-');
        string[] sampai1 = sampai.Text.Split('-');
        string start = dari1[2] + "-" + dari1[1] + "-" + dari1[0] + " 00:00:00.000";
        string end = sampai1[2] + "-" + sampai1[1] + "-" + sampai1[0] + " 23:59:00.000";

        string query = "sp_get_qc_new";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@MODE",SqlDbType.VarChar){ Value = mode },
            new SqlParameter("@TIPE", SqlDbType.VarChar){ Value = producttype },
            new SqlParameter("@KEYWORD", SqlDbType.VarChar){ Value = keyword },
            new SqlParameter("@START_DATE", SqlDbType.DateTime){ Value = start },
            new SqlParameter("@END_DATE", SqlDbType.DateTime){ Value = end },
            new SqlParameter("@STATUS", SqlDbType.Int){ Value = 0 },
            new SqlParameter("@PAGE", SqlDbType.Int){ Value = 1 },
            new SqlParameter("@ROWS", SqlDbType.Int){ Value = 20 },
            new SqlParameter("@EXCEL", SqlDbType.Int){ Value = 1 }
        };
        DataTable dt = DbAccess.Rs(query, Db.Cnn_DB, parameters);

        int rowcount = dt.Rows.Count;
        int index = 1;
        if (rowcount == 0)
        {
			TableRow tr = new TableRow();
            TableCell td = new TableCell();
			td.ColumnSpan = 11;
			td.Text = "No Data";
			tb.Rows.Add(tr);
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                DateTime? tglapprove = dr["ApprovalProcurementTgl"] != DBNull.Value ? Convert.ToDateTime(dr["ApprovalProcurementTgl"]) : (DateTime?)null;
                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(dr["ID"]), tglapprove);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["PLU"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["CustomerNama"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDateTime(dr["OperatorTgl"]).ToString("dd MMMM yyyy");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["OperatorNama"].ToString();
                tr.Cells.Add(td);

                string approvalSM = dr["ApprovalSM"].ToString();
                string approvalSMTgl = (dr["ApprovalSMTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalSMTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalSM + "<br/>" + approvalSMTgl;
                tr.Cells.Add(td);

                string approvalAM = dr["ApprovalAM"].ToString();
                string approvalAMTgl = (dr["ApprovalAMTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalAMTgl"]).ToString("dd-MM-yyyy hh:mm:ss");

                string approveAMFinal = string.Empty;
                if (approvalSM != "")
                {
                    approveAMFinal = ((approvalAM == "") ? "Approve By SM" : approvalAM) + "<br/>" + approvalAMTgl;
                }
                else
                {
                    approveAMFinal = approvalAM + "<br/>" + approvalAMTgl;
                }
                td = new TableCell();
                td.Text = approveAMFinal;
                tr.Cells.Add(td);

                string approvalProc = (dr["ApprovalProcurement"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalProcurement"].ToString();
                string approvalProcTgl = (dr["ApprovalProcurementTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalProcurementTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalProc + "<br/>" + approvalProcTgl;
                tr.Cells.Add(td);

                string approvalFinalProc = (dr["ApprovalProcurementFinal"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalProcurementFinal"].ToString();
                string approvalFinalProcTgl = (dr["ApprovalProcurementFinalTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalProcurementFinalTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalFinalProc + "<br/>" + approvalFinalProcTgl;
                tr.Cells.Add(td);

                string approvalFinal = (dr["ApprovalFinal"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalFinal"].ToString();
                string approvalFinalTgl = (dr["ApprovalFinalTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalFinalTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalFinal + "<br/>" + approvalFinalTgl;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected string TD_HyperLink(int IDDoc, DateTime? TglApproveProc)
    {
        string x = string.Empty;
        string query = "SELECT * FROM DataProcurement WHERE NIK = @NIK";
        string nik = string.Empty;
        string jabatan = string.Empty;
        using (Map_AC ac = new Map_AC())
        {
            var user = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
            if(user != null)
            {
                nik = user.UserNumber;
                jabatan = user.Keterangan;
            }
        }

        DataTable dt = new DataTable();
        if (nik != "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@NIK", SqlDbType.VarChar, 50){ Value = nik }
            };

            dt = Rs(query,Db.Cnn_DB,parameters);
        }

        int rowcount = dt.Rows.Count;

        if (rowcount == 0)
        {
            if (TglApproveProc.HasValue)
            {
                TimeSpan ts = DateTime.Now.Date.Subtract(TglApproveProc.Value.Date);
                if (ts.Days <= 7)
                {
                    x += "<div class=\"btn-group\">";
                    x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCLD_Detail.aspx?id=" + IDDoc + "\">";
                    x += "      <i class=\"icon-search\"></i> ";
                    x += "      ";
                    x += "  </a>";
                    x += "</div>";
                }
                else
                {
                    x += "<label class='txt-danger'>QC Expired</label>";
                }
            }
            else
            {
                x += "<div class=\"btn-group\">";
                x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCLD_Detail.aspx?id=" + IDDoc + "\">";
                x += "      <i class=\"icon-search\"></i> ";
                x += "      ";
                x += "  </a>";
                x += "</div>";

            }
        }
        else
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCLD_Detail.aspx?id=" + IDDoc + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      ";
            x += "  </a>";
            x += "</div>";
        }
        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = CommandType.Text;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
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

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }

    protected void exportExcel(object sender, EventArgs e)
    {
        string mode = "PROC";
        string producttype = "LD";
        string keyword = kw.Text;
        string[] dari1 = dari.Text.Split('-');
        string[] sampai1 = sampai.Text.Split('-');
        string start = dari1[2] + "-" + dari1[1] + "-" + dari1[0] + " 00:00:00.000";
        string end = sampai1[2] + "-" + sampai1[1] + "-" + sampai1[0] + " 23:59:00.000";

        string query = "sp_get_qc_new";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@MODE",SqlDbType.VarChar){ Value = mode },
            new SqlParameter("@TIPE", SqlDbType.VarChar){ Value = producttype },
            new SqlParameter("@KEYWORD", SqlDbType.VarChar){ Value = keyword },
            new SqlParameter("@START_DATE", SqlDbType.DateTime){ Value = start },
            new SqlParameter("@END_DATE", SqlDbType.DateTime){ Value = end }
        };
        DataTable dt = DbAccess.Rs(query, Db.Cnn_DB, parameters);

        int rowcount = dt.Rows.Count;
        int index = 1;
        if (rowcount == 0)
        {
            TableRow tr = new TableRow();
            TableCell td = new TableCell();
            td.ColumnSpan = 11;
            td.Text = "No Data";
            tb.Rows.Add(tr);
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                DateTime? tglapprove = dr["ApprovalProcurementTgl"] != DBNull.Value ? Convert.ToDateTime(dr["ApprovalProcurementTgl"]) : (DateTime?)null;
                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(dr["ID"]), tglapprove);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["PLU"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["CustomerNama"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDateTime(dr["OperatorTgl"]).ToString("dd MMMM yyyy");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["OperatorNama"].ToString();
                tr.Cells.Add(td);

                string approvalSM = dr["ApprovalSM"].ToString();
                string approvalSMTgl = (dr["ApprovalSMTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalSMTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalSM + "<br/>" + approvalSMTgl;
                tr.Cells.Add(td);

                string approvalAM = dr["ApprovalAM"].ToString();
                string approvalAMTgl = (dr["ApprovalAMTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalAMTgl"]).ToString("dd-MM-yyyy hh:mm:ss");

                string approveAMFinal = string.Empty;
                if (approvalSM != "")
                {
                    approveAMFinal = ((approvalAM == "") ? "Approve By SM" : approvalAM) + "<br/>" + approvalAMTgl;
                }
                else
                {
                    approveAMFinal = approvalAM + "<br/>" + approvalAMTgl;
                }
                td = new TableCell();
                td.Text = approveAMFinal;
                tr.Cells.Add(td);

                string approvalProc = (dr["ApprovalProcurement"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalProcurement"].ToString();
                string approvalProcTgl = (dr["ApprovalProcurementTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalProcurementTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalProc + "<br/>" + approvalProcTgl;
                tr.Cells.Add(td);

                string approvalFinalProc = (dr["ApprovalProcurementFinal"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalProcurementFinal"].ToString();
                string approvalFinalProcTgl = (dr["ApprovalProcurementFinalTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalProcurementFinalTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalFinalProc + "<br/>" + approvalFinalProcTgl;
                tr.Cells.Add(td);

                string approvalFinal = (dr["ApprovalFinal"].ToString() == "") ? "<label style='background-color:orange'>Waiting Approval</label>" : dr["ApprovalFinal"].ToString();
                string approvalFinalTgl = (dr["ApprovalFinalTgl"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["ApprovalFinalTgl"]).ToString("dd-MM-yyyy hh:mm:ss");
                td = new TableCell();
                td.Text = approvalFinal + "<br/>" + approvalFinalTgl;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment; filename=QCLD_Export.xls");
        Response.Charset = "";

        System.IO.StringWriter sw = new System.IO.StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        tb.RenderControl(hw);
        Response.Write(sw.ToString());
        Response.End();
    }
}