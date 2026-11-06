using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Export : System.Web.UI.Page
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
    EncryptDecrypt enc = new EncryptDecrypt();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
        }
    }

    protected void exportDJ(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue=="0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = Convert.ToInt32(From.Substring(1, 2));
                int YearTo = Convert.ToInt32(To.Substring(1, 2));
                int Start = Convert.ToInt32(From.Substring(3, 6));
                int End = Convert.ToInt32(To.Substring(3, 6));

                if (YearFrom != YearTo) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue);
                    if (Start == End)
                    {
                        var dj = from p in db.StockProductDJ
                                 where p.Nomor == "" + From + ""
                                 select p;


                    }
                    else
                    {
                        if (Start > End) DataError("Format No. PLU salah.");
                        else
                        {
                            var dj_list = from p in db.StockProductDJ
                                          where Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                          Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                          Convert.ToInt32(p.Nomor.Substring(3, 6)) < End
                                          select p;
                        }
                    }
                }
            }
            else
            {
                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue);
                }
                else DataError("Format No. PLU / Product Item salah.");
            }
        }
        catch (Exception ex)
        {
            DataError("Format No. PLU salah.");
        }
    }
    protected void exportPG(object sender, EventArgs e)
    {
        err.Visible = false;

        string From = from.Text;
        string To = to.Text;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = Convert.ToInt32(From.Substring(1, 2));
                int YearTo = Convert.ToInt32(To.Substring(1, 2));
                int Start = Convert.ToInt32(From.Substring(3, 6));
                int End = Convert.ToInt32(To.Substring(3, 6));

                if (YearFrom != YearTo) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue);
                    if (Start == End)
                    {
                        var dj = from p in db.StockProductDJ
                                 where p.Nomor == "" + From + ""
                                 select p;


                    }
                    else
                    {
                        if (Start > End) DataError("Format No. PLU salah.");
                        else
                        {
                            var dj_list = from p in db.StockProductDJ
                                          where Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                          Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                          Convert.ToInt32(p.Nomor.Substring(3, 6)) < End
                                          select p;
                        }
                    }
                }
            }
            else
            {
                int ProductItem = Convert.ToInt32(productitem.SelectedValue);
                if (ProductItem > 0)
                {
                    Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue);
                }
                else DataError("Format No. PLU / Product Item salah.");
            }
        }
        catch (Exception ex)
        {
            DataError("Format No. PLU / Product Item salah.");
        }
    }
    protected void DataError(string msg)
    {
        err.InnerText = msg;
        err.Attributes["class"] = "text-danger";
        err.Visible = true;
    }
}