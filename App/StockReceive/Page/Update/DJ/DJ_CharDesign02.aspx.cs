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
        var d = db.StockReceiveDJ_CharDesign.SingleOrDefault(p => p.ID == myID && p.StockReceiveDJ.Approval==null);
        if (d == null)
        {
            var x = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            if (x == null) DataError();
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + x.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + x.Nomor + "</a>";
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.StockReceiveDJ.Nomor + "</a>";


            naming.Text = d.Naming;
            setitem.Checked = (d.SetItem == true) ? true : false;
            special.Checked = (d.Special == true) ? true : false;
            grafir.Text = d.Grafir;



            DLC.CharProcessFinishingFromDesign(cb,d.ID);
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) DataError();
        else
        {
            var product = d.StockReceiveDJ_CharProduct;

            var x = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);

            //Process
            saveData(d);
            saveFinishing(d);
            /*if (d.Proto != null && d.Proto != 0)
                moveStone(d, x);*/

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
        moveStone1A(ms, x);
        moveStone1B(ms, x);
        moveStone2(ms, x);
        moveStone3(ms, x);
        moveStone4(ms, x);
        moveStone5(ms, x);
    }
    protected void moveStone1A(StockReceiveDJ ms, KodeKaret_Detail x)
    {
        var z = db.StockReceiveDJ_Stone1A.Where(p => p.ID == ms.ID);
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
        var z = db.StockReceiveDJ_Stone1B.Where(p => p.ID == ms.ID);
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
        var z = db.StockReceiveDJ_Stone2.Where(p => p.ID == ms.ID);
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
        var z = db.StockReceiveDJ_Stone3.Where(p => p.ID == ms.ID);
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
        var z = db.StockReceiveDJ_Stone4.Where(p => p.ID == ms.ID);
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
        var z = db.StockReceiveDJ_Stone5.Where(p => p.ID == ms.ID);
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
}