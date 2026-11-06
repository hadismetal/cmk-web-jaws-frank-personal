using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_Detail_Certificate : System.Web.UI.Page
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

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //fillFilter();
            fillTable(sender, e);
            fillnotes2();
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        bool isExpired = false;
        string color = "green";
        DateTime dta = DateTime.Today;

        var rs = (from p in db.PrintCertificate
                  where p.StatusFinish == 1 && p.ID == myID
                  select new
                  { docnama = p.Nama,
                      docno = p.Nomor,
                      docket = p.Keterangan,
                      doctgl = p.Tgl,
                      doctglex = p.TglExpire,
                      docoperat = p.Operator,
                  }
                  ).SingleOrDefault();

        if (rs.doctglex < dta) { isExpired = true; color = "red"; }

        var rs1 = (from p1 in db.PrintCertificate_Detail
                  where p1.IDForm == myID && p1.URL != null
                  select new
                  {
                      doctipepro = (p1.TypeProduct == 1 ? "DJ" : "PG"),
                      docidpro = p1.IDProduct,
                      docurl = p1.URL,
                      docnoplu = p1.PLU,
                      docitem = p1.Item
                  });

        if (rs != null)
        {
            txnama.InnerText = rs.docnama != null ? rs.docnama : "";
            txketerangan.InnerText = (rs.docket != null) ? rs.docket : "";
            txoperator.InnerText = (rs.docoperat != null) ? rs.docoperat : "";
            txdc.InnerText = (rs.doctgl != null) ? Str.FullDate(rs.doctgl) : "";
            txde.InnerHtml = "<span style='color:" + color + ";'>" + ((rs.doctglex != null) ? Str.FullDate(rs.doctglex) : "") + "</span>";
            txdn.InnerText = (rs.docno != null) ? rs.docno : "";
            txstat.InnerHtml = "<span style='color:" + color + ";'>" + ((dta <= rs.doctglex) ? "Active" : "Expired") + "</span>";
            int qty = Db.SingleInteger(" select COUNT(*) from PrintCertificate_Detail where IDForm =" + myID, Db.Cnn_DB);
            txqty.InnerText = qty.ToString();
        }

        //if (rs1.Count() == 0) App.TableNoResult(tb);

        string st = "";
        //RESULT
        int prev = 0;
        int index = 1;

        lit_plu_list.Text = "<div class='row'>";
        foreach (var r in rs1)
        {
            if (!Response.IsClientConnected) break;
            viewPLU(lit_plu_list, r.doctipepro, r.docidpro, r.docurl, isExpired);
        }
        lit_plu_list.Text += "</div>";
    }

    protected void fillnotes2()
    {
        var result = from p in db.PrintCertificate_Detail
                      where p.IDForm == myID
                      group p by new { p.Item } into g
                      select g;

        if (result.Count() > 0)
        {
            foreach (var r in result)
            {
                tproditem.InnerHtml += r.Key.Item + " (" + r.Count().ToString() + ")<br />";

            }
        }

    }

    protected string TD_hyperlink(string url)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"" + url  + "\">";
        x += "  <i class=\"icon-print\"></i> ";
        x += "  ";
        x += "</a>";
        x += "</div>";

        return x;
    }

    protected void viewPLU(Literal x, string tipeplu, int idplu, string imgplu, bool isExpired = true)
    {
        string xplu = "";
        string xtipeplu = "";
        string xharga = "";
        string xcurrency = "";
        string ximage = "";
        string xproductinfo = "";
        string xproductitem = "";

        if (tipeplu == "DJ") // dj
        {
            var sample1 = (from p in db.StockProductDJ
                           where p.ID == idplu && p.Draft == false && p.Approval != null
                           select p).SingleOrDefault();

            xplu = sample1.Nomor;
            xharga = Convert.ToDecimal(sample1.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
            xcurrency = "IDR";
            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";
            xproductitem = sample1.StockProductDJ_CharProduct.CharProductItem.Nama;
        }
        else if (tipeplu == "PG") //pg
        {
            var sample1 = (from p in db.StockProductPG
                           where p.ID == idplu && p.Draft == false && p.Approval != null
                           select p).SingleOrDefault();

            xplu = sample1.Nomor;
            xharga = "";
            xcurrency = "";
            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";
            xproductitem = sample1.CharProductItem.Nama;
        }

        if (tipeplu == "DJ") { xtipeplu = "Diamond Jewellery"; }
        if (tipeplu == "PG") { xtipeplu = "Plain Gold"; }

        //xproductinfo = "<div class='row'>";
        xproductinfo += "<div class='col-md-3 col-sm-4 col-xs-12'>";

        /*
        
        x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
        
        x += "  <i class=\"icon-print\"></i> ";
        x += "  ";
        x += "</a>";
        x += "</div>";

        */
        xproductinfo += (!isExpired) ? ("<a target=\"_blank\" href=\"" + imgplu + "\">") : "";

        xproductinfo += "<div class='info-box'>";
        xproductinfo += "<span class='info-box-icon bg-silver'>";
        if (ximage != "")
        {
            xproductinfo += "<img src=\"/Images.aspx?path=" + ximage + "\" />";
        }
        xproductinfo += "</span>";
        xproductinfo += "<div class='info-box-content'>";
        xproductinfo += "<span class='info-box-text'>" + xplu + "</span>";
        xproductinfo += "<span class='info-box-number'>" + xharga + "<small class='text-muted'>&nbsp;" + xcurrency + "</small></span>";
        xproductinfo += "<span class='info-box-descripton'>" + tipeplu + " : " + xproductitem + "</span>";
        xproductinfo += "</div>";
        xproductinfo += "<!-- /.info-box-content -->";
        xproductinfo += "</div>";
        xproductinfo += "<!-- /.info-box -->";

        xproductinfo += (!isExpired) ? "</a>" : "";

        xproductinfo += "</div>";
        //xproductinfo += "</div>";

        x.Text += xproductinfo;
    }


}