using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;


public partial class Page_Create_Create_Certificate : System.Web.UI.Page
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

    string proditem = "";
    int prodid;
    string nama = "";
    string ket = "";
    int statplu;
    int tp;
    DateTime dtp;
    string noregisdok = "";
    DateTime dta = DateTime.Today;

    protected int myID { get { return App.QueryInt(this); } }
    protected int stat { get { return App.QueryInt(this, "stat"); } }
    protected string neplu { get { return Request.QueryString["plu"]; } }
    protected string xstatus { get { return Request.QueryString["status"]; } }

    protected int tipep { get { return App.QueryInt(this, "tipep"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            FillForm();
            Page.MaintainScrollPositionOnPostBack = true;
        }

    }

    protected void ButtonView(int mode)
    {
        bool isExistItem = false;
        isExistItem = db.PrintCertificate_Detail.Any(p => p.PrintCertificate.ID == myID);

        if (mode == 0)
        {
            next2.Visible = true;
            btn_add_cplu.Visible = false;
            btrev.Visible = false;
            btcancel.Visible = false;
            btsubmit.Visible = false;
        }
        else if (mode == 1)
        {
            next2.Visible = false;
            btn_add_cplu.Visible = true;
            btrev.Visible = (isExistItem);
            btcancel.Visible = false;
            btsubmit.Visible = false;
        }
        else if (mode == 2)
        {
            next2.Visible = false;
            btn_add_cplu.Visible = false;
            btrev.Visible = false;
            btcancel.Visible = true;
            btsubmit.Visible = true;
        }
        else if (mode == 3)
        {
            next2.Visible = false;
            btn_add_cplu.Visible = false;
            btrev.Visible = false;
            btcancel.Visible = false;
            btsubmit.Visible = false;
        }
    }

    protected void FillForm()
    {
        content2.Visible = false;
        tb1.Visible = false;

        if (myID > 0)
        {
            ButtonView(1);

            content2.Visible = true;
            tb1.Visible = true;

            var a = db.PrintCertificate.SingleOrDefault(p => p.ID == myID);

            if (a != null)
            {
                if (a.StatusFinish == 1)
                {
                    Response.Redirect("/OutletCorporate/Page/Report/Detail_Certificate.aspx?id=" + myID);
                }
                else
                {

                    header.Visible = false;
                    content1.Visible = true;
                    content.Visible = true;
                    nama = a.Nama;
                    ket = a.Keterangan;
                    lbnama.Text = nama;
                    lbket.Text = ket;
                    txnama.Text = nama;
                    txketerangan.Text = ket;
                    ldc.Text = Str.FullDate(a.Tgl);
                    lde.Text = Str.FullDate(a.TglExpire);

                    fillTableDJ();
                    fillTablePG();
                    fillnotes2();
                }
            }

        }
        else
        {
            ButtonView(0);
        }

        if (stat == 1)
        {
            lit_msg_warning.Text = neplu + " has not found";
            proditem = "";
            prodid = 0;
        }
        else if (stat == 2)
        {
            if (tipep == 1)
            {
                lit_msg_warning.Text = neplu + " (DJ) has been submit on " + noregisdok + " (" + dtp.ToShortDateString() + ")";
            }
            else if (tipep == 2)
            {
                lit_msg_warning.Text = neplu + " (PG) has been submit on " + noregisdok + " (" + dtp.ToShortDateString() + ")";
            }
        }

    }

    protected void Sub_Header(int tipeplu)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        //    x += "<br />";
        x += DL.ItemIndex(tipeplu);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillTableDJ()
    {
        var result = from p in db.PrintCertificate_Detail where p.IDForm == myID && p.TypeProduct == 1 orderby p.Item ascending, p.PLU ascending select p;

        DateTime dtap = dta.AddDays(3);

        if (result.Count() > 0) Sub_Header(0);

        int idx = 1;
        foreach (var r in result)
        {

            int tipep = 0;

            tipep = 1;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + idx.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.PLU != null) ? r.PLU : "";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.Item != null) ? r.Item : "";
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (btrev.Visible == true) ? TD_HyperLink(tipep, r.IDProduct) : "";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
            idx++;
        }
    }
    protected void fillTablePG()
    {
        var result = from p in db.PrintCertificate_Detail where p.IDForm == myID && p.TypeProduct == 2 select p;

        DateTime dtap = dta.AddDays(3);

        if (result.Count() > 0) Sub_Header(1);

        int idx = 1;
        foreach (var r in result)
        {
            int tipep = 0;
            tipep = 2;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + idx.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.PLU != null) ? r.PLU : "";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (r.Item != null) ? r.Item : "";
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (btrev.Visible == true) ? TD_HyperLink(tipep, r.IDProduct) : "";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
            idx++;
        }
    }

    protected void fillnotes()
    {
        string query = "exec vregis_printcertif " + myID;
        DataTable tnotes = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        int qty = 0;
        string wr = "";
        if (tnotes.Rows.Count > 0)
        {
            int idx = 1;
            foreach (DataRow dtRow in tnotes.Rows)
            {

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = idx.ToString() + ".";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                int col = 1;
                foreach (DataColumn dc in tnotes.Columns)
                {
                    wr = dtRow[dc].ToString();
                    if (col == 1)
                    {
                        td = new TableCell();
                        td.Text = wr;
                        td.Style.Add("text-align", "left");
                        tr.Cells.Add(td);
                    }
                    if (col == 2)
                    {
                        qty += Convert.ToInt32(dtRow[dc]);
                        td = new TableCell();
                        td.Text = wr;
                        td.Style.Add("text-align", "right");
                        tr.Cells.Add(td);
                    }



                    col++;

                }
                tb1.Rows.Add(tr);
                idx++;
            }
            TableRow tr1 = new TableRow();
            TableCell td1;

            td1 = new TableCell();
            td1.Text = "<b>Total</b>";
            td1.Style.Add("text-align", "right");
            td1.ColumnSpan = 2;
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.Text = "<b>" + qty.ToString() + "</b>";
            td1.Style.Add("text-align", "center");
            tr1.Cells.Add(td1);

            tb1.Rows.Add(tr1);
        }


    }
    protected void fillnotes2()
    {
        var results = from p in db.PrintCertificate_Detail
                      where p.IDForm == myID
                      group p by new { p.Item } into g
                      select g;


        int idx = 1;
        int qty = 0;
        foreach (var r in results)
        {

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = idx.ToString() + ".";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);


            td = new TableCell();
            td.Text = r.Key.Item;
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Count().ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);


            qty += r.Count();

            tb1.Rows.Add(tr);
            idx++;
        }

        TableRow tr1 = new TableRow();
        TableCell td1;

        td1 = new TableCell();
        td1.Text = "<b>Total</b>";
        td1.Style.Add("text-align", "right");
        td1.ColumnSpan = 2;
        tr1.Cells.Add(td1);

        td1 = new TableCell();
        td1.Text = "<b>" + qty.ToString() + "</b>";
        td1.Style.Add("text-align", "center");
        tr1.Cells.Add(td1);

        tb1.Rows.Add(tr1);

    }
    protected void btn_add_cplu_Click(object sender, EventArgs e)
    {
        lit_msg_warning.Text = "";

        int flag_dj = 0;
        int flag_pg = 0;

        string exist_doc = "";

        bool cek_dj = false;
        bool cek_pg = false;

        string plu = iplu.Text.Trim();
        flag_dj = cek_plu(plu, "dj");
        flag_pg = cek_plu(plu, "pg");

        if (flag_dj > 0) { cek_dj = true; }
        if (!cek_dj && flag_pg > 0) { cek_pg = true; }

        lit_msg_warning.Text = flag_pg.ToString();

        if (cek_dj || cek_pg)
        {
            /// check dokumen sendiri jika sudah pernah terinput
            var xplu1 = db.PrintCertificate_Detail.Where(p => p.PrintCertificate.ID == myID && p.TypeProduct == (cek_dj ? 1 : 2) && p.IDProduct == (cek_dj ? flag_dj : flag_pg));
            exist_doc += (xplu1.Count() > 0) ? ("PLU <b>" + plu + "</b> sudah terinput" + "<br />") : "";

            /// check dokumen lain jika sudah pernah terinput dan belum expire
            var xplu2 = db.PrintCertificate_Detail.Where(p => p.PrintCertificate.StatusFinish == 1 && p.TypeProduct == (cek_dj ? 1 : 2) && p.IDProduct == (cek_dj ? flag_dj : flag_pg) && p.PrintCertificate.TglExpire >= System.DateTime.Now);
            exist_doc += (xplu2.Count() > 0) ? ("PLU <b>" + plu + "</b> sudah terdaftar dan masih aktif di dokumen <b>" + xplu2.First().PrintCertificate.Nomor + "</b> hingga <b>" + Str.FullDate(xplu2.First().PrintCertificate.TglExpire) + "</b><br />") : "";

        }
        else
        {
            /// PLU tidak ditemukan
            exist_doc += "PLU <b>" + plu + "</b> tidak ditemukan" + "<br />";

        }

        if ((exist_doc != ""))
        {
            // jika ada warning error
            //lit_msg_warning.Text = exist_doc;
            lit_msg_warning.Text = "<div class='alert alert-warning'>" + exist_doc + "</div>";
        }
        else
        {
            Db.Execute("exec sp_print_certificate_create 2, '" + nama + "', '" + ket + "', '" + App.UserNama + "', '" + myID + "', '" + (cek_dj ? "1" : "2") + "','" + (cek_dj ? flag_dj.ToString() : flag_pg.ToString()) + "' ", Db.Cnn_DB);
            Response.Redirect("Create_Certificate.aspx?id=" + myID);
        }

        FillForm();
    }


    protected string TD_HyperLink(int tipe, int idpro)
    {
        string x = string.Empty;
        string href = string.Empty;
        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-xs btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + tipe + "', '" + idpro + "' )\">";
        x += "      <span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span> ";
        x += "      ";
        x += "  </a>";
        x += "</div>";
        return x;
    }
    protected int cek_plu(string plu, string tipeplu)
    {
        int result = 0;

        if (tipeplu == "dj")
        {
            var rs = (from p in db.StockProductDJ
                      where p.Draft == false
                          //&& p.ReturnProduct == false
                          //&& p.StockActualDJ.Substorage == 1
                          //&& p.StatusPenjualan == 0
                          //&& p.StockActualDJ.InHand == 1
                      && p.Nomor == plu
                      select p).SingleOrDefault();

            if (rs != null)
            {
                result = rs.ID;
            }

        }
        else if (tipeplu == "pg")
        {
            var rs = (from p in db.StockProductPG
                      where p.Draft == false
                         && p.Nomor == plu
                      //&& p.StockActualPG.Substorage == 1
                      //&& p.StatusPenjualan == 0
                      //&& p.StockActualPG.InHand == 1
                      select p).SingleOrDefault();

            if (rs != null)
            {
                result = rs.ID;
            }

        }


        return result;
    }


    protected void save_Click(object sender, EventArgs e)
    {
        Db.Execute("exec sp_print_certificate_create 1, '', '', '', '" + myID + "'", Db.Cnn_DB);
        Response.Redirect("/OutletCorporate/Page/Report/Detail_Certificate.aspx?id=" + myID);
    }

    protected void next_Click(object sender, EventArgs e)
    {
        string nama = txnama.Text;
        string keterangan = txketerangan.Text;
        string operat = App.UserNama;

        int idc = 0;
        try
        {
            idc = Db.SingleInteger("exec sp_print_certificate_create 0, '" + nama + "', '" + keterangan + "', '" + operat + "' ", Db.Cnn_DB);
        }
        catch (Exception) { }

        Response.Redirect("Create_Certificate.aspx?id=" + idc);

    }

    protected void btrev_ServerClick(object sender, EventArgs e)
    {
        FillForm();
        header.Visible = false;
        txnama.Enabled = false;
        txketerangan.Enabled = false;
        content1.Visible = true;

        if (header.Visible == true)
        {
            
        }
        else
        {
            content.Visible = true;
            next2.Visible = false;

            lit_msg_warning.Visible = false;
            content2.Visible = false;
        }

        ButtonView(2);

    }

    protected void btcancel_ServerClick(object sender, EventArgs e)
    {
        FillForm();
        header.Visible = false;
        content1.Visible = true;
        lbnama.Visible = true;
        lbket.Visible = true;
        iplu.Enabled = true;
        next2.Visible = false;
        lit_msg_warning.Visible = true;
        content2.Visible = true;

        ButtonView(1);
    }

    protected void txnama_TextChanged(object sender, EventArgs e)
    {
        lbnama.Text = txnama.Text;
    }

    protected void txketerangan_TextChanged(object sender, EventArgs e)
    {
        lbket.Text = txketerangan.Text;
    }
}