using System;
using System.Data;
using System.Linq;

public partial class Page_Sync_Process_2 : System.Web.UI.Page {
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

    protected string myDb { get { return Request.QueryString["db"]; } }
    protected string myStone { get { return Request.QueryString["stone"]; } }
    protected string myID { get { return Request.QueryString["noid"]; } }
    protected string myMode { get { return Request.QueryString["mode"]; } }
    


    //Exception
    protected void DataError() { Response.Redirect("2.aspx"); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);


        //////
        string xDb = "-1";
        try
        {
            xDb = myDb;
        }
        catch (Exception ex) { }
        /////
        string xStone = "2";
        try
        {
            xStone = myStone;
        }
        catch (Exception ex) { }

        int xId = 0;
        try
        {
            xId =Convert.ToInt32(myID);
        }
        catch (Exception ex) { }

        int xMode = -1;
        try
        {
            xMode = Convert.ToInt32(myMode);
        }
        catch (Exception ex) { }

        string zDb = xDb;
        string zStone = xStone;
        string zId = xId.ToString();
        string zMode = xMode.ToString();
        string zList = "0";

        string spname = "dbo.sp_sync_all_stone";
        string query = " exec " + spname
                        + " '" + zDb + "'"  //1
                        + ", '" + zStone + "'" //2
                        + ", '" + zId + "'" //3
                        + ", '" + zMode + "'" //4
                        + ", '" + zList + "'" //4
                         ;



        lit_result.Text = query;

        

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


        //string brand = string.Empty;
        //if((_strF == 0) && (_strP == 0) && (_strW == 0))
        //{
        //    brand = "0";
        //}

        Response.Redirect("2.aspx?refreshpage=1&act=1&stone=" + stone + "&result=" + result + "");

      

       
    }


   


}