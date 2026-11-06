using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class KodeKaretCleansingAddDetailRevision : System.Web.UI.Page
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
    protected string mycp { get { return Request.QueryString["cp"]; } }
    protected string mycb { get { return Request.QueryString["cb"]; } }
    protected string myrp { get { return Request.QueryString["rp"]; } }
    protected string myrb { get { return Request.QueryString["rb"]; } }

    protected string mybasic { get { return Request.QueryString["basic"]; } }
    protected string mysearch { get { return Request.QueryString["search"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        int iBasic = -1;
        string xsearch = "";

        if (!IsPostBack)
        {
            fillFilter();

            if (mybasic != null)
            {
                iBasic = Convert.ToInt32(ddl_basic.SelectedValue.ToString());
            }

            if (mysearch != null)
            {
                xsearch = txt_search.Text;
            }

            fillData(iBasic, xsearch);
        }

        lit_basic.Text = ddl_basic.SelectedItem.Text.ToString();
        lit_search.Text = txt_search.Text;
    }

    protected void fillFilter()
    {
        if (mybasic != null)
        {
            ddl_basic.SelectedValue = mybasic;
        }

        if (mysearch != null)
        {
            txt_search.Text = mysearch;
        }

    }

    protected void fillData(int iBasic, string xsearch, bool isGo = false)
    {
        if (myID > 0)
        {
            var d = db.KodeKaretCleansing.SingleOrDefault(p => p.ID == myID);

            if (d != null)
            {
                string str_sumber = "";
                string str_item = "";
                if (d.Sumber == 0)
                {
                    str_sumber = "Manual";
                }
                else if (d.Sumber == 1)
                {
                    str_sumber = "SPK";
                }
                else if (d.Sumber == 2)
                {
                    str_sumber = "Outsource";
                }

                str_item = d.CharProductItem.Nama + " (" + d.CharProductItem.NamaKode + ")";

                lit_sumber.Text = str_sumber; //////////////////////////////
                lit_productitem.Text = str_item; //////////////////////////////


                var x = db.KodeKaretCleansing_Detail.Where(p => p.IDForm == d.ID && p.Operator != null);

                int totalRevision = 0;
                totalRevision = x.Count();

                //if (d.DocStatus == 0) { btnSubmit.Visible = true; } else { btnSubmit.Visible = false; }
                //if (d.DocStatus == 1 && totalRevision > 0) { btnCancel.Visible = true; } else { btnCancel.Visible = false; }

                lit_status.Text = "";
                if (d.DocStatus == 0)
                {
                    lit_status.Text = "Draft";
                }
                else if (d.DocStatus == 1)
                {
                    lit_status.Text = "Waiting for Approval";
                }
                else if (d.DocStatus == 2)
                {
                    lit_status.Text = "Approved";
                }
                else if (d.DocStatus == 3)
                {
                    lit_status.Text = "Rejected";
                }

                lit_totalRevision.Text = totalRevision.ToString();

                int limitperpage = 10;

                int id = d.ID;
                int isumber = d.Sumber;
                int iproductionitem = d.ProductItem;

                int cp = 1;
                string cb = "1";
                int rp = 1;
                string rb = "1";

                //try
                //{
                //    isumber = (mysumber != null) ? Convert.ToInt32(mysumber) : isumber;
                //}
                //catch (Exception) { }

                //try
                //{
                //    iproductionitem = (myitem != null) ? Convert.ToInt32(myitem) : iproductionitem;
                //}
                //catch (Exception) { }

                try
                {
                    cp = (mycp != null) ? Convert.ToInt32(mycp) : cp;
                }
                catch (Exception) { }

                try
                {
                    cb = (mycb != null) ? mycb : cb;
                }
                catch (Exception) { }

                try
                {
                    rp = (myrp != null) ? Convert.ToInt32(myrp) : rp;
                }
                catch (Exception) { }

                try
                {
                    rb = (myrb != null) ? myrb : rb;
                }
                catch (Exception) { }


                fillDetail(isumber, iproductionitem, limitperpage, cp, cb, rp, rb, isGo, id, iBasic, xsearch);

                //err.Visible = true;
                //lit_err.Visible = true;
                //lit_err.Text = "";
                //lit_err.Text += "<br />sum : " + isumber.ToString();
                //lit_err.Text += "<br />item : " + iproductionitem.ToString();
                //lit_err.Text += "<br />limit : " + limitperpage.ToString();
                //lit_err.Text += "<br />td : " + totalrow.ToString();
                //lit_err.Text += "<br />cp : " + cp.ToString();
                //lit_err.Text += "<br />cb : " + cb.ToString();
                //lit_err.Text += "<br />rp : " + rp.ToString();
                //lit_err.Text += "<br />rb : " + rb.ToString();

            }
            
        }
    }

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        int iBasic = Convert.ToInt32(ddl_basic.SelectedValue.ToString());
        string xsearch = txt_search.Text.Trim();

        fillData(iBasic, xsearch);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var d = db.KodeKaretCleansing.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            d.DocStatus = 1;
            db.SubmitChanges();
        }

        Response.Redirect("KodeKaretCleansingAddDetailRevision.aspx?id=" + myID);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        var d = db.KodeKaretCleansing.SingleOrDefault(p => p.ID == myID);

        if (d != null)
        {
            d.DocStatus = 0;
            db.SubmitChanges();
        }

        Response.Redirect("KodeKaretCleansingAddDetailRevision.aspx?id=" + myID);
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        int iBasic = Convert.ToInt32(ddl_basic.SelectedValue.ToString());
        string xsearch = txt_search.Text.Trim();

        int irpgo = 0;
        string xrpgo = txt_gopage.Text.Trim();

        try
        {
            irpgo = Convert.ToInt32(xrpgo);
        }
        catch (Exception) { }

        if (irpgo > 0)
        {
            fillData(iBasic, xsearch, true);
        }
    }

    protected void fillPaging(int isumber, int iproductionitem, int limitperpage, int totalrow, int cp, string cb, int rp, string rb, bool isGo, int id, int iBasic, string xsearch)
    {
        Paging.BootstrapPaging(lit_paging, App.CurrentPath, isumber, iproductionitem, limitperpage, totalrow, cp, cb, rp, rb, id, iBasic, xsearch);
    }

    protected void fillDetail(int isumber, int iproductionitem, int limitperpage, int cp, string cb, int rp, string rb, bool isGo, int id, int iBasic, string xsearch)
    {
        var d = db.KodeKaretCleansing.SingleOrDefault(p => p.ID == id);

        if (d != null)
        {
            bool bBasic = false;
            //if (iBasic == 0) { bBasic = true; }
            if (iBasic == 1) { bBasic = true; }
            if (iBasic == 2) { bBasic = false; }

            var x = db.KodeKaretCleansing_Detail.Where(
                    p => p.StatusBasic == (iBasic <= 0 ? p.StatusBasic : bBasic)
                    && (p.ProtoNomor.Contains(xsearch) || p.KodeKaret.Contains(xsearch))
                    && p.IDForm == myID
                    && p.Operator != null
                    && p.OperatorTgl != null
                    );

            if (isGo)
            {
                int igo = Convert.ToInt32(txt_gopage.Text.Trim());
                int max = Convert.ToInt32(txt_hidden_max.Text.Trim());

                //igo = 54;
                //max = 54;

                //err.Visible = true;
                //lit_err.Visible = true;
                //lit_err.Text = "";
                //lit_err.Text += "<br />igo : " + igo.ToString();
                //lit_err.Text += "<br />max : " + max.ToString();

                if (igo == 1) { rp = 1; rb = "1"; }
                else if (igo == 2) { rp = 2; rb = "2"; }
                else if (igo == max) { rp = max; rb = "5"; }
                else if (igo == max - 1) { rp = max - 1; rb = "4"; }
                else { rp = igo; rb = "3"; }
            }

            Table formtable = tbl;
            string username = App.UserNama;
            string xsumber = isumber.ToString(); //////////////////////////////
            string xproductitem = iproductionitem.ToString(); //////////////////////////////

            string query = " exec sp_get_kode_karet_cleansing_list 5, " + xsumber + ", " + xproductitem + ", " + rp.ToString() + ", " + limitperpage.ToString() + ", " + id.ToString() + ", " + iBasic.ToString() + ", '" + xsearch.ToString() + "'";

            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

            int totalrow = 0;
            totalrow = dt.Rows.Count;

            int gtotalrec = x.Count();

            //if (dt != null) { App.TableNoResult(formtable); }
            //else if (totalrow == 0) { App.TableNoResult(formtable); }
            //else
            if (dt != null && totalrow > 0)
            {
                div_table.Visible = true;

                int index = 1;
                TableRow tr;
                TableCell td;

                // On all tables' rows
                foreach (DataRow dtRow in dt.Rows)
                {
                    tr = new TableRow();

                    // for aligning text
                    int a = 0;
                    List<int> listMiddle = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
                    List<int> listRight = new List<int>(new int[] { });

                    foreach (DataColumn dc in dt.Columns)
                    {
                        //if (index == 1 && a == 7) { gtotalrec = Convert.ToInt32(dtRow[dc].ToString()); }

                        if (a <= 8)
                        {
                            td = new TableCell();

                            if (dtRow[dc].ToString() != "")
                            {
                                string y = dtRow[dc].ToString();

                                if (a == 0)
                                {
                                    //td.Text = (d.DocStatus >= 1) ? "" : TD_Hyperlink(Convert.ToInt32(y));
                                    td.Text = "";
                                    tr.Cells.Add(td);
                                }
                                else if (a == 1)
                                {
                                    td.Text = y + ".";
                                    tr.Cells.Add(td);
                                }
                                else if (a == 5 || a == 6)
                                {
                                    td.Text = "<img src='/Images.aspx?path=" + y + "' width='100'>";
                                }
                                else if (a == 7)
                                {
                                    if (y == "True")
                                    {
                                        td.Text = "<span class='label label-info'>Basic</span>";
                                    }
                                }
                                else if (a == 2 || a == 3 || a == 8)
                                {
                                    if (a == 2) { td.Text = "<span class='text-bold'>" + y + "</span>"; }
                                    if (a == 3) { td.Text = "<span class='text-bold text-orange'>" + y + "</span>"; }
                                    if (a == 8) { td.Text = "<span class='text-bold text-blue'>" + y + "</span>"; }

                                    td.Text += "<br />(" + y.Length.ToString() + ")";

                                    if (a == 8) { td.Text += "<br />" + dtRow["Operator"].ToString() + "<br />" + Str.FullDateJam(Convert.ToDateTime(dtRow["OperatorTgl"].ToString())); }

                                }
                                else
                                {
                                    td.Text = y;
                                }

                            }
                            else
                            {
                                td.Text = "";
                            }

                            if (listMiddle.Contains(a))
                            {
                                td.Style.Add("text-align", "center");
                            }
                            else if (listRight.Contains(a))
                            {
                                td.Style.Add("text-align", "right");
                            }
                            else
                            {
                                td.Style.Add("text-align", "left");
                            }
                            td.Style.Add("vertical-align", "top");
                            tr.Cells.Add(td);

                        }

                        a++;
                    }

                    formtable.Rows.Add(tr);
                    index++;
                }

                lit_total.Text = gtotalrec.ToString();
            }
            else
            {
                ddl_basic.Enabled = false;
                txt_search.Enabled = false;
                btnDisplay.Visible = false;

                err.Visible = true;
                lit_err.Visible = true;
                lit_err.Text = "Belum ada list data yang akan direvisi. Silahkan melakukan seleksi revisi kode karet pada tab halaman List";

            }
            txt_hidden_max.Text = Math.Ceiling(Convert.ToDouble(gtotalrec) / Convert.ToDouble(limitperpage)).ToString();
            fillPaging(isumber, iproductionitem, limitperpage, gtotalrec, cp, cb, rp, rb, isGo, myID, iBasic, xsearch);
            if (gtotalrec > limitperpage) { div_paging.Visible = true; div_paging_gotopage.Visible = true; }

        }
    }


    //protected string TD_Hyperlink(int id)
    //{
    //    string x = string.Empty;

    //    x += "<a class=\"btn btn-xs btn-info\" target=\"_BLANK\" href=\"KodeKaretCleansingAddDetailInfo.aspx?id=" + id + "\">";
    //    x += "  <i class=\"icon-search\"></i> ";
    //    x += "";
    //    x += "</a>";

    //    return x;
    //}

}