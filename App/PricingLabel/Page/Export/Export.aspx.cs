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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    EncryptDecrypt enc = new EncryptDecrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.LocationList(cabang, tipe);
            DLC.CharProductItem(productitem);
        }
    }

    protected void exportDJ(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;
        string Clarity = clarity.SelectedValue;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue=="0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (!From.Contains("PLU") && From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));

                }
                else if (From.Length == 6)
                {
                    if (From.Length == 6 && (From.Contains("P") || From.Contains("Q")))
                    {
                        Start = Convert.ToInt32(From.Substring(1, 5));
                        End = Convert.ToInt32(To.Substring(1, 5));
                    }
                    else
                    {
                        Start = Convert.ToInt32(From);
                        End = Convert.ToInt32(To);
                    }
                }
                else
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }

                if (YearFrom != YearTo) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                    Response.End();
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
                                          where p.Nomor.Contains("PLU") ?
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                             Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
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
                    Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                }
                else DataError("Format No. PLU / Product Item salah.");
            }
        }
        catch (Exception ex)
        {
            DataError("Format No. PLU salah.");
        }
    }
    protected void exportGJ(object sender, EventArgs e)
    {
        err.Visible = false;

        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (!From.Contains("PLU") && From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));

                }
                else
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(End);
                }

                if (YearFrom != YearTo && !From.Contains("PLU")) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportGJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
                    if (Start == End)
                    {
                        var dj = from p in db.StockProductGJ
                                 where p.Nomor == "" + From + ""
                                 select p;


                    }
                    else
                    {
                        if (Start > End) DataError("Format No. PLU salah.");
                        else
                        {
                            var dj_list = from p in db.StockProductGJ
                                          where p.Nomor.Contains("PLU") ?
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                             Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
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
                    Response.Redirect("ExportGJ.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
                }
                else DataError("Format No. PLU / Product Item salah.");
            }
        }
        catch (Exception ex)
        {
            DataError("Format No. PLU / Product Item salah.");
        }
    }
    protected void exportPG(object sender, EventArgs e)
    {
        err.Visible = false;

        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }
                else if (From.Contains("PLU"))
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }

                if (YearFrom != YearTo && !From.Contains("PLU")) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
                    if (Start == End)
                    {
                        var dj = from p in db.StockProductPG
                                 where p.Nomor == "" + From + ""
                                 select p;


                    }
                    else
                    {
                        if (Start > End) DataError("Format No. PLU salah.");
                        else
                        {
                            var dj_list = from p in db.StockProductPG
                                          where p.Nomor.Contains("PLU") ?
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                             Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
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
                    Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
                }
                else DataError("Format No. PLU / Product Item salah.");
            }
        }
        catch (Exception ex)
        {
            DataError("Format No. PLU / Product Item salah.");
        }
    }
    protected void exportLD(object sender, EventArgs e)
    {
        err.Visible = false;

        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;

        if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) DataError("No. PLU / Product Item harap diisi.");

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (!From.Contains("PLU"))
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                if (YearFrom != YearTo && !From.Contains("PLU")) DataError("Format No. PLU salah.");
                else
                {
                    Response.Redirect("ExportLD.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
                    if (Start == End)
                    {
                        var dj = from p in db.StockProductLD_Stone1B
                                 where p.Nomor == "" + From + ""
                                 select p;


                    }
                    else
                    {
                        if (Start > End) DataError("Format No. PLU salah.");
                        else
                        {
                            var dj_list = from p in db.StockProductLD_Stone1B
                                          where p.Nomor.Contains("PLU") ?
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                             Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                             Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
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
                    Response.Redirect("ExportLD.aspx?from=" + From + "&to=" + To + "&productitem=" + productitem.SelectedValue + "&tipe=" + tipe + "&cabang=" + Cabang);
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