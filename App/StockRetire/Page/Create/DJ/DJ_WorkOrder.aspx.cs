using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ : System.Web.UI.Page
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (ac != null)
            ac.Dispose();
        base.Dispose();
    }
    protected string nomor { get { return Request.QueryString["nomor"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            //datepicker.Text = DateTime.Now.ToString("MM-yyyy");
            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);
            DLC.CharFrameMaterial(ddl_material);
            //Generate
            fillFilter();
            if (!string.IsNullOrEmpty(nomor))
            {
                lblMessage.Text = "Created DO : " + nomor;
                lblMessage.Visible = true;
            }
            //samp.Visible = false;
        }

        if (App.Refresh(this)) fillTable(sender, e);
        //samp.Visible = false;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //if (kw.Text != "" || kw.Text.Length > 0)
        //{
        DateTime start;
        DateTime end;
        DateTime dchoose;
        //SELECT
        //var rs = (from p in db.StockActualDJ
        //          join p1 in db.StockRetireDJ on p.IDProduct equals p1.IDProduct 
        //          into p2
        //          from p3 in p2.Where(a => (a.Status !=2) && a.Approval == null).OrderByDescending(a => a.ID).DefaultIfEmpty()
        //          where p.StockProductDJ.Draft == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0 
        //          && p.StockProductDJ.StatusLebur == false && p.Substorage == 6 
        //          select p);          

        //if (datepicker.Text != "" && datepicker2.Text != "")
        //{

        //while using month to pick
        //dchoose = Convert.ToDateTime(datepicker.Text);
        //start = new DateTime(dchoose.Year, dchoose.Month, 1);
        //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));

        start = Convert.ToDateTime(datepicker.Text);
        end = Convert.ToDateTime(datepicker2.Text);

        DataTable populate = Db.Rs("SELECT A.ID FROM StockRetireDJ A JOIN StockProductDJ_CharProduct B ON A.IDProduct = B.ID WHERE OperatorTgl > '" + start.ToString("yyyy-MM-dd") + "' AND OperatorTgl < '" + end.AddDays(1).ToString("yyyy-MM-dd") + "' AND IDWO IS NULL AND B.FrameMaterial = " + ddl_material.SelectedValue, Db.Cnn_DB);

        List<int> ids = new List<int>();

        if (populate.Rows.Count > 0)
        {
            foreach (DataRow r in populate.Rows)
            {
                ids.Add(Convert.ToInt32(r["ID"]));
            }
        }

        var rs = (from p1 in db.StockRetireDJ
                  join p in db.StockIncomingDJ_Product  on p1.IDProduct equals p.IDProduct
                  into p2
                  from p3 in p2.Where(a => a.ApprovalTgl >= start && a.ApprovalTgl < end.AddDays(1)).OrderByDescending(a => a.ID).DefaultIfEmpty()
                  where p1.StockProductDJ.Draft == false && p1.StockProductDJ.ReturnProduct == false && p1.StockProductDJ.StatusPenjualan == 0
                      && p1.StockProductDJ.StatusLebur == false && p1.Status != 2 && p1.Approval == null && ids.Contains(p1.ID)
                  select p1);

        if (kw.Text != "" || kw.Text.Length > 0)
        {
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        }
        //}
        if (rs.Count() == 0) App.TableNoResult(tb);

        if (rs != null)
        {

            rs = rs.OrderBy(p => p.StockProductDJ.LocWarehouse.Nama).ThenBy(p => p.StockProductDJ.ID);



            //NO-RESULT
            //zero.Visible = rs.Count() == 0;
            //form_area.Visible = !zero.Visible;

            //RESULT
            int prev = 0;
            int index = 1;
            string previous = "";
            foreach (var r in rs)
            {

                btnRegister.Visible = true;
                if (!Response.IsClientConnected) break;

                //Sub-Header
                if (prev != r.StockProductDJ.IDWarehouse.Value)
                {
                    //Sub_Header(r.StockProductDJ);
                    prev = r.StockProductDJ.IDWarehouse.Value;
                }

                if (previous != r.Nomor)
                {
                    TableRow tr = new TableRow();
                    TableCell td;


                    CheckBox cb = new CheckBox();

                    cb.ID = "cb_po" + index.ToString();
                    cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.ID.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(cb);
                    td.Controls.Add(hf);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.CssClass = "text-right";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Image(r.StockProductDJ);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Nomor(r.StockProductDJ);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Data(r.StockProductDJ);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    if (r.StockProductDJ.StockProductDJ_CharProduct != null)
                    {

                        td = new TableCell();
                        td.Text = r.StockProductDJ.StockProductDJ_CharProduct.KadarLogam.ToString("N2");
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharFrameMaterial.Nama;
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Product01(r.StockProductDJ.StockProductDJ_CharProduct);
                        td.CssClass = "text-left";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Product02(r.StockProductDJ.StockProductDJ_CharProduct);
                        td.CssClass = "text-middle";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Product03(r.StockProductDJ.StockProductDJ_CharProduct);
                        td.CssClass = "text-center";
                        tr.Cells.Add(td);
                    }

                    td = new TableCell();
                    td.Text = r.StockProductDJ.LocWarehouse.Nama + " - " + r.StockProductDJ.LocWarehouse.CompanyBrand.Nama;
                    tr.Cells.Add(td);
                    td.CssClass = "text-left";
                    tb.Rows.Add(tr);

                    previous = r.Nomor;
                }
            }
        }

        //}
        //else
        //{
        //    lb_valid.Text = "mohon isi no plu";
        //}

    }
    protected void Sub_Header(StockProductDJ r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(StockProductDJ r)
    {
        string x = string.Empty;

        var cek = db.StockRetireDJ.OrderByDescending(p => p.ID).FirstOrDefault(p => p.IDProduct == r.ID);

        if (cek == null)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Submit.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Open";
            x += "</a>";
        }
        else
        {
            if (cek.Status == 2)
            {
                var cek2 = from p in db.StockIncomingDJ_Product where p.IDProduct == cek.IDProduct && p.ApprovalTgl > cek.ApprovalTgl select p;
                if (cek2 != null)
                {
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"DJ_Submit.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-search\"></i> ";
                    x += "  Open";
                    x += "</a>";
                }
                else
                { x += ""; }
            }
            else
            {
                x += "<a class=\"btn btn-primary\" target=\"_blank\" href=\"SPL_DJ.aspx?id=" + cek.ID + "&status=1" + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                // x += "  Print with pic";
                x += "</a>";

                x += " &nbsp; <a class=\"btn btn-primary\" target=\"_blank\" href=\"SPL_DJ.aspx?id=" + cek.ID + "&status=2" + "\">";
                x += "  <i class=\"icon-file\"></i> ";
                //x += "  Print no pic";
                x += "</a>";
            }
        }
        return x;
    }
    protected string TD_Nomor(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

        return x;
    }
    protected string TD_Operator(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
    protected string TD_Data(StockProductDJ r)
    {
        string x = string.Empty;

        if (r.ProtoNomor != "" && r.ProtoNomor != null)
            x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

        if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
            x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

        if (r.SupplierNomor != "" && r.SupplierNomor != null)
            x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

        return x;
    }
    protected string TD_Product01(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Image(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //while using month to pic
        //DateTime start;
        //DateTime end;
        //DateTime dchoose;
        //dchoose = Convert.ToDateTime(datepicker.Text);
        //start = new DateTime(dchoose.Year, dchoose.Month, 1);
        //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));

        href += "&kw=" + kw.Text;
        //href += "&dari=" + start.ToShortDateString();
        //href += "&sampai=" + end.ToShortDateString();
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&material=" + ddl_material.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
            //while using month to pic
            //DateTime start;
            //DateTime end;
            //DateTime dchoose;
            //dchoose = Convert.ToDateTime(Request.QueryString["dari"]);            
            //start = new DateTime(dchoose.Year, dchoose.Month, 1);
            //end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
            //datepicker.Text = dchoose.ToString("MM-yyyy");

            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            ddl_material.Text = Request.QueryString["material"];
        }
    }

    protected void btnRegister_ServerClick(object sender, EventArgs e)
    {
        CheckBox cb = new CheckBox();

        int index = 1;
        bool isError = true;
        string Nomor = string.Empty;
        bool WOCreated = false;
        int IDWO = 0;

        string NIK = Db.SingleString("SELECT UserNumber FROM UserAccount WHERE ID = " + App.UserID, Db.Cnn_AC);

        foreach (TableRow row in tb.Rows)
        {
            if (index < tb.Rows.Count)
            {
                cb = (CheckBox)row.FindControl("cb_po" + index.ToString());
                if (cb.Checked == true)
                {
                    int temp = Convert.ToInt32(((HiddenField)row.FindControl("hf" + index.ToString())).Value);


                    if (!WOCreated)
                    {
                        while (isError)
                        {
                            Nomor = GenerateWorkOrder();
                            try
                            {
                                Db.Execute("INSERT INTO StockRetireDJ_WorkOrder (Nomor, NIK, Operator, OperatorTgl) VALUES ('" + Nomor + "', '" + NIK + "', '" + App.UserNama + "', GETDATE())", Db.Cnn_DB);
                                IDWO = Db.SingleInteger("SELECT ID FROM StockRetireDJ_WorkOrder WHERE Nomor = '" + Nomor + "'", Db.Cnn_DB);
                                isError = false;
                            }
                            catch { }
                        }
                        WOCreated = true;
                    }

                    Db.Execute("UPDATE StockRetireDJ SET IDWO = " + IDWO.ToString() + " WHERE ID = " + temp.ToString(), Db.Cnn_DB);
                }
            }

            index++;
        }

        Db.Execute("exec sp_syncWorkOrderLebur " + IDWO, Db.Cnn_DB);

        if (WOCreated)
        {
            string href = App.CurrentPath;
            href += "?d=1";
            href += "&nomor=" + Nomor;
            Response.Redirect(href);
        }
        else
        {
            lblMessage.Text = "Pilih item untuk membuat Work Order";
            lblMessage.Visible = true;
        }
    }

    private string GenerateWorkOrder()
    {
        string headPhrase = System.Configuration.ConfigurationManager.AppSettings["KodeMounting"];
        string datePhase = DateTime.Now.ToString("yyMMdd");

        string latest = Db.SingleString("SELECT TOP 1 Nomor FROM StockRetireDJ_WorkOrder WHERE OperatorTgl >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND OperatorTgl < '" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "' AND Nomor like '" + headPhrase + datePhase + "%' ORDER BY Nomor DESC", Db.Cnn_DB);
        int seq = string.IsNullOrEmpty(latest) ? 1 : Convert.ToInt32(latest.Split('-').Last()) + 1;

        string tailPhrase = ("0" + seq.ToString());
        tailPhrase = tailPhrase.Substring(tailPhrase.Length - 2, 2);

        string workOrder = headPhrase + datePhase + "-" + tailPhrase;

        return workOrder;
    }
}