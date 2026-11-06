using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_LD : System.Web.UI.Page
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
    protected int AuditID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //Set();
            if (Request.QueryString["nomeja"] != "")
                nomeja.Text = Request.QueryString["nomeja"];
        }
        Set();
    }

    protected void Set()
    {
        var d = db.StockAuditLD.SingleOrDefault(p => p.ID == AuditID);
        if (d == null)
            Response.Redirect("LD_LocationType.aspx");
        else
        {
            if (ItemID > 0)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == ItemID);
                if (x != null)
                    Upload.Image(imgpicture, x.ImgPicture);
            }

            var rs = (from p in db.StockAuditLDDetail
                      where p.AuditID == d.ID
                      && p.Operator == "" + App.UserNama + ""
                      orderby p.ID descending
                      select p).Take(5);
            rs = rs.OrderByDescending(p => p.ID);

            if (rs != null)
            {
                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductLD_Stone1B.Stone1B.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.HargaTotal).ToString("N0");
                    tr.Cells.Add(td);

                    
                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalButir).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalCarat).ToString("N3");
                    tr.Cells.Add(td);

                    CheckBox cb_img = new CheckBox();

                    cb_img.ID = "cb_img" + index.ToString();
                    cb_img.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    if (r.ImgCheck == true)
                        cb_img.Checked = true;
                    cb_img.AutoPostBack = true;
                    cb_img.CheckedChanged += new EventHandler(this.SaveAudit);

                    td = new TableCell();
                    td.Controls.Add(cb_img);
                    tr.Cells.Add(td);

                    CheckBox cb_bd = new CheckBox();

                    cb_bd.ID = "cb_bd" + index.ToString();
                    cb_bd.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    if (r.BandrolCheck == true)
                        cb_bd.Checked = true;
                    cb_bd.AutoPostBack = true;
                    cb_bd.CheckedChanged += new EventHandler(this.SaveAudit);

                    td = new TableCell();
                    td.Controls.Add(cb_bd);
                    tr.Cells.Add(td);

                    TextBox txt2 = new TextBox();
                    txt2.ID = "tb2_" + index.ToString();
                    txt2.Attributes["style"] = "width: 100px;";
                    txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txt2.TextMode = TextBoxMode.MultiLine;
                    txt2.Text = r.NoMeja;
                    txt2.AutoPostBack = true;
                    txt2.TextChanged += new EventHandler(this.SaveAuditKeterangan);

                    td = new TableCell();
                    td.Controls.Add(txt2);
                    tr.Cells.Add(td);

                    TextBox txt = new TextBox();
                    txt.ID = "tb_" + index.ToString();
                    txt.Attributes["style"] = "width: 200px;";
                    txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txt.TextMode = TextBoxMode.MultiLine;
                    txt.Text = r.Keterangan;
                    txt.AutoPostBack = true;
                    txt.TextChanged += new EventHandler(this.SaveAuditKeterangan);

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.IDProduct.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(txt);
                    td.Controls.Add(hf);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Operator;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }

                //if (index > 1)
                //{
                //    var tf = ((TextBox)tb.FindControl("tb_1"));
                //    tf.Focus();
                //}
                kw.Focus();

                string List = "";
                var rd = from p in db.StockAuditLDDetail
                         where p.AuditID == d.ID
                         select p;
                var u = rd.GroupBy(p => p.StockProductLD_Stone1B.ProductItem);
                foreach (var r in u)
                {
                    var it = db.CharProductItem.SingleOrDefault(p => p.ID == r.Key);
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = it.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    tb2.Rows.Add(tr);
                }

                tb2.Attributes["style"] = "vertical-align: top !important;";

                int totalitem = 0;
                var u2 = rd.GroupBy(p => p.NoMeja);
                foreach (var r in u2)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Key;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    tb3.Rows.Add(tr);

                    totalitem += r.Count();
                }

                tb3.Attributes["style"] = "vertical-align: top !important;";

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = totalitem.ToString();
                tr2.Cells.Add(td2);

                tb3.Rows.Add(tr2);
            }
        }
    }

    protected void Add(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";
        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == AuditID);
                if (d != null)
                {
                    d.ImgCheck = cb_img.Checked;
                    d.BandrolCheck = cb_bd.Checked;
                    d.Keterangan = txt.Text;
                    d.NoMeja = txt2.Text;
                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }

        var rs = from p in db.StockAuditLDDetail
                 where p.AuditID == AuditID
                 select p;

        decimal TotalItem = 0m;
        if (rs != null)
        {
            TotalItem = rs.Count();
        }

        var x = db.StockAuditLD.SingleOrDefault(p => p.ID == AuditID);
        if (x != null)
        {
            x.StatusFinish = false;
            x.FinishJam = DateTime.Now;
            x.TotalItemActual = TotalItem;
            db.SubmitChanges();
        }

        string plu = kw.Text;
        if (plu != "")
        {
            var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == "" + plu + "" && !p.StockProductLD.Draft.Value);
            if (d == null)
            {
                err.InnerText = "PLU Tidak ditemukan.";
                err.Visible = true;
            }
            else
            {
                Upload.Image(imgpicture, d.ImgPicture);
                err.Visible = false;

                //var x = db.StockAuditLDDetail.SingleOrDefault(p => p.AuditID == AuditID && p.IDProduct == d.ID);
                //if (x == null)
                //{
                StockAuditLDDetail item = new StockAuditLDDetail();

                item.AuditID = AuditID;
                item.Nomor = d.Nomor;
                item.IDProduct = d.ID;
                item.Operator = App.UserNama;
                item.OperatorTgl = DateTime.Now;
                item.Keterangan = "";
                item.NoMeja = nomeja.Text;

                db.StockAuditLDDetail.InsertOnSubmit(item);
                db.SubmitChanges();
                //}
                Response.Redirect("LD_Opname.aspx?id=" + AuditID + "&item=" + d.ID.ToString() + "&nomeja=" + nomeja.Text);
            }
        }
    }

    protected void Finish(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == AuditID);
                if (d != null)
                {
                    d.ImgCheck = cb_img.Checked;
                    d.BandrolCheck = cb_bd.Checked;
                    d.NoMeja = txt2.Text;
                    d.Keterangan = txt.Text;
                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }

        var rs = from p in db.StockAuditLDDetail
                 where p.AuditID == AuditID
                 select p;

        decimal TotalItem = 0m;
        if (rs != null)
        {
            TotalItem = rs.Count();
        }

        var x = db.StockAuditLD.SingleOrDefault(p => p.ID == AuditID);
        if (x != null)
        {
            x.StatusFinish = true;
            x.FinishJam = DateTime.Now;
            x.TotalItemActual = TotalItem;
            db.SubmitChanges();
        }

        Response.Redirect("LD_LocationType.aspx?d=1");
    }
    protected void SaveAudit(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        CheckBox cb_imgnone = new CheckBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_imgnone = (CheckBox)row.FindControl("cb_imgnone" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == AuditID);
                if (d != null)
                {
                    //d.ImgNone = cb_imgnone.Checked;
                    d.ImgCheck = cb_img.Checked;
                    d.BandrolCheck = cb_bd.Checked;
                    d.NoMeja = txt2.Text;
                    d.Keterangan = txt.Text;
                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        CheckBox checkBox = sender as CheckBox;
        checkBox.Focus();
    }
    protected void SaveAuditKeterangan(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        CheckBox cb_imgnone = new CheckBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";

        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_imgnone = (CheckBox)row.FindControl("cb_imgnone" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == AuditID);
                if (d != null)
                {
                    //d.ImgNone = cb_imgnone.Checked;
                    d.ImgCheck = cb_img.Checked;
                    d.BandrolCheck = cb_bd.Checked;
                    d.NoMeja = txt2.Text;
                    d.Keterangan = txt.Text;
                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        TextBox tbox = sender as TextBox;
        tbox.Focus();
    }
}