using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

public partial class KodeKaretCleansingAddDetailInfo : System.Web.UI.Page
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

    protected int revmin = 6;
    protected int revmax = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            fillData();
        }

    }

    protected void fillData()
    {
        if (revmin < 3) { revmin = 3; } // min 3
        if (revmax < revmin) { revmax = revmin; }

        btnCopy.Disabled = true;
                        //<li><i>revisi kode karet maksimal 7 digit</i></li>
        lit_rule_1.Text = "<li><i>Total panjang karakter Kode Karet revisi harus berjumlah " + revmin.ToString() + (revmin < revmax ? (" - " + revmax.ToString()) : "") + " karakter</i></li>";

        if (myID > 0)
        {
            var d = db.KodeKaretCleansing_Detail.SingleOrDefault(p => p.ID == myID);

            if (d != null)
            {
                if (d.KodeKaret.Trim() != "") 
                {
                    btnCopy.Disabled = !(d.KodeKaret.Length <= 7);
                }

                txt_kodekaretjaws.Text = d.ProtoNomor;
                txt_kodekaretrecommendation.Text = d.KodeKaret;
                txt_kodekaretrevisi.Text = (d.KodeKaretRev != null) ? d.KodeKaretRev : "";

                span_jaws.InnerText = (d.ProtoNomor != null) ? d.ProtoNomor.Length.ToString() : "0";
                span_recommendation.InnerText = (d.KodeKaret != null) ? d.KodeKaret.Length.ToString() : "0";
                span_revision.InnerText = (d.KodeKaretRev != null) ? d.KodeKaretRev.Length.ToString() : "0";

                if (d.ImgPictureBRJ != "") { lit_img_brj.Text = "<img src='/Images.aspx?path=" + d.ImgPictureBRJ + "' class='img-thumbnail' style='width:100%'>"; }
                if (d.ImgPictureSPK != "") { lit_img_spk.Text = "<img src='/Images.aspx?path=" + d.ImgPictureSPK + "' class='img-thumbnail' style='width:100%'>"; }
            }
            else
            {
                DataError();
            }
        }
        else
        {
            DataError();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("KodeKaretCleansing.aspx"); }

    //btnCopy_Click
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        var d = db.KodeKaretCleansing_Detail.Single(p => p.ID == myID);

        if (d != null)
        {
            d.KodeKaretRev = d.KodeKaret;

            d.Operator = App.UserNama;
            d.OperatorTgl = System.DateTime.Now;

            db.SubmitChanges();
        }

        Response.Redirect("KodeKaretCleansingAddDetailInfo.aspx?id=" + myID);
    }

    //cancel_Click
    protected void cancel_Click(object sender, EventArgs e)
    {
        App.ReloadClose(this);
    }

    //save_Click
    protected void save_Click(object sender, EventArgs e)
    {
        string xrevisi = Str.CapUpper(txt_kodekaretrevisi.Text.Trim());

        if (revmin < 3) { revmin = 3; } // min 3
        if (revmax < revmin) { revmax = revmin; }

        var d = db.KodeKaretCleansing_Detail.Single(p => p.ID == myID);

        if (d != null)
        {
            if (xrevisi.Length > 0)
            {
                if (xrevisi.Length >= revmin && xrevisi.Length <= revmax)
                {
                    d.KodeKaretRev = xrevisi;

                    d.Operator = App.UserNama;
                    d.OperatorTgl = System.DateTime.Now;

                    db.SubmitChanges();

                    App.ReloadClose(this);
                }
                else
                {
                    txt_kodekaretrevisi.Text = (d.KodeKaretRev != null) ? d.KodeKaretRev : "";
                    span_revision.InnerText = (d.KodeKaretRev != null) ? d.KodeKaretRev.Length.ToString() : "0";

                    //span_revision.InnerText = xrevisi.Length.ToString();

                    err.Visible = true;
                    lit_err.Visible = true;
                    lit_err.Text = "Kode Karet '<b>" + xrevisi + "</b>' tidak dapat diinput. Total panjang karakter Kode Karet revisi harus berjumlah " + revmin.ToString() + (revmin < revmax ? (" - " + revmax.ToString()) : "") + " karakter";

                }
            }
            else
            {
                d.KodeKaretRev = null;

                d.Operator = null;
                d.OperatorTgl = null;

                db.SubmitChanges();

                App.ReloadClose(this);
            }
        }
    }
}