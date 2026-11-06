using System;

public class TableList
{
    public static string ProductList_DJ(int index
        , string ProductItem, string Nomor, string PathImage, string Status
        , string Netto, string Gross, string Butir, string Carat
        , string Harga, string Lokasi
        , string errMsg = ""
        , bool isPromoActive = false, bool isPromoUpcoming = false
        )
    {
        string xcolor = "default";
        string xcolor2 = "";
        string xcolor3 = "";
        string xcolor4 = "";

        if (Status.ToLower() != "jual")
        {
            xcolor = "warning";
            xcolor2 = "style='background-color:#fbeed5;'";
            xcolor3 = "style='color:red;'";
        }

        if (isPromoActive || isPromoUpcoming)
        {
            if (isPromoActive) { Status = "Promo Aktif !!!"; xcolor3 = "style='color:red;'"; }
            if (isPromoUpcoming) { Status = "Promo Upcoming !!!"; xcolor3 = "style='color:red;'"; }

            xcolor = "danger";
            xcolor2 = "style='background-color:#fbeed5;'";
            //xcolor4 = "style='color:red;'";
        }
        
        string x;
        x = "";
        x += "<div class='panel panel-" + xcolor + "' " + xcolor2 + " >";
        x += "<div class='panel-body'>";
        ////////////////////////////////// end panel start
        x += "<div class='row'>"; //////////////////////////////////// row 1 : start
        x += "<div class='col-xs-12 col-sm-12'>"; //////////////////////////////////// col head 1 left : start
        x += "<div class='row'>";
        x += "<div class='col-xs-12 col-sm-1 text-center'></div>";
        //////////////////////////////////
        x += "<div class='col-xs-12 col-sm-1 text-center'>" + index.ToString() + ".</div>";
        //////////////////////////////////
        x += "<div class='col-xs-12 col-sm-2 text-center'>";
        x += "<span class='label label-" + xcolor + " label-sm ' " + xcolor4 + " >" + Nomor + "</span>";
        x += "<img src='/Images.aspx?path=" + PathImage + "' class='img-thumbnail' style='margin-top:3px;' />";
        x += "</div>";
        //////////////////////////////////
        x += "<div class='col-xs-12 col-sm-2 '>";
        x += "<table>";
        x += "<tr><td class='text-bold'>Netto</td><td>&nbsp;:&nbsp;</td><td>" + Netto + "</td></tr>";
        x += "<tr><td class='text-bold'>Gross</td><td>&nbsp;:&nbsp;</td><td>" + Gross + "</td></tr>";
        x += "<tr><td class='text-bold'>Butir</td><td>&nbsp;:&nbsp;</td><td>" + Butir + "</td></tr>";
        x += "<tr><td class='text-bold'>Carat</td><td>&nbsp;:&nbsp;</td><td>" + Carat + "</td></tr>";
        x += "</table>";
        x += "</div>";
        //////////////////////////////////
        x += "<div class='col-xs-12 col-sm-6 '>";
        x += "<table>";
        x += "<tr><td class='text-bold'>Status</td><td>&nbsp;:&nbsp;</td><td " + xcolor3 + ">" + Status + "</td></tr>";
        x += "<tr><td class='text-bold'>Lokasi</td><td>&nbsp;:&nbsp;</td><td>" + Lokasi + "</td></tr>";
        x += "<tr><td class='text-bold'>Item</td><td>&nbsp;:&nbsp;</td><td>" + ProductItem + "</td></tr>";
        x += "<tr><td class='text-bold'>Harga</td><td>&nbsp;:&nbsp;</td><td>" + Harga + "</td></tr>";
        x += "</table>";
        x += "</div>";
        x += "</div>";
        //////////////////////////////////
        x += "</div>"; //////////////////////////////////// col head 1 left : end
        //x += "<div class='col-xs-12 col-sm-12'>"; //////////////////////////////////// col head 2 mid : start
        //x += "<div class='row'>";
        //x += (isPromoActive) ? "<div class='col-xs-12 col-sm-12 text-center'><div class='alert alert-danger' " + xcolor4 + " role='alert'><b>Promo Aktif !!!</b><br />" + errMsg + "</div></div>" : "";
        //x += (isPromoUpcoming) ? "<div class='col-xs-12 col-sm-12 text-center'><div class='alert alert-danger' " + xcolor4 + " role='alert'><b>Promo Upcoming !!!</b><br />" + errMsg + "</div></div>" : "";
        //x += "</div>";

        //x += "</div>"; //////////////////////////////////// col head 2 mid : end
        x += "</div>"; //////////////////////////////////// row 1 : end
        ////////////////////////////////// end panel close
        x += "</div>";
        x += "</div>";
        ;

        return x;
    }

