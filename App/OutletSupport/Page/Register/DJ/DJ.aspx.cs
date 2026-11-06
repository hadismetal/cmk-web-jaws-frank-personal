using System;
using System.Linq;

public partial class Page_Create_Product : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

            DLC.PromoStatus(tipe);

            datepicker.Text = Str.Tgl(DateTime.Now.AddDays(1));
            datepicker2.Text = Str.Tgl(DateTime.Now.AddDays(1));

            nama.Focus();

            /// promo 1 harga
            tipe.SelectedValue = "3";
            discbox.Visible = false;
            discpersenbox.Visible = false;
            poinbox.Visible = false;
            promohargabox.Visible = true;
			divextraringsize.Visible = true;
			extraringsize.Text = "0";

            //promoharga.Focus();
            //discbox.Visible = true;
            //discpersenbox.Visible = true;
            //App.NumberFormat(discount);
		}
	}

	
	protected void next_Click(object sender, EventArgs e) {

        if (!valid()) { lit_msg.Text = "<div class='alert alert-warning' role='alert'>Tanggal tidak Valid, Tanggal Mulai tidak boleh kurang dari hari ini atau besok</div>"; }
        else
        {
            PromoDJ n = new PromoDJ();

            n.Nama = Str.CapTitle(nama.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.TglStart = Convert.ToDateTime(datepicker.Text);
            n.TglExpire = Convert.ToDateTime(datepicker2.Text);
            n.DiscountNominal = discount.Text != "" ? Convert.ToDecimal(discount.Text) : 0;
            n.DiscountPersen = discountpersen.Text != "" ? Convert.ToDecimal(discountpersen.Text) : 0;
            n.Tipe = Convert.ToInt32(tipe.SelectedValue);
            n.PoinReward = poin.Text != "" ? Convert.ToDecimal(poin.Text) : 0;
            n.PromoHarga = promoharga.Text != "" ? Convert.ToDecimal(promoharga.Text) : 0;
            n.StatusASP = Convert.ToInt32(statusasp.Checked);

            n.Draft = true;
            n.DraftDate = DateTime.Now;
            n.OperatorTgl = DateTime.Now;
            n.OperatorNama = App.UserNama;

            n.StatusDiscMembership = (statusmembership.Checked) ? true : false;
            n.StatusDiscNormal = (statusdiscnorm.Checked) ? true : false;

            db.PromoDJ.InsertOnSubmit(n);
            db.SubmitChanges();
			
			int n_extraringsize = Convert.ToInt32(extraringsize.Text);
			string s_extraringsize = n_extraringsize.ToString();
			
			if(checkextraringsize.Checked)
			{
				if(n_extraringsize > 0)
				{
					Db.Execute("UPDATE PromoDJ SET StatusExtraRingSize = 1, ExtraRingSize = " + s_extraringsize + " WHERE ID = " + n.ID, Db.Cnn_DB);
				}
			}

            if (rdo_plu.Checked)
            {
                Response.Redirect("DJInfoPLU.aspx?id=" + n.ID);
            }
            else
            {
                Response.Redirect("DJInfo.aspx?id=" + n.ID);
            }
        }
	}

    protected bool valid()
    {
        bool status = true;

        DateTime d_pf = Convert.ToDateTime(datepicker.Text);
        DateTime d_pt = Convert.ToDateTime(datepicker2.Text);

        if (d_pf > d_pt) { status = false; }
        //if ((d_pf.Year + d_pf.Month + d_pf.Day) <= (DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day)) { status = false; }
        if (Convert.ToInt32(d_pf.Year.ToString("0000") + d_pf.Month.ToString("00") + d_pf.Day.ToString("00")) 
            < (Convert.ToInt32(DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00"))
               )
            ){ status = false; }

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