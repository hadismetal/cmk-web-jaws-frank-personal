using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;

public partial class Page_Summary : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Report Reparasi Service Point.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int TipeLokasi = Convert.ToInt32(Request.QueryString["TipeLokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["IDLokasi"]);
        string KDBrand = Convert.ToString(Request.QueryString["KodeBrand"]);
        string KW = Convert.ToString(Request.QueryString["Keywords"]);
        string TglAwal = Convert.ToString(Request.QueryString["TglAwal"]);
        string TglHingga = Convert.ToString(Request.QueryString["TglHingga"]);       

    
        string exportReportSP = "sp_getListRepairServicePoint_Report";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(exportReportSP, connection))
            {                
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeBrand", SqlDbType.VarChar) { Value = KDBrand });
                command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = KW });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@TglAwal", SqlDbType.VarChar) { Value = TglAwal });
                command.Parameters.Add(new SqlParameter("@TglHingga", SqlDbType.VarChar) { Value = TglHingga });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                int no = 0;

                Response.Write(dt.Rows.Count);

                foreach (DataRow rs in dt.Rows)
                {
                    no++;
                    TableRow tr2 = new TableRow();
                    tr2.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter                                         
                                                                     //dj        
                    AddFooterCell(tr2, "No", Convert.ToString(no));
                    AddFooterCell(tr2, "TglTerima", Convert.ToString(rs["TglTerima"]));
                    AddFooterCell(tr2, "Perajin", Convert.ToString(rs["Perajin"]));
                    AddFooterCell(tr2, "NIK", Convert.ToString(rs["NIKPengrajin"]));
                    AddFooterCell(tr2, "NoBON", Convert.ToString(rs["Nomor"]));
                    AddFooterCell(tr2, "KodeToko", Convert.ToString(rs["NamaAsal"]));
                    AddFooterCell(tr2, "ServicePoint", Convert.ToString(rs["NamaServicePoint"]));
                    AddFooterCell(tr2, "JenisBarang", Convert.ToString(rs["Item"]));
                    AddFooterCell(tr2, "KodePLU", Convert.ToString(rs["PLU"]));
                    AddFooterCell(tr2, "PaketUtama", Convert.ToString(rs["PaketUtam"]));
                    AddFooterCell(tr2, "PaketTambahan", Convert.ToString(rs["PaketTambahan"]));
                    AddFooterCell(tr2, "KetPekerjaan", Convert.ToString(rs["Keterangan"]));
                    AddFooterCell(tr2, "FrameColor", Convert.ToString(rs["FrameColor"]));
                    AddFooterCell(tr2, "SizeAwal", string.Format("{0:N2}", Convert.ToDecimal(rs["SizeAwal"])));
                    AddFooterCell(tr2, "BeratAwal", string.Format("{0:N2}", rs["WeightActual"]));
                    AddFooterCell(tr2, "BeratAkhir", string.Format("{0:N2}", rs["BeratTimbangAkhir"]));
                    AddFooterCell(tr2, "SizePolesAkhir", string.Format("{0:N2}", rs["SizeAkhir"]));
                    AddFooterCell(tr2, "BeratPolesAkhir", string.Format("{0:N2}", rs["BeratTimbang"]));
                    AddFooterCell(tr2, "TglSelesai", Convert.ToString(rs["TglSelesai"]));
                    AddFooterCell(tr2, "TglKirim", Convert.ToString(rs["TglKirim"]));

                    // AddFooterCell(tr2, "SizeAwal", Convert.ToDecimal(rs["SizeAwal"])).ToString("N2");
                    // AddFooterCell(tr2, "BeratAwal", Convert.ToDecimal(rs["WeightActual"])).ToString("N2");
                    // AddFooterCell(tr2, "BeratAkhir", Convert.ToDecimal(rs["BeratTimbangAkhir"])).ToString("N2");
                    // AddFooterCell(tr2, "SizePolesAkhir", Convert.ToDecimal(rs["SizeAkhir"])).ToString("N2");
                    // AddFooterCell(tr2, "BeratPolesAkhir", Convert.ToDecimal(rs["BeratTimbang"])).ToString("N2");
                    // AddFooterCell(tr2, "TglSelesai", Convert.ToString(rs["TglSelesai"]));
                    // AddFooterCell(tr2, "TglKirim", Convert.ToString(rs["TglKirim"]));

                    tb.Rows.Add(tr2);

                }
            }
        }            
    
    }
    
    private void AddFooterCell(TableRow tr,string idElement, string value)
    {
        TableCell cell = new TableCell();            
        cell.ID = idElement;        
        cell.Attributes.Add("style", "border:1px solid #3A3939; "); // Add border style and make a little disappear
        cell.HorizontalAlign = HorizontalAlign.Center; // Center align the text
        cell.Text = value.ToString();  // Convert value to string        
        tr.Cells.Add(cell);  // Add the cell to the row
    }
}