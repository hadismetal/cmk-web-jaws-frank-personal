using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Lib {
	//Recalculate
	public static void RecalWeight(Map_DB db, StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		//Master
		decimal totalCarat = 0;

		//Stone
		var rs_1A = from p in d.StockReceiveDJ_Stone1A select p.TotalCarat;
		foreach (var r in rs_1A) totalCarat += r;

		var rs_1B = from p in d.StockReceiveDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalCarat;
		foreach (var r in rs_1B) totalCarat += r;

		var rs_2 = from p in d.StockReceiveDJ_Stone2 select p.TotalCarat;
		foreach (var r in rs_2) totalCarat += r;

		var rs_3 = from p in d.StockReceiveDJ_Stone3 select p.TotalCarat;
		foreach (var r in rs_3) totalCarat += r;

		var rs_4 = from p in d.StockReceiveDJ_Stone4 select p.TotalCarat;
		foreach (var r in rs_4) totalCarat += r;

		var rs_5 = from p in d.StockReceiveDJ_Stone5 select p.TotalCarat;
		foreach (var r in rs_5) totalCarat += r;

		//Save to Database
		product.StoneCarat = totalCarat;
		product.StoneWeight = totalCarat * (decimal) 0.2;
		product.NettoWeight = product.GrossWeight - product.StoneWeight;

		//Commit
		db.SubmitChanges();
	}

    public static bool isTimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
    {
        // convert datetime to a TimeSpan
        TimeSpan now = datetime.TimeOfDay;
        // see if start comes before end
        if (start < end)
            return start <= now && now <= end;
        // start is after end, so do the inverse comparison
        return !(end < now && now < start);
    }

    public static decimal CalcGJJual(int IDSupplier, int Model, decimal Weight, bool USD)
    {
        decimal HargaJual = 0m;
        decimal GoldRate = 0m;
        decimal Rate = 0m;
        int IDGroupSupplier = 1;
        IDGroupSupplier = Db.SingleInteger("SELECT IDGROUP FROM DataSupplier WHERE ID=" + IDSupplier, Db.Cnn_DB);
        Rate = Db.SingleDecimal("SELECT Harga FROM PricingTableUS WHERE ID=3", Db.Cnn_DB);
        if (IDGroupSupplier == 1 || IDGroupSupplier == 17 || IDGroupSupplier == 12)
        {
            if (Model == 4)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 21", Db.Cnn_DB);
            }
            else if (Model == 5)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 22", Db.Cnn_DB);
            }
        }
        else if (IDGroupSupplier == 2)
        {
            if (Model == 4)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 17", Db.Cnn_DB);
            }
            else if (Model == 5)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 18", Db.Cnn_DB);
            }
        }
        else if (IDGroupSupplier == 3)
        {
            if (Model == 4)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 19", Db.Cnn_DB);
            }
            else if (Model == 5)
            {
                GoldRate = Db.SingleDecimal("SELECT Harga FROM PricingTableTG WHERE ID = 20", Db.Cnn_DB);
            }
        }

        if (USD == true)
            HargaJual = GoldRate * Weight;
        else
            HargaJual = GoldRate * Weight * Rate;

        return HargaJual;
    }

    public static void Image(Image img, string Path)
    {
        string NoImage = App.ImagesDir + "null.png";
        string url = string.Empty;

        url += "/Images.aspx?path=";
        url += Path == null ? NoImage : Path;

        img.ImageUrl = url;
    }
	
	public static void InsertPlantToPlantToSAPStaging(string no_trima, string brand, string tipe_product, bool iscrossbrand, string nodobrj, string nopro)
	{
		string crossbrand = "0";
		if (iscrossbrand)
			crossbrand = "1";
		//string query = "EXEC sp_posting_plant_to_plant_to_sap_staging '" + no_trima + "','" + brand + "','" + tipe_product + "'," + crossbrand + ",'" + nodobrj + "','" + nopro + "'";
		string query = "SELECT 'TEST' ";
		Db.Execute(query, Db.Cnn_DB);
	}
	
	public static void InsertUpdateCharPLUToSAPStaging(string no_brj, string no_validasi, string no_matnr, string no_plu, string tipe_product)
	{
		//string query = "EXEC sp_posting_update_characteristic_plu_to_sap_staging '" + no_brj + "','" + no_validasi + "','" + no_matnr + "','" + no_plu + "','" + tipe_product + "'";
		string query = "SELECT 'TEST' ";
		Db.Execute(query, Db.Cnn_DB);
	}
}