using System;
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
            Upload.Image(imgpicture, d.StockProductDJ.ImgPicture);
             bruto.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.GrossWeight.ToString("N2") + "";
             netto.InnerText = d.StockProductDJ.StockProductDJ_CharProduct.NettoWeight.ToString("N2") + "";
             bsb.InnerText = d.NettoSetelahBongkar.Value.ToString("N2") + "";
             sb.InnerText = d.SusutBongkar.Value.ToString("N2") + "";
             sl.InnerText = d.SusutLebur.Value.ToString("N2") + "";
             ba.InnerText = d.BeratAkhir.Value.ToString("N2") + "";
             penyetor.Text = d.Operator;
            string kodemelt = (from p in db.MasterMeltingReason where p.ID == d.Reason select p.Kode).SingleOrDefault();
            alasan.InnerText = kodemelt;

            DateTime tgl_brj = (from p1 in db.StockRetireDJ
                                join p2 in db.StockProductDJ on p1.IDProduct equals p2.ID
                                join p3 in db.StockReceiveDJ on p2.Nomor equals p3.Nomor
                                where p1.ID == myID
                                select p3.Tgl).SingleOrDefault();

            tglbrj.InnerText = Str.Tgl(tgl_brj);

            int total_butir1 = 0;
            decimal total_carat1 = 0m;
            int total_butir2 = 0;
            decimal total_carat2 = 0m;
            int total_butir3 = 0;
            decimal total_carat3 = 0m;
            int total_butir4 = 0;
            decimal total_carat4 = 0m;

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
                 td.Text = r.StockProductDJ_Stone1A.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone1A.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone1A.TotalButir;
                total_carat1 += r.StockProductDJ_Stone1A.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                td11.Text = "<br />";

                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone1B)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone1B.Stone1B.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone1B.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone1B.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone1B.TotalButir;
                total_carat1 += r.StockProductDJ_Stone1B.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                if (r.StockProductDJ_Stone1B.GIA != null)
                {
                    td11.Text = r.StockProductDJ_Stone1B.GIA;
                }
                else
                {
                    td11.Text = "<br />";
                }
                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone2)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone2.Stone2.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone2.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone2.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone2.TotalButir;
                total_carat1 += r.StockProductDJ_Stone2.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                if (r.StockProductDJ_Stone2.GIA != null)
                {
                    td11.Text = r.StockProductDJ_Stone2.GIA;
                }
                else
                {
                    td11.Text = "<br />";
                }
                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone3)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone3.Stone3.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone3.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone3.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone3.TotalButir;
                total_carat1 += r.StockProductDJ_Stone3.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                if (r.StockProductDJ_Stone3.GIA != null)
                {
                    td11.Text = r.StockProductDJ_Stone3.GIA;
                }
                else
                {
                    td11.Text = "<br />";
                }
                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone4)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone4.Stone4.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone4.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone4.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone4.TotalButir;
                total_carat1 += r.StockProductDJ_Stone4.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                if (r.StockProductDJ_Stone4.GIA != null)
                {
                    td11.Text = r.StockProductDJ_Stone4.GIA;
                }
                else
                {
                    td11.Text = "<br />";
                }
                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone5)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone5.Stone5.Nama;
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone5.TotalButir.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.StockProductDJ_Stone5.TotalCarat.ToString("N3");
                 tr.Cells.Add(td);

                total_butir1 += r.StockProductDJ_Stone5.TotalButir;
                total_carat1 += r.StockProductDJ_Stone5.TotalCarat;
                tb1.Rows.Add(tr);

                TableRow tr11 = new TableRow();
                TableCell td11;
                td11 = new TableCell();
                td11.HorizontalAlign = HorizontalAlign.Center;
                if (r.StockProductDJ_Stone5.GIA != null)
                {
                    td11.Text = r.StockProductDJ_Stone5.GIA;
                }
                else
                {
                    td11.Text = "<br />";
                }
                tr11.Cells.Add(td11);
                tb5.Rows.Add(tr11);
            }

             foreach (var r in d.StockRetireDJ_Stone1A)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                 tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;

                tb2.Rows.Add(tr);
            }

             foreach (var r in d.StockRetireDJ_Stone1B)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1B.Stone1B.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;
                tb2.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone2)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone2.Stone2.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;
                tb2.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone3)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone3.Stone3.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;
                tb2.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone4)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone4.Stone4.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;
                tb2.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone5)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone5.Stone5.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalTerima.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.CaratTerima.ToString("N3");
                tr.Cells.Add(td);

                total_butir2 += r.TotalTerima;
                total_carat2 += r.CaratTerima;
                tb2.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone1A)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);

             }

             foreach (var r in d.StockRetireDJ_Stone1B)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1B.Stone1B.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone2)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone2.Stone2.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone3)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone3.Stone3.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone4)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone4.Stone4.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone5)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone5.Stone5.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalRusak.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = "-";
                 tr.Cells.Add(td);

                total_butir3 += r.TotalRusak;
                total_carat3 += r.TotalRusak;
                tb3.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone1A)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1A.Stone1A.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone1A.TotalCarat - r.CaratTerima).ToString("N3");
                 tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone1B)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone1B.Stone1B.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone1B.TotalCarat - r.CaratTerima).ToString("N3");
                tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone2)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone2.Stone2.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone2.TotalCarat - r.CaratTerima).ToString("N3");
                tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone3)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone3.Stone3.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone3.TotalCarat - r.CaratTerima).ToString("N3");
                tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone4)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone4.Stone4.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone4.TotalCarat - r.CaratTerima).ToString("N3");
                tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             foreach (var r in d.StockRetireDJ_Stone5)
             {
                 TableRow tr = new TableRow();
                 TableCell td;

                 //td = new TableCell();
                 //td.HorizontalAlign = HorizontalAlign.Center;
                 //td.Text = r.StockProductDJ_Stone5.Stone5.Nama;
                 //tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = r.TotalSelisih.ToString("N0");
                 tr.Cells.Add(td);

                 td = new TableCell();
                 td.HorizontalAlign = HorizontalAlign.Center;
                 td.Text = (r.StockProductDJ_Stone5.TotalCarat - r.CaratTerima).ToString("N3");
                tr.Cells.Add(td);

                total_butir4 += r.TotalSelisih;
                total_carat4 += r.TotalSelisih;
                tb4.Rows.Add(tr);
             }

             //total on data awal sk
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
            td1.Text = Str.Integer(total_butir1);
            td1.Attributes["style"] = "Font-Weight:Bold;";
            tr1.Cells.Add(td1);

            td1 = new TableCell();
            td1.HorizontalAlign = HorizontalAlign.Center;
            td1.VerticalAlign = VerticalAlign.Bottom;
            td1.Text = Str.Number3(total_carat1);
            td1.Attributes["style"] = "Font-Weight:Bold;";
            tr1.Cells.Add(td1);

            Table1.Rows.Add(tr1);

            // total on setor stone
            TableFooterRow tr2 = new TableFooterRow();
            TableCell td2;

          

            td2 = new TableCell();
            td2.HorizontalAlign = HorizontalAlign.Center;
            td2.VerticalAlign = VerticalAlign.Bottom;
            td2.Text = Str.Integer(total_butir2);
            td2.Attributes["style"] = "Font-Weight:Bold;";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.HorizontalAlign = HorizontalAlign.Center;
            td2.VerticalAlign = VerticalAlign.Bottom;
            td2.Text = Str.Number3(total_carat2);
            td2.Attributes["style"] = "Font-Weight:Bold;";
            tr2.Cells.Add(td2);

            Table2.Rows.Add(tr2);

            // total on setor pecah
            TableFooterRow tr3 = new TableFooterRow();
            TableCell td3;

           

            td3 = new TableCell();
            td3.HorizontalAlign = HorizontalAlign.Center;
            td3.VerticalAlign = VerticalAlign.Bottom;
            td3.Text = Str.Integer(total_butir3);
            td3.Attributes["style"] = "Font-Weight:Bold;";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.HorizontalAlign = HorizontalAlign.Center;
            td3.VerticalAlign = VerticalAlign.Bottom;
            td3.Text = Str.Number3(total_carat3);
            td3.Attributes["style"] = "Font-Weight:Bold;";
            tr3.Cells.Add(td3);

            Table3.Rows.Add(tr3);

            // total on selisih
            TableFooterRow tr4 = new TableFooterRow();
            TableCell td4;

          

            td4 = new TableCell();
            td4.HorizontalAlign = HorizontalAlign.Center;
            td4.VerticalAlign = VerticalAlign.Bottom;
            td4.Text = Str.Integer(total_butir4);
            td4.Attributes["style"] = "Font-Weight:Bold;";
            tr4.Cells.Add(td4);

            td4 = new TableCell();
            td4.HorizontalAlign = HorizontalAlign.Center;
            td4.VerticalAlign = VerticalAlign.Bottom;
            td4.Text = Str.Number3(total_carat4);
            td4.Attributes["style"] = "Font-Weight:Bold;";
            tr4.Cells.Add(td4);

            Table4.Rows.Add(tr4);

        }
    }



    
}