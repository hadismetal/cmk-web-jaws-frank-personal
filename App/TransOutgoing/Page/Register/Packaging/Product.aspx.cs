using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Product : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //DLC
            Ddl.Filter(brand, "CompanyBrand");
            // DLC.CompanyBrand(brand);
            DLC.LocationType(ddl_from, ddl_to);

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
        href += "&from=" + ddl_from.SelectedValue;
        href += "&to=" + ddl_to.SelectedValue;
        href += "&keyfrom=" + txt_from.Text.Trim();
        href += "&keyto=" + txt_to.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            ddl_from.SelectedValue = Request.QueryString["from"];
            ddl_to.SelectedValue = Request.QueryString["to"];

            lit_TypeFrom.Text = ddl_from.SelectedItem.Text.Trim();
            lit_TypeTo.Text = ddl_to.SelectedItem.Text.Trim();

            txt_from.Text = Request.QueryString["keyfrom"];
            txt_to.Text = Request.QueryString["keyto"];

        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();
            string href = "";

            if (x.TipeLokasi == 1)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Outlet", "1"));

            }
            else if (x.TipeLokasi == 2)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Exhibition", "2"));
            }
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            var x = f.First();
            string href = "";

            if (x.TipeLokasi == 0)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Warehouse", "0"));
            }
            else if (x.TipeLokasi == 1)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Exhibition", "2"));
            }
        }
    }
    protected void fillTable1(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        int vF = Convert.ToInt32(ddl_from.SelectedValue);
        int vT = Convert.ToInt32(ddl_to.SelectedValue);
        string kF = txt_from.Text.Trim();
        string kT = txt_to.Text.Trim();

        int UserID = App.UserID;

        //SELECT
        string getLokasi = @"
                            DECLARE @IDLokasi int = 0
                            IF EXISTS (SELECT 1 FROM DataAdminSales WHERE IDUser = @UserID)
                            BEGIN
                                SELECT @IDLokasi = IDLokasi FROM DataAdminSales WHERE IDUser = @UserID
                            END

                            SELECT AX.IDLokasi, IDBrand, AX.TipeLokasi, NamaLokasi, AEX.NamaKode AS KodeGroup,AddrAlamat  
                            FROM (
                            	SELECT IDBrand,0 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocWarehouse where Draft = 0 and Disable = 0
                            	UNION
                            	SELECT IDBrand,1 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocOutlet where Draft = 0 and Disable = 0
                            	UNION
                            	SELECT IDBrand,2 TipeLokasi, ID IDLokasi, Nama NamaLokasi, IDGroup, AddrAlamat from LocExhibition where Draft = 0 and Disable = 0
                            )AX
                            JOIN (
                            	SELECT ID, 0 TipeLokasi, NamaKode FROM LocWarehouseGroup	where Disable=0
                            	UNION
                            	SELECT ID, 1 TipeLokasi, NamaKode FROM LocOutletGroup where Disable=0
                            	UNION
                            	SELECT ID, 2 TipeLokasi, NamaKode FROM LocExhibitionGroup where Disable=0
                            ) AEX ON AEX.ID=AX.IDGroup and AEX.TipeLokasi=AX.TipeLokasi                            
                            WHERE (@Brand <= 0 or IDBrand = @Brand) and (@Keyword = '' or AX.NamaLokasi like '%'+ @Keyword +'%') 
                            and (@TipeLokasi < 0 or AX.TipeLokasi = @TipeLokasi) ";


        DataSet From = GetDataFrom(vB, vF, kF, getLokasi, UserID);                
        DataSet To = GetDataTo(vB, vT, kT, getLokasi);

        Response.Write(From.Tables[0].Rows.Count+"--"+To.Tables[0].Rows.Count);

        int prev = 0;
        int prevTipe = 0;
        int index = 1;

        foreach (DataRow r in From.Tables[0].Rows)
        {
            foreach (DataRow s in To.Tables[0].Rows)
            {
                if (!Response.IsClientConnected) break;

                prev = Convert.ToInt32(r["IDLokasi"]);
                //Sub-Header
                if (vF != vT || (prev != Convert.ToInt32(s["IDLokasi"])))
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(Convert.ToInt32(r["IDBrand"]), Convert.ToInt32(s["IDBrand"]), vF, vT, Convert.ToInt32(r["IDLokasi"]), Convert.ToInt32(s["IDLokasi"]));
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Nama(r["NamaLokasi"].ToString()) + TD_Group(r["KodeGroup"].ToString()) + TD_Addr(r["AddrAlamat"].ToString());
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Nama(s["NamaLokasi"].ToString()) + TD_Group(s["KodeGroup"].ToString()) + TD_Addr(s["AddrAlamat"].ToString());
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
                prev = Convert.ToInt32(r["IDLokasi"]);
            }
        }
    }


    //Display Table
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     tb.Visible = true;

    //     //GET DDL BRAND FROM-TO TYPE VALUE
    //     int vB = Convert.ToInt32(brand.SelectedValue);
    //     int vF = Convert.ToInt32(ddl_from.SelectedValue);
    //     int vT = Convert.ToInt32(ddl_to.SelectedValue);
    //     string kF = txt_from.Text.Trim();
    //     string kT = txt_to.Text.Trim();

    //     //WHERE
    //     string w = "1=1";
    //     //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

    //     //SELECT
    //     var rsW = from p in db.LocWarehouse.Where(w)
    //               orderby p.CompanyBrand.SN, p.Nama
    //               where !p.Disable && ((vB > 0) ? p.IDBrand == Convert.ToInt32(brand.SelectedValue) : 1 == 1)
    //               select p;
    //     var rsO = from p in db.LocOutlet.Where(w)
    //               orderby p.CompanyBrand.SN, p.Nama
    //               where !p.Disable && ((vB > 0) ? p.IDBrand == Convert.ToInt32(brand.SelectedValue) : 1 == 1)
    //               select p;
    //     var rsE = from p in db.LocExhibition.Where(w)
    //               orderby p.CompanyBrand.SN, p.Nama
    //               where !p.Disable && ((vB > 0) ? p.IDBrand == Convert.ToInt32(brand.SelectedValue) : 1 == 1)
    //               select p;



    //     //COUNT FROM RESULT
    //     int iFW = rsW.Count();
    //     int iFO = rsO.Count();
    //     int iFE = rsE.Count();

    //     bool isValidFromTo = true;

    //     //NO-RESULT FROM-TO (SAME LOCATION) RESULT
    //     if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
    //     else if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
    //     else if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
    //     //NO-RESULT FROM RESULT
    //     else if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
    //     else if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
    //     else if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

    //     //NO-RESULT
    //     //if (rs.Count() == 0) App.TableNoResult(tb);

    //     //RESULT
    //     int prev = 0;
    //     int index = 1;

    //     if (isValidFromTo)
    //     {
    //         //KEY SEARCH (MUST SPLIT TABLE LIST !!!)
    //         var rsW1 = rsW; var rsW2 = rsW;
    //         var rsO1 = rsO; var rsO2 = rsO;
    //         var rsE1 = rsE; var rsE2 = rsE;

    //         int UserID = App.UserID;
    //         var d = from p in db.DataAdminSales
    //                 where p.IDUser == UserID
    //                 select p;
    //         if (d.Count() > 0)
    //         {
    //             var x = d.First();
    //             string href = "";

    //             if (x.TipeLokasi == 1)
    //             {
    //                 rsO1 = rsO.Where(p => p.ID == x.IDLokasi);
    //             }
    //             else if (x.TipeLokasi == 2)
    //             {
    //                 rsE1 = rsE.Where(p => p.ID == x.IDLokasi);
    //             }
    //         }

    //         var f = from p in db.DataSK
    //                 where p.IDUser == UserID
    //                 select p;
    //         if (f.Count() > 0)
    //         {
    //             var x = f.First();
    //             string href = "";

    //             if (x.TipeLokasi == 0)
    //             {
    //                 rsW1 = rsW.Where(p => p.ID == x.IDLokasi);
    //             }
    //             else if (x.TipeLokasi == 1)
    //             {
    //                 rsO1 = rsO.Where(p => p.ID == x.IDLokasi);
    //             }
    //             else if (x.TipeLokasi == 2)
    //             {
    //                 rsE1 = rsE.Where(p => p.ID == x.IDLokasi);
    //             }
    //         }

    //         if (vF == 0) { rsW1 = rsW1.Where(p => p.Nama.Contains(kF)); }
    //         else if (vF == 1) { rsO1 = rsO1.Where(p => p.Nama.Contains(kF)); }
    //         else if (vF == 2) { rsE1 = rsE1.Where(p => p.Nama.Contains(kF)); }

    //         if (vT == 0) { rsW2 = rsW2.Where(p => p.Nama.Contains(kT)); }
    //         else if (vT == 1) { rsO2 = rsO2.Where(p => p.Nama.Contains(kT)); }
    //         else if (vT == 2) { rsE2 = rsE2.Where(p => p.Nama.Contains(kT)); }

    //         if (vF == 0 && vT == 0) // W -> W
    //         {
    //             foreach (var r1 in rsW1)
    //             {
    //                 foreach (var r2 in rsW2.Where(p => p.ID != r1.ID))
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 0 && vT == 1) // W -> O
    //         {
    //             foreach (var r1 in rsW1)
    //             {
    //                 foreach (var r2 in rsO2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 0 && vT == 2) // W -> E
    //         {
    //             foreach (var r1 in rsW1)
    //             {
    //                 foreach (var r2 in rsE2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 1 && vT == 0) // O -> W
    //         {
    //             foreach (var r1 in rsO1)
    //             {
    //                 foreach (var r2 in rsW2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 1 && vT == 1) // O -> O
    //         {
    //             foreach (var r1 in rsO1)
    //             {
    //                 foreach (var r2 in rsO2.Where(p => p.ID != r1.ID))
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 1 && vT == 2) // O -> E
    //         {
    //             foreach (var r1 in rsO1)
    //             {
    //                 foreach (var r2 in rsE2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 2 && vT == 0) // E -> W
    //         {
    //             foreach (var r1 in rsE1)
    //             {
    //                 foreach (var r2 in rsW2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 2 && vT == 1) // E -> O
    //         {
    //             foreach (var r1 in rsE1)
    //             {
    //                 foreach (var r2 in rsO2)
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //         else if (vF == 2 && vT == 2) // E -> E
    //         {
    //             foreach (var r1 in rsE1)
    //             {
    //                 foreach (var r2 in rsE2.Where(p => p.ID != r1.ID))
    //                 {
    //                     if (!Response.IsClientConnected) break;

    //                     //Sub-Header
    //                     if (prev != r1.IDBrand)
    //                     {
    //                         Sub_Header(vT, r1.ID);
    //                         prev = r1.IDBrand;
    //                     }

    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
    //     }

    // }

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
    
    protected string TD_HyperLink(int brandAsal, int brandTujuan, int tf, int tt, int rf, int rt)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Product_Form.aspx?brAsal=" + brandAsal + "&brTujuan=" + brandTujuan + "&tf=" + tf + "&tt=" + tt + "&idf=" + rf + "&idt=" + rt + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;
                
        x += "<p><strong>" + Nama + "</strong></p>";        

        return x;
    }
    protected string TD_Addr(string Addr)
    {
        string x = string.Empty;

        if (Addr != "" && Addr != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(Addr) + "</p>";

        return x;
    }
    protected string TD_Group(string KodeGroup)
    {
        string x = string.Empty;
        
        x += "<p class=\"text-info\">" + KodeGroup + "</p>";        

        return x;
    }

    public static DataSet GetDataFrom(int Brand, int TipeLokasi, string Keywords, string getLokasi,int UserID)    
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(getLokasi+="and (@IDLokasi = 0 or ((AX.TipeLokasi = 1 or AX.TipeLokasi = 2) AND @IDLokasi = AX.IDLokasi))", connection))
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Brand", SqlDbType.Int) { Value = Brand });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = Keywords });
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar) { Value = UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                connection.Close();                
                return ds;
            }
        }
    }  

    public static DataSet GetDataTo(int Brand, int TipeLokasi, string Keywords, string getLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(getLokasi, connection))
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Brand", SqlDbType.Int) { Value = Brand });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = Keywords });
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar) { Value = -1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                connection.Close();                
                return ds;
            }
        }
    }
    
    // protected string TD_HyperLink(int brand, int tf, int tt, int rf, int rt)
    // {
    //     string x = string.Empty;

    //     x += "<a class=\"btn btn-info\" href=\"Product_Form.aspx?idb=" + brand + "&tf=" + tf + "&tt=" + tt + "&idf=" + rf + "&idt=" + rt + "\">";
    //     x += "  <i class=\"icon-ok\"></i> ";
    //     x += "  Select";
    //     x += "</a>";

    //     return x;
    // }
    // protected string TD_Nama(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }
    //     else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         x += "<p><strong>" + rs.Nama + "</strong></p>";
    //     }

    //     return x;
    // }
    // protected string TD_Addr(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //             x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //             x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }
    //     else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
    //         x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
    //     }

    //     return x;
    // }
    // protected string TD_Group(int t, int r)
    // {
    //     string x = string.Empty;

    //     if (t == 0)
    //     {
    //         var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
    //     }
    //     else if (t == 1)
    //     {
    //         var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
    //     }
    //     else if (t == 2)
    //     {
    //         var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
    //         x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
    //     }

    //     return x;
    // }
}