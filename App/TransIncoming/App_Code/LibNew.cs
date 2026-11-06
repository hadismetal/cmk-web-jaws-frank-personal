using System;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

/// <summary>
/// Common Function
/// </summary>
public partial class LibNew
{
    //Item List DJ
    public static string ProductList_DJ(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross, string Butir, string Carat, string Harga)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:105px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " )</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Butir + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Carat + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Harga + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_DJ(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross, string Butir, string Carat, string Harga, string Lokasi)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:105px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " ) <br />" + Lokasi + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Butir + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Carat + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Harga + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_GJ(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " )</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_GJ(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross, string Lokasi)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " )<br />" + Lokasi + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_LD(string Nomor, string PathImage, string Status, string Carat)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + 1 + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Carat + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_LD(string Nomor, string PathImage, string Status, string Carat, string Lokasi)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + "<br />" + Lokasi + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "</div><div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>1<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Carat + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    //Item List PG
    public static string ProductList_PG(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross, string Nama)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " )</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 300px;'>" + Nama + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_PG(string ProductItem, string Nomor, string PathImage, string Status, string Netto, string Gross, string Nama, string Lokasi)
    {
        string x;
        x = "<div style='width: 1100px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + PathImage + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='float: left; width: 200px;'>" + Nomor + " ( " + ProductItem + " )<br />" + Lokasi + "</div>"
            + "<div style='float: left; width: 120px;'>" + Status + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Netto + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 120px;'>" + Gross + "<div style='clear:both;'></div></div>"
            + "<div style='float: left; width: 300px;'>" + Nama + "<div style='clear:both;'></div></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; height: 1px;'>&nbsp;</div>"
            ;
        return x;
    }
    public static string ProductList_Packaging(string KodeLabel)
    {
        string x;
        x = "<div>"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Souvenir(string KodeLabel)
    {
        string x;
        x = "<div>"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Packaging(string KodeLabel, string qty)
    {
        string x;
        x = "<div>"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Souvenir(string KodeLabel, string qty)
    {
        string x;
        x = "<div>"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Packaging(string KodeLabel, string qty, string id)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px; display:table-cell;'>"
            + "<div style='float: left; width: 200px;'>" + KodeLabel + "<div style='clear:both;'></div></div>"

            + "<div style='float: left; width: 120px;'>" + qty + "<div style='clear:both;'></div></div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : <input readonly=\"readonly\" size=\"6\" class=\"span2\" type=\"text\" name=\"qty_" + id + "\" value=\"" + qty + "\" /></div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Souvenir(string KodeLabel, string qty, string id)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : <input readonly=\"readonly\" size=\"6\" class=\"span2\" type=\"text\" name=\"qty_" + id + "\" value=\"" + qty + "\" /></div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Packaging(string KodeLabel, string id, string qty, string qty_rcv, string qty_dmg, bool StockIncoming)
    {
        string x;
        x = "<div class=\"form-inline\" id='" + id + "'>"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Terima : <input style=\"max-width:75px\" size=\"6\" class=\"span2\" type=\"number\" name=\"qty_rcv" + id + "\" value=\"" + qty_rcv + "\" /></div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Rusak : <input style=\"max-width:75px\" size=\"6\" class=\"span2\" type=\"number\" name=\"qty_dmg" + id + "\" value=\"" + qty_dmg + "\" /></div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Souvenir(string KodeLabel, string id, string qty, string qty_rcv, string qty_dmg, bool StockIncoming)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Terima : <input style=\"max-width:75px\" size=\"6\" class=\"span2\" type=\"number\" name=\"qty_rcv" + id + "\" value=\"" + qty_rcv + "\" /></div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Rusak : <input style=\"max-width:75px\" size=\"6\" class=\"span2\" type=\"number\" name=\"qty_dmg" + id + "\" value=\"" + qty_dmg + "\" /></div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Packaging(string KodeLabel, string id, string qty, string qty_rcv, string qty_dmg, string qty_lst, bool StockIncoming, bool finish)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Terima : " + qty_rcv + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Rusak : " + qty_dmg + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Selisih : " + qty_lst + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }
    public static string ProductList_Souvenir(string KodeLabel, string id, string qty, string qty_rcv, string qty_dmg, string qty_lst, bool StockIncoming, bool finish)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='border-bottom:solid 1px black;'>" + KodeLabel + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Qty : " + qty + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Terima : " + qty_rcv + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Rusak : " + qty_dmg + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>Selisih : " + qty_lst + "</div>"
            + "</div>"
            + "</div>"
            ;
        return x;
    }

    public static string PackList(string pic, string nama, string kode, string qtyin, string qtyout, string id)
    {
        string x;
        x = "<div style='width: 850px;'>"
            + "<div style='padding:5px; display: table-cell;'>"
            + "<div style='float: left; width: 150px;'><img src='/Images.aspx?path=" + pic + "' style='max-width:95px; max-height:85px' /></div>"
            + "<div style='float: left; width: 195px;'>" + nama + "</div>"
            + "<div style='float: left; width: 200px;'>" + kode + "</div>"
            + "<div style='float: left; width: 150px;'>" + qtyin + "</div>"
            + "<div style='float: left; width: 130px;'><input readonly=\"readonly\" size=\"6\" class=\"span2\" type=\"text\" name=\"qty_" + id + "\" value=\"" + qtyout + "\" /></div>"
            + "</div>"
            + "</div>"
            + "<div style='clear: both; font-size:1px; width:5px;'>&nbsp;</div>"
            ;
        return x;
    }

}