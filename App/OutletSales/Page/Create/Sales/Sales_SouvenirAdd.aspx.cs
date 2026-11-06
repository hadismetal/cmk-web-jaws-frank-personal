using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    // Prefetch Harga dari katalog Souvenir untuk semua baris yang tampil di tb_souv_venir — dulu
    // di-query per baris raw-SQL terpisah di savesouvenir_Click.
    private Dictionary<int, decimal> PrefetchSouvenirHarga(List<int> productIds)
    {
        Dictionary<int, decimal> result = new Dictionary<int, decimal>();
        if (productIds == null || productIds.Count == 0) return result;
        string idList = string.Join(",", productIds.Distinct());
        string query = "SELECT ID, ISNULL(Harga,0) AS Harga FROM Souvenir WHERE ID IN (" + idList + ")";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>(), CommandType.Text);
        foreach (DataRow row in dt.Rows)
        {
            result[Convert.ToInt32(row["ID"])] = Convert.ToDecimal(row["Harga"]);
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
    protected void refreshPage(object sender, EventArgs e)
    {
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
            var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                cancel.HRef += "?id=" + myID;
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
                new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
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

    //Display Column
    protected string TD_HyperLink(int ID, int itemindex, int PLU, int itemindexSouvenir)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Sales_SouvenirAddSave.aspx";
        href += "?id=" + myID;
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

        using (Map_DB db = new Map_DB())
        {
            // Pre-pass: kumpulkan semua IDProduct yang tampil di tb_souv_venir dulu, supaya
            // SalesOrderSouvenir existing + Harga katalog Souvenir bisa di-bulk-fetch sekali
            // sebelum loop utama — dulu masing-masing di-query PER BARIS (untuk SEMUA baris
            // katalog yang tampil di layar, bukan cuma yang dicek/diubah).
            List<int> productIds = new List<int>();
            {
                int preIndex = 1;
                foreach (TableRow row in tb_souv_venir.Rows)
                {
                    try
                    {
                        HiddenField preHf = (HiddenField)row.FindControl("hfsov" + preIndex.ToString());
                        productIds.Add(Convert.ToInt32(preHf.Value));
                    }
                    catch (Exception) { }
                    preIndex++;
                }
            }
            // IDProduct nullable & bisa benar2 NULL untuk baris souvenir yang bukan dari katalog ini
            // (lihat filter "IDProduct IS NOT NULL" di fillSouvenirNew) — exclude biar konsisten
            // dengan SingleOrDefault(p => p.IDProduct == X) yang lama (tidak akan match baris NULL).
            // ExecuteQuery<T>() (raw SQL -> entity ter-track penuh) — BUKAN .Where()/query-syntax.
            // Lihat komentar lengkap di Sales_PackagingAdd.aspx.cs: di file yang punya "using
            // System.Linq.Dynamic;", SETIAP pemanggilan method "Where" (lambda ATAU query-syntax,
            // kondisi tunggal ATAU majemuk) salah resolve ke overload indexed -> CS1593.
            List<SalesOrderSouvenir> existingRows = db.ExecuteQuery<SalesOrderSouvenir>(
                "SELECT * FROM SalesOrderSouvenir WHERE IDForm = {0} AND IDProduct IS NOT NULL", myID).ToList();
            Dictionary<int, SalesOrderSouvenir> existingByProduct = existingRows.ToDictionary(p => p.IDProduct.Value);
            Dictionary<int, decimal> hargaByProduct = PrefetchSouvenirHarga(productIds);

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

                    int idProductRow = Convert.ToInt32(hftxt);
                    SalesOrderSouvenir d;
                    existingByProduct.TryGetValue(idProductRow, out d);
                    if (d != null)
                    {
                        if (!ischoosedbool)
                        {
                            db.SalesOrderSouvenir.DeleteOnSubmit(d);
                            db.SubmitChanges();
                        }
                        else
                        {
                            decimal ModalRph;
                            hargaByProduct.TryGetValue(d.IDProduct.Value, out ModalRph);
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
                            decimal ModalRph;
                            hargaByProduct.TryGetValue(idProductRow, out ModalRph);

                            SalesOrderSouvenir n = new SalesOrderSouvenir();
                            n.IDForm = myID;
                            n.IDProduct = idProductRow;
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
        }

        //Redirect
        Response.Redirect("Sales_Data.aspx?id=" + myID + "#div_souvenir");
    }

    protected string setJavascriptKeyPressStone(string index, string idsouvenir, string qty)
    {
        string x = string.Empty;
        x += "$(window).load(function(){ ";
        x += "onblursouvenir" + index + "(document.getElementById('" + idsouvenir + "')); ";
        x += "}); ";

        x += "function onblursouvenir" + index + "(e){ ";
        x += "var idsouvenir = document.getElementById('" + idsouvenir + "'); ";
        x += "var qtymax = parseInt(idsouvenir.getAttribute('max')); ";
        x += "var kode = idsouvenir.getAttribute('kodesov'); ";
        x += "var nama = idsouvenir.getAttribute('namasov'); ";
        x += "var cb = document.getElementById('cbsov" + index + "'); ";
        x += "if(e.value != ''){ ";
        x += "var qty = parseInt(e.value); ";
        //x += "console.log(qtymax); console.log(kode); console.log(nama); console.log(qty); ";
        x += "if(qty > qtymax){ ";
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

        x += "function onchecksouvenir" + index + "(e){ ";
        x += "var idsouvenir = document.getElementById('" + idsouvenir + "'); ";
        x += "var ssov = document.getElementById('ssov" + index + "'); ";
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