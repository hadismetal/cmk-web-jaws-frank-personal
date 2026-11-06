using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

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
    //protected int myID { get { return App.QueryInt(this); } }
    protected string myIDSO { get { return Request.QueryString["SO"]; } }
    protected string myIDINV { get { return Request.QueryString["INV"]; } }
    protected string myPLU { get { return Request.QueryString["PLU"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Fill();

        }
    }
    protected void Fill()
    {
        byte[] dataso = Convert.FromBase64String(myIDSO);
        int xso = Convert.ToInt32(Encoding.UTF8.GetString(dataso));

        byte[] datainv = Convert.FromBase64String(myIDINV);
        int xinv = Convert.ToInt32(Encoding.UTF8.GetString(datainv));

        byte[] dataplu = Convert.FromBase64String(myPLU);
        string xplu = Encoding.UTF8.GetString(dataplu);

        var x = db.SalesOrderDJ.SingleOrDefault(p => p.ID == xinv && p.SalesOrder.ID == xso && p.StockProductDJ.Nomor == xplu);
        if (x != null)
        {

            var d = x.StockProductDJ;
            if (d != null)
            {
                if (d.LocWarehouse.CompanyBrand.NamaKode == "F")
                {
                    css.Text = "<style>body { color: #7c0545; font-weight: bold;}</style>";
                }
                else
                {
                    css.Text = "<style>body { color: #000000; font-weight: bold;}</style>";
                }

                plu.Text = d.Nomor;

                kadar.Text = (Math.Floor(d.StockProductDJ_CharProduct.KadarLogam) == 37)
                           ? (d.StockProductDJ_CharProduct.KadarLogam).ToString("N1")
                           : Math.Floor(d.StockProductDJ_CharProduct.KadarLogam).ToString("N0")
                           ;
                weight.Text = d.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                kodekaret.Text = d.ProtoNomor;
                item.Text = d.StockProductDJ_CharProduct.CharProductItem.Nama;
                img.Text = "<img src=\"/Images.aspx?path=" + d.ImgPicture + "\" style='max-width:90px; max-height:67px;' />";

                fillStone(d);
            }


        }

    }

    protected void fillStone(StockProductDJ d)
    {
        string plunumber = d.Nomor;
        string stonemodelist = "c"; // b:bandroll / c:certificate
        string plutype = "dj"; // dj or ld

        string spname = "sp_get_stone_information_for_bandroll_certificate_report";
        string query = " exec " + spname
                     + " '" + stonemodelist + "'"
                     + ",'" + plunumber + "'"
                     + ",'" + plutype + "'"
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt.Rows.Count > 0)
        {
            int TotalButir = 0;
            decimal TotalCarat = 0m;
            string Keterangan = "";

            //RESULT
            foreach (DataRow dtRow in dt.Rows)
            {
                TotalButir = Convert.ToInt32(dtRow["TotalButir"]);
                TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"]);
                Keterangan = (dtRow["Keterangan"]).ToString();

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = (TotalButir).ToString("N0");
                td.Style.Add("text-align", "right");
                td.Style.Add("padding", "0");
                td.Style.Add("margin", "0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = " = ";
                td.Style.Add("text-align", "center");
                td.Style.Add("padding", "0");
                td.Style.Add("margin", "0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (TotalCarat).ToString("N3");
                td.Style.Add("text-align", "left");
                td.Style.Add("padding", "0");
                td.Style.Add("margin", "0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Keterangan;
                td.Style.Add("text-align", "left");
                td.Style.Add("padding", "0");
                td.Style.Add("margin", "0");
                tr.Cells.Add(td);

                tbl_stonelist.Rows.Add(tr);

            }


        }

    }
}