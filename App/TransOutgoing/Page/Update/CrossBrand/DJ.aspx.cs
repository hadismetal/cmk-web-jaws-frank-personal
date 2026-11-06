using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CrossBrand;
using CrossBrand.Model;
using System.Data;
using System.Data.SqlClient;

public partial class DJ : System.Web.UI.Page
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
    Modelize cross = new Modelize();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //DLC
            DLC.CompanyBrand(brand);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable1(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&brand=" + brand.SelectedValue;
        href += "&kw=" + keyword.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];

        }
    }

    //Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;

        //FILTER
        int vB = Convert.ToInt32(Request.QueryString["brand"].ToString());
        string kw = Request.QueryString["kw"].ToString();

        keyword.Text = kw;

        //SELECT
        // var rs = cross.OutgoingDJ().Where(p => (p.IDBrandAsal == vB || (p.IDBrandAsal > 0 && vB == -1)) && p.StatusTransOut <= 1
        //            && (p.CrossBrand_StockOutgoingDJ_Product.Any(c => c.StockProductDJ.Nomor.Contains(kw))
        //            || p.NamaAsal.Contains(kw) || p.NamaTujuan.Contains(kw) || p.Operator.Contains(kw)));

        int UserID = App.UserID;
        DataSet ds = new DataSet();

        string getDataDraft = @"
        DECLARE @TipeLokasi int = 9, @IDLokasi int = 9
        IF EXISTS(SELECT 1 FROM DataAdminSales where IDUser = @IDUser AND Disable = 0 AND Draft = 0)
        BEGIN
            SELECT @TipeLokasi = TipeLokasi, @IDLokasi = IDLokasi FROM DataAdminSales where IDUser = @IDUser AND Disable = 0 AND Draft = 0
        END

            SELECT a.ID AS IDOutgoing, a.IDBrandAsal, a.IDBrandTujuan, a.TipeAsal, a.TipeTujuan, a.IDAsal, a.IDTujuan, COUNT(b.IDProduct) TotalItems, a.StatusTransOut
            , FORMAT(CONVERT(DATETIME, a.Tgl, 113), 'dddd, dd MMMM yyyy', 'id-ID') Tgl, a.Operator
            FROM CrossBrand_StockOutgoingDJ a 
            LEFT JOIN CrossBrand_StockOutgoingDJ_Product b on b.IDForm = a.ID
            WHERE StatusTransOut = 0 and (@BrandAsal = 0 OR a.IDBrandAsal = @BrandAsal)
            AND (@Keyword = '' OR (b.Nomor like '%'+ @Keyword +'%' OR a.NamaAsal like '%'+ @Keyword +'%' OR a.NamaTujuan like '%'+ @Keyword +'%' OR a.Operator like '%'+ @Keyword +'%'))
            AND (@TipeLokasi = 9 OR a.TipeAsal = @TipeLokasi) AND (@IDLokasi = 9 OR a.IDAsal = @IDLokasi)            
            GROUP BY a.ID,a.IDBrandAsal, a.IDBrandTujuan, a.TipeAsal, a.TipeTujuan, a.IDAsal, a.IDTujuan, a.StatusTransOut, a.Tgl, a.Operator, a.OperatorTgl
            ORDER BY a.OperatorTgl ASC
        ";
        
         using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(getDataDraft, connection))
            {

                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = UserID });
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = kw });
                command.Parameters.Add(new SqlParameter("@BrandAsal", SqlDbType.Int) { Value = vB });                
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);                
                adapter.Fill(ds);                

            }
        }


        int IDOutgoing = 0; // id from
        int vF = 0; // id from
        int vT = 0; // id to
        int tF = 0; // value from
        int tT = 0; // value to
        int bT = 0;
        int bF = 0;

        int index = 1;

        if (ds.Tables.Count == 0) { App.TableNoResult(tb); }
        else
        {
            DataTable dt = ds.Tables[0];

            string TotalData = ds.Tables.Count.ToString();
            string msg = "";

            // if (ds.Tables.Count > 100) { rs = rs.Take(100); msg = "Showing 100 from " + TotalData + " data"; } 
            //     else { msg = "Showing " + TotalData + " data"; }
            // if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
            // if (msg != "") { lit_msg.Text = msg; }
            // rs = rs.OrderByDescending(p => p.OperatorTgl);

            foreach (DataRow rs in dt.Rows)
            {               
                if (!Response.IsClientConnected) break;

                IDOutgoing = Convert.ToInt32(rs["IDOutgoing"]); // id from
                vF = Convert.ToInt32(rs["IDAsal"]); // id from
                vT = Convert.ToInt32(rs["IDTujuan"]); // id to
                tF = Convert.ToInt32(rs["TipeAsal"]); // value from
                tT = Convert.ToInt32(rs["TipeTujuan"]); // value to
                bF = Convert.ToInt32(rs["IDBrandAsal"]);
                bT = Convert.ToInt32(rs["IDBrandTujuan"]);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLinkNew(IDOutgoing, bF, tF, vF, bT, tT, vT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(tF) + "<br>" + TD_Nama(tF, vF, bF) + TD_Group(tF, vF, bF) + TD_Addr(tF, vF, bF) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(tT) + "<br>" + TD_Nama(tT, vT, bT) + TD_Group(tT, vT, bT) + TD_Addr(tT, vT, bT) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(rs["TotalItems"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(Convert.ToInt32(rs["StatusTransOut"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToString(rs["Operator"]) + "<br>" + Convert.ToString(rs["Tgl"]);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        //COUNT FROM RESULT
        //int iFW = rsW.Count();
        //int iFO = rsO.Count();
        //int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;


    }
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //FILTER
        int vB = Convert.ToInt32(Request.QueryString["brand"].ToString());
        string kw = Request.QueryString["kw"].ToString();

        keyword.Text = kw;

        //SELECT
        var rs = cross.OutgoingDJ().Where(p => (p.IDBrandAsal == vB || (p.IDBrandAsal > 0 && vB == -1)) && p.StatusTransOut <= 1
                   && (p.CrossBrand_StockOutgoingDJ_Product.Any(c => c.StockProductDJ.Nomor.Contains(kw))
                   || p.NamaAsal.Contains(kw) || p.NamaTujuan.Contains(kw) || p.Operator.Contains(kw)));

        //var rs = (from p in db.StockOutgoingDJ
        //          where ((vB > 0) ? p.IDBrand == vB : 1 == 1) && p.StatusTransOut == 0 // 0 : Draft
        //                && (p.StockOutgoingDJ_Product.Any(z => z.StockProductDJ.Nomor.Contains(kw))
        //                    || p.NamaAsal.Contains(kw) || p.NamaTujuan.Contains(kw) || p.Operator.Contains(kw))
        //          select p);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 select p;
        }

        int vF = 0; // id from
        int vT = 0; // id to
        int tF = 0; // value from
        int tT = 0; // value to
        int bT = 0;
        int bF = 0;

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            string TotalData = rs.Count().ToString();
            string msg = "";

            if (rs.Count() > 100) { rs = rs.Take(100); msg = "Showing 100 from " + TotalData + " data"; } 
                else { msg = "Showing " + TotalData + " data"; }
            if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
            if (msg != "") { lit_msg.Text = msg; }
            rs = rs.OrderByDescending(p => p.OperatorTgl);

            foreach (var item in rs)
            {
                var rsI = from p in db.StockOutgoingDJ_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.IDAsal; // id from
                vT = item.IDTujuan; // id to
                tF = item.TipeAsal; // value from
                tT = item.TipeTujuan; // value to
                bF = item.IDBrandAsal;
                bT = item.IDBrandTujuan;

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(item);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF, bF) + TD_Group(tF, vF, bF) + TD_Addr(tF, vF, bF) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT, bT) + TD_Group(tT, vT, bT) + TD_Addr(tT, vT, bT) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.CrossBrand_StockOutgoingDJ_Product.Count.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator + "<br>" + Str.FullDate(item.Tgl);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        //COUNT FROM RESULT
        //int iFW = rsW.Count();
        //int iFO = rsO.Count();
        //int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;


    }

    protected void Sub_Header(int t, int r)
    {
        //TableRow tr = new TableRow();
        //TableCell td;

        //tr.CssClass = "sub-header";
        //string x = string.Empty;

        //if (t == 0)
        //{
        //    var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 1)
        //{
        //    var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 2)
        //{
        //    var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}

        //td = new TableCell();
        //td.Text = x;
        //td.CssClass = "text-lg text-warning";
        //td.ColumnSpan = App.TableColspan(tb);
        //tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }
    protected string TD_HyperLinkNew(int IDOutgoing, int IDBrandAsal, int TipeAsal, int IDAsal, int IDBrandTujuan, int TipeTujuan, int IDTujuan)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + IDOutgoing + "&idb=" + IDBrandAsal + "&tf=" + TipeAsal + "&tt=" + TipeTujuan + "&idf=" + IDAsal + "&idt=" + IDTujuan + "&idbt=" + IDBrandTujuan + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "</a>";

        return x;
    }
    protected string TD_HyperLink(CrossBrand_StockOutgoingDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?id=" + r.ID + "&idb=" + r.IDBrandAsal + "&tf=" + r.TipeAsal + "&tt=" + r.TipeTujuan + "&idf=" + r.IDAsal + "&idt=" + r.IDTujuan + "&idbt=" + r.IDBrandTujuan + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }

        return x;
    }
}