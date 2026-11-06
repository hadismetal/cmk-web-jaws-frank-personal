using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Web;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page {
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
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
	protected int myIDItem { get { return App.QueryInt(this, "iditem"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("Payment.aspx"); }

    protected void DataInvalidResponseWrite(string err) { Response.Write(err); }
	
	protected void Save() {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myIDItem);
            if (x != null)
            {
                // DO NOT DELETE, VALIDASI ADD DP BASED ON NOPRO
                // if (x.IDProduct == null)
                // {
                //     using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
                //     {
                //         using (SqlCommand cmd = new SqlCommand(
                //             "SELECT NoValidasi, NoValidasi2 FROM SalesReceiptDPPO WHERE ID = @ID AND Tgl > '2026-03-08'",
                //             conn))
                //         {
                //             cmd.Parameters.Add("@ID", SqlDbType.Int).Value = myIDItem;

                //             conn.Open();

                //             using (SqlDataReader rdr = cmd.ExecuteReader())
                //             {
                //                 if (rdr.Read())
                //                 {
                //                     bool NoValidasiEmpty = (rdr["NoValidasi"] == DBNull.Value || rdr["NoValidasi"].ToString() == "");
                //                     bool NoValidasi2Empty = (rdr["NoValidasi2"] == DBNull.Value || rdr["NoValidasi2"].ToString() == "");

                //                     if (NoValidasiEmpty && NoValidasi2Empty)
                //                     {
                //                         // Response.Redirect("Payment_ItemList.aspx?id=" + myID + "#divrowpayment");
                //                         // return;
                //                         Response.Redirect(
                //                             "Payment_ItemList.aspx?id=" + myID + "&alert=dpinvalid#divrowpayment"
                //                         );
                //                         return;

                //                     }else{
                //                         //bikin list/array, push novalidasi dan novalidasi2 dari dppo 
                //                         //find novalidasi dari stockproduct, based on plu dari so
                //                         //if novalidasi dppo = novalidasi stockproduct then continue if not, alert + tambahin dpnya punya novalidasi berapa

                //                         List<string> NoValidasiList = new List<string>(); 
                //                         NoValidasiList.Add(rdr["NoValidasi"].ToString());
                //                         NoValidasiList.Add(rdr["NoValidasi2"].ToString());

                //                         // bool isDataExist = false;

                //                         string sql = @"
                //                         SELECT COUNT(*) 
                //                         FROM salesorder a
                //                         LEFT JOIN salesorderdj b ON b.idform = a.id
                //                         LEFT JOIN salesorderpg c ON c.idform = a.id
                //                         LEFT JOIN salesordergj e ON e.idform = a.id

                //                         LEFT JOIN stockproductdj f 
                //                             ON f.id = b.idproduct 
                //                             AND (
                //                                 (@NoPro  IS NOT NULL AND f.NoValidasi = @NoPro)
                //                                 OR
                //                                 (@NoPro2 IS NOT NULL AND f.NoValidasi = @NoPro2)
                //                             )

                //                         LEFT JOIN stockproductpg g 
                //                             ON g.id = c.idproduct 
                //                             AND (
                //                                 (@NoPro  IS NOT NULL AND g.NomorValidasi = @NoPro)
                //                                 OR
                //                                 (@NoPro2 IS NOT NULL AND g.NomorValidasi = @NoPro2)
                //                             )

                //                         LEFT JOIN stockproductgj i 
                //                             ON i.id = e.idproduct 
                //                             AND (
                //                                 (@NoPro  IS NOT NULL AND i.NoValidasi = @NoPro)
                //                                 OR
                //                                 (@NoPro2 IS NOT NULL AND i.NoValidasi = @NoPro2)
                //                             )

                //                         WHERE a.nomor = @NomorSO
                //                         AND (
                //                             f.id IS NOT NULL
                //                             OR g.id IS NOT NULL
                //                             OR i.id IS NOT NULL
                //                         );



                //                         ";
                //                         string noVal1 = rdr["NoValidasi"].ToString();
                //                         string noVal2 = rdr["NoValidasi2"].ToString();

                //                         using (SqlCommand cmd2 = new SqlCommand(sql, conn))
                //                         {
                //                             cmd2.Parameters.Add("@NomorSO", SqlDbType.VarChar).Value = d.Nomor;

                //                             cmd2.Parameters.Add("@NoPro", SqlDbType.VarChar).Value =
                //                                 string.IsNullOrWhiteSpace(noVal1) ? (object)DBNull.Value : noVal1;

                //                             cmd2.Parameters.Add("@NoPro2", SqlDbType.VarChar).Value =
                //                                 string.IsNullOrWhiteSpace(noVal2) ? (object)DBNull.Value : noVal2;

                //                             int count = Convert.ToInt32(cmd2.ExecuteScalar());

                //                             bool isValid = count > 0;

                //                             if (!isValid)
                //                             {
                //                                 // pick which DP to show
                //                                 string dpToShow = !string.IsNullOrWhiteSpace(noVal1) ? noVal1 : noVal2;

                //                                 Response.Redirect(
                //                                     "Payment_ItemList.aspx?id=" + myID +
                //                                     "&alert=dpinvalidr" +
                //                                     "&dp=" + HttpUtility.UrlEncode(dpToShow) +
                //                                     "#divrowpayment"
                //                                 );
                //                                 return;
                //                             }
                //                         }


                //                         // string msg = "DP Pesanan ";

                //                         // if (rdr["NoValidasi"].ToString() != "" && rdr["NoValidasi2"].ToString() != "")
                //                         // {
                //                         //     msg += rdr["NoValidasi"].ToString() + " dan " + rdr["NoValidasi2"].ToString();
                //                         // }
                //                         // else if (rdr["NoValidasi"].ToString() != "")
                //                         // {
                //                         //     msg += rdr["NoValidasi"].ToString();
                //                         // }
                //                         // else if (rdr["NoValidasi2"].ToString() != "")
                //                         // {
                //                         //     msg += rdr["NoValidasi2"].ToString();
                //                         // }

                //                         // ClientScript.RegisterStartupScript(
                //                         //     this.GetType(),
                //                         //     "toastrInfo",
                //                         //     "toastr.info('" + msg + "');",
                //                         //     true
                //                         // );

                //                         // Response.Redirect("Payment_ItemList.aspx?id=" + myID + "#divrowpayment");
                //                     }
                //                 }
                //             }
                //         }
                //     }
                // }

                if (IsUsed())
                {
                    DataInvalidResponseWrite("Nomor DP Sudah Digunakan di SO Lain.");
                }
                else
                {
					decimal TotalBayar = x.TotalBayar.Value;
					SalesReceipt s = new SalesReceipt();

					s.IDSalesOrder = myID;
					s.NoSalesOrder = d.Nomor;
					s.OperatorNama = App.UserNama;
					s.OperatorTgl = DateTime.Now;
					s.TotalBayar = x.TotalBayar.Value;

					db.SalesReceipt.InsertOnSubmit(s);
					db.SubmitChanges();

					int IDSalesReceipt = s.ID;

					SalesReceiptDetail sd = new SalesReceiptDetail();

					sd.IDSalesReceipt = IDSalesReceipt;
					
					sd.IDPaymentType = 8;
					sd.CCNumber = x.NomorDP;
					sd.Nominal = x.TotalBayar.Value;
					sd.MDR = 0;
					sd.OperatorNama = App.UserNama;
					sd.OperatorTgl = DateTime.Now;

					db.SalesReceiptDetail.InsertOnSubmit(sd);
					db.SubmitChanges();

					// sales order information update
					var z = db.SalesReceipt.Where(p => p.IDSalesOrder == d.ID).Sum(p => p.TotalBayar);
					d.Paid = z;
					d.Unpaid = d.TotalBayar - z;
					db.SubmitChanges();

					var y = db.SalesReceipt.SingleOrDefault(p => p.ID == IDSalesReceipt);

					string NoInvoice = Document.Generate_Nomor_Invoice(d);
					string NoDP = Document.Generate_Nomor_DownPayment(d);

					if (d.Unpaid <= 0m)
					{
						//d.StatusPembayaran = true;
						y.TotalBayar = TotalBayar;
						y.NomorInvoice = NoInvoice;

						decimal SisaDP = TotalBayar;

						var dj = d.SalesOrderDJ;
						foreach (var r in dj)
						{
							if (d.TradeIn == true)
								r.StockProductDJ.StockActualDJ.Substorage = 3;
							r.StockProductDJ.StatusPenjualan = 2;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						var pg = d.SalesOrderPG;
						foreach (var r in pg)
						{
							if (d.TradeIn == true)
								r.StockProductPG.StockActualPG.Substorage = 3;
							r.StockProductPG.StatusPenjualan = 2;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						var gj = d.SalesOrderGJ;
						foreach (var r in gj)
						{
							if (d.TradeIn == true)
								r.StockProductGJ.StockActualGJ.Substorage = 3;
							r.StockProductGJ.StatusPenjualan = 2;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						var ld = d.SalesOrderLD;
						foreach (var r in ld)
						{
							if (d.TradeIn == true)
								r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 3;
							r.StockProductLD_Stone1B.StatusPenjualan = 2;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						//Commit
						db.SubmitChanges();
					}
					else
					{
						x.TotalBayar = TotalBayar;
						d.StatusPembayaran = false;
						//x.NomorDP = NoDP;

						decimal SisaDP = TotalBayar;

						var dj = d.SalesOrderDJ;
						foreach (var r in dj)
						{
							r.StockProductDJ.StatusPenjualan = 3;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						var pg = d.SalesOrderPG;
						foreach (var r in pg)
						{
							r.StockProductPG.StatusPenjualan = 3;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}

						var gj = d.SalesOrderGJ;
						foreach (var r in gj)
						{
							r.StockProductGJ.StatusPenjualan = 3;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}

						var ld = d.SalesOrderLD;
						foreach (var r in ld)
						{
							r.StockProductLD_Stone1B.StatusPenjualan = 3;

							decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

							if (SisaDP > 0)
							{
								if (TotalDP < r.TotalBayar)
								{
									if (SisaDP > r.TotalBayar)
									{
										SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
										r.TotalDP = r.TotalBayar.Value;
									}
									else
									{
										r.TotalDP = TotalDP + SisaDP;
										SisaDP = 0m;
									}
									db.SubmitChanges();
								}
							}
						}
						//Commit
						db.SubmitChanges();

					}

					//x.StatusDP = 1;
					//x.IDSalesOrder = d.ID;
					db.SubmitChanges();
				}
            }
                
            Response.Redirect("Payment_ItemList.aspx?id=" + myID + "#divrowpayment");
        }	
	}

    protected bool IsUsed()
    {
        bool isused = false;
        string query = 
            "DECLARE @NOMORPO VARCHAR(100) = (SELECT TOP 1 ISNULL(NomorDP,'') FROM SalesReceiptDPPO WHERE ID = @IDPO) " +
            "DECLARE @ISANY INT = 0 " +
            "IF EXISTS (SELECT 1 FROM SalesReceipt A JOIN SalesReceiptDetail B ON A.ID = B.IDSalesReceipt " +
            "JOIN SalesOrder C ON A.IDSalesOrder = C.ID WHERE C.StatusPembayaran IN (0,1) " +
            "AND C.StatusVoid = 0 AND C.Draft = 0 AND B.IDPaymentType = 8 AND B.CCNumber = @NOMORPO) " +
            "BEGIN " +
            "SET @ISANY = 1 " +
            "END " +
            "SELECT @ISANY [ISANY] ";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@IDPO", SqlDbType.Int){ Value = myIDItem }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        int status = Convert.ToInt32(dt.Rows[0][0]);

        if(status == 1)
        {
            isused = true;
        }

        return isused;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}