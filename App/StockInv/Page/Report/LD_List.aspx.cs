using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            DLC.LocationList(cabang, tipe);
            Ddl.Substorage(substorage);

            DLC.Parcel101(parcel01);
            DLC.Parcel102(parcel02, true);
            DLC.Parcel103(parcel03);
            DLC.Parcel104(parcel04);
            DLC.Parcel105(parcel05);
            DLC.Parcel106(parcel06);
            DLC.Parcel107(parcel07);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        int Shape = 0;
        int Carat = 0;
        int Colour = 0;
        int Clarity = 0;
        int Cutting = 0;
        int Brand = 0;
        int Category = 0;
        int substrg = 0;
        int Cabang = Convert.ToInt32(cabang.SelectedValue);


        if (substorage.SelectedIndex > 0)
        {
            substrg = Convert.ToInt32(substorage.SelectedValue);
        }
        else
        {
            substrg = 0;
        }
        if (parcel01.SelectedIndex > 0)
        {
            Shape = Convert.ToInt32(parcel01.SelectedValue);
        }
        if (parcel02.SelectedIndex > 0)
        {
            Carat = Convert.ToInt32(parcel02.SelectedValue);
        }
        if (parcel03.SelectedIndex > 0)
        {
            Colour = Convert.ToInt32(parcel03.SelectedValue);
        }
        if (parcel04.SelectedIndex > 0)
        {
            Clarity = Convert.ToInt32(parcel04.SelectedValue);
        }
        if (parcel05.SelectedIndex > 0)
        {
            Cutting = Convert.ToInt32(parcel05.SelectedValue);
        }
        if (parcel06.SelectedIndex > 0)
        {
            Brand = Convert.ToInt32(parcel06.SelectedValue);
        }
        if (parcel07.SelectedIndex > 0)
        {
            Category = Convert.ToInt32(parcel07.SelectedValue);
        }

        int pageNumber = 0;
        int numberOfObjectsPerPage = 30;
        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 0;

        DataTable product;
        DataTable total;

        string query1 = "EXEC sp_StockRekapLD_hadis '" + kw.Text + "'," + Cabang + "," + Shape + "," + Carat + "," + Colour + "," + Clarity + "," + Cutting + "," + Brand + "," + Category + "," + substrg + "," + tipe + ", '','','list'," + pageNumber + "," + numberOfObjectsPerPage + "";
        DataSet ds = QuerySPDataSet(query1, Db.Cnn_DB);
        // Response.Write(query1);
        if (ds.Tables.Count > 0)
        {
            product = ds.Tables[0];
            total = ds.Tables[1];

            //var TotalPage = Convert.ToDecimal(total.Rows[0]["totalData"]) / Convert.ToDecimal(numberOfObjectsPerPage);
            var TotalPage = Math.Ceiling(Convert.ToDecimal(total.Rows[0]["totalData"]) / Convert.ToDecimal(numberOfObjectsPerPage));
            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            href += "&cabang=" + cabang.Text;
            href += "&tipe=" + tipe;
            href += "&parcel01=" + parcel01.SelectedIndex;
            href += "&parcel02=" + parcel02.SelectedIndex;
            href += "&parcel03=" + parcel03.SelectedIndex;
            href += "&parcel04=" + parcel04.SelectedIndex;
            href += "&parcel05=" + parcel05.SelectedIndex;
            href += "&parcel06=" + parcel06.SelectedIndex;
            href += "&parcel07=" + parcel07.SelectedIndex;
            href += "&substorage=" + substorage.SelectedValue;
            href += "&page=" + (pageNumber + 1).ToString();

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            href2 += "&cabang=" + cabang.Text;
            href2 += "&tipe=" + tipe;
            href2 += "&parcel01=" + parcel01.SelectedIndex;
            href2 += "&parcel02=" + parcel02.SelectedIndex;
            href2 += "&parcel03=" + parcel03.SelectedIndex;
            href2 += "&parcel04=" + parcel04.SelectedIndex;
            href2 += "&parcel05=" + parcel05.SelectedIndex;
            href2 += "&parcel06=" + parcel06.SelectedIndex;
            href2 += "&parcel07=" + parcel07.SelectedIndex;
            href2 += "&substorage=" + substorage.SelectedValue;
            href2 += "&page=" + (pageNumber - 1).ToString();

            string href3 = App.RefreshURL(this);

            //Customize-Qstr
            href3 += "&kw=" + kw.Text;
            href3 += "&cabang=" + cabang.Text;
            href3 += "&tipe=" + tipe;
            href3 += "&parcel01=" + parcel01.SelectedIndex;
            href3 += "&parcel02=" + parcel02.SelectedIndex;
            href3 += "&parcel03=" + parcel03.SelectedIndex;
            href3 += "&parcel04=" + parcel04.SelectedIndex;
            href3 += "&parcel05=" + parcel05.SelectedIndex;
            href3 += "&parcel06=" + parcel06.SelectedIndex;
            href3 += "&parcel07=" + parcel07.SelectedIndex;
            href3 += "&substorage=" + substorage.SelectedValue;
            href3 += "&page=0";

            string href4 = App.RefreshURL(this);

            //Customize-Qstr
            href4 += "&kw=" + kw.Text;
            href4 += "&cabang=" + cabang.Text;
            href4 += "&tipe=" + tipe;
            href4 += "&parcel01=" + parcel01.SelectedIndex;
            href4 += "&parcel02=" + parcel02.SelectedIndex;
            href4 += "&parcel03=" + parcel03.SelectedIndex;
            href4 += "&parcel04=" + parcel04.SelectedIndex;
            href4 += "&parcel05=" + parcel05.SelectedIndex;
            href4 += "&parcel06=" + parcel06.SelectedIndex;
            href4 += "&parcel07=" + parcel07.SelectedIndex;
            href4 += "&substorage=" + substorage.SelectedValue;
            href4 += "&page=" + (TotalPage - 1);

            string paging = "";

            paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href4 + "\">Last</a>";
            paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

            paging_text.Text = paging;


            //rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);    
            int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalCarat = 0m;
            decimal TotalQty = 0m;
            foreach (DataRow r in product.Rows)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nomor"].ToString();
                tr.Cells.Add(td);



                td = new TableCell();
                td.Text = r["Parcel"].ToString(); ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["GIA"].ToString();
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r["m_report"].ToString();
                tr.Cells.Add(td);



                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r["Substorage"].ToString() == "1")
                    Jual = Convert.ToDecimal(r["InHand"]);
                else if (r["Substorage"].ToString() == "2")
                    Resell = Convert.ToDecimal(r["InHand"]);
                else if (r["Substorage"].ToString() == "3")
                    Exchange = Convert.ToDecimal(r["InHand"]);

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = Convert.ToInt32(r["InHand"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(Convert.ToInt32(r["Substorage"]));
                tr.Cells.Add(td);

                string Lokasi = r["nmLoc"].ToString();


                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaTotalM"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaTotal"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["totCarat"]).ToString("N3");
                tr.Cells.Add(td);

                string Status = r["status"].ToString();

                string Href = "";
                int Cancel = Convert.ToInt32(r["cancel"]);

                td = new TableCell();
                td.Text = Status;
                tr.Cells.Add(td);

                /// Shape
                td = new TableCell();

                td.Text = Convert.ToString(r["shape"]);
                tr.Cells.Add(td);

                /// Size
                td = new TableCell();
                td.Text = r["size"].ToString();
                tr.Cells.Add(td);

                /// Brand
                td = new TableCell();
                td.Text = r["brandStone"].ToString();
                tr.Cells.Add(td);

                /// Colour
                td = new TableCell();
                td.Text = Convert.ToString(r["color"]);
                tr.Cells.Add(td);

                /// Clarity
                td = new TableCell();
                td.Text = Convert.ToString(r["clarity"]);
                tr.Cells.Add(td);

                /// Cancel Booking
                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(r["ID"]), Cancel);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalModal += Convert.ToDecimal(r["HargaTotalM"]);
                TotalHarga += Convert.ToDecimal(r["HargaTotal"]);
                TotalCarat += Convert.ToDecimal(r["totCarat"]);
                TotalQty += Convert.ToDecimal(r["InHand"]);
                index++;
            }

            if (ds.Tables.Count > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 5;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "";
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalQty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalModal.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
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

                td = new TableCell();
                td.Text = "";
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

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }



    // protected void fillTable(object sender, EventArgs e)
    // {

    //     DateTime start;
    //     DateTime end;

    //     var rs = from p in db.StockProductLD_Stone1B
    //              where p.ReturnProduct == false && !p.StockProductLD.Draft.Value
    //              && p.StockActualLD_Stone1B.IDLokasi > 0
    //              && !(from s in db.SalesOrderLD
    //                   where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
    //                   select s.IDProduct).Contains(p.ID)
    //              && !(from s in db.SalesOrderSouvenir
    //                   where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
    //                   select s.IDProductSouvenir).Contains(p.ID)
    //              select p;

    //     int Substorage = Convert.ToInt32(substorage.SelectedValue);
    //     if (Substorage > 0)
    //     {
    //         rs = rs.Where(p => p.StockActualLD_Stone1B.Substorage == Substorage);
    //     }

    //     rs = rs.OrderByDescending(p => p.StockProductLD.ApprovalTgl);


    //     if (kw.Text != "") { rs = rs.Where(p => p.Nomor == "" + kw.Text + "" || p.GIA == "" + kw.Text + ""); }

    //     int Cabang = Convert.ToInt32(cabang.SelectedValue);
    //     if (Cabang > 0) rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == Cabang && p.StockActualLD_Stone1B.TipeLokasi == tipe);
    //     else rs = rs.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe);

    //     if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
    //     if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
    //     if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
    //     if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));
    //     if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel05 == Convert.ToInt32(parcel05.SelectedValue));
    //     if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel06 == Convert.ToInt32(parcel06.SelectedValue));
    //     if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Stone1B.Parcel07 == Convert.ToInt32(parcel07.SelectedValue));

    //     int pageNumber = 0;
    //     int numberOfObjectsPerPage = 30;

    //     if (Request.QueryString["page"] != "")
    //         pageNumber = Convert.ToInt32(Request.QueryString["page"]);
    //     if (pageNumber < 0)
    //         pageNumber = 0;

    //     decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

    //     TotalPage = Math.Ceiling(TotalPage);

    //     rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

    //     if (rs != null)
    //     {
    //         int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
    //         decimal TotalStock = 0m;
    //         decimal TotalResell = 0m;
    //         decimal TotalExchange = 0m;
    //         decimal TotalHarga = 0m;
    //         decimal TotalModal = 0m;
    //         decimal TotalCarat = 0m;
    //         foreach (var r in rs)
    //         {


    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = index.ToString() + ".";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.Nomor;
    //             tr.Cells.Add(td);

    //             string NamaParcel = "";
    //             try
    //             {
    //                 NamaParcel = r.Stone1B.Nama;
    //             }catch{}

    //             td = new TableCell();
    //             td.Text = NamaParcel;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.GIA;
    //             tr.Cells.Add(td);

    //             decimal Jual = 0m;
    //             decimal Exchange = 0m;
    //             decimal Resell = 0m;

    //             if (r.StockActualLD_Stone1B.Substorage == 1)
    //                 Jual = r.StockActualLD_Stone1B.InHand;
    //             else if (r.StockActualLD_Stone1B.Substorage == 3)
    //                 Exchange = r.StockActualLD_Stone1B.InHand;
    //             else if (r.StockActualLD_Stone1B.Substorage == 2)
    //                 Resell = r.StockActualLD_Stone1B.InHand;

    //             /*td = new TableCell();
    //             td.Text = Jual.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Exchange.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Resell.ToString("N0");
    //             tr.Cells.Add(td);*/

    //             td = new TableCell();
    //             td.Text = r.StockActualLD_Stone1B.InHand.ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = DL.SubStorage(r.StockActualLD_Stone1B.Substorage);
    //             tr.Cells.Add(td);

    //             string Lokasi = "";
    //             if (r.StockActualLD_Stone1B.TipeLokasi == 0)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
    //             else if (r.StockActualLD_Stone1B.TipeLokasi == 1)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
    //             else if (r.StockActualLD_Stone1B.TipeLokasi == 2)
    //                 Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualLD_Stone1B.IDLokasi, Db.Cnn_DB);
    //             td = new TableCell();
    //             td.Text = Lokasi;
    //             tr.Cells.Add(td);


    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.HargaTotalM.Value).ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.HargaTotal.Value).ToString("N2");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.TotalCarat).ToString("N3");
    //             tr.Cells.Add(td);

    //             string Status = "Available";
    //             string BookLokasi = "";
    //             string Href= "";
    //             int Cancel = 0;
    //             if (r.BookExpire.HasValue && r.BookExpire > DateTime.Now)
    //             {
    //                 Cancel = 1;
    //                 Status = "Booking oleh ";
    //                 if (r.TipeLokasi == 1)
    //                     BookLokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
    //                 else
    //                     BookLokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
    //                 Status += BookLokasi;
    //             }

    //             td = new TableCell();
    //             td.Text = Status;
    //             tr.Cells.Add(td);

    //             /// Shape
    //             td = new TableCell();
    //             td.Text = r.Stone1B.Parcel101.Nama;
    //             tr.Cells.Add(td);

    //             /// Size
    //             td = new TableCell();
    //             td.Text = r.Stone1B.Parcel102.Nama;
    //             tr.Cells.Add(td);

    //             /// Brand
    //             td = new TableCell();
    //             td.Text = r.Stone1B.Parcel106.Nama;
    //             tr.Cells.Add(td);

    //             /// Colour
    //             td = new TableCell();
    //             td.Text = Convert.ToString(r.Stone1B.Parcel103.Nama);
    //             tr.Cells.Add(td);

    //             /// Clarity
    //             td = new TableCell();
    //             td.Text = Convert.ToString(r.Stone1B.Parcel104.Nama);
    //             tr.Cells.Add(td);

    //             /// Cancel Booking
    //             td = new TableCell();
    //             td.Text = TD_HyperLink(r.ID, Cancel);
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);

    //             TotalStock += Jual;
    //             TotalResell += Resell;
    //             TotalExchange += Exchange;
    //             TotalModal += Convert.ToDecimal(r.HargaTotalM.Value);
    //             TotalHarga += Convert.ToDecimal(r.HargaTotal.Value);
    //             TotalCarat += Convert.ToDecimal(r.TotalCarat);
    //             index++;
    //         }

    //         if (rs.Count() > 0)
    //         {
    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = "<b>Total</b>";
    //             td.ColumnSpan = 4;
    //             tr.Cells.Add(td);

    //             //td = new TableCell();
    //             //td.Text = "";
    //             //tr.Cells.Add(td);

    //             //td = new TableCell();
    //             //td.Text = "";
    //             //tr.Cells.Add(td);

    //             //td = new TableCell();
    //             //td.Text = "";
    //             //tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = rs.Sum(p => p.StockActualLD_Stone1B.InHand).ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<b>" + TotalModal.ToString("N2") + "</b>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<b>" + TotalHarga.ToString("N2") + "</b>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = "";
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);
    //         }
    //     }

    //     string href = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href += "&kw=" + kw.Text;
    //     href += "&cabang=" + cabang.Text;
    //     href += "&tipe=" + tipe;
    //     href += "&parcel01=" + parcel01.SelectedIndex;
    //     href += "&parcel02=" + parcel02.SelectedIndex;
    //     href += "&parcel03=" + parcel03.SelectedIndex;
    //     href += "&parcel04=" + parcel04.SelectedIndex;
    //     href += "&parcel05=" + parcel05.SelectedIndex;
    //     href += "&parcel06=" + parcel06.SelectedIndex;
    //     href += "&parcel07=" + parcel07.SelectedIndex;
    //     href += "&substorage=" + substorage.SelectedValue;
    //     href += "&page=" + (pageNumber + 1).ToString();

    //     string href2 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href2 += "&kw=" + kw.Text;
    //     href2 += "&cabang=" + cabang.Text;
    //     href2 += "&tipe=" + tipe;
    //     href2 += "&parcel01=" + parcel01.SelectedIndex;
    //     href2 += "&parcel02=" + parcel02.SelectedIndex;
    //     href2 += "&parcel03=" + parcel03.SelectedIndex;
    //     href2 += "&parcel04=" + parcel04.SelectedIndex;
    //     href2 += "&parcel05=" + parcel05.SelectedIndex;
    //     href2 += "&parcel06=" + parcel06.SelectedIndex;
    //     href2 += "&parcel07=" + parcel07.SelectedIndex;
    //     href2 += "&substorage=" + substorage.SelectedValue;
    //     href2 += "&page=" + (pageNumber - 1).ToString();

    //     string href3 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href3 += "&kw=" + kw.Text;
    //     href3 += "&cabang=" + cabang.Text;
    //     href3 += "&tipe=" + tipe;
    //     href3 += "&parcel01=" + parcel01.SelectedIndex;
    //     href3 += "&parcel02=" + parcel02.SelectedIndex;
    //     href3 += "&parcel03=" + parcel03.SelectedIndex;
    //     href3 += "&parcel04=" + parcel04.SelectedIndex;
    //     href3 += "&parcel05=" + parcel05.SelectedIndex;
    //     href3 += "&parcel06=" + parcel06.SelectedIndex;
    //     href3 += "&parcel07=" + parcel07.SelectedIndex;
    //     href3 += "&substorage=" + substorage.SelectedValue;
    //     href3 += "&page=0";

    //     string href4 = App.RefreshURL(this);

    //     //Customize-Qstr
    //     href4 += "&kw=" + kw.Text;
    //     href4 += "&cabang=" + cabang.Text;
    //     href4 += "&tipe=" + tipe;
    //     href4 += "&parcel01=" + parcel01.SelectedIndex;
    //     href4 += "&parcel02=" + parcel02.SelectedIndex;
    //     href4 += "&parcel03=" + parcel03.SelectedIndex;
    //     href4 += "&parcel04=" + parcel04.SelectedIndex;
    //     href4 += "&parcel05=" + parcel05.SelectedIndex;
    //     href4 += "&parcel06=" + parcel06.SelectedIndex;
    //     href4 += "&parcel07=" + parcel07.SelectedIndex;
    //     href4 += "&substorage=" + substorage.SelectedValue;
    //     href4 += "&page=" + (TotalPage-1);

    //     string paging = "";

    //     paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
    //     paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
    //     paging += "<a href=\"" + href4 + "\">Last</a>";
    //     paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

    //     paging_text.Text = paging;
    // }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
            parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
            parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
            parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
            parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
            parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
            parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
            substorage.SelectedValue = Request.QueryString["substorage"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "LD_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "LD_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&tipe=" + tipe;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&substorage=" + substorage.SelectedValue;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(int ID, int Cancel)
    {
        string x = string.Empty;
        string href = string.Empty;

        href += "LD_ItemCancelSave.aspx";
        href += "?id=" + ID;

        if (Cancel == 1)
        {
            x += "<a class=\"btn btn-danger\" href=\"" + href + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Cancel";
            x += "</a>";
        }
        else
            x += "-";

        return x;
    }
    protected DataSet QuerySPDataSet(string query, string connectionString)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            SqlTransaction trans;
            trans = conn.BeginTransaction();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
                sda.SelectCommand.CommandTimeout = 3000;
                sda.Fill(ds);
                sda.Dispose();
                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
                trans.Rollback();
                conn.Close();
            }
        }
        return ds;
    }
}