    public static string ProductHeader_DJ(string Text = "")
    {
        string x;
        x = "";
        x += "<div class='panel panel-default' style='background-color:#ddd; text-bold'>";
        x += "<div class='panel-body'>";
        x += "<div class='row'>";
        x += "<div class='col-xs-12 col-sm-12'>"; ///////////////////////// 1
        x += "<div class='row'>";
        //x += "<div class='col-xs-12 col-sm-1 text-center'></div>";
        //x += "<div class='col-xs-12 col-sm-1 text-center'>No.</div>";
        //x += "<div class='col-xs-12 col-sm-2 text-center'>PLU</div>";
        //x += "<div class='col-xs-12 col-sm-8 text-center'>Keterangan</div>";
        // <span class="glyphicon glyphicon-align-left" aria-hidden="true"></span>
        x += "<div class='col-xs-12 col-sm-12 text-center'>" + Text + "</div>";
        x += "<div class='col-xs-12 col-sm-12 text-center'><span style='font-size:18px;' class='glyphicon glyphicon-chevron-down' aria-hidden='true'></span></div>";
        x += "</div>";
        x += "</div>"; ///////////////////////// 1
        //x += "<div class='col-xs-12 col-sm-12'>"; ///////////////////////// 2
        //x += "</div>"; ///////////////////////// 2
        x += "</div>";
        x += "</div>";
        x += "</div>";
        ;

        return x;
    }


