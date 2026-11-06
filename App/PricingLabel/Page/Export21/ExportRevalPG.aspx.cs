using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

    protected int limitdata = 500;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Set();
        }
    }
    protected void Set()
    {
		db.CommandTimeout = 1000;
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        
        int IDProduct = 0;

        try
        {
            IDProduct = Convert.ToInt32(Request.QueryString["idproduct"]);
        }
        catch (Exception) { }

        string xTest = "0";

        try
        {
            StringBuilder builder = new StringBuilder();
            List<PGReval> list = new List<PGReval>();
            if (1 == 1)
            {
                xTest = "4";

                string spname = "sp_ExportLabel_reval_pg ";
                string query = " exec " + spname
                             + cabang + ", "
                             + tipe + ", "
                             + ProductItem + ", "
                             + IDProduct;

                DataTable dt = Db.Rs(query, Db.Cnn_DB);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new PGReval
                        {
                            IDProduct = Convert.ToInt32(row["IDProduct"]),
                            Nomor = row["Nomor"].ToString(),
                            Tgl = Convert.ToDateTime(row["Tgl"]),
                            SupplierNama = row["SupplierNama"].ToString(),
                            ProductItem = row["ProductItem"].ToString(),
                            KodeItem = row["KodeItem"].ToString(),
                            FrameColor = row["FrameColor"].ToString(),
                            FrameMaterial = row["Material"].ToString(),
                            KadarLogam = Convert.ToDecimal(row["KadarLogam"]),
                            GrossWeight = Convert.ToDecimal(row["GrossWeight"]),
                            DimensiR = Convert.ToDecimal(row["DimensiR"]),
                            Harga = Convert.ToDecimal(row["Harga"])
                        });
                    }
                }

                xTest = "5B";

                if (list.Count > 0 && list.Count <= limitdata)
                {
                    xTest = "6";

                    Response.ContentType = "text/csv";
                    Response.AppendHeader("content-disposition", "attachment; filename=PG_"
                        + App.UserNama.Replace(" ", "") + "_"
                        + System.DateTime.Now.Year.ToString().Substring(2)
                        + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                        + "_" + list.Count.ToString() + ".csv");

                    int count = 1;
                    foreach (var rslt in list)
                    {
                        string xNomor = "";
                        string xItem = "";
                        string xGross = "";
                        string xHarga = "";
                        string xKadar = "";
                        string xKode = "";
                        string xColor = "";
                        string xDimR = "";
                        string xMaterial = "";
                        string xRingSize = "";

                        xNomor = rslt.Nomor;
                        xItem = rslt.ProductItem;
                        xGross = Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr";
                        xKadar = Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%";
                        xHarga = rslt.Harga.ToString("N0");
                        if (rslt.SupplierNama != "" && rslt.SupplierNama != null)
                        {
                            xKode += rslt.SupplierNama.Substring(0, 1);
                        }
                        xKode += rslt.KodeItem;
                        xKode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                        xKode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                        xKode += "1";
                        xColor = rslt.FrameColor;
                        xDimR = rslt.DimensiR.ToString("N2");
                        xMaterial = db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode;
                        xRingSize = "";

                        if (count > 1) { builder.Append("\n"); }

                        builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xItem) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xGross) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xHarga) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xKadar) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xKode) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xColor) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xDimR) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xMaterial) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xRingSize) + "\",");

                        count++;
                    }

                    Response.Write(builder.ToString());
                }

            }
            else if (list.Count > 0 && list.Count > limitdata)
            {
                string xmsg = "";
                xmsg = "ExportReval.aspx?msg=" + limitdata.ToString();
                xmsg += "&v=" + "PG";
                xmsg += "&it=" + ProductItem;
                xmsg += "&tp=" + tipe;
                xmsg += "&cb=" + cabang;

                Response.Redirect(xmsg);
            }
            else
            {
                string xmsg = "";
                xmsg = "ExportReval.aspx?msg=0";
                xmsg += "&v=" + "PG";
                xmsg += "&it=" + ProductItem;
                xmsg += "&tp=" + tipe;
                xmsg += "&cb=" + cabang;

                Response.Redirect(xmsg);
            }
        }
       
        catch (Exception ex)
        {
            xTest = "Error Code : 0x" + xTest.PadLeft(3, '0')
                  + "<br /><br />Error Message : <span>" + ex.Message.ToString() + "</span>"
                  + "<br /><br />Please contact IT Administrator within the error code number and message." 
                  ;

            Response.Write(xTest);
        }
    }

    class PGReval
    {
        public int IDProduct { get; set; }
        public string Nomor { get; set; }
        public string SupplierNama { get; set; }
        public DateTime Tgl { get; set; }
        public string ProductItem { get; set; }
        public string KodeItem { get; set; }
        public string FrameColor { get; set; }
        public string FrameMaterial { get; set; }
        public decimal KadarLogam { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal DimensiR { get; set; }
        public decimal Harga { get; set; }
    }

    protected void DataError()
    {
        Response.Redirect("ExportReval.aspx");
    }
}