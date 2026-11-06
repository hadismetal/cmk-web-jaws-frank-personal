using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class KodeKaretCleansingAdd : System.Web.UI.Page
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
    protected string mysumber { get { return Request.QueryString["sumber"]; } }
    protected string myitem { get { return Request.QueryString["item"]; } }

    protected string mytd { get { return Request.QueryString["td"]; } }
    protected string mycp { get { return Request.QueryString["cp"]; } }
    protected string mycb { get { return Request.QueryString["cb"]; } }
    protected string myrp { get { return Request.QueryString["rp"]; } }
    protected string myrb { get { return Request.QueryString["rb"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            btnRegister.Visible = false;
            DLC.CharProductItem(ddl_productitem);

            if (mysumber != null && myitem != null) { fillData(); }
        }
    }

    protected void fillData(bool isGo = false)
    {
        if (mysumber != null) { ddl_sumber.SelectedValue = mysumber; }
        if (myitem != null) { ddl_productitem.SelectedValue = myitem; }

        int limitperpage = 10;

        int isumber = Convert.ToInt32(ddl_sumber.SelectedValue);
        int iproductionitem = Convert.ToInt32(ddl_productitem.SelectedValue);

        int cp = 1;
        string cb = "1";
        int rp = 1;
        string rb = "1";

        try
        {
            isumber = (mysumber != null) ? Convert.ToInt32(mysumber) : isumber;
        }
        catch (Exception) { }

        try
        {
            iproductionitem = (myitem != null) ? Convert.ToInt32(myitem) : iproductionitem;
        }
        catch (Exception) { }

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

        fillDetail(isumber, iproductionitem, limitperpage, cp, cb, rp, rb, isGo);
        
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

    protected void btnDisplay_Click(object sender, EventArgs e)
    {
        err.Visible = false;
        lit_err.Visible = false;
        lit_err.Text = "";

        lit_sumber.Text = "...";
        lit_productitem.Text = "...";
        lit_total.Text = "0";

        fillData();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        int irpgo = 0;
        string xrpgo = txt_gopage.Text.Trim();

        try
        {
            irpgo = Convert.ToInt32(xrpgo);
        }
        catch (Exception) { }

        if (irpgo > 0)
        {
            fillData(true);
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        int xsumber = Convert.ToInt32(ddl_sumber.SelectedValue);
        int xproductitem = Convert.ToInt32(ddl_productitem.SelectedValue);

        if(xsumber >= 0 && xproductitem > 0)
        {
            var d = db.KodeKaretCleansing.Where(p => p.Sumber == xsumber && p.ProductItem == xproductitem && p.Approval == null);

            if (d.Count() == 0)
            {
                KodeKaretCleansing n = new KodeKaretCleansing();

                n.Sumber = xsumber;
                n.ProductItem = xproductitem;
                n.Operator = App.UserNama;
                n.OperatorTgl = System.DateTime.Now;
                n.DocStatus = 0;

                db.KodeKaretCleansing.InsertOnSubmit(n);
                db.SubmitChanges();

                try
                {
                    Db.Execute("EXEC sp_get_kode_karet_cleansing_list 3, " + xsumber + ", " + xproductitem + ", 0, 0, " + n.ID.ToString() , Db.Cnn_DB);
                    
                }
                catch (Exception) { }

                Response.Redirect("KodeKaretCleansingAddDetail.aspx?id=" + n.ID);
            }
            else
            {
                string txtButtonGo = "";

                txtButtonGo += "<br /><br /><a href='KodeKaretCleansingAddDetail.aspx?id=" + d.First().ID.ToString() + "' class='btn btn-warning'  >";
                txtButtonGo += "<i class='icon-arrow-right'></i> Go to Document List";
                txtButtonGo += "</a>";

                err.Visible = true;
                lit_err.Visible = true;
                lit_err.Text = "Warning !!!<br />Terdapat Dokumen yang masih pending Approval." + txtButtonGo;

            }
        }
    }

    protected void fillPaging(int isumber, int iproductionitem, int limitperpage, int totalrow, int cp, string cb, int rp, string rb, bool isGo)
    {
        Paging.BootstrapPaging(lit_paging, App.CurrentPath, isumber, iproductionitem, limitperpage, totalrow, cp, cb, rp, rb);
    }

    protected void fillDetail(int isumber, int iproductionitem, int limitperpage, int cp, string cb, int rp, string rb, bool isGo)
    {
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
        string xsumber = ddl_sumber.SelectedValue;
        string xproductitem = ddl_productitem.SelectedValue;

        string query = " exec sp_get_kode_karet_cleansing_list 2, " + xsumber + ", " + xproductitem + ", " + rp.ToString() + ", " + limitperpage.ToString();

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        int totalrow = 0;
        totalrow = dt.Rows.Count;

        int gtotalrec = 0;

        if (dt == null) { App.TableNoResult(formtable); }
        else if (totalrow == 0) { App.TableNoResult(formtable); }
        else
        {
            div_table.Visible = true; 
            btnRegister.Visible = true; /// true if not exist on document pending approval

            int index = 1;
            TableRow tr;
            TableCell td;

            // On all tables' rows
            foreach (DataRow dtRow in dt.Rows)
            {
                tr = new TableRow();

                //td = new TableCell();
                //td.Text = index.ToString() + ".";
                //td.Style.Add("text-align", "center");
                //td.Style.Add("vertical-align", "top");
                //tr.Cells.Add(td);

                // for aligning text
                int a = 0;
                List<int> listMiddle = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6 });
                List<int> listRight = new List<int>(new int[] {  });

                foreach (DataColumn dc in dt.Columns)
                {
                    if (index == 1 && a == 7) { gtotalrec = Convert.ToInt32(dtRow[dc].ToString()); }

                    if (a < 7)
                    {
                        td = new TableCell();

                        if (dtRow[dc].ToString().Trim() != "")
                        {
                            if (a == 0)
                            {
                                td.Text = dtRow[dc].ToString() + ".";
                            }
                            else if (a == 4 || a == 5)
                            {
                                td.Text = "<img src='/Images.aspx?path=" + dtRow[dc].ToString() + "' width='100'>";
                            }
                            else if (a == 6)
                            {
                                td.Text = "<span class='label label-info'>Basic</span>";
                            }
                            else
                            {
                                td.Text = dtRow[dc].ToString();
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

            lit_sumber.Text = ddl_sumber.SelectedItem.Text.ToString();
            lit_productitem.Text = ddl_productitem.SelectedItem.Text.ToString();
            lit_total.Text = gtotalrec.ToString();
        }

        txt_hidden_max.Text = Math.Ceiling(Convert.ToDouble(gtotalrec) / Convert.ToDouble(limitperpage)).ToString();
        fillPaging(isumber, iproductionitem, limitperpage, gtotalrec, cp, cb, rp, rb, isGo);
        if (gtotalrec > limitperpage) { div_paging.Visible = true; div_paging_gotopage.Visible = true; }

    }
}