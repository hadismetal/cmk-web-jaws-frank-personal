using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_ApprovalCode : System.Web.UI.Page
{
    private string msg { get { return Request.QueryString["msg"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        /*if (!string.IsNullOrEmpty(nomorso))
            so.Text = nomorso;
        if (!string.IsNullOrEmpty(iso))
            ddltipe.SelectedValue = iso == "T" ? "SO" : "DP";
        if (!string.IsNullOrEmpty(nomorso) && !string.IsNullOrEmpty(iso)) filltable();*/
        DLC.EDCList(ddledc as DropDownList);
        //if (!string.IsNullOrEmpty(msg))
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "msgScript", "$('#lblmsg').text('" + msg + "'); $('#divalert').modal('show');", true);
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        filltable();
    }
    protected void filltable()
    {
        if (!string.IsNullOrEmpty(so.Text))
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            if (ddltipe.SelectedItem.Value == "SO")
            {
                dt = Db.Rs("SELECT TOP 1000 C.ID [IDPaymentType], ISNULL(C.IDEDC,0) [IDEDC] , B.Nomor [NomorSO], D.Nama [PaymentType], ISNULL(E.Nama,'-') [Bank], ISNULL(G.TransactionNumber,'-') [ApprovalCode], ISNULL(F.Nama,'-') [EDCBank], C.Nominal FROM SalesReceipt A " +
                    "JOIN SalesOrder B ON A.IDSalesOrder = B.ID " +
                    "JOIN SalesReceiptDetail C ON A.ID = C.IDSalesReceipt " +
                    "JOIN PaymentType D ON C.IDPaymentType = D.ID " +
                    "LEFT JOIN BankIssuer E ON C.IDBankIssuer = E.ID " +
                    "LEFT JOIN EDCList F ON C.IDEDC = F.ID " +
                    "LEFT JOIN SalesReceiptDetailExtension G ON C.ID = G.ID " +
                    "WHERE IDPaymentType IN (2,4,13,15) AND C.VoidReference IS NULL AND B.StatusPembayaran = 1 AND B.StatusVoid = 0 AND B.Draft = 0  AND B.Nomor = '" + so.Text + "' " +
                    "ORDER BY C.ID DESC ", Db.Cnn_DB);
            }
            else
            {
                dt = Db.Rs("SELECT TOP 1000 B.ID [IDPaymentType], ISNULL(B.IDEDC,0) [IDEDC], A.NomorDP [NomorSO], D.Nama [PaymentType], ISNULL(E.Nama,'-') [Bank], ISNULL(G.TransactionNumber,'-') [ApprovalCode], ISNULL(F.Nama,'-') [EDCBank], B.Nominal FROM SalesReceiptDPPO A " +
                    "JOIN SalesReceiptDPPODetail B ON A.ID = B.IDSalesReceipt " +
                    "JOIN PaymentType D ON B.IDPaymentType = D.ID " +
                    "LEFT JOIN BankIssuer E ON B.IDBankIssuer = E.ID " +
                    "LEFT JOIN EDCList F ON B.IDEDC = F.ID " +
                    "LEFT JOIN SalesReceiptDPPODetailExtension G ON B.ID = G.ID " +
                    "WHERE IDPaymentType IN(2,4,13,15) AND " +
                    "A.StatusDP <> 2 AND A.Draft = 0 AND A.NomorDP = '" + so.Text + "' " +
                    "ORDER BY B.ID DESC ", Db.Cnn_DB);
            }
            if (dt.Rows.Count > 0)
            {
                System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                nfi.NumberDecimalDigits = 2;
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    td.Text = dr["NomorSO"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["PaymentType"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Bank"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["ApprovalCode"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["EDCBank"].ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Convert.ToDecimal(dr["Nominal"]).ToString("N2", nfi);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<button type='button' class='btn btn-success' id='btn-update-" + dr["IDPaymentType"] + "' data-appcode='" + (dr["ApprovalCode"] ?? "") + "' data-id='" + dr["IDPaymentType"] + "' data-idedc='" + dr["IDEDC"] + "' data-edc='" + dr["EDCBank"] + "' data-nominal='" + Convert.ToDecimal(dr["Nominal"]).ToString("N2", nfi) + "' onclick=\"onClickModalUpdateAppCode(this);\">Update</button>";
                    td.Text += "<script type='text/javascript'>function onClickModalUpdateAppCode(e){ document.getElementById('txtappcodemodal').value = e.getAttribute('data-appcode'); document.getElementById('content_ddledc').value = e.getAttribute('data-idedc'); document.getElementById('txtidtable').value = e.getAttribute('data-id'); document.getElementById('txtnominalmodal').value = e.getAttribute('data-nominal'); document.getElementById('content_ddledc').value = e.getAttribute('data-idedc'); document.getElementById('textisdp').value = (document.getElementById('content_ddltipe').value == 'DP') ? 'T' : 'F'; $('#modalupdateappcode').modal('show'); }</script>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    tblPayment.Rows.Add(tr);
                }
                so_RF.Visible = false;
                so_RF.Text = "";
            }
            else
            {
                so_RF.Visible = true;
                so_RF.Text = "<p style='color:red'>*Data Not Found.</p>";
                so.Focus();
            }
        }
        else
        {
            so_RF.Visible = true;
            so_RF.Text = "<p style='color:red'>Fill The Blank Text.</p>";
            so.Focus();
        }
    }
}