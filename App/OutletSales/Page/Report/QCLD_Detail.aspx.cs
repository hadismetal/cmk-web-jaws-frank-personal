using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Page_Report_QCLD_Detail : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        var data = db.DocQCLD.SingleOrDefault(p => p.ID == myID).StockProductLD_Stone1B.StockActualLD_Stone1B;
        var so = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == data.IDProduct && p.SalesOrder.StatusPembayaran == true && p.SalesOrder.StatusVoid == false);
        var locationbrand_outlet = db.LocOutlet;
        var locationbrand_exhibition = db.LocExhibition;

        string CB = ""; // mondial, franknco, missmondial, thepalace
        string Alamat = "";
        if (data.TipeLokasi == 1)
        {
            CB = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama;
            Alamat = locationbrand_outlet.SingleOrDefault(p => p.ID == data.IDLokasi).AddrAlamat;
        }
        else if (data.TipeLokasi == 2)
        {
            CB = locationbrand_exhibition.SingleOrDefault(p => p.ID == data.IDLokasi).CompanyBrand.Nama;
            Alamat = locationbrand_exhibition.SingleOrDefault(p => p.ID == data.IDLokasi).AddrAlamat;
        }

        CB = CB.ToLower();

        if (CB == "mondial") { img_logo_web.Src = "/Images/web/logo_web_mondial.jpg"; }
        else if (CB == "miss mondial") { img_logo_web.Src = "/Images/web/logo_web_missmondial.jpg"; }
        else if (CB == "the palace") { img_logo_web.Src = "/Images/web/logo_web_thepalace.jpg"; }
        else if (CB == "frank & co") { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }
        else { img_logo_web.Src = "/Images/web/logo_web_franknco.jpg"; }

        cabang.Text = Alamat;
        lit_nomor_repair.Text = db.DocQCLD.SingleOrDefault(p => p.ID == myID).Nomor;

        if (so != null)
        {
            invoice.InnerText = so.Invoice;
            cust.InnerText = so.SalesOrder.CustomerNama;
            if (so.SalesOrder.TipeLokasi == 1)
                storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocOutlet WHERE ID = " + so.SalesOrder.IDLokasi, Db.Cnn_DB);
            else if (so.SalesOrder.TipeLokasi == 2)
                storeasal.InnerText = Db.SingleString("SELECT TOP 1 UPPER(Nama) [Nama] FROM LocExhibition WHERE ID =" + so.SalesOrder.IDLokasi, Db.Cnn_DB);
            hg_beli.InnerText = "IDR " + (Convert.ToInt64(so.TotalBayar)).ToString("N0");
        }
        fillForm();
    }

    protected void fillForm()
    {
        var q = db.DocQCLD.SingleOrDefault(p => p.ID == myID);
        if (q.StockProductLD_Stone1B == null) App.DataNull(this);
        else
        {
            nodocqc.InnerText = q.Nomor;
            tgldocqc.InnerText = Convert.ToDateTime(q.OperatorTgl).ToString("dd-MM-yyyy");
            plu.InnerText = q.StockProductLD_Stone1B.Nomor;
            gia.InnerText = q.StockProductLD_Stone1B.GIA;
            petugasqc.InnerText = q.QCNama.ToUpper();
            sertifikat.InnerText = q.Sertifikat;
            valbeli.InnerText = Db.SingleDecimal("SELECT TOP 1 ISNULL(HargaJualCustomer,0) [HargaJualCustomer] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N0");
            item.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Item,'') [Item] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            weight.InnerText = Db.SingleDecimal("SELECT TOP 1 ISNULL(Carat,0) [Carat] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N3");
            shape.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Shape,'') [Shape] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            cla.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Clarity,'') [Clarity] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            col.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Colour,'') [Color] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            cut.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Cutting,'') [Cutting] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            brand.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(Brand,'') [Brand] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            isanycertificate.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckSertifikatBySM,'') [CheckSertifikatBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            isanyinvoice.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckInvoiceBySM,'') [CheckInvoiceBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            laserincription.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckLaserIncriptionBySM,'') [CheckLaserIncriptionBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            measurement.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckMeasurementBySM,'') [CheckMeasurementBySM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            kondisibatu.InnerText = Db.SingleString("SELECT TOP 1 ISNULL(CheckKondisiBatu,'') [CheckKondisiBatu] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
            keterangan.InnerText = q.Keterangan;
            createdby.InnerText = q.QCNama.ToUpper();
        }

        string approval = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurement,'') [ApprovalProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string approval2 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalProcurementFinal,'') [ApprovalProcurementFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string approval3 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalSM,'') [ApprovalSM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string approval4 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalAM,'') [ApprovalAM] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string approval5 = Db.SingleString("SELECT TOP 1 ISNULL(ApprovalFinal,'') [ApprovalFinal] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        string keteranganproc = Db.SingleString("SELECT TOP 1 ISNULL(KeteranganProcurement,'') [KeteranganProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        decimal hargaproc = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [KeteranganProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);

        string usernumber = Db.SingleString("SELECT TOP 1 ISNULL(UserNumber,'') [UserNumber] FROM UserAccount WHERE ID = " + App.UserID, Db.Cnn_AC);
        int countproc = Db.SingleInteger("SELECT COUNT('') [Total] FROM DataProcurement WHERE Disable = 0 AND NIK = '" + usernumber + "'", Db.Cnn_DB);
        int countproc2 = Db.SingleInteger("SELECT COUNT('') [Total] FROM DataProcurement WHERE Disable = 0 AND Role = 'Supervisor' AND NIK = '" + usernumber + "'", Db.Cnn_DB);

        string query = "SELECT ApprovalSMTgl, ApprovalAMTgl, ApprovalProcurementFinalTgl, ApprovalProcurementTgl FROM DocQCLD WHERE ID = @ID";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataTable dr = Rs(query, Db.Cnn_DB, parameters);
        DateTime? tglapproveSMAM = dr.Rows[0]["ApprovalSMTgl"] != DBNull.Value ? Convert.ToDateTime(dr.Rows[0]["ApprovalSMTgl"]) : (dr.Rows[0]["ApprovalAMTgl"] != DBNull.Value ? Convert.ToDateTime(dr.Rows[0]["ApprovalAMTgl"]) : (DateTime?)null);
        DateTime? tglapprovefinal = dr.Rows[0]["ApprovalProcurementFinalTgl"] != DBNull.Value ? Convert.ToDateTime(dr.Rows[0]["ApprovalProcurementFinalTgl"]) : (DateTime?)null;
        DateTime? tglapprove = dr.Rows[0]["ApprovalProcurementTgl"] != DBNull.Value ? Convert.ToDateTime(dr.Rows[0]["ApprovalProcurementTgl"]) : (DateTime?)null;
        string statusExpired = StatusExpired(myID, tglapproveSMAM, tglapprove, tglapprovefinal);

        string ekstension = Db.SingleString("SELECT TOP 1 ISNULL(Extension,'[]') [Extension] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB);
        try
        {
            List<ExtensionLD> test = JsonConvert.DeserializeObject<List<ExtensionLD>>(ekstension);
            fillExtensions(test);
        }
        catch { }

        if (approval3 == "")
        {
            approvesmby.InnerText = "";
        }
        else
        {
            approvesmby.InnerText = approval3;
        }
        if (approval4 == "")
        {
            approveamby.InnerText = "";
        }
        else
        {
            approveamby.InnerText = approval4;
        }
        if (approval5 == "")
        {
            approvemerchfinalby.InnerText = "";
        }
        else
        {
            approvemerchfinalby.InnerText = approval5;
        }

        if (approval != "")
        {
            divprint2.Visible = true;
            divprint.Visible = true;
            divapprove.Visible = false;
            panelapproved.Visible = false;
            panelnotapproved.Visible = false;
            divqcproc.Visible = false;
            lblwarning.Visible = false;
            divfinalprice.Visible = true;
            btnEdit.Visible = false;
            approveprocurementby.InnerText = approval;

            val.InnerText = Db.SingleDecimal("SELECT TOP 1 CAST(ISNULL(HargaProcurement,0) AS MONEY) [HargaProcurement] FROM DocQCLD WHERE ID = " + myID, Db.Cnn_DB).ToString("N2");

            hargajualcust_1.Text = "$ " + Convert.ToDecimal(hargaproc).ToString("N2");
            keteranganprocurement_1.Text = keteranganproc;
            operatorprocurement.Text = approval;

            if (approval2 != "")
            {
                divprint2.Visible = false;
                divprint.Visible = true;
                divapprove.Visible = false;
                btnApprove.Visible = false;
                btnApproveFinal.Visible = false;
                btnEdit.Visible = false;
                approveprocurementfinalby.InnerText = approval2;
            }
            else
            {
                divprint2.Visible = false;
                divprint.Visible = false;
                divapprove.Visible = false;
                btnApprove.Visible = false;
                btnApproveFinal.Visible = false;
                btnEdit.Visible = false;
                approveprocurementfinalby.InnerText = approval2;
                lblwarning.InnerText = statusExpired == string.Empty ? "Need Approval Final. Please Contact Procurement." : statusExpired;
                lblwarning.Visible = true;
                divfinalprice.Visible = false;

            }
        }
        else
        {
            divprint2.Visible = false;
            divprint.Visible = false;
            divapprove.Visible = false;
            btnApprove.Visible = false;
            btnApproveFinal.Visible = false;
            btnEdit.Visible = false;
            lblwarning.Visible = true;
            lblwarning.InnerText = statusExpired == string.Empty ? "Need Approval. Please Contact Procurement." : statusExpired;
            divfinalprice.Visible = false;
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {

    }
    protected void btnApproveFinal_Click(object sender, EventArgs e)
    {

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        divqcproc.Visible = false;
        btnEdit.Visible = false;
        divapprove.Visible = false;
        divprint.Visible = false;
        panelapproved.Visible = false;
        panelnotapproved.Visible = false;
    }

    public class ExtensionLD
    {
        public string clarity { get; set; }
        public string position { get; set; }
        public string number { get; set; }
        public string size { get; set; }
    }

    protected void fillExtensions(List<ExtensionLD> test)
    {
        int index = 1;
        tbextension.Dispose();
        tbextension.Rows.Clear();

        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thd = new TableHeaderCell();
        thd.Text = "No.";
        thr.Cells.Add(thd);

        thd = new TableHeaderCell();
        thd.Text = "Clarity Characteristic";
        thr.Cells.Add(thd);

        thd = new TableHeaderCell();
        thd.Text = "Size";
        thr.Cells.Add(thd);

        thd = new TableHeaderCell();
        thd.Text = "Number";
        thr.Cells.Add(thd);

        thd = new TableHeaderCell();
        thd.Text = "Position";
        thr.Cells.Add(thd);

        tbextension.Rows.Add(thr);

        foreach (var x in test)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index.ToString();
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = x.clarity;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = x.position;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = x.number;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = x.size;
            tr.Cells.Add(td);

            tbextension.Rows.Add(tr);
            index++;
        }
    }

    protected string StatusExpired(int IDDoc, DateTime? TglApprovalSMAM, DateTime? TglApproveProc, DateTime? TglApproveProcFinal)
    {
        string x = string.Empty;

        if (TglApprovalSMAM.HasValue)
        {
            if (TglApproveProc.HasValue && !TglApproveProcFinal.HasValue)
            {
                TimeSpan ts = DateTime.Now.Date.Subtract(TglApproveProc.Value.Date);
                if (ts.Days <= 7)
                {
                    x += "";
                }
                else
                {
                    x += "QC above 7 days before approve procurement is expired. Please create new QC.";
                }
            }
            else
            {
                x += "";
            }
        }
        else
        {
            x += "";
        }

        return x;
    }

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = CommandType.Text;
                foreach (var x in param)
                {
                    cmd.Parameters.Add(x);
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