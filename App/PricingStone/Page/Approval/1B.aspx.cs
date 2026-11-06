using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_1B : System.Web.UI.Page {
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

            //DLC
            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, true);
            DLC.Parcel103(parcel03);
            DLC.Parcel104(parcel04);
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);
            status.SelectedValue = "2";

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
        href += "&display=" + display.SelectedIndex;
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&status=" + status.SelectedIndex;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
            parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
            parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
            parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
            status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            display.SelectedIndex = Convert.ToInt32(Request.QueryString["display"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.Stone1B
				 orderby p.Nama
				 where !p.Disable
				 select p;

        //NO-RESULT
        //zero.Visible = rs.Count() == 0;
        //form_area.Visible = !zero.Visible;

        if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
        if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
        if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
        if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));
        if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(parcel05.SelectedValue));
        if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(parcel06.SelectedValue));
        if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(parcel07.SelectedValue));

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
            rs = rs.Where(p => p.StonePricing1B.OperatorTgl >= start && p.StonePricing1B.OperatorTgl < end.AddDays(1));
        }

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusPricing == 0);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusPricing == 2);

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
			td.Text = TD_Operator(r.StonePricing1B);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Modal(r.StonePricing1B);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StonePricing1B);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_HargaPending(r.StonePricing1B);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r.StonePricing1B);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_MU((r.StonePricing1B.MarkUp.HasValue) ? r.StonePricing1B.MarkUp.Value : 0);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_MU((r.StonePricing1B.MarkUpPending.HasValue) ? r.StonePricing1B.MarkUpPending.Value : 0);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
		}
	}
	protected string TD_HyperLink(Stone1B r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Update/Process_1B.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(Stone1B r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Operator(StonePricing1B r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Modal(StonePricing1B r) {
		string x = string.Empty;

		if (display.SelectedIndex == 0) {
			x += "<p class=\"text-info text-lg\">";
			x += Str.Number2(r.Modal);
			x += "</p>";
		}
		else {
			x += "<p class=\"text-info\">" + Str.Number2(r.ModalH) + "</p>";
			x += "<p class=\"text-info\">" + Str.Number2(r.ModalP) + "</p>";

		}

		return x;
	}
	protected string TD_Harga(StonePricing1B r) {
		string x = string.Empty;

		if (display.SelectedIndex == 0) {
			x += "<p class=\"text-success text-lg\">";
			x += Str.Number2(r.Harga);
			x += "</p>";
		}
		else {
			x += "<p class=\"text-success\">" + Str.Number2(r.InputH) + "</p>";
			x += "<p class=\"text-success\">" + Str.Number2(r.InputP) + "</p>";
		}

		return x;
	}
	protected string TD_HargaPending(StonePricing1B r) {
		string x = string.Empty;

		if (display.SelectedIndex == 0) {
			x += "<p class=\"text-danger text-lg\">";
			x += Str.Number2(r.HargaPending);
			x += "</p>";
		}
		else {
			x += "<p class=\"text-danger\">" + Str.Number2(r.PendingH) + "</p>";
			x += "<p class=\"text-danger\">" + Str.Number2(r.PendingP) + "</p>";
		}

		return x;
	}
	protected string TD_Mutasi(StonePricing1B r) {
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

			var d = db.StonePricing1B.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.Stone1B.Disable == false &&
				p.Stone1B.StatusPricing == 2
				);

			if (d != null) {
				decimal? HargaLama = d.Harga;
				decimal? HargaBaru = d.HargaPending;
                decimal? MarkUpLama = d.MarkUp;
                decimal? MarkUpBaru = d.MarkUpPending;

				d.Harga = d.HargaPending;
				d.HargaPending = null;

				d.InputH = d.PendingH; d.PendingH = null;
				d.InputP = d.PendingP; d.PendingP = null;

				//Mutasi
				d.Mutasi = null;
				d.MutasiPersen = null;

                d.MarkUp = d.MarkUpPending.Value;
                d.MarkUpPending = null;

				//Selisih
				if (d.Modal.HasValue) {
					d.Selisih = d.Harga - d.Modal;
					d.SelisihPersen = d.Modal != 0 ? (d.Selisih / d.Modal) * 100 : null;
				}

				//Status
				d.Stone1B.StatusPricing = 0;

				//User-Info
				d.Approval = App.UserNama;
				d.ApprovalTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Post-Process
				Log(d.ID, HargaLama, HargaBaru, MarkUpLama, MarkUpBaru);
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

            var d = db.StonePricing1B.SingleOrDefault(
                p =>
                p.ID == myID &&
                p.Stone1B.Disable == false &&
                p.Stone1B.StatusPricing == 2
                );

            if (d != null)
            {
                decimal? HargaLama = d.Harga;
                decimal? HargaBaru = d.HargaPending;

                d.HargaPending = null;

                d.PendingH = null;
                d.PendingP = null;

                //Mutasi
                d.Mutasi = null;
                d.MutasiPersen = null;

                d.MarkUpPending = null;

                //Status
                d.Stone1B.StatusPricing = 0;

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

	protected void Log(int IDMaster, decimal? HargaLama, decimal? HargaBaru, decimal? MarkUpLama, decimal? MarkUpBaru) {
		if (HargaLama != HargaBaru) {
            try
            {
                StonePricingLog1B n = new StonePricingLog1B();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;
                n.MarkUpLama = MarkUpLama.HasValue ? MarkUpLama.Value : 0;
                n.MarkUpBaru = MarkUpBaru.HasValue ? MarkUpBaru.Value : 0;

                //Commit
                db.StonePricingLog1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
            catch
            {
                DLC.Reseed("StonePricingLog1B");

                StonePricingLog1B n = new StonePricingLog1B();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;
                n.MarkUpLama = MarkUpLama.HasValue ? MarkUpLama.Value : 0;
                n.MarkUpBaru = MarkUpBaru.HasValue ? MarkUpBaru.Value : 0;

                //Commit
                db.StonePricingLog1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
		}
	}

    protected string TD_MU(decimal MU)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg\">";
        x += Str.Number0(MU);
        x += "%";
        x += "</p>";

        return x;
    }
}