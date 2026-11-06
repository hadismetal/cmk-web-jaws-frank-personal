using System;
using System.Linq;

public partial class GJ_Expire : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this, "id"); } }
	protected void Page_Load(object sender, EventArgs e) 
    {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) 
        {
            fillTable();
		}
	}

    protected void fillTable()
    {
        var d = db.PromoGJ.SingleOrDefault(p => p.ID == myID);

        nama.Text = d.Nama;
        nama.Enabled = false;

        keterangan.Text = d.Keterangan;
        keterangan.Enabled = false;

        statusasp.Checked = (d.StatusASP == 1) ? true : false;
        statusasp.Enabled = false;

        datepicker.Text = Str.Tgl(d.TglStart);
        datepicker2.Text = Str.Tgl(d.TglExpire);

        datepicker.Enabled = false;
        datepicker2.Focus();

        tipe.SelectedValue = d.Tipe.ToString();
        tipe.Enabled = false;

        if (d.Tipe == 1)
        {
            discbox.Visible = true;
            discpersenbox.Visible = true;
            poinbox.Visible = false;
            promohargabox.Visible = false;

            discount.Text = Convert.ToDecimal(d.DiscountNominal).ToString("#,##0");
            discountpersen.Text = Convert.ToDecimal(d.DiscountPersen).ToString("#0.00");
        }
        else if (d.Tipe == 2)
        {
            discbox.Visible = false;
            discpersenbox.Visible = false;
            poinbox.Visible = true;
            promohargabox.Visible = false;

            poin.Text = Convert.ToDecimal(d.PoinReward).ToString("#,##0");
        }
        else if (d.Tipe == 3)
        {
            discbox.Visible = false;
            discpersenbox.Visible = false;
            poinbox.Visible = false;
            promohargabox.Visible = true;

            promoharga.Text = Convert.ToDecimal(d.PromoHarga).ToString("#,##0");
        }

        discount.Enabled = false;
        discountpersen.Enabled = false;
        poin.Enabled = false;
        promoharga.Enabled = false;
    }

	protected void update_Click(object sender, EventArgs e) 
    {
        if (!valid()) { lit_msg.Text = "<div class='alert alert-warning' role='alert'>Tanggal tidak Valid, Tanggal Expire tidak boleh kurang dari hari ini</div>"; }
        else
        {
            var d = db.PromoGJ.SingleOrDefault(p => p.ID == myID);

            d.TglExpire = Convert.ToDateTime(datepicker2.Text);
            
            db.SubmitChanges();

            Response.Redirect("GJ_Detail.aspx?id=" + myID);
        }
	}


    protected void back_Click(object sender, EventArgs e)
    {
        /// GJ.aspx?refreshPage=1&kw=&dari=01-11-2015&sampai=09-11-2015&status=0&tipelokasi=-1&lokasi=-1
        
        string href = "";
        href = "GJ.aspx?refreshPage=1";
        href += "&kw=" + Request.QueryString["kw"];
        href += "&dari=" + Request.QueryString["dari"];
        href += "&sampai=" + Request.QueryString["sampai"];
        href += "&status=" + Request.QueryString["status"];
        href += "&tipelokasi=" + Request.QueryString["tipelokasi"];
        href += "&lokasi=" + Request.QueryString["lokasi"];

        Response.Redirect(href);
    }


    protected bool valid()
    {
        bool status = true;

        var d = db.PromoGJ.SingleOrDefault(p => p.ID == myID);


        DateTime d_pf = Convert.ToDateTime(d.TglStart);
        DateTime d_pt = Convert.ToDateTime(datepicker2.Text);

        if (d_pf > d_pt) { status = false; }
        if ((d_pt.Year + d_pt.Month + d_pt.Day) < (DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day)) { status = false; }

        return status;
    }

    protected void promoSelected(object sender, EventArgs e)
    {
        if (tipe.SelectedValue == "1")
        {
            discbox.Visible = true;
            discpersenbox.Visible = true;
            poinbox.Visible = false;
            promohargabox.Visible = false;

            discount.Focus();
        }
        else if (tipe.SelectedValue == "2")
        {
            discbox.Visible = false;
            discpersenbox.Visible = false;
            poinbox.Visible = true;
            promohargabox.Visible = false;
            poin.Focus();
        }
        else if (tipe.SelectedValue == "3")
        {
            discbox.Visible = false;
            discpersenbox.Visible = false;
            poinbox.Visible = false;
            promohargabox.Visible = true;
            promoharga.Focus();
        }
    }
}