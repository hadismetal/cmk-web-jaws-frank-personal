using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Summary : System.Web.UI.Page
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
   

    protected int myID { get { return App.QueryInt(this); } }
    protected int status { get { return App.QueryInt(this,"status"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillform();
        }
    }

    protected void fillform()
    {
         var d = db.StockRetireDJ.SingleOrDefault(p => p.ID == myID);
         if (d == null) App.DataNull(this);
         else
         {
             no.InnerText = d.Nomor;
             tgl.InnerText = Str.TglJam(d.Tgl);
            kobar.InnerText = d.StockProductDJ.SupplierNomor;
            kplu.InnerText = d.StockProductDJ.Nomor;
            pitem.InnerHtml = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
             divisi.InnerText = d.StockProductDJ.StockActualDJ.CompanyBrand.Nama;
            string kodemelt = (from p in db.MasterMeltingReason where p.ID == d.Reason select p.Kode).SingleOrDefault();
            alasan.InnerText = kodemelt;

            if (status == 1)
            {
                Upload.Image(imgpicture, d.StockProductDJ.ImgPicture);
            }
            else
            {
                imgpicture.Visible = false;
            }

             bruto.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight.ToString("N2") + "";
             netto.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2") + "";
             //bsb.InnerText = d.NettoSetelahBongkar.Value.ToString("N2") + " gr";
             //sb.InnerText = d.SusutBongkar.Value.ToString("N2") + " gr";
             //sl.InnerText = d.SusutLebur.Value.ToString("N2") + " gr";
             //ba.InnerText = d.BeratAkhir.Value.ToString("N2") + " gr";
             penyetor.Text = d.Operator;

            DateTime tgl_brj = (from p1 in db.StockRetireDJ join
                                   p2 in db.StockProductDJ on p1.IDProduct equals p2.ID join
                                   p3 in db.StockReceiveDJ on p2.Nomor equals p3.Nomor
                                where p1.ID == myID
                                select p3.Tgl).SingleOrDefault();

            tglbrj.InnerText = Str.Tgl(tgl_brj);

            int total_butir = 0;
            decimal total_carat = 0m;

            DataTable dt = Db.Rs("SELECT Nomor, Parcel, Carat, GIA FROM StockRetireDJLD WHERE IDForm = " + myID.ToString(), Db.Cnn_DB);

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = row["Nomor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["Parcel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(row["Carat"]).ToString("N3");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = row["GIA"].ToString();
                    tr.Cells.Add(td);

                    tb_ld.Rows.Add(tr);
                }
            }

             foreach (var r in d.StockProductDJ.StockProductDJ_Stone1A)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone1A.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;
                 tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                td2.Text = "<br />";

                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }

             foreach (var r in d.StockProductDJ.StockProductDJ_Stone1B)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone1B.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                if (r.GIA != null)
                {
                    td2.Text = r.GIA ;
                }
                else
                {
                    td2.Text = "<br />";
                }
                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Center;
                    td.Text = row["Parcel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Center;
                    td.Text = "1";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.HorizontalAlign = HorizontalAlign.Center;
                    td.Text = Convert.ToDecimal(row["Carat"]).ToString("N3");
                    tr.Cells.Add(td);

                    total_butir += 1;
                    total_carat += Convert.ToDecimal(row["Carat"]);
                    tb1.Rows.Add(tr);

                    TableRow tr2 = new TableRow();
                    TableCell td2;
                    td2 = new TableCell();
                    td2.HorizontalAlign = HorizontalAlign.Center;
                    td2.Text = row["GIA"].ToString();
                    tr2.Cells.Add(td2);
                    tb5.Rows.Add(tr2);
                }
            }

            foreach (var r in d.StockProductDJ.StockProductDJ_Stone2)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone2.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;

                tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                if (r.GIA != null)
                {
                    td2.Text = r.GIA ;
                }
                else
                {
                    td2.Text = "<br />";
                }
                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }

             foreach (var r in d.StockProductDJ.StockProductDJ_Stone3)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone3.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                if (r.GIA != null)
                {
                    td2.Text = r.GIA ;
                }
                else
                {
                    td2.Text = "<br />";
                }
                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }

             foreach (var r in d.StockProductDJ.StockProductDJ_Stone4)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone4.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                if (r.GIA != null)
                {
                    td2.Text =  r.GIA ;
                }
                else
                {
                    td2.Text = "<br />";
                }
                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }

             foreach (var r in d.StockProductDJ.StockProductDJ_Stone5)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.Stone5.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir += r.TotalButir;
                total_carat += r.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr2 = new TableRow();
                TableCell td2;
                td2 = new TableCell();
                td2.HorizontalAlign = HorizontalAlign.Center;
                if (r.GIA != null)
                {
                    td2.Text =  r.GIA  ;
                }
                else
                {
                    td2.Text = "<br />";
                }
                tr2.Cells.Add(td2);
                tb5.Rows.Add(tr2);
            }


            TableFooterRow tr1 = new TableFooterRow();
            TableCell td1;

            td1 = new TableCell();
            td1.HorizontalAlign = HorizontalAlign.Center;
            td1.VerticalAlign = VerticalAlign.Bottom;
            td1.Text = "Total";
            td1.Attributes["style"] = "Font-Weight:Bold;";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.HorizontalAlign = HorizontalAlign.Center;
            td1.VerticalAlign = VerticalAlign.Bottom;
            td1.Text = Str.Integer(total_butir);
            td1.Attributes["style"] = "Font-Weight:Bold;";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.HorizontalAlign = HorizontalAlign.Center;
            td1.VerticalAlign = VerticalAlign.Bottom;
            td1.Text = Str.Number3(total_carat);
            td1.Attributes["style"] = "Font-Weight:Bold;";
            tr1.Cells.Add(td1);

            Table1.Rows.Add(tr1);


            /*foreach (var r in d.StockRetireDJ_Stone1A)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.TotalTerima.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = "-";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);
            }

            foreach (var r in d.StockRetireDJ_Stone1A)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.TotalRusak.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = "-";
                tr.Cells.Add(td);

                tb3.Rows.Add(tr);
            }

            foreach (var r in d.StockRetireDJ_Stone1A)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = r.TotalSelisih.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.HorizontalAlign = HorizontalAlign.Center;
                td.Text = "-";
                tr.Cells.Add(td);

                tb4.Rows.Add(tr);
            }*/



        }
    }



    
}