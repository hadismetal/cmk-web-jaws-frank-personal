using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_DJ_Detail : System.Web.UI.Page {
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

			//Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        string kode = kw.Text;
       
        start = Convert.ToDateTime(datepicker.Text);
        end = Convert.ToDateTime(datepicker2.Text);


        //RESULT
        //int prev = 0;
        int index = 1;

        string query = @"SELECT A.ID, A.Nomor, SUM(E.Qty) as Qty, A.Operator, A.OperatorTgl FROM StockRetireDJ_WorkOrder A 
                        JOIN StockRetireDJ B ON A.ID = B.IDWO 
                        JOIN (SELECT ID, COUNT(ID) + dbo.fx_get_qty_retire_ld(ID) as Qty FROM StockRetireDJ WHERE IDWO IS NOT NULL GROUP BY ID, ID) E ON B.ID = E.ID
                        JOIN StockProductDJ C ON B.IDProduct = C.ID
                        LEFT JOIN StockRetireDJLD D ON B.ID = D.IDForm
                        WHERE A.OperatorTgl > '" + start.ToString("yyyy-MM-dd") + "' AND A.OperatorTgl < '" + end.AddDays(1).ToString("yyyy-MM-dd") + "' AND " +
                        "(A.Nomor like '%" + kw.Text + "%' OR A.ID in " +
                        "   (SELECT C.ID FROM StockProductDJ A JOIN StockRetireDJ B ON A.ID = B.IDProduct " +
                        "   JOIN StockRetireDJ_WorkOrder C ON B.IDWO = C.ID " +
                        "   WHERE A.Nomor = '" + kw.Text + @"'))
                        GROUP BY A.ID, A.Nomor, A.Operator, A.OperatorTgl
                        ORDER BY A.ID, A.Nomor, A.Operator, A.OperatorTgl";
        //int qty;

        var dt = Db.Rs(query, Db.Cnn_DB);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(row["ID"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Qty"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Operator"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["OperatorTgl"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
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
    protected string TD_HyperLink(int ID)
    {
		string x = string.Empty;

        
		x += "<div class='row' style='margin: 0 !important; text-align:center'><a class=\"btn btn-info\" style='margin:0 2px' target=\"_blank\" href=\"DJ_WorkOrderDetail.aspx?id=" + ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "</a>";
        x += "<a class=\"btn btn-info\" style='margin:0 2px' target=\"_blank\" href=\"DJ_PrintWO.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-print\"></i> ";
        x += "</a></div>";

        return x;
	}

    protected string TD_Print(int ID)
    {
        string x = string.Empty;


        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_PrintWO.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Detail";
        x += "</a>";

        return x;
    }
    protected string TD_Nomor_Dokumen(string Nomor)
    {
		string x = string.Empty;

		x += Nomor;

		return x;
	}
    protected string TD_Nomor(StockProductDJ r)
    {
        string x = string.Empty;

        x += r.Nomor;

        return x;
    }
    protected string TD_Operator(StockProductDJ r)
    {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
    protected string TD_Data(StockProductDJ r)
    {
		string x = string.Empty;

		if (r.ProtoNomor != "" && r.ProtoNomor != null)
			x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

		if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
			x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

		if (r.SupplierNomor != "" && r.SupplierNomor != null)
			x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

		return x;
	}
    protected string TD_Product01(StockProductDJ_CharProduct r)
    {
		string x = string.Empty;

		x += "<p class=\"text-bold text-primary\">";
		x += r.CharProductItem.Nama;
		x += "</p>";

		return x;
	}
    protected string TD_Product02(StockProductDJ_CharProduct r)
    {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductCategory.NamaKode;
		x += "</p>";

		return x;
	}
    protected string TD_Product03(StockProductDJ_CharProduct r)
    {
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

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
            status.SelectedValue = Request.QueryString["status"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_Detail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}