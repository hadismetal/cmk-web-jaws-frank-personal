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
        if (!IsPostBack)
        {
            //DLC
            var rs = from p in db.PaymentType orderby p.ID select p;
            //ddlPaymentType.Items.Add(new ListItem("None Selected", "0"));
            foreach (var r in rs)
            {
                string t = r.Nama;
                string v = r.ID.ToString();
                ddlPaymentType.Items.Add(new ListItem(t, v));
            }

            ddlBank.Items.Add(new ListItem("None Selected", "0"));
            DLC.BankList(ddlBank);

            //DLC.CompanyBrand(ddlCompanyBrand);
            //ddlProgramCicilan.Items.Add(new ListItem("None Selected", "0"));
            DLC.ProgramList(ddlProgramCicilan);

            //var rsa = from p in db.CompanyBrand orderby p.SN select p;
            //foreach (var r in rsa)
            //{
            //    string t = r.Nama + " (" + r.NamaKode + ")";
            //    string v = r.ID.ToString();
            //    ddlCompanyBrand.Items.Add(new ListItem(t, v));
            //}

            var rt = (from p in db.LocOutlet where p.Kode != null && p.Draft == false && p.Disable == false select new { p.ID, p.IDBrand , p.Nama, p.Kode }).Union
                     (from t in db.LocExhibition where t.Kode != null && t.Draft == false && t.Disable == false select new { t.ID, t.IDBrand , t.Nama, t.Kode });
            ddlLokasi.Items.Add(new ListItem("None Selected","0--None Selected--0"));
            foreach (var r in rt)
            {
                ddlLokasi.Items.Add(new ListItem(r.Kode + "--" + r.Nama, r.IDBrand + "--" + r.Nama + "--" + r.Kode));
            }
            //checkSession();}
            ddlLokasi.SelectedIndex = 0; ddlBank.SelectedIndex = 0; ddlPaymentType.SelectedIndex = 0; ddlProgramCicilan.SelectedIndex = 0;
        }
    }
    protected void CreateNew(bool IsSubmitted)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                int StatusSave = (IsSubmitted) ? 1 : 0;
                int ID = GetRunningNumber();
                SqlCommand cmd = new SqlCommand("", con, tr);
                cmd.CommandText = "INSERT INTO CouponPrizedraw(ID,DateStart,DateEnd,[Status],[Title],CreatedDate,CreatedBy) VALUES (@ID,@DateStart,DATEADD(HH,23,DATEADD(MI,59,DATEADD(SS,59,@DateEnd))),@Status,@Title,GetDate(),'ADMIN');";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@DateStart", txtDateStart.Value);
                cmd.Parameters.AddWithValue("@DateEnd", txtDateEnd.Value);
                cmd.Parameters.AddWithValue("@Status", StatusSave);
                cmd.Parameters.AddWithValue("@Title", txtTitle.Value);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                if (int.Parse(hfcountlocationtable.Value) > 0)
                {
                    string JsonTable = Request.Form["hfjsonlocationtable"];
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonTable);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO CouponPrizedrawLoc(IDCouponPrizedraw,IDBrand,LocationCode) VALUES (@IDCouponPrizedraw, @IDBrand, @LocationCode);";
                        cmd.Parameters.AddWithValue("@IDCouponPrizedraw", ID);
                        cmd.Parameters.AddWithValue("@IDBrand", dt.Rows[i]["IDBrand"]);
                        cmd.Parameters.AddWithValue("@LocationCode", dt.Rows[i]["LocCode"]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }

                if (int.Parse(hfcountpaymenttable.Value) > 0)
                {
                    string JsonTable = Request.Form["hfjsonpaymenttable"];
                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonTable);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd.CommandText = "INSERT INTO CouponPrizedrawPayment(IDCouponPrizedraw, IDPaymentType, IDBankIssuer, IDProgramCicilan , Multiple, CouponReceived) VALUES(@IDCouponPrizedraw,@IDPaymentType,@IDBankIssuer,@IDProgramCicilan,@Multiple,@CouponReceived)";
                        cmd.Parameters.AddWithValue("@IDCouponPrizedraw", ID);
                        cmd.Parameters.AddWithValue("@IDPaymentType", dt.Rows[i]["paymentId"]);
                        cmd.Parameters.AddWithValue("@IDBankIssuer", dt.Rows[i]["bankIssuerId"]);
                        cmd.Parameters.AddWithValue("@IDProgramCicilan", dt.Rows[i]["programCicilanId"]);
                        cmd.Parameters.AddWithValue("@Multiple", dt.Rows[i]["multiple"]);
                        cmd.Parameters.AddWithValue("@CouponReceived", dt.Rows[i]["couponrec"]);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                }

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
    }
    private int GetRunningNumber()
    {
        int RunningNo = 1;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Cnn_DB"].ToString()))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 ID FROM CouponPrizedraw ORDER BY ID DESC", con);
            object obj = cmd.ExecuteScalar();

            if (obj != null)
            {
                RunningNo = int.Parse(obj.ToString());
                RunningNo = RunningNo + 1;
            }
        }
        return RunningNo;
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        bool Validation = true; string Field = "";
        if (hfcountlocationtable.Value.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Locations,";
        }
        if (hfcountpaymenttable.Value.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Payments,";
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
            CreateNew(false);
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
        if (hfcountlocationtable.Value.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Locations,";
        }
        if (hfcountpaymenttable.Value.ToString() == "0")
        {
            Validation = false;
            Field = Field + "Table of Payments,";
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
            CreateNew(true);
            //Response.Redirect("../Home.aspx");
            callout_script.Visible = true;
        }
        else
        {
            Field = Field.Remove(Field.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Submit Failed. " + Field + " Must be Filled!!!');", true);
            hfcountlocationtable.Value = "0";
            hfcountpaymenttable.Value = "0";
        }
    }

    protected void ddlCompanyBrand_TextChanged(object sender, EventArgs e)
    {

    }
}