using System;
using System.Linq;

public class Lib {
	//Recalculate

    public static void RecalWeight(Map_DB db, ProductDesign d, ProductDesign_CharProduct product)
    {
        //Master
        decimal totalCarat = 0;

        //Stone
        var rs_1A = from p in d.ProductDesign_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) totalCarat += r;

        var rs_1B = from p in d.ProductDesign_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) totalCarat += r;

        var rs_2 = from p in d.ProductDesign_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) totalCarat += r;

        var rs_3 = from p in d.ProductDesign_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) totalCarat += r;

        var rs_4 = from p in d.ProductDesign_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) totalCarat += r;

        var rs_5 = from p in d.ProductDesign_Stone5 select p.TotalCarat;
        foreach (var r in rs_5) totalCarat += r;

        //Save to Database
        product.StoneCarat = totalCarat;
        product.StoneWeight = totalCarat * (decimal)0.2;
        product.NettoWeight = product.GrossWeight - product.StoneWeight;

        //Commit
        db.SubmitChanges();
    }
}