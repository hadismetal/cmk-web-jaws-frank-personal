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

            Response.AppendHeader("content-disposition", "attachment; filename=PLU_LD.csv");
            Set();
        }
    }
    protected void Set()
    {
        string From = Request.QueryString["from"];
        string To = Request.QueryString["to"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        int clarity = Convert.ToInt32(Request.QueryString["clarity"]);

        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = Convert.ToInt32(From.Substring(1, 2));
                int YearTo = Convert.ToInt32(To.Substring(1, 2));
                int Start = Convert.ToInt32(From.Substring(3, 6));
                int End = Convert.ToInt32(To.Substring(3, 6));

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var dj = from p in db.StockProductLD_Stone1B
                                 where p.Nomor.Contains("PLU") ? 
                                 Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                 Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                 Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom && 
                                 Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                 Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
                                 select p;
                        
                        
                        if (tipe > 0 && cabang > 0)
                        {
                            dj = dj.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe && p.StockActualLD_Stone1B.IDLokasi == cabang);
                        }


                        if (dj.Count() > 0)
                        {
                            StringBuilder builder = new StringBuilder();
                            string val = "";
                            int count = 1;
                            foreach (var rslt in dj)
                            {
                                if (count > 1)
                                    builder.Append("\n");
                                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.GIA) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.TotalCarat).ToString("N3") + rslt.Stone1B.Parcel101.NamaKode + rslt.Stone1B.Parcel103.NamaKode + rslt.Stone1B.Parcel104.NamaKode) + "\",");
                                count++;
                            }
                            Response.Write(builder.ToString());
                        }
                    }
                }
            }
            else
            {
                
                if (ProductItem > 0)
                {

                    var dj = from p in db.StockProductLD_Stone1B
                             
                             select p;


                    if (tipe > 0 && cabang > 0)
                    {
                        dj = dj.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe && p.StockActualLD_Stone1B.IDLokasi == cabang);
                    }
                    

                    if (dj.Count() > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        string val = "";
                        int count = 1;
                        foreach (var rslt in dj)
                        {
							if (count > 1)
                                    builder.Append("\n");
                            builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                            builder.Append("\"" + enc.EncryptSha(rslt.GIA) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.TotalCarat).ToString("N3")) + "\",");
							count++;
                        }
                        Response.Write(builder.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DataError();
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}