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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myIDSO { get { return App.QueryInt(this, "so"); } }
    protected int myIDRO { get { return App.QueryInt(this, "ro"); } }
    protected int tipe { get { return App.QueryInt(this,"tipe"); } }

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
	protected void DataError() { Response.Redirect("TradeIn.aspx"); }

	//Form
	protected void fillForm() 
	{
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO && p.Draft);
		if (d == null) DataError();
		else 
		{
			//Cancel-Link
            cancel.HRef = "TradeIn.aspx";
            cancel.HRef += "?id=" + myID;
            cancel.HRef += "&so=" + myIDSO;
            cancel.HRef += "&ro=" + myIDRO;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) 
	{
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
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);
            if (d != null)
            {
                //Cancel-Link
                cancel.HRef = "TradeIn_Data.aspx";
                cancel.HRef += "?id=" + myID;
                cancel.HRef += "&so=" + myIDSO;
                cancel.HRef += "&ro=" + myIDRO;

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
                new SqlParameter("@ID", SqlDbType.Int){ Value = myIDSO }
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

    protected void fillPackaging() 
    {
        var d = db.SalesOrder.SingleOrDefault(p=>p.ID == myIDSO);

        if(d!=null)
        {
            //Cancel-Link
            cancel.HRef = "TradeIn_Data.aspx";
            cancel.HRef += "?id=" + myID;
            cancel.HRef += "&so=" + myIDSO;
            cancel.HRef += "&ro=" + myIDRO;
            var rs = from p in db.StockActualPackaging
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
                     select p;
                
            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Packaging.Nama.Contains(kw.Text)
                     || p.Packaging.Kode.Contains(kw.Text)
                     select p;
            }
			
			rs = rs.OrderBy(p => p.Packaging.Nama);

            int index = 1;

            int itemindex = 1;
            int PLU = 0;
            
			foreach (var r in rs)
			{
				TableRow tr = new TableRow();
				TableCell td;
				
				CheckBox cb = new CheckBox();
				cb.ID = "cbpak" + index.ToString();
				cb.Checked = false;
				cb.Style.Add("zoom", "2");
				cb.Attributes.Add("onclick","return false;");
				cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				HiddenField hf = new HiddenField();
				hf.ID = "hfpak" + index.ToString();
				hf.Value = r.IDProduct.ToString();
				hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				td = new TableCell();
				td.Controls.Add(cb);
				td.Controls.Add(hf);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.Packaging.Kode;
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.Packaging.Nama;
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = r.InHand.ToString("N0");
				tr.Cells.Add(td);
				
				var ncuy = (from z in db.SalesOrderPackaging
						   where z.IDForm == myIDSO
							  && z.IDProductDJ == null
							  && z.IDProductPG == null
							  && z.IDProductGJ == null
							  && z.IDProductLD == null
							  && z.IDProduct == r.IDProduct
							  && z.IDForm == myIDSO
						   select z).FirstOrDefault();
				
				TextBox txt = new TextBox();
				txt.ID = "spak" + index.ToString();
				txt.Attributes["style"] = "width: 200px;";
				txt.CssClass = "form-control";
				txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				txt.Attributes.Add("onblur", "onblurpackaging" + index.ToString() + "(this);");
				txt.Attributes.Add("onclick", "onblurpackaging" + index.ToString() + "(this);");
				txt.Attributes["type"] = "number";
				txt.Attributes["value"] = "0";
				if (ncuy != null)
				{
					txt.Attributes["value"] = Convert.ToInt32(ncuy.Qty).ToString();
				}
				txt.Attributes["max"] = r.InHand.ToString();
				txt.Attributes["min"] = "0";
				txt.Attributes["kodepak"] = r.Packaging.Kode;
				txt.Attributes["namapak"] = r.Packaging.Nama;
				
				HiddenField hfpakmax = new HiddenField();
				hfpakmax.ID = "hfpakmax" + index.ToString();
				hfpakmax.Value = Convert.ToInt32(r.ID).ToString();
				hfpakmax.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				td = new TableCell();
				td.Controls.Add(txt);
				td.Controls.Add(hfpakmax);
				tr.Cells.Add(td);

				tb_pak_kaging.Rows.Add(tr);
				
				ClientScript.RegisterStartupScript(Page.GetType(), "script_packaging_" + index.ToString(), " " + setJavascriptKeyPressStone(index.ToString(),"spak" + index.ToString(), txt.Text) + " ", true);
				
				index++;
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
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
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

				var d = db.SalesOrderPackaging.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hftxt) && p.IDForm == myIDSO);
				if (d != null)
				{
					if (!ischoosedbool)
					{
						db.SalesOrderPackaging.DeleteOnSubmit(d);
						db.SubmitChanges();
					}
					else
					{
						decimal ModalRph = Db.SingleDecimal("SELECT TOP 1 ISNULL(Harga,0) [Harga] FROM Packaging WHERE ID = " + d.IDProduct, Db.Cnn_DB);
						d.ModalRupiah = ModalRph;
						d.TotalRupiah = 0m;//totalout * ModalRph;
						d.TotalModalRupiah = totalout * ModalRph;
						d.Qty = totalout;

						db.SubmitChanges();
					}
				}
				else
				{
					if (ischoosedbool)
					{
						decimal ModalRph = Db.SingleDecimal("SELECT TOP 1 ISNULL(Harga,0) [Harga] FROM Packaging WHERE ID = " + hftxt, Db.Cnn_DB);
						
						SalesOrderPackaging n = new SalesOrderPackaging();
						n.IDForm = myIDSO;
						n.IDProduct = Convert.ToInt32(hftxt);
						n.ModalRupiah = ModalRph;
						n.TotalRupiah = 0m;//totalout * ModalRph;
						n.TotalModalRupiah = totalout * ModalRph;
						n.Qty = totalout;
						
						db.SalesOrderPackaging.InsertOnSubmit(n);
						db.SubmitChanges();

						n.Invoice = Document.Generate_Nomor_SalesOrderPackaging(n);
						db.SubmitChanges();
						
						int tipe = Db.SingleInteger("SELECT TOP 1 ISNULL(Tipe,1) FROM Packaging WHERE ID = " + n.IDProduct, Db.Cnn_DB);
						Db.Execute("UPDATE SalesOrderPackaging SET Tipe = '" + tipe.ToString() + "' WHERE ID = " + n.ID,Db.Cnn_DB);
					}
				}
			}
			catch (Exception ex)
			{
				ClientScript.RegisterStartupScript(Page.GetType(), "script_packaging_" + index.ToString(), "alert(" + hftxt + ")", true);
			}
			index++;
		}

        //Redirect
        Response.Redirect("TradeIn_Data.aspx?id=" + myID + "&so=" + myIDSO + "&ro=" + myIDRO + "#div_packaging");
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