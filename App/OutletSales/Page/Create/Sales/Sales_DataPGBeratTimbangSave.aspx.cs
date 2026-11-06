using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Sales_Sales_DataPGBeratTimbangSave : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected string myIDSoPg { get { return Request.QueryString["idsopg"]; } }
    protected string WeightPG { get { return Request.QueryString["weightpg"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        save();
        Response.Redirect("Sales_Data.aspx?refreshPage=1&id="+myID+"");
    }
	protected void save()
    {
        var so = db.SalesOrderPG.SingleOrDefault(p => p.ID == Convert.ToInt32(myIDSoPg));
        var d = so.SalesOrder;
        var sp = so.StockProductPG;
        var s = db.StockActualPG.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == sp.ID);
        if(s != null)
        {
            decimal GoldPrice = GetPGPricingMyAPPSNew(s.StockProductPG.ID);
            decimal ModalTGP = GetPGPricing(s.StockProductPG, true);

            if (s.StockProductPG.FixRate == true)
            {
                GoldPrice = s.StockProductPG.TotalHargaJual.Value;
                ModalTGP = s.StockProductPG.Harga.Value;

                GoldPrice = Math.Ceiling(GoldPrice / 10000) * 10000;
            }



            var c = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == sp.ID && p.SalesOrder.StatusVoid == false);
            if (c != null)
            {
                decimal Discount = 0m;
                decimal DiscountPersen = 0m;
                decimal DiscountHarga = 0m;
                decimal TotalBayar = GoldPrice;

                var rs = from p in db.PromoPG_Detail
                         where p.PromoPG.ApprovalNama != null && p.PromoPG.TglStart <= DateTime.Today
                         && p.PromoPG.TglExpire >= DateTime.Today && p.IDProduct == s.IDProduct
                         && (p.PromoPG.StatusActive != false || p.PromoPG.StatusActive == null)
                         select p;
                if (rs.Count() > 0)
                {
                    var disc = rs.First();
                    if (disc.PromoPG.Tipe == 1)
                    {
                        DiscountHarga = disc.PromoPG.DiscountNominal.Value;
                        DiscountPersen = (TotalBayar * (disc.PromoPG.DiscountPersen.Value / 100));
                        Discount = (DiscountHarga + DiscountPersen);
                        TotalBayar = TotalBayar - Discount;
                    }
                    if (disc.PromoPG.Tipe == 3)
                    {
                        TotalBayar = disc.PromoPG.PromoHarga.Value;
                        Discount = GoldPrice - TotalBayar;

                    }
                }
                
                so.ModalRupiah = s.StockProductPG.TotalHarga24;
                so.ModalTGP = ModalTGP;
                so.HargaRupiah = GoldPrice;
                so.DiscountPromo = Discount;
                so.TotalBayar = TotalBayar;
                so.TotalRupiah = GoldPrice;
                db.SubmitChanges();
            }
        }

        Db.Execute("UPDATE SalesOrderPG SET BeratTimbang = '" + WeightPG.Replace(",", ".") + "' WHERE ID = " + myIDSoPg + " ", Db.Cnn_DB);
    }

    protected decimal GetPGPricingMyAPPSNew(int IDProduct)
    {
        decimal GoldPrice = 0m;
        decimal TotalRupiah = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if(dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_JUAL"]);
        }

        TotalRupiah = GoldPrice * Convert.ToDecimal(WeightPG);
        return TotalRupiah;
    }

    protected decimal GetPGPricing(StockProductPG r, bool Costing)
    {
        int IDPricing = 0;
        decimal GoldPrice = 0m;
        decimal TotalRupiah = 0m;

        if (Costing)
        {
            if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    IDPricing = 9;
                else
                    IDPricing = 10;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        IDPricing = 5;
                    else
                        IDPricing = 6;
                }
                else
                {
                    if (r.FrameColor == 1)
                        IDPricing = 7;
                    else
                        IDPricing = 8;
                }
            }
        }
        else
        {
			if(r.CharGoldLevel.ID == 13){ IDPricing = 37; }
			//else if(r.CharGoldLevel.ID == 14){ IDPricing = 38; }
			else if(r.CharGoldLevel.ID == 14){ IDPricing = 37; } //Update tgl 19 Oktober 2024, mappingan sama dengan id 13
            else if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
            {
                IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
            }
            else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7) // 6 = zirconia 37,5 ; 7 = zirconia 75 
            {
                if (r.CharGoldLevel.ID == 6) { IDPricing = 34; }
                else if (r.CharGoldLevel.ID == 7) { IDPricing = 35; }
            }
			else if (r.CharGoldLevel.ID == 9) { IDPricing = 37; }
            //else if (r.CharGoldLevel.ID == 10) {IDPricing = 38; }
			else if (r.CharGoldLevel.ID == 10) {IDPricing = 37; } //Update tgl 19 Oktober 2024, mappingan sama dengan id 9
            else if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    IDPricing = 15;
                else
                    IDPricing = 16;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        IDPricing = 11;
                    else
                        IDPricing = 12;
                }
                else
                {
                    if (r.FrameColor == 1)
                        IDPricing = 13;
                    else
                        IDPricing = 14;
                }
            }
        }

        GoldPrice = PGPrice(IDPricing);

        TotalRupiah = CalcPrice(GoldPrice, Convert.ToDecimal(WeightPG), Costing);

        return TotalRupiah;
    }

    protected decimal PGPrice(int IDPricing)
    {
        decimal Price = 0m;

        var d = db.PricingTableTG.SingleOrDefault(p => p.ID == IDPricing);
        if (d != null)
            Price = d.Harga.Value;

        return Price;
    }

    protected decimal CalcPrice(decimal GoldPrice, decimal Weight, bool Costing)
    {
        decimal Price = 1m;
        decimal Rate = 1m;
        int IDRate = 1;

        if (Costing)
            IDRate = 1;
        else
            IDRate = 2;

        var d = db.PricingTableUS.SingleOrDefault(p => p.ID == IDRate);
        Rate = d.Harga.Value;

        //Price = GoldPrice * Rate * Weight;
        Price = GoldPrice * Weight;
        //Price = (Price > 0m) ? (Math.Ceiling(Price / 1000) * 1000) : 1m;

        return Price;
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandTimeout = 300;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader); // Not streaming, but async-safe here
                }
            }
        }
        return dt;
    }
}