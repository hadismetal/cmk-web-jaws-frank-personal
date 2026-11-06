using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_StatusLog_TGP_V2 : System.Web.UI.Page
{
    protected int myID { get { return App.QueryInt(this); } }

    //Parameter Periode
    protected int GetBulan { get { return Convert.ToInt32(bulan.SelectedValue); } }
    protected int GetTahun { get { return Convert.ToInt32(tahun.SelectedValue); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            fillForm();

            //DLC
            DateDrv.ListBulan(bulan);
            DateDrv.ListTahun(tahun);

            //Generate
            fillFilter();
            fillTable(sender, e);
        }
    }
    protected void fillForm()
    {
        string query = "SELECT Nama FROM PricingTableTGP WHERE ID = @ID";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        if (dt.Rows.Count != 1) App.DataNull(this);
        else
        {
            //Header
            header_nama.InnerHtml = dt.Rows[0]["Nama"].ToString();
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&bulan=" + bulan.SelectedIndex;
        href += "&tahun=" + tahun.SelectedIndex;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            bulan.SelectedIndex = Convert.ToInt32(Request.QueryString["bulan"]);
            tahun.SelectedIndex = Convert.ToInt32(Request.QueryString["tahun"]);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        /*var rs = from p in db.PricingLogTG
                 where
                    p.IDMaster == myID &&
                    p.Tgl.Month == GetBulan &&
                    p.Tgl.Year == GetTahun // && p.IDMaster < 5
                 select p;*/

        string query = "SELECT * FROM PricingLogTGP WHERE IDMaster = @IDMaster AND MONTH(Tgl) = @Month AND YEAR(Tgl) = @Year";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@IDMaster", SqlDbType.Int){ Value = myID },
            new SqlParameter("@Month", SqlDbType.Int){ Value = GetBulan },
            new SqlParameter("@Year", SqlDbType.Int){ Value = GetTahun }
        };
        DataTable rs = Rs(query,Db.Cnn_DB, parameters, CommandType.Text);

        //NO-RESULT
        if (rs.Rows.Count == 0) App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (DataRow r in rs.Rows)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(Convert.ToDateTime(r["Tgl"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Jam(Convert.ToDateTime(r["Tgl"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Approval"].ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(Convert.ToDecimal(r["HargaJualLama"]));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(Convert.ToDecimal(r["HargaJualBaru"]));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(Convert.ToDecimal(r["HargaBeliLama"]));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(Convert.ToDecimal(r["HargaBeliBaru"]));
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
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
                cmd.CommandTimeout = 300;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader); // Not streaming, but async-safe here
                }
            }
        }
        return dt;
    }
}