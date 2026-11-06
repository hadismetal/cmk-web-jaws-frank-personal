using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Default : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected string myError { get { return Request.QueryString["err"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //Form
            fillForm();

        }
    }

    protected void DataError() { Response.Redirect("SPK.aspx"); }
    protected void DataError2() { Response.Redirect("SPK_Data02.aspx?id=" + myID); }

    protected void fillForm()
    {
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null)
        {
            if (myID != null && myID > 0)
            {
                DataError2();
            }
            else
            {
                DataError();
            }

        }
        else
        {
            if (myError != null && myError != "")
            {
                rdo1_st.Checked = true;
                lit_errormsg.Text = "<span style='color:red;'>Error Code #" + myError + "</span>";
            }
            else
            {
                if (((d.NoPO != null) && (d.NoPO != "")) || ((d.RequestCustomer != 0) && (d.RequestCustomerNama != "")))
                {
                    rdo1_pe.Checked = true;

                    rdo1_pr.Disabled = true;
                    rdo1_st.Disabled = true;
                    rdo1_stt.Disabled = true;
                }
                else
                {
                    rdo1_st.Checked = true;
                }
            }
        }
   }

    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            string statbar = "";
            if (rdo1_pe.Checked) { statbar = rdo1_pe.Value.Trim(); }
            else if (rdo1_pr.Checked) { statbar = rdo1_pr.Value.Trim(); }
            else if (rdo1_st.Checked) { statbar = rdo1_st.Value.Trim(); }
            else if (rdo1_stt.Checked) { statbar = rdo1_stt.Value.Trim(); }

            d.StatusBarang = statbar;
            int sbid = (from p in db.MasterStatusBarangSPK where p.Nama == statbar select p.ID).SingleOrDefault();
            d.StatusBarangID = sbid;
            d.StatusBarangKeterangan = ketstatus.Text;
            db.SubmitChanges();

            //Redirect
            Response.Redirect("SPK_CharProduct01.aspx?id=" + myID + "&tipe=" + tipe);
        }

    }
}