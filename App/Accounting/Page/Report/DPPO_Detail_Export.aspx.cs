using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DPPO_Detail.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesReceiptDPPO
                 where p.Draft == false
                 select p;

        int Tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        if (Tipe > 0) rs = rs.Where(p => p.TipeLokasi == Tipe);

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        if (Cabang > 0) rs = rs.Where(p => p.IDLokasi == Cabang);

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.NomorDP == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        int Status = Convert.ToInt32(Request.QueryString["status"]);
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
                    ndp = "Ref = " + (from p in db.SalesReceiptDPPO where p.ID == r.IDDPPOReference select p.NomorDP).SingleOrDefault().ToString();
                }

                td = new TableCell();
                td.Text = "<span style='font-size: small; vertical-align:top;'>"+ r.NomorDP +"</span>"
                    +", "
                    + "<span style='font-size: x-small; vertical-align:bottom; color: #6699FF;'>" 
                    + ndp + "</span>";
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
                td.Text = r.TotalBayar.Value.ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                if (r.EstimasiHarga == 0)
                {
                    td.Text = "-";
                }
                else
                {
                    td.Text = r.EstimasiHarga.ToString("N0").Replace(".", "").Replace(",", ".");
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
                    td.Text = per.ToString("N0").Replace(".", "").Replace(",", ".") + " %";
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
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))).ToString("N0").Replace(".","").Replace(",",".");
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
            td2.ColumnSpan = 5;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Total.ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + total_estimasi.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            if (total_estimasi == 0)
            {
                td2.Text = "<b> 0 % </b>";
            }
            else
            {
                td2.Text = "<b>" + ((Total / total_estimasi) * 100).ToString("N0").Replace(".", "").Replace(",", ".") + " % </b>";
            }
            //td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar5).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar6).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar7).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar8).ToString("N0").Replace(".","").Replace(",",".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}