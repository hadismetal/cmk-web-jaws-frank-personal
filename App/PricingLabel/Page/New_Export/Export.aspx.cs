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
        string Item = productitem.SelectedValue;

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportDJ.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }

    protected void exportDJ2(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;
        string Clarity = clarity.SelectedValue;
        string Item = productitem.SelectedValue;

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportDJv2.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportGJ(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;
        string Clarity = clarity.SelectedValue;
        string Item = productitem.SelectedValue;

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportGJ.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportPG(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;
        string Clarity = clarity.SelectedValue;
        string Item = productitem.SelectedValue;

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0, 3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportPG.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void exportLD(object sender, EventArgs e)
    {
        err.Visible = false;
        string From = from.Text;
        string To = to.Text;
        string Cabang = cabang.SelectedValue;
        string Clarity = clarity.SelectedValue;
        string Item = productitem.SelectedValue;

        if (To == "" && From != "") { To = From; }

        string error_msg = "";

        try
        {
            if ((From == "" || To == "" || From == null || To == null) && (productitem.SelectedValue == "0" || productitem.SelectedValue == "")) { error_msg = "No. PLU / Product Item harus diisi."; }
            if (From.Length != To.Length) { error_msg = "Jumlah karakter tidak sama."; }
            if (!((From.Length == 10 && From.Substring(0,3) == "PLU") || From.Length == 9 || From.Length == 6)) { error_msg = "Format PLU tidak valid."; }

            if (error_msg == "")
            {
                Response.Redirect("ExportLD.aspx?from=" + From + "&to=" + To + "&productitem=" + Item + "&tipe=" + tipe + "&cabang=" + Cabang + "&clarity=" + Clarity);
                Response.End();
            }
            else
            {
                DataError(error_msg);
            }
        }
        catch (Exception ex)
        {
            DataError("Terjadi kesalahan teknis. Silahkan hubungi IT Admin.");
        }
    }
    protected void DataError(string msg)
    {
        err.InnerText = msg;
        err.Attributes["class"] = "text-danger";
        err.Visible = true;
    }
}