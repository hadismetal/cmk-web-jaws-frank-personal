using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

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
            Response.ContentType = "text/csv";

            Response.AppendHeader("content-disposition", "attachment; filename=PLU_GJ.csv");
            Set();
        }
    }
    protected void Set()
    {


        var dj = from p in db.RevalProductGJ

                 select p;

        dj = dj.OrderBy(p => p.RevalProductGJ_CharProduct.ProductItem);

        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];

        if (kw!="")
            dj = dj.Where(p => p.Nomor == "" + kw + "");
        
        if (ProductLevel > 0) dj = dj.Where(p => p.RevalProductGJ_CharProduct.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) dj = dj.Where(p => p.RevalProductGJ_CharProduct.ProductItem == ProductItem);
        
        if (ProductCategory > 0) dj = dj.Where(p => p.RevalProductGJ_CharProduct.ProductCategory == ProductCategory);

        if (dj.Count() > 0)
        {
            StringBuilder builder = new StringBuilder();
            string val = "";
            int count = 1;
            foreach (var rslt in dj)
            {
                string Kode = "";
                if (rslt.SupplierNomor != "" && rslt.SupplierNomor != null)
                    Kode += rslt.SupplierNomor.Substring(0, 1);
                Kode += rslt.RevalProductGJ_CharProduct.CharProductItem.NamaKode;
                Kode += rslt.RevalProductGJ_CharProduct.CharProductCategory.NamaKode;
                Kode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                Kode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                Kode += "1";

                var stone = 0m; 

                if (count > 1)
                    builder.Append("\n");
                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                builder.Append("\"" + enc.EncryptSha(rslt.RevalProductGJ_CharProduct.CharProductItem.Nama) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.RevalProductGJ_PricingMU.HargaRupiahRound).ToString("N0")) + "\",");
                builder.Append("\"" + enc.EncryptSha(rslt.RevalProductGJ_CharProduct.KadarLogam.ToString("N0") + "%") + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.RevalProductGJ_CharProduct.StoneQty).ToString("N0")) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.RevalProductGJ_CharProduct.StoneCarat).ToString("N3")) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.RevalProductGJ_CharProduct.NettoWeight).ToString("N2")) + "\",");
                
                count++;
            }
            Response.Write(builder.ToString());
        }

    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}