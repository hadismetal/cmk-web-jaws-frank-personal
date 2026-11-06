using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_Souvenir_Form : System.Web.UI.Page
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

    decimal GT_Qty = 0;
    decimal GT_Harga = 0m;
    decimal GT_Ongkos = 0m;
    decimal GT_OngkosPacking = 0m;
    decimal GT_HPP = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }
        fillForm();
    }

    //Exception
    protected void DataError() { Response.Redirect("Product.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveSouvenir.SingleOrDefault(p => p.ID == myID && !p.Draft && p.Approval == null);
        if (d == null) DataError();
        else
        {
            //Header

            nopo.InnerText = d.NoPO;
            nodo.InnerText = d.NoDO;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            tgl.InnerHtml = Str.Tgl(d.Tgl);

            //Souvenir
            fillSouvenir(d);
        }
    }


    //Souvenir
    protected void fillSouvenir(StockReceiveSouvenir d)
    {
        //SELECT
        var rs = from p in db.StockReceiveSouvenir_Product
                 where p.IDForm == myID
                 orderby p.Souvenir.Nama
                 select p;

        //RESULT
        int index = 1;
        decimal TotalQty = rs.Sum(p => p.Qty.Value);
        decimal TotalHarga = rs.Sum(p => p.Harga.Value);

        foreach (var r in rs)
        {
            decimal Ongkos = 0m;
            decimal HPP = 0m;

            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Souvenir.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.CssClass = "text-center";
            td.Text = TD_Nama(r.Souvenir.Kode);
            tr.Cells.Add(td);

            TextBox rcv = new TextBox();
            App.NumberFormat(rcv);

            rcv.ID = "tb_receive" + index.ToString();
            rcv.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            rcv.Attributes["style"] = "width: 70px;";
            rcv.Text = r.Qty.Value.ToString("N0");

            HiddenField hf_rcv = new HiddenField();
            hf_rcv.ID = "hf_rcv" + index.ToString();
            hf_rcv.Value = r.ID.ToString();
            hf_rcv.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.CssClass = "text-center";
            td.Controls.Add(rcv);
            td.Controls.Add(hf_rcv);
            tr.Cells.Add(td);

            TextBox dmg = new TextBox();
            App.NumberFormat(dmg);

            dmg.ID = "tb_dmg" + index.ToString();
            dmg.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            dmg.Attributes["style"] = "width: 70px;";
            dmg.Text = "0";

            HiddenField hf_dmg = new HiddenField();
            hf_dmg.ID = "hf_dmg" + index.ToString();
            hf_dmg.Value = r.ID.ToString();
            hf_dmg.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.CssClass = "text-center";
            td.Controls.Add(dmg);
            td.Controls.Add(hf_dmg);
            tr.Cells.Add(td);

            TextBox lst = new TextBox();
            App.NumberFormat(lst);

            lst.ID = "tb_lst" + index.ToString();
            lst.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            lst.Attributes["style"] = "width: 70px;";
            lst.Text = "0";

            HiddenField hf_lst = new HiddenField();
            hf_lst.ID = "hf_lst" + index.ToString();
            hf_lst.Value = r.ID.ToString();
            hf_lst.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            td = new TableCell();
            td.CssClass = "text-center";
            td.Controls.Add(lst);
            td.Controls.Add(hf_lst);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Qty);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            GT_Qty += r.Qty.Value;
            GT_Harga += r.Harga.Value;

            tb.Rows.Add(tr);
            index++;
        }

        //Grand-Total
        Sub_Total(GT_Qty, GT_Harga, GT_Ongkos, GT_OngkosPacking, GT_HPP, true);
    }

    protected void Sub_Total(decimal TotalQty, decimal TotalHarga, decimal Ongkos, decimal OngkosPacking, decimal HPP, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 6;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalQty) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalHarga) + "</p>";
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    //Display Column
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }
    protected string TD_Info(decimal? Value)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number0(Value) + "</p>";

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveSouvenir.SingleOrDefault(p => p.ID == myID && !p.Draft && p.ApprovalTgl == null);
        if (d == null) DataError();
        else
        {

            d.StatusReceive = true;
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;

            //db.SubmitChanges();

            var rs = from p in db.StockReceiveSouvenir_Product
                     where p.IDForm == myID
                     orderby p.Souvenir.Nama
                     select p;

            //RESULT

            decimal TotalQty = rs.Sum(p => p.Qty.Value);
            decimal TotalHarga = rs.Sum(p => p.Harga.Value);

            int index = 1;
            string val = "";
            TextBox rcv = new TextBox();
            string hf_rcv = "";
            TextBox dmg = new TextBox();
            string hf_dmg = "";
            TextBox lst = new TextBox();
            string hf_lst = "";
            foreach (TableRow row in tb.Rows)
            {
                
                try
                {
                    hf_rcv = ((HiddenField)row.FindControl("hf_rcv" + index.ToString())).Value;
                    rcv = (TextBox)row.FindControl("tb_receive" + index.ToString());
                    hf_dmg = ((HiddenField)row.FindControl("hf_dmg" + index.ToString())).Value;
                    dmg = (TextBox)row.FindControl("tb_dmg" + index.ToString());
                    hf_lst = ((HiddenField)row.FindControl("hf_lst" + index.ToString())).Value;
                    lst = (TextBox)row.FindControl("tb_lst" + index.ToString());

                    var k = db.StockReceiveSouvenir_Product.SingleOrDefault(p => p.ID == Convert.ToInt32(hf_rcv));
                    if (k == null) App.DataNull(this);
                    {

                        decimal RCV = 0m;
                        decimal DMG = 0m;
                        decimal LST = 0m;
                        try
                        {
                            RCV = Convert.ToDecimal(rcv.Text);
                        }
                        catch (Exception ex) { }
                        try
                        {
                            DMG = Convert.ToDecimal(dmg.Text);
                        }
                        catch (Exception ex) { }
                        try
                        {
                            LST = Convert.ToDecimal(lst.Text);
                        }
                        catch (Exception ex) { }

                        k.Received = RCV;
                        k.Damaged = DMG;
                        k.NeverArrived = LST;
                        db.SubmitChanges();


                            var st = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == k.IDProduct && p.TipeLokasi == 0 && p.IDLokasi == k.StockReceiveSouvenir.IDWarehouse);

                            if (st == null)
                            {
                                StockActualSouvenir stock = new StockActualSouvenir();
                                stock.IDProduct = k.IDProduct;
                                //stock.NamaLokasi = x.P;
                                //stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
                                stock.TipeLokasi = 0;
                                stock.IDLokasi = k.StockReceiveSouvenir.IDWarehouse;
                                stock.Substorage = 1;
                                stock.Keterangan = d.Keterangan;
                                stock.Tgl = d.Tgl.Value;
                                stock.Nomor = k.Souvenir.Kode;
                                stock.InHand = stock.InHand + Convert.ToDecimal(rcv.Text);
                                //z.Receive = q.Receive - (Convert.ToDecimal(rcv.Text) + Convert.ToDecimal(dmg.Text) + Convert.ToDecimal(lst.Text));
                                stock.Damaged = Convert.ToDecimal(dmg.Text);
                                stock.NeverArrived = Convert.ToDecimal(lst.Text);
                                stock.Operator = d.Operator;
                                stock.OperatorTgl = d.OperatorTgl;
                                db.StockActualSouvenir.InsertOnSubmit(stock);
                                db.SubmitChanges();
                            }
                            else
                            {
                                
                                st.InHand = st.InHand + Convert.ToDecimal(rcv.Text);
                                //z.Receive = q.Receive - (Convert.ToDecimal(rcv.Text) + Convert.ToDecimal(dmg.Text) + Convert.ToDecimal(lst.Text));
                                st.Damaged = Convert.ToDecimal(dmg.Text);
                                st.NeverArrived = Convert.ToDecimal(lst.Text);
                                db.SubmitChanges();
                            }
                    }
                }
                catch (Exception ex)
                {
                }
                index++;
            }

            //Redirect
            //Response.Redirect("Souvenir.aspx?d=1");
            App.ReloadClose(this);
        }
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
    }
}