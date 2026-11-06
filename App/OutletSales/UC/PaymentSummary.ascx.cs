using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UC_PaymentSummary : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        summary_nilaiso.InnerText = _sebelumdiscount;
        summary_totaldiscount.InnerText = _totaldiscount;
        summary_totalharusdibayar.InnerText = _totalharusdibayar;
        summary_sebelum_tradein.InnerText = _sebelumtradein;
        summary_resell_tradein.InnerText = _nilaitradein;
        summary_totalharusdibayar_setelahtradein.InnerText = _sisaharusdibayar;
        summary_totalpembayaran.InnerText = _totalterbayar;
        jmlh_status_pembayaran.InnerHtml = _statuspembayaran;

        div_row_tradein.Visible = (_nilaitradein == "" || _nilaitradein == "0") ? false : true;
    }


    private string _sebelumdiscount;
    private string _totaldiscount;
    private string _totalharusdibayar;
    private string _sebelumtradein;
    private string _nilaitradein;
    private string _sisaharusdibayar;
    private string _totalterbayar;
    private string _statuspembayaran;
    
    public string SebelumDiscount
    {
        get { return _sebelumdiscount; }
        set { _sebelumdiscount = value; }   
    }

    public string TotalDiscount
    {
        get { return _totaldiscount; }
        set { _totaldiscount = value; }
    }

    public string TotalHarusDiBayar
    {
        get { return _totalharusdibayar; }
        set { _totalharusdibayar = value; }
    }

    public string SebelumTradeIn
    {
        get { return _sebelumtradein; }
        set { _sebelumtradein = value; }
    }

    public string NilaiTradeIn
    {
        get { return _nilaitradein; }
        set { _nilaitradein = value; }
    }

    public string SisaHarusDiBayar
    {
        get { return _sisaharusdibayar; }
        set { _sisaharusdibayar = value; }
    }

    public string TotalTerbayar
    {
        get { return _totalterbayar; }
        set { _totalterbayar = value; }
    }

    public string StatusPembayaran
    {
        get { return _statuspembayaran; }
        set { _statuspembayaran = value; }
    }
}