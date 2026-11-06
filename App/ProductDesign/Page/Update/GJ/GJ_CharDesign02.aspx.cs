using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_GJ_CharDesign02 : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("GJ.aspx"); }
    protected void LinkError() { Response.Redirect("GJ_CharDesign01.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.ProductDesignGJ_CharDesign.SingleOrDefault(p => p.ID == myID && p.ProductDesignGJ.Approval==null);
        if (d == null)
        {
            var x = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            if (x == null) DataError();
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + x.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + x.Nomor + "</a>";
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.ProductDesignGJ.Nomor + "</a>";


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
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) DataError();
        else
        {
            var product = d.ProductDesignGJ_CharProduct;

            //Process
            saveData(d);
            saveFinishing(d);
            /*if (d.Proto != null && d.Proto != 0)
                moveStone(d, x);*/

            //Recal Weight
            //Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("GJ_Picture.aspx?id=" + myID);
        }
    }

    //Data
    protected void saveData(ProductDesignGJ ms)
    {
        var d = ms.ProductDesignGJ_CharDesign;
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
    protected void saveFinishing(ProductDesignGJ d)
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
    protected void insertProcess(ProductDesignGJ ms, int myIDFinishing)
    {
        var d = ms.ProductDesignGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            ProductDesignGJ_Finishing n = new ProductDesignGJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.ProductDesignGJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(ProductDesignGJ ms, int myIDFinishing)
    {
        var d = ms.ProductDesignGJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.ProductDesignGJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}