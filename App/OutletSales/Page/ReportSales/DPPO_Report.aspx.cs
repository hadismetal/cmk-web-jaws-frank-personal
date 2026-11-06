using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class Page_ReportSales_DPPO : System.Web.UI.Page
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
    protected string mode = "pos"; // pos, acc

    public int[] locationAM { get; set; }
    public int locationAE_Tipe { get; set; }
    public int locationAE_ID { get; set; }
    public int imode { get; set; }
    public string role { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }
    protected string validateReport(string TglDari, string TglSampai, string Location, string Mode)
    {
        string result = "";

        DateTime dDate;
        bool isValid_dateFrom = true;
        bool isValid_dateTo = true;

        if (result == "" && Mode != "pos" && !(new[] { "1" , "2" }.Contains(Location.Substring(0,1))))
        {
            result = "Lokasi harus dipilih. ";
        }

        if (result == "" && !(DateTime.TryParse(TglDari, out dDate)))
        {
            isValid_dateFrom = false;
            result = "Kolom tanggal 'From' tidak sesuai format (DD-MM-YYYY). ";
        }

        if (result == "" && !(DateTime.TryParse(TglSampai, out dDate)))
        {
            isValid_dateTo = false;
            result = "Kolom tanggal 'To' tidak sesuai format (DD-MM-YYYY). ";
        }

        if(result == "" && isValid_dateFrom && isValid_dateTo)
        {
            DateTime dtFrom = Convert.ToDateTime(TglDari);
            DateTime dtTo = Convert.ToDateTime(TglSampai).AddDays(1);

            if((dtTo - dtFrom).TotalDays > 31)
            {
                result = "Range tanggal tidak boleh lebih dari 31 hari.";
            }
        }

        return result;
    }
    
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        
        string TglDari = dari.Text.Trim();
        string TglSampai = sampai.Text.Trim();
        string keyword = kw.Text.Trim();
        int Status = Convert.ToInt32(status.SelectedValue);
        string xddlLocation = ddl_location.SelectedValue;

        string err = validateReport(TglDari, TglSampai, xddlLocation, mode);

        if (err != "")
        {
            lit_err.Text = "<div class='alert alert-danger' role='alert'>" + err + "<div>";
        }
        else
        {
            Button1.Disabled = false;
            List<SalesReceiptDPPO> rs = new List<SalesReceiptDPPO>();
            if (filterDate.SelectedValue.Equals("0"))
            {
                rs = (from p in db.SalesReceiptDPPO
                      where ((p.Tgl >= Convert.ToDateTime(TglDari) && p.Tgl < Convert.ToDateTime(TglSampai).AddDays(1)) || 
                      (p.Tgl < Convert.ToDateTime(TglSampai) && p.SalesOrder == null))
                      select p).ToList();
            }
            if (filterDate.SelectedValue.Equals("1"))
            {
                rs = (from p in db.SalesReceiptDPPO
                      where (p.SalesOrder.Tgl <= Convert.ToDateTime(TglSampai) && p.SalesOrder.Tgl >= Convert.ToDateTime(TglDari))
                      select p).ToList();
            }
            if (role == "AE")
            {
                rs = rs.Where(p => p.TipeLokasi == locationAE_Tipe && p.IDLokasi == locationAE_ID).ToList();
            }

            if (role == "AM")
            {
                rs = rs.Where(p => locationAM.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1).ToList();
            }

            if(new[] { "1", "2" }.Contains(xddlLocation.Substring(0, 1)))
            {
                int tipeLokasi = Convert.ToInt32(xddlLocation.Substring(0, 1));
                int idLokasi = 0;
                if (xddlLocation.Length == 2)
                    idLokasi = Convert.ToInt32(xddlLocation.Substring(1, 1));
                else if (xddlLocation.Length == 3)
                    idLokasi = Convert.ToInt32(xddlLocation.Substring(1, 2));
                else
                    idLokasi = Convert.ToInt32(xddlLocation.Substring(1, 3));

                rs = rs.Where(p => p.TipeLokasi == tipeLokasi && p.IDLokasi == idLokasi).ToList();
            }
            
            if (keyword != "") rs.Where(p => p.NomorDP == keyword || p.DataCustomer.Nama.Contains(keyword));

            if (Status == 0)
                rs = rs.Where(p => p.StatusDP == 0 && p.Draft == true).ToList();
            else if (Status == 1)
            {
                if (filterDate.SelectedValue.Equals("0"))
                {
                    rs = rs.Where(p => p.StatusDP == 0 && p.Draft == false).ToList();
                }
                else if (filterDate.SelectedValue.Equals("1"))
                {
                    rs = rs.Where(p => p.StatusDP == 1 && p.Draft == false).ToList();
                }

            }
            else if (Status == 2)
                rs = rs.Where(p => p.StatusDP == 1).ToList();
            else if (Status == 3)
                rs = rs.Where(p => p.StatusDP == 2).ToList();
            else if (Status == 9)
                rs = rs.Where(p => (p.StatusDP == 0 && p.Draft == false) || (p.StatusDP == 1) || (p.StatusDP == 2)).ToList();

            rs = rs.OrderByDescending(p => p.Tgl).ToList();

            if (rs.Count() > 0)
            {
                decimal Total = 0m;
                decimal TotalBayar1 = 0m;
                decimal TotalBayar2 = 0m;
                decimal TotalBayar3 = 0m;
                decimal TotalBayar4 = 0m;
                decimal TotalBayar10 = 0m;
                decimal total_estimasi = 0m;
                decimal t_per = 0m;

                int prev = 0;
                int index = 1;
                bool isFilled = false;
                foreach (var r in rs)
                {
                    if (r.Tgl < Convert.ToDateTime(TglDari))
                    {
                        break;
                    }
                    isFilled = true;
                    TableRow tr = new TableRow();
                    TableCell td;

                    string ndp = "";
                    if (r.IDDPPOReference != null)
                    {
                        ndp = (from p in db.SalesReceiptDPPO where p.ID == r.IDDPPOReference select p.NomorDP).SingleOrDefault().ToString();
                    }

                    td = new TableCell();
                    td.Text = TD_HyperLink(r);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (index++) + ".";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);    


                    td = new TableCell();
                    td.Text = (r.NomorDP == null ? "-" : r.NomorDP)
                            + (ndp != "" ? ("<div style='clear:both; border-bottom:solid 1px silver;'></div>" + "<span style='color: #6699FF'>" + ndp + "</span>") : "")
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + Str.FullDate(r.Tgl)
                            ;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);


                    string Lokasi = "";
                    if (r.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                    else if (r.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + r.SalesNama
                            ;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CustomerNama
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + r.DataCustomer.NoCustomer
                            ;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.IDSalesOrder.HasValue)
                            ?
                            (r.SalesOrder.Nomor
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + Str.FullDate(r.SalesOrder.Tgl)
                            )
                            : "-"
                            ;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.TotalBayar.HasValue ? r.TotalBayar.Value.ToString("N0") : "-")
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + ((r.EstimasiHarga == 0) ? "-" : "<span class='text-muted'>" + r.EstimasiHarga.ToString("N0") + "</span>")
                            ;
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    var so = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID.Equals(r.ID));

                    decimal T1 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal));
                    decimal T2 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal));
                    decimal T3 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal));
                    decimal T4 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal));
                    decimal T10 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal));

                    TotalBayar1 += T1;
                    TotalBayar2 += T2;
                    TotalBayar3 += T3;
                    TotalBayar4 += T4;
                    TotalBayar10 += T10;

                    td = new TableCell();
                    td.Text = "<table class='pull-right tbl-payment' style='width:100%;'>"
                            + (T1 <= 0 ? "" : "<tr><td style='text-align:left;'>CASH</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T1) + "</td></tr>")
                            + (T2 <= 0 ? "" : "<tr><td style='text-align:left;'>CREDIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T2) + "</td></tr>")
                            + (T3 <= 0 ? "" : "<tr><td style='text-align:left;'>TRANSFER</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T3) + "</td></tr>")
                            + (T4 <= 0 ? "" : "<tr><td style='text-align:left;'>DEBIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T4) + "</td></tr>")
                            + (T10 <= 0 ? "" : "<tr><td style='text-align:left;'>BRIDESTORY PAY</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T10) + "</td></tr>")
                            + "</table>"
                            ;
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);
					
					int lettercount = Db.SingleInteger("SELECT COUNT('') Total FROM LetterWR WHERE IDSalesReceiptDPPO = '" + r.ID.ToString() + "' ", Db.Cnn_DB);
                    string gwp = "";
                    if (lettercount == 1)
                    {
                        string letter = Db.SingleString("SELECT TOP 1 ISNULL(Letter,'') FROM LetterWR WHERE IDSalesReceiptDPPO = '" + r.ID.ToString() + "' ", Db.Cnn_DB);
                        gwp = "Promo Gift With Purchase Wedding Ring(Letter = "+letter+")";
                    }

                    td = new TableCell();
                    td.Text = Str.HtmlBreak(r.Keterangan + "." + gwp);
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Total += r.TotalBayar.HasValue ? r.TotalBayar.Value : 0;
                    total_estimasi += r.EstimasiHarga;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 6;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Total.ToString("N0") + "</b>"
                         + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                         + "<span class='text-muted'>" + total_estimasi.ToString("N0") + "</span>"
                         ;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<table class='pull-right tbl-payment' style='width:100%;'>"
                            + ("<tr><td style='text-align:left;'>CASH</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar1) + "</td></tr>")
                            + ("<tr><td style='text-align:left;'>CREDIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar2) + "</td></tr>")
                            + ("<tr><td style='text-align:left;'>TRANSFER</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar3) + "</td></tr>")
                            + ("<tr><td style='text-align:left;'>DEBIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar4) + "</td></tr>")
                            + ("<tr><td style='text-align:left;'>BRIDESTORY PAY</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar10) + "</td></tr>")
                            + "<tr><td colspan='3'><div style='clear:both; border-bottom:solid 1px silver;'></div></td></tr>"
                            + ("<tr><td style='text-align:left;'></td><td></td><td><b>" + Str.Number0(TotalBayar1 + TotalBayar2 + TotalBayar3 + TotalBayar4 + TotalBayar10) + "</b></td></tr>")
                            + "</table>"
                            ;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "";
                tr2.Cells.Add(td2);
                
                tb.Rows.Add(tr2);
				
				if (filterDate.SelectedValue.Equals("0"))
				{
					tb2.Visible = true;
				
					rs = rs.Where(p => p.Tgl < Convert.ToDateTime(TglDari)).ToList();

					foreach (var r in rs)
					{
						TableRow tr = new TableRow();
						TableCell td;

						string ndp = "";
						if (r.IDDPPOReference != null)
						{
							ndp = (from p in db.SalesReceiptDPPO where p.ID == r.IDDPPOReference select p.NomorDP).SingleOrDefault().ToString();
						}

						td = new TableCell();
						td.Text = TD_HyperLink(r);
						td.Style.Add("text-align", "center");
						tr.Cells.Add(td);

						td = new TableCell();
						td.Text = (index++) + ".";
						td.Style.Add("text-align", "center");
						tr.Cells.Add(td);


						td = new TableCell();
						td.Text = (r.NomorDP == null ? "-" : r.NomorDP)
								+ (ndp != "" ? ("<div style='clear:both; border-bottom:solid 1px silver;'></div>" + "<span style='color: #6699FF'>" + ndp + "</span>") : "")
								+ "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
								+ Str.FullDate(r.Tgl)
								;
						td.Style.Add("text-align", "center");
						tr.Cells.Add(td);


						string Lokasi = "";
						if (r.TipeLokasi == 1)
							Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
						else if (r.TipeLokasi == 2)
							Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

						td = new TableCell();
						td.Text = Lokasi
								+ "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
								+ r.SalesNama
								;
						td.Style.Add("text-align", "left");
						tr.Cells.Add(td);

						td = new TableCell();
						td.Text = r.CustomerNama
								+ "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
								+ r.DataCustomer.NoCustomer
								;
						td.Style.Add("text-align", "left");
						tr.Cells.Add(td);

						td = new TableCell();
						td.Text = (r.IDSalesOrder.HasValue)
								?
								(r.SalesOrder.Nomor
								+ "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
								+ Str.FullDate(r.SalesOrder.Tgl)
								)
								: "-"
								;
						td.Style.Add("text-align", "center");
						tr.Cells.Add(td);

						td = new TableCell();
						td.Text = (r.TotalBayar.HasValue ? r.TotalBayar.Value.ToString("N0") : "-")
								+ "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
								+ ((r.EstimasiHarga == 0) ? "-" : "<span class='text-muted'>" + r.EstimasiHarga.ToString("N0") + "</span>")
								;
						td.Style.Add("text-align", "right");
						tr.Cells.Add(td);

						var so = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID.Equals(r.ID));

						decimal T1 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal));
						decimal T2 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal));
						decimal T3 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal));
						decimal T4 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal));
						decimal T10 = Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal));

						TotalBayar1 += T1;
						TotalBayar2 += T2;
						TotalBayar3 += T3;
						TotalBayar4 += T4;
						TotalBayar10 += T10;

						td = new TableCell();
						td.Text = "<table class='pull-right tbl-payment' style='width:100%;'>"
								+ (T1 <= 0 ? "" : "<tr><td style='text-align:left;'>CASH</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T1) + "</td></tr>")
								+ (T2 <= 0 ? "" : "<tr><td style='text-align:left;'>CREDIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T2) + "</td></tr>")
								+ (T3 <= 0 ? "" : "<tr><td style='text-align:left;'>TRANSFER</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T3) + "</td></tr>")
								+ (T4 <= 0 ? "" : "<tr><td style='text-align:left;'>DEBIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T4) + "</td></tr>")
								+ (T10 <= 0 ? "" : "<tr><td style='text-align:left;'>BRIDESTORY PAY</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(T10) + "</td></tr>")
								+ "</table>"
								;
						td.Style.Add("text-align", "right");
						tr.Cells.Add(td);

						td = new TableCell();
						td.Text = Str.HtmlBreak(r.Keterangan);
						td.Style.Add("text-align", "left");
						tr.Cells.Add(td);

						tb2.Rows.Add(tr);

						Total += r.TotalBayar.HasValue ? r.TotalBayar.Value : 0;
						total_estimasi += r.EstimasiHarga;
					}

					tr2 = new TableRow();

					td2 = new TableCell();
					td2.Text = "<b>Total</b>";
					td2.ColumnSpan = 6;
					tr2.Cells.Add(td2);

					td2 = new TableCell();
					td2.Text = "<b>" + Total.ToString("N0") + "</b>"
							 + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
							 + "<span class='text-muted'>" + total_estimasi.ToString("N0") + "</span>"
							 ;
					tr2.Cells.Add(td2);

					td2 = new TableCell();
					td2.Text = "<table class='pull-right tbl-payment' style='width:100%;'>"
								+ ("<tr><td style='text-align:left;'>CASH</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar1) + "</td></tr>")
								+ ("<tr><td style='text-align:left;'>CREDIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar2) + "</td></tr>")
								+ ("<tr><td style='text-align:left;'>TRANSFER</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar3) + "</td></tr>")
								+ ("<tr><td style='text-align:left;'>DEBIT CARD</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar4) + "</td></tr>")
								+ ("<tr><td style='text-align:left;'>BRIDESTORY PAY</td><td>&nbsp;:&nbsp;</td><td>" + Str.Number0(TotalBayar10) + "</td></tr>")
								+ "<tr><td colspan='3'><div style='clear:both; border-bottom:solid 1px silver;'></div></td></tr>"
								+ ("<tr><td style='text-align:left;'></td><td></td><td><b>" + Str.Number0(TotalBayar1 + TotalBayar2 + TotalBayar3 + TotalBayar4 + TotalBayar10) + "</b></td></tr>")
								+ "</table>"
								;
					tr2.Cells.Add(td2);

					td2 = new TableCell();
					td2.Text = "";
					tr2.Cells.Add(td2);

					tb2.Rows.Add(tr2);
				}
				else tb2.Visible = false;

                if (!isFilled)
                {
                    Button1.Disabled = true;
                }
                Button2.Disabled = false;
            }
            else
            {
                App.TableNoResult(tb);
            }

        }
        
    }

    protected void fillFilter()
    {
        imode = 2;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;
        if (ad.Count() > 0)
        {
            imode = 2;
            role = "AE";
            var c = ad.First();
            locationAE_ID = ad.FirstOrDefault().IDLokasi;
            locationAE_Tipe = ad.FirstOrDefault().TipeLokasi;
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            imode = 4;
            role = "AM";
            locationAM = DLC.AMOutlet(App.UserID);
        }

        Location.ddlLocation(ddl_location, imode, role, locationAE_Tipe, locationAE_ID, locationAM);
        
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            ddl_location.SelectedValue = Request.QueryString["location"];
            filterDate.SelectedValue = Request.QueryString["filterDate"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&location=" + ddl_location.SelectedValue;
        href += "&filterDate=" + filterDate.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export_Filter(object sender, EventArgs e)
    {
        string href = "DPPO_Report_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&location=" + ddl_location.SelectedValue;
        href += "&filterDate=" + filterDate.SelectedValue;
        href += "&excel=0";

        Response.Redirect(href);
    }
    protected void Export_All(object sender, EventArgs e)
    {
        string href = "DPPO_Report_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&location=" + ddl_location.SelectedValue;
        href += "&filterDate=" + filterDate.SelectedValue;
        href += "&excel=1";

        Response.Redirect(href);
    }

    protected string TD_HyperLink(SalesReceiptDPPO r)
    {
        string x = string.Empty;

        bool isNotSameDay = false;

        if (r.Tgl < System.DateTime.Now.AddDays(-1)) { isNotSameDay = true; }
        
        if (r.Draft.Value == false && (r.StatusDP == 0 || r.StatusDP == 1 ))
        {
            x += "<a data-toggle='tooltip' title='View' class=\"btn btn-xs btn-primary\" target=\"_blank\" href=\"DPPO_Report_Detail.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";

            if (!isNotSameDay && mode == "pos")
            {
                x += "&nbsp;&nbsp;<a data-toggle='tooltip' title='Void' class=\"btn btn-xs btn-danger\" onClick=\"return confirm('Void DPPO Payment, Are you Sure');\" href=\"DPPO_Report_FormVoid.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-trash\"></i> ";
                x += " ";
                x += "</a>";
            }

        }
        else if (r.Draft.Value == false && r.StatusDP == 2)
        {
            x += "<a data-toggle='tooltip' title='View' class=\"btn btn-xs btn-primary\" target=\"_blank\" href=\"DPPO_Report_Detail.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (r.Draft.Value == true)
        {
            string dppo = string.Empty;
            
            if (r.IDProduct == null) { dppo = "&dp=2"; }

            x += "<a data-toggle='tooltip' title='View' class=\"btn btn-xs btn-warning\" target=\"_blank\" href=\"..\\Create\\Payment\\PaymentDP_Form.aspx?id=" + r.ID + "&status=1" + dppo + "\">";
            x += "  <i class=\"icon-pencil\"></i> ";
            x += "  ";
            x += "</a>";
            x += "&nbsp;&nbsp;<a data-toggle='tooltip' title='Cancel' class=\"btn btn-xs btn-default\" onClick=\"return confirm('Cancel Down Payment, Are you Sure?');\" href=\"DPPO_Report_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-remove\"></i> ";
            x += "  ";
            x += "</a>";
        }
        


        return x;
    }
}