    public static string ProductAffected_Header_DJ(string NamaPromo
        , string ApprovalNama
        , string ApprovalTgl
        , string TglStart
        , string TglExpire
        , string TipePromo
        , string HargaPromo
        , string DiscountNominal
        , string DiscountPersen
        , string StatusPromo
        , string IDPromo
        )
    {
        string xStatusPromo = "";

        if (StatusPromo == "ONGOING") { xStatusPromo = "<span class='label label-danger'>" + StatusPromo + "</span>"; }
        else if (StatusPromo == "UPCOMING") { xStatusPromo = "<span class='label label-warning'>" + StatusPromo + "</span>"; }
        else { xStatusPromo = "<span class='label label-success'>" + StatusPromo + "</span>"; }

        string x;
        x = "";
        x += "<div class='panel panel-default' style='background-color:#fff; margin-top:25px;' >";
        x += "<div class='panel-body'>";

        x += "<div class='row'>";
        x += "<div class='col-xs-12 col-sm-12'>"; ///////////////////////// HT
        x += "<b><u>" + NamaPromo + "</u></b> " + xStatusPromo + "";
        x += (StatusPromo == "ONGOING") ? TD_HyperLink_Expire(IDPromo) : "";
        x += "</div>"; ///////////////////////// H
        x += "<div class='col-xs-12 col-sm-6'>"; ///////////////////////// H
        x += "<table>";
        //x += "<tr><td>Nama Promo</td><td>&nbsp;:&nbsp;</td><td>" + NamaPromo + "</td></tr>";
        x += "<tr><td>Approval Nama</td><td>&nbsp;:&nbsp;</td><td>" + ApprovalNama + "</td></tr>";
        x += "<tr><td>Approval Tgl</td><td>&nbsp;:&nbsp;</td><td>" + ApprovalTgl + "</td></tr>";
        x += "<tr><td>Tgl Start</td><td>&nbsp;:&nbsp;</td><td>" + TglStart + "</td></tr>";
        x += "<tr><td>Tgl Expire</td><td>&nbsp;:&nbsp;</td><td>" + TglExpire + "</td></tr>";

        x += "</table>";

        x += "</div>"; ///////////////////////// H
        x += "<div class='col-xs-12 col-sm-6'>"; ///////////////////////// H
        x += "<table>";
        x += "<tr><td>Tipe Promo</td><td>&nbsp;:&nbsp;</td><td>" + TipePromo + "</td></tr>";
        x += "<tr><td>Harga Promo</td><td>&nbsp;:&nbsp;</td><td>" + HargaPromo + "</td></tr>";
        x += "<tr><td>Discount Nominal</td><td>&nbsp;:&nbsp;</td><td>" + DiscountNominal + "</td></tr>";
        x += "<tr><td>Discount Persen</td><td>&nbsp;:&nbsp;</td><td>" + DiscountPersen + "</td></tr>";
        //x += "<tr><td>Status Promo</td><td>&nbsp;:&nbsp;</td><td>" + StatusPromo + "</td></tr>";
        x += "</table>";
        x += "</div>"; ///////////////////////// H
        x += "<div class='col-xs-12 col-sm-12' style='border-bottom:solid 1px #ddd;'>&nbsp;</div>"; ///////////////////////// HORIZONTAL
        x += "<div class='col-xs-12 col-sm-12'>"; ///////////////////////// 1
        ;

        return x;
    }


    protected static string TD_HyperLink_Expire(string IDPromo)
    {
        /// refreshPage=1&kw=&dari=01-11-2015&sampai=09-11-2015&status=0&tipelokasi=-1&lokasi=-1

        string x = string.Empty;
        string href = string.Empty;
        //fast-backward
        href = "/OutletSupport/Page/Report/DJ/DJ_Expire.aspx?id=" + IDPromo;

        x += "<a target='_BLANK' class=\"btn btn-warning btn-xs pull-right\" href=\"" + href + "\">";
        x += "  <span class=\"glyphicon glyphicon-time\" aria-hidden=\"true\"> </span> ";
        x += "  ";
        x += "</a>";

        return x;
    }


    public static string ProductAffected_Data_DJ(string Index, string PLU, string ProtoNomor, string ProductItem, string StatusBarang, string Flag)
    {
        string xFlag = "";
        
        if (StatusBarang == "0") { StatusBarang = "-"; }
        else { StatusBarang = DL.SubStorage(Convert.ToInt32(StatusBarang)); xFlag = "text-green"; }

        if (Flag == "1")
        {
            xFlag = "text-red";
        }

        string x;
        x = "";
        
        x += "<div class='row' style='border-bottom:solid 1px #EEE;'>";
        x += "<div class='col-xs-12 col-sm-1 text-center " + xFlag + "'>" + Index + ".</div>";
        x += "<div class='col-xs-12 col-sm-2 text-center " + xFlag + "'>" + PLU + "</div>";
        x += "<div class='col-xs-12 col-sm-3 text-center " + xFlag + "'>" + ProtoNomor + "</div>";
        x += "<div class='col-xs-12 col-sm-3 text-center " + xFlag + "'>" + ProductItem + "</div>";
        x += "<div class='col-xs-12 col-sm-3 text-center " + xFlag + "'>" + StatusBarang + "</div>";
        x += "</div>";
        
        ;

        return x;
    }

    public static string ProductAffected_Footer_DJ()
    {
        string x;
        x = "";
        x += "</div>"; ///////////////////////// 1
        x += "</div>";
        x += "</div>";
        x += "</div>";
        ;

        return x;
    }
}