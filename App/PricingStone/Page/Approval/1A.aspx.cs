using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_1A : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
			//Role Protection
			App.TableRole(tb, Role.Operator);

            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, false);
            DLC.Parcel103(parcel03);
            DLC.Parcel104(parcel04);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);
            status.SelectedValue = "2";

            fillFilter();

			//Generate
			fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.Stone1A
				 orderby p.Nama
				 where !p.Disable //&& p.StatusPricing == 2
				 select p;

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);

        if (kw.Text != "")
        {
            rs = rs.Where(p => p.KodeParcel.Contains(kw.Text));
        }

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StonePricing1A.OperatorTgl >= start && p.StonePricing1A.OperatorTgl < end.AddDays(1));
        }

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusPricing == 0);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusPricing == 2);

        //NO-RESULT
        //zero.Visible = rs.Count() == 0;
        //form_area.Visible = !zero.Visible;

        if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			if (Role.Operator) {
				td = new TableCell();
				td.Text = TD_HyperLink(r);
				tr.Cells.Add(td);
			}

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r.StonePricing1A);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Modal(r.StonePricing1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StonePricing1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_HargaPending(r.StonePricing1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r.StonePricing1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
		}
	}
	protected string TD_HyperLink(Stone1A r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Update/Process_1A.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(Stone1A r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Operator(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Modal(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-info text-lg\">";
		x += Str.Number2(r.Modal);
		x += "</p>";

		return x;
	}
	protected string TD_Harga(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_HargaPending(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number2(r.HargaPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(StonePricing1A r) {
		string x = string.Empty;

		if (r.MutasiPersen.HasValue) {
			string sgn = r.MutasiPersen > 0 ? "+" : "";
			string css = r.MutasiPersen > 10 || r.MutasiPersen < -10 ? "text-red" : "";

			x += "<p class=\"" + css + " text-lg\">";
			x += sgn;
			x += Str.Number2(r.MutasiPersen);
			x += "%";
			x += "</p>";
		}
		else if (r.Mutasi.HasValue) {
			string sgn = r.Mutasi > 0 ? "+" : "";

			x += "<p class=\"text-lg\">";
			x += sgn;
			x += Str.Number2(r.Mutasi);
			x += "</p>";
		}

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var rs = from p in process_id.Items.Cast<ListItem>()
				 select p.Value;

		foreach (var r in rs) {
			int myID = Convert.ToInt32(r);

			var d = db.StonePricing1A.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.Stone1A.Disable == false &&
				p.Stone1A.StatusPricing == 2
				);

			if (d != null) {
				decimal? HargaLama = d.Harga;
				decimal? HargaBaru = d.HargaPending;

				d.Harga = d.HargaPending;
				d.HargaPending = null;

				//Mutasi
				d.Mutasi = null;
				d.MutasiPersen = null;

				//Selisih
				if (d.Modal.HasValue) {
					d.Selisih = d.Harga - d.Modal;
					d.SelisihPersen = d.Modal != 0 ? (d.Selisih / d.Modal) * 100 : null;
				}

				//Status
				d.Stone1A.StatusPricing = 0;

				//User-Info
				d.Approval = App.UserNama;
				d.ApprovalTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Post-Process
				Log(d.ID, HargaLama, HargaBaru);
			}
		}

		//Redirect
		App.Reload(this);
	}

    //Cancel
    protected void cancel_Click(object sender, EventArgs e)
    {
        var rs = from p in process_id.Items.Cast<ListItem>()
                 select p.Value;

        foreach (var r in rs)
        {
            int myID = Convert.ToInt32(r);

            var d = db.StonePricing1A.SingleOrDefault(
                p =>
                p.ID == myID &&
                p.Stone1A.Disable == false &&
                p.Stone1A.StatusPricing == 2
                );

            if (d != null)
            {
                decimal? HargaLama = d.Harga;
                decimal? HargaBaru = d.HargaPending;

                d.HargaPending = null;

                //Mutasi
                d.Mutasi = null;
                d.MutasiPersen = null;

                //Status
                d.Stone1A.StatusPricing = 0;

                //User-Info
                d.Approval = App.UserNama;
                d.ApprovalTgl = DateTime.Now;

                //Commit
                db.SubmitChanges();
            }
        }

        //Redirect
        App.Reload(this);
    }

	protected void Log(int IDMaster, decimal? HargaLama, decimal? HargaBaru) {
		if (HargaLama != HargaBaru) {
            try
            {
                StonePricingLog1A n = new StonePricingLog1A();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;

                //Commit
                db.StonePricingLog1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
            catch
            {
                DLC.Reseed("StonePricingLog1A");

                StonePricingLog1A n = new StonePricingLog1A();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;

                //Commit
                db.StonePricingLog1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
		}
	}
    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&status=" + status.SelectedIndex;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
            parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
            parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
            parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
            status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }
}