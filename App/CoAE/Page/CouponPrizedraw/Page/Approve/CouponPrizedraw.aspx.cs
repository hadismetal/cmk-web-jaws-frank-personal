using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class WS_CouponPrizedraw : System.Web.UI.Page
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
        fillForm();
    }
    private void fillForm()
    {
        string id = Request.QueryString["id"];
        if (!String.IsNullOrEmpty(id))
        {
            DataSet ds = TableSet(id);

            lblTitle.InnerText = ds.Tables["TableCouponPrizedraw"].Rows[0]["Title"].ToString();
            lblDateStart.InnerText = ds.Tables["TableCouponPrizedraw"].Rows[0]["DateStart"].ToString();
            lblDateEnd.InnerText = ds.Tables["TableCouponPrizedraw"].Rows[0]["DateEnd"].ToString();

            gvLocations.DataSource = ds.Tables["TableCouponPrizedrawLoc"];
            gvLocations.DataBind();

            gvPayment.DataSource = ds.Tables["TableCouponPrizedrawPayment"];
            gvPayment.DataBind();
        }
    }
    private DataSet TableSet(string id)
    {
        DataSet ds = new DataSet();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString());
        con.Open();

        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CouponPrizedraw WHERE ID = '" + id + "'", con);
        DataTable datatable1 = new DataTable("TableCouponPrizedraw");
        adapter.Fill(datatable1);

        adapter = new SqlDataAdapter("SELECT A.ID, A.IDCouponPrizedraw, A.IDBrand, B.Nama NamaBrand, A.LocationCode, C.Nama NamaLokasi FROM " + "CouponPrizedrawLoc A LEFT JOIN CompanyBrand B ON A.IDBrand = B.ID " +
"LEFT JOIN(SELECT Kode, Nama FROM LocExhibition WHERE Kode IS NOT NULL AND Draft = 0 AND Disable = 0 " +
"UNION ALL SELECT Kode, Nama FROM LocOutlet WHERE Kode IS NOT NULL AND Draft = 0 AND Disable = 0) C " +
"ON A.LocationCode = C.Kode WHERE IDCouponPrizedraw = '" + id + "'", con);
        DataTable datatable2 = new DataTable("TableCouponPrizedrawLoc");
        adapter.Fill(datatable2);

        adapter = new SqlDataAdapter("SELECT A.ID, A.IDCouponPrizedraw, A.IDPaymentType, B.Nama NamaPayment, A.IDBankIssuer, C.Nama NamaBank, A.IDProgramCicilan, D.Nama ProgramCicilan, A.Multiple, A.CouponReceived FROM CouponPrizedrawPayment A LEFT JOIN PaymentType B ON A.IDPaymentType = B.ID " +
            "LEFT JOIN BankIssuer C ON A.IDBankIssuer = C.ID " +
            "LEFT JOIN ProgramCicilan D ON A.IDProgramCicilan = D.ID " +
            "WHERE IDCouponPrizedraw = '" + id + "'", con);
        DataTable datatable3 = new DataTable("TableCouponPrizedrawPayment");
        adapter.Fill(datatable3);

        con.Close();

        ds.Tables.Add(datatable1); ds.Tables.Add(datatable2); ds.Tables.Add(datatable3);

        return ds;
    }

    protected void btnApprove_ServerClick(object sender, EventArgs e)
    {
        int ID = int.Parse(Request.QueryString["id"]);
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("",con,tr);
                cmd.CommandText = "UPDATE CouponPrizedraw SET [Status] = '2' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID",ID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                tr.Commit();
                con.Close();
                callout_script.Visible = true;
            }
            catch (Exception ex)
            {
                tr.Rollback();
                con.Close();
            }
        }
    }


    protected void btnReject_ServerClick(object sender, EventArgs e)
    {
        int ID = int.Parse(Request.QueryString["id"]);
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "UPDATE CouponPrizedraw SET [Status] = '3' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                tr.Commit();
                con.Close();
                callout_script.Visible = true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }
    }
}