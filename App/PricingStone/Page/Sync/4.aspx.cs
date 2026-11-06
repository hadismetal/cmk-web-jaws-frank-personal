using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class Page_Sync_4 : System.Web.UI.Page
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

    protected int act { get { return App.QueryInt(this, "act"); } }
    protected string stone { get { return Request.QueryString["stone"]; } }
    protected string result { get { return Request.QueryString["result"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {           



            if (App.Refresh(this)) fillTable(sender, e);

            if ((Request.QueryString["act"] != null) || (Request.QueryString["stone"] != null) || (Request.QueryString["result"] != null))
            {
                if (act == 1)
                {
                    string strResult = string.Empty;



                    if (result == "1")
                    {
                        strResult = "Berhasil";
                        lit_syncresult.Text = "<h4><p class='alert alert-success' style='margin-bottom:0px'>Sync Stone " + stone + " " + strResult + "</p></h4>";
                    }
                    else
                    {
                        strResult = "Gagal";
                        lit_syncresult.Text = "<h4><p class='alert alert-danger' style='margin-bottom:0px'>Sync Stone " + stone + " " + strResult + "</p></h4>";
                    }


                }



            }


        }
    }


  

    protected string TD_Nama(string Nama, string NamaSource)
    {
        string x = string.Empty;
        string css = Nama.Trim() != NamaSource.Trim() ? " text-red" : "";

        x += "<p class=\"text-md " + css + "\">";
        x += Nama;
        x += "</p>";

        return x;
    }

    protected string TD_Approval(string Nama)
    {
        string x = string.Empty;

        x += "<p class=\"text-md\">";
        x += Nama;
        x += "</p>";

        return x;
    }


    protected string TD_Value_M(string Value,string ValueSource, bool isSelisih)
    {
        string x = string.Empty;
        string css = string.Empty;

        //cek jika angka sama dengan data source atau selisih minus (lebih kecil dari 0)
        if ((Value.Trim() != ValueSource.Trim()) || (isSelisih))
        {
            css = " text-red";
        }
        else
        {
            css = "";
        }

        x += "<p class=\"text-md " + css + "\">";
        x += Value;
        x += "</p>";

        return x;
    }

    protected string TD_Value(string Value, string ValueSource, bool isSelisih)
    {
        string x = string.Empty;
        string css = string.Empty;

        //cek jika angka sama dengan data source atau selisih minus (lebih kecil dari 0)
        if (isSelisih)
        {
            css = "";
        }
        else if (Value.Trim() != ValueSource.Trim())
        {
            css = " text-red";
        }
        else
        {
            css = "";
        }

        x += "<p class=\"text-md " + css + "\">";
        x += Value;
        x += "</p>";

        return x;
    }

    protected string TD_Warning(bool isSelisih)
    {
        string x = string.Empty;

        string css = string.Empty;

        css = isSelisih == true ? "<p class='text-danger'> <span class='icon-warning-sign icon-2x text-red'></span> Selisih minus </p>" : "";

        x += css;
       
       

        return x;
    }

    protected string TD_Sync(string xID, bool isSelisih, bool isSyncAll, string name, string harga, string modal, int db)
    {
        string x = string.Empty;

        string css = string.Empty;

        if ((name == "1") & (harga == "1") & (modal == "1"))
        {
            x += "<p></p>";
        }
        else if (isSelisih)
        {
            x += "<p></p>";
        }
        else
        { 

            if (isSyncAll)
            {
                x += "<p><a data-toggle='tooltip' title='Sync All'  class=\"btn btn-warning \" onClick =\"javascript: return confirm('Sync Stone to All Brand ?');\" href=\"Process_4.aspx?db=0&stone=4&noid=" + xID + "&mode=1\">";
                x += "  <i class=\"glyphicon glyphicon-transfer\"></i>Sync All";
                x += "</a></p>";

                
            }
            else
            {


                x += "<p><a class=\"btn btn-info\" href=\"Process_4.aspx?db=" + db.ToString() + "&stone=4&noid=" + xID + "&mode=1\"><i class=\"glyphicon glyphicon-transfer\"></i>Sync</a></p>";
            }
        }


     
        return x;
    }


    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {


        string zDb = "0";
        string zStone = "4";
        string zId = "0";
        string zMode = "0";
        string zList = "1";

        int xUserID = App.UserID;



        string spname = "dbo.sp_sync_all_stone";
        string query = " exec " + spname
               + " '" + zDb + "'"  //1
               + ", '" + zStone + "'" //2
               + ", '" + zId + "'" //3
               + ", '" + zMode + "'" //4
               + ", '" + zList + "'" //4
                ;


        try
            {
                DataTable dt = new DataTable();
                dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                string xresult = "";


            if (dt.Rows.Count > 0)
            {
                lit_display.Text = "<h4><p class='alert alert-info' style='margin-bottom:0px'>Total Display : " + dt.Rows.Count.ToString() + " Stone</p></h4>";


                int idx = 1;
                int idx_so = 1;
                string NOID_current = "";
                string NOID_next = "";


                string wlabel = "140";
                string sTitle = "";
                string sContent = "";


                foreach (DataRow dtRow in dt.Rows)
                {

                    //Mondial str
                    string xIDM = dtRow["IDM"].ToString();
                    string xNamaM = dtRow["NamaM"].ToString().Trim() != "" ? dtRow["NamaM"].ToString().Trim() : "-"  ;
                    string xModalM = Str.Number2(Convert.ToDecimal(dtRow["ModalM"].ToString()));
                    string xHargaM = Str.Number2(Convert.ToDecimal(dtRow["HargaM"].ToString()));                   
                    string xApprovalM = dtRow["ApprovalM"].ToString().Trim() != "" ? dtRow["ApprovalM"].ToString().Trim() : "-";
                    string xMutasiM = Str.Number2(Convert.ToDecimal(dtRow["MutasiM"]));
                    string xMutasiPersenM = Str.Number2(Convert.ToDecimal(dtRow["MutasiPersenM"]));
                    string xSelisihM = Str.Number2(Convert.ToDecimal(dtRow["SelisihM"]));
                    string xSelisihPersenM = Str.Number2(Convert.ToDecimal(dtRow["SelisihPersenM"]));
                    string xMatchNameM = dtRow["MatchNameM"].ToString();
                    string xMatchHargaM = dtRow["MatchHargaM"].ToString();
                    string xMatchModalM = dtRow["MatchModalM"].ToString();


                    //Frank str
                    string xIDF = dtRow["IDF"].ToString();
                    string xNamaF = dtRow["NamaF"].ToString().Trim() != "" ? dtRow["NamaF"].ToString().Trim() : "-";
                    string xModalF = Convert.ToDecimal(dtRow["ModalF"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["ModalF"].ToString())) : "-" ;
                    string xHargaF = Convert.ToDecimal(dtRow["HargaF"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["HargaF"].ToString())) : "-";  
                    string xApprovalF = dtRow["ApprovalF"].ToString().Trim() != "" ? dtRow["ApprovalF"].ToString().Trim() : "-";
                    string xMutasiF = Str.Number2(Convert.ToDecimal(dtRow["MutasiF"]));
                    string xMutasiPersenF = Str.Number2(Convert.ToDecimal(dtRow["MutasiPersenF"]));
                    string xSelisihF = Convert.ToDecimal(dtRow["SelisihF"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihF"].ToString())) : "-"; 
                    string xSelisihPersenF = Convert.ToDecimal(dtRow["SelisihPersenF"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihPersenF"].ToString())) : "-";
                    string xMatchNameF = dtRow["MatchNameF"].ToString();
                    string xMatchHargaF = dtRow["MatchHargaF"].ToString();
                    string xMatchModalF = dtRow["MatchModalF"].ToString();

                    //Palace str
                    string xIDP = dtRow["IDP"].ToString();
                    string xNamaP = dtRow["NamaP"].ToString().Trim() != "" ? dtRow["NamaP"].ToString().Trim() : "-";
                    string xModalP = Convert.ToDecimal(dtRow["ModalP"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["ModalP"].ToString())) : "-"; 
                    string xHargaP = Convert.ToDecimal(dtRow["HargaP"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["HargaP"].ToString())) : "-"; 
                    string xApprovalP = dtRow["ApprovalP"].ToString().Trim() != "" ? dtRow["ApprovalP"].ToString().Trim() : "-";
                    string xMutasiP = Str.Number2(Convert.ToDecimal(dtRow["MutasiP"]));
                    string xMutasiPersenP = Str.Number2(Convert.ToDecimal(dtRow["MutasiPersenP"]));
                    string xSelisihP = Convert.ToDecimal(dtRow["SelisihP"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihP"].ToString())) : "-"; 
                    string xSelisihPersenP = Convert.ToDecimal(dtRow["SelisihPersenP"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihPersenP"].ToString())) : "-";
                    string xMatchNameP = dtRow["MatchNameP"].ToString();
                    string xMatchHargaP = dtRow["MatchHargaP"].ToString();
                    string xMatchModalP = dtRow["MatchModalP"].ToString();

                    //Wholesale str
                    string xIDW = dtRow["IDW"].ToString();
                    string xNamaW = dtRow["NamaW"].ToString().Trim() != "" ? dtRow["NamaW"].ToString().Trim() : "-";
                    string xModalW = Convert.ToDecimal(dtRow["ModalW"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["ModalW"].ToString())) : "-";
                    string xHargaW = Convert.ToDecimal(dtRow["HargaW"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["HargaW"].ToString())) : "-"; 
                    string xApprovalW = dtRow["ApprovalW"].ToString().Trim() != "" ? dtRow["ApprovalW"].ToString().Trim() : "-";
                    string xMutasiW = Str.Number2(Convert.ToDecimal(dtRow["MutasiW"]));
                    string xMutasiPersenW = Str.Number2(Convert.ToDecimal(dtRow["MutasiPersenW"]));
                    string xSelisihW = Convert.ToDecimal(dtRow["SelisihW"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihW"].ToString())) : "-";
                    string xSelisihPersenW = Convert.ToDecimal(dtRow["SelisihPersenW"]) != 0m ? Str.Number2(Convert.ToDecimal(dtRow["SelisihPersenW"].ToString())) : "-";
                    string xMatchNameW = dtRow["MatchNameW"].ToString();
                    string xMatchHargaW = dtRow["MatchHargaW"].ToString();
                    string xMatchModalW = dtRow["MatchModalW"].ToString();

                    //selisih mondial
                    bool xIsSelisih = false;
                    xIsSelisih = Convert.ToDecimal(dtRow["SelisihM"]) < 0 ? true : false;

                    //handle
                    bool xIsDataEmpty = false;
                    xIsDataEmpty = xNamaF == "-" ? true : false;





                    NOID_next = xIDM;

                    if (NOID_current != NOID_next)
                    {
                       
                        xresult += "<div class='table-responsive' >";

                        xresult += "<table style='width:100%;table-layout:auto;' class='table-list'>"; //start table
                       // xresult += "<thead>"; //start head 1
                        xresult += "<tr >"; //start row 1   
                        xresult += "<th class='col-xs-12 col-sm-3 col-md-3'><p class='text-lg text-info'>Information</p></th>";
                        xresult += "<th class='col-xs-12 col-sm-3 col-md-3'><p class='text-lg text-info'>Mondial</p></th>";
                        xresult += "<th class='col-xs-12 col-sm-3 col-md-2'><p class='text-lg text-info'>Frank & co</p></th>";
                        xresult += "<th class='col-xs-12 col-sm-3 col-md-2'><p class='text-lg text-info'>The Palace</p></th>";
                        xresult += "<th class='col-xs-12 col-sm-3 col-md-2'><p class='text-lg text-info'>Wholesale</p></th>";
                        xresult += "</tr>"; //end row 1
                       // xresult += "</thead>"; //start head 1
                      //  xresult += "<tbody>"; //start body 1

                        xresult += "<tr class='danger'> "; //start row 1   
                        xresult += "<td class='info'><p><strong>Nama Batu</strong></p></td>";
                        xresult += "<td>" + TD_Nama(xNamaM, xNamaM) + "</td>";
                        xresult += "<td>" + TD_Nama(xNamaF, xNamaM) + "</td>";
                        xresult += "<td>" + TD_Nama(xNamaP, xNamaM) + "</td>";
                        xresult += "<td>" + TD_Nama(xNamaW, xNamaM) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr>"; //start row 1
                        xresult += "<td id='tdcolor'><p><strong>Approval</strong></p></td>";
                        xresult += "<td>" + TD_Approval(xApprovalM) + "</td>";
                        xresult += "<td>" + TD_Approval(xApprovalF) + "</td>";
                        xresult += "<td>" + TD_Approval(xApprovalP) + "</td>";
                        xresult += "<td>" + TD_Approval(xApprovalW) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr >"; //start row 1
                        xresult += "<td><p><strong>Cost (USD)</strong></p></td>";
                        xresult += "<td  >" + TD_Value_M(xModalM, xModalM, xIsSelisih) + "</td>";
                        xresult += "<td  >" + TD_Value(xModalF, xModalM, xIsSelisih) + "</td>";
                        xresult += "<td  >" + TD_Value(xModalP, xModalM, xIsSelisih) + "</td>";
                        xresult += "<td  >" + TD_Value(xModalW, xModalM, xIsSelisih) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr >"; //start row 1
                        xresult += "<td><p><strong>Harga (USD)</strong></p></td>";
                        xresult += "<td>" + TD_Value_M(xHargaM, xHargaM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xHargaF, xHargaM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xHargaP, xHargaM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xHargaW, xHargaM, xIsSelisih) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr>"; //start row 1
                        xresult += "<td><p><strong>Selisih (USD)</strong></p></td>";
                        xresult += "<td>" + TD_Value_M(xSelisihM, xSelisihM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihF, xSelisihM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihP, xSelisihM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihW, xSelisihM, xIsSelisih) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr>"; //start row 1
                        xresult += "<td><p><strong>Selisih Persen (%)</strong></p></td>";
                        xresult += "<td>" + TD_Value_M(xSelisihPersenM, xSelisihPersenM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihPersenF, xSelisihPersenM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihPersenP, xSelisihPersenM, xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Value(xSelisihPersenW, xSelisihPersenM, xIsSelisih) + "</td>";
                        xresult += "</tr>"; //end row 1

                        xresult += "<tr>"; //start row 1
                        xresult += "<td>" + TD_Sync(xIDM,xIsSelisih, true, xMatchNameM, xMatchHargaM, xMatchModalM, 0) + "</td>";
                        xresult += "<td>" + TD_Warning(xIsSelisih) + "</td>";
                        xresult += "<td>" + TD_Sync(xIDM, xIsSelisih, false, xMatchNameF, xMatchHargaF, xMatchModalF, 1) + "</td>";
                        xresult += "<td>" + TD_Sync(xIDM, xIsSelisih, false, xMatchNameP, xMatchHargaP, xMatchModalP, 2) + "</td>";
                        xresult += "<td>" + TD_Sync(xIDM, xIsSelisih, false, xMatchNameW, xMatchHargaW, xMatchModalW, 3) + "</td>";
                        xresult += "</tr>"; //end row 1


                        xresult += "</table>"; //end table
                        xresult += "</div>";
                      




                    }

               

                    idx++;

                }

            }


            if (dt.Rows.Count == 0)
            {
                zero.Visible = true;
                form_area.Visible = false;

            }

            lit_result.Text = xresult;
            }
            catch (Exception ex)
            {
            lit_errmsg.Text = "<div class='alert alert-danger' role='alert'>" + ex.Message.ToString() + "</div>";
            }

       
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "SyncDetail_Export.aspx?id=1";

      

        Response.Redirect(href);
    }

    protected void lbtnSyncAll_Click(object sender, EventArgs e)
    {
        
        string xDb = "0";            
        string xStone = "4";
        int xId = 0;
        int xMode = 0;
     

        string zDb = xDb;
        string zStone = xStone;
        string zId = xId.ToString();
        string zMode = xMode.ToString();

        string spname = "dbo.sp_sync_all_stone";
        string query = " exec " + spname
                        + " '" + zDb + "'"  //1
                        + ", '" + zStone + "'" //2
                        + ", '" + zId + "'" //3
                        + ", '" + zMode + "'" //4
                         ;



     

        string stone = string.Empty;
        string result = "0";
        int _strF = 0;
        int _strP = 0;
        int _strW = 0;

        try
        {

            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

            foreach (DataRow dtRow in dt.Rows)
            {


                stone = dtRow["Nama"].ToString();
                result = dtRow["Result"].ToString().Trim();
                _strF = Convert.ToInt32(dtRow["transF"].ToString().Trim());
                _strP = Convert.ToInt32(dtRow["transP"].ToString().Trim());
                _strW = Convert.ToInt32(dtRow["transW"].ToString().Trim());
            }

        }
        catch { }

        string href = App.RefreshURL(this);

        Response.Redirect(href);


       

    }
   
}