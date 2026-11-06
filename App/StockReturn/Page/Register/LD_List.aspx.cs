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
    protected int ReturnID { get { return App.QueryInt(this, "id"); } }
    protected int ItemID { get { return App.QueryInt(this, "item"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            //Set();

        }
        Set();
    }

    protected void Set()
    {
        var d = db.StockReturnLD.SingleOrDefault(p => p.ID == ReturnID);
        if (d == null)
            Response.Redirect("LD.aspx");
        else
        {
            if (ItemID > 0)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == ItemID);
                if (x != null)
                    Upload.Image(imgpicture, x.ImgPicture);
            }

            var rs = from p in db.StockReturnLD_Product
                     where p.IDForm == d.ID
                     select p;
            rs = rs.OrderByDescending(p => p.ID);

            if (rs != null)
            {
                int index = 1;
                foreach (var r in rs)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    string x = "";

                    x += "<div class=\"btn-group\">";
                    x += "  <a class=\"btn btn-danger\" href=\"LD_Delete.aspx?id=" + r.ID.ToString() + "\">";
                    x += "      <i class=\"icon-trash\"></i> ";
                    x += "      Delete";
                    x += "  </a>";
                    x += "</div>";

                    td = new TableCell();
                    td.Text = x;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Nomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductLD_Stone1B.Stone1B.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalButir).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductLD_Stone1B.TotalCarat).ToString("N3");
                    tr.Cells.Add(td);

                    HiddenField hf = new HiddenField();
                    hf.ID = "hf" + index.ToString();
                    hf.Value = r.IDProduct.ToString();
                    hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                    td = new TableCell();
                    td.Controls.Add(hf);
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    index++;
                }
            }
        }
    }

    protected void Add(object sender, EventArgs e)
    {
        int index = 1;

        TextBox txt = new TextBox();
        CheckBox cb_img = new CheckBox();
        CheckBox cb_bd = new CheckBox();
        string hf = "";
        foreach (TableRow row in tb.Rows)
        {

            try
            {
                hf = ((HiddenField)row.FindControl("hf" + index.ToString())).Value;
                txt = (TextBox)row.FindControl("tb_" + index.ToString());
                cb_img = (CheckBox)row.FindControl("cb_img" + index.ToString());
                cb_bd = (CheckBox)row.FindControl("cb_bd" + index.ToString());
                var d = db.StockReturnLD_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.IDForm == ReturnID);
                if (d != null)
                {

                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
            }
            index++;
        }

        string plu = kw.Text;
        if (plu != "")
        {
            var rt = db.StockReturnLD.SingleOrDefault(p=>p.ID == ReturnID);
            var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockActualLD_Stone1B.TipeLokasi == 0 && p.StockActualLD_Stone1B.IDLokasi == rt.IDWarehouse && p.ReturnProduct == false);
            if (d == null)
            {
                err.InnerText = "PLU Tidak ditemukan.";
                err.Visible = true;
                return;
            }


            else
            {
                Upload.Image(imgpicture, d.ImgPicture);
                err.Visible = false;

                var cekProduct = db.StockReturnLD_Product.FirstOrDefault(a => a.IDForm == ReturnID && a.Nomor == plu);

                if (cekProduct != null)
                {
                    err.InnerText = " Duplikat PLU, Silahkan pilih PLU lain";
                    err.Visible = true;
                    return;
                }

                //2. Validasi cek di dokumen yang aktif tidak boleh ada double PLU yang sama
                var val2 = db.StockReturnLD_Product.SingleOrDefault(p => p.IDForm == ReturnID && p.StockReturnLD.Draft == true && p.Nomor == "" + plu + "");

                if (val2 != null)
                {
                    err.InnerText = " PLU SUDAH TERINPUT , TOLONG CEK LAGI PLU YANG DI INPUT ";
                    err.Visible = true;
                    return;
                }

                //3. Validasi cek di dokumen yang sudah pernah di approve / draft
                var val3 = db.StockReturnLD_Product.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockReturnLD.Draft == false);

                if (val3 != null)
                {
                    var doc = val3.StockReturnLD.NoDokumen.ToString();
                    err.InnerText = " PLU SUDAH PERNAH DI INPUT DI DOKUMEN" + "   " + doc;
                    err.Visible = true;
                    return;
                }

                //var x = db.StockReturnLD_Product.SingleOrDefault(p => p.AuditID == AuditID && p.IDProduct == d.ID);
                //if (x == null)
                //{
                StockReturnLD_Product item = new StockReturnLD_Product();

                item.IDForm = ReturnID;
                item.Nomor = d.Nomor;
                item.IDProduct = d.ID;

                db.StockReturnLD_Product.InsertOnSubmit(item);
                db.SubmitChanges();
                //}
                Response.Redirect("LD_List.aspx?id=" + ReturnID + "&item=" + d.ID.ToString());
            }
        }
    }

    protected void Finish(object sender, EventArgs e)
    {

        var rs = db.StockReturnLD.SingleOrDefault(p=>p.ID == ReturnID);
        if(rs!=null)
        {
            rs.Draft = false;
            db.SubmitChanges();
        }

        Response.Redirect("LD.aspx?d=1");
    }
}