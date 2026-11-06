using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataCustomer : System.Web.UI.Page
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

    private Map_CS _cs;

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
	protected int myID { get { return App.QueryInt(this); } }
    protected int myTipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
            //Header
            //header_nomor.InnerHtml =
            //    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //    + d.Nomor + "</a>";

            //Hidden-ID
            //idbrand.Text = d.DataCustomer.IDBrand.ToString();
            string qstr = "?id=" + myID + "&tipe=" + myTipe;
            cancel.HRef += qstr;
        }
	}

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + myTipe;
        //href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            //brand.SelectedValue = Request.QueryString["brand"];
        }
    }

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        if (kw.Text.Trim().Length >= 3)
        {
            //var qr1 = (from p in cs.Mscustomer select p).ToList();
            /*
            var rs = (from p in db.DataCustomer
                      //join p3 in qr1 on p.NoCustomer equals p3.M_kode
                      join p1temp in db.SalesReceiptDPPO on p.ID equals p1temp.IDCustomer into temp
                      from p2 in temp.Where(x => x.IDSalesOrder == null
                                                  && x.Draft == false
                                                  && x.Tgl > DateTime.Now.AddMonths(-6)
                                          ).DefaultIfEmpty()
                      where
                       p.Disable == false && p.Draft == false
                      && ((p.Nama.Contains(kw.Text.Trim()))
                      //|| (p2.NomorDP.Contains(kw.Text.Trim()))
                      || (p.NoCustomer.Contains(kw.Text.Trim()))
                      //|| (p3.M_member.Contains(kw.Text.Trim()))
                      )
                      orderby p.Nama
                      select new { p.ID, p.Nama, p2.NomorDP, p2.Tgl, p.NoCustomer });
            */
            var rs = (from p in cs.Mscustomer
                      where 
                            p.M_nama.Contains(kw.Text.Trim())
                            || p.M_member.Contains(kw.Text.Trim())
                            || p.M_kode.Contains(kw.Text.Trim())
                       orderby p.M_nama
                      select p);


            //NO-RESULT
            if (rs.Count() == 0)
                App.TableNoResult(tb);

            //RESULT
            //if (rs.Count() > 10) { lit_countcustomer.Text = "Showing 10 / " + rs.Count() + " data"; rs = rs.Take(10); }

            string nocust_prev = "";
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                if (r.M_kode != nocust_prev)
                {

                    td = new TableCell();
                    //td.Text = TD_HyperLink(r.ID);
                    td.Text = TD_HyperLink(r.M_kode); 
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class='text-muted pull-left' style='text-align:left;'>" + r.M_kode + "</span>"
                               + ((r.M_member != null)
                               ? ("<span class='text-muted pull-right' style='text-align:left; color:green;'>" + r.M_member + "</span>") : "")
                               + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                               + "<span class='pull-left' style='text-align:left;'><b>" + r.M_nama + "</b></span>";
                    tr.Cells.Add(td);
                     
                    //if (r.NomorDP == null || r.NomorDP == "")
                    //{
                    //    td = new TableCell();
                    //    td.Text = "";
                    //    tr.Cells.Add(td);
                    //}
                    //else
                    //{
                    //    td = new TableCell();
                    //    td.Text = "<span class='text-muted pull-left' style='text-align:left;'>"
                    //            + r.NomorDP + " (" + Str.FullDate(r.Tgl) + ") </span>";
                    //    tr.Cells.Add(td);
                    //}


                    //td = new TableCell();
                    ////td.Text = String.Format("{0:D}", r.Tgl);
                    //td.Text = Str.FullDate(r.Tgl);
                    //td.CssClass = "text-center";
                    //tr.Cells.Add(td);                
                }

                //else if (r.M_kode == nocust_prev)
                //{
                //    td = new TableCell();               
                //    td.Text = "";
                //    td.ColumnSpan = 3;
                //    tr.Cells.Add(td);

                //    if (r.NomorDP != null || r.NomorDP != "" || r.NomorDP != " ")
                //    {
                //        td = new TableCell();
                //        td.Text = "<span class='text-muted pull-left' style='text-align:left;'>"
                //                + r.NomorDP + " (" + Str.FullDate(r.Tgl) + ") </span>";
                //        tr.Cells.Add(td);
                //    }
                //}

                nocust_prev = r.M_kode;
                tb.Rows.Add(tr);
            }
        }
        else
        {
            lit_countcustomer.Text = "<span style='color:red;'>Minimal 3 karakter Nama / Kode Customer  / No. member!</span>";
        }
    }
	protected void Sub_Header(DataCustomer r) {
		/*TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

        string Username = Db.SingleString("SELECT TOP 1 Nama FROM UserAccount WHERE ID = " + r.IDUser, Db.Cnn_AC);

		x += "<br />";
		x += r.Nama + " (" + Username + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);*/
	}
    //protected string TD_HyperLink(DataCustomer r) {
    //    string x = string.Empty;

    //    x += "<a class=\"btn btn-info\" href=\"SPK_DataCustomerSave.aspx?id=" + myID + "&link=" + r.ID + "&tipe=" + myTipe + "\">";
    //    x += "  <i class=\"icon-ok\"></i> ";
    //    x += "  Select";
    //    x += "</a>";

    //    return x;
    //}
    protected string TD_HyperLink(string nocust)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"SPK_DataCustomerSave.aspx?id=" + myID + "&idcust=" + nocust + "&tipe=" + myTipe + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(DataCustomer r)
    {
        string x = string.Empty;

        string Nama = "";

        Nama = r.Nama;

        x += "<p class=\"text-sm\">" + Nama + "</p>";

        return x;
    }
	protected string TD_Addr(DataCustomer r) {
		string x = string.Empty;

        string Email = "";

        Email = r.AddrEmail;

		x += "<p class=\"text-sm\">" + Email + "</p>";

		return x;
	}
    protected string TD_Group(DataCustomer r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.CompanyBrand.Nama + "</p>";

        return x;
    }
}