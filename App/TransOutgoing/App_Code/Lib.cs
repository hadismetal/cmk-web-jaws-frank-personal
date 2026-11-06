using System;
using System.Web.UI.WebControls;
using System.Linq;

public partial class Lib
{

    //Recalculate
    public static void RecalWeight(Map_DB db, DocRepair d, DocRepair_CharProduct product)
    {
        //Master
        decimal totalCarat = 0;

        //Stone
        var rs_1A = from p in d.DocRepair_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) totalCarat += r;

        var rs_1B = from p in d.DocRepair_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) totalCarat += r;

        var rs_2 = from p in d.DocRepair_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) totalCarat += r;

        var rs_3 = from p in d.DocRepair_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) totalCarat += r;

        var rs_4 = from p in d.DocRepair_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) totalCarat += r;

        var rs_5 = from p in d.DocRepair_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) totalCarat += r;

        var rs2_1A = from p in d.StockProductDJ.StockProductDJ_Stone1A select p.TotalCarat;
        foreach (var r in rs2_1A) totalCarat += r;

        var rs2_1B = from p in d.StockProductDJ.StockProductDJ_Stone1B select p.TotalCarat;
        foreach (var r in rs2_1B) totalCarat += r;

        var rs2_2 = from p in d.StockProductDJ.StockProductDJ_Stone2 select p.TotalCarat;
        foreach (var r in rs2_2) totalCarat += r;

        var rs2_3 = from p in d.StockProductDJ.StockProductDJ_Stone3 select p.TotalCarat;
        foreach (var r in rs2_3) totalCarat += r;

        var rs2_4 = from p in d.StockProductDJ.StockProductDJ_Stone4 select p.TotalCarat;
        foreach (var r in rs2_4) totalCarat += r;

        var rs2_5 = from p in d.StockProductDJ.StockProductDJ_Stone5 select p.TotalCarat;
        foreach (var r in rs2_5) totalCarat += r;

        //Save to Database
        product.StoneCarat = totalCarat;
        product.StoneWeight = totalCarat * (decimal)0.2;
        product.NettoWeight = product.GrossWeight - product.StoneWeight;

        //Commit
        db.SubmitChanges();
    }

    public static void RecalWeight(Map_DB db, StockProductDJ d, StockProductDJ_CharProduct product)
    {
        //Master
        decimal totalCarat = 0;

        //Stone
        var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) totalCarat += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) totalCarat += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) totalCarat += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) totalCarat += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) totalCarat += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) totalCarat += r;

        var rs2_1A = from p in d.StockProductDJ_Stone1A select p.TotalCarat;
        foreach (var r in rs2_1A) totalCarat += r;

        var rs2_1B = from p in d.StockProductDJ_Stone1B select p.TotalCarat;
        foreach (var r in rs2_1B) totalCarat += r;

        var rs2_2 = from p in d.StockProductDJ_Stone2 select p.TotalCarat;
        foreach (var r in rs2_2) totalCarat += r;

        var rs2_3 = from p in d.StockProductDJ_Stone3 select p.TotalCarat;
        foreach (var r in rs2_3) totalCarat += r;

        var rs2_4 = from p in d.StockProductDJ_Stone4 select p.TotalCarat;
        foreach (var r in rs2_4) totalCarat += r;

        var rs2_5 = from p in d.StockProductDJ_Stone5 select p.TotalCarat;
        foreach (var r in rs2_5) totalCarat += r;

        //Save to Database
        product.StoneCarat = totalCarat;
        product.StoneWeight = totalCarat * (decimal)0.2;
        product.NettoWeight = product.GrossWeight - product.StoneWeight;

        //Commit
        db.SubmitChanges();
    }

    public static string PackList(string pic, string nama, string kode, string qtyin, string qtyout)
    {
        string x;
        x = "<div class=\"form-inline\">"
            + "<div style='padding:5px;'>"
            + "<div style='float: left; width: 110px;'><img src='/Images.aspx?path=" + pic + "' style='max-width:85px; max-height:65px' /></div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>" + nama + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>" + kode + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>" + qtyin + "</div>"
            + "<div style='border-bottom:solid 1px black; padding: 5px 0px;'>" + qtyout + "</div>"
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
}