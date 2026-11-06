using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Ddl
/// </summary>
public class Ddl
{
    public static void Substorage(DropDownList ddl)
    {
        string query = "SELECT ID, Nama FROM SubStorage";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Add(new ListItem("All", "0"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    public static void Location(DropDownList ddl, int UserID, DropDownList ddl1)
    {
        string query = @"
         DECLARE @TIPEACCOUNT INT = 0, @IsAm int = 0
        
        SELECT @TIPEACCOUNT = IIF(B.M_JABATAN LIKE '%AREA MANAGER%', 9, IIF(LTRIM(RTRIM(B.M_JABATAN)) IN ('ADMINISTRATIVE EXECUTIVE','ADMINISTRATIVE EXECUTIVE KELILING') 
        OR B.M_JABATAN LIKE '%JEWELLERY REPRESENTATIVE%' OR B.M_JABATAN LIKE '%STORE MANAGER%' OR B.M_JABATAN LIKE '%GOLD REPRESENTATIVE%' ,0,9)) 
        FROM palace_ac.dbo.UserAccount a
        JOIN [DB_PROD_MYAPPS].[dbhrd].[dbo].[mskaryawan] b on b.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS =a.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS
        WHERE a.ID=" + UserID + @";
        
        CREATE TABLE #tempLoc (TipeLokasi INT, IDLokasi INT, NamaLokasi NVARCHAR(100))

        INSERT INTO #tempLoc
        SELECT 1 AS TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi FROM LocOutlet WHERE Disable=0
        
        INSERT INTO #tempLoc
        SELECT 2 AS TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi FROM LocExhibition WHERE Disable=0

        select @IsAm = COUNT(*) FROM DataAreaManager WHERE IDUser= " + UserID + @"
        
        IF @TIPEACCOUNT=9
        BEGIN
            IF @IsAm > 0
            BEGIN
            SELECT * FROM (                
                    SELECT 1 AS TipeLokasi, 0 AS IDlokasi, 'All' as NamaLokasi
                    UNION
                    SELECT a.TipeLokasi, a.IDLokasi, a.NamaLokasi 
                    FROM #tempLoc a
                    JOIN DataAreaManager_Lokasi aa on aa.IDLokasi=a.IDLokasi and aa.TipeLokasi = a.TipeLokasi
                    JOIN DataAreaManager ab on ab.ID=aa.IDForm
                    WHERE ab.IDUser =" + UserID + @"
                )A ORDER BY NamaLokasi asc

                SELECT * FROM (
                    SELECT aa.TipeLokasi AS TipeLokasi, IIF(aa.TipeLokasi = 1, 'Outlet','Exhibition') NamaTipeLokasi
                    FROM DataAreaManager_Lokasi aa
                    JOIN DataAreaManager ab on ab.ID=aa.IDForm
                    WHERE ab.IDUser =" + UserID + @"
                    GROUP BY aa.TipeLokasi
                )A


            END
            ELSE
            BEGIN
                SELECT * FROM (                
                    SELECT TipeLokasi, IDLokasi, NamaLokasi  FROM #tempLoc a
                )A
                ORDER BY NamaLokasi ASC                
                
                select -1 as TipeLokasi, 'All' as NamaTipeLokasi
                UNION
                select 1 as TipeLokasi, 'Outlet' as NamaTipeLokasi
                UNION
                select 2 as TipeLokasi, 'Exhibition' as NamaTipeLokasi
            END
        END
        ELSE
        BEGIN
            select a.IDLokasi, a.TipeLokasi, a.NamaLokasi
            from #tempLoc a 
            LEFT JOIN DataAdminSales b on b.IDLokasi=a.IDLokasi and a.TipeLokasi=b.TipeLokasi
            JOIN palace_ac.dbo.UserAccount ac on ac.ID=b.IDUser 
            WHERE b.Disable=0  AND ac.ID =  " + UserID + @"

            SELECT aa.TipeLokasi AS TipeLokasi, IIF(aa.TipeLokasi = 1, 'Outlet','Exhibition') NamaTipeLokasi            
            FROM DataAdminSales AA
            JOIN palace_ac.dbo.UserAccount ac on ac.ID=AA.IDUser 
            WHERE AA.Disable=0  AND ac.ID =  " + UserID + @"

        END
        
        DROP TABLE #tempLoc
        ";
        DataSet ds = new DataSet();
        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);                
                adapter.Fill(ds);
            }
        }
        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[1];
        
        ddl.Items.Clear();
        foreach (DataRow item in dt.Rows)
        {
            string t = item["NamaLokasi"].ToString();
            string v = item["IDLokasi"].ToString();
            // ddl.Items.Add(new ListItem(t, v));
            ListItem data = new ListItem(t, v);
            
            // Add an extra field from database (e.g. Color)
            data.Attributes["TipeLokasi"] = item["TipeLokasi"].ToString();

            ddl.Items.Add(data);
            
        }

        if(dt1.Rows.Count > 0)
        {
            ddl1.Items.Clear();
            foreach (DataRow item in dt1.Rows)
            {
                string t = item["NamaTipeLokasi"].ToString();
                string v = item["TipeLokasi"].ToString();
                // ddl.Items.Add(new ListItem(t, v));
                ListItem data = new ListItem(t, v);                                            

                ddl1.Items.Add(data);
                
            }
        }
        
        if (ddl.Items.Count == 1)
        {
            ddl.Enabled = false;
            ddl1.Enabled = false;
        }
        else
        {
            ddl.Enabled = true;
            ddl1.Enabled = true;
        }
    }    

