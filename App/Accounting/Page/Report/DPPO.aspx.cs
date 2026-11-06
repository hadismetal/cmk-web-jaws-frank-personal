
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.LocationList(cabang, tipe);

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesReceiptDPPO
                 where p.Draft == false
                 select p;

        int Tipe = Convert.ToInt32(locationtype.SelectedValue);
        if (Tipe > 0) rs = rs.Where(p => p.TipeLokasi == Tipe);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang);

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NomorDP == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int Status = Convert.ToInt32(status.SelectedValue);
        if (Status == 1)
            rs = rs.Where(p => p.StatusDP == 0);
        else if (Status == 2)
            rs = rs.Where(p => p.StatusDP == 1);
        else if (Status == 3)
            rs = rs.Where(p => p.StatusDP == 2);

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalBayar1 = 0m;
            decimal TotalBayar2 = 0m;
            decimal TotalBayar3 = 0m;
            decimal TotalBayar4 = 0m;
            decimal TotalBayar5 = 0m;
            decimal TotalBayar6 = 0m;
            decimal TotalBayar7 = 0m;
            decimal TotalBayar8 = 0m;
            decimal total_estimasi = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                string ndp = "";
                if (r.IDDPPOReference != null)
                {
                    ndp = "Ref = "+ (from p in db.SalesReceiptDPPO where p.ID == r.IDDPPOReference select p.NomorDP).SingleOrDefault().ToString();
                }

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NomorDP + "<br />" + "<p style='font-size: x-small; color: #6699FF'>" + ndp + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl.Value);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama + " (" + r.DataCustomer.NoCustomer + ")";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDSalesOrder.HasValue ? r.SalesOrder.Nomor : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.EstimasiHarga == 0)
                {
                    td.Text = "-";
                }
                else
                {
                    td.Text = r.EstimasiHarga.ToString("N0");
                }
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.EstimasiHarga == 0)
                {
                    td.Text = "-";
                }
                else
                {
                    decimal tobayar = Convert.ToDecimal(r.TotalBayar);
                    decimal esth = Convert.ToDecimal(r.EstimasiHarga);
                    decimal per = (tobayar / esth) * 100;
                    td.Text = per.ToString("N0") + " %";
                }
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
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Db.SingleString("SELECT TOP 1 ISNULL(VoidKeterangan,'') [VoidKeterangan] FROM SalesReceiptDPPO WHERE ID = " + r.ID, Db.Cnn_DB);
                tr.Cells.Add(td);

                var so = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID.Equals(r.ID));

                TotalBayar1 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal));
                TotalBayar2 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal));
                TotalBayar3 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal));
                TotalBayar4 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal));
                TotalBayar5 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal));
                TotalBayar6 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal));
                TotalBayar7 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal));
                TotalBayar8 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal));

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))).ToString("N0");
                tr.Cells.Add(td);

                string Bank = "-";
				string EDC = "-";
                int counter = 1;
                foreach (var t in so.SalesReceiptDPPODetail)
                {
                    if (t.IDBankIssuer.HasValue)
                    {
                        if (counter == 1)
						{
							Bank = t.BankIssuer.Nama;
                            EDC = (t.IDEDC.HasValue) ? t.EDCList.Nama : EDC;
						}
                        else
                        {
                            Bank += ", " + t.BankIssuer.Nama;
							EDC += (t.IDEDC.HasValue) ? ", " + t.EDCList.Nama : EDC;
                            counter++;
                        }
                    }
                }

                td = new TableCell();
                td.Text = Bank;
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = EDC;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += r.TotalBayar.Value;
                total_estimasi += r.EstimasiHarga;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Total.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + total_estimasi.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            if (total_estimasi == 0)
            {
                td2.Text = "<b> 0 % </b>";
            }
            else
            {
                td2.Text = "<b>"+((Total/total_estimasi)*100).ToString("N0") + " % </b>";
            }
            //td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar5).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar6).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar7).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar8).ToString("N0") + "</b>";
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
            cabang.SelectedValue = Request.QueryString["cabang"];
            locationtype.SelectedValue = Request.QueryString["tipe"];
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
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + locationtype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "DPPO_Detail_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + locationtype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "DPPO_Detail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + locationtype.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(SalesReceiptDPPO r)
    {
        string x = string.Empty;

        if (r.Draft.Value == false && r.StatusDP == 0)
        {
            x += "<a class=\"btn btn-info\" href=\"DPPO_Print.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-info\"></i> ";
            x += "  Print";
            x += "</a>";
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"DPPO_FormVoid.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Void";
            x += "</a>";
        }
        else if (r.Draft.Value == true)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"DPPO_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        return x;
    }
    protected void getLokasi(object sender, EventArgs e)
    {
        cabang.Items.Clear();

        ListItem item = new ListItem("Cabang", "0");
        cabang.Items.Add(item);

        if (locationtype.SelectedValue == "0")
        {
            DLC.LocationList(cabang, 0);
        }
        else if (locationtype.SelectedValue == "1")
        {
            DLC.LocationList(cabang, 1);
        }
        else if (locationtype.SelectedValue == "2")
        {
            DLC.LocationList(cabang, 2);
        }
    }
}