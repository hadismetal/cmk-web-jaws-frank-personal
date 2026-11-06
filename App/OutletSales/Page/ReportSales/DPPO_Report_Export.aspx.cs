using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DPPO_Detail.xls");
            fillTable(sender, e);  
        }
    }
    
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string TglDari = Request.QueryString["dari"];
        string TglSampai = Request.QueryString["sampai"];
        string keyword = Request.QueryString["kw"];
        int Status = Convert.ToInt32(Request.QueryString["status"]);
        string xddlLocation = Request.QueryString["location"];
        string filterDate = Request.QueryString["filterDate"];

        List<SalesReceiptDPPO> rs = new List<SalesReceiptDPPO>();
        if (filterDate.Equals("0"))
        {
            rs = (from p in db.SalesReceiptDPPO
                  where ((p.Tgl >= Convert.ToDateTime(TglDari) && p.Tgl < Convert.ToDateTime(TglSampai).AddDays(1)) ||
                  (p.Tgl < Convert.ToDateTime(TglSampai) && p.SalesOrder == null))
                  select p).ToList();
        }
        if (filterDate.Equals("1"))
        {
            rs = (from p in db.SalesReceiptDPPO
                  where (p.SalesOrder.Tgl <= Convert.ToDateTime(TglSampai) && p.SalesOrder.Tgl >= Convert.ToDateTime(TglDari))
                  select p).ToList();
        }

        if (new[] { "1", "2" }.Contains(xddlLocation.Substring(0, 1)))
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
            if (filterDate.Equals("0"))
            {
                rs = rs.Where(p => p.StatusDP == 0 && p.Draft == false).ToList();
            }
            else if (filterDate.Equals("1"))
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

        if (Request.QueryString["excel"] == "0")
        {
            tb.Visible = true;
            tb2.Visible = false;
            if (rs.Count() > 0)
            {
                decimal Total = 0m;
                decimal TotalBayar1 = 0m;
                decimal TotalBayar2 = 0m;
                decimal TotalBayar3 = 0m;
                decimal TotalBayar4 = 0m;
                decimal TotalBayar10 = 0m;
                decimal total_estimasi = 0m;

                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    if (r.Tgl < Convert.ToDateTime(TglDari))
                    {
                        break;
                    }

                    TableRow tr = new TableRow();
                    TableCell td;


                    td = new TableCell();
                    td.Text = r.NomorDP;
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
                    td.Text = r.SalesNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CustomerNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.DataCustomer.NoCustomer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.IDSalesOrder.HasValue ? r.SalesOrder.Nomor : "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0").Replace(".", "").Replace(",", ".");
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

                    var so = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID.Equals(r.ID));

                    TotalBayar1 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal));
                    TotalBayar2 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal));
                    TotalBayar3 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal));
                    TotalBayar4 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal));
                    TotalBayar10 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal));

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    string Bank = "-";
                    int counter = 1;
                    foreach (var t in so.SalesReceiptDPPODetail)
                    {
                        if (t.IDBankIssuer.HasValue)
                        {
                            if (counter == 1)
                                Bank = t.BankIssuer.Nama;
                            else
                            {
                                Bank += ", " + t.BankIssuer.Nama;
                                counter++;
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    Total += r.TotalBayar.Value;
                    total_estimasi += r.EstimasiHarga;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 7;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Total.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
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
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar10).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (Request.QueryString["excel"] == "1")
        {
            tb.Visible = false;
            tb2.Visible = true;
            if (rs.Count() > 0)
            {
                decimal Total = 0m;
                decimal TotalBayar1 = 0m;
                decimal TotalBayar2 = 0m;
                decimal TotalBayar3 = 0m;
                decimal TotalBayar4 = 0m;
                decimal TotalBayar10 = 0m;
                decimal total_estimasi = 0m;

                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {

                    TableRow tr = new TableRow();
                    TableCell td;


                    td = new TableCell();
                    td.Text = r.NomorDP;
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
                    td.Text = r.SalesNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.CustomerNama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.DataCustomer.NoCustomer;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.IDSalesOrder.HasValue ? r.SalesOrder.Nomor : "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0").Replace(".", "").Replace(",", ".");
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

                    td = new TableCell();
                    td.Text = Str.HtmlBreak(r.Keterangan);
                    tr.Cells.Add(td);

                    var so = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID.Equals(r.ID));

                    TotalBayar1 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal));
                    TotalBayar2 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal));
                    TotalBayar3 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal));
                    TotalBayar4 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal));
                    TotalBayar10 += Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal));

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(Convert.ToDecimal(so.SalesReceiptDPPODetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal))).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    string Bank = "-";
                    int counter = 1;
                    foreach (var t in so.SalesReceiptDPPODetail)
                    {
                        if (t.IDBankIssuer.HasValue)
                        {
                            if (counter == 1)
                                Bank = t.BankIssuer.Nama;
                            else
                            {
                                Bank += ", " + t.BankIssuer.Nama;
                                counter++;
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);

                    Total += r.TotalBayar.Value;
                    total_estimasi += r.EstimasiHarga;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 7;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Total.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
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
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalBayar10).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb2.Rows.Add(tr2);
            }
        }
    }
}