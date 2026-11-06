using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;

public partial class Page_Register_DJ_Form : System.Web.UI.Page {
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
	protected int myProductDesign { get { return App.QueryInt(this, "idproductdesign"); } }
    protected int myKodeKaret { get { return App.QueryInt(this, "idkodekaret"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

            datepicker.Text = Str.Tgl(DateTime.Today);

            DLC.CompanyBrand(ddl_CompanyBrand);

            //Generate
            fillFilter();
            if (App.Refresh(this)) Set();
		}
	}

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }

    private void Set()
    {
        
        int Brand = Convert.ToInt32(ddl_CompanyBrand.SelectedValue);

        var rs = from p in db.ProductOrder
                 where p.StatusFPS == 0 && p.IDBrand == Brand && p.Draft == false
                 select p;

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        DataTable dtMember = new DataTable("AdditionalMember");

        int counter = 1;
        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nospk") });
        if (rs != null)
        {
            foreach (var r in rs)
            {

                drMember = dtMember.NewRow();
                drMember["number"] = counter;
                drMember["nospk"] = r.NoSPK;

                dtMember.Rows.Add(drMember);
                counter++;
            }
            SPKGridView.DataSource = dtMember;
            SPKGridView.DataBind();
        }

    }

    protected void Brand_Click(object sender, EventArgs e)
    {
        int Brand = Convert.ToInt32(ddl_CompanyBrand.SelectedValue);

        var rs = from p in db.ProductOrder
                 where p.StatusFPS == 0 && p.IDBrand == Brand && p.Draft == false
                 select p;

        DataTable dtMember = new DataTable("AdditionalMember");

        int counter = 1;
        DataRow drMember;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nospk") });
        if (rs != null)
        {
            foreach (var r in rs)
            {

                drMember = dtMember.NewRow();
                drMember["number"] = counter;
                drMember["nospk"] = r.NoSPK;

                dtMember.Rows.Add(drMember);
                counter++;
            }
            SPKGridView.DataSource = dtMember;
            SPKGridView.DataBind();
        }
    }

    protected void SPKGridView_onRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dtMember = new DataTable("AdditionalMember");
        DataTable dtMember2 = new DataTable("AdditionalMember");

        DataRow drMember, drMember2;
        dtMember.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nospk") });
        dtMember2.Columns.AddRange(new DataColumn[] { new DataColumn("number"), new DataColumn("nospk") });

        var counter = 1;
        int i = 0;
        for (i = 0; i < SPKGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = SPKGridView.Rows[i];



            drMember = dtMember.NewRow();
            drMember["number"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txbNumberList")).Text;
            drMember["nospk"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtNoSPK")).Text;

            dtMember.Rows.Add(drMember);
            counter++;
        }

        dtMember.Rows[e.RowIndex].Delete();

        SPKGridView.DataSource = dtMember;
        SPKGridView.DataBind();

        counter = 1;

        i = 0;
        for (i = 0; i < SPKGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = SPKGridView.Rows[i];

            drMember2 = dtMember2.NewRow();
            drMember2["number"] = counter;
            drMember2["nospk"] = ((Literal)gridViewRowTemp.Cells[0].FindControl("txtNoSPK")).Text;
            dtMember2.Rows.Add(drMember2);
            counter++;
        }



        SPKGridView.DataSource = dtMember2;
        SPKGridView.DataBind();
    }

    protected bool valid
    {
        get { return Fv.isDateTime(datepicker, datepicker_ERR); }
    }

    protected void save_Click(object sender, EventArgs e)
    {
        int Brand = Convert.ToInt32(ddl_CompanyBrand.SelectedValue);
        int SeqFPSNumber = 1;
        string Bulan = DateTime.Now.ToString("MM");
        string Tahun = DateTime.Now.ToString("yy");

        try
        {
            SeqFPSNumber = Db.SingleInteger("SELECT COUNT(*) FROM ProductStoneRequest",Db.Cnn_DB);
            SeqFPSNumber = SeqFPSNumber + 1;
        }
        catch (Exception ex)
        {
            SeqFPSNumber = 1;
        }

        string NoDokumen = "FPS/" + Bulan + "/" + Tahun + "/" + Str.KodeFormat(SeqFPSNumber, 5);
        DateTime TanggalJatuhTempo = Convert.ToDateTime(datepicker.Text);
        DateTime Today = DateTime.Now;
        string Keterangan = txt_keterangan.Text;

        ProductStoneRequest n = new ProductStoneRequest();

        n.Tgl = Today;
        n.TglJatuhTempo = TanggalJatuhTempo;
        n.Keterangan = Keterangan;
        n.NomorDokumen = NoDokumen;
        n.IDBrand = Brand;
        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;

        db.ProductStoneRequest.InsertOnSubmit(n);
        db.SubmitChanges();

        int IDFPS = Db.SingleInteger("SELECT TOP 1 ID FROM ProductStoneRequest ORDER BY ID DESC", Db.Cnn_DB);

        SaveFPSDetail(IDFPS);

        Response.Redirect("ProductStoneRequest.aspx?d=1");

    }

    protected void SaveFPSDetail(int IDFPS)
    {
        for (var i = 0; i < SPKGridView.Rows.Count; i++)
        {
            var gridViewRowTemp = SPKGridView.Rows[i];

            string NoSPK = ((Literal)gridViewRowTemp.Cells[1].FindControl("txtNoSPK")).Text;

            int IDSPK = Db.SingleInteger("SELECT ID FROM ProductOrder WHERE NoSPK ='" + NoSPK + "'",Db.Cnn_DB);

            var d = db.ProductOrder.SingleOrDefault(x => x.ID == IDSPK);

            if (d != null)
            {
                d.StatusFPS = 1;
                d.IDFPS = IDFPS;

                ProductStoneRequest_Detail n = new ProductStoneRequest_Detail();
                n.IDProductStoneRequest = IDFPS;
                n.Nomor = NoSPK;
                n.IDProductOrder = IDSPK;

                db.ProductStoneRequest_Detail.InsertOnSubmit(n);
                db.SubmitChanges();
            }

        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
}