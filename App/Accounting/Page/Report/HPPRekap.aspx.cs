using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

            var x = from n in db.PaymentType
                    select new { n.ID, n.Nama };
            
            foreach (var r in x)
            {
                payment.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            int Tipe = 1;
            if (tipelokasi.SelectedValue != "")
                Tipe = Convert.ToInt32(tipelokasi.SelectedValue);

            DLC.LocationList(lokasi, 1);

            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);

        int Filter = Convert.ToInt32(Request.QueryString["filter"]);

        var rs = from p in db.SalesOrderDJ
                  where p.StockProductDJ.Legacy.Value == null && !p.SalesOrder.Draft
                  && !(from z in db.SalesOrderDJ
                         where z.StockProductDJ.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.IDForm)
                  //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                     select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : 1 == 1)
                  select p;

        var rsstone = (from p in db.StockProductDJ_Stone1A
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone1A.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone1A.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0, 
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      }).Concat(
                      from p in db.StockProductDJ_Stone1B
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone1B.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone1B.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0,
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      }).Concat(
                      from p in db.StockProductDJ_Stone2
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone2.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone2.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0,
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      }).Concat(from p in db.StockProductDJ_Stone3
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone3.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone3.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0,
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      }).Concat(
                      from p in db.StockProductDJ_Stone4
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone4.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone4.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0,
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      }).Concat(from p in db.StockProductDJ_Stone5
                      join q in db.SalesOrderDJ on p.StockProductDJ.ID equals q.IDProduct
                      where p.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                      && !(from z in db.SalesOrderDJ
                           where z.StockProductDJ.Legacy.Value == 1
                           select z.SalesOrder.ID).Contains(p.IDForm)
                          //&& p.SalesOrder.StatusPembayaran == false
                      && q.SalesOrder.StatusPembayaran == true
                      && ((Filter == 1) ?
                      (from z in db.StockBasic
                       select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : (Filter == 2) ? !(from z in db.StockBasic
                                          select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                      : 1 == 1)
                      select new
                      {
                          IDStone = p.Stone5.Nama,
                          Invoice = q.Invoice,
                          SalesOrder = q.SalesOrder,
                          StockProductDJ = p.StockProductDJ,
                          Parcel = p.Stone5.Nama,
                          HargaTotalM = p.HargaTotalM.HasValue ? p.HargaTotalM.Value : 0,
                          Butir = p.TotalButir,
                          Carat = p.TotalCarat
                      });

        int Payment = Convert.ToInt32(Request.QueryString["payment"]);

        if (Tipe != 0)
        {
            rs = rs.Where(p => p.SalesOrder.TipeLokasi == Tipe);
            rsstone = rsstone.Where(p => p.SalesOrder.TipeLokasi == Tipe);
        }
        if (IDLokasi != 0)
        {
            rs = rs.Where(p => p.SalesOrder.IDLokasi == IDLokasi);
            rsstone = rsstone.Where(p => p.SalesOrder.IDLokasi == IDLokasi);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl).ThenBy(p=>p.SalesOrder.Nomor);

        string keyword = kw.Text;
        if (keyword != "")
        {
            rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.StockProductDJ.Nomor.Contains(kw.Text));
            rsstone = rsstone.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.StockProductDJ.Nomor.Contains(kw.Text));
        }

        if (status.SelectedValue == "1")
        {
            rs = rs.Where(p => p.SalesOrder.StatusVoid == true);
            rsstone = rsstone.Where(p => p.SalesOrder.StatusVoid == true);
        }
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.SalesOrder.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            rsstone = rsstone.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var rsg = rs.GroupBy(p => p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory);

        if (rs.Count() > 0)
        {
            TableRow tr3 = new TableRow();
            TableCell td3;

            td3 = new TableCell();
            td3.Attributes["style"] = "background: #ffffcc;";
            td3.Text = "";
            td3.ColumnSpan = 4;
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Attributes["style"] = "text-align: center;";
            td3.Text = "<b>T. M Stone</b>";
            td3.ColumnSpan = 2;
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td3.Text = "<b>Frame</b>";
            td3.ColumnSpan = 2;
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Attributes["style"] = "text-align: center;";
            td3.Text = "<b>Ongkos</b>";
            td3.ColumnSpan = 2;
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td3.Text = "<b>Total HPP</b>";
            td3.ColumnSpan = 2;
            tr3.Cells.Add(td3);

            tb.Rows.Add(tr3);

            decimal TotalHarga = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHPP = 0m;
            decimal TotalMBatu = 0m;
            decimal TotalMBatuRp = 0m;
            decimal TotalFrame = 0m;
            decimal TotalFrameRp = 0m;
            decimal TotalOngkos = 0m;
            decimal TotalOngkosRp = 0m;
            decimal TotalHPPRp = 0m;
            int prev = 0;
            int index = 1;
            string SO = "";
            foreach (var r in rsg)
            {
                

                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = r.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                string ProductCategory = "";
                ProductCategory = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.Key, Db.Cnn_DB);
                
                td = new TableCell();
                td.Text = ProductCategory;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p=>p.TotalBayar.Value).ToString("N0");
                tr.Cells.Add(td);

                decimal TotalHargaParcelM = 0m;
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.HargaTotalM.Value));
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1B.Sum(z => z.HargaTotalM.Value));
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.HargaTotalM.Value));
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.HargaTotalM.Value));
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.HargaTotalM.Value));
                TotalHargaParcelM += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.HargaTotalM.Value));

                decimal TotalHargaParcelMRp = 0m;
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone1B.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                TotalHargaParcelMRp += r.Sum(p => p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.HargaTotalM.Value * z.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
 
                //Stone Rekap
                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = TotalHargaParcelM.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = TotalHargaParcelMRp.ToString("N0");
                tr.Cells.Add(td);

                decimal HargaFrame = r.Sum(p=>p.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct);
                decimal HargaFrameRp = (r.Sum(p=>p.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct * p.StockProductDJ.StockProductDJ_Costing.TotalRate.Value));
                //Frame
                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = HargaFrame.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = HargaFrameRp.ToString("N0");
                tr.Cells.Add(td);

                decimal Ongkos = ((r.Sum(p=>(p.StockProductDJ.StockProductDJ_Costing.MountingNO.Value + p.StockProductDJ.StockProductDJ_Costing.MountingRO) / p.StockProductDJ.StockProductDJ_Costing.TotalRate.Value)));
                decimal OngkosRp = ((r.Sum(p=>p.StockProductDJ.StockProductDJ_Costing.MountingNO.Value + p.StockProductDJ.StockProductDJ_Costing.MountingRO)));
                //Ongkos
                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = Ongkos.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = OngkosRp.ToString("N0");
                tr.Cells.Add(td);

                decimal HPP = 0m;
                decimal HPPRp = 0m;
                HPP = Ongkos + HargaFrame + TotalHargaParcelM;
                HPPRp = OngkosRp + HargaFrameRp + TotalHargaParcelMRp;

                //Total HPP
                td = new TableCell();
                td.Attributes["style"] = "text-align: center; background: #ffffcc;";
                td.Text = HPP.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Attributes["style"] = "text-align: center;";
                td.Text = HPPRp.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalHarga += r.Sum(p=>p.TotalBayar.Value);
                TotalMBatu += TotalHargaParcelM;
                TotalMBatuRp += TotalHargaParcelMRp;
                TotalFrame += HargaFrame;
                TotalFrameRp += HargaFrameRp;
                TotalOngkos += Ongkos;
                TotalOngkosRp += OngkosRp;
                TotalHPP += HPP;
                TotalHPPRp += HPPRp;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.Attributes["style"] = "text-align: center;";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td2.Text = "<b>" + TotalMBatu.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            //Stone Rekap
            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center;";
            td2.Text = "<b>" + TotalMBatuRp.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td2.Text = "<b>" + TotalMBatuRp.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            //Frame
            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center;";
            td2.Text = "<b>" + TotalFrame.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            //Ongkos
            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td2.Text = "<b>" + TotalOngkos.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center;";
            td2.Text = "<b>" + TotalOngkosRp.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            //Total HPP
            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center; background: #ffffcc;";
            td2.Text = "<b>" + TotalHPP.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Attributes["style"] = "text-align: center;";
            td2.Text = "<b>" + TotalHPPRp.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);


            var stoneg = rsstone.GroupBy(p => p.IDStone);
            int index2 = 1;
            decimal TotalStoneM = 0m;
            foreach (var r in stoneg)
            {
                TableRow tr5 = new TableRow();
                TableCell td5;

                //Stone
                td5 = new TableCell();
                td5.Text = index2++.ToString() + ".";
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = r.Key;
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = r.Sum(p=>p.Butir).ToString("N0");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = r.Sum(p => p.Carat).ToString("N3");
                tr5.Cells.Add(td5);

                td5 = new TableCell();
                td5.Text = r.Sum(p => p.HargaTotalM).ToString("N2");
                tr5.Cells.Add(td5);

                tb2.Rows.Add(tr5);

                TotalButir += r.Sum(p => p.Butir);
                TotalCarat += r.Sum(p => p.Carat);
                TotalStoneM += r.Sum(p => p.HargaTotalM);
            }

            TableRow tr6 = new TableRow();
            TableCell td6;

            //Stone
            td6 = new TableCell();
            td6.Text = "";
            tr6.Cells.Add(td6);

            td6 = new TableCell();
            td6.Text = "";
            tr6.Cells.Add(td6);

            td6 = new TableCell();
            td6.Text = "<b>" + TotalButir.ToString("N0") + "</b>"; 
            tr6.Cells.Add(td6);

            td6 = new TableCell();
            td6.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr6.Cells.Add(td6);

            td6 = new TableCell();
            td6.Text = "<b>" + TotalStoneM.ToString("N2") + "</b>";
            tr6.Cells.Add(td6);

            tb2.Rows.Add(tr6);


        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            payment.SelectedValue = Request.QueryString["payment"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "HPPDetail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "HPPDetail_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            ListItem item = new ListItem("All", "0");
            lokasi.Items.Add(item);
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}