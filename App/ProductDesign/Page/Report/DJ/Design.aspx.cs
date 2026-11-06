using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class Page_Design : System.Web.UI.Page {
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
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductCategory(productcategory);
            DLC.Designer(designer);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharProductSegmen(segmen);
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected class PD
    {
        public int brand_id;
        public int doc_id;
        public string material_code;
    }

    protected IEnumerable<PD> PD_IEnum(DataTable dataTable)
    {
        if(dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new PD
                {
                    brand_id = Convert.ToInt32(row["brand_id"]),
                    doc_id = Convert.ToInt32(row["doc_id"]),
                    material_code = Convert.ToString(row["material_code"]),
                };
            }
        }
        

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string xMsg = "";
        string xDari = dari.Text.Trim();
        string xSampai = sampai.Text.Trim();
        
        if(xDari == "" || xSampai == "")
        {
            xMsg += "Tanggal harus diisi ! ";
        }

        string[] format = new string[] { "dd-MM-yyyy" };
        //string valueDari = "2011-09-02 15:30:20";
        DateTime datetime;

        if (!(DateTime.TryParseExact(xDari, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime)))
        {
            xMsg += "Format tanggal dari invalid ! ";
        }

        if (!(DateTime.TryParseExact(xSampai, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out datetime)))
        {
            xMsg += "Format tanggal sampai invalid ! ";
        }


        if (xMsg != "")
        {
            lit_msg.Text = xMsg;
        }
        else
        {

            var rs = from p in db.ProductDesign
                     where p.Draft == false
                     && (from z in db.ProductDesign_CharDesign
                         select z.ID).Contains(p.ID)
                     && (from z in db.ProductDesign_CharProduct
                         select z.ID).Contains(p.ID)
                     && (from z in db.ProductDesign_PricingSegmen
                         select z.ID).Contains(p.ID)
                     && p.Tgl >= Convert.ToDateTime(xDari) 
                     && p.Tgl < Convert.ToDateTime(xSampai).AddDays(1)
                     select p;

            rs = rs.OrderByDescending(p => p.Tgl);

            string keyword = kw.Text.Trim();
            if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
            int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
            if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignCategory == DesignCategory);
            int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
            if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignConcept == DesignConcept);
            int TargetAge = Convert.ToInt32(targetage.SelectedValue);
            if (TargetAge > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetAge == TargetAge);
            int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
            if (TargetGender > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetGender == TargetGender);
            int ProductItem = Convert.ToInt32(productitem.SelectedValue);
            if (ProductItem > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductItem == ProductItem);
            int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
            if (ProductLevel > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductLevel == ProductLevel);
            int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
            if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductCategory == ProductCategory);
            int Designer = Convert.ToInt32(designer.SelectedValue);
            if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);
            int StoneDist = Convert.ToInt32(stonedistribution.SelectedValue);
            if (StoneDist > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.StoneDist == StoneDist);
            int ProductSegmen = Convert.ToInt32(segmen.SelectedValue);
            if (ProductSegmen > 0) rs = rs.Where(p => p.ProductDesign_PricingSegmen.ProductSegmen == ProductSegmen);

            if (status.SelectedValue == "1")
                rs = rs.Where(p => p.Approval != null);
            else if (status.SelectedValue == "2")
                rs = rs.Where(p => p.Approval == null);
            
            string spname = "[SP_SAP_PRODUCTDESIGN_GET_DATA]";
            string query = " exec " + spname + " "
                         + Str.isString(Convert.ToDateTime(xDari).ToString("yyyy-MM-dd"), false)
                         + Str.isString(Convert.ToDateTime(xSampai).ToString("yyyy-MM-dd"))
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            var x = PD_IEnum(dt);

            //if(x != null)
            //{
            //    if(x.Count() > 0)
            //    {
            //        foreach(var data in x)
            //        {
            //            lit_msg.Text += "ienum_" + data.material_code + "<br />";
            //        }
            //    }
            //}
            

            //if (dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dtRow in dt.Rows)
            //    {
            //        lit_msg.Text += "dtRow_" + dtRow["material_code"].ToString() + "<br />";
                    
            //    }
            //}

            if (rs != null)
            {
                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;

                    //Sub-Header
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                    
                    td = new TableCell();
                    td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                    
                    td = new TableCell();
                    td.Text = r.Nomor
                            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                            + Str.FullDate(r.Tgl)
                            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                            + r.HumanDesignerNama
                            ;
                    td.Style.Add("text-align","center");
                    tr.Cells.Add(td);
                    
                    //td = new TableCell();
                    //td.Text = TD_Data(r);
                    //tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProductDesign_CharProduct.CharProductItem.Nama
                            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                            + r.ProductDesign_CharProduct.CharProductCategory.Nama
                            ;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProductDesign_CharProduct.CharProductLevel.Nama
                            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                            + r.ProductDesign_CharProduct.CharStoneDist.Nama
                            ;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                    
                    td = new TableCell();
                    td.Text = r.ProductDesign_CharDesign.CharDesignCategory.Nama
                            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                            + r.ProductDesign_CharDesign.CharDesignConcept.Nama
                            ;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                    
                    td = new TableCell();
                    td.Text = r.ProductDesign_PricingSegmen.CharProductSegmen.Nama;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.ProductDesign_CharDesign.Naming;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);
                    
                    var xmat = x.Where(p => p.doc_id == r.ID && p.brand_id == r.DataHuman.IDBrand);
                    string matcode = "";
                    if(xmat.Count() > 0)
                    {
                        foreach(var data in xmat)
                        {
                            matcode += data.material_code + "<br />";
                        }
                    }
                    
                    td = new TableCell();
                    td.Text = matcode;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);
                }
            }
        }

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            status.SelectedValue = Request.QueryString["status"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            segmen.SelectedValue = Request.QueryString["segmen"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            designer.Text = Request.QueryString["designer"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&segmen=" + segmen.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(ProductDesign r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Design_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        return x;
    }
    protected string TD_Data(ProductDesign r)
    {
        string x = string.Empty;

        if (r.ID > 0 && r.Nomor != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "</p>";

        return x;
    }
    protected string TD_Product01(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }

    protected string TD_ProductChar(string text)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += text;
        x += "</p>";

        return x;
    }
}