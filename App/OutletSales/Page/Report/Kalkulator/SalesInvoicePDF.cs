using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Kalkulator_SalesInvoicePDF : System.Web.UI.Page
{
    Map_DB db = new Map_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }

    protected void go_Click(object sender, EventArgs e) 
    {
        string res = "";
        var pludj = db.StockProductDJ.SingleOrDefault(p => p.Nomor == kwplu.Text);

        if (pludj == null && plugj == null && plupg == null && pluld == null)
        {
            res = "PLU tidak terdaftar.";
        }
        else
        {
            PLU obj = new PLU
            {
                merchant_slug = "franknco",
                variant_code = kwplu.Text
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //byte[] objects = Encoding.UTF8.GetBytes(obj.ToString());
            //res = InactiveProductStamps.WebRequestPOST(json);
        }
        //lblresult.Text = res;
    }

    public class PLU 
    {
        public string merchant_slug;
        public string variant_code;
    }
}