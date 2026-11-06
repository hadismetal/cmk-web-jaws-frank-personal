using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Reflection;

public partial class Page_Register_Resell : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);

            btnadd.HRef += "?id=" + myID;
            btnaddpg.HRef += "?id=" + myID;
            btnaddld.HRef += "?id=" + myID;
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&id=" + myID;
        href += "&kw=" + kw.Text;
        href += "&searchby=" + searchby.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            searchby.SelectedValue = Request.QueryString["searchby"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        /*
        fillPG();
        fillDJ();
        fillGJ();
        fillLD();
        */

        fill();

    }

    protected void fillDJ()
    {
        int SearchType = 1;

        if (kw.Text.Contains("D"))
            SearchType = 1;
        else if (kw.Text.Contains("P"))
            SearchType = 2;

        if (SearchType == 1)
        {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID);
            //Tambahan By Andri Search Menggunakan Kode Customer Lamma 
            var oldcodecust = Db.SingleInteger("Exec sp_select_kode_customer_lama '" + myID + "','RS' ", Db.Cnn_DB);
            //SELECT
            var rs = from p in db.SalesOrderDJ
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && p.SalesOrder.IDCustomer == d.IDCustomer
                     select p;

            if (oldcodecust != -1)
            {
                rs = from p in db.SalesOrderDJ
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && (p.SalesOrder.IDCustomer == d.IDCustomer || p.SalesOrder.IDCustomer == oldcodecust)
                     select p;
            }
            int SearchBy = Convert.ToInt32(searchby.SelectedValue);
            if (SearchBy == 1)
                rs = rs.Where(p => p.SalesOrder.Nomor.Contains(kw.Text));
            else
                rs = rs.Where(p => p.StockProductDJ.Nomor.Contains(kw.Text));

            rs = rs.OrderBy(p => p.SalesOrder.Tgl.Value);

            //NO-RESULT
            if (rs.Count() == 0 && kw.Text != "") App.TableNoResult(tb);

            //RESULT
            int prev = 0;
            int index = 1;
            if (rs.Count() == 1 && kw.Text != "")
            {
                Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=1&iditem=" + rs.Single().IDProduct);
            }
            else
            {
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;


                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct, 1);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.SalesOrder.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);
                    else if (r.SalesOrder.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
    }

    protected void fillGJ()
    {
        int SearchType = 3;
        if (kw.Text.Contains("G"))
            SearchType = 3;

        if (SearchType == 3)
        {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID);
            //Tambahan By Andri Search Menggunakan Kode Customer Lamma 
            var oldcodecust = Db.SingleInteger("Exec sp_select_kode_customer_lama '" + myID + "','RS' ", Db.Cnn_DB);
            //SELECT
            var rs = from p in db.SalesOrderGJ
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && p.SalesOrder.IDCustomer == d.IDCustomer
                     select p;

            if (oldcodecust != -1)
            {
                rs = from p in db.SalesOrderGJ
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && (p.SalesOrder.IDCustomer == d.IDCustomer || p.SalesOrder.IDCustomer == oldcodecust)
                     select p;
            }

            int SearchBy = Convert.ToInt32(searchby.SelectedValue);
            if (SearchBy == 1)
                rs = rs.Where(p => p.SalesOrder.Nomor.Contains(kw.Text));
            else
                rs = rs.Where(p => p.StockProductGJ.Nomor.Contains(kw.Text));

            rs = rs.OrderBy(p => p.SalesOrder.Tgl.Value);

            //NO-RESULT
            if (rs.Count() == 0) App.TableNoResult(tb3);

            //RESULT
            int prev = 0;
            int index = 1;
            if (rs.Count() == 1 && kw.Text != "")
            {
                Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=3&iditem=" + rs.First().IDProduct);
            }
            else
            {
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;


                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct, 3);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductGJ.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.SalesOrder.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);
                    else if (r.SalesOrder.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    tb3.Rows.Add(tr);
                }
            }
        }
    }

    protected void fillLD()
    {
        int SearchType = 4;
        if (kw.Text.Contains("L"))
            SearchType = 4;

        if (SearchType == 4)
        {
            var d = db.Resell.SingleOrDefault(p => p.ID == myID);
            //Tambahan By Andri Search Menggunakan Kode Customer Lamma 
            var oldcodecust = Db.SingleInteger("Exec sp_select_kode_customer_lama '" + myID + "','RS' ", Db.Cnn_DB);
            //SELECT
            var rs = from p in db.SalesOrderLD
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && p.SalesOrder.IDCustomer == d.IDCustomer
                     select p;

            if (oldcodecust != -1)
            {
                rs = from p in db.SalesOrderLD
                     where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && (p.SalesOrder.IDCustomer == d.IDCustomer || p.SalesOrder.IDCustomer == oldcodecust)
                     select p;
            }

            int SearchBy = Convert.ToInt32(searchby.SelectedValue);
            if (SearchBy == 1)
                rs = rs.Where(p => p.SalesOrder.Nomor.Contains(kw.Text));
            else
                rs = rs.Where(p => p.StockProductLD_Stone1B.Nomor.Contains(kw.Text));

            rs = rs.OrderBy(p => p.SalesOrder.Tgl.Value);

            //NO-RESULT
            if (rs.Count() == 0) App.TableNoResult(tb4);

            //RESULT
            int prev = 0;
            int index = 1;
            if (rs.Count() == 1 && kw.Text != "")
            {
                Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=4&iditem=" + rs.First().IDProduct);
            }
            else
            {
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;


                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r.IDProduct, 4);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.SalesOrder.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductLD_Stone1B.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.TotalBayar.Value.ToString("N0");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    if (r.SalesOrder.TipeLokasi == 1)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);
                    else if (r.SalesOrder.TipeLokasi == 2)
                        Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);

                    td = new TableCell();
                    td.Text = Lokasi;
                    tr.Cells.Add(td);

                    tb4.Rows.Add(tr);
                }
            }
        }
    }

    protected void fillPG()
    {
        //int SearchType = 1;

        //if (kw.Text.Contains("D"))
        //    SearchType = 1;
        //else if (kw.Text.Contains("P"))
        //    SearchType = 2;

        //if (SearchType == 2)
        //{
        var d = db.Resell.SingleOrDefault(p => p.ID == myID);
        //Tambahan By Andri Search Menggunakan Kode Customer Lamma 
        var oldcodecust = Db.SingleInteger("Exec sp_select_kode_customer_lama '" + myID + "','RS' ", Db.Cnn_DB);

        //SELECT
        var rs = from p in db.SalesOrderPG
                 where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && p.SalesOrder.IDCustomer == d.IDCustomer
                 select p;

        if (oldcodecust != -1)
        {
            rs = from p in db.SalesOrderPG
                 where p.StatusResell == 0 && p.SalesOrder.StatusVoid == false && (p.SalesOrder.IDCustomer == d.IDCustomer || p.SalesOrder.IDCustomer == oldcodecust)
                 select p;
        }

        int SearchBy = Convert.ToInt32(searchby.SelectedValue);
        if (SearchBy == 1)
            rs = rs.Where(p => p.SalesOrder.Nomor.Contains(kw.Text));
        else
            rs = rs.Where(p => p.StockProductPG.Nomor.Contains(kw.Text));

        rs = rs.OrderBy(p => p.SalesOrder.Tgl.Value);

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;
        if (rs.Count() == 1 && kw.Text != "")
        {
            Response.Redirect("Resell_ItemAddSave.aspx?id=" + myID + "&item=2&iditem=" + rs.First().IDProduct);
        }
        else
        {
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r.IDProduct, 2);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalBayar.Value).ToString("N0");
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.SalesOrder.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);
                else if (r.SalesOrder.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);
            }
        }
        //}
    }


    protected string TD_HyperLink(int ID, int itemindex)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "Resell_ItemAddSave.aspx";
        href += "?id=" + myID;
        href += "&item=" + itemindex;
        href += "&iditem=" + ID;

        x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(LocOutlet r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }
    protected string TD_Addr(LocOutlet r)
    {
        string x = string.Empty;

        if (r.AddrAlamat != "" && r.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

        return x;
    }
    protected string TD_Group(LocOutlet r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.LocOutletGroup.NamaKode + "</p>";

        return x;
    }


    protected void Sub_Header(LocExhibition r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.CompanyBrand.Nama + " (" + r.CompanyBrand.NamaKode + ")";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(LocExhibition r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Resell_Form.aspx?idlokasi=" + r.ID + "&tipe=2\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }
    protected string TD_Addr(LocExhibition r)
    {
        string x = string.Empty;

        if (r.AddrAlamat != "" && r.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

        return x;
    }
    protected string TD_Group(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.LocExhibitionGroup.NamaKode + "</p>";

        return x;
    }

    protected void btnoldcustomercode_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/Page/Create/Resell/Resell_Form.aspx?id=" + Request.QueryString["id"] + "&status=1");
    }

    protected void fill()
    {
        string query = "sp_get_resell_item_by_customer";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@KEYWORD", SqlDbType.VarChar){ Value = kw.Text },
            new SqlParameter("@ID_RESELL", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);

        int indexdj = 1;
        int indexpg = 1;
        int indexgj = 1;
        int indexld = 1;
        foreach (DataRow dr in dt.Rows)
        {
            string tipe = dr["TIPE"].ToString();
            int IDSO = Convert.ToInt32(dr["ID_SALES_ORDER"]);
            int IDProduct = Convert.ToInt32(dr["ID_PRODUCT"]);
            string PLU = dr["NOMOR"].ToString();
            string NOSO = dr["NOSO"].ToString();
            string Invoice = dr["NO_INVOICE"].ToString();
            decimal HargaBeli = Convert.ToDecimal(dr["HARGA_BELI"]);
            string Item = dr["ITEM"].ToString();
            string StoreAsal = dr["STORE_ASAL"].ToString();
            string ImageUrl = (dr["IMAGE"] == DBNull.Value) ? "" : dr["IMAGE"].ToString();
            DateTime TglBeli = Convert.ToDateTime(dr["TGL"]);

            int tipe_product = 0;
            switch (tipe)
            {
                case "DJ":
                    tipe_product = 1;
                    break;
                case "PG":
                    tipe_product = 2;
                    break;
                case "GJ":
                    tipe_product = 3;
                    break;
                case "LD":
                    tipe_product = 4;
                    break;
                default:
                    tipe_product = 1;
                    break;
            }

            if (tipe == "DJ")
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(IDProduct, tipe_product);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + indexdj++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<img src = '/Images.aspx?path=" + ImageUrl + "' style = 'max-width:105px; max-height:65px' />";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NOSO;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PLU;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaBeli.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = StoreAsal;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
            else if (tipe == "PG")
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(IDProduct, tipe_product);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + indexpg++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<img src = '/Images.aspx?path=" + ImageUrl + "' style = 'max-width:105px; max-height:65px' />";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NOSO;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PLU;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaBeli.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = StoreAsal;
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);
            }
            else if (tipe == "GJ")
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(IDProduct, tipe_product);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + indexgj++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<img src = '/Images.aspx?path=" + ImageUrl + "' style = 'max-width:105px; max-height:65px' />";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NOSO;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PLU;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaBeli.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = StoreAsal;
                tr.Cells.Add(td);

                tb3.Rows.Add(tr);
            }
            else if (tipe == "LD")
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(IDProduct, tipe_product);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + indexld++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NOSO;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PLU;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Item;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaBeli.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = StoreAsal;
                tr.Cells.Add(td);

                tb4.Rows.Add(tr);
            }
        }
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType typex)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = typex;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
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
}