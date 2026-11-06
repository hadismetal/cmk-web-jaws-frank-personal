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

            //datepicker.Text = DateTime.Now.ToString("MM-yyyy");
            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);

            //samp.Visible = false;
		}
        //samp.Visible = false;
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e)
    {
        //if (kw.Text != "" || kw.Text.Length > 0)
        //{
        DateTime start;
        DateTime end;
        DateTime dchoose;
        //SELECT
        //var rs = (from p in db.StockActualDJ
        //          join p1 in db.StockRetireDJ on p.IDProduct equals p1.IDProduct 
        //          into p2
        //          from p3 in p2.Where(a => (a.Status !=2) && a.Approval == null).OrderByDescending(a => a.ID).DefaultIfEmpty()
        //          where p.StockProductDJ.Draft == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0 
        //          && p.StockProductDJ.StatusLebur == false && p.Substorage == 6 
        //          select p);          

        //if (datepicker.Text != "" && datepicker2.Text != "")
        //{

        //while using month to pick
        //dchoose = Convert.ToDateTime(datepicker.Text);
        //start = new DateTime(dchoose.Year, dchoose.Month, 1);
        //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
        start = Convert.ToDateTime(datepicker.Text);
        end = Convert.ToDateTime(datepicker2.Text);

        var rs = (from p in db.StockActualDJ
                    join pp in db.StockIncomingDJ_Product on p.IDProduct equals pp.IDProduct
                    into pptemp
                    from pp1 in pptemp.DefaultIfEmpty()
                    join p1 in db.StockRetireDJ on p.IDProduct equals p1.IDProduct
                    into p2
                    from p3 in p2.Where(a => a.Status != 2 && a.Approval == null).OrderByDescending(a => a.ID).DefaultIfEmpty()
                    where p.StockProductDJ.Draft == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0
                        && p.StockProductDJ.StatusLebur == false && p.Substorage == 6
                        && pp1.ApprovalTgl >= start && pp1.ApprovalTgl < end.AddDays(1)
                    select p);

            if (kw.Text != "" || kw.Text.Length > 0)
                {
                    rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
                }
            //}
            if (rs.Count() == 0) App.TableNoResult(tb);

            if (rs != null)
            {
                rs = rs.OrderBy(p => p.StockProductDJ.LocWarehouse.Nama).ThenBy(p => p.StockProductDJ.ID);

               

                //NO-RESULT
                //zero.Visible = rs.Count() == 0;
                //form_area.Visible = !zero.Visible;

                //RESULT
                int prev = 0;
                int index = 1;
                string previous = "";
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;

                    //Sub-Header
                    if (prev != r.StockProductDJ.IDWarehouse.Value)
                    {
                        //Sub_Header(r.StockProductDJ);
                        prev = r.StockProductDJ.IDWarehouse.Value;
                    }
                    
                    if(previous != r.Nomor)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(r.StockProductDJ);
                        //td.Text = "";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Image(r.StockProductDJ);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nomor(r.StockProductDJ);
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Data(r.StockProductDJ);
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);

                        if (r.StockProductDJ.StockProductDJ_CharProduct != null)
                        {
                            td = new TableCell();
                            td.Text = TD_Product01(r.StockProductDJ.StockProductDJ_CharProduct);
                            td.CssClass = "text-left";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = TD_Product02(r.StockProductDJ.StockProductDJ_CharProduct);
                            td.CssClass = "text-middle";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = TD_Product03(r.StockProductDJ.StockProductDJ_CharProduct);
                            td.CssClass = "text-center";
                            tr.Cells.Add(td);
                        }

                        td = new TableCell();
                        td.Text = r.StockProductDJ.LocWarehouse.Nama + " - " + r.StockProductDJ.LocWarehouse.CompanyBrand.Nama;
                        tr.Cells.Add(td);
                        td.CssClass = "text-left";
                        tb.Rows.Add(tr);

                        previous = r.Nomor;
                    }
                }
            }
          
        //}
        //else
        //{
        //    lb_valid.Text = "mohon isi no plu";
        //}

	}
	protected void Sub_Header(StockProductDJ r) {
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
	protected string TD_HyperLink(StockProductDJ r)
    {
		string x = string.Empty;

        var cek = db.StockRetireDJ.OrderByDescending(p => p.ID).FirstOrDefault(p => p.IDProduct == r.ID);

        if (cek == null)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Submit.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Open";
            x += "</a>";            
        }
        else
        {
            if (cek.Status == 2)
            {
                var cek2 = from p in db.StockIncomingDJ_Product where p.IDProduct == cek.IDProduct && p.ApprovalTgl > cek.ApprovalTgl select p;
                if (cek2 != null)
                {
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Submit.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-search\"></i> ";
                    x += "  Open";
                    x += "</a>";
                }
                else
                { x += ""; }
            }
            else
            {
                x += "<a class=\"btn btn-primary\" target=\"_blank\" href=\"SPL_DJ.aspx?id=" + cek.ID + "&status=1" + "\">";
                x += "  <i class=\"icon-print\"></i> ";
               // x += "  Print with pic";
                x += "</a>";

                x += " &nbsp; <a class=\"btn btn-primary\" target=\"_blank\" href=\"SPL_DJ.aspx?id=" + cek.ID + "&status=2" + "\">";
                x += "  <i class=\"icon-file\"></i> ";
                //x += "  Print no pic";
                x += "</a>";
            }
        }
		return x;
	}
	protected string TD_Nomor(StockProductDJ r)
    {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}
	protected string TD_Operator(StockProductDJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Data(StockProductDJ r) {
		string x = string.Empty;

		if (r.ProtoNomor != "" && r.ProtoNomor != null)
			x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

		if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
			x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

		if (r.SupplierNomor != "" && r.SupplierNomor != null)
			x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

		return x;
	}
	protected string TD_Product01(StockProductDJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-bold text-primary\">";
		x += r.CharProductItem.Nama;
		x += "</p>";

		return x;
	}
	protected string TD_Product02(StockProductDJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductCategory.NamaKode;
		x += "</p>";

		return x;
	}
	protected string TD_Product03(StockProductDJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductLevel.NamaKode;
		x += "</p>";

		return x;
	}
    protected string TD_Image(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //while using month to pic
        //DateTime start;
        //DateTime end;
        //DateTime dchoose;
        //dchoose = Convert.ToDateTime(datepicker.Text);
        //start = new DateTime(dchoose.Year, dchoose.Month, 1);
        //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));

        href += "&kw=" + kw.Text;
        //href += "&dari=" + start.ToShortDateString();
        //href += "&sampai=" + end.ToShortDateString();
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];

            //while using month to pic
            //DateTime start;
            //DateTime end;
            //DateTime dchoose;
            //dchoose = Convert.ToDateTime(Request.QueryString["dari"]);            
            //start = new DateTime(dchoose.Year, dchoose.Month, 1);
            //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
            //datepicker.Text = dchoose.ToString("MM-yyyy");
            
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }
}