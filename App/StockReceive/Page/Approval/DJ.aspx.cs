using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page {
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

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

            DLC.DDLSupplier(supplier, 0);

			//Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e)
    {
        string errMsg = "";

        string[] format = new string[] { "dd-MM-yyyy" };
        string dari = datepicker.Text;
        string sampai = datepicker2.Text;

        DateTime start;

        if (!(DateTime.TryParseExact(dari, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out start)))
        {
            errMsg = "Tanggal 'Dari' tidak sesuai format";
        }

        DateTime end;

        if (!(DateTime.TryParseExact(sampai, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out start)))
        {
            errMsg = "Tanggal 'Sampai' tidak sesuai format";
        }

        if(errMsg != "")
        {
            lit_msg.Text = "<span style='color:red;'>" + errMsg + "</span>";
        }
        else
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);

            //SELECT
            var rs = from p in db.StockReceiveDJ
                     where p.StatusReceive == 0
                        && p.Draft == false
                        && p.Tgl >= start
                        && p.Tgl < end.AddDays(1)
                        && (p.Nomor == null || p.Nomor == "")
                     // && !(from o in db.StockProductDJ
                     //      select o.Nomor)
                     //.Contains(p.Nomor)  

                     select p;

            //if(rs != null)
            //{

            //    var no = from p in rs
            //             where p.Nomor != null
            //                && p.Nomor != ""
            //             select p.Nomor;

            //    if (no != null)
            //    {
            //        var dj = from p in db.StockProductDJ
            //                 where no.ToList().Contains(p.Nomor)
            //                 select p.Nomor;

            //        rs = rs.Where(p => !dj.Contains(p.Nomor));
            //    }

            //}



            string keyword = kw.Text;
            if (keyword != "") rs = rs.Where(p => p.Nomor.Contains(keyword) || p.ProtoNomor.Contains(keyword) || p.SupplierNomor.Contains(keyword));

            //int Status = Convert.ToInt32(status.SelectedValue);
            //if (Status == 1)
            //    rs = rs.Where(p => p.Approval != null);
            //else if (Status == 2)
            //    rs = rs.Where(p => p.Approval == null);

            string Supplier = supplier.SelectedValue.ToString();
            
            if (Supplier != "" && Supplier != "0")
                rs = rs.Where(p => p.SupplierNama.Equals(Supplier));

            rs = rs.OrderBy(p => p.LocWarehouse.Nama).ThenBy(p => p.ID);

            //NO-RESULT
            zero.Visible = rs.Count() == 0;
            form_area.Visible = !zero.Visible;

            //RESULT
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header
                if (prev != r.IDWarehouse)
                {
                    Sub_Header(r);
                    prev = r.IDWarehouse;
                }

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = TD_Image(r);
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Operator(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Form(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nomor(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Human(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = (r.StockReceiveDJ_CharProduct == null) ? "" : TD_Product01(r.StockReceiveDJ_CharProduct);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (r.StockReceiveDJ_CharProduct == null) ? "" : TD_Product02(r.StockReceiveDJ_CharProduct);
                td.Style.Add("text-align","center");
                tr.Cells.Add(td);
                
                tb.Rows.Add(tr);
            }
        }
        
	}
	protected void Sub_Header(StockReceiveDJ r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(StockReceiveDJ r)
    {
		string x = string.Empty;

        if (r.Approval == null)
        {
            // proses pricing setelah register
            x += "<a class=\"btn btn-success\" target=\"_blank\" href=\"DJ_Form.aspx?id=" + r.ID + "\">";
            x += "<i class=\"icon-check\"></i>";
            //x += "&nbsp;Pricing";
        }
        else
        {
            // proses view detail karena sudah approve
            
            // kondisi jika error approve
            var sa = db.StockActualDJ.SingleOrDefault(p => p.Nomor == r.Nomor);
            var sp = db.StockProductDJ.SingleOrDefault(p => p.Nomor == r.Nomor);

            if (sa == null || sp == null)
            {
                x += "<a class=\"btn btn-danger\" target=\"_blank\" href=\"DJ_PricingSubmit.aspx?id=" + r.ID + "\">";
                x += "<i class=\"icon-wrench\"></i> ";
                //x += "&nbsp;Fix";
            }
            else
            {
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_PricingSubmit.aspx?id=" + r.ID + "\">";
                x += "<i class=\"icon-search\"></i> ";
               // x += "&nbsp;Open";
            }
            
        }
        x += "</a>";

		return x;
	}
	protected string TD_Nomor(StockReceiveDJ r) {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}
	protected string TD_Operator(StockReceiveDJ r)
    {
		string x = string.Empty;
        string appopr = "";
        string apptgl = "";

        if (r.Approval != null & r.ApprovalTgl != null)
        {
            appopr = r.Approval;
            apptgl = Str.FullDateJam(r.ApprovalTgl);
        }

        x += "<span class='text-sm'>" + r.Operator + "<br />" + Str.FullDateJam(r.OperatorTgl) + "</span>"
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + "<span class='text-sm'>" + appopr + "<br />" + apptgl + "</span>"
            ;


  //      x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		//x += "<p class=\"text-sm\">" + Str.FullDateJam(r.OperatorTgl) + "</p>";

		return x;
	}

	protected string TD_Form(StockReceiveDJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + DL.SumberStockReceive(r.Sumber) + "</strong></p>";
		//x += "<p class=\"text-sm\">" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Data(StockReceiveDJ r) {
		string x = string.Empty;

		if (r.ProtoNomor != "" && r.ProtoNomor != null)
			x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

		if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
			x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

		if (r.SupplierNomor != "" && r.SupplierNomor != null)
			x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

		return x;
	}
	protected string TD_Human(StockReceiveDJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\">" + r.SupplierNama + "</p>";

		return x;
	}

	protected string TD_Product01(StockReceiveDJ_CharProduct r)
    {
		string x = string.Empty;

		//x += "<p class=\"text-bold text-primary\">";
		//x += r.CharProductItem.Nama;
		//x += "</p>";

        x += r.CharProductItem.Nama
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + r.CharProductCategory.Nama
            ;


        return x;
	}

	protected string TD_Product02(StockReceiveDJ_CharProduct r)
    {
		string x = string.Empty;

        x += r.CharProductLevel.Nama
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + r.CharStoneDist.Nama
            ;

        return x;
	}
	
    protected string TD_Image(StockReceiveDJ r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        //href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
		string temp = supplier.SelectedValue.Replace("&", "%26");
        href += "&supplier=" + temp;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
            //status.SelectedValue = Request.QueryString["status"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
			string temp = Request.QueryString["supplier"];
            supplier.SelectedValue = temp;
        }
    }
}