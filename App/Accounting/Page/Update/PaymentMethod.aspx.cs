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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            int Tipe = 1;
            if (tipelokasi.SelectedValue != "")
                Tipe = Convert.ToInt32(tipelokasi.SelectedValue);

            DLC.LocationList(lokasi, 1);
            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        /*var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.SalesOrder.StatusVoid == false
                 group p by p.PaymentType into g
                 select new { Tipe = g.Key.Nama, Jumlah = g.Sum(x=>x.Nominal)};*/

        var rs = from p in db.SalesReceiptDetail
                 where p.SalesReceipt.SalesOrder.StatusVoid == false
                 select p;

        //check admin sales
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.SalesReceipt.SalesOrder.IDLokasi == Lokasi && p.SalesReceipt.SalesOrder.TipeLokasi == Tipe);
        }

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.OperatorTgl >= Convert.ToDateTime(dari.Text) && p.OperatorTgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        rs = rs.OrderBy(p => p.OperatorTgl);

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalMDR = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.SalesReceipt.SalesOrder.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.SalesReceipt.OperatorTgl);
                tr.Cells.Add(td);

                string Lokasi2 = "";
                if (r.SalesReceipt.SalesOrder.TipeLokasi == 1)
                    Lokasi2 = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesReceipt.SalesOrder.IDLokasi,Db.Cnn_DB);
                else
                    Lokasi2 = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesReceipt.SalesOrder.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi2;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesReceipt.SalesOrder.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesReceipt.SalesOrder.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.PaymentType.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDCard.HasValue ? r.CardType.Nama : "-"; 
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDProgram.HasValue ? r.ProgramCicilan.Nama : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDEDC.HasValue ? r.EDCList.Nama : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CCName;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CCNumber;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number0(r.Nominal);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.MDR.HasValue ? Str.Number0(r.MDR.Value) : "-";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }
    protected string TD_HyperLink(SalesReceiptDetail r)
    {
        string x = string.Empty;

        
        x += "<a class=\"btn btn-info\" href=\"PaymentMethod_Update.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-edit\"></i> ";
        x += "  Update";
        x += "</a>";
        
        return x;
    }
}