using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

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

    protected string myV { get { return Request.QueryString["valid"]; } }

    private int IDLocation = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //DLC
            DLC.CompanyBrand(brandFrom);
            brandTo.SelectedValue = brandFrom.SelectedValue;
            foreach(ListItem x in brandFrom.Items)
            {
                brandTo.Items.Remove(x);
            }
            //App.Brand;
            DLC.LocationType(ddl_from, ddl_to);

            //valid transfer
            if (myV == "0")
            {
                lit_valid.Text = "<h4><p class='alert alert-info'>Mapping location not found !!</p></h4>";
                return;
            }

            //Generate
            fillFilter();

            fillDropdownLocation(ddl_locFrom, ddl_from, brandFrom);
            ddl_locFrom.SelectedValue = IDLocation.ToString();
            IDLocation = 0;
            fillDropdownLocation(ddl_locTo, ddl_to, brandTo);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        //string href = App.RefreshURL(this);

        //Customize-Qstr
        //href += "&brand=" + brand.SelectedValue;
        //href += "&from=" + ddl_from.SelectedValue;
        //href += "&to=" + ddl_to.SelectedValue;
        //href += "&keyfrom=" + txt_from.Text.Trim();
        //href += "&keyto=" + txt_to.Text.Trim();

        Response.Redirect("DJ_Form.aspx?idbf=" + brandFrom.SelectedValue + "&tf=" + ddl_from.SelectedValue + "&tt=" + ddl_to.SelectedValue + "&idf=" + ddl_locFrom.SelectedValue + "&idt=" + ddl_locTo.SelectedValue + "&idbt=" + brandTo.SelectedValue);
    }
    protected void fillFilter()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();
            string href = "";

            int idlokasi = db.DataAdminSales.SingleOrDefault(p => p.IDUser == App.UserID && p.Disable == false && p.Draft == false).IDLokasi;

            if (x.TipeLokasi == 1)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Outlet", "1"));
                brandFrom.SelectedValue = db.LocOutlet.SingleOrDefault(p => p.ID == idlokasi).IDBrand.ToString();
            }
            else if (x.TipeLokasi == 2)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Exhibition", "2"));
                brandFrom.SelectedValue = db.LocExhibition.SingleOrDefault(p => p.ID == idlokasi).IDBrand.ToString();
            }
            IDLocation = x.IDLokasi;
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
            IDLocation = x.IDLokasi;
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = 0;
        int vF = Convert.ToInt32(ddl_from.SelectedValue);
        int vT = Convert.ToInt32(ddl_to.SelectedValue);
        //string kF = txt_from.Text.Trim();
        //string kT = txt_to.Text.Trim();

        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

        //SELECT
        var rsW = from p in db.LocWarehouse.Where(w)
                  orderby p.CompanyBrand.SN, p.Nama
                  where !p.Disable && ((vB>0)?p.IDBrand == Convert.ToInt32(0) : 1==1)
                  select p;
        var rsO = from p in db.LocOutlet.Where(w)
                  orderby p.CompanyBrand.SN, p.Nama
                  where !p.Disable && ((vB > 0) ? p.IDBrand == Convert.ToInt32(0) : 1 == 1)
                  select p;
        var rsE = from p in db.LocExhibition.Where(w)
                  orderby p.CompanyBrand.SN, p.Nama
                  where !p.Disable && ((vB > 0) ? p.IDBrand == Convert.ToInt32(0) : 1 == 1)
                  select p;

        

        //COUNT FROM RESULT
        int iFW = rsW.Count();
        int iFO = rsO.Count();
        int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        else if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        else if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //NO-RESULT FROM RESULT
        else if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        else if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        else if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;

        if (isValidFromTo)
        {
            //KEY SEARCH (MUST SPLIT TABLE LIST !!!)
            var rsW1 = rsW; var rsW2 = rsW;
            var rsO1 = rsO; var rsO2 = rsO;
            var rsE1 = rsE; var rsE2 = rsE;

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
                    rsO1 = rsO.Where(p => p.ID == x.IDLokasi);
                }
                else if (x.TipeLokasi == 2)
                {
                    rsE1 = rsE.Where(p => p.ID == x.IDLokasi);
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
                    rsW1 = rsW.Where(p => p.ID == x.IDLokasi);
                }
                else if (x.TipeLokasi == 1)
                {
                    rsO1 = rsO.Where(p => p.ID == x.IDLokasi);
                }
                else if (x.TipeLokasi == 2)
                {
                    rsE1 = rsE.Where(p => p.ID == x.IDLokasi);
                }
            }
            
            //if (vF == 0) { rsW1 = rsW1.Where(p => p.Nama.Contains(kF)); }
            //else if (vF == 1) { rsO1 = rsO1.Where(p => p.Nama.Contains(kF)); }
            //else if (vF == 2) { rsE1 = rsE1.Where(p => p.Nama.Contains(kF)); }

            //if (vT == 0) { rsW2 = rsW2.Where(p => p.Nama.Contains(kT)); }
            //else if (vT == 1) { rsO2 = rsO2.Where(p => p.Nama.Contains(kT)); }
            //else if (vT == 2) { rsE2 = rsE2.Where(p => p.Nama.Contains(kT)); }

            if (vF == 0 && vT == 0) // W -> W
            {
                foreach (var r1 in rsW1)
                {
                    foreach (var r2 in rsW2.Where(p => p.ID != r1.ID))
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 0 && vT == 1) // W -> O
            {
                foreach (var r1 in rsW1)
                {
                    foreach (var r2 in rsO2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 0 && vT == 2) // W -> E
            {
                foreach (var r1 in rsW1)
                {
                    foreach (var r2 in rsE2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 1 && vT == 0) // O -> W
            {
                foreach (var r1 in rsO1)
                {
                    foreach (var r2 in rsW2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 1 && vT == 1) // O -> O
            {
                foreach (var r1 in rsO1)
                {
                    foreach (var r2 in rsO2.Where(p => p.ID != r1.ID))
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 1 && vT == 2) // O -> E
            {
                foreach (var r1 in rsO1)
                {
                    foreach (var r2 in rsE2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 2 && vT == 0) // E -> W
            {
                foreach (var r1 in rsE1)
                {
                    foreach (var r2 in rsW2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 2 && vT == 1) // E -> O
            {
                foreach (var r1 in rsE1)
                {
                    foreach (var r2 in rsO2)
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
            else if (vF == 2 && vT == 2) // E -> E
            {
                foreach (var r1 in rsE1)
                {
                    foreach (var r2 in rsE2.Where(p => p.ID != r1.ID))
                    {
                        if (!Response.IsClientConnected) break;

                        //Sub-Header
                        if (prev != r1.IDBrand)
                        {
                            Sub_Header(vT, r1.ID);
                            prev = r1.IDBrand;
                        }

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = TD_HyperLink(vB, vF, vT, r1.ID, r2.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<p>" + index++.ToString() + ".</p>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vF, r1.ID) + TD_Group(vF, r1.ID) + TD_Addr(vF, r1.ID);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Nama(vT, r2.ID) + TD_Group(vT, r2.ID) + TD_Addr(vT, r2.ID);
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);
                    }
                }
            }
        }

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
    protected string TD_HyperLink(int brand, int tf, int tt, int rf, int rt)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Form.aspx?idb=" + brand + "&tf=" + tf + "&tt=" + tt + "&idf=" + rf + "&idt=" + rt + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
        }

        return x;
    }

    private void fillDropdownLocation(DropDownList location, DropDownList locationType, DropDownList brand)
    {

        var data = Db.Rs("sp_getLocationBrand " + brand.SelectedValue + ", " + locationType.SelectedValue + ", " + IDLocation, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
        
        location.Items.Clear();
        location.DataBind();
        location.Items.Add(new ListItem("Location :", "0"));

        foreach (DataRow d in data.Rows)
        {

            string t = d["text"].ToString();
            string v = d["value"].ToString();
            location.Items.Add(new ListItem(t, v));
        }
    }

    protected void ddl_from_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDropdownLocation(ddl_locFrom, ddl_from, brandFrom);
    }

    protected void ddl_to_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDropdownLocation(ddl_locTo, ddl_to, brandTo);
    }
}