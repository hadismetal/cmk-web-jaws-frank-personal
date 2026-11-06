using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_ItemAdd : System.Web.UI.Page 
{
	protected int myID { get { return App.QueryInt(this); } }
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

    // Prefetch Harga+Tipe dari katalog Packaging untuk semua baris yang tampil di tb_pak_kaging —
    // dulu di-query per baris raw-SQL terpisah (Harga 2x, Tipe 1x) di savepackaging_Click.
    private class PackagingInfo
    {
        public decimal Harga;
        public int Tipe;
    }
    private Dictionary<int, PackagingInfo> PrefetchPackagingInfo(List<int> productIds)
    {
        Dictionary<int, PackagingInfo> result = new Dictionary<int, PackagingInfo>();
        if (productIds == null || productIds.Count == 0) return result;
        string idList = string.Join(",", productIds.Distinct());
        string query = "SELECT ID, ISNULL(Harga,0) AS Harga, ISNULL(Tipe,1) AS Tipe FROM Packaging WHERE ID IN (" + idList + ")";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["ID"])] = new PackagingInfo
            {
                Harga = Convert.ToDecimal(row["Harga"]),
                Tipe = Convert.ToInt32(row["Tipe"])
            };
        }
        return result;
    }

	protected void Page_Load(object sender, EventArgs e)
	{
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) 
		{
			//Generate
			fillFilter();
			//if (App.Refresh(this)) fillTable(sender, e);
		}
		fillTable(sender, e);
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() 
	{
		if (App.Refresh(this)) 
		{
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//fillPackaging();
		fillPackagingNew();
    }

	protected void fillPackagingNew()
	{
		int TipeLokasi = 0;
		int IDLokasi = 0;
		string keyword = kw.Text;
		using (Map_DB db = new Map_DB())
		{
			var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
			if (d != null)
			{
                cancel.HRef += "?id=" + myID;
                TipeLokasi = Convert.ToInt32(d.TipeLokasi);
				IDLokasi = Convert.ToInt32(d.IDLokasi);

				string query = "SELECT A.ID, A.IDProduct, B.Kode, B.Nama, A.InHand [TotalStock], ISNULL(C.Qty,0) [TotalOutForSales] FROM StockActualPackaging A " +
				"JOIN Packaging B ON A.IDProduct = B.ID " +
				"LEFT JOIN (SELECT IDProduct, Qty FROM SalesOrderPackaging WHERE IDForm = @ID) C ON A.IDProduct = C.IDProduct " +
				"WHERE IDLokasi = @IDLokasi " +
				"AND TipeLokasi = @TipeLokasi " +
				"AND (@KEYWORD = '' OR (@KEYWORD <> '' AND CHARINDEX(@KEYWORD,B.Kode + B.Nama) > 0)) " +
				"AND InHand > 0 " +
				"AND B.Disable = 0 " +
				"AND B.Draft = 0 " +
				"ORDER BY B.Nama";
				List<SqlParameter> parameters = new List<SqlParameter>
			{
				new SqlParameter("@IDLokasi", SqlDbType.Int){ Value = IDLokasi },
				new SqlParameter("@TipeLokasi", SqlDbType.Int){ Value = TipeLokasi },
				new SqlParameter("@KEYWORD", SqlDbType.VarChar){ Value = keyword },
				new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
			};
				DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);

				int index = 1;

				foreach (DataRow r in dt.Rows)
				{
					TableRow tr = new TableRow();
					TableCell td;

					CheckBox cb = new CheckBox();
					cb.ID = "cbpak" + index.ToString();
					cb.Checked = false;
					cb.Style.Add("zoom", "2");
					cb.Attributes.Add("onclick", "return false;");
					cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

					HiddenField hf = new HiddenField();
					hf.ID = "hfpak" + index.ToString();
					hf.Value = r["IDProduct"].ToString();
					hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

					td = new TableCell();
					td.Controls.Add(cb);
					td.Controls.Add(hf);
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = r["Kode"].ToString();
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = r["Nama"].ToString();
					tr.Cells.Add(td);

					td = new TableCell();
					td.Text = Convert.ToInt32(r["TotalStock"]).ToString("N0");
					tr.Cells.Add(td);

					int totalout = Convert.ToInt32(r["TotalOutForSales"]);
					TextBox txt = new TextBox();
					txt.ID = "spak" + index.ToString();
					txt.Attributes["style"] = "width: 200px;";
					txt.CssClass = "form-control";
					txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
					txt.Attributes.Add("onblur", "onblurpackaging" + index.ToString() + "(this);");
					txt.Attributes.Add("onclick", "onblurpackaging" + index.ToString() + "(this);");
					txt.Attributes["type"] = "number";
					txt.Attributes["value"] = "0";
					if (totalout > 0)
					{
						txt.Attributes["value"] = Convert.ToInt32(totalout).ToString();
					}
					txt.Attributes["max"] = Convert.ToInt32(r["TotalStock"]).ToString();
					txt.Attributes["min"] = "0";
					txt.Attributes["kodepak"] = r["Kode"].ToString();
					txt.Attributes["namapak"] = r["Nama"].ToString();

					HiddenField hfpakmax = new HiddenField();
					hfpakmax.ID = "hfpakmax" + index.ToString();
					hfpakmax.Value = Convert.ToInt32(r["ID"]).ToString();
					hfpakmax.ClientIDMode = System.Web.UI.ClientIDMode.Static;

					td = new TableCell();
					td.Controls.Add(txt);
					td.Controls.Add(hfpakmax);
					tr.Cells.Add(td);

					tb_pak_kaging.Rows.Add(tr);

					ClientScript.RegisterStartupScript(Page.GetType(), "script_packaging_" + index.ToString(), " " + setJavascriptKeyPressStone(index.ToString(), "spak" + index.ToString(), txt.Text) + " ", true);

					index++;
				}
			}
			else
			{
				DataError();
			}
		}
    }

	//Display Column
	protected string TD_HyperLink(int ID, int itemindex, int PLU) {
		string x = string.Empty;
		string href = string.Empty;

        itemindex = 1;
        PLU = 0;
		
		href += "Sales_PackagingAddSave.aspx";
		href += "?id=" + myID;
		href += "&item=" + itemindex;
		href += "&iditem=" + ID;
        href += "&plu=" + PLU;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  ";
		x += "</a>";

		return x;
	}
	
	protected void savepackaging_Click(object sender, EventArgs e)
    {
        int index = 1;

		HiddenField hf = new HiddenField();
		string hftxt = "";
		
		TextBox txt = new TextBox();
		string txttxt = "0";
		
		CheckBox ischoosed = new CheckBox();
		bool ischoosedbool = false;
		
		HiddenField hfpakmax = new HiddenField();
		int inhand = 0;
		int totalout = 0;

		using (Map_DB db = new Map_DB())
		{
            // Pre-pass: kumpulkan semua IDProduct yang tampil di tb_pak_kaging dulu, supaya
            // SalesOrderPackaging existing + Harga/Tipe katalog Packaging bisa di-bulk-fetch
            // sekali sebelum loop utama — dulu masing-masing di-query PER BARIS (untuk SEMUA
            // baris katalog yang tampil di layar, bukan cuma yang dicek/diubah).
            List<int> productIds = new List<int>();
            {
                int preIndex = 1;
                foreach (TableRow row in tb_pak_kaging.Rows)
                {
                    try
                    {
                        HiddenField preHf = (HiddenField)row.FindControl("hfpak" + preIndex.ToString());
                        productIds.Add(Convert.ToInt32(preHf.Value));
                    }
                    catch (Exception) { }
                    preIndex++;
                }
            }
            // ExecuteQuery<T>() (raw SQL -> entity ter-track penuh, aman untuk DeleteOnSubmit/
            // property mutation/SubmitChanges sesudahnya) — BUKAN .Where()/query-syntax LINQ.
            // Baik .Where(lambda) maupun "from...where...select" sama-sama translate ke pemanggilan
            // method bernama "Where", dan di file yang punya "using System.Linq.Dynamic;" ini,
            // SETIAP pemanggilan "Where" (apa pun bentuknya, kondisi tunggal atau majemuk) salah
            // resolve ke overload Func<T,int,bool> (indexed) -> CS1593. ExecuteQuery tidak lewat
            // Where sama sekali, jadi tidak mungkin kena masalah ini.
            // (Catatan: SalesOrderPackaging.IDProduct itu int biasa, bukan nullable — beda dengan
            // SalesOrderSouvenir.IDProduct yang memang int?.)
            List<SalesOrderPackaging> existingRows = db.ExecuteQuery<SalesOrderPackaging>(
                "SELECT * FROM SalesOrderPackaging WHERE IDForm = {0}", myID).ToList();
            Dictionary<int, SalesOrderPackaging> existingByProduct = existingRows.ToDictionary(p => p.IDProduct);
            Dictionary<int, PackagingInfo> packagingInfoByProduct = PrefetchPackagingInfo(productIds);

            foreach (TableRow row in tb_pak_kaging.Rows)
            {
                try
                {
                    //ID Packaging
                    hf = (HiddenField)row.FindControl("hfpak" + index.ToString());
                    hftxt = hf.Value;

                    txt = (TextBox)row.FindControl("spak" + index.ToString());
                    txttxt = txt.Text;

                    hfpakmax = (HiddenField)row.FindControl("hfpakmax" + index.ToString());
                    inhand = Convert.ToInt32(hfpakmax.Value);
                    totalout = Convert.ToInt32(txttxt);

                    ischoosed = (CheckBox)row.FindControl("cbpak" + index.ToString());
                    ischoosedbool = ischoosed.Checked ? true : false;

                    int idProductRow = Convert.ToInt32(hftxt);
                    SalesOrderPackaging d;
                    existingByProduct.TryGetValue(idProductRow, out d);
                    if (d != null)
                    {
                        if (!ischoosedbool)
                        {
                            db.SalesOrderPackaging.DeleteOnSubmit(d);
                            db.SubmitChanges();
                        }
                        else
                        {
                            PackagingInfo pinfo;
                            packagingInfoByProduct.TryGetValue(d.IDProduct, out pinfo);
                            decimal ModalRph = (pinfo != null) ? pinfo.Harga : 0m;
                            d.ModalRupiah = ModalRph;
                            d.TotalRupiah = 0m;
                            d.TotalModalRupiah = totalout * ModalRph;
                            d.Qty = totalout;

                            db.SubmitChanges();
                        }
                    }
                    else
                    {
                        if (ischoosedbool)
                        {
                            PackagingInfo pinfo;
                            packagingInfoByProduct.TryGetValue(idProductRow, out pinfo);
                            decimal ModalRph = (pinfo != null) ? pinfo.Harga : 0m;

                            SalesOrderPackaging n = new SalesOrderPackaging();
                            n.IDForm = myID;
                            n.IDProduct = idProductRow;
                            n.ModalRupiah = ModalRph;
                            n.TotalRupiah = 0m;
                            n.TotalModalRupiah = totalout * ModalRph;
                            n.Qty = totalout;

                            db.SalesOrderPackaging.InsertOnSubmit(n);
                            db.SubmitChanges();

                            // Generate_Nomor_SalesOrderPackaging membaca n.SalesOrder (nav property) —
                            // pola di SELURUH codebase (semua varian brand) selalu memanggil ini SESUDAH
                            // InsertOnSubmit+SubmitChanges, jadi urutan ini dipertahankan, bukan digabung
                            // jadi 1 SubmitChanges (sudah dicoba, ternyata bukan optimasi yang aman).
                            n.Invoice = Document.Generate_Nomor_SalesOrderPackaging(n);
                            db.SubmitChanges();

                            int tipe = (pinfo != null) ? pinfo.Tipe : 1;
                            Db.Execute("UPDATE SalesOrderPackaging SET Tipe = '" + tipe.ToString() + "' WHERE ID = " + n.ID, Db.Cnn_DB);

                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "script_packaging_" + index.ToString(), "alert(" + hftxt + ")", true);
                }
                index++;
            }
        }

		//Redirect
		Response.Redirect("Sales_Data.aspx?id=" + myID + "#div_packaging");
    }
	
	protected string setJavascriptKeyPressStone(string index, string idpackaging, string qty)
	{
		string x = string.Empty;
		//x += "$(document).ready(function(){ ";
		x += "$(window).load(function(){ ";
		x += "onblurpackaging" + index + "(document.getElementById('" + idpackaging + "')); ";
		x += "}); ";
		//x += "}); ";
		
		/*x += "function onloadpackaging" + index + "(e){ ";
		x += "console.log(e.value); ";
		x += "} ";*/
		
		x += "function onblurpackaging" + index + "(e){ " ;
		x += "var idpakkaging = document.getElementById('" + idpackaging + "'); ";
		x += "var qtymax = parseInt(idpakkaging.getAttribute('max')); ";
		x += "var kode = idpakkaging.getAttribute('kodepak'); ";
		x += "var nama = idpakkaging.getAttribute('namapak'); ";
		x += "var cb = document.getElementById('cbpak" + index + "'); " ;
		x += "if(e.value != ''){ ";
		x += "var qty = parseInt(e.value); ";
		//x += "console.log(qtymax); console.log(kode); console.log(nama); console.log(qty); ";
		x += "if(qty > qtymax){ " ;
		x += "alert('Total Melebihi Total Stock Yaitu :' + qtymax.toString()); ";
		x += "idpakkaging.value = '0'; ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white' ";
		x += "} ";
		x += "else if(qty < 0){ ";
		x += "alert('Total Tidak Boleh Kurang Dari 0'); ";
		x += "idpakkaging.value = '0'; ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white' ";
		x += "} ";
		x += "else if(qty == 0){ ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white' ";
		x += "} ";
		x += "else{ ";
		x += "cb.checked = true;";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'lightblue' ";
		x += "} ";
		x += "} ";
		x += "else{ ";
		x += "idpakkaging.value = '0'; ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white'; ";
		x += "} ";
		x += "} ";
		
		x += "function oncheckpackaging" + index + "(e){ " ;
		x += "var idpakkaging = document.getElementById('" + idpackaging + "'); ";
		x += "var spak = document.getElementById('spak" + index + "'); " ;
		x += "if(e.checked) { ";
		x += "spak.value = '1'; ";
		x += "e.parentElement.parentElement.parentElement.style.backgroundColor = 'lightblue'; ";
		x += "} ";
		x += "else { ";
		x += "spak.value = '0'; ";
		x += "e.parentElement.parentElement.parentElement.style.backgroundColor = 'white'; ";
		x += "} ";
		x += "} ";
		
		return x;
	}

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                cmd.ExecuteNonQuery();
            }
        }
    }
}