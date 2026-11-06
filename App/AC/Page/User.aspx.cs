using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_User : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
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

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//DLC
			DLC.Status(status);

			//Form-Default
			status.SelectedIndex = 0;

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&login=" + login.SelectedIndex;
		href += "&pass=" + pass.SelectedIndex;
		href += "&status=" + status.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			login.SelectedIndex = Convert.ToInt32(Request.QueryString["login"]);
			pass.SelectedIndex = Convert.ToInt32(Request.QueryString["pass"]);
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (login.SelectedIndex == 1) w += " AND Blokir.HasValue";
		if (login.SelectedIndex == 2) w += " AND (LastLogin.Value = Null OR LastLogin.Value.AddMonths(3) < DateTime.Now)";
		if (pass.SelectedIndex == 1) w += " AND Recycle = True";
		if (pass.SelectedIndex == 2) w += " AND Recycle = False AND PassStrong = True";
		if (pass.SelectedIndex == 3) w += " AND Recycle = False AND PassStrong = False";
		if (status.SelectedIndex > 0) w += " AND Status = " + status.SelectedValue;

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.UserAccount.Where(w).OrderBy(o)
				 where
					(
					p.Nama.Contains(kw.Text) ||
					p.Keterangan.Contains(kw.Text) ||
					p.Email.Contains(kw.Text) ||
					p.SMS.Contains(kw.Text) ||
                    p.UserNumber.Contains(kw.Text)
                    )
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
            td.Text = TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nama(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Email(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Status(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Password(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_LastLogin(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(UserAccount r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"UserFile.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"UserUpdate.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-pencil\"></i> ";
		x += "      Update";
		x += "  </a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(UserAccount r) {
		string x = string.Empty;

        string color = "";
        if (r.UserNumber == null) { color = "style='color:red;'"; }

        x += "<span " + color + "><strong>" + r.Nama.ToUpper() + "</strong></span><br />";
        x += "<span class='text-muted'>" + r.UserNumber + "</span>";

		return x;
	}
    protected string TD_Email(UserAccount r)
    {
        string x = string.Empty;

        string color = "";
        if (!Email.valid_Email2(r.Email)) { color = "style='color:red;'"; }
        x += "<span " + color + ">" + r.Email + "</span>";

        if (r.Keterangan.Trim() != "")
        {
            x += "<br /><span class='text-muted'>" + r.Keterangan.Trim() + "</span>";
        }

        return x;
    }
	protected string TD_Contact(UserAccount r) {
		string x = string.Empty;

		x += "<p>" + r.Email + "</p>";
		if (r.SMS != "")
			x += "<p>" + r.SMS + "</p>";

		return x;
	}
	protected string TD_Status(UserAccount r) {
		string x = string.Empty;

        if (r.Status == 1)
        {
            x += "<span class='label label-success'>" + DL.Status(r.Status) + "</span>";
        }
        else if (r.Status == 2)
        {
            x += "<span class='label label-danger'>" + DL.Status(r.Status) + "</span>";
        }
        else
        {
            x += "<span class='label label-warning'>" + DL.Status(r.Status) + "</span>";
        }

		return x;
	}
	protected string TD_LastLogin(UserAccount r) {
		string x = string.Empty;

		if (!r.LastLogin.HasValue)
			x += "<p class=\"text-danger\">Tidak Pernah</p>";
		else {
			string css = r.LastLogin.Value.AddMonths(3) < DateTime.Now ? "text-danger" : "";
			x += "<p class=\"" + css + "\">" + Str.FullDate(r.LastLogin.Value) + "</p>";
			x += "<p class=\"" + css + "\">" + Str.Jam(r.LastLogin.Value) + "</p>";
		}

		return x;
	}
	protected string TD_Password(UserAccount r) {
		string x = string.Empty;


        if (r.Blokir.HasValue)
        {
            x += "<span class='label label-danger'>Blokir Account</span>";
        }
        else
        {
            if (r.Recycle)
            {
                x += "<span class='label label-warning'>Wajib Ganti</span>";
            }
            else
            {
                if (r.PassStrong)
                {
                    x += "<span class='label label-success'>Baik</span>";
                }
                else
                {
                    x += "<span class='label label-warning'>Kurang Baik</span>";
                }
                
            }
        }
		

		return x;
	}
}