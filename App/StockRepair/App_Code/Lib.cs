using System;
using System.Linq;

public class Lib {
	//Recalculate
	public static void RecalWeight(Map_DB db, DocRepair d, DocRepair_CharProduct product) {
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
		product.StoneWeight = totalCarat * (decimal) 0.2;
		product.NettoWeight = product.GrossWeight - product.StoneWeight;

		//Commit
		db.SubmitChanges();
	}

    public static void RecalWeight(Map_DB db, DocRepairResult d, DocRepairResult_CharProduct product)
    {
        //Master
        decimal totalCarat = 0;

        //Stone
        var rs_1A = from p in d.DocRepairResult_Stone1A select p.TotalCarat;
        foreach (var r in rs_1A) totalCarat += r;

        var rs_1B = from p in d.DocRepairResult_Stone1B select p.TotalCarat;
        foreach (var r in rs_1B) totalCarat += r;

        var rs_2 = from p in d.DocRepairResult_Stone2 select p.TotalCarat;
        foreach (var r in rs_2) totalCarat += r;

        var rs_3 = from p in d.DocRepairResult_Stone3 select p.TotalCarat;
        foreach (var r in rs_3) totalCarat += r;

        var rs_4 = from p in d.DocRepairResult_Stone4 select p.TotalCarat;
        foreach (var r in rs_4) totalCarat += r;

        var rs_5 = from p in d.DocRepairResult_Stone5 select p.TotalCarat;
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
        var rs_1A = from p in d.StockProductDJ_Stone1A select new { p.TotalCarat , p.TotalButir };
        var rs_1B = from p in d.StockProductDJ_Stone1B select new { p.TotalCarat , p.TotalButir };
        var rs_2 = from p in d.StockProductDJ_Stone2 select new { p.TotalCarat , p.TotalButir };
        var rs_3 = from p in d.StockProductDJ_Stone3 select new { p.TotalCarat , p.TotalButir };
        var rs_4 = from p in d.StockProductDJ_Stone4 select new { p.TotalCarat , p.TotalButir };
        var rs_5 = from p in d.StockProductDJ_Stone5 select new { p.TotalCarat , p.TotalButir };
        
        product.StoneCarat = (rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalCarat) : 0)
                       + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalCarat) : 0)
                       + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalCarat) : 0)
                       + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalCarat) : 0)
                       + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalCarat) : 0)
                       + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalCarat) : 0)
                       ;
        product.StoneWeight = product.StoneCarat * (decimal)0.2;
        product.NettoWeight = product.GrossWeight - product.StoneWeight;
        product.StoneQty = (rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalButir) : 0)
                       + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalButir) : 0)
                       + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalButir) : 0)
                       + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalButir) : 0)
                       + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalButir) : 0)
                       + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalButir) : 0)
                       ;
        //Commit
        db.SubmitChanges();
    }

    public static void RecalWeightStockResult(Map_DB db, StockProductDJ d, StockProductDJ_CharProduct dcp)
    {
        var rs_1A = from p in d.StockProductDJ_Stone1A select new { p.TotalCarat, p.TotalButir };
        var rs_1B = from p in d.StockProductDJ_Stone1B select new { p.TotalCarat, p.TotalButir };
        var rs_2 = from p in d.StockProductDJ_Stone2 select new { p.TotalCarat, p.TotalButir };
        var rs_3 = from p in d.StockProductDJ_Stone3 select new { p.TotalCarat, p.TotalButir };
        var rs_4 = from p in d.StockProductDJ_Stone4 select new { p.TotalCarat, p.TotalButir };
        var rs_5 = from p in d.StockProductDJ_Stone5 select new { p.TotalCarat, p.TotalButir };

        decimal sc = 0m;
        decimal sw = 0m;
        decimal nw = 0m;
        decimal sq = 0m;

        sc = sc + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalCarat) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalCarat) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalCarat) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalCarat) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalCarat) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalCarat) : 0))
                    ;
        sw = sw + ((decimal)dcp.StoneCarat * (decimal)0.2);
        nw = nw + ((decimal)dcp.GrossWeight - (decimal)dcp.StoneWeight);
        sq = sq + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalButir) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalButir) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalButir) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalButir) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalButir) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalButir) : 0))
                    ;

        dcp.StoneCarat = sc;
        dcp.StoneWeight = sw;
        dcp.NettoWeight = nw;
        dcp.StoneQty = sq;

        //Commit
        db.SubmitChanges();
    }


    public static void RecalWeightRepair(Map_DB db, DocRepair d, DocRepair_CharProduct dcp)
    {
        var rs_1A = from p in d.DocRepair_Stone1A select new { p.TotalCarat, p.TotalButir };
        var rs_1B = from p in d.DocRepair_Stone1B select new { p.TotalCarat, p.TotalButir };
        var rs_2 = from p in d.DocRepair_Stone2 select new { p.TotalCarat, p.TotalButir };
        var rs_3 = from p in d.DocRepair_Stone3 select new { p.TotalCarat, p.TotalButir };
        var rs_4 = from p in d.DocRepair_Stone4 select new { p.TotalCarat, p.TotalButir };
        var rs_5 = from p in d.DocRepair_Stone5 select new { p.TotalCarat, p.TotalButir };

        decimal sc = 0m;
        decimal sw = 0m;
        decimal nw = 0m;
        decimal sq = 0m;

        sc = sc + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalCarat) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalCarat) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalCarat) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalCarat) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalCarat) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalCarat) : 0))
                    ;
        sw = sw + ((decimal)dcp.StoneCarat * (decimal)0.2);
        nw = nw + ((decimal)dcp.GrossWeight - (decimal)dcp.StoneWeight);
        sq = sq + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalButir) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalButir) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalButir) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalButir) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalButir) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalButir) : 0))
                    ;

        dcp.StoneCarat = sc;
        dcp.StoneWeight = sw;
        dcp.NettoWeight = nw;
        dcp.StoneQty = sq;

        //Commit
        db.SubmitChanges();
    }

    public static void RecalWeightRepairResult(Map_DB db, DocRepairResult d, DocRepairResult_CharProduct dcp)
    {
        var rs_1A = from p in d.DocRepairResult_Stone1A select new { p.TotalCarat, p.TotalButir };
        var rs_1B = from p in d.DocRepairResult_Stone1B select new { p.TotalCarat, p.TotalButir };
        var rs_2 = from p in d.DocRepairResult_Stone2 select new { p.TotalCarat, p.TotalButir };
        var rs_3 = from p in d.DocRepairResult_Stone3 select new { p.TotalCarat, p.TotalButir };
        var rs_4 = from p in d.DocRepairResult_Stone4 select new { p.TotalCarat, p.TotalButir };
        var rs_5 = from p in d.DocRepairResult_Stone5 select new { p.TotalCarat, p.TotalButir };

        decimal sc = 0m;
        decimal sw = 0m;
        decimal nw = 0m;
        decimal sq = 0m;

        sc = sc + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalCarat) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalCarat) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalCarat) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalCarat) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalCarat) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalCarat) : 0))
                    ;
        sw = sw + ((decimal)dcp.StoneCarat * (decimal)0.2);
        nw = nw + ((decimal)dcp.GrossWeight - (decimal)dcp.StoneWeight);
        sq = sq + ((rs_1A.Count() > 0 ? rs_1A.Sum(p => p.TotalButir) : 0)
                    + (rs_1B.Count() > 0 ? rs_1B.Sum(p => p.TotalButir) : 0)
                    + (rs_2.Count() > 0 ? rs_2.Sum(p => p.TotalButir) : 0)
                    + (rs_3.Count() > 0 ? rs_3.Sum(p => p.TotalButir) : 0)
                    + (rs_4.Count() > 0 ? rs_4.Sum(p => p.TotalButir) : 0)
                    + (rs_5.Count() > 0 ? rs_5.Sum(p => p.TotalButir) : 0))
                    ;

        dcp.StoneCarat = sc;
        dcp.StoneWeight = sw;
        dcp.NettoWeight = nw;
        dcp.StoneQty = sq;

        //Commit
        db.SubmitChanges();
    }
}