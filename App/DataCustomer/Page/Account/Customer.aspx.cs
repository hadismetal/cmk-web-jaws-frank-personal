using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Page_Account_Customer : System.Web.UI.Page {
    private Map_DB _db;
    private Map_CS _cs;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//DLC
			//DLC.CompanyBrand(brand);
			//DLC.DisableDraft(disable);

			//Form-Default
			//disable.SelectedIndex = 1;

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
		//href += "&brand=" + brand.SelectedIndex;
		//href += "&disable=" + disable.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			//brand.SelectedIndex = Convert.ToInt32(Request.QueryString["brand"]);
			//disable.SelectedIndex = Convert.ToInt32(Request.QueryString["disable"]);
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) 
    {
        if (kw.Text.Trim().Length > 2)
        {
            //WHERE
            string w = "1=1";
            //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
            //if (disable.SelectedIndex == 1) w += " AND Draft = False AND Disable = False";
            //if (disable.SelectedIndex == 2) w += " AND Draft = False AND Disable = True";
            //if (disable.SelectedIndex == 3) w += " AND Draft = True";

            //ORDER BY
            string o = order.SelectedValue + " " + orderby.SelectedValue;

            //SELECT
            var rs = from p in db.DataCustomer.Where(w).OrderBy(o)
                     where
                        (
                        p.Nama.Contains(kw.Text)
                        || p.NoCustomer.Contains(kw.Text)
                        )
                     select p;


            //int pageNumber = 1;
            //int numberOfObjectsPerPage = 30;

            //if (Request.QueryString["page"] != "")
            //    pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            //if (pageNumber < 0)
            //    pageNumber = 1;

            //rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

            //NO-RESULT
            if (rs.Count() == 0) { App.TableNoResult(tb); }
            else
            {
                int max_data = 100;
                if (rs.Count() > max_data) { lit_countcustomer.Text = "Showing " + max_data.ToString() + " / " + rs.Count() + " data"; rs = rs.Take(max_data); }

                List<string> listcust = new List<string>();
                listcust = rs.Select(p => p.NoCustomer).ToList();
                
                var rs_cs = from p in cs.Mscustomer where listcust.Contains(p.M_kode) select p;
                var rs_cs_merge = from p in cs.Mscustomermerge where listcust.Contains(p.M_kode) select p;

                string xstatus_dbcmk = "";
                string xstatus_merge = "";

                //RESULT
                int index = 1;
                foreach (var r in rs)
                {
                    string kodecust = r.NoCustomer;
                    xstatus_dbcmk = "";
                    xstatus_merge = "";

                    var rs_status_dbcmk = rs_cs.SingleOrDefault(p => p.M_kode == kodecust);
                    var rs_status_merge = rs_cs_merge.SingleOrDefault(p => p.M_kode == kodecust);

                    if (rs_status_dbcmk != null)
                    {
                        xstatus_dbcmk = "<span style='font-size:28px; color:green;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                    }
                    else
                    {
                        xstatus_dbcmk = "<span style='font-size:28px; color:red;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>";
                    }

                    if (rs_status_merge != null)
                    {
                        xstatus_merge = "<span style='font-size:28px; color:green;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>";
                    }

                    if (!Response.IsClientConnected) break;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Nama(r);
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Tgl(r);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Status(xstatus_dbcmk);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Status(xstatus_merge);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    //td = new TableCell();
                    //td.Text = TD_Addr(r);
                    //tr.Cells.Add(td);

                    //td = new TableCell();
                    //td.Text = TD_Brand(r);
                    //td.CssClass = "text-center";
                    //tr.Cells.Add(td);

                    //         td = new TableCell();
                    //         td.Text = Convert.ToDecimal(r.TotalReward).ToString("N0");
                    //         td.CssClass = "text-center";
                    //         tr.Cells.Add(td);

                    //         td = new TableCell();
                    //         td.Text = Convert.ToDecimal(r.RewardTaken).ToString("N0");
                    //         td.CssClass = "text-center";
                    //         tr.Cells.Add(td);

                    //         td = new TableCell();
                    //         td.Text = Convert.ToDecimal(r.RewardRemaining).ToString("N0");
                    //         td.CssClass = "text-center";
                    //         tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            //href += "&brand=" + brand.SelectedIndex;
            //href += "&disable=" + disable.SelectedIndex;
            href += "&order=" + order.SelectedIndex;
            href += "&orderby=" + orderby.SelectedIndex;
            //href += "&page=" + (pageNumber + 1).ToString();

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            //href2 += "&brand=" + brand.SelectedIndex;
            //href2 += "&disable=" + disable.SelectedIndex;
            href2 += "&order=" + order.SelectedIndex;
            href2 += "&orderby=" + orderby.SelectedIndex;
            //href2 += "&page=" + (pageNumber - 1).ToString();

            //string paging = "";

            //paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            //paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            //paging += "<a href=\"" + href + "\">Next</a>";
            //paging += "</div>";

            //paging_text.Text = paging;
        }
        else
        {
            lit_countcustomer.Text = "<span style='color:red;'>Search minimum karakter 2 digit</span>";
        }
	}
	protected string TD_HyperLink(DataCustomer r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"CustomerFile.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";

		//if (Role.Operator || Role.Manager)
  //      {
		//	x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"CustomerUpdate.aspx?id=" + r.ID + "\">";
		//	x += "      <i class=\"icon-pencil\"></i> ";
		//	x += "      Update";
		//	x += "  </a>";
		//}
		//else
  //      {
		//	x += "  <a class=\"btn btn-default\" target=\"_blank\" disabled=\"disabled\">";
		//	x += "      <i class=\"icon-pencil\"></i> ";
		//	x += "      Update";
		//	x += "  </a>";
		//}

		x += "</div>";

		return x;
	}
	protected string TD_Nama(DataCustomer r) {
		string x = string.Empty;

        string xnama = r.Nama.ToUpper();
        string xkeyword = Request.QueryString["kw"].ToUpper();
        string xkodecust = r.NoCustomer;

        x += "<p><strong>" + xnama.Replace(xkeyword, ("<span style='color:blue;'>" + xkeyword + "</span>")) + "</strong></p>";
        if (r.NoCustomer != "")
            x += "<p class=\"text-muted\">" + xkodecust.Replace(xkeyword, ("<span style='color:blue;'>" + xkeyword + "</span>")) + "</p>";
        //if (r.Keterangan != "")
        //    x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
	protected string TD_Tgl(DataCustomer r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Status(string xstatus) {
		string x = string.Empty;

        //if (r.Draft)
        //    x += "<p class=\"text-draft\">" + DL.DisableDraft() + "</p>";
        //else if (r.Disable)
        //    x += "<p class=\"text-warning\">" + DL.Disable(r.Disable) + "</p>";

        x += xstatus;

		return x;
	}
	protected string TD_Addr(DataCustomer r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Brand(DataCustomer r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.CompanyBrand.NamaKode + "</p>";

		return x;
	}
}