using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_Register_PG_Form : System.Web.UI.Page
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
    App price = new App();

    decimal GoldPrice = 0m;
    static int i = 1;

    protected int myIDWarehouse { get { return App.QueryInt(this, "IDWarehouse"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //keterangan.Focus();

            DLC.CompanyBrand(ddlbrand);
            App.NumberFormat(masaberlaku);

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
        }
    }

    //Save
    protected bool validTgl
    {
        get { return Fv.isDateTime(tgl, tgl_ERR); }
    }

    protected void save_Click(object sender, EventArgs e) {
        if (validTgl && idcustomerhidden.Value != "")
        {
            int IDCustomer = Convert.ToInt32(idcustomerhidden.Value);
            string CustomerNama = customer.Text;
            string Tgl = tgl.Text;
            int MasaBerlaku = Convert.ToInt32(masaberlaku.Text);
            int IDBrand = Convert.ToInt32(ddlbrand.SelectedValue);
            string Peruntukan = peruntukan.Text;
            int Jenis = Convert.ToInt32(ddljenis.SelectedValue);

            for (i = 0; i < EmasGridView.Rows.Count; i++)
            {
                
                string NoVoucher = ((TextBox)EmasGridView.Rows[i].Cells[0].FindControl("txtNoVoucher")).Text;
                decimal Nominal = Convert.ToDecimal(((TextBox)EmasGridView.Rows[i].Cells[0].FindControl("txtNominal")).Text);

                DataVoucher n = new DataVoucher();

                n.IDBrand = IDBrand;
                n.IDCustomer = IDCustomer;
                n.CustomerNama = CustomerNama;
                n.JenisVoucher = Jenis;
                n.Tgl = Convert.ToDateTime(Tgl);
                n.TglExpired = n.Tgl.AddDays(MasaBerlaku);
                n.Keterangan = Peruntukan;
                n.Status = 0;
                n.Nomor = NoVoucher;
                n.Nominal = Nominal;
                n.Operator = App.UserNama;
                n.OperatorTgl = DateTime.Now;
                n.Approval = App.UserNama;
                n.ApprovalTgl = DateTime.Now;

                db.DataVoucher.InsertOnSubmit(n);
                db.SubmitChanges();

            }
            Response.Redirect("Voucher.aspx?d=1");
        }
	}
    protected void btn_add_emas_click(object sender, EventArgs e)
    {
        DataTable dtMember = new DataTable("AdditionalMember");

        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("novoucher"), new DataColumn("nominal")});
        var counter = 1;

        for (var i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];
            //var COIHOSB = Convert.ToDouble(((TextBox)gridViewRowTemp.Cells[2].FindControl("TextBoxAdditionalInsuranceCostHOSB")).Text);

            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["novoucher"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoVoucher")).Text;
            drMember["nominal"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNominal")).Text;

            dtMember.Rows.Add(drMember);

            counter++;
        }



        drMember = dtMember.NewRow();
        drMember["number"] = counter;
        drMember["novoucher"] = "";
        drMember["nominal"] = "0";

        dtMember.Rows.Add(drMember);


        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();
        EmasGridView.Rows[(EmasGridView.Rows.Count - 1)].Cells[1].FindControl("txtNoVoucher").Focus();
    }

    protected void EmasGridView_onRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        DataTable dtMember = new DataTable("AdditionalMember");
        DataTable dtMember2 = new DataTable("AdditionalMember");

        DataRow drMember, drMember2;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("novoucher"), new DataColumn("nominal")});
        dtMember2.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("novoucher"), new DataColumn("nominal")});

        var counter = 1;
        int i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];


            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["novoucher"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoVoucher")).Text;
            drMember["nominal"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNominal")).Text;

            dtMember.Rows.Add(drMember);
            counter++;
        }



        dtMember.Rows[e.RowIndex].Delete();

        EmasGridView.DataSource = dtMember;
        EmasGridView.DataBind();

        counter = 1;

        i = 0;
        for (i = 0; i < EmasGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = EmasGridView.Rows[i];

            drMember2 = dtMember2.NewRow();
            drMember2["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember2["novoucher"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNoVoucher")).Text;
            drMember2["nominal"] = ((TextBox)gridViewRowTemp.Cells[0].FindControl("txtNominal")).Text;

            dtMember2.Rows.Add(drMember2);
            counter++;
        }



        EmasGridView.DataSource = dtMember2;
        EmasGridView.DataBind();
        if (i > 0)
            EmasGridView.Rows[(EmasGridView.Rows.Count - 1)].Cells[1].FindControl("txtNoVoucher").Focus();

    }

    protected void checkVoucher(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();

        string NoVoucher = ((TextBox)sender).Text;

        var d = from p in db.DataVoucher
                where p.Nomor.Equals(NoVoucher)
                select p;
        if (d.Count() > 0)
        {
            err.InnerText = "No. Voucher " + NoVoucher + " sudah terdaftar";
        }
    }
}