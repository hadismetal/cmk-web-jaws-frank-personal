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

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;

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
    protected void fillTable(object sender, EventArgs e)
    {
        //fillSouvenir();
		fillSouvenirNew();
    }

    protected void fillSouvenirNew()
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

                string query = "SELECT A.ID, A.IDProduct, B.Kode, B.Nama, A.InHand [TotalStock], ISNULL(C.Qty,0) [TotalOutForSales] FROM StockActualSouvenir A " +
                "JOIN Souvenir B ON A.IDProduct = B.ID " +
                "LEFT JOIN (SELECT IDProduct, Qty FROM SalesOrderSouvenir WHERE IDForm = @ID AND IDProduct IS NOT NULL) C ON A.IDProduct = C.IDProduct " +
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
                    cb.ID = "cbsov" + index.ToString();
                    cb.Checked = false;
                    cb.Style.Add("zoom", "2");
                    cb.Attributes.Add("onclick", "return false;");
                    cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    HiddenField hf = new HiddenField();
                    hf.ID = "hfsov" + index.ToString();
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
                    txt.ID = "ssov" + index.ToString();
                    txt.Attributes["style"] = "width: 200px;";
                    txt.CssClass = "form-control";
                    txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txt.Attributes.Add("onblur", "onblursouvenir" + index.ToString() + "(this);");
                    txt.Attributes.Add("onclick", "onblursouvenir" + index.ToString() + "(this);");
                    txt.Attributes["type"] = "number";
                    txt.Attributes["value"] = "0";
                    if (totalout > 0)
                    {
                        txt.Attributes["value"] = Convert.ToInt32(totalout).ToString();
                    }
                    txt.Attributes["max"] = Convert.ToInt32(r["TotalStock"]).ToString();
                    txt.Attributes["min"] = "0";
                    txt.Attributes["kodesov"] = r["Kode"].ToString();
                    txt.Attributes["namasov"] = r["Nama"].ToString();

                    HiddenField hfsovmax = new HiddenField();
                    hfsovmax.ID = "hfsovmax" + index.ToString();
                    hfsovmax.Value = Convert.ToInt32(r["ID"]).ToString();
                    hfsovmax.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(txt);
                    td.Controls.Add(hfsovmax);
                    tr.Cells.Add(td);

                    tb_souv_venir.Rows.Add(tr);

                    ClientScript.RegisterStartupScript(Page.GetType(), "script_souvenir_" + index.ToString(), " " + setJavascriptKeyPressStone(index.ToString(), "ssov" + index.ToString(), txt.Text) + " ", true);

                    index++;
                }
            }
        }
    }

    protected void fillSouvenir()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myIDSO);

        if (d != null)
        {
            //Cancel-Link
            cancel.HRef = "TradeIn_Data.aspx";
            cancel.HRef += "?id=" + myID;
            cancel.HRef += "&so=" + myIDSO;
            cancel.HRef += "&ro=" + myIDRO;
            var rs = from p in db.StockActualSouvenir
                     where p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.InHand > 0
                        && p.Souvenir.Draft == false && p.Souvenir.Disable == false
                     select p;

            if (kw.Text != "")
            {
                rs = from p in rs
                     where p.Souvenir.Nama.Contains(kw.Text)
                     || p.Souvenir.Kode.Contains(kw.Text)
                     select p;
            }
			
			rs = rs.OrderBy(p => p.Souvenir.Nama);

            int index = 1;

            int itemindex = 1;
            int PLU = 0;
			
			foreach (var r in rs)
			{
				TableRow tr = new TableRow();
				TableCell td;
				
				CheckBox cb = new CheckBox();
				cb.ID = "cbsov" + index.ToString();
				cb.Checked = false;
				cb.Style.Add("zoom", "2");
				cb.Attributes.Add("onclick","return false;");
				cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				HiddenField hf = new HiddenField();
				hf.ID = "hfsov" + index.ToString();
				hf.Value = r.IDProduct.ToString();
				hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				td = new TableCell();
				td.Controls.Add(cb);
				td.Controls.Add(hf);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.Souvenir.Kode;
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.Souvenir.Nama;
				tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = r.InHand.ToString("N0");
				tr.Cells.Add(td);
				
				var ncuy = (from z in db.SalesOrderSouvenir
						   where z.IDForm == myIDSO
							  && z.IDProductDJ == null
							  && z.IDProductPG == null
							  && z.IDProductGJ == null
							  && z.IDProductLD == null
							  && z.IDProduct == r.IDProduct
							  && z.IDForm == myID
						   select z).FirstOrDefault();
				
				TextBox txt = new TextBox();
				txt.ID = "ssov" + index.ToString();
				txt.Attributes["style"] = "width: 200px;";
				txt.CssClass = "form-control";
				txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				txt.Attributes.Add("onblur", "onblursouvenir" + index.ToString() + "(this);");
				txt.Attributes.Add("onclick", "onblursouvenir" + index.ToString() + "(this);");
				txt.Attributes["type"] = "number";
				txt.Attributes["value"] = "0";
				if (ncuy != null)
				{
					txt.Attributes["value"] = Convert.ToInt32(ncuy.Qty).ToString();
				}
				txt.Attributes["max"] = r.InHand.ToString();
				txt.Attributes["min"] = "0";
				txt.Attributes["kodesov"] = r.Souvenir.Kode;
				txt.Attributes["namasov"] = r.Souvenir.Nama;
				
				HiddenField hfsovmax = new HiddenField();
				hfsovmax.ID = "hfsovmax" + index.ToString();
				hfsovmax.Value = Convert.ToInt32(r.ID).ToString();
				hfsovmax.ClientIDMode = System.Web.UI.ClientIDMode.Static;
				
				td = new TableCell();
				td.Controls.Add(txt);
				td.Controls.Add(hfsovmax);
				tr.Cells.Add(td);

				tb_souv_venir.Rows.Add(tr);
				
				ClientScript.RegisterStartupScript(Page.GetType(), "script_souvenir_" + index.ToString(), " " + setJavascriptKeyPressStone(index.ToString(),"ssov" + index.ToString(), txt.Text) + " ", true);

				index++;
			}
        }
    }
	
    //Display Column
    protected string TD_HyperLink(int ID, int itemindex, int PLU, int itemindexSouvenir)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_SouvenirAddSave.aspx";
        href += "?id=" + myID;
        href += "&so=" + myIDSO;
        href += "&ro=" + myIDRO;
        href += "&item=" + itemindex;
        href += "&itemsouvenir=" + itemindexSouvenir;
        href += "&iditem=" + ID;
        href += "&plu=" + PLU;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
	
	protected void savesouvenir_Click(object sender, EventArgs e)
    {
        int index = 1;

		HiddenField hf = new HiddenField();
		string hftxt = "";
		
		TextBox txt = new TextBox();
		string txttxt = "0";
		
		CheckBox ischoosed = new CheckBox();
		bool ischoosedbool = false;
		
		HiddenField hfsovmax = new HiddenField();
		int inhand = 0;
		int totalout = 0;

		foreach (TableRow row in tb_souv_venir.Rows)
		{
			try
			{
				//ID Souvenir
				hf = (HiddenField)row.FindControl("hfsov" + index.ToString());
				hftxt = hf.Value;

				txt = (TextBox)row.FindControl("ssov" + index.ToString());
				txttxt = txt.Text;
				
				hfsovmax = (HiddenField)row.FindControl("hfsovmax" + index.ToString());
				inhand = Convert.ToInt32(hfsovmax.Value);
				totalout = Convert.ToInt32(txttxt);
				
				ischoosed = (CheckBox)row.FindControl("cbsov" + index.ToString());
				ischoosedbool = ischoosed.Checked ? true : false;

				var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hftxt) && p.IDForm == myIDSO);
				if (d != null)
				{
					if (!ischoosedbool)
					{
						db.SalesOrderSouvenir.DeleteOnSubmit(d);
						db.SubmitChanges();
					}
					else
					{
						decimal ModalRph = Db.SingleDecimal("SELECT TOP 1 ISNULL(Harga,0) [Harga] FROM Souvenir WHERE ID = " + d.IDProduct, Db.Cnn_DB);
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
						decimal ModalRph = Db.SingleDecimal("SELECT TOP 1 ISNULL(Harga,0) [Harga] FROM Souvenir WHERE ID = " + hftxt, Db.Cnn_DB);
						
						SalesOrderSouvenir n = new SalesOrderSouvenir();
						n.IDForm = myIDSO;
						n.IDProduct = Convert.ToInt32(hftxt);
						n.ModalRupiah = ModalRph;
						n.TotalRupiah = 0m;
						n.TotalModalRupiah = totalout * ModalRph;
						n.Qty = totalout;
						
						db.SalesOrderSouvenir.InsertOnSubmit(n);
						db.SubmitChanges();

						n.Invoice = Document.Generate_Nomor_SalesOrderSouvenir(n);
						db.SubmitChanges();
					}
				}
			}
			catch (Exception ex)
			{
				ClientScript.RegisterStartupScript(Page.GetType(), "script_souvenir__" + index.ToString(), "alert(" + hftxt + ")", true);
			}
			index++;
		}

		//Redirect
		Response.Redirect("TradeIn_Data.aspx?id=" + myID +"&so=" + myIDSO +"&ro=" + myIDRO + "#div_souvenir");
    }
	
	protected string setJavascriptKeyPressStone(string index, string idsouvenir, string qty)
	{
		string x = string.Empty;
		x += "$(window).load(function(){ ";
		x += "onblursouvenir" + index + "(document.getElementById('" + idsouvenir + "')); ";
		x += "}); ";
		
		x += "function onblursouvenir" + index + "(e){ " ;
		x += "var idsouvenir = document.getElementById('" + idsouvenir + "'); ";
		x += "var qtymax = parseInt(idsouvenir.getAttribute('max')); ";
		x += "var kode = idsouvenir.getAttribute('kodesov'); ";
		x += "var nama = idsouvenir.getAttribute('namasov'); ";
		x += "var cb = document.getElementById('cbsov" + index + "'); " ;
		x += "if(e.value != ''){ ";
		x += "var qty = parseInt(e.value); ";
		//x += "console.log(qtymax); console.log(kode); console.log(nama); console.log(qty); ";
		x += "if(qty > qtymax){ " ;
		x += "alert('Total Melebihi Total Stock Yaitu :' + qtymax.toString()); ";
		x += "idsouvenir.value = '0'; ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white' ";
		x += "} ";
		x += "else if(qty < 0){ ";
		x += "alert('Total Tidak Boleh Kurang Dari 0'); ";
		x += "idsouvenir.value = '0'; ";
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
		x += "idsouvenir.value = '0'; ";
		x += "cb.checked = false; ";
		x += "cb.parentElement.parentElement.parentElement.style.backgroundColor = 'white'; ";
		x += "} ";
		x += "} ";
		
		x += "function onchecksouvenir" + index + "(e){ " ;
		x += "var idsouvenir = document.getElementById('" + idsouvenir + "'); ";
		x += "var ssov = document.getElementById('ssov" + index + "'); " ;
		x += "if(e.checked) { ";
		x += "ssov.value = '1'; ";
		x += "e.parentElement.parentElement.parentElement.style.backgroundColor = 'lightblue'; ";
		x += "} ";
		x += "else { ";
		x += "ssov.value = '0'; ";
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