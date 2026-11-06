using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Kalkulator_Kalkulator_Buyback_Pricing : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static object Display(RequestData request)
    {
        string user = string.Empty;
        int userid = 0;
        if (IsSessionExpired())
        {
            return new ResponseData { success = false, error = "Session expired" };
        }
        else
        {
            user = HttpContext.Current.Session["UserNama"].ToString();
            userid = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        }

        try
        {
            string query = "sp_buyback_calculator_new";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@IDPRODUCT", SqlDbType.Int){ Value = request.id_product },
                new SqlParameter("@TIPE", SqlDbType.Int){ Value = 1 },
                new SqlParameter("@IDDOCQC", SqlDbType.Int){ Value = request.id_qc}
            };
            DataSet ds = Ds(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
            DataTable dtproduct = ds.Tables[0];
            DataTable dtstone = ds.Tables[1];
            DataTable dtasp = ds.Tables[2];
            DataTable dtsummary = ds.Tables[3];

            string plu = dtproduct.Rows[0]["PLU"].ToString();
            string no_so = dtproduct.Rows[0]["NOSO"].ToString();
            string lokasi_so = dtproduct.Rows[0]["LOKASI_SO"].ToString();
            string tanggal_so = Convert.ToDateTime(dtproduct.Rows[0]["TANGGAL_SO"]).ToString("dd-MM-yyyy");
            decimal harga_so = Convert.ToDecimal(dtproduct.Rows[0]["HARGA_SO"]);
            string item = dtproduct.Rows[0]["ITEM"].ToString();
            string kategori = dtproduct.Rows[0]["KATEGORI"].ToString();
            decimal harga_acuan_buyback = Convert.ToDecimal(dtproduct.Rows[0]["HARGA_ACUAN_BUYBACK"]);
            decimal buying_price = Convert.ToDecimal(dtproduct.Rows[0]["BUYING_PRICE"]);
            string brand = dtproduct.Rows[0]["BRAND"].ToString();
            string item_kode = dtproduct.Rows[0]["KODE_ITEM"].ToString();
            string stone_dist_kode = dtproduct.Rows[0]["KODE_STONEDIST"].ToString();
            bool is_tiny_tan = dtproduct.Rows[0]["IS_TINY_TAN"].ToString() == "1" ? true : false;
            bool is_mounting = dtproduct.Rows[0]["IS_MOUNTING"].ToString() == "1" ? true : false;
            string customer_nama = dtproduct.Rows[0]["CUSTOMER_NAMA"].ToString();
            string given_to = dtproduct.Rows[0]["GIVEN_TO"].ToString();
            string tanggal_jual = DateTime.Now.ToString("dd-MM-yyyy");
            string frame_material = dtproduct.Rows[0]["FRAME_MATERIAL"].ToString();
            decimal gross_weight = Convert.ToDecimal(dtproduct.Rows[0]["GROSS_WEIGHT_ACTUAL"]);
            decimal gross_weight_qc = Convert.ToDecimal(dtproduct.Rows[0]["GROSS_WEIGHT_QC"]);
            string no_qc = dtproduct.Rows[0]["NO_QC"].ToString();

            ResponseProduct product = new ResponseProduct()
            {
                plu = plu,
                no_so = no_so,
                lokasi_so = lokasi_so,
                tanggal_so = tanggal_so,
                harga_so = harga_so,
                item = item,
                kategori = kategori,
                harga_acuan_buyback = harga_acuan_buyback,
                buying_price = buying_price,
                brand = brand,
                item_kode = item_kode,
                stone_dist_kode = stone_dist_kode,
                is_tiny_tan = is_tiny_tan,
                is_mounting = is_mounting,
                customer_nama = customer_nama,
                given_to = given_to,
                tanggal_jual = tanggal_jual,
                frame_material = frame_material,
                gross_weight = gross_weight,
                gross_weight_qc = gross_weight_qc,
                no_qc = no_qc
            };

            List<ResponseStone> stone = new List<ResponseStone>();
            foreach (DataRow dr in dtstone.Rows)
            {
                string nama_stone = dr["NAMA_STONE"].ToString();
                string stone_type = dr["TIPE_STONE"].ToString();
                int total_butir = Convert.ToInt32(dr["TOTAL_BUTIR"]);
                int total_butir_qc = Convert.ToInt32(dr["TOTAL_BUTIR_QC"]);
                decimal total_carat = Convert.ToDecimal(dr["TOTAL_CARAT"]);
                decimal total_carat_qc = Convert.ToDecimal(dr["TOTAL_CARAT_QC"]);
                string gia = dr["GIA"].ToString();

                stone.Add(new ResponseStone
                {
                    nama = nama_stone,
                    stone_type = stone_type,
                    total_butir = total_butir,
                    total_butir_qc = total_butir_qc,
                    total_carat = total_carat,
                    total_carat_qc = total_carat_qc,
                    gia = gia
                });
            }

            string nama = dtasp.Rows[0]["NAMA"].ToString();
            string tipe = dtasp.Rows[0]["TIPE"].ToString();
            string keterangan = dtasp.Rows[0]["KETERANGAN"].ToString();
            int me_periode_min = Convert.ToInt32(dtasp.Rows[0]["ME_PERIODE_MIN"]);
            int me_periode_max = Convert.ToInt32(dtasp.Rows[0]["ME_PERIODE_MAX"]);
            int me_rumus = Convert.ToInt32(dtasp.Rows[0]["ME_RUMUS"]);
            int ti1_periode_min = Convert.ToInt32(dtasp.Rows[0]["TI1_PERIODE_MIN"]);
            int ti1_periode_max = Convert.ToInt32(dtasp.Rows[0]["TI1_PERIODE_MAX"]);
            int ti1_rumus = Convert.ToInt32(dtasp.Rows[0]["TI1_RUMUS"]);
            int ti2_periode_min = Convert.ToInt32(dtasp.Rows[0]["TI2_PERIODE_MIN"]);
            int ti2_periode_max = Convert.ToInt32(dtasp.Rows[0]["TI2_PERIODE_MAX"]);
            int ti2_rumus = Convert.ToInt32(dtasp.Rows[0]["TI2_RUMUS"]);
            int ti3_periode_min = Convert.ToInt32(dtasp.Rows[0]["TI3_PERIODE_MIN"]);
            int ti3_periode_max = Convert.ToInt32(dtasp.Rows[0]["TI3_PERIODE_MAX"]);
            int ti3_rumus = Convert.ToInt32(dtasp.Rows[0]["TI3_RUMUS"]);
            int r1_periode_min = Convert.ToInt32(dtasp.Rows[0]["R1_PERIODE_MIN"]);
            int r1_periode_max = Convert.ToInt32(dtasp.Rows[0]["R1_PERIODE_MAX"]);
            int r1_rumus = Convert.ToInt32(dtasp.Rows[0]["R1_RUMUS"]);
            int r2_periode_min = Convert.ToInt32(dtasp.Rows[0]["R2_PERIODE_MIN"]);
            int r2_periode_max = Convert.ToInt32(dtasp.Rows[0]["R2_PERIODE_MAX"]);
            int r2_rumus = Convert.ToInt32(dtasp.Rows[0]["R2_RUMUS"]);
            int me_periode_max_day = Convert.ToInt32(dtasp.Rows[0]["ME_PERIODE_MAX_DAY"]);

            int ti01_periode_min = 0;
            int ti01_periode_max = 2;
            int ti01_rumus = 100;

            int ti0_periode_min = 0;
            int ti0_periode_max = 6;
            int ti0_rumus = 85;

            ResponseASP asp = new ResponseASP()
            {
                nama = nama,
                tipe = tipe,
                keterangan = keterangan,
                me_periode_min = me_periode_min,
                me_periode_max = me_periode_max,
                me_rumus = me_rumus,
                ti1_periode_min = ti1_periode_min,
                ti1_periode_max = ti1_periode_max,
                ti1_rumus = ti1_rumus,
                ti2_periode_min = ti2_periode_min,
                ti2_periode_max = ti2_periode_max,
                ti2_rumus = ti2_rumus,
                ti3_periode_min = ti3_periode_min,
                ti3_periode_max = ti3_periode_max,
                ti3_rumus = ti3_rumus,
                r1_periode_min = r1_periode_min,
                r1_periode_max = r1_periode_max,
                r1_rumus = r1_rumus,
                r2_periode_min = r2_periode_min,
                r2_periode_max = r2_periode_max,
                r2_rumus = r2_rumus,
                me_periode_max_day = me_periode_max_day
            };

            if (nama == "DOS BRANDED")
            {
                asp.me_periode_max = 1;
                asp.ti01_periode_min = ti01_periode_min;
                asp.ti01_periode_max = ti01_periode_max;
                asp.ti01_rumus = ti01_rumus;
                asp.ti0_periode_min = ti0_periode_min;
                asp.ti0_periode_max = ti0_periode_max;
                asp.ti0_rumus = ti0_rumus;
            }

            int umur_hari = Convert.ToInt32(dtsummary.Rows[0]["UMUR_HARI"]);
            int umur_bulan = Convert.ToInt32(dtsummary.Rows[0]["UMUR_BULAN"]);
            int umur_tahun = Convert.ToInt32(dtsummary.Rows[0]["UMUR_TAHUN"]);
            /*decimal harga_trade_in = Convert.ToDecimal(dtsummary.Rows[0]["HARGA_TRADE_IN"]);
            decimal harga_resell = Convert.ToDecimal(dtsummary.Rows[0]["HARGA_RESELL"]);
            string status = dtsummary.Rows[0]["STATUS"].ToString();
            string keterangan_summary = dtsummary.Rows[0]["KETERANGAN"].ToString();*/

            CountBuyback cb = CountResellTradeInPrice(asp, umur_tahun, umur_bulan, umur_hari, product.harga_acuan_buyback, product.buying_price, product.item, product.frame_material);

            ResponseSummary summary = new ResponseSummary()
            {
                umur_hari = umur_hari,
                umur_bulan = umur_bulan,
                umur_tahun = umur_tahun,
                harga_resell = cb.harga_resell,
                harga_trade_in = cb.harga_trade_in,
                keterangan = cb.keterangan,
                status_trade_in = cb.status_trade_in,
                status_resell = cb.status_resell
            };

            product.stone = stone;
            product.asp = asp;
            product.summary = summary;

            return new ResponseData()
            {
                success = true,
                error = string.Empty,
                data = product
            };
        }
        catch (Exception ex)
        {
            return new ResponseData { success = false, error = ex.Message };
        }
    }

    protected static CountBuyback CountResellTradeInPrice(ResponseASP asp, int tahun, int bulan, int hari, decimal hargaacuanbb, decimal buyingprice, string item, string material)
    {
        CountBuyback cb = new CountBuyback()
        {
            harga_resell = 0,
            harga_trade_in = 0,
            status_resell = string.Empty,
            status_trade_in = string.Empty,
            keterangan = asp.keterangan
        };

        if (!string.IsNullOrEmpty(asp.nama))
        {
            if (asp.tipe.ToLower() == "dj" && asp.nama == "PG FIXED")
            {
                cb.keterangan = asp.keterangan;

                cb.harga_trade_in = 0;
                cb.status_trade_in = "(*Call Pusat.)";

                cb.harga_resell = 0;
                cb.status_resell = "(*Call Pusat.)";
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "DOS BRANDED")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti01max = asp.ti01_periode_max;
                int ti01rumus = asp.ti01_rumus;

                int ti0max = asp.ti0_periode_max;
                int ti0rumus = asp.ti0_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 2)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 2 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if(bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti0rumus / 100;
                        cb.status_trade_in = "(" + ti0rumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 6 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * ti0rumus / 100;
                        cb.status_trade_in = "(" + ti0rumus + " % *Invoice Price Without PPN)";
					}
                    else 
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if(tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                    cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if(tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if(asp.tipe.ToLower() == "dj" && asp.nama == "SNIPS Tanpa Sertifikat")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 6 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if(tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if(asp.tipe.ToLower() == "dj" && asp.nama == "SNIPS Kurang Dari 0.3 Carat Bersertifikat")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 6 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if(asp.tipe.ToLower() == "dj" && asp.nama == "SNIPS Lebih Dari Atau Sama Dengan 0.3 Carat Bersertifikat")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                if(tahun == 0 && bulan <= 0 && hari <= 15)
                {
                    cb.harga_trade_in = hargaacuanbb * merumus / 100;
                    cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = 0;
                    cb.status_trade_in = "(*Call Pusat.)";
                }

                cb.harga_resell = 0;
                cb.status_resell = "(*Call Pusat.)";
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "DOS GENERIC")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 6 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "SEMI MOUNTING FASHION")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 6)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "SOL/TEN/TRI/ SNI BRANDED FOREVERMARK")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 6 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "INTRO")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun <= 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if (bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 6 && hari == 0)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if (asp.tipe.ToLower() == "pg" && asp.nama == "PG FIXED")
            {
                cb.keterangan = asp.keterangan;

                cb.harga_trade_in = 0;
                cb.status_trade_in = "(*Call Pusat.)";

                cb.harga_resell = 0;
                cb.status_resell = "(*Call Pusat.)";
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama != "")
            {
                int memax = asp.me_periode_max;
                int merumus = asp.me_rumus;

                int ti1max = asp.ti1_periode_max;
                int ti1rumus = asp.ti1_rumus;

                int ti2max = asp.ti2_periode_max;
                int ti2rumus = asp.ti2_rumus;

                int ti3max = asp.ti3_periode_max;
                int ti3rumus = asp.ti3_rumus;

                int r1max = asp.r1_periode_max;
                int r1rumus = asp.r1_rumus;

                int r2max = asp.r2_periode_max;
                int r2rumus = asp.r2_rumus;

                if (tahun == 0)
                {
                    if (bulan < 1)
                    {
                        cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
                    }
					else if (bulan == 1 && hari == 0)
					{
						cb.harga_trade_in = hargaacuanbb * merumus / 100;
                        cb.status_trade_in = "(" + merumus + " % *Invoice Price Without PPN)";
					}
                    else if(bulan < 6)
                    {
                        cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
                    }
					else if(bulan == 6)
					{
						cb.harga_trade_in = hargaacuanbb * ti1rumus / 100;
                        cb.status_trade_in = "(" + ti1rumus + " % *Invoice Price Without PPN)";
					}
                    else
                    {
                        cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                        cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                    }
                }
                else if (tahun < 2)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 2 && bulan == 0 && hari == 0)
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(" + ti2rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_trade_in = hargaacuanbb * ti2rumus / 100;
                    cb.status_trade_in = "(Buying Price)";
                }

                if (tahun < 1)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else if (tahun == 1 && bulan == 0 && hari == 0)
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(" + r1rumus + " % *Invoice Price Without PPN)";
                }
                else
                {
                    cb.harga_resell = hargaacuanbb * r1rumus / 100;
                    cb.status_resell = "(Buying Price)";
                }
            }
            else if (asp.tipe.ToLower() == "dj" && asp.nama == "")
            {
                cb.keterangan = "No Buyback";
            }
        }
        else if (item.ToLower().Contains("wedding ring") || item.ToLower().Contains("aretino"))
        {
            if (material.ToLower() == "emas")
            {
                //hardcode wedding ting
                cb.harga_resell = hargaacuanbb * 50 / 100;
                cb.status_resell = "(50 % *Invoice Price Without PPN)";
                cb.keterangan = "";
            }
        }

        return cb;
    }

    protected static bool IsSessionExpired()
    {
        if (HttpContext.Current.Session == null)
            return true;

        return HttpContext.Current.Session["UserID"] == null &&
               HttpContext.Current.Session["UserNama"] == null;
    }

    protected static DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
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
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }

    protected static DataSet Ds(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
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
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds); // Fill the DataSet
                }
            }
        }
        return ds;
    }

    public class RequestData
    {
        public int id_product { get; set; }
        public int id_qc { get; set; }
    }

    public class ResponseData
    {
        public bool success { get; set; }
        public string error { get; set; }
        public ResponseProduct data { get; set; }
    }

    public class ResponseProduct
    {
        public string plu { get; set; }
        public string no_so { get; set; }
        public string lokasi_so { get; set; }
        public string tanggal_so { get; set; }
        public string tanggal_jual { get; set; }
        public decimal harga_so { get; set; }
        public string item { get; set; }
        public string item_kode { get; set; }
        public string stone_dist_kode { get; set; }
        public string kategori { get; set; }
        public string frame_material { get; set; }
        public decimal harga_acuan_buyback { get; set; }
        public decimal buying_price { get; set; }
        public string brand { get; set; }
        public bool is_tiny_tan { get; set; }
        public bool is_mounting { get; set; }
        public string customer_nama { get; set; }
        public string given_to { get; set; }
        public decimal gross_weight { get; set; }
        public string no_qc { get; set; }
        public decimal gross_weight_qc { get; set; }
        public List<ResponseStone> stone { get; set; }
        public ResponseASP asp { get; set; }
        public ResponseSummary summary { get; set; }
    }

    public class ResponseStone
    {
        public string nama { get; set; }
        public string stone_type { get; set; }
        public int total_butir { get; set; }
        public int total_butir_qc { get; set; }
        public decimal total_carat { get; set; }
        public decimal total_carat_qc { get; set; }
        public string gia { get; set; }
    }

    public class ResponseASP
    {
        public string nama { get; set; }
        public string tipe { get; set; }
        public string keterangan { get; set; }
        public int me_periode_min { get; set; }
        public int me_periode_max { get; set; }
        public int me_rumus { get; set; }
        public int ti01_periode_min { get; set; }
        public int ti01_periode_max { get; set; }
        public int ti01_rumus { get; set; }
        public int ti0_periode_min { get; set; }
        public int ti0_periode_max { get; set; }
        public int ti0_rumus { get; set; }
        public int ti1_periode_min { get; set; }
        public int ti1_periode_max { get; set; }
        public int ti1_rumus { get; set; }
        public int ti2_periode_min { get; set; }
        public int ti2_periode_max { get; set; }
        public int ti2_rumus { get; set; }
        public int ti3_periode_min { get; set; }
        public int ti3_periode_max { get; set; }
        public int ti3_rumus { get; set; }
        public int r1_periode_min { get; set; }
        public int r1_periode_max { get; set; }
        public int r1_rumus { get; set; }
        public int r2_periode_min { get; set; }
        public int r2_periode_max { get; set; }
        public int r2_rumus { get; set; }
        public int me_periode_max_day { get; set; }
    }

    public class ResponseSummary
    {
        public int umur_hari { get; set; }
        public int umur_bulan { get; set; }
        public int umur_tahun { get; set; }
        public decimal harga_trade_in { get; set; }
        public decimal harga_resell { get; set; }
        public string status_trade_in { get; set; }
        public string status_resell { get; set; }
        public string keterangan { get; set; }
    }

    public class CountBuyback
    {
        public decimal harga_trade_in { get; set; }
        public decimal harga_resell { get; set; }
        public string status_trade_in { get; set; }
        public string status_resell { get; set; }
        public string keterangan { get; set; }
    }
}