    public static void Store(DropDownList ddl)
    {
        string query = "SELECT Kode, UPPER(Nama) [Nama] FROM LocOutlet WHERE Draft = 0 AND Disable = 0 UNION ALL SELECT Kode, UPPER(Nama) [Nama] FROM LocExhibition WHERE Draft = 0 AND Disable = 0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        ddl.Items.Add(new ListItem("All", "0"));
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
	public static void StoreMondialMissMondial(DropDownList ddl)
    {
        string query = "SELECT Kode, UPPER(Nama) [Nama], 1 [Tipe], [ID] FROM mondial_db_real.dbo.LocOutlet WHERE Draft = 0 AND Disable = 0 UNION ALL SELECT Kode, UPPER(Nama) [Nama], 2 [Tipe], [ID] FROM mondial_db_real.dbo.LocExhibition WHERE Draft = 0 AND Disable = 0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        //ddl.Items.Add(new ListItem("All", "0"));
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Kode"].ToString() + " (" + item["Nama"].ToString().ToUpper() + ")";
            string v = item["Tipe"].ToString() + "-" + item["ID"].ToString() + "-" + item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    public static void StoreFrank(DropDownList ddl)
    {
        string query = "SELECT Kode, UPPER(Nama) [Nama], 1 [Tipe], [ID] FROM frank_db.dbo.LocOutlet WHERE Draft = 0 AND Disable = 0 UNION ALL SELECT Kode, UPPER(Nama) [Nama], 2 [Tipe], [ID] FROM frank_db.dbo.LocExhibition WHERE Draft = 0 AND Disable = 0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        //ddl.Items.Add(new ListItem("All", "0"));
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Kode"].ToString() + " (" + item["Nama"].ToString().ToUpper() + ")";
            string v = item["Tipe"].ToString() + "-" + item["ID"].ToString() + "-" + item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    public static void StorePalace(DropDownList ddl)
    {
        string query = "SELECT Kode, UPPER(Nama) [Nama], 1 [Tipe], [ID] FROM palace_db_real.dbo.LocOutlet WHERE Draft = 0 AND Disable = 0 UNION ALL SELECT Kode, UPPER(Nama) [Nama], 2 [Tipe], [ID] FROM palace_db_real.dbo.LocExhibition WHERE Draft = 0 AND Disable = 0 ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        //ddl.Items.Add(new ListItem("All", "0"));
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Kode"].ToString() + " (" + item["Nama"].ToString().ToUpper() + ")";
            string v = item["Tipe"].ToString() + "-" + item["ID"].ToString() + "-" + item["Kode"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
    public static DataTable SalesByStoreMondialMissMondial(string storecode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT A.ID, A.Nama, A.Keterangan, B.NamaKode FROM mondial_db_real.dbo.DataSales A " +
                       "JOIN mondial_db_real.dbo.DataSalesGroup B ON A.IDGroup = B.ID " +
                       "WHERE A.Draft = 0 AND A.Disable = 0 AND B.NamaKode = '" + storecode + "' ";
        dt = Db.Rs(query, Db.Cnn_DB);
        return dt;
    }
    public static DataTable SalesByStoreFrank(string storecode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT A.ID, A.Nama, A.Keterangan, B.NamaKode FROM frank_db.dbo.DataSales A " +
               "JOIN frank_db.dbo.DataSalesGroup B ON A.IDGroup = B.ID " +
               "WHERE A.Draft = 0 AND A.Disable = 0 AND B.NamaKode = '" + storecode + "' ";
        dt = Db.Rs(query, Db.Cnn_DB);
        return dt;
    }
    public static DataTable SalesByStorePalace(string storecode)
    {
        DataTable dt = new DataTable();
        string query = "SELECT A.ID, A.Nama, A.Keterangan, B.NamaKode FROM palace_db_real.dbo.DataSales A " +
               "JOIN palace_db_real.dbo.DataSalesGroup B ON A.IDGroup = B.ID " +
               "WHERE A.Draft = 0 AND A.Disable = 0 AND B.NamaKode = '" + storecode + "' ";
        dt = Db.Rs(query, Db.Cnn_DB);
        return dt;
    }
    public static void CompanyBrand(DropDownList ddl)
    {
        ddl.Items.Add(new ListItem("-", ""));
        ddl.Items.Add(new ListItem("MONDIAL", "M"));
        ddl.Items.Add(new ListItem("MISS MONDIAL", "S"));
        ddl.Items.Add(new ListItem("FRANK N CO", "F"));
        ddl.Items.Add(new ListItem("THE PALACE", "P"));
    }
	
	public static void CompanyBrand1(DropDownList ddl)
    {
        ddl.Items.Add(new ListItem("-", ""));
        ddl.Items.Add(new ListItem("MONDIAL", "M"));
        //ddl.Items.Add(new ListItem("FRANK N CO", "F"));
        ddl.Items.Add(new ListItem("THE PALACE", "P"));
    }
	
    public static void ResellPayment(DropDownList ddl)
    {
        string query = "SELECT ID, Nama FROM PaymentType WHERE ID IN (1,3) ";
        System.Data.DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
    }
	public static void GrafirLogoQC(DropDownList ddl)
	{
		ddl.Items.Add(new ListItem("Ada","Ada"));
		ddl.Items.Add(new ListItem("Tidak","Tidak"));
		ddl.Items.Add(new ListItem("Damage","Damage"));
	}
	public static void GrafirKadarEmasQC(DropDownList ddl)
	{
		ddl.Items.Add(new ListItem("Ada","Ada"));
		ddl.Items.Add(new ListItem("Tidak","Tidak"));
		ddl.Items.Add(new ListItem("Damage","Damage"));
	}
	public static void FisikBarangQC(DropDownList ddl)
    {
        ddl.Items.Add(new ListItem("Baik", "Baik"));
        ddl.Items.Add(new ListItem("Cacat", "Cacat"));
    }
	public static void Parcel101(DropDownList ddl)
	{
		string query = "SELECT ID, Nama FROM Parcel101 WHERE Disable = 0";
        System.Data.DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
	}
	public static void Parcel103(DropDownList ddl)
	{
		string query = "SELECT ID, Nama FROM Parcel103 WHERE Disable = 0";
        System.Data.DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
	}
	public static void Parcel104(DropDownList ddl)
	{
		string query = "SELECT ID, Nama FROM Parcel104 WHERE Disable = 0";
        System.Data.DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
	}
	public static void Parcel106(DropDownList ddl)
	{
		string query = "SELECT ID, Nama FROM Parcel106 WHERE Disable = 0";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }
	}
	public static void DropdownColor(DropDownList ddl)
    {
        ListItem li = new ListItem("D", "D");
        ddl.Items.Add(li);
        li = new ListItem("E", "E");
        ddl.Items.Add(li);
        li = new ListItem("F", "F");
        ddl.Items.Add(li);
        li = new ListItem("G", "G");
        ddl.Items.Add(li);
        li = new ListItem("H", "H");
        ddl.Items.Add(li);
        li = new ListItem("J", "J");
        ddl.Items.Add(li);
        li = new ListItem("K", "K");
        ddl.Items.Add(li);
        li = new ListItem("L", "L");
        ddl.Items.Add(li);
    }
    public static void DropdownShape(DropDownList ddl)
    {
        ListItem li = new ListItem("ROUND", "ROUND");
        ddl.Items.Add(li);
        li = new ListItem("PEAR SHAPE", "PEAR SHAPE");
        ddl.Items.Add(li);
        li = new ListItem("PRINCESS", "PRINCESS");
        ddl.Items.Add(li);
        li = new ListItem("HEART SHAPE", "HEART SHAPE");
        ddl.Items.Add(li);
        li = new ListItem("OVAL", "OVAL");
        ddl.Items.Add(li);
        li = new ListItem("EMERALD", "EMERALD");
        ddl.Items.Add(li);
        li = new ListItem("MARQUISE", "MARQUISE");
        ddl.Items.Add(li);
        li = new ListItem("CUSHION", "CUSHION");
        ddl.Items.Add(li);
        li = new ListItem("BAGUETTE", "BAGUETTE");
        ddl.Items.Add(li);
        li = new ListItem("ASSCHER CUT", "ASSCHER CUT");
        ddl.Items.Add(li);
        li = new ListItem("RADIANT", "RADIANT");
        ddl.Items.Add(li);
    }
    public static void DropdownClarity(DropDownList ddl)
    {
        ListItem li = new ListItem("IF", "IF");
        ddl.Items.Add(li);
        li = new ListItem("VVS1", "VVS1");
        ddl.Items.Add(li);
        li = new ListItem("VVS2", "VVS2");
        ddl.Items.Add(li);
        li = new ListItem("VS1", "VS1");
        ddl.Items.Add(li);
        li = new ListItem("VS2", "VS2");
        ddl.Items.Add(li);
        li = new ListItem("SI1", "SI1");
        ddl.Items.Add(li);
        li = new ListItem("SI2", "SI2");
        ddl.Items.Add(li);
    }
    public static void DropdownCutting(DropDownList ddl)
    {
        ListItem li = new ListItem("X-X-X (S1)", "X-X-X (S1)");
        ddl.Items.Add(li);
        li = new ListItem("X-X-VG (A1)", "X-X-VG (A1)");
        ddl.Items.Add(li);
        li = new ListItem("X-VG-VG (B1)", "X-VG-VG (B1)");
        ddl.Items.Add(li);
        li = new ListItem("VG-VG-VG (C1)", "VG-VG-VG (C1)");
        ddl.Items.Add(li);
        li = new ListItem("VG-VG-G (D1)", "VG-VG-G (D1)");
        ddl.Items.Add(li);
        li = new ListItem("VG-G-G (E1)", "VG-G-G (E1)");
        ddl.Items.Add(li);
        li = new ListItem("X-X (S2)", "X-X (S2)");
        ddl.Items.Add(li);
        li = new ListItem("X-VG (A2)", "X-VG (A2)");
        ddl.Items.Add(li);
        li = new ListItem("X-G (C2)", "X-G (C2)");
        ddl.Items.Add(li);
        li = new ListItem("VG-VG (B2)", "VG-VG (B2)");
        ddl.Items.Add(li);
        li = new ListItem("VG-G (D2)", "VG-G (D2)");
        ddl.Items.Add(li);
        li = new ListItem("G-G (E2)", "G-G (E2)");
        ddl.Items.Add(li);
        li = new ListItem("G-G-G (F1)", "G-G-G (F1)");
        ddl.Items.Add(li);
    }
    public static void DropdownBrand(DropDownList ddl)
    {
        ListItem li = new ListItem("FRANK FIRE (FF)", "FRANK FIRE (FF)");
        ddl.Items.Add(li);
        li = new ListItem("FIREMARK (FM)", "FIREMARK (FM)");
        ddl.Items.Add(li);
        li = new ListItem("GIRASOLE (GR)", "GIRASOLE (GR)");
        ddl.Items.Add(li);
        li = new ListItem("LIGHT OF MONDIAL (LM)", "LIGHT OF MONDIAL (LM)");
        ddl.Items.Add(li);
        li = new ListItem("MONDIAL EXCELLENT CUT (MC)", "MONDIAL EXCELLENT CUT (MC)");
        ddl.Items.Add(li);
        li = new ListItem("GENERIC (NM)", "GENERIC (NM)");
        ddl.Items.Add(li);
        li = new ListItem("BRILLIANT ROSE (BR)", "BRILLIANT ROSE (BR)");
        ddl.Items.Add(li);
        li = new ListItem("GENERIC NON 3X (GN)", "GENERIC NON 3X (GN)");
        ddl.Items.Add(li);
        li = new ListItem("FANCY SHAPE (NF)", "FANCY SHAPE (NF)");
        ddl.Items.Add(li);
        li = new ListItem("FANCY COLOR DIAMOND (FC)", "FANCY COLOR DIAMOND (FC)");
        ddl.Items.Add(li);
        li = new ListItem("FOREVERMARK (FV)", "FOREVERMARK (FV)");
        ddl.Items.Add(li);
    }
    // Start Hadits Kalkulator Pricing Stone
	public static void DropdownSize(DropDownList ddl)
    {
        string query = "SELECT ID, Nama FROM Parcel102 WHERE Disable = 0 AND Rapaport=1 and ID>=19 and NOT ID=54 order by SizeMin asc";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void CompanyBrand2(DropDownList ddl)
    {        
        ddl.Items.Add(new ListItem("MONDIAL", "M"));
        ddl.Items.Add(new ListItem("FRANK N CO", "F"){ Selected = true });
        ddl.Items.Add(new ListItem("THE PALACE", "P"));
    }
    public static void DropdownShapeCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel101 where Disable = 0 and NamaKode in ('DR','BL','EM','HS','MQ','CU','PR','OV','RT') ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void DropdownCatCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel107 where Disable = 0 and NamaKode in ('NN','U1','U2','U3') ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void DropdownClarityCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel104 where Disable = 0 and Nama in ('IF','VVS1','VVS2','VS1','VS2','SI1')";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void DropdownColorCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel103 where Disable = 0 and NamaKode in ('D','E','F','G','H','I','J') ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void DropdownCuttingCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel105 where Disable = 0 and NamaKode in ('S1','S2') ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    public static void DropdownBrandStoneCalc(DropDownList ddl){
        string query = "select ID,Nama from Parcel106 where Disable = 0 and NamaKode in ('FF','FM','LM','MC','NM','SR','NF','FV')";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        foreach (System.Data.DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            ddl.Items.Add(new ListItem(t, v));
        }   
    }
    // End Hadits Kalkulator Pricing Stone
}