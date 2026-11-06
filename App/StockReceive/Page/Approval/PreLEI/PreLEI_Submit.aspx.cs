using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;
using System.Configuration;

public partial class PreLEI_Submit : System.Web.UI.Page
{
	protected int id { get { return App.QueryInt(this); } } /// id trans out
	protected string myBrand = ConfigurationManager.AppSettings["KodeBrand"];
	protected string myNomorRTS = "";
	protected string connleiString = ConfigurationManager.ConnectionStrings["Cnn_DB_LEI"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        fillTable();
    }
    //Fill
    protected void fillTable()
    {
		string query = "sp_get_detail_stock_prelei_new_iqbal " + id.ToString();
		DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);
		DataTable dt = ds.Tables[0];
		DataTable dtitem = ds.Tables[1];
		
		if(dt.Rows.Count == 1)
		{
			string NomorRTS = dt.Rows[0]["NO_RTS"].ToString();

			myNomorRTS = NomorRTS;

			no_rtr.InnerText = dt.Rows[0]["NO_RTS"].ToString();
			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = dt.Rows[0]["NO_RTS"].ToString();
			tr.Cells.Add(td);

			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = dt.Rows[0]["NAMA_ASAL"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = dt.Rows[0]["NAMA_TUJUAN"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = dt.Rows[0]["OPERATOR"].ToString();
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Style.Add("text-align","center");
			td.Text = (dt.Rows[0]["OPERATOR_TGL"] == DBNull.Value) ? "" : Convert.ToDateTime(dt.Rows[0]["OPERATOR_TGL"]).ToString("dd-MM-yyyy");
			tr.Cells.Add(td);
			
			tb.Rows.Add(tr);
			
			if(NomorRTS.Contains("P")) {
				tbitem.Visible = true;
				tbitem2.Visible = false;
				int index = 0;
				double TotalWeightOriginal = 0;
				double TotalWeightActual = 0;
				foreach(DataRow dr in dtitem.Rows)
				{
					TableRow tritem = new TableRow();
					TableCell tditem;
					
					index++;
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = index.ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
                    tditem.Text = "<img src=\"/ImageCrop.aspx?path=" + dr["IMAGE"].ToString() + "\" />";
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = dr["TIPE_PRODUCT"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = dr["PLU"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = dr["KODEBARANG"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = dr["ITEM"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","center");
					tditem.Text = dr["FRAME_COLOR"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["WEIGHT_ORIGINAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["WEIGHT_AFTER_QC"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["KADAR_TUKARAN_BELI_ORIGINAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["KADAR_TUKARAN_BELI_ACTUAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["KADAR_TUKARAN_JUAL_ORIGINAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Style.Add("text-align","right");
					tditem.Text = Convert.ToDouble(dr["KADAR_TUKARAN_JUAL_ACTUAL"]).ToString(CultureInfo.CreateSpecificCulture("en-GB"));
					tritem.Cells.Add(tditem);
					
					tbitem.Rows.Add(tritem);
					TotalWeightOriginal += Convert.ToDouble(dr["WEIGHT_ORIGINAL"]);
					TotalWeightActual += Convert.ToDouble(dr["WEIGHT_AFTER_QC"]);
				}
				
				TableRow tr1 = new TableRow();
				tr1.Style.Add("background-color","cadetblue");
				TableCell td1;
				
				td1 = new TableCell();
				td1.ColumnSpan = 7;
				td1.Style.Add("border","solid 1px darkcyan;");
				td1.Style.Add("text-align","left");
				tr1.Style.Add("color","black");
				td1.Text = "<b>TOTAL</b>";
				tr1.Cells.Add(td1);
				
				td1 = new TableCell();
				td1.Style.Add("border","solid 1px darkcyan;");
				td1.Style.Add("text-align","right");
				tr1.Style.Add("color","black");
				td1.Text = "<b>" + Convert.ToDecimal(TotalWeightOriginal).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
				tr1.Cells.Add(td1);
				
				td1 = new TableCell();
				td1.Style.Add("border","solid 1px darkcyan;");
				td1.Style.Add("text-align","right");
				tr1.Style.Add("color","black");
				td1.Text = "<b id='cuy'>" + Convert.ToDecimal(TotalWeightActual).ToString(CultureInfo.CreateSpecificCulture("en-GB")) + "</b>";
				tr1.Cells.Add(td1);
				
				td1 = new TableCell();
				td1.ColumnSpan = 4;
				td1.Style.Add("border","solid 1px darkcyan;");
				td1.Style.Add("text-align","left");
				tr1.Style.Add("color","black");
				td1.Text = "";
				tr1.Cells.Add(td1);
				
				tbitem.Rows.Add(tr1);
				save.Visible = true;
				
			}else{
				tbitem.Visible = false;
				tbitem2.Visible = true;

				int PrevID = 0;
				int indexCuy = 0;

				NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
				string plu = string.Empty;
				foreach (DataRow dr in dtitem.Rows){
					TableRow tritem = new TableRow();
					TableCell tditem;
					
					bool isSamePLU = plu == dr["PLU"].ToString();

					if(!isSamePLU) indexCuy++;

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : indexCuy.ToString() + ".";
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["PLU"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["BRAND"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["NAMA_LOKASI"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["PROTO_NOMOR"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["SUPPLIER_NOMOR"].ToString();
					tritem.Cells.Add(tditem);
					
					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["PRODUCT_ITEM"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["PRODUCT_CATEGORY"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["STONE_DIST"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["SEGMENT"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Convert.ToDecimal(dr["NETTO_WEIGHT"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Convert.ToDecimal(dr["GROSS_WEIGHT"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Convert.ToDecimal(dr["WeightActual"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAM_USD"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Convert.ToDecimal(dr["HARGAR_USD"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = dr["STONE"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = Convert.ToDecimal(dr["TOTAL_BUTIR"]).ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = Convert.ToDecimal(dr["TOTAL_CARAT"]).ToString("N4", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = Convert.ToDecimal(dr["TOTAL_HARGA_M"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = Convert.ToDecimal(dr["TOTAL_HARGA_R"]).ToString("N2", nfi);
					tritem.Cells.Add(tditem);							

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : Str.Tgl(Convert.ToDateTime(dr["APPROVAL_TGL_RETURN"]));
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["APPROVAL_RETURN"].ToString();
					tritem.Cells.Add(tditem);

					tditem = new TableCell();
					tditem.Text = isSamePLU ? "" : dr["KETERANGAN_RETURN"].ToString();
					tritem.Cells.Add(tditem);

					plu = dr["PLU"].ToString();
					
					
					tbitem2.Rows.Add(tritem);
				}
				save.Visible = true;
			}
		}
		else
		{
			no_rtr.InnerText = "";
			save.Visible = false;
			Response.Redirect("PreLEI.aspx");
		}
    }
	
	protected DataSet QuerySPDataSet(string query, string connectionString)
	{
		DataSet ds = new DataSet();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			conn.Open();
			SqlTransaction trans;
			trans = conn.BeginTransaction();
			try
			{
				SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
				sda.SelectCommand.CommandTimeout = 3000;
				sda.Fill(ds);
				sda.Dispose();
				trans.Commit();
				conn.Close();
			}
			catch(Exception ex)
			{
				string ErrorMessage = ex.Message.ToString();
				trans.Rollback();
				conn.Close();
			}
		}
		return ds;
	}
	
	protected void save_Click(object sender, EventArgs e)
	{
		try
		{
			string query = "EXEC sp_approve_data_prelei_stock_receive " + id.ToString() + ",'" + App.UserNama + "','" + myBrand + "'";
			Db.Execute(query, Db.Cnn_DB);
			string result = ExecSPLEI();
			if(string.IsNullOrEmpty(result))
			{
				Db.Execute("UPDATE PreLei_StockReceive SET StatusHIT = 1, Response = 'Success' WHERE ID = " + id.ToString(), Db.Cnn_DB);
				string scriptText = "alert(`Sukses Approve...`); window.location=`" + Request.ApplicationPath + "/Page/Approval/PreLEI/PreLEI.aspx`";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
			}
			else
			{
				string scriptText = "alert(`" + result + "`); window.location=`" + Request.ApplicationPath + "/Page/Approval/PreLEI/PreLEI.aspx`";
				ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
			}
		}
		catch(Exception ex)
		{
			string scriptText = "alert(`Terjadi Kesalahan " + ex.Message.ToString() + "`); window.location=`" + Request.ApplicationPath + "/Page/Approval/PreLEI/PreLEI.aspx`";
			ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
		}
	}
	
	protected string ExecSPLEI()
	{
		//string Cnn_DB = "Data Source=202.133.5.149,3315;Initial Catalog=lei_db;Persist Security Info=True;User ID=leiadminsuper;Password=CMKxLEIber5atU777";
		
		string Cnn_DB = connleiString;

		string tipe;


		if(myNomorRTS.Contains("P")) {
			tipe = "PG";
		} else{
			tipe = "DJ";
		}
	


		if(tipe == "PG"){
			string query = "exec sp_get_data_product_rts_from_jaws_new_banget '" + myBrand + "', " + id.ToString() + ", 0";
			return Db.SingleString(query, Cnn_DB);
		}else{
			string query = "exec sp_get_data_product_rts_dj_from_jaws '" + myBrand + "', " + id.ToString() + ", 0";
			return Db.SingleString(query, Cnn_DB);
		}
	}
}