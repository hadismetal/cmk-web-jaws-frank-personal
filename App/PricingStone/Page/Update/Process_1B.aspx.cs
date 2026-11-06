using System;
using System.Linq;

public partial class Page_Update_Process_1B : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			harga.Focus();

			//Number-Format
			App.NumberFormat(harga);
			App.NumberFormat(persen);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.StonePricing1B.SingleOrDefault(p => p.ID == myID && !p.Stone1B.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Stone1B.Nama;

			harga.Text = d.InputH.HasValue ? Str.Number2(d.InputH) : harga.Text;
			harga.Text = d.PendingH.HasValue ? Str.Number2(d.PendingH) : harga.Text;

			persen.Text = d.InputP.HasValue ? Str.Number2(d.InputP) : persen.Text;
			persen.Text = d.PendingP.HasValue ? Str.Number2(d.PendingP) : persen.Text;

            markup.Text = d.MarkUp.HasValue ? Str.Number0(d.MarkUp) : markup.Text;
            markup.Text = d.MarkUpPending.HasValue ? Str.Number0(d.MarkUpPending) : markup.Text;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(harga, harga_ERR) && Fv.isDecimal(persen, persen_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.StonePricing1B.SingleOrDefault(p => p.ID == myID && !p.Stone1B.Disable);
			if (d == null) App.DataNull(this);
			else {
                //var rs = from p in db.StonePricing1B
                //         where p.Stone1B.Parcel101 == d.Stone1B.Parcel101 && p.Stone1B.Parcel102 == d.Stone1B.Parcel102
                //         && p.Stone1B.Parcel103 == d.Stone1B.Parcel103 && p.Stone1B.Parcel104 == d.Stone1B.Parcel104
                //         && p.Stone1B.Parcel106.NamaKode != "NN"
                //         select p;
                //foreach (var r in rs)
                //{
                //    if (d.Stone1B.Parcel106.NamaKode != "NN")
                //    {
                //        r.PendingH = Convert.ToDecimal(harga.Text);

                //        decimal InputP = 0m;
                //        if (r.PendingP.HasValue)
                //            InputP = r.PendingP.Value;
                //        else if (r.InputP.HasValue)
                //            InputP = r.InputP.Value;

                //        r.PendingP = InputP;

                //        //Rumus Rapaport
                //        r.HargaPending = r.PendingH + (r.PendingH * (InputP / 100));

                //        //Mutasi
                //        r.Mutasi = r.Harga.HasValue ? r.HargaPending - r.Harga : r.HargaPending;
                //        r.MutasiPersen = r.Harga != 0 ? (r.Mutasi / r.Harga) * 100 : null;
                        

                //        //Status
                //        r.Stone1B.StatusPricing = 2;

                //        //User-Info
                //        r.Operator = App.UserNama;
                //        r.OperatorTgl = DateTime.Now;

                //        //Commit
                //        db.SubmitChanges();
                //    }
                //}
			
				d.PendingH = Convert.ToDecimal(harga.Text);
				d.PendingP = Convert.ToDecimal(persen.Text);

				//Rumus Rapaport
				d.HargaPending = d.PendingH + (d.PendingH * (d.PendingP / 100));

				//Mutasi
				d.Mutasi = d.Harga.HasValue ? d.HargaPending - d.Harga : d.HargaPending;
				d.MutasiPersen = d.Harga != 0 ? (d.Mutasi / d.Harga) * 100 : null;
                d.MarkUpPending = Convert.ToDecimal(markup.Text);

				//Status
				d.Stone1B.StatusPricing = 2;

				//User-Info
				d.Operator = App.UserNama;
				d.OperatorTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
		}
	}
}