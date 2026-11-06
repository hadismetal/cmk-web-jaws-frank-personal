using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Report_QCPG : System.Web.UI.Page
{
    protected DateTime TglCutOff = Convert.ToDateTime("2026-03-05");
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }

    }

    protected void fillTable(object sender, EventArgs e)
    {
        // Base SQL query
        string query = @"
        SELECT A.ID, A.Nomor, A.QCNama, A.OperatorTgl, A.ApprovalNama, A.ApprovalTgl , A.RejectNama, A.RejectTgl,  A.RejectKeterangan,
               B.Nomor AS StockNomor
        FROM DocQCPG A
        JOIN StockProductPG B ON A.IDProduct = B.ID
        WHERE 1=1"; // 1=1 makes appending conditions easier

        List<SqlParameter> parameters = new List<SqlParameter>();

        // Filter by keyword
        string keyword = kw.Text.Trim();
        if (!string.IsNullOrEmpty(keyword))
        {
            query += " AND (B.Nomor = @Keyword OR A.Nomor = @Keyword)";
            parameters.Add(new SqlParameter("@Keyword", SqlDbType.NVarChar) { Value = keyword });
        }

        // Filter by date range
        if (!string.IsNullOrEmpty(dari.Text) && !string.IsNullOrEmpty(sampai.Text))
        {
            query += " AND A.OperatorTgl >= @Dari AND A.OperatorTgl < @Sampai";
            parameters.Add(new SqlParameter("@Dari", SqlDbType.DateTime) { Value = Convert.ToDateTime(dari.Text) });
            parameters.Add(new SqlParameter("@Sampai", SqlDbType.DateTime) { Value = Convert.ToDateTime(sampai.Text).AddDays(1) });
        }

        // Order by OperatorTgl descending
        query += " ORDER BY A.OperatorTgl DESC";

        // Execute query
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);

        int index = 1;
        foreach (DataRow r in dt.Rows)
        {
            bool IsApproved = r["ApprovalNama"] != DBNull.Value;
            string ApprovalNama = r["ApprovalNama"] != DBNull.Value ? r["ApprovalNama"].ToString() : string.Empty;
            string TglApproved = r["ApprovalTgl"] != DBNull.Value ? Convert.ToDateTime(r["ApprovalTgl"]).ToString("dd-MM-yyyy") : string.Empty;
            bool IsRejected = r["RejectNama"] != DBNull.Value;
            string RejectNama = r["RejectNama"] != DBNull.Value ? r["RejectNama"].ToString() : string.Empty;
            string TglRejected = r["RejectTgl"] != DBNull.Value ? Convert.ToDateTime(r["RejectTgl"]).ToString("dd-MM-yyyy") : string.Empty;

            TableRow tr = new TableRow();
            TableCell td;

            // Hyperlink
            td = new TableCell();
            td.Text = TD_HyperLink(Convert.ToInt32(r["ID"]), IsApproved, IsRejected, Convert.ToDateTime(r["OperatorTgl"]));
            tr.Cells.Add(td);

            // Index
            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            // Nomor
            td = new TableCell();
            td.Text = r["Nomor"].ToString();
            tr.Cells.Add(td);

            // StockProductPG Nomor
            td = new TableCell();
            td.Text = r["StockNomor"].ToString();
            tr.Cells.Add(td);

            // OperatorTgl
            td = new TableCell();
            td.Text = Str.TglJam(Convert.ToDateTime(r["OperatorTgl"]));
            tr.Cells.Add(td);

            // QCNama
            td = new TableCell();
            td.Text = r["QCNama"].ToString();
            tr.Cells.Add(td);

            string Status = IsRejected
            ? "<label class=\"text-danger\">Rejected</label><br /><small class=\"text-muted\">" + RejectNama + " (" + TglRejected + ")</small>"
            : IsApproved
                ? "<label class=\"text-success\">Approved</label><br /><small class=\"text-muted\">" + ApprovalNama + " (" + TglApproved + ")</small>"
                : "<label class=\"text-muted\">Not Yet Approved</label>";
            td = new TableCell();
            td.Text = Status;
            tr.Cells.Add(td);

            td = new TableCell();
            if (IsRejected)
            {
                td.Text = r["RejectKeterangan"].ToString();
            }
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
            index++;
        }
    }

    protected string TD_HyperLink(int IDDoc, bool IsApproved, bool IsRejected, DateTime TglQC)
    {
        string x = string.Empty;

        if (TglCutOff <= TglQC)
        {
            if (!IsRejected)
            {
                if (IsApproved)
                {
                    x += "<div class=\"btn-group\">";
                    x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCPG_Detail.aspx?id=" + IDDoc + "&isprint=1\">";
                    x += "      Print";
                    x += "      ";
                    x += "  </a>";
                    x += "</div>&nbsp;";
                }
                x += "<div class=\"btn-group\">";
                x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCPG_Detail.aspx?id=" + IDDoc + "&isprint=0\">";
                x += "      Detail";
                x += "      ";
                x += "  </a>";
                x += "</div>";
            }
        }
        else
        {
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCPG_Detail.aspx?id=" + IDDoc + "&isprint=1\">";
            x += "      Print";
            x += "      ";
            x += "  </a>";
            x += "</div>&nbsp;";
            x += "<div class=\"btn-group\">";
            x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCPG_Detail.aspx?id=" + IDDoc + "&isprint=0\">";
            x += "      Detail";
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
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }
}