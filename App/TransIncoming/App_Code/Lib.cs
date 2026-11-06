using System;
using CrossBrand;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;

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

    public static bool TransferValidationCrossBrand(int _xTipeFrom = 0, int _xIdFrom = 0, int _xTipeTo = 0, int _xIdTo = 0, int _xBrandFrom = 0, int _xBrandTo = 0)
    {

        using (Map_DB d = new Map_DB())
        {
            Modelize cross = new Modelize();

            var locF_W = cross.Warehouses();
            var locF_O = cross.Outlets();
            var locF_E = cross.Exhibitions();

            var locT_W = locF_W;
            var locT_O = locF_O;
            var locT_E = locF_E;

            var xKodeLokasiFrom = "";
            var xIdBrandFrom = 0;
            var xTipeLokasiFrom = _xTipeFrom;
            var xIdLokasiFrom = _xIdFrom;

            var xKodeLokasiTo = "";
            var xIdBrandTo = 0;
            var xTipeLokasiTo = _xTipeTo;
            var xIdLokasiTo = _xIdTo;

            int isValidFrom = 0;
            int isValidTo = 0;

            bool isValidTransfer = false;

    
    
            if (xTipeLokasiFrom == 0)
            {
                locF_W = locF_W.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiFrom && p.IDBrand == _xBrandFrom).ToList();
                if (locF_W.Count() == 1)
                {
                    xKodeLokasiFrom = locF_W.First().Nama;
                    xIdBrandFrom = locF_W.First().IDBrand;
                }
            }
            else if (xTipeLokasiFrom == 1)
            {
                locF_O = locF_O.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiFrom && p.IDBrand == _xBrandFrom).ToList();
                if (locF_O.Count() == 1)
                {

                    xKodeLokasiFrom = locF_O.First().Kode;
                    xIdBrandFrom = locF_O.First().IDBrand;
                }
            }
            else if (xTipeLokasiFrom == 2)
            {
                locF_E = locF_E.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiFrom && p.IDBrand == _xBrandFrom).ToList();
                if (locF_E.Count() == 1)
                {

                    xKodeLokasiFrom = locF_E.First().Kode;
                    xIdBrandFrom = locF_E.First().IDBrand;
                }
            }

    
            if (xTipeLokasiTo == 0)
            {
                locT_W = locT_W.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiTo && p.IDBrand == _xBrandTo).ToList();
                if (xTipeLokasiTo == xTipeLokasiFrom) { locT_W = locT_W.Where(p => p.ID != xIdLokasiFrom).ToList(); }
                if (locT_W.Count() == 1)
                {
                    xKodeLokasiTo = locT_W.First().Nama;
                    xIdBrandTo = locT_W.First().IDBrand;
                }
            }
            else if (xTipeLokasiTo == 1)
            {
                locT_O = locT_O.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiTo && p.IDBrand == _xBrandTo).ToList();
                if (xTipeLokasiTo == xTipeLokasiFrom) { locT_O = locT_O.Where(p => p.ID != xIdLokasiFrom).ToList(); }
                if (locT_O.Count() == 1)
                {
                    xKodeLokasiTo = locT_O.First().Kode;
                    xIdBrandTo = locT_O.First().IDBrand;
                }
            }
            else if (xTipeLokasiTo == 2)
            {
                locT_E = locT_E.Where(p => p.Disable == 0 && p.Draft == 0 && p.ID == xIdLokasiTo && p.IDBrand == _xBrandTo).ToList();
                if (xTipeLokasiTo == xTipeLokasiFrom) { locT_E = locT_E.Where(p => p.ID != xIdLokasiFrom).ToList(); }
                if (locT_E.Count() == 1)
                {
                    xKodeLokasiTo = locT_E.First().Kode;
                    xIdBrandTo = locT_E.First().IDBrand;
                }
            }

            try
            {
                string spname = "[SP_SAP_TRANSFER_VALIDATION]";
                string query = " exec " + spname
                             + " '" + xKodeLokasiFrom + "',"
                             + " '" + xIdBrandFrom + "',"
                             + " '" + xTipeLokasiFrom + "',"
                             + " '" + xIdLokasiFrom + "',"
                             + " '" + xKodeLokasiTo + "',"
                             + " '" + xIdBrandTo + "',"
                             + " '" + xTipeLokasiTo + "',"
                             + " '" + xIdLokasiTo + "'"
                             ;

                DataTable dt = new DataTable();
                dt = Db.Rs(query, Db.Cnn_IN);

                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dtRow in dt.Rows)
                    {
                        if (dtRow["IsValidFrom"] != DBNull.Value)
                        {
                            isValidFrom = Convert.ToInt16(dtRow["IsValidFrom"]);
                        }

                        if (dtRow["IsValidTo"] != DBNull.Value)
                        {
                            isValidTo = Convert.ToInt16(dtRow["IsValidTo"]);
                        }
                    }
                }

    
                if (isValidFrom == 1 && isValidTo == 1)
                {
                    isValidTransfer = true;
                }

    
                return isValidTransfer;
            }
            catch
            {
                return isValidTransfer;
            }

                }
}
	
	public static void InsertToSAPStaging(string no_trima, string brand, string tipe_product, bool iscrossbrand)
	{
		string crossbrand = "0";
		if (iscrossbrand)
			crossbrand = "1";
		//string query = "EXEC sp_posting_plant_to_plant_to_sap_staging '" + no_trima + "','" + brand + "','" + tipe_product + "'," + crossbrand;
		string query = "SELECT 'TEST' ";
		Db.Execute(query, Db.Cnn_DB);
	}
	
	public static string CheckDoKRM(string nokrm)
    {
        string query = "EXEC sp_check_incoming_dokrm_new '" + nokrm + "'";
        string result = Db.SingleString(query, Db.Cnn_DB);
        return result;
    }
}