using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;

public partial class Page_Register_LainLain_Promo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if(!IsPostBack)
		{
			datepicker.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("dd-MM-yyyy");
			datepicker2.Text = DateTime.Now.ToString("dd-MM-yyyy");
		}
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string name = nama.Text;
        string ket = keterangan.Text;
        string start = datepicker.Text;
        string[] timestart = start.Split('-');
        string expire = datepicker2.Text;
        string[] timeexpire = expire.Split('-');
		string tipepromo = tipe.SelectedValue;

        var time = DateTime.Now;
        string timenow = "00:00";
        string timeexpired = "23:59";

        string msgError = "";
		int promoid = 0;
		
		if(msgError == "")
		{
			string query = "EXEC sp_create_promo_lainlain_auto_approve '"+ tipepromo + "'," + 
						   "'" + name + "'," + 
						   "'" + timestart[2] + "-" + timestart[1] + "-" + timestart[0] + " " + timenow + "'," + 
						   "'" + timeexpire[2] + "-"+ timeexpire[1] + "-" + timeexpire[0] + " " + timeexpired +"'," +
						   "'" + ket + "'," +
						   "'" + App.UserNama + "' ";
			
			
			DataTable dt = Db.Rs(query, Db.Cnn_DB);
			promoid = Convert.ToInt32(dt.Rows[0][0]);
        }
        
        string href = "PromoDetail.aspx?id=" + promoid;

        //Customize-Qstr
        if(msgError != "")
		{
			ClientScript.RegisterClientScriptBlock(this.GetType(), "alert","alert("+ msgError + ");", true /* addScriptTags */);
		}
		else
		{
			Response.Redirect(href);
		}
    }
}