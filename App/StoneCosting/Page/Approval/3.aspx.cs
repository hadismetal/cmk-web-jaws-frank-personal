using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_3 : System.Web.UI.Page {
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
		App.Protect();
		App.ProtectRole(Role.Manager);

		if (!IsPostBack)
        {
            DLC.Parcel301(parcel01);
            DLC.Parcel302(parcel02);
            DLC.Parcel303(parcel03);
            DLC.Parcel304(parcel04);
            DLC.Parcel305(parcel05);
            DLC.Parcel306(parcel06);
            DLC.Parcel307(parcel07);
            DLC.Parcel308(parcel08);

            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);
            //Role Protection
            App.TableRole(tb, Role.Operator);
            
            fillFilter();
            fillTable(sender, e);

        }
        
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
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
            parcel08.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel08"]);
            order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
            orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&parcel08=" + parcel08.SelectedIndex;
        href += "&order=" + order.SelectedIndex;
        href += "&orderby=" + orderby.SelectedIndex;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        Response.Redirect(href);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e) {

        string w = "1=1";
        if (parcel01.SelectedIndex > 0) w += " AND Parcel01 = " + parcel01.SelectedValue;
        if (parcel02.SelectedIndex > 0) w += " AND Parcel02 = " + parcel02.SelectedValue;
        if (parcel03.SelectedIndex > 0) w += " AND Parcel03 = " + parcel03.SelectedValue;
        if (parcel04.SelectedIndex > 0) w += " AND Parcel04 = " + parcel04.SelectedValue;
        if (parcel05.SelectedIndex > 0) w += " AND Parcel05 = " + parcel05.SelectedValue;
        if (parcel06.SelectedIndex > 0) w += " AND Parcel06 = " + parcel06.SelectedValue;
        if (parcel07.SelectedIndex > 0) w += " AND Parcel07 = " + parcel07.SelectedValue;
        if (parcel08.SelectedIndex > 0) w += " AND Parcel08 = " + parcel08.SelectedValue;

        //ORDER BY
        string o = order.SelectedValue + " " + orderby.SelectedValue;



        //SELECT
        var rs = from p in db.Stone3.Where(w).OrderBy(o)
                 //orderby p.Nama
				 where !p.Disable && p.StatusCosting == 2
				 select p;


        if (kw.Text != "")
        {
            rs = rs.Where(p => p.KodeParcel.Contains(kw.Text) || p.Nama.Contains(kw.Text));
        }

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.StoneCosting3.OperatorTgl >= start && p.StoneCosting3.OperatorTgl < end.AddDays(1));
        }

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
			td.Text = TD_Operator(r.StoneCosting3);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StoneCosting3);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_HargaPending(r.StoneCosting3);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r.StoneCosting3);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
		}
	}
	protected string TD_HyperLink(Stone3 r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Update/Process_3.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(Stone3 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Operator(StoneCosting3 r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Harga(StoneCosting3 r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_HargaPending(StoneCosting3 r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number2(r.HargaPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(StoneCosting3 r) {
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

			var d = db.StoneCosting3.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.Stone3.Disable == false &&
				p.Stone3.StatusCosting == 2
				);

			if (d != null) {
				decimal? HargaLama = d.Harga;
				decimal? HargaBaru = d.HargaPending;

				d.Harga = d.HargaPending;
				d.HargaPending = null;

				//Mutasi
				d.Mutasi = null;
				d.MutasiPersen = null;

				//Status
				d.Stone3.StatusCosting = 0;
				d.Stone3.StatusPricing = 1;

				//User-Info
				d.Approval = App.UserNama;
				d.ApprovalTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Post-Process
				Modal(d);
				Log(d.ID, HargaLama, HargaBaru);

                //// Fungsi untuk posting data Costing Stone ke manufacture
                try
                {
                    var rsCB = (from p in db.CompanyBrand
                                where p.Draft == false && p.Disable == false
                                select p.Nama).First();

                    string CB = ""; /// mondial, franknco, missmondial, thepalace
                    if (rsCB.Count() > 0) { CB = rsCB.ToLower(); }

                    if (CB == "mondial" || CB == "miss mondial")
                    {
                        string SPname = "sp_poststonecosting";

                        Db.Execute("EXEC " + SPname + " "
                            + " @stone_type = N'" + "3" + "'"
                            + ",@stone_name = N'" + d.Stone3.Nama + "'"
                            + ",@stone_id = N'" + d.ID.ToString() + "'"
                            + ", " + "@flag_all = 0"
                            , Db.Cnn_DB);
                    }
                }
                catch (Exception ex) { }

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

            var d = db.StoneCosting3.SingleOrDefault(
                p =>
                p.ID == myID &&
                p.Stone3.Disable == false &&
                p.Stone3.StatusCosting == 2
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
                d.Stone3.StatusCosting = 0;
                //d.Stone3.StatusPricing = 1;

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

	protected void Modal(StoneCosting3 costing) {
		var d = db.StonePricing3.SingleOrDefault(p => p.ID == costing.ID);
		if (d == null) {
            try
            {
                StonePricing3 n = new StonePricing3();
                n.ID = costing.ID;
                n.Modal = costing.Harga;
            }
            catch
            {
                DLC.Reseed("StonePricingLog3");

                StonePricing3 n = new StonePricing3();
                n.ID = costing.ID;
                n.Modal = costing.Harga;
            }
		}
		else {
			d.Modal = costing.Harga;
			if (d.Harga.HasValue) {
				d.Selisih = d.Harga - d.Modal;
				d.SelisihPersen = d.Modal != 0 ? (d.Selisih / d.Modal) * 100 : null;
			}
		}

		//Commit
		db.SubmitChanges();
	}
	protected void Log(int IDMaster, decimal? HargaLama, decimal? HargaBaru) {
		if (HargaLama != HargaBaru) {
            try
            {
                StoneCostingLog3 n = new StoneCostingLog3();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;

                //Commit
                db.StoneCostingLog3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
            catch
            {
                DLC.Reseed("StoneCostingLog3");

                StoneCostingLog3 n = new StoneCostingLog3();

                n.IDMaster = IDMaster;
                n.Tgl = DateTime.Now;
                n.Approval = App.UserNama;

                n.HargaLama = HargaLama.HasValue ? HargaLama.Value : 0;
                n.HargaBaru = HargaBaru.HasValue ? HargaBaru.Value : 0;

                //Commit
                db.StoneCostingLog3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
		}
	}
}