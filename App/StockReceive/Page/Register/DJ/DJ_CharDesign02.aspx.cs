using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_CharDesign02 : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            naming.Focus();

            //DLC
            //DLC.CharProcessFinishing(cb);

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DJ.aspx"); }
    protected void LinkError() { Response.Redirect("DJ_CharDesign01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            if (d.Proto != null && d.Proto != 0 && d.Sumber == 1)
            {
                var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (proto != null)
                {
                    naming.Text = proto.ProductOrder.ProductOrder_CharDesign.Naming;
                    setitem.Checked = (proto.ProductOrder.ProductOrder_CharDesign.SetItem == true) ? true : false;
                    special.Checked = (proto.ProductOrder.ProductOrder_CharDesign.Special == true) ? true : false;
                    grafir.Text = proto.ProductOrder.ProductOrder_CharDesign.Grafir;

                    //DLC
                    DLC.CharProcessFinishingFromSPK(cb, Convert.ToInt32(proto.IDProductOrder));
                }
            }
            else if (d.IDMounting.HasValue && d.Sumber == 3)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt != null)
                {
                    naming.Text = mt.StockReceiveMounting_CharDesign.Naming;
                    setitem.Checked = (mt.StockReceiveMounting_CharDesign.SetItem == true) ? true : false;
                    special.Checked = (mt.StockReceiveMounting_CharDesign.Special == true) ? true : false;
                    grafir.Text = mt.StockReceiveMounting_CharDesign.Grafir;

                    //DLC
                    DLC.CharProcessFinishingFromMounting(cb, Convert.ToInt32(mt.ID));
                }
            }
            else
            {
                DLC.CharProcessFinishing(cb);
            }
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            var product = d.StockReceiveDJ_CharProduct;

            var x = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);

            if (d.Sumber == 1)
            {
                var pt = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (pt != null)
                {
                    //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + ".jpg";
                    //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + pt.KodeKaretDJ.KodeKaretDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
                    d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + pt.KodeKaretDJ.Nomor + d.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
                }
            }
            else
            {
                d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + ".jpg";
                //d.ImgPicture = App.ImagesDir.ToString() + "StockReceiveDJ\\" + d.ProtoNomor + d.StockReceiveDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
            }

            //Process
            saveData(d);
            saveFinishing(d);

            if (d.NoValidasi != null)
            {
                moveStoneBRJ(d, x);
            }
            else if (d.Proto != null && d.Proto != 0)
            {
                moveStone(d, x);
            }

            //Recal Weight
            Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("DJ_Stone.aspx?id=" + myID);
        }
    }

    //Data
    protected void saveData(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharDesign;
        if (d == null) LinkError();
        else
        {
            d.Naming = Str.CapTitle(naming.Text);
            d.Grafir = Str.CapUpper(grafir.Text);

            d.SetItem = setitem.Checked;
            d.Special = special.Checked;

            //Commit
            db.SubmitChanges();
        }
    }

    //Finishing
    protected void saveFinishing(StockReceiveDJ d)
    {
        var rs = from p in cb.Items.Cast<ListItem>()
                 select p;

        foreach (var r in rs)
        {
            int myIDFinishing = Convert.ToInt32(r.Value);
            if (r.Selected)
                insertProcess(d, myIDFinishing);
            else
                deleteProcess(d, myIDFinishing);
        }
    }
    protected void insertProcess(StockReceiveDJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            StockReceiveDJ_Finishing n = new StockReceiveDJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.StockReceiveDJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(StockReceiveDJ ms, int myIDFinishing)
    {
        var d = ms.StockReceiveDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }

    //Move Stone
    protected void moveStone(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        moveStone1A(ms,x);
        moveStone1B(ms,x);
        moveStone2(ms,x);
        moveStone3(ms,x);
        moveStone4(ms,x);
        moveStone5(ms,x);
    }

    //moveStoneBRJ
    protected void moveStoneBRJ(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        moveStoneBRJ1A(ms);
        moveStoneBRJ1B(ms);
        moveStoneBRJ2(ms);
        moveStoneBRJ3(ms);
        moveStoneBRJ4(ms);
        moveStoneBRJ5(ms);
    }

    protected void moveStone1A(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone1A
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone1A.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone1A n = new StockReceiveDJ_Stone1A();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone1A.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone1B(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone1B
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone1B.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone1B n = new StockReceiveDJ_Stone1B();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;
                n.GIA = r.GIA;
                n.StatusTerpasang = r.StatusTerpasang;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone1B.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone2(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone2
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone2.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone2 n = new StockReceiveDJ_Stone2();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone2.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone3(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone3
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone3.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone3 n = new StockReceiveDJ_Stone3();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;
                n.GIA = r.GIA;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone3.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone4(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone4
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone4.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone4 n = new StockReceiveDJ_Stone4();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone4.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
    protected void moveStone5(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in db.ProductOrder_Stone5
                    where p.IDForm == x.IDProductOrder
                    orderby p.Stone5.Nama
                    select p;
        if (stone != null)
        {
            foreach (var r in stone)
            {
                StockReceiveDJ_Stone5 n = new StockReceiveDJ_Stone5();

                n.IDForm = ms.ID;
                n.IDStone = r.IDStone;

                n.TotalButir = r.TotalButir;
                n.TotalCarat = r.TotalCarat;
                n.CaratButir = r.CaratButir;

                //Dimensi
                n.DimensiP = r.DimensiP;
                n.DimensiL = r.DimensiL;
                n.DimensiT = r.DimensiT;

                //Commit
                db.StockReceiveDJ_Stone5.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }

    protected void moveStoneBRJ1A(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone1A.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1A.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone1A.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone1A.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                if(r.p.Butir > 0)
                {
                    StockReceiveDJ_Stone1A n = new StockReceiveDJ_Stone1A();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;

                    //Commit
                    db.StockReceiveDJ_Stone1A.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }
    }

    protected void moveStoneBRJ1B(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone1B.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone1B.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone1B.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone1B.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                // kondisi untuk belah stone rapaport qty karena harus diinput GIA
                if ((int)r.p.Butir == 1)
                {
                    StockReceiveDJ_Stone1B n = new StockReceiveDJ_Stone1B();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;
					
					n.GIA = "";

                    //Commit
                    db.StockReceiveDJ_Stone1B.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
                else if ((int)r.p.Butir > 1)
                {
                    decimal caratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    for (int x = 0; x < (int)r.p.Butir; x++)
                    {

                        StockReceiveDJ_Stone1B n = new StockReceiveDJ_Stone1B();

                        n.IDForm = ms.ID;
                        n.IDStone = r.q.ID;

                        n.TotalButir = 1;
                        n.TotalCarat = caratButir;
                        n.CaratButir = caratButir;

                        //Dimensi
                        n.DimensiP = 0;
                        n.DimensiL = 0;
                        n.DimensiT = 0;
						
						n.GIA = "";

                        //Commit
                        db.StockReceiveDJ_Stone1B.InsertOnSubmit(n);
                        db.SubmitChanges();

                    }

                }
            }
        }
    }

    protected void moveStoneBRJ2(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone2.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone2.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone2.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone2.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                if(r.p.Butir > 0)
                {
                    StockReceiveDJ_Stone2 n = new StockReceiveDJ_Stone2();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;

                    //Commit
                    db.StockReceiveDJ_Stone2.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }
    }

    protected void moveStoneBRJ3(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone3.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone3.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone3.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone3.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                if(r.p.Butir > 0)
                {
                    StockReceiveDJ_Stone3 n = new StockReceiveDJ_Stone3();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;

                    //Commit
                    db.StockReceiveDJ_Stone3.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }
    }

    protected void moveStoneBRJ4(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone4.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone4.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone4.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone4.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                if(r.p.Butir > 0)
                {
                    StockReceiveDJ_Stone4 n = new StockReceiveDJ_Stone4();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;

                    //Commit
                    db.StockReceiveDJ_Stone4.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }
    }

    protected void moveStoneBRJ5(StockReceiveDJ ms)
    {
        var z = db.StockReceiveDJ_Stone5.Where(p => p.IDForm == ms.ID);
        if (z != null)
        {
            db.StockReceiveDJ_Stone5.DeleteAllOnSubmit(z);
            db.SubmitChanges();
        }

        var stone = from p in cs.T_brj_stone.AsEnumerable()
                    join q in db.Stone5.AsEnumerable() on p.Parcel equals q.Nama
                    where p.NoValidasi == ms.NoValidasi
                        && (from y in db.Stone5.AsEnumerable() select y.Nama).Contains(p.Parcel)
                    orderby p.Parcel
                    select new { p, q };
        if (stone != null)
        {
            foreach (var r in stone)
            {
                if(r.p.Butir > 0)
                {
                    StockReceiveDJ_Stone5 n = new StockReceiveDJ_Stone5();

                    n.IDForm = ms.ID;
                    n.IDStone = r.q.ID;

                    n.TotalButir = (int)r.p.Butir;
                    n.TotalCarat = (decimal)r.p.Carat;
                    n.CaratButir = (decimal)r.p.Carat / (decimal)r.p.Butir;

                    //Dimensi
                    n.DimensiP = 0;
                    n.DimensiL = 0;
                    n.DimensiT = 0;

                    //Commit
                    db.StockReceiveDJ_Stone5.InsertOnSubmit(n);
                    db.SubmitChanges();
                }
            }
        }
    }


}