using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Update_LainLain_PromoDetail_Update : System.Web.UI.Page
{
    protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
	protected int tipe { get { return Convert.ToInt32(Request.QueryString["tipe"]); }}
    protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
		if (!IsPostBack)
		{
			fill();
		}
		fillDetail();
	}
	
	protected void fill()
	{
		DataTable dt1 = Db.Rs("SELECT TOP 1 ID, Nama, StartDate, EndDate, Draft, Disable, Keterangan, Tipe FROM Promo WHERE ID = " + myID, Db.Cnn_DB);

		if (dt1.Rows.Count == 1)
		{
			promoname.Value = dt1.Rows[0]["Nama"].ToString();
			start.Value = Convert.ToDateTime(dt1.Rows[0]["StartDate"]).ToString("dd-MM-yyyy");
			end.Value = Convert.ToDateTime(dt1.Rows[0]["EndDate"]).ToString("dd-MM-yyyy");
			if (Convert.ToInt32(dt1.Rows[0]["Draft"]) == 1)
				status.InnerText = "Draft";
			else if (Convert.ToInt32(dt1.Rows[0]["Disable"]) == 1)
				status.InnerText = "Disabled";
			else if (Convert.ToDateTime(dt1.Rows[0]["StartDate"]) <= DateTime.Now || Convert.ToDateTime(dt1.Rows[0]["StartDate"]) >= DateTime.Now)
				status.InnerText = "Inactive";
			else
				status.InnerText = "Active";
			keterangan.Value = dt1.Rows[0]["Keterangan"].ToString();

		}
		dt1.Dispose();
	}
	
	protected void fillDetail()
	{
		DataTable dt2 = Db.Rs("SELECT ID, IDForm, MinValue, MaxValue, Discount, OrderNumber, Nama FROM PromoDetail WHERE IDForm = " + myID, Db.Cnn_DB);
		if (dt2.Rows.Count > 0)
		{
			divdetail.Visible = true;

			int i = 1;
			foreach (DataRow dr in dt2.Rows)
			{
				TableRow tr = new TableRow();
				TableCell td = new TableCell();
				td.Text = i.ToString();
				tr.Cells.Add(td);

				HiddenField hf = new HiddenField();
				hf.ID = "hfpromo_" + dr["ID"].ToString();
				hf.Value = dr["ID"].ToString();

				td = new TableCell();
				td.Controls.Add(hf);
				td.Style.Add("display", "none");
				tr.Cells.Add(td);

				TextBox txt1 = new TextBox();
				txt1.ID = "txtkode_" + dr["ID"].ToString();
				txt1.Text = dr["Nama"].ToString();
				txt1.CssClass = "form-control";
				txt1.TextChanged += new EventHandler(name_Changed);
				txt1.AutoPostBack = true;

				td = new TableCell();
				td.Controls.Add(txt1);
				tr.Cells.Add(td);

				TextBox txt2 = new TextBox();
				txt2.ID = "txtminvalue_" + dr["ID"].ToString();
				txt2.Text = Convert.ToInt64(dr["MinValue"]).ToString();
				txt2.CssClass = "form-control";
				txt2.TextMode = TextBoxMode.Number;
				txt2.TextChanged += new EventHandler(minvalue_Changed);
				txt2.AutoPostBack = true;

				td = new TableCell();
				td.Controls.Add(txt2);
				tr.Cells.Add(td);

				TextBox txt3 = new TextBox();
				txt3.ID = "txtmaxvalue_" + dr["ID"].ToString();
				txt3.Text = Convert.ToInt64(dr["MaxValue"]).ToString();
				txt3.CssClass = "form-control";
				txt3.TextMode = TextBoxMode.Number;
				txt3.TextChanged += new EventHandler(maxvalue_Changed);
				txt3.AutoPostBack = true;

				td = new TableCell();
				td.Controls.Add(txt3);
				tr.Cells.Add(td);

				TextBox txt4 = new TextBox();
				txt4.ID = "txtdiscount_" + dr["ID"].ToString();
				txt4.Text = Convert.ToInt64(dr["Discount"]).ToString();
				txt4.CssClass = "form-control";
				txt4.TextMode = TextBoxMode.Number;
				txt4.TextChanged += new EventHandler(discount_Changed);
				txt4.AutoPostBack = true;

				td = new TableCell();
				td.Controls.Add(txt4);
				tr.Cells.Add(td);

				tb.Rows.Add(tr);

				i++;
			}
		}
		else
		{
			divdetail.Visible = false;
		}
	}
	
	protected void name_Changed(object sender, EventArgs e) 
	{
		TextBox name = (TextBox)sender;
		string id = name.ID.Split('_')[1];
		string nilai = name.Text;
		string query = "UPDATE PromoDetail SET Nama = '" + nilai + "' WHERE ID = " + id;
		Db.Execute(query, Db.Cnn_DB);
	}

	protected void minvalue_Changed(object sender, EventArgs e)
	{
		TextBox minvalue = (TextBox)sender;
		string id = minvalue.ID.Split('_')[1];
		string nilai = minvalue.Text;
		string query = "UPDATE PromoDetail SET MinValue = '" + nilai + "' WHERE ID = " + id;
		Db.Execute(query, Db.Cnn_DB);
	}

	protected void maxvalue_Changed(object sender, EventArgs e)
	{
		TextBox maxvalue = (TextBox)sender;
		string id = maxvalue.ID.Split('_')[1];
		string nilai = maxvalue.Text;
		string query = "UPDATE PromoDetail SET MaxValue = '" + nilai + "' WHERE ID = " + id;
		Db.Execute(query, Db.Cnn_DB);
	}

	protected void discount_Changed(object sender, EventArgs e)
	{
		TextBox discount = (TextBox)sender;
		string id = discount.ID.Split('_')[1];
		string nilai = discount.Text;
		string query = "UPDATE PromoDetail SET Discount = '" + nilai + "' WHERE ID = " + id;
		Db.Execute(query, Db.Cnn_DB);
	}

    protected void save_Click(object sender, EventArgs e)
    {
		string txstart = start.Value;
		string[] timestart = txstart.Split('-');
		string txexpire = end.Value;
		string[] timeexpire = txexpire.Split('-');

		string timenow = "00:00:00.000";
		string timeexpired = "23:59:00.000";

		string query = "EXEC sp_update_promo_lainlain " + myID + ", '" + promoname.Value + "','" + timestart[2] + "-" + timestart[1] + "-" + timestart[0] + " " + timenow + "','" + timeexpire[2] + "-" + timeexpire[1] + "-" + timeexpire[0] + " " + timeexpired + "','" + keterangan.Value + "','" + App.UserNama + "' ";
		
		Db.Execute(query, Db.Cnn_DB);
		/*for (int i = 1; i < tb.Rows.Count; i++)
		{
			string iddetail = ((HiddenField)tb.Rows[i].Cells[1].FindControl("hf_promo_" + i)).Value;
			string namadetail = ((TextBox)tb.Rows[i].Cells[2].FindControl("txt_kode_" + i)).Text;
			string minvaluedetail = ((TextBox)tb.Rows[i].Cells[3].FindControl("txt_min_value_" + i)).Text;
			string maxvaluedetail = ((TextBox)tb.Rows[i].Cells[4].FindControl("txt_max_value_" + i)).Text;
			string discountdetail = ((TextBox)tb.Rows[i].Cells[5].FindControl("txt_discount_" + i)).Text;

			string query2 = "EXEC sp_update_promo_bank ";
			Db.Execute(query2, Db.Cnn_DB);
			warning.InnerText += query2;
		}
		warning.InnerText += query;*/
		Response.Redirect("PromoDetail.aspx?id=" + myID);
	}

	protected void cancel_Click(object sender, EventArgs e)
    {
		Response.Redirect("PromoDetail.aspx?id=" + myID);
	}
}