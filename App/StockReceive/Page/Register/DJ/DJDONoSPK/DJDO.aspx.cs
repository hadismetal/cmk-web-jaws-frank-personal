using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;

public partial class Page_Register_DJDO : System.Web.UI.Page
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }
    protected string myNODO { get { return Request.QueryString["nodo"]; } }
    protected string myKW { get { return Request.QueryString["kw"]; } }
    protected string mySTS { get { return Request.QueryString["sts"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw.Focus(); //Keyword

            //Generate
            fillFilter();
            //if (App.Refresh(this)) fillTable(sender, e);
            fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJDO.aspx"); }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        search_do();
    }
    protected void retakestonedata_Click(object sender, EventArgs e)
    {
        search_do(1);
    }
    protected void retakestonedata1_Click(object sender, EventArgs e)
    {
        search_do(3);
    }
    protected void search_do(int retakedatastone = 0)
    {
        string keyword_nomor_do = string.IsNullOrEmpty(kw.Text.Trim()) ? myKW : kw.Text.Trim();
        string href = App.RefreshURL(this);

        if (keyword_nomor_do != "")
        {
            //cek dulu jika ada di table t_brj jika tidak ada maka baru lakukan tarik data dari pabrik
            var brj = cs.T_brj_receive.Where(p => p.NoDOBRJ == keyword_nomor_do);

            //Customize-Qstr
            href += "&kw=" + keyword_nomor_do;

            //lit_test_msg.Text += "<br />____________refreshPage_" + brj.Count().ToString();

            try
            {
                if (brj.Count() == 0 || retakedatastone == 1 || retakedatastone == 3)
                {
                    string SPname = "sp_getbrjpabrik";
                    if (retakedatastone == 1)
                    {
                        Db.Execute("EXEC " + SPname + " "
                            + "@nodo = N'" + keyword_nomor_do + "'"
                            + ", " + "@flag_all = 0"
                            + ", " + "@retakedatastone = " + retakedatastone.ToString() // retakedata
                        , Db.Cnn_CS);
                    }
                    if (brj.Count() == 0 || retakedatastone == 3)
                    {
                        Db.Execute("EXEC sp_getdobrj_myapps '" + keyword_nomor_do + "' ", Db.Cnn_CS);
                    }
                    var brji = cs.T_brj_invalid.Where(p => p.NoDoBRJ == keyword_nomor_do);
                    var brj2 = cs.T_brj_receive.Where(p => p.NoDOBRJ == keyword_nomor_do);
                    if (brji.Count() > 0)
                    {
                        href += "&sts=" + "7";
                    }
                    else if (brj2.Count() == 0)
                    {
                        if (!cs.T_brj.Any(p => p.NoDOBRJ == keyword_nomor_do))
                        {
                            href += "&sts=" + "4";
                        }
                        else
                        {
                            Db.Execute(@"insert into [DB_PROD_MYAPPS].[dbcmk].dbo.t_brj_receive (NoDOBRJ, GrossWeight, StoneQty, StoneCarat, BRJQty)
                                        select a.NoDOBRJ, SUM(a.GrossWeight) , SUM(a.StoneQty) , SUM(a.StoneCarat) , count(a.NoDOBRJ)
                                        from t_brj a
                                        where a.NoDOBRJ = '" + keyword_nomor_do + "' group by a.NoDOBRJ", Db.Cnn_CS);
                        }
                    }
                    else
                    {
                        try
                        {

                            if (retakedatastone == 1)
                            {
                                var brj3 = cs.T_brj.Where(p => p.NoDOBRJ == keyword_nomor_do).Sum(p => p.StoneQty);
                                var brj4 = cs.T_brj_stone.Where(p => p.NoDOBRJ == keyword_nomor_do).Sum(p => p.Butir);

                                if (brj3 == brj4) { href += "&sts=" + "5"; }
                                else { href += "&sts=" + "3"; }

                            }
                            else
                            {
                                href += "&sts=" + "3";

                            }
                        }
                        catch (Exception ex)
                        {
                            href += "&sts=" + "6";
                            show_process(6);
                            //lit_test_msg.Text += "<br />____________refreshPage_" + "err".ToString();
                        }
                    }
                }
                else
                {
                    href += "&sts=" + "1";
                    show_process(1);
                }
            }
            catch (Exception ex)
            {
                href += "&sts=" + "2";
                show_process(2);
                //lit_test_msg.Text += "<br />____________refreshPage_" + "err".ToString();
            }


        }
        else
        {
            href += "&sts=" + "0";
        }

        Response.Redirect(href);

    }
    protected void submit_Click(object sender, EventArgs e)
    {
        int warehouse = Convert.ToInt32(ddl_warehouse.SelectedValue);

        if (warehouse <= 0)
        {
            Response.Redirect("DJDO.aspx?nodo=" + myNODO + "&err=submit");
        }
        else
        {
            Response.Redirect("DJDO_DataSave.aspx?nodo=" + myNODO + "&cmd=2&warehouseid=" + warehouse.ToString());
        }
    }
    //submit_SPK_Click
    protected void submit_SPK_Click(object sender, EventArgs e)
    {

        var rs = cs.T_brj_receive.SingleOrDefault(p => p.NoDOBRJ == myNODO);

        rs.StatusDO = 3;
        cs.SubmitChanges();

        if (myNODO.Substring(0, 2) == "IT")
        {
            Response.Redirect("DJDO_DataBRJSave_Wholesale.aspx?nodo=" + myNODO + "");
        }
        else Response.Redirect("DJDO.aspx?nodo=" + myNODO + "&d=1");
    }
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DJDO_DataSave.aspx?nodo=" + myNODO + "&cmd=0");
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = myKW;
        }
    }

    //fillStoneTable
    protected void fillStoneTable(object sender, EventArgs e)
    {
        var rsDOStone = from p in cs.T_brj_stone
                        where p.NoDOBRJ == myNODO
                            && (p.GIA != null && p.GIA != "")
                            && (!p.GIA.ToLower().Contains("x") && !p.GIA.ToLower().Contains("."))
                        orderby p.Carat descending
                        select p
                        ;

        if (rsDOStone.Count() == 0) { App.TableNoResult(tb_stone); }
        else
        {
            int index = 1;

            TableRow tr = new TableRow();
            TableCell td;

            foreach (var r in rsDOStone)
            {
                if (!Response.IsClientConnected) break;

                tr = new TableRow();

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Parcel;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Number3(r.Carat);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.GIA.Trim();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoKodeBarang;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoValidasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoKodeBagian;
                tr.Cells.Add(td);

                tb_stone.Rows.Add(tr);
            }

        }

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        lit_message_status.Text = "";
        div_panel_info.Visible = false;

        if (mySTS == "0")
        {
            show_process(0);
            lit_message_status.Text = "Nomor DO harus diinput !";
        }
        else if (mySTS == "1") { show_process(1); }
        else if (mySTS == "2")
        {
            show_process(2);
            lit_message_status.Text = "Gagal mendapatkan data DO dengan nomor <b>" + myKW + "</b>, proses koneksi ke Database Factory gagal.";
        }
        else if (mySTS == "3") { show_process(3); }
        else if (mySTS == "4")
        {
            show_process(4);
            lit_message_status.Text = "Gagal mendapatkan data DO dengan nomor <b>" + myKW + "</b>, proses koneksi ke Database Factory berhasil namun data DO tidak ditemukan.";
        }
        else if (mySTS == "5") { show_process(5); }
        else if (mySTS == "6")
        {
            show_process(6);
            lit_message_status.Text = "Gagal mendapatkan data stone dengan DO nomor <b>" + myKW + "</b>, proses koneksi ke Database Factory berhasil namun data DO stone tidak ditemukan.";
        }
        else if (mySTS == "7")
        {
            var brji = cs.T_brj_invalid.Where(p => p.NoDoBRJ == myKW);

            if (brji.Count() > 0)
            {
                string err_msg = "";

                err_msg += "<div id='content_legacy_ERR'>";
                err_msg += "<i class='icon-warning-sign icon-5x pull-left text-red'></i>";
                err_msg += "<h4 style='color:red;'>Error data BRJ</h4>";

                err_msg += "<p class='text-red'>";
                foreach (var r in brji)
                {
                    err_msg += "<b>" + r.Keterangan + "</b>";
                }

                err_msg += "</p>";
                err_msg += "<div class='clearfix'></div>";
                err_msg += "</div>";
                lit_invalid_msg.Text = err_msg;

                show_process(4);
                lit_message_status.Text = "Gagal mendapatkan data DO dengan nomor <b>" + myKW + "</b>, proses koneksi ke Database Factory berhasil namun salah satu data BRJ ditemukan error.";
            }


        }

        bool statusView = false;
        div_warehouse_info.Visible = false;

        if (myNODO != null)
        {
            decimal Total_GrossWeight = 0m;
            decimal Total_StoneQty = 0m;
            decimal Total_StoneCarat = 0m;

            decimal TotalSPK_GrossWeight = 0m;
            decimal TotalSPK_StoneQty = 0m;
            decimal TotalSPK_StoneCarat = 0m;

            div_warehouse1.Visible = true;
            div_warehouse2.Visible = true;
            div_search1.Visible = false;
            div_search2.Visible = false;
            div_info.Visible = true;

            tb2.Visible = true;

            DLC.WarehouseList2(ddl_warehouse);

            var rsDO = cs.T_brj_receive.SingleOrDefault(p => p.NoDOBRJ == myNODO && (p.StatusDO == 1 || p.StatusDO == 2 || p.StatusDO == 3));
            //rsDO.StatusDO = 3;
            //cs.SubmitChanges();


            if (rsDO != null)
            {
                do_number.InnerText = myNODO;
                do_brjqty.InnerText = Str.Number0(rsDO.BRJQty);
                do_grossweight.InnerText = Str.Number2(rsDO.GrossWeight);
                do_stoneqty.InnerText = Str.Number0(rsDO.StoneQty);
                do_stonecarat.InnerText = Str.Number3(rsDO.StoneCarat);

                var rs = from p in cs.T_brj
                         where p.NoDOBRJ.Equals(myNODO)
                         orderby p.NoKodeBarang ascending, p.NoValidasi ascending
                         select p;


                var rsG = (from c in rs
                           where c.NoSPK != null
                           group c by c.NoSPK into grp
                           where grp.Count() > 1
                           select grp.Key).ToList()
                           ;

                fillStoneTable(sender, e);

                //foreach (string data in rsG)
                //{
                //    lit_test_msg.Text += data + "<br />";
                //}


                //rs = rs.Where(p => p.NoValidasi == "1814131RT");

                //NO-RESULT
                if (rs != null)
                {
                    if (rs.Count() == 0) { App.TableNoResult(tb2); }
                    else
                    {
                        // statusDO = 1
                        if (rsDO.StatusDO == 1)
                        {
                            // kondisi jika BELUM dipilih warehouse by comp. Brand

                            div1.Visible = true;
                            tb2.Visible = true;

                            div2.Visible = false;
                            tb3.Visible = false;

                            //RESULT
                            int prev = 0;
                            int index = 1;

                            TableRow tr = new TableRow();
                            TableCell td;

                            foreach (var r in rs)
                            {
                                if (!Response.IsClientConnected) break;

                                tr = new TableRow();

                                td = new TableCell();
                                td.Text = index++.ToString() + ".";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = r.NoValidasi;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = r.NoKodeBarang;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = r.NoKodeBagian;
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                Total_GrossWeight += r.GrossWeight;
                                td.Text = Str.Number2(r.GrossWeight);
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                Total_StoneQty += r.StoneQty;
                                td.Text = Str.Number0(r.StoneQty);
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                Total_StoneCarat += r.StoneCarat;
                                td.Text = Str.Number3(r.StoneCarat);
                                td.Style.Add("text-align", "right");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                Total_StoneCarat += r.StoneCarat;
                                td.Text = r.NoSPK != null ? r.NoSPK : "";
                                td.Style.Add("text-align", "center");
                                td.Style.Add("vertical-align", "middle");
                                tr.Cells.Add(td);

                                tb2.Rows.Add(tr);
                            }

                            tr = new TableRow();
                            tr.Style.Add("font-weight", "bold");

                            td = new TableCell();
                            td.ColumnSpan = 4;
                            td.Text = "Total ";
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number2(Total_GrossWeight);
                            td.Style.Add("text-align", "right");
                            td.Style.Add("vertical-align", "middle");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number0(Total_StoneQty);
                            td.Style.Add("text-align", "right");
                            td.Style.Add("vertical-align", "middle");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            td.Text = Str.Number3(Total_StoneCarat);
                            td.Style.Add("text-align", "right");
                            td.Style.Add("vertical-align", "middle");
                            tr.Cells.Add(td);

                            td = new TableCell();
                            tr.Cells.Add(td);

                            tb2.Rows.Add(tr);
                        }
                        // statusDO = 2
                        else if (rsDO.StatusDO == 2)
                        {
                            // kondisi jika SUDAH dipilih warehouse by comp. Brand
                            if (status_view_DO_by_SBU(rsDO))
                            {
                                div1.Visible = false;
                                tb2.Visible = false;

                                div2.Visible = true;
                                tb3.Visible = true;

                                do_warehouse.InnerText = rsDO.NamaWarehouse + " (" + rsDO.NamaCompanyBrand + ")";
                                div_warehouse_info.Visible = true;

                                // kondisi jika semua SPK sudah terinput
                                if (rs.Count(p => p.NoSPK == null || p.IDSPK == null) == 0)
                                {
                                    btn_submit_SPK.Disabled = false;
                                    lit_msg_SPK.Text = "<div class='alert alert-warning'>Pastikan semua SPK sudah terinput dengan benar. Dengan menekan tombol Submit, anda menyetujui tidak akan ada lagi perubahan SPK terhadap Kode Barang.</div>";
                                }
                                else
                                {
                                    btn_submit_SPK.Disabled = false;
                                    //lit_msg_SPK.Text = "<div class='alert alert-danger'>Isilah semua SPK terhadap masing-masing Kode Barang untuk melanjutkan proses penerimaan.</div>";

                                }

                                //RESULT
                                int prev = 0;
                                int index = 1;

                                TableRow tr = new TableRow();
                                TableCell td;

                                foreach (var r in rs)
                                {
                                    if (!Response.IsClientConnected) break;

                                    tr = new TableRow();

                                    td = new TableCell();
                                    td.Text = "<p>" + index++.ToString() + ".</p>";
                                    td.Style.Add("text-align", "center");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoKodeBarang;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoValidasi;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoKodeBagian;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_GrossWeight += r.GrossWeight;
                                    td.Text = Str.Number2(r.GrossWeight);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_StoneQty += r.StoneQty;
                                    td.Text = Str.Number0(r.StoneQty);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_StoneCarat += r.StoneCarat;
                                    td.Text = Str.Number3(r.StoneCarat);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = TD_HyperLink_BRJ_SPK(r, rsG);
                                    tr.Cells.Add(td);

                                    var rsspk = db.ProductOrder.SingleOrDefault(p => p.ID == r.IDSPK);

                                    td = new TableCell();
                                    TotalSPK_GrossWeight += (rsspk != null) ? rsspk.ProductOrder_CharProduct.GrossWeight : 0m;
                                    td.Text = (rsspk != null) ? Str.Number2(rsspk.ProductOrder_CharProduct.GrossWeight) : "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    TotalSPK_StoneQty += (rsspk != null) ? (!rsspk.ProductOrder_CharProduct.StoneQty.HasValue ? 0m : rsspk.ProductOrder_CharProduct.StoneQty.Value) : 0m;
                                    td.Text = (rsspk != null) ? (!rsspk.ProductOrder_CharProduct.StoneQty.HasValue ? "0" : Str.Number0(rsspk.ProductOrder_CharProduct.StoneQty.Value)) : "0";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    TotalSPK_StoneCarat += (rsspk != null) ? rsspk.ProductOrder_CharProduct.StoneCarat : 0m;
                                    td.Text = (rsspk != null) ? Str.Number3(rsspk.ProductOrder_CharProduct.StoneCarat) : "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = "";
                                    tr.Cells.Add(td);


                                    tb3.Rows.Add(tr);
                                }

                                tr = new TableRow();
                                tr.Style.Add("font-weight", "bold");

                                td = new TableCell();
                                td.ColumnSpan = 4;
                                td.Text = "Total ";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(Total_GrossWeight);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(Total_StoneQty);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number3(Total_StoneCarat);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(TotalSPK_GrossWeight);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(TotalSPK_StoneQty);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number3(TotalSPK_StoneCarat);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "";
                                td.ColumnSpan = 3;
                                tr.Cells.Add(td);

                                tb3.Rows.Add(tr);
                            }
                            else
                            {
                                //// pesan DO tidak valid terhadap id brand company
                                tb2.Visible = false;
                                div1.Visible = false;
                                div2.Visible = false;
                                btn_cancel_invalid.Visible = true;
                                lit_data.Text = "<span style='color:red;'>Dokumen DO tidak valid berdasarkan Brand Company. Silahkan check kembali Nomor DO anda.</span>";
                            }
                        }
                        // statusDO = 3
                        else if (rsDO.StatusDO == 3)
                        {
                            // kondisi jika SUDAH dipilih warehouse by comp. Brand dan sudah setuju semua SPK
                            if (status_view_DO_by_SBU(rsDO))
                            {
                                div1.Visible = false;
                                tb2.Visible = false;

                                div2.Visible = true;
                                tb3.Visible = true;

                                do_warehouse.InnerText = rsDO.NamaWarehouse + " (" + rsDO.NamaCompanyBrand + ")";
                                div_warehouse_info.Visible = true;

                                btn_submit_SPK.Visible = false;

                                //RESULT
                                int prev = 0;
                                int index = 1;

                                TableRow tr = new TableRow();
                                TableCell td;

                                foreach (var r in rs)
                                {
                                    if (!Response.IsClientConnected) break;

                                    tr = new TableRow();

                                    td = new TableCell();
                                    td.Text = "<p>" + index++.ToString() + ".</p>";
                                    td.Style.Add("text-align", "center");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoKodeBarang;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoValidasi;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    td.Text = r.NoKodeBagian;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_GrossWeight += r.GrossWeight;
                                    td.Text = Str.Number2(r.GrossWeight);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_StoneQty += r.StoneQty;
                                    td.Text = Str.Number0(r.StoneQty);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    Total_StoneCarat += r.StoneCarat;
                                    td.Text = Str.Number3(r.StoneCarat);
                                    tr.Cells.Add(td);

                                    thc_spk.Width = 110;

                                    td = new TableCell();
                                    td.Text = r.NoSPK;
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    var rsspk = db.ProductOrder.SingleOrDefault(p => p.ID == r.IDSPK);

                                    td = new TableCell();
                                    TotalSPK_GrossWeight += (rsspk != null) ? rsspk.ProductOrder_CharProduct.GrossWeight : 0m;
                                    td.Text = (rsspk != null) ? Str.Number2(rsspk.ProductOrder_CharProduct.GrossWeight) : "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    TotalSPK_StoneQty += (rsspk != null) ? (!rsspk.ProductOrder_CharProduct.StoneQty.HasValue ? 0m : rsspk.ProductOrder_CharProduct.StoneQty.Value) : 0m;
                                    td.Text = (rsspk != null) ? (!rsspk.ProductOrder_CharProduct.StoneQty.HasValue ? "0" : Str.Number0(rsspk.ProductOrder_CharProduct.StoneQty.Value)) : "0";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    TotalSPK_StoneCarat += (rsspk != null) ? rsspk.ProductOrder_CharProduct.StoneCarat : 0m;
                                    td.Text = (rsspk != null) ? Str.Number3(rsspk.ProductOrder_CharProduct.StoneCarat) : "";
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    if (rsDO.StatusDO == 3) { statusView = true; }
                                    td.Text = TD_HyperLink_BRJ_Detail(r, statusView);
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    if (rsDO.StatusDO == 3 && r.IDReceive != null)
                                    {
                                        try
                                        {
                                            int IDrec = r.IDReceive.Value;
                                            var receive = db.StockReceiveDJ.Single(p => p.ID == IDrec);
                                            td.Text = receive.Operator + "<br />" + Str.FullDate(receive.OperatorTgl);
                                            td.Style.Add("text-align", "left");
                                        }
                                        catch (Exception ex)
                                        {
                                            td.Text = "  <i class=\"icon-warning-sign text-red\"></i> ";
                                            td.Style.Add("text-align", "center");
                                        }
                                    }
                                    else
                                    {
                                        td.Text = "";
                                        td.Style.Add("text-align", "left");
                                    }
                                    tr.Cells.Add(td);

                                    td = new TableCell();
                                    if (rsDO.StatusDO == 3 && r.IDReceive != null && r.NoReceive != null)
                                    {
                                        int IDrec = r.IDReceive.Value;
                                        var receive = db.StockReceiveDJ.Single(p => p.ID == IDrec);
                                        td.Text = receive.Nomor + "<br />" + receive.Approval + "<br />" + Str.FullDate(receive.ApprovalTgl);
                                    }
                                    else
                                    {
                                        td.Text = "";
                                    }
                                    td.Style.Add("text-align", "left");
                                    tr.Cells.Add(td);

                                    tb3.Rows.Add(tr);
                                }

                                tr = new TableRow();
                                tr.Style.Add("font-weight", "bold");

                                td = new TableCell();
                                td.ColumnSpan = 4;
                                td.Text = "Total ";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(Total_GrossWeight);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(Total_StoneQty);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number3(Total_StoneCarat);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number2(TotalSPK_GrossWeight);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number0(TotalSPK_StoneQty);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Str.Number3(TotalSPK_StoneCarat);
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "";
                                td.ColumnSpan = 3;
                                tr.Cells.Add(td);

                                tb3.Rows.Add(tr);
                            }
                            else
                            {
                                //// pesan DO tidak valid terhadap id brand company
                                tb2.Visible = false;
                                div1.Visible = false;
                                div2.Visible = false;
                                btn_cancel_invalid.Visible = true;
                                lit_data.Text = "<span style='color:red;'>Dokumen DO tidak valid berdasarkan Brand Company. Silahkan check kembali Nomor DO anda.</span>";
                            }
                        }
                    }
                }

                // antisipasi nomor SPK yang terinput multiple dalam 1 dokumen yg sama
                if (rsG.Count() > 0 && rsDO.StatusDO == 2)
                {
                    //btn_submit_SPK.Disabled = true; //// sementara dibuka karena ada problem untuk wedding ring aureola dibelah secara data penerimaan
                    //lit_msg_SPK_multiple.Text = "<div class='alert alert-warning'>Terdapat inputan multiple SPK. Silahkan check kembali inputan SPK yang sudah terinput.</div><br /><br />"; 
                }

            }
        }
        else
        {
            // kondisi jika masih search nomor do
            div_warehouse1.Visible = false;
            div_warehouse2.Visible = false;
            div_search1.Visible = true;
            div_search2.Visible = true;

            tb_stone.Visible = false;
            tb2.Visible = false;
            row2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
            div_info.Visible = false;

            if (myKW != null)
            {
                //Db.Execute(" exec sp_getbrjpabrik '" + myKW + "'", Db.Cnn_CS);

                //check if not exist then get data from manufacture
                bool hasData = false;

                var rsData = from p in cs.T_brj_receive
                             where p.NoDOBRJ.Contains(myKW)
                             select p;

                //NO-RESULT
                if (rsData != null)
                {
                    hasData = true;
                }

                if (!hasData)
                {
                    //SELECT
                    try
                    {
                        Db.Execute(" exec sp_getbrjpabrik '" + myKW + "'", Db.Cnn_CS);
                    }
                    catch (Exception ex) { }
                }

                var rs = from p in cs.T_brj_receive
                         where p.NoDOBRJ.Contains(myKW)
                         select p;

                var r = rs.FirstOrDefault();

                //NO-RESULT
                if (r != null)
                {
                    string xStatus = "";

                    string xinfo_left = "";
                    string xinfo_right = "";
                    string xinfo_stone_left = "";
                    string xinfo_stone_right = "";

                    string left_1_label = "";
                    string left_2_label = "";
                    string left_3_label = "";
                    string left_4_label = "";

                    string right_1_label = "";
                    string right_2_label = "";
                    string right_3_label = "";
                    string right_4_label = "";

                    string left_1_value = "";
                    string left_2_value = "";
                    string left_3_value = "";
                    string left_4_value = "";

                    string right_1_value = "";
                    string right_2_value = "";
                    string right_3_value = "";
                    string right_4_value = "";

                    spn_info_head_1_label.InnerText = "DO Number";
                    spn_info_head_2_label.InnerText = "Status";
                    spn_info_head_3_label.InnerText = "Stone Status";

                    lit_info_link.Text = TD_HyperLink(r);

                    // status
                    xStatus = "Ready";
                    if (r.StatusDO == 3) { xStatus = "Used (SPK Finished)"; }
                    else if (r.StatusDO == 2) { xStatus = "Used (SPK Listing)"; }
                    else if (r.StatusDO == 1) { xStatus = "Warehouse Selection"; }

                    spn_info_head_1_value.InnerText = r.NoDOBRJ;
                    spn_info_head_2_value.InnerText = xStatus;

                    left_1_label = "BRJ Qty"; left_1_value = Str.Number0(r.BRJQty);
                    left_2_label = "Gross Weight"; left_2_value = Str.Number2(r.GrossWeight);
                    left_3_label = "Stone Qty"; left_3_value = Str.Number0(r.StoneQty);
                    left_4_label = "Stone Carat"; left_4_value = Str.Number3(r.StoneCarat);

                    right_1_label = "Operator"; right_1_value = r.Operator;
                    right_2_label = "Operator Tgl"; right_2_value = Str.FullDateJam(r.OperatorTgl);
                    right_3_label = "SBU"; right_3_value = r.NamaCompanyBrand;
                    right_4_label = "Warehouse"; right_4_value = r.NamaWarehouse;

                    xinfo_left += "<table>";
                    if (left_1_label != "") { xinfo_left += "<tr><td style='vertical-align:top;'>" + left_1_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + left_1_value + "</td></tr>"; }
                    if (left_2_label != "") { xinfo_left += "<tr><td style='vertical-align:top;'>" + left_2_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + left_2_value + "</td></tr>"; }
                    if (left_3_label != "") { xinfo_left += "<tr><td style='vertical-align:top;'>" + left_3_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + left_3_value + "</td></tr>"; }
                    if (left_4_label != "") { xinfo_left += "<tr><td style='vertical-align:top;'>" + left_4_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + left_4_value + "</td></tr>"; }
                    xinfo_left += "</table>";

                    xinfo_right += "<table>";
                    if (right_1_label != "") { xinfo_right += "<tr><td style='vertical-align:top;'>" + right_1_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + right_1_value + "</td></tr>"; }
                    if (right_2_label != "") { xinfo_right += "<tr><td style='vertical-align:top;'>" + right_2_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + right_2_value + "</td></tr>"; }
                    if (right_3_label != "") { xinfo_right += "<tr><td style='vertical-align:top;'>" + right_3_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + right_3_value + "</td></tr>"; }
                    if (right_4_label != "") { xinfo_right += "<tr><td style='vertical-align:top;'>" + right_4_label + "</td><td style='vertical-align:top;'>&nbsp;:&nbsp;</td><td style='vertical-align:top;'>" + right_4_value + "</td></tr>"; }
                    xinfo_right += "</table>";

                    lit_info_left.Text = xinfo_left;
                    lit_info_right.Text = xinfo_right;

                    xinfo_stone_left += "<table>";
                    xinfo_stone_right += "<table>";

                    string spname = "sp_report_do_stone_missing";
                    string query = " exec " + spname
                                 + " '" + myKW + "'"
                                 ;
                    try
                    {
                        DataTable dt = new DataTable();
                        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

                        if (dt.Rows.Count > 0)
                        {
                            int qty_ok = 0;
                            int qty_missing = 0;

                            int jml = dt.Rows.Count;
                            int idx = 1;

                            foreach (DataRow dtRow in dt.Rows)
                            {
                                if (dtRow["Result"].ToString() == "1") { qty_ok++; } else { qty_missing++; }

                                if ((jml / 2) + 1 > (idx - 1))
                                {
                                    xinfo_stone_left += "<tr>";
                                    xinfo_stone_left += "<td style='vertical-align:top; text-align:center;'>" + idx.ToString() + ". </td>";
                                    xinfo_stone_left += "<td style='vertical-align:top;'>" + dtRow["NoKodeBarang"].ToString() + "</td>";
                                    xinfo_stone_left += "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td>";
                                    xinfo_stone_left += "<td style='vertical-align:top;'>" + (dtRow["Result"].ToString() == "1" ? "<span style='color:green;'>ok</span>" : "<span style='color:red;'>missing</span>") + "</td>";
                                    xinfo_stone_left += "</tr>";
                                }
                                else
                                {
                                    xinfo_stone_right += "<tr>";
                                    xinfo_stone_right += "<td style='vertical-align:top; text-align:center;'>" + idx.ToString() + ". </td>";
                                    xinfo_stone_right += "<td style='vertical-align:top;'>" + dtRow["NoKodeBarang"].ToString() + "</td>";
                                    xinfo_stone_right += "<td style='vertical-align:top;'>&nbsp;:&nbsp;</td>";
                                    xinfo_stone_right += "<td style='vertical-align:top;'>" + (dtRow["Result"].ToString() == "1" ? "<span style='color:green;'>ok</span>" : "<span style='color:red;'>missing</span>") + "</td>";
                                    xinfo_stone_right += "</tr>";
                                }
                                idx++;
                            }

                            bool statusDisabledretake = true;

                            if (qty_missing > 0)
                            {
                                statusDisabledretake = false;
                                spn_info_head_3_value.InnerText = qty_missing + " missing";
                                spn_info_head_3_value.Style.Add("color", "red");
                            }
                            else
                            {
                                statusDisabledretake = true;
                                spn_info_head_3_value.InnerText = "OK";
                                spn_info_head_3_value.Style.Add("color", "green");
                            }

                            div_panel_info.Visible = true;
                            div_panel_info.Attributes.Clear();

                            if (status_view_DO_by_SBU(r))
                            {
                                statusDisabledretake = false;
                                div_panel_info.Attributes.Add("class", "alert alert-info");
                            }
                            else
                            {
                                statusDisabledretake = true;
                                div_panel_info.Attributes.Add("class", "alert alert-danger");
                            }


                            btn_retake.Disabled = statusDisabledretake;
                        }
                    }
                    catch (Exception ex) { }

                    xinfo_stone_left += "</table>";
                    xinfo_stone_right += "</table>";

                    lit_info_stone_left.Text = xinfo_stone_left;
                    lit_info_stone_right.Text = xinfo_stone_right;
                }
                else
                {
                    string txt_msg = "DO Number <b>" + myKW + "</b> tidak dapat ditemukan. Silahkan check nomor DO kembali.";

                    lit_msg_search_do.Text = "<div class='alert alert-warning' role='alert'>" + txt_msg + "</div>";
                }

            }
        }
    }

    protected string TD_HyperLink(T_brj_receive r)
    {
        string x = string.Empty;

        if (status_view_DO_by_SBU(r))
        {
            x += "<a class=\"btn btn-info\" href=\"DJDO_DataSave.aspx?nodo=" + r.NoDOBRJ + "&cmd=1\">";
            x += "  <i class=\"icon-ok\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else
        {
            x += "<a class=\"btn btn-danger disabled\">";
            x += "  <i class=\"icon-remove\"></i> ";
            x += "  ";
            x += "</a>";
        }
        return x;
    }

    protected string TD_HyperLink_BRJ_SPK(T_brj r, List<string> q)
    {
        string x = string.Empty;

        //x += "<a class=\"btn btn-info\" href=\"DJDO_InsertKodeKaret.aspx?kdbrg=" + r.NoKodeBarang + "\">";
        //x += "  <i class=\"icon-ok\"></i> ";
        //x += "  ";
        //x += "</a>";

        string x2 = "";
        if (q.Contains(r.NoSPK)) { x2 = "color:red;"; }

        x += "<div class='input-group'>";
        x += "    <div class='input-group-btn'>";
        x += "	    <a href='DJDO_SPKList.aspx?nodo=" + myNODO + "&kdbrg=" + r.NoKodeBarang + "&kdvlds=" + r.NoValidasi + "' class='btn btn-info'>";
        x += "		    <i class='icon-search'></i>";
        x += "	    </a>";
        x += "    </div>";
        x += "    <input type='text' maxlength='50' class='form-control' style='" + x2 + "width:150px;' disabled='disabled' value='" + r.NoSPK + "' >";
        x += "</div>";

        return x;
    }

    protected bool status_view_DO_by_SBU(T_brj_receive r)
    {
        bool status = false;

        if (r.IDCompanyBrand.HasValue)
        {
            var rsCB = (from p in db.CompanyBrand
                        where p.Draft == false && p.Disable == false
                        select p.ID).First();

            int CBID = 0; /// mondial, franknco, missmondial, thepalace

            if (rsCB != null)
            {
                CBID = rsCB;
            }

            if (r.IDCompanyBrand == 3 || r.IDCompanyBrand == 4)
            {
                if (CBID == 3 || CBID == 4) //// mondial / missmondial
                {
                    status = true;
                }
            }
            else if (CBID == r.IDCompanyBrand)
            {
                status = true;
            }
        }
        else
        {
            status = true;
        }

        return status;
    }

    protected string TD_HyperLink_BRJ_Detail(T_brj r, bool statusDO)
    {
        string x = string.Empty;
        string txtDisabled = "";
        if (!statusDO) { txtDisabled = "disabled='disabled'"; }
        if (r.IDReceive != null && r.NoReceive != null) { txtDisabled = "disabled='disabled'"; }

        if (r.IDReceive == null && r.NoReceive == null)
        {
            if (r.IDSPK == null)
            {
                x += "<a class=\"btn btn-default\" " + txtDisabled + " href=\"DJDO_DataBRJSave.aspx"
                    + "?nodo=" + r.NoDOBRJ
                    + "&nobrg=" + r.NoKodeBarang
                    + "&noval=" + r.NoValidasi
                    + "&nobag=" + r.NoKodeBagian
                    + "\">"
                    ;

                x += "  <i class=\"icon-edit\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-default\" " + txtDisabled + " href=\"../DJDO_DataBRJSave.aspx"
                    + "?nodo=" + r.NoDOBRJ
                    + "&nobrg=" + r.NoKodeBarang
                    + "&noval=" + r.NoValidasi
                    + "&nobag=" + r.NoKodeBagian
                    + "\">"
                    ;

                x += "  <i class=\"icon-edit\"></i> ";
                x += "  ";
                x += "</a>";
            }

        }
        else if (r.IDReceive != null && r.NoReceive == null)
        {
            try
            {
                int IDRec = r.IDReceive.Value;
                var d = db.StockReceiveDJ.Single(p => p.ID == IDRec);

                if (d.Draft)
                {
                    x += "<a class=\"btn btn-warning\" " + txtDisabled + " href=\"/StockReceive/Page/Register/DJ/DJDONoSPK/DJDO_Detail.aspx"
                        + "?id=" + r.IDReceive
                        + "&nodo=" + myNODO
                        + "&nobrg=" + r.NoKodeBarang
                        + "&noval=" + r.NoValidasi
                        + "&nobag=" + r.NoKodeBagian
                        + "\">"
                        ;

                    x += "  <i class=\"icon-edit\"></i> ";
                    x += "  ";
                    x += "</a>";

                }
                else
                {
                    // kondisi jika error approve
                    var sa = db.StockActualDJ.SingleOrDefault(p => p.Nomor == d.Nomor);
                    var sp = db.StockProductDJ.SingleOrDefault(p => p.Nomor == d.Nomor);

                    string mode1 = "default";
                    string mode2 = "search";

                    if ((sa == null || sp == null) && d.Nomor != null)
                    {
                        mode1 = "danger";
                        mode2 = "warning-sign";
                    }

                    //redirect informasi yg pernah diinput
                    x += "<a>";
                    x += "<a class=\"btn btn-" + mode1 + "\" " + txtDisabled + " href=\"/StockReceive/Page/Register/DJ/DJDONoSPK/DJDO_Detail.aspx"
                        + "?id=" + r.IDReceive
                        + "&nodo=" + myNODO
                        + "&conf=2"
                        + "\">"
                        ;

                    x += "  <i class=\"icon-" + mode2 + "\"></i> ";
                    x += "  ";
                    x += "</a>";

                    //x += "&nbsp;";

                    //redirect ke update
                    //x += "<a target=\"_blank\" class=\"btn btn-success\" " + txtDisabled + " href=\"../../Update/DJ/DJ_Form.aspx"
                    //    + "?id=" + r.IDReceive
                    //    + "\">"
                    //    ;

                    //x += "  <i class=\"icon-edit\"></i> ";
                    //x += "  ";
                    //x += "</a>";


                }
            }
            catch (Exception ex)
            {
                txtDisabled = "disabled='disabled'";
                //x += "  <i class=\"icon-warning-sign text-red\"></i> ";

                x += "<a>";
                x += "<a class=\"btn btn-default\" " + txtDisabled + ""
                    + "\">"
                    ;

                x += "  <i class=\"icon-warning-sign text-red\"></i> ";
                x += "  ";
                x += "</a>";
            }

        }
        else if (r.IDReceive != null && r.NoReceive != null)
        {
            txtDisabled = "";
            //redirect informasi yg pernah diinput
            x += "<a>";
            x += "<a target=\"_blank\" class=\"btn btn-info\" " + txtDisabled + " href=\"/StockReceive/Page/Register/DJ/DJDONoSPK/DJDO_Detail.aspx"
                + "?id=" + r.IDReceive
                + "\">"
                ;

            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";

        }

        return x;
    }

    public void show_process(int flag = 0)
    {
        lit_process.Text = "";
        string result = "";
        string mode = "default";
        string icon = "ok";
        string color = "gray";
        string stepname = "";
        string statustext = "-";

        result += "<div class='row'>";
        result += "<div class='col-xs-12'><hr /></div>";

        mode = "default";
        icon = "minus";
        color = "gray";
        statustext = "-";

        if (flag == 1 || flag == 5 || flag == 6)
        {
            // local data success
            mode = "success";
            color = "green";
            icon = "ok";
            statustext = "Success";
        }
        else if (flag == 0)
        {
            // no text input
            mode = "default";
            icon = "minus";
            color = "gray";
            statustext = "-";
        }
        else
        {
            // local data failed
            mode = "danger";
            color = "red";
            icon = "remove";
            statustext = "Failed";
        }

        stepname = "1. Searching Local Data...";
        result += "<div class='col-xs-12 col-sm-4 col-md-4'>";
        result += "<div class='panel panel-" + mode + "'><div class='panel-body'>";
        result += "<div><div class='col-xs-2 col-sm-4 col-md-3'><span style='font-size:38px; color:" + color + ";' class='glyphicon glyphicon-" + icon + "' aria-hidden='true'></span></div>";
        result += "<div class='col-xs-10 col-sm-8 col-md-9'>";
        result += "<span class=''>" + stepname + "</span><br />";
        result += "<span class='text-muted'>Status: </span>";
        result += "<span class='text-lg text-" + mode + "'>" + statustext + "</span>";
        result += "</div></div>";
        result += "</div></div>";
        result += "</div>";

        mode = "default";
        icon = "minus";
        color = "gray";
        statustext = "-";

        if (flag == 3 || flag == 4 || flag == 5 || flag == 6)
        {
            // local data success
            mode = "success";
            color = "green";
            icon = "ok";
            statustext = "Success";
        }
        else if (flag == 2)
        {
            // local data failed
            mode = "danger";
            color = "red";
            icon = "remove";
            statustext = "Failed";
        }

        stepname = "2. Connecting to Factory Database...";
        result += "<div class='col-xs-12 col-sm-4 col-md-4'>";
        result += "<div class='panel panel-" + mode + "'><div class='panel-body'>";
        result += "<div><div class='col-xs-2 col-sm-4 col-md-3'><span style='font-size:38px; color:" + color + ";' class='glyphicon glyphicon-" + icon + "' aria-hidden='true'></span></div>";
        result += "<div class='col-xs-10 col-sm-8 col-md-9'>";
        result += "<span class=''>" + stepname + "</span><br />";
        result += "<span class='text-muted'>Status: </span>";
        result += "<span class='text-lg text-" + mode + "'>" + statustext + "</span>";
        result += "</div></div>";
        result += "</div></div>";
        result += "</div>";

        mode = "default";
        icon = "minus";
        color = "gray";
        statustext = "-";

        if (flag == 3 || flag == 5)
        {
            // storing and select local data success
            mode = "success";
            color = "green";
            icon = "ok";
            statustext = "Success";
        }
        else if (flag == 4 || flag == 6)
        {
            // storing and select local data success
            mode = "danger";
            color = "red";
            icon = "remove";
            statustext = "Failed";
        }

        stepname = "3. Searching Factory Data...";
        result += "<div class='col-xs-12 col-sm-4 col-md-4'>";
        result += "<div class='panel panel-" + mode + "'><div class='panel-body'>";
        result += "<div><div class='col-xs-2 col-sm-4 col-md-3'><span style='font-size:38px; color:" + color + ";' class='glyphicon glyphicon-" + icon + "' aria-hidden='true'></span></div>";
        result += "<div class='col-xs-10 col-sm-8 col-md-9'>";
        result += "<span class=''>" + stepname + "</span><br />";
        result += "<span class='text-muted'>Status: </span>";
        result += "<span class='text-lg text-" + mode + "'>" + statustext + "</span>";
        result += "</div></div>";
        result += "</div></div>";
        result += "</div>";

        result += "</div>";



        lit_process.Text = result;
    }
}