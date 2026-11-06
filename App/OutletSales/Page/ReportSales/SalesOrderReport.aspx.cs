using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class SalesOrderReport : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    string Module = "pos"; // pos, acc

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }
	
	//Display Table
	protected void fillTableNew(object sender, EventArgs e)
	{
		string kw_text = kw.Text;
		DateTime start_text = Convert.ToDateTime(dari.Text);
		DateTime end_text = Convert.ToDateTime(sampai.Text);
		string status_text = status.SelectedValue;
		
		string location = getLocationUser();

		string query = "sp_report_sales_order_new";

		List<SqlParameter> parameters = new List<SqlParameter>();
		parameters.Add(new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text });
		parameters.Add(new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text });
		parameters.Add(new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_text });
		parameters.Add(new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = location });
		parameters.Add(new SqlParameter("@STATUS", SqlDbType.Int) { Value = status_text });
		parameters.Add(new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 });
		parameters.Add(new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 });
		parameters.Add(new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 1 });

		DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, parameters);
		DataTable data = ds.Tables[0];
		int datacount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

		decimal TotalBayar = 0m;
		decimal TotalPaid = 0m;
		decimal TotalUnpaid = 0m;
		decimal TotalQty = 0m;

		if (datacount > 0)
		{
			string ti_nomor = "";
			string ti_sisa_resell = "";

			int prev = 0;
			int index = 1;
			foreach (DataRow r in data.Rows)
			{
				ti_nomor = "";
				ti_sisa_resell = "";

				decimal nilaiResell = 0m;
				decimal nilaiHarusDibayar = 0m;

				string rTradeIn = r["NOMORTI"].ToString();
				decimal rTotalBayar = Convert.ToDecimal(r["TOTALBAYAR"]);
				decimal rTotalResell = Convert.ToDecimal(r["TOTALRESELL"]);
				string rNomor = r["NOMORSO"].ToString();
				bool rStatusVoid = Convert.ToBoolean(r["STATUSVOID"]);
				bool rStatusPembayaran = Convert.ToBoolean(r["STATUSPEMBAYARAN"]);
				bool rDraft = Convert.ToBoolean(r["DRAFT"]);
				DateTime rTgl = Convert.ToDateTime(r["TGLSO"]);
				string rCustomerNama = r["CUSTNAMA"].ToString();
				string rSalesNama = r["SALESNAMA"].ToString();
				string rKeterangan = r["KETERANGAN"].ToString();
				string rKeteranganVoid = r["KETERANGANVOID"].ToString();

				int ID = Convert.ToInt32(r["ID"]);
				decimal Harga = Convert.ToDecimal(r["TOTALBAYAR"]);
				decimal Paid = Convert.ToDecimal(r["TERBAYAR"]);
				decimal Unpaid = Convert.ToDecimal(r["SISABAYAR"]);
				int Qty = Convert.ToInt32(r["QTY"]);

				if (!string.IsNullOrEmpty(rTradeIn))
				{
					ti_nomor = "<br /><span style='color:blue;'>" + rTradeIn + "</span>";
					nilaiResell = rTotalResell;
					nilaiHarusDibayar = rTotalBayar;
					ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
				}

				if (!Response.IsClientConnected) break;

				//Sub-Header

				TableRow tr = new TableRow();
				TableCell td;

				td = new TableCell();
				td.Text = TD_HyperLinkNew(ID, rStatusVoid, rStatusPembayaran, rDraft, rNomor, rTgl);
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = "<p>" + index++.ToString() + ".</p>";
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.FullDate(rTgl);
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = rNomor + ti_nomor;
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = rCustomerNama;
				td.Style.Add("text-align", "left");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = rSalesNama;
				td.Style.Add("text-align", "left");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Qty.ToString();
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = Harga.ToString("N0") + ti_sisa_resell;
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Paid.ToString("N0");
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Unpaid.ToString("N0");
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.HtmlBreak(rKeterangan);
				td.Style.Add("text-align", "left");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = rKeteranganVoid;
				td.Style.Add("text-align", "left");
				tr.Cells.Add(td);

				tb.Rows.Add(tr);

				TotalBayar += Harga;
				TotalUnpaid += Unpaid;
				TotalPaid += Paid;
				TotalQty += Qty;
			}
		}
		else
		{
			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.ColumnSpan = 12;
			td.Text = "No Data";
			td.Style.Add("text-align", "center");
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}

		TableRow tr2 = new TableRow();
		TableCell td2;

		td2 = new TableCell();
		td2.Text = "<b>Total</b>";
		td2.ColumnSpan = 6;
		tr2.Cells.Add(td2);

		td2 = new TableCell();
		td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
		td2.Style.Add("text-align", "center");
		tr2.Cells.Add(td2);

		td2 = new TableCell();
		td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
		tr2.Cells.Add(td2);

		td2 = new TableCell();
		td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
		tr2.Cells.Add(td2);

		td2 = new TableCell();
		td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
		tr2.Cells.Add(td2);

		td2 = new TableCell();
		td2.Text = "";
		td2.ColumnSpan = 2;
		tr2.Cells.Add(td2);

		tb.Rows.Add(tr2);
	}

	protected string getLocationUser()
	{
		string result = "";
		int xUserID = App.UserID;
		int xisAM = 0;
		string listStore = "";
		//check area manager
		var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
		int amcount = am.Count();
		if (amcount > 0)
		{
			int i = 0;
			foreach (var x in am)
			{
				int j = 0;
				var lokasi = x.DataAreaManager_Lokasi
					.GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
					.Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
				foreach (var y in lokasi)
				{
					//listStore = "";
					if (y.TipeLokasi == 1)
						listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
					else
						listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
					j++;
					listStore += ",";
				}
				i++;
			}
			xisAM = 1;
		}

		if (xisAM == 0)
		{
			//check admin sales
			var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
			if (ad.Count() > 0)
			{
				int k = 0;
				foreach (var x in ad)
				{
					listStore = "";
					if (x.TipeLokasi == 1)
						listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
					else
						listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
					k++;
					listStore += ",";
				}
			}
		}

		result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

		return result;
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrder
                 where !(from z in db.SalesOrderDJ
                         where z.StockProductDJ.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.ID)
                         && !(from z in db.SalesOrderPG
                              where z.StockProductPG.Legacy.Value == 1
                              select z.SalesOrder.ID).Contains(p.ID)
                         && (p.SalesOrderDJ.Count() > 0 || p.SalesOrderGJ.Count() > 0 || p.SalesOrderLD.Count() > 0 || p.SalesOrderPG.Count() > 0)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "")
        {
             rs = from p in rs
                 where p.Nomor.Contains(keyword)
                        || p.SalesOrderDJ.Any(q => q.StockProductDJ.Nomor == kw.Text)
                        || p.SalesOrderGJ.Any(q => q.StockProductGJ.Nomor == kw.Text)
                        || p.SalesOrderPG.Any(q => q.StockProductPG.Nomor == kw.Text)
                        || p.SalesOrderLD.Any(q => q.StockProductLD_Stone1B.Nomor == kw.Text)
                        || p.CustomerNama.Contains(kw.Text)
                 select p;
        }
        
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.StatusVoid == true);
        if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            string ti_nomor = "";
            string ti_keterangan = "";
            string ti_sisa_resell = "";

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                ti_nomor = "";
                ti_keterangan = "";
                ti_sisa_resell = "";

                decimal nilaiResell = 0m;
                decimal nilaiHarusDibayar = 0m;

                if (r.TradeIn.HasValue)
                {
                    var ti = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == r.ID);
                    if (ti != null)
                    {
                        ti_nomor = "<br /><span style='color:blue;'>" + ti.Nomor + "</span>";
                        ti_keterangan = ti.Keterangan;

                        try
                        {
                            nilaiResell = r.TotalResell.HasValue ? r.TotalResell.Value : 0m;
                        }
                        catch (Exception) { }

                        try
                        {
                            nilaiHarusDibayar = r.TotalBayar.HasValue ? r.TotalBayar.Value : 0m;
                        }
                        catch (Exception) { }

                        ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
                    }
                }

                decimal Harga = 0m;
                decimal Paid = 0m;
                decimal Unpaid = 0m;
                int Qty = 0;

                Qty = r.SalesOrderDJ.Count() + r.SalesOrderPG.Count() + r.SalesOrderGJ.Count() + r.SalesOrderLD.Count() ;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align","center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor + ti_nomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDCustomer.HasValue ? r.CustomerNama : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDSales.HasValue ? r.DataSales.Nama : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.TotalBayar.Value);
                }
                catch (Exception) { }

                try
                {
                    Paid = Convert.ToDecimal(r.Paid.Value);
                }
                catch (Exception) { }

                try
                {
                    Unpaid = Convert.ToDecimal(r.Unpaid.Value);
                }
                catch (Exception) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0") + ti_sisa_resell;
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Paid.ToString("N0");
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Unpaid.ToString("N0");
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.Keterangan) + Str.HtmlBreak(ti_keterangan);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KeteranganVoid != null ? Str.HtmlBreak(r.KeteranganVoid) : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalBayar += Harga;
                TotalUnpaid += Unpaid;
                TotalPaid += Paid;
                TotalQty += Qty;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesOrderReport_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
	protected string TD_HyperLinkNew(int ID, bool StatusVoid, bool StatusPembayaran, bool Draft, string Nomor, DateTime tglSO)
	{
		string x = string.Empty;        
        DateTime formattedDate = DateTime.Today;

        

		if (!(StatusVoid == false && StatusPembayaran == false && Draft == true))
		{
			x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SalesOrderReport_Detail.aspx?id=" + ID + "\">";
			x += "  <i class=\"icon-search\"></i> ";
			x += "  ";
			x += "</a>";
		}
		if (StatusVoid == false && Draft == false && tglSO >= formattedDate /*&& Payment.showVoidPaymentStatusGiftCard(r)*/)
		{
			// Gift Card Payment Void Warning
			x += "&nbsp;<a class=\"btn btn-danger\" href=\"SalesOrderReport_FormVoid.aspx?id=" + ID + "\">";
			x += "  <i class=\"icon-warning-sign\"></i> ";
			x += "  ";
			x += "</a>";
		}
		//SAP Void Tidak ada lagi cancel Sales Order
		else if (StatusVoid == false && StatusPembayaran == false && Draft == true && (Nomor == null || Nomor == ""))
		{
			x += "&nbsp;&nbsp;<a class=\"btn btn-warning\" onClick=\"return confirm('Cancel Sales Order, Are you Sure?');\" href=\"SalesOrderReport_Delete.aspx?id=" + ID + "\">";
			x += "  <i class=\"icon-remove\"></i> ";
			x += "  ";
			x += "</a>";
		}
		else if (StatusVoid == false && StatusPembayaran == false && Draft == true)
		{
			x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Void Sales Order, Are you Sure?');\" href=\"SalesOrderReport_FormVoid.aspx?id=" + ID + "\">";
			x += "  <i class=\"icon-trash\"></i> ";
			x += "  ";
			x += "</a>";
		}
		return x;
	}
    protected string TD_HyperLink(SalesOrder r)
    {
        string x = string.Empty;

        if (!(r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true))
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SalesOrderReport_Detail.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }

        if (r.StatusVoid == true && r.Draft == false && Payment.showVoidPaymentStatusGiftCard(r))
        {
            // Gift Card Payment Void Warning
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" href=\"SalesOrderReport_FormVoid.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-warning-sign\"></i> ";
            x += "  ";
            x += "</a>";
        }
        //SAP Void Tidak ada lagi cancel Sales Order
        else if (r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true && (r.Nomor == null || r.Nomor == ""))
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-warning\" onClick=\"return confirm('Cancel Sales Order, Are you Sure?');\" href=\"SalesOrderReport_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-remove\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (VoidValidation.validVoidSalesOrderRole(r, Module)
            || (r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true)
            )
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Void Sales Order, Are you Sure?');\" href=\"SalesOrderReport_FormVoid.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  ";
            x += "</a>";
        }
        
        return x;
    }
}