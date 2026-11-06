using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Page_Approval_LD_PricingSubmit : System.Web.UI.Page
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
    decimal GT_HargaM = 0;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            if (d.Approval != null)
                save.Visible = false;

            fillStone();
            
        }
    }
    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Harga = 0;
        

        //Stone
        
        fillStone1B(1, ref GT_Butir, ref GT_Harga, ref GT_HargaM);

        //Grand-Total
        Sub_Total(GT_Butir, GT_Harga, GT_HargaM, true);
    }

    //Stone
    protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat, ref decimal GT_CaratM)
    {
        //SELECT
        var rs = from p in db.StockReceiveLD_Stone1B
                 where p.IDForm == myID
                 orderby p.Stone1B.Nama
                 select p;

        //Sub-Total
        int ST_Butir = 0;
        decimal ST_Harga = 0;
        decimal ST_HargaM = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(StoneIndex);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Stone1B.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaSatuanM) + " / " + Str.Number2(r.HargaSatuan);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Integer(r.TotalButir);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number3(r.TotalCarat);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.HargaTotalM) + " / " + Str.Number2(r.HargaTotal);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            //Total
            ST_Butir += r.TotalButir;
            ST_Harga += r.HargaTotal.Value;
            ST_HargaM += r.HargaTotalM.Value;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT_Butir += ST_Butir;
            GT_Carat += ST_Harga;
            GT_CaratM += ST_HargaM;

            Sub_Total(ST_Butir, ST_Harga, ST_HargaM, false);
        }
    }
    protected void Sub_Header(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalHarga, decimal TotalHargaM, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 5;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number2(TotalHargaM) + " / " + Str.Number2(TotalHarga);
        td.CssClass = "text-right text-bold";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) App.DataNull(this);
        else
        {
            //Status

            //User-Info
            //d.Nomor = Generate_Nomor(d);
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.StatusReceive = 1;

            //Commit
            db.SubmitChanges();

            StockProductLD prod = new StockProductLD();
            prod.IDWarehouse = d.IDWarehouse;
            prod.Keterangan = d.Keterangan;
            prod.Tgl = d.Tgl;
            prod.Nomor = d.Nomor;
            prod.Draft = d.Draft;
            prod.DraftDate = d.DraftDate;
            prod.Supplier = d.Supplier;
            prod.SupplierNama = d.SupplierNama;
            prod.SupplierNomor = d.SupplierNomor;
            prod.RequestOutlet = d.RequestOutlet;
            prod.RequestOutletNama = d.RequestOutletNama;
            prod.RequestCustomer = d.RequestCustomer;
            prod.RequestCustomerNama = d.RequestCustomerNama;
            prod.HumanStock = d.HumanStock;
            prod.HumanStockNama = d.HumanStockNama;
            prod.ExpiredProduct = d.ExpiredProduct;
            prod.ExpiredRequest = d.ExpiredRequest;
            prod.Operator = d.Operator;
            prod.OperatorTgl = d.OperatorTgl;
            prod.Approval = d.Approval;
            prod.ApprovalTgl = d.ApprovalTgl;
            prod.CatatanManager = d.CatatanManager;
            prod.ImgPicture = d.ImgPicture;
            prod.ReturnProduct = false;
            prod.StatusPenjualan = 0;

            db.StockProductLD.InsertOnSubmit(prod);
            db.SubmitChanges();

            int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductLD ORDER BY ID DESC",Db.Cnn_DB);

            foreach (var r in d.StockReceiveLD_Stone1B)
            {
                r.Nomor = Generate_Nomor(r);
                r.ImgPicture = "D:\\Images\\StockReceiveLD\\" + r.Stone1B.Nama +".jpg";
                db.SubmitChanges();

            }

            using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
            {
                conn.Open();

                // SELECT * (ganti LINQ)
                string sqlSelect = "SELECT * FROM StockReceiveLD_Stone1B WHERE IDForm = @IDForm";
                SqlCommand cmdSelect = new SqlCommand(sqlSelect, conn);
                cmdSelect.Parameters.AddWithValue("@IDForm", d.ID);

                SqlDataReader s1B = cmdSelect.ExecuteReader();

                if (!s1B.HasRows)
                {
                    s1B.Close();
                    return;
                }

                while (s1B.Read())
                {
                   
                    string sqlInsertProd = @"
                        UPDATE StockProductLD
                        SET IsConsignment = @IsConsignment
                        WHERE ID = @IDForm

                        INSERT INTO StockProductLD_Stone1B
                        (
                            Nomor, IDForm, IDStone, TotalButir, TotalCarat, CaratButir,
                            DimensiL, DimensiP, DimensiT,
                            HargaSatuan, HargaTotal,
                            GIA, HargaInputH, HargaInputP,
                            HargaSatuanM, HargaTotalM,
                            Efektif, Setting, ReturnProduct, StatusPenjualan,
                            ProductItem, HargaMInputH, HargaMInputP, IsConsignment
                        )
                        VALUES
                        (
                            @Nomor, @IDForm, @IDStone, @TotalButir, @TotalCarat, @CaratButir,
                            @DimensiL, @DimensiP, @DimensiT,
                            @HargaSatuan, @HargaTotal,
                            @GIA, @HargaInputH, @HargaInputP,
                            @HargaSatuanM, @HargaTotalM,
                            @Efektif, @Setting, 0, 0,
                            @ProductItem, @HargaMInputH, @HargaMInputP, @IsConsignment
                        );
                        SELECT SCOPE_IDENTITY();
                    ";

                    SqlCommand cmdInsert = new SqlCommand(sqlInsertProd, conn);

                    cmdInsert.Parameters.AddWithValue("@Nomor", s1B["Nomor"]);
                    cmdInsert.Parameters.AddWithValue("@IDForm", ProdID);
                    cmdInsert.Parameters.AddWithValue("@IDStone", s1B["IDStone"]);
                    cmdInsert.Parameters.AddWithValue("@TotalButir", s1B["TotalButir"]);
                    cmdInsert.Parameters.AddWithValue("@TotalCarat", s1B["TotalCarat"]);
                    cmdInsert.Parameters.AddWithValue("@CaratButir", s1B["CaratButir"]);
                    cmdInsert.Parameters.AddWithValue("@DimensiL", s1B["DimensiL"] == DBNull.Value ? (object)DBNull.Value : s1B["DimensiL"]);
                    cmdInsert.Parameters.AddWithValue("@DimensiP", s1B["DimensiP"]);
                    cmdInsert.Parameters.AddWithValue("@DimensiT", s1B["DimensiT"] == DBNull.Value ? (object)DBNull.Value : s1B["DimensiT"]);
                    cmdInsert.Parameters.AddWithValue("@HargaSatuan", s1B["HargaSatuan"]);
                    cmdInsert.Parameters.AddWithValue("@HargaTotal", s1B["HargaTotal"]);
                    cmdInsert.Parameters.AddWithValue("@GIA", s1B["GIA"]);
                    cmdInsert.Parameters.AddWithValue("@HargaInputH", s1B["HargaInputH"]);
                    cmdInsert.Parameters.AddWithValue("@HargaInputP", s1B["HargaInputP"]);
                    cmdInsert.Parameters.AddWithValue("@HargaSatuanM", s1B["HargaSatuanM"]);
                    cmdInsert.Parameters.AddWithValue("@HargaTotalM", s1B["HargaTotalM"]);
                    cmdInsert.Parameters.AddWithValue("@Efektif", s1B["Efektif"]);
                    cmdInsert.Parameters.AddWithValue("@Setting", s1B["Setting"]);
                    cmdInsert.Parameters.AddWithValue("@ProductItem", s1B["ProductItem"]);
                    cmdInsert.Parameters.AddWithValue("@HargaMInputH", s1B["HargaMInputH"]);
                    cmdInsert.Parameters.AddWithValue("@HargaMInputP", s1B["HargaMInputP"]);
                    cmdInsert.Parameters.Add("@IsConsignment", SqlDbType.Bit).Value = 0;

                    int newProductID = Convert.ToInt32(cmdInsert.ExecuteScalar());

                    StockActualLD_Stone1B stock = new StockActualLD_Stone1B();
                    stock.IDProduct = newProductID;
                    stock.NamaLokasi = d.LocWarehouse.Nama;
                    stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
                    stock.TipeLokasi = 0;
                    stock.IDLokasi = d.IDWarehouse;
                    stock.Substorage = 1;
                    stock.Keterangan = d.Keterangan;
                    stock.Tgl = d.Tgl;
                    stock.Nomor = s1B["Nomor"].ToString();
                    stock.InHand = 1;
                    stock.Operator = d.Operator;
                    stock.OperatorTgl = d.OperatorTgl;

                    db.StockActualLD_Stone1B.InsertOnSubmit(stock);
                    db.SubmitChanges();

                  
                    StockLedgerLD l = new StockLedgerLD();

                    decimal Value = 1;
                    int Remark = 4;
                    string Keterangan = s1B["Nomor"].ToString();
                    if (Keterangan.Trim() == "") 
                        Keterangan = DL.LedgerRemark(Remark);

                    l.IDProduct = newProductID;
                    l.TipeLokasi = 0;
                    l.IDLokasi = d.IDWarehouse;
                    l.NamaLokasi = d.LocWarehouse.Nama;
                    l.Value = Value;
                    l.Remark = Remark;
                    l.Keterangan = Keterangan;
                    l.Operator = App.UserNama;
                    l.OperatorTgl = DateTime.Now;

                    db.StockLedgerLD.InsertOnSubmit(l);
                    db.SubmitChanges();
                }

                s1B.Close();
            }


            // var s1B = from p in db.StockReceiveLD_Stone1B
            //           where p.IDForm == d.ID
            //           select p;
            // if (s1B != null)
            // {
            //     foreach (var r in s1B)
            //     {
            //         StockProductLD_Stone1B stone = new StockProductLD_Stone1B();
            //         stone.Nomor = r.Nomor;
            //         stone.IDForm = ProdID;
            //         stone.IDStone = r.IDStone;
            //         stone.TotalButir = r.TotalButir;
            //         stone.TotalCarat = r.TotalCarat;
            //         stone.CaratButir = r.CaratButir;
            //         stone.DimensiL = r.DimensiL;
            //         stone.DimensiP = r.DimensiP;
            //         stone.DimensiT = r.DimensiT;
            //         stone.HargaSatuan = r.HargaSatuan;
            //         stone.HargaTotal = r.HargaTotal;
            //         stone.GIA = r.GIA;
            //         stone.HargaInputH = r.HargaInputH;
            //         stone.HargaInputP = r.HargaInputP;
            //         stone.HargaSatuanM = r.HargaSatuanM;
            //         stone.HargaTotalM = r.HargaTotalM;
            //         stone.Efektif = r.Efektif;
            //         stone.Setting = r.Setting;
            //         stone.ReturnProduct = false;
            //         stone.StatusPenjualan = 0;
            //         stone.ProductItem = r.ProductItem;
            //         stone.HargaMInputH = r.HargaMInputH;
            //         stone.HargaMInputP = r.HargaMInputP;
            //         db.StockProductLD_Stone1B.InsertOnSubmit(stone);
            //         db.SubmitChanges();

            //         StockActualLD_Stone1B stock = new StockActualLD_Stone1B();
            //         stock.IDProduct = stone.ID;
            //         stock.NamaLokasi = d.LocWarehouse.Nama;
            //         stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
            //         stock.TipeLokasi = 0;
            //         stock.IDLokasi = d.IDWarehouse;
            //         stock.Substorage = 1;
            //         stock.Keterangan = d.Keterangan;
            //         stock.Tgl = d.Tgl;
            //         stock.Nomor = r.Nomor;
            //         stock.InHand = 1;
            //         stock.Operator = d.Operator;
            //         stock.OperatorTgl = d.OperatorTgl;
            //         db.StockActualLD_Stone1B.InsertOnSubmit(stock);
            //         db.SubmitChanges();

            //         // Ledger 
            //         StockLedgerLD l = new StockLedgerLD();

            //         // Value (See Library DB - Ledger for Value and Remark)
            //         decimal Value = 1; // 1 : in , -1 : out
            //         int Remark = 4; // See Library DB - Ledger
            //         string Keterangan = r.Nomor; // Commonly filled by document number or id 
            //         if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            //         //Input
            //         l.IDProduct = stone.ID;
            //         l.TipeLokasi = 0;
            //         l.IDLokasi = d.IDWarehouse;
            //         l.NamaLokasi = d.LocWarehouse.Nama;
            //         l.Value = Value;
            //         l.Remark = Remark;
            //         l.Keterangan = Keterangan;

            //         //User-Info
            //         l.Operator = App.UserNama;
            //         l.OperatorTgl = DateTime.Now;

            //         //Commit
            //         db.StockLedgerLD.InsertOnSubmit(l);
            //         db.SubmitChanges();
            //     }
            // }

            


            //Redirect
            App.ReloadClose(this);
        }
    }

    protected string Generate_Nomor(StockReceiveLD_Stone1B d)
    {
        string x = string.Empty;

        int Tahun = d.StockReceiveLD.Tgl.Year;
        int Bulan = d.StockReceiveLD.Tgl.Month;

        /*int Nomor = db.StockReceiveLD.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReceiveLD_Stone1B
                     where p.StockReceiveLD.Tgl.Year == Tahun && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "L";
        try
        {
            Kode = App.KodeLD;
        }
        catch (Exception ex) { }

        string Kode2 = convertCharPLU(Tahun.ToString().Remove(0, 2));

        string PLU = Kode + Kode2 + Str.KodeFormat(Nomor, 6);

        return PLU;

    }

    private string convertCharPLU(string number)
    {

        char alph1 = Convert.ToInt32(number.Substring(0, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(0, 1)) + 64);
        char alph2 = Convert.ToInt32(number.Substring(1, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(1, 1)) + 64);
        return alph1.ToString() + alph2.ToString();
    }
}