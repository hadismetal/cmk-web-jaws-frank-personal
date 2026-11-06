using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int TipeID { get { return App.QueryInt(this, "tipe"); } }
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
        
        var d = db.StockAudit.SingleOrDefault(p => p.ID == AuditID);
        if (d == null)
        {
            Response.Redirect("Bulk_LocationType.aspx");
        }
        else
        {
            var bulkid = db.StockAudit_Detail.SingleOrDefault(p => p.IDForm == d.ID);

            //// Picture
            if (ItemID > 0)
            {
                if (TipeID == 1)
                {
                    var x = (from p in db.StockAuditDJDetail
                             where p.AuditID == bulkid.IDDJ && p.IDProduct == ItemID
                             select new
                             {
                                 ImgPicture = p.StockProductDJ.ImgPicture
                             }).First();
                    if (x != null) { Upload.Image(imgpicture, x.ImgPicture); }
                }
                else if (TipeID == 2)
                {
                    var x = (from p in db.StockAuditGJDetail
                             where p.AuditID == bulkid.IDGJ && p.IDProduct == ItemID
                             select new
                             {
                                 ImgPicture = p.StockProductGJ.ImgPicture
                             }).First();
                    if (x != null) { Upload.Image(imgpicture, x.ImgPicture); }
                }
                else if (TipeID == 3)
                {
                    var x = (from p in db.StockAuditPGDetail
                             where p.AuditID == bulkid.IDPG && p.IDProduct == ItemID
                             select new
                             {
                                 ImgPicture = p.StockProductPG.ImgPicture
                             }).First();
                    if (x != null) { Upload.Image(imgpicture, x.ImgPicture); }
                }
                else if (TipeID == 4)
                {
                    var x = (from p in db.StockAuditLDDetail
                             where p.AuditID == bulkid.IDLD && p.IDProduct == ItemID
                             select new
                             {
                                 ImgPicture = p.StockProductLD_Stone1B.ImgPicture
                             }).First();
                    if (x != null) { Upload.Image(imgpicture, x.ImgPicture); }
                }

            }


            //// List
            var rs = (from p in db.StockAuditDJDetail
                      where p.AuditID == bulkid.IDDJ
                      select new
                      {
                          Harga = p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value,
                          Carat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                          Butir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                          GrossWeight = p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight,
                          NettoWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                          Nomor = p.StockProductDJ.Nomor,
                          IDProduct = p.StockProductDJ.ID,
                          ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                          IDItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.ID,
                          NoMeja = p.NoMeja,
                          ImgPicture = p.StockProductDJ.ImgPicture,
                          Tipe = 1,
                          OperatorNama = p.Operator,
                          OperatorTgl = p.OperatorTgl,
                          ImgNone = p.ImgNone.HasValue ? p.ImgNone.Value : false,
                          ImgCheck = p.ImgCheck,
                          BandrolCheck = p.BandrolCheck,
                          Keterangan = p.Keterangan
                      }
                          ).Concat(
                         from p in db.StockAuditGJDetail
                         where p.AuditID == bulkid.IDGJ
                         select new
                          {
                              Harga = p.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value,
                              Carat = 0m,
                              Butir = 0m,
                              GrossWeight = p.StockProductGJ.StockProductGJ_CharProduct.GrossWeight,
                              NettoWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                              Nomor = p.StockProductGJ.Nomor,
                              IDProduct = p.StockProductGJ.ID,
                              ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                              IDItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.ID,
                              NoMeja = p.NoMeja,
                              ImgPicture = p.StockProductGJ.ImgPicture,
                              Tipe = 2,
                              OperatorNama = p.Operator,
                              OperatorTgl = p.OperatorTgl,
                              ImgNone = p.ImgNone.HasValue ? p.ImgNone.Value : false,
                              ImgCheck = p.ImgCheck,
                              BandrolCheck = p.BandrolCheck,
                              Keterangan = p.Keterangan
                          }).Concat(
                         from p in db.StockAuditPGDetail
                         where p.AuditID == bulkid.IDPG
                         select new
                         {
                             Harga = p.StockProductPG.TotalHargaJual.Value,
                             Carat = 0m,
                             Butir = 0m,
                             GrossWeight = p.StockProductPG.GrossWeight.Value,
                             NettoWeight = 0m,
                             Nomor = p.StockProductPG.Nomor,
                             IDProduct = p.StockProductPG.ID,
                             ProductItem = p.StockProductPG.CharProductItem.Nama,
                             IDItem = p.StockProductPG.ProductItem,
                             NoMeja = p.NoMeja,
                             ImgPicture = p.StockProductPG.ImgPicture,
                             Tipe = 3,
                             OperatorNama = p.Operator,
                             OperatorTgl = p.OperatorTgl,
                             ImgNone = p.ImgNone.HasValue ? p.ImgNone.Value : false,
                             ImgCheck = p.ImgCheck,
                             BandrolCheck = p.BandrolCheck,
                             Keterangan = p.Keterangan
                         }).Concat(
                         from p in db.StockAuditLDDetail
                         where p.AuditID == bulkid.IDLD
                         select new
                         {
                             Harga = 0m,
                             Carat = p.StockProductLD_Stone1B.TotalCarat,
                             Butir = 1m,
                             GrossWeight = 0m,
                             NettoWeight = 0m,
                             Nomor = p.StockProductLD_Stone1B.Nomor,
                             IDProduct = p.StockProductLD_Stone1B.ID,
                             ProductItem = p.StockProductLD_Stone1B.CharProductItem.Nama,
                             IDItem = p.StockProductLD_Stone1B.ProductItem.Value,
                             NoMeja = p.NoMeja,
                             ImgPicture = "",
                             Tipe = 4,
                             OperatorNama = p.Operator,
                             OperatorTgl = p.OperatorTgl,
                             ImgNone = false,
                             ImgCheck = p.ImgCheck,
                             BandrolCheck = p.BandrolCheck,
                             Keterangan = p.Keterangan
                         }).OrderByDescending(z => z.OperatorTgl);

            var rd = rs.Where(p => p.OperatorNama == App.UserNama);
            int totaldatabyuser = rd.Count();
            rd.Take(10); 
            int totaldata = rs.Count();

            //var rp = rd.Where(p => p.Tipe == TipeID && p.IDProduct == ItemID).OrderByDescending(z => z.OperatorTgl).First();
            //if (rp != null) { Upload.Image(imgpicture, rp.ImgPicture); }

            var u = rs.GroupBy(p => p.ProductItem);
            foreach (var r in u)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Key;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Count().ToString();
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);
            }

            int totalitem = 0;
            var u2 = rs.GroupBy(p => p.NoMeja);
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

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = totalitem.ToString();
            tr2.Cells.Add(td2);

            tb3.Rows.Add(tr2);

            if (rd != null)
            {
                int maxdata = 10;
                if (rd.Count() > 0)
                {

                    int totalitemaudit = Convert.ToInt32(db.StockAudit.Single(p => p.ID == AuditID).TotalItemAudit);
                
                    lit_countdata.Text = "Showing last " + (rd.Count() < maxdata ? rd.Count().ToString() : maxdata.ToString())  + " / " + totaldatabyuser.ToString() + " data input by " + App.UserNama
                                       + "<br />Total data input by multiuser " + Str.Number0(totaldata) + " / " + Str.Number0(totalitemaudit) + " data (" + Str.Number2((Convert.ToDecimal(totaldata) / Convert.ToDecimal(totalitemaudit)) * 100) + "%)";

                }

                rd = rd.Take(maxdata);

                int index = 1;
                foreach (var r in rd)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.Nomor + "<br />" + r.ProductItem; ;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.Harga).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.NettoWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.Butir).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.Carat).ToString("N3");
                    tr.Cells.Add(td);

                    CheckBox cb_imgnone = new CheckBox();

                    cb_imgnone.ID = "cb_imgnone" + index.ToString();
                    cb_imgnone.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    if (r.ImgNone == true)
                        cb_imgnone.Checked = true;
                    cb_imgnone.AutoPostBack = true;
                    cb_imgnone.CheckedChanged += new EventHandler(this.SaveAudit);

                    td = new TableCell();
                    td.Attributes["style"] = "text-align:center;";
                    td.Controls.Add(cb_imgnone);
                    tr.Cells.Add(td);

                    CheckBox cb_img = new CheckBox();

                    cb_img.ID = "cb_img" + index.ToString();
                    cb_img.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    if (r.ImgCheck == true)
                        cb_img.Checked = true;
                    cb_img.AutoPostBack = true;
                    cb_img.CheckedChanged += new EventHandler(this.SaveAudit);

                    td = new TableCell();
                    td.Attributes["style"] = "text-align:center;";
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
                    td.Attributes["style"] = "text-align:center;";
                    td.Controls.Add(cb_bd);
                    tr.Cells.Add(td);

                    TextBox txt2 = new TextBox();
                    txt2.ID = "tb2_" + index.ToString();
                    txt2.Attributes["style"] = "width: 90px;";
                    txt2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txt2.Text = r.NoMeja;
                    txt2.AutoPostBack = true;
                    txt2.TextChanged += new EventHandler(this.SaveAuditKeterangan);

                    td = new TableCell();
                    td.Controls.Add(txt2);
                    tr.Cells.Add(td);

                    TextBox txt = new TextBox();
                    txt.ID = "tb_" + index.ToString();
                    txt.Attributes["style"] = "width: 90px;";
                    txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    txt.Text = r.Keterangan;
                    txt.AutoPostBack = true;
                    txt.TextChanged += new EventHandler(this.SaveAuditKeterangan);

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.IDProduct.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    HiddenField hf2 = new HiddenField();
                    hf2.ID = "hf2_" + index.ToString();
                    hf2.Value = r.Tipe.ToString();
                    hf2.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(txt);
                    td.Controls.Add(hf);
                    td.Controls.Add(hf2);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.OperatorNama + "<br />" + Str.FullDateJam(r.OperatorTgl);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }

                kw.Focus();

            }
        }
    }

    protected void Add(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        TextBox txt2 = new TextBox();
        CheckBox cb_imgnone = new CheckBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";
        string hf2 = "";

        var us = db.StockAudit.SingleOrDefault(p => p.ID == AuditID);
        var bulkid = db.StockAudit_Detail.SingleOrDefault(p => p.IDForm == us.ID);

        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                hf2 = ((HiddenField)row.FindControl("hf2_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_imgnone = (CheckBox)row.FindControl("cb_imngnone" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());

                if (hf2 == "1")
                {
                    var d = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDDJ);
                    if (d != null)
                    {

                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.Keterangan = txt.Text;
                        d.NoMeja = txt2.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "2")
                {
                    var d = db.StockAuditGJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDGJ);
                    if (d != null)
                    {

                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.Keterangan = txt.Text;
                        d.NoMeja = txt2.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "3")
                {
                    var d = db.StockAuditPGDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDPG);
                    if (d != null)
                    {

                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.Keterangan = txt.Text;
                        d.NoMeja = txt2.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "4")
                {
                    var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDLD);
                    if (d != null)
                    {

                        //d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.Keterangan = txt.Text;
                        d.NoMeja = txt2.Text;
                        db.SubmitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }

        //var rs = from p in db.StockAuditDJDetail
        //         where p.AuditID == AuditID
        //         select p;

        //decimal TotalItem = 0m;
        //if (rs != null)
        //{
        //    TotalItem = rs.Count();
        //}

        //var x = db.StockAuditDJ.SingleOrDefault(p => p.ID == AuditID);
        //if (x != null)
        //{
        //    x.StatusFinish = false;
        //    x.FinishJam = DateTime.Now;
        //    x.TotalItemActual = TotalItem;
        //    db.SubmitChanges();
        //}

        string plu = kw.Text.Trim();
        if (plu != "")
        {
            //var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor == "" + plu + "");
            //if (d == null)
            //{
            //    err.InnerText = "PLU Tidak ditemukan.";
            //    err.Visible = true;
            //}

            

            var rslt = (from p in db.StockProductDJ
                     where p.Nomor.Equals(plu) && !p.Draft.Value
                     select new 
                     {
                         ID = p.ID,
                         Nomor = p.Nomor,
                         Tipe = 1,
                         ImgPicture = p.ImgPicture
                     }).Concat(
                     from p in db.StockProductGJ
                     where p.Nomor.Equals(plu) && !p.Draft.Value
                     select new 
                     {
                         ID = p.ID,
                         Nomor = p.Nomor,
                         Tipe = 2,
                         ImgPicture = p.ImgPicture
                     }).Concat(
                     from p in db.StockProductPG
                     where p.Nomor.Equals(plu) && !p.Draft.Value
                     select new 
                     {
                         ID = p.ID,
                         Nomor = p.Nomor,
                         Tipe = 3,
                         ImgPicture = p.ImgPicture
                     }).Concat(
                     from p in db.StockProductLD_Stone1B
                     where p.Nomor.Equals(plu) && !p.StockProductLD.Draft.Value
                     select new 
                     {
                         ID = p.ID,
                         Nomor = p.Nomor,
                         Tipe = 4,
                         ImgPicture = ""
                     });
            if (rslt.Count() == 0)
            {
                err.InnerText = "PLU Tidak ditemukan.";
                err.Visible = true;
            }
            else
            {
                var d = rslt.First();
                Upload.Image(imgpicture, d.ImgPicture);
                err.Visible = false;

                //var x = db.StockAuditDJDetail.SingleOrDefault(p => p.AuditID == AuditID && p.IDProduct == d.ID);
                //if (x == null)
                //{
                if (d.Tipe == 1)
                {
                    StockAuditDJDetail item = new StockAuditDJDetail();

                    item.AuditID = bulkid.IDDJ;
                    item.Nomor = d.Nomor;
                    item.IDProduct = d.ID;
                    item.Operator = App.UserNama;
                    item.OperatorTgl = DateTime.Now;
                    item.Keterangan = "";
                    item.NoMeja = nomeja.Text;

                    db.StockAuditDJDetail.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                else if (d.Tipe == 2)
                {
                    StockAuditGJDetail item = new StockAuditGJDetail();

                    item.AuditID = bulkid.IDGJ;
                    item.Nomor = d.Nomor;
                    item.IDProduct = d.ID;
                    item.Operator = App.UserNama;
                    item.OperatorTgl = DateTime.Now;
                    item.Keterangan = "";
                    item.NoMeja = nomeja.Text;

                    db.StockAuditGJDetail.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                else if (d.Tipe == 3)
                {
                    StockAuditPGDetail item = new StockAuditPGDetail();

                    item.AuditID = bulkid.IDPG;
                    item.Nomor = d.Nomor;
                    item.IDProduct = d.ID;
                    item.Operator = App.UserNama;
                    item.OperatorTgl = DateTime.Now;
                    item.Keterangan = "";
                    item.NoMeja = nomeja.Text;

                    db.StockAuditPGDetail.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                else if (d.Tipe == 4)
                {
                    StockAuditLDDetail item = new StockAuditLDDetail();

                    item.AuditID = bulkid.IDLD;
                    item.Nomor = d.Nomor;
                    item.IDProduct = d.ID;
                    item.Operator = App.UserNama;
                    item.OperatorTgl = DateTime.Now;
                    item.Keterangan = "";
                    item.NoMeja = nomeja.Text;

                    db.StockAuditLDDetail.InsertOnSubmit(item);
                    db.SubmitChanges();
                }

                Response.Redirect("Bulk_Opname.aspx?id=" + AuditID + "&item=" + d.ID.ToString() + "&nomeja=" + nomeja.Text + "&tipe=" + d.Tipe);
            }
        }
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
        string hf2 = "";

        var us = db.StockAudit.SingleOrDefault(p => p.ID == AuditID);
        var bulkid = db.StockAudit_Detail.SingleOrDefault(p => p.IDForm == us.ID);

        foreach (TableRow row in tb.Rows)
        {
            
            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                hf2 = ((HiddenField)row.FindControl("hf2_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_imgnone = (CheckBox)row.FindControl("cb_imgnone" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                if (hf2 == "1")
                {
                    var d = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDDJ);
                    if (d != null)
                    {
                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "2")
                {
                    
                    var d = db.StockAuditGJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDGJ);
                    if (d != null)
                    {
                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "3")
                {
                    var d = db.StockAuditPGDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDPG);
                    if (d != null)
                    {
                        
                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "4")
                {
                    var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDLD);
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
        string hf2 = "";

        var us = db.StockAudit.SingleOrDefault(p => p.ID == AuditID);
        var bulkid = db.StockAudit_Detail.SingleOrDefault(p => p.IDForm == us.ID);

        foreach (TableRow row in tb.Rows)
        {
            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                hf2 = ((HiddenField)row.FindControl("hf2_" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                txt2 = (TextBox)row.FindControl("tb2_" + index.ToString());
                cb_imgnone = (CheckBox)row.FindControl("cb_imgnone" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                if (hf2 == "1")
                {
                    var d = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDDJ);
                    if (d != null)
                    {
                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "2")
                {

                    var d = db.StockAuditGJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDGJ);
                    if (d != null)
                    {
                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "3")
                {
                    var d = db.StockAuditPGDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDPG);
                    if (d != null)
                    {

                        d.ImgNone = cb_imgnone.Checked;
                        d.ImgCheck = cb_img.Checked;
                        d.BandrolCheck = cb_bd.Checked;
                        d.NoMeja = txt2.Text;
                        d.Keterangan = txt.Text;
                        db.SubmitChanges();
                    }
                }
                else if (hf2 == "4")
                {
                    var d = db.StockAuditLDDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == bulkid.IDLD);
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

            }
            catch (Exception ex)
            {
            }
            index++;
        }
        //TextBox tbox = sender as TextBox;
        //tbox.Focus();
        kw.Focus();
    }
    protected void Finish(object sender, EventArgs e)
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
                var d = db.StockAuditDJDetail.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.AuditID == AuditID);
                if (d != null)
                {
                    d.ImgNone = cb_imgnone.Checked;
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

        var rs = from p in db.StockAuditDJDetail
                 where p.AuditID == AuditID
                 select p;

        decimal TotalItem = 0m;

        TotalItem += rs.Count();

        var rs2 = from p in db.StockAuditGJDetail
                 where p.AuditID == AuditID
                 select p;

        TotalItem += rs2.Count();

        var rs3 = from p in db.StockAuditPGDetail
                  where p.AuditID == AuditID
                  select p;

        TotalItem += rs3.Count();

        var rs4 = from p in db.StockAuditLDDetail
                  where p.AuditID == AuditID
                  select p;

        TotalItem += rs4.Count();
        

        var x = db.StockAudit.SingleOrDefault(p => p.ID == AuditID);
        if (x != null)
        {
            x.StatusFinish = true;
            x.FinishJam = DateTime.Now;
            x.TotalItemActual = TotalItem;
            db.SubmitChanges();

            x.StockAudit_Detail.First().StockAuditDJ.StatusFinish = true;
            x.StockAudit_Detail.First().StockAuditDJ.FinishJam = DateTime.Now;
            x.StockAudit_Detail.First().StockAuditDJ.TotalItemActual = rs.Count();
            db.SubmitChanges();

            x.StockAudit_Detail.First().StockAuditGJ.StatusFinish = true;
            x.StockAudit_Detail.First().StockAuditGJ.FinishJam = DateTime.Now;
            x.StockAudit_Detail.First().StockAuditGJ.TotalItemActual = rs2.Count();
            db.SubmitChanges();

            x.StockAudit_Detail.First().StockAuditPG.StatusFinish = true;
            x.StockAudit_Detail.First().StockAuditPG.FinishJam = DateTime.Now;
            x.StockAudit_Detail.First().StockAuditPG.TotalItemActual = rs3.Count();
            db.SubmitChanges();

            x.StockAudit_Detail.First().StockAuditLD.StatusFinish = true;
            x.StockAudit_Detail.First().StockAuditLD.FinishJam = DateTime.Now;
            x.StockAudit_Detail.First().StockAuditLD.TotalItemActual = rs4.Count();
            db.SubmitChanges();
        }

        Response.Redirect("DJ_LocationType.aspx?d=1");
    }
}