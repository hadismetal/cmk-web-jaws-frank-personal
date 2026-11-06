using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
        //checkSession();
        if (!IsPostBack)
        {
            //DLC
            var rs = from p in db.PaymentType orderby p.ID select p;
            ddlPaymentType.Items.Add(new ListItem("None Selected", "0"));
            foreach (var r in rs)
            {
                string t = r.Nama;
                string v = r.ID.ToString();
                ddlPaymentType.Items.Add(new ListItem(t, v));
            }

            ddlBank.Items.Add(new ListItem("None Selected", "0"));
            DLC.BankList(ddlBank); //lbLokasi

            //DLC.CompanyBrand(ddlCompanyBrand);
            ddlProgramCicilan.Items.Add(new ListItem("None Selected", "0"));
            DLC.ProgramList(ddlProgramCicilan);

            var rt = (from p in db.LocOutlet where p.Kode != null && p.Draft == false && p.Disable == false select new { p.ID, p.IDBrand, p.Nama, p.Kode }).Union
                     (from t in db.LocExhibition where t.Kode != null && t.Draft == false && t.Disable == false select new { t.ID, t.IDBrand, t.Nama, t.Kode });
            foreach (var r in rt)
            {
                ddlLokasi.Items.Add(new ListItem(r.Kode + "--" + r.Nama, r.IDBrand + "--" + r.Nama + "--" + r.Kode));
            }

            fillForm();
        }
    }
    private void fillForm()
    {
        string id = Request.QueryString["id"];
        if (!String.IsNullOrEmpty(id))
        {
            DataSet ds = TableSet(id);

            txtTitle.Value = ds.Tables["TableCouponPrizedraw"].Rows[0]["Title"].ToString();
            txtDateStart.Value = ds.Tables["TableCouponPrizedraw"].Rows[0]["DateStart"].ToString();
            txtDateEnd.Value = ds.Tables["TableCouponPrizedraw"].Rows[0]["DateEnd"].ToString();

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

        SqlDataAdapter adapter = new SqlDataAdapter("SELECT [ID], [Title], CONVERT(varchar,DateStart,101) [DateStart], CONVERT(varchar,DateEnd,101) [DateEnd], [Status], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate] FROM CouponPrizedraw WHERE ID = '" + id + "'", con);
        DataTable datatable1 = new DataTable("TableCouponPrizedraw");
        adapter.Fill(datatable1);
        adapter.Dispose();

        adapter = new SqlDataAdapter("SELECT A.ID, A.IDCouponPrizedraw, A.IDBrand, B.Nama NamaBrand, A.LocationCode, C.Nama NamaLokasi FROM " + "CouponPrizedrawLoc A LEFT JOIN CompanyBrand B ON A.IDBrand = B.ID " +
" LEFT JOIN(SELECT Kode, Nama FROM LocExhibition WHERE Kode IS NOT NULL AND Draft = 0 AND Disable = 0 " +
"UNION ALL SELECT Kode, Nama FROM LocOutlet WHERE Kode IS NOT NULL AND Draft = 0 AND Disable = 0) C " +
"ON A.LocationCode = C.Kode WHERE IDCouponPrizedraw = '" + id + "'", con);
        DataTable datatable2 = new DataTable("TableCouponPrizedrawLoc");
        adapter.Fill(datatable2);
        adapter.Dispose();

        adapter = new SqlDataAdapter("SELECT A.ID, A.IDCouponPrizedraw, A.IDPaymentType, B.Nama NamaPayment, A.IDBankIssuer, CASE WHEN A.IDBankIssuer = 0 THEN 'None Selected' ELSE C.Nama END NamaBank, A.IDProgramCicilan, D.Nama NamaProgramCicilan, A.Multiple, A.CouponReceived FROM CouponPrizedrawPayment A LEFT JOIN PaymentType B ON A.IDPaymentType = B.ID " +
            "LEFT JOIN BankIssuer C ON A.IDBankIssuer = C.ID " +
            "LEFT JOIN ProgramCicilan D ON A.IDProgramCicilan = D.ID " +
            "WHERE IDCouponPrizedraw = '" + id + "'", con);
        DataTable datatable3 = new DataTable("TableCouponPrizedrawPayment");
        adapter.Fill(datatable3);
        adapter.Dispose();

        con.Close();

        ds.Tables.Add(datatable1); ds.Tables.Add(datatable2); ds.Tables.Add(datatable3);

        return ds;
    }

    protected void UpdateTable(bool IsSubmitted)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                int StatusSave = (IsSubmitted) ? 1 : 0;
                int ID = int.Parse(Request.QueryString["id"]);
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "UPDATE CouponPrizedraw SET Title = @Title, DateStart = @DateStart, DateEnd = DATEADD(HH,23,DATEADD(MI,59,DATEADD(SS,59,@DateEnd))), [Status] = @Status, ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@DateStart", txtDateStart.Value);
                cmd.Parameters.AddWithValue("@DateEnd", txtDateEnd.Value);
                cmd.Parameters.AddWithValue("@Status", StatusSave);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Value);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }
        fillForm();
    }

    protected void gvLocations_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvLocations.EditIndex = e.NewEditIndex;

        //Select the NamaBrand and  in DropDownList.
        Label NamaBrand = (Label)gvLocations.Rows[gvLocations.EditIndex].FindControl("lblNamaBrand");
        Label NamaLokasi = (Label)gvLocations.Rows[gvLocations.EditIndex].FindControl("lblNamaLokasi");


        fillForm();

        DropDownList ddlNamaBrand = (DropDownList)gvLocations.Rows[gvLocations.EditIndex].FindControl("ddlNamaBrand");

        ddlNamaBrand.DataSource = GetData("SELECT ID IDBrand, Nama NamaBrand FROM CompanyBrand WHERE Disable = 0");
        ddlNamaBrand.DataTextField = "NamaBrand";
        ddlNamaBrand.DataValueField = "IDBrand";
        ddlNamaBrand.DataBind();

        DropDownList ddlNamaLokasi = (DropDownList)gvLocations.Rows[gvLocations.EditIndex].FindControl("ddlNamaLokasi");
        var rt = (from p in db.LocOutlet where p.Kode != null && p.Draft == false && p.Disable == false select new { p.ID, p.Nama, p.Kode }).Union
                 (from t in db.LocExhibition where t.Kode != null && t.Draft == false && t.Disable == false select new { t.ID, t.Nama, t.Kode });
        foreach (var r in rt)
        {
            ddlNamaLokasi.Items.Add(new ListItem(r.Nama, r.Kode));
        }

        //TextBox txtLocCode = (TextBox)gvLocations.Rows[gvLocations.EditIndex].FindControl("txtLocCode");

        ddlNamaBrand.Items.FindByText(NamaBrand.Text).Selected = true;
        ddlNamaLokasi.Items.FindByText(NamaLokasi.Text).Selected = true;
        //txtLocCode.Text = ddlNamaLokasi.SelectedItem.Value;

    }

    protected void gvLocations_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvLocations.EditIndex = -1;
        fillForm();
    }

    protected void gvLocations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int SubjectID = Convert.ToInt32(gvLocations.DataKeys[e.RowIndex].Value.ToString());
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "DELETE FROM CouponPrizedrawLoc WHERE ID=@ID";
                cmd.Parameters.AddWithValue("@ID", SubjectID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }

        gvLocations.EditIndex = -1;
        fillForm();
    }

    protected void gvLocations_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DropDownList ddlNamaBrand = (DropDownList)gvLocations.Rows[gvLocations.EditIndex].FindControl("ddlNamaBrand");
        DropDownList ddlNamaLokasi = (DropDownList)gvLocations.Rows[gvLocations.EditIndex].FindControl("ddlNamaLokasi");
        int SubjectID = Convert.ToInt32(gvLocations.DataKeys[e.RowIndex].Value.ToString());
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "UPDATE CouponPrizedrawLoc SET IDBrand = @IDBrand, LocationCode = @LocCode WHERE ID=@ID";
                cmd.Parameters.AddWithValue("@IDBrand", ddlNamaBrand.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@LocCode", ddlNamaLokasi.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@ID", SubjectID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }

        gvLocations.EditIndex = -1;
        fillForm();
    }


    private DataSet GetData(string query)
    {
        string conString = ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString;
        SqlCommand cmd = new SqlCommand(query);
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    return ds;
                }
            }
        }
    }

    protected void gvPayment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvPayment.EditIndex = e.NewEditIndex;

        Label NamaPayment = (Label)gvPayment.Rows[gvPayment.EditIndex].FindControl("lblPaymentType");
        Label NamaBank = (Label)gvPayment.Rows[gvPayment.EditIndex].FindControl("lblBankIssuer");
        Label NamaProgramCicilan = (Label)gvPayment.Rows[gvPayment.EditIndex].FindControl("lblProgramCicilan");

        fillForm();

        DropDownList ddlPaymentTypeEdit = (DropDownList)gvPayment.Rows[gvPayment.EditIndex].FindControl("ddlPaymentTypeEdit");
        var rs = from p in db.PaymentType orderby p.ID select p;
        //ddlPaymentTypeEdit.Items.Add(new ListItem("None Selected", "0"));
        foreach (var r in rs)
        {
            string t = r.Nama;
            string v = r.ID.ToString();
            ddlPaymentTypeEdit.Items.Add(new ListItem(t, v));
        }

        DropDownList ddlBankIssuer = (DropDownList)gvPayment.Rows[gvPayment.EditIndex].FindControl("ddlBankIssuer");
        ddlBankIssuer.Items.Add(new ListItem("None Selected", "0"));
        DLC.BankList(ddlBankIssuer);

        DropDownList ddlProgramCicilanEdit = (DropDownList)gvPayment.Rows[gvPayment.EditIndex].FindControl("ddlProgramCicilanEdit");
        //ddlProgramCicilanEdit.Items.Add(new ListItem("None Selected", "0"));
        DLC.ProgramList(ddlProgramCicilanEdit);

        ddlPaymentTypeEdit.Items.FindByText(NamaPayment.Text).Selected = true;
        ddlBankIssuer.Items.FindByText(NamaBank.Text).Selected = true;
        ddlProgramCicilanEdit.Items.FindByText(NamaProgramCicilan.Text).Selected = true;
    }

    protected void gvPayment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvPayment.EditIndex = -1;
        fillForm();
    }

    protected void gvPayment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int SubjectID = Convert.ToInt32(gvPayment.DataKeys[e.RowIndex].Value.ToString());
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "DELETE FROM CouponPrizedrawPayment WHERE ID=@ID";
                cmd.Parameters.AddWithValue("@ID", SubjectID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }
        gvPayment.EditIndex = -1;
        fillForm();
    }

    protected void gvPayment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int SubjectID = Convert.ToInt32(gvPayment.DataKeys[e.RowIndex].Value.ToString());
        DropDownList ddlPaymentTypeEdit = (DropDownList)gvPayment.Rows[e.RowIndex].FindControl("ddlPaymentTypeEdit");
        DropDownList ddlBankIssuer = (DropDownList)gvPayment.Rows[e.RowIndex].FindControl("ddlBankIssuer");
        DropDownList ddlProgramCicilanEdit = (DropDownList)gvPayment.Rows[gvPayment.EditIndex].FindControl("ddlProgramCicilanEdit");
        TextBox txtMultiple = (TextBox)gvPayment.Rows[e.RowIndex].FindControl("txtMultiple");
        TextBox txtCouponReceived = (TextBox)gvPayment.Rows[e.RowIndex].FindControl("txtCouponReceived");
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "UPDATE CouponPrizedrawPayment SET IDPaymentType = @IDPaymentType, IDBankIssuer = @IDBankIssuer ," +
                    "IDProgramCicilan = @IDProgramCicilan , Multiple = @Multiple, CouponReceived = @CouponReceived WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@IDPaymentType", ddlPaymentTypeEdit.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@IDBankIssuer", ddlBankIssuer.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@IDProgramCicilan", ddlProgramCicilanEdit.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Multiple", Convert.ToDecimal(txtMultiple.Text));
                cmd.Parameters.AddWithValue("@CouponReceived", txtCouponReceived.Text);
                cmd.Parameters.AddWithValue("@ID", SubjectID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }

        gvPayment.EditIndex = -1;
        fillForm();
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        bool Validation = true; string Field = "";
        if (gvLocations.Rows.Count.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Locations,";
        }
        if (gvPayment.Rows.Count.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Payments,";
        }
        if (String.IsNullOrEmpty(txtTitle.Value))
        {
            Validation = false;
            Field = Field + "Title,";
        }
        if (String.IsNullOrEmpty(txtDateStart.Value))
        {
            Validation = false;
            Field = Field + "Date Start,";
        }
        if (String.IsNullOrEmpty(txtDateEnd.Value))
        {
            Validation = false;
            Field = Field + "Date End,";
        }
        if (Validation == true)
        {
            UpdateTable(false);
            callout_script.Visible = true;
        }
        else
        {
            Field = Field.Remove(Field.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Save Failed. " + Field + " Must be Filled!!!');", true);
            hfcountlocationtable.Value = "0";
            hfcountpaymenttable.Value = "0";
        }
    }

    protected void btnSubmit_ServerClick(object sender, EventArgs e)
    {
        bool Validation = true; string Field = "";
        if (gvLocations.Rows.Count.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Locations,";
        }
        if (gvPayment.Rows.Count.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Payments,";
        }
        if (String.IsNullOrEmpty(txtTitle.Value))
        {
            Validation = false;
            Field = Field + "Title,";
        }
        if (String.IsNullOrEmpty(txtDateStart.Value))
        {
            Validation = false;
            Field = Field + "Date Start,";
        }
        if (String.IsNullOrEmpty(txtDateEnd.Value))
        {
            Validation = false;
            Field = Field + "Date End,";
        }
        if (Validation == true)
        {
            UpdateTable(true);
            callout_script.Visible = true;
        }
        else
        {
            Field = Field.Remove(Field.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Save Failed. " + Field + " Must be Filled!!!');", true);
            hfcountlocationtable.Value = "0";
            hfcountpaymenttable.Value = "0";
        }
    }

    protected void btnAddLoc_ServerClick(object sender, EventArgs e)
    {
        bool Validation = true; string Field = "";
        //if (String.IsNullOrEmpty(ddlLokasi.SelectedItem.Text))
        //{
        //    Validation = false;
        //    Field = Field + "Company Brand,";
        //}
        if (ddlLokasi.SelectedValue == null)
        {
            Validation = false;
            Field = Field + "Location,";
        }
        if (Validation == true)
        {
            AddLocation();
            callout_script.Visible = true;
        }
        else
        {
            Field = Field.Remove(Field.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Add Failed. " + Field + " Must be Filled!!!');", true);
        }
    }

    protected void btnAddPayment_ServerClick(object sender, EventArgs e)
    {
        bool Validation = true; string Field = "";
        if (String.IsNullOrEmpty(ddlPaymentType.SelectedItem.Text))
        {
            Validation = false;
            Field = Field + "Payment Type,";
        }
        if (String.IsNullOrEmpty(ddlBank.SelectedItem.Text))
        {
            Validation = false;
            Field = Field + "Bank,";
        }
        if (String.IsNullOrEmpty(txtMultiple.Value))
        {
            Validation = false;
            Field = Field + "Multiple,";
        }
        if (String.IsNullOrEmpty(txtCouponReceived.Value))
        {
            Validation = false;
            Field = Field + "Coupon Received,";
        }
        if (Validation == true)
        {
            AddPayment();
            callout_script.Visible = true;
        }
        else
        {
            Field = Field.Remove(Field.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Add Failed. " + Field + " Must be Filled!!!');", true);
        }
    }
    private void AddLocation()
    {
        string[] arrayList = Regex.Split(ddlLokasi.SelectedItem.Value.Trim(),"--");
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("",con,tr);
                cmd.CommandText = "INSERT INTO CouponPrizedrawLoc(IDCouponPrizedraw, IDBrand, LocationCode) VALUES(@IDCouponPrizedraw, @IDBrand,@LocCode);";
                cmd.Parameters.AddWithValue("@IDCouponPrizedraw",Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@IDBrand", arrayList[0].ToString());
                cmd.Parameters.AddWithValue("@LocCode", arrayList[2].ToString());
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }
        fillForm();
    }
    private void AddPayment()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "INSERT INTO CouponPrizedrawPayment(IDCouponPrizedraw, IDPaymentType, IDBankIssuer, IDProgramCicilan, [Multiple], CouponReceived) VALUES(@IDCouponPrizedraw, @IDPaymentType,@IDBankIssuer,@IDProgramCicilan,@Multiple,@CouponReceived);";
                cmd.Parameters.AddWithValue("@IDCouponPrizedraw", Request.QueryString["id"]);
                cmd.Parameters.AddWithValue("@IDPaymentType", ddlPaymentType.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@IDBankIssuer", ddlBank.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@IDProgramCicilan", ddlProgramCicilan.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Multiple", txtMultiple.Value);
                cmd.Parameters.AddWithValue("@CouponReceived", txtCouponReceived.Value);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                cmd.CommandText = "UPDATE CouponPrizedraw SET ModifiedDate = GETDATE(), ModifiedBy = 'ADMIN' WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Request.QueryString["id"]);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                tr.Commit();
                con.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                tr.Rollback();
                con.Close();
            }
        }
        fillForm();
    }
}
