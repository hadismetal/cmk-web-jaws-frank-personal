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
        var d = db.StockReturnDJ.SingleOrDefault(p => p.ID == ReturnID);
        if (d == null)
            Response.Redirect("DJ.aspx");
        else
        {
            if (ItemID > 0)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == ItemID);
                if (x != null)
                    Upload.Image(imgpicture, x.ImgPicture);
            }

            var rs = from p in db.StockReturnDJ_Product
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
                    x += "  <a class=\"btn btn-danger\" href=\"DJ_Delete.aspx?id=" + r.ID.ToString() + "\">";
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
                    td.Text = r.StockProductDJ.ProtoNomor;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.ModalRupiah).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.StoneQty).ToString("N2");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3");
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
                var d = db.StockReturnDJ_Product.SingleOrDefault(p => p.IDProduct == Convert.ToInt32(hf) && p.IDForm == ReturnID);
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
            //1. Validasi PLU cek di stock ada gak
            var rt = db.StockReturnDJ.SingleOrDefault(p=>p.ID == ReturnID);
            var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor == plu && p.StockActualDJ.TipeLokasi == 0 && p.StockActualDJ.IDLokasi == rt.IDWarehouse && p.ReturnProduct == false );
            //hasil validasi 1
            if (d == null)
            {
                err.InnerText = "PLU Tidak ditemukan.";
                err.Visible = true;
            }else
            {
                Upload.Image(imgpicture, d.ImgPicture);
                err.Visible = false;


                //2. Validasi cek di dokumen yang aktif tidak boleh ada double PLU yang sama

                var val2 = db.StockReturnDJ_Product.SingleOrDefault(p => p.IDForm == ReturnID && p.StockReturnDJ.Draft == true && p.Nomor == plu);

                if (val2 != null)
                {
                    err.InnerText = " PLU SUDAH TERINPUT , TOLONG CEK LAGI PLU YANG DI INPUT ";
                    err.Visible = true;
                    return;
                }

                //3. Validasi cek di dokumen yang sudah pernah di approve / draft
                //var val3 = db.StockReturnDJ_Product.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockReturnDJ.Draft == false);
                // var val3 = db.StockReturnDJ_Product.Where(p => p.Nomor == plu && p.StockReturnDJ.Draft == false);

                var val3 = db.StockReturnDJ_Product
                    .Where(p => p.Nomor == plu && p.StockReturnDJ.Draft == false)
                    .ToList();

                if (val3.Count > 0)
                {
                    string txt3 = "";
                    int index3 = 0;

                    foreach (var r3 in val3)
                    {
                        if (r3.StockReturnDJ != null && r3.StockReturnDJ.NoDokumen != null)
                        {
                            txt3 += (index3 > 0)
                                ? (", " + r3.StockReturnDJ.NoDokumen.ToString())
                                : r3.StockReturnDJ.NoDokumen.ToString();
                            index3++;
                        }
                    }

                    if (txt3 != "")
                    {
                        err.InnerText = "PLU SUDAH PERNAH DI INPUT DI DOKUMEN " + txt3;
                        err.Visible = true;
                        return;
                    }
                }

                var cekProduct = db.StockReturnDJ_Product.FirstOrDefault(a => a.IDForm == ReturnID && a.Nomor == plu);

                if (cekProduct != null)
                {
                    err.InnerText = " Duplikat PLU, Silahkan pilih PLU lain";
                    err.Visible = true;
                    return;
                }

                // var val3 = db.StockReturnDJ_Product.Where(p => p.Nomor == plu && p.StockReturnDJ.Draft == false);
                // var val3 = db.StockReturnDJ_Product.SingleOrDefault(p => p.Nomor == "" + plu + "" && p.StockReturnDJ.Draft == false);

                // if (val3 != null)
                // {
                //     string txt3 = "";
                //     int index3 = 0;

                //     foreach(var r3 in val3)
                //     {

                //         txt3 += (index3 > 0) ? (", " + r3.StockReturnDJ.NoDokumen.ToString()) : r3.StockReturnDJ.NoDokumen.ToString();
                //         index3++;
                //     }

                //     //var doc = val3.StockReturnDJ.NoDokumen.ToString();
                //     err.InnerText = " PLU SUDAH PERNAH DI INPUT DI DOKUMEN " + txt3;
                //     err.Visible = true;
                //     return;
                // }

                //var x = db.StockReturnDJ_Product.SingleOrDefault(p => p.AuditID == AuditID && p.IDProduct == d.ID);
                //if (x == null)
                //{
                StockReturnDJ_Product item = new StockReturnDJ_Product();

                item.IDForm = ReturnID;
                item.IDProduct = d.ID;
                item.Nomor = d.Nomor;

                db.StockReturnDJ_Product.InsertOnSubmit(item);
                db.SubmitChanges();
                //}
                Response.Redirect("DJ_List.aspx?id=" + ReturnID + "&item=" + d.ID.ToString());
            }
        }
        else
        {
            err.InnerText = "TOLONG ISI NOMOR PLU NYA";
        }
    }

    protected void Finish(object sender, EventArgs e)
    {

        var rs = db.StockReturnDJ.SingleOrDefault(p=>p.ID == ReturnID);
        if(rs!=null)
        {
            rs.Draft = false;
            db.SubmitChanges();
        }

        Response.Redirect("DJ.aspx?d=1");
    }
}