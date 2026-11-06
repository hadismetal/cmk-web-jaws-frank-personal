<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales_InvoiceVoucherLEI.aspx.cs" Inherits="Page_Summary" %>

<%@ Register Src="~/UC/DetailPaymentControl.ascx" TagName="detailPaymentControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Order</title>
    <link rel="stylesheet" href="/UI/CSS/style_print.css" />
    <style>
        body {
            font-weight: bold;
        }

        .table-no, .table-no td {
            border: 0px solid #000 !important;
        }

        .noborder, .noborder tr, .noborder td {
            border: none !important;
        }
    </style>
</head>

<body>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>Untitled Document</title>
        <style>
            
        </style>
    </head>

    <body>
        <div id="judul1">
            <div class="judul_kiri">
                <h2 style="margin-bottom: 0px;">INVOICE GABUNGAN</h2>
                PT. CENTRAL MEGA KENCANA<br />
                <label runat="server" id="lblnpwp">NPWP : 01.764.536.7-073.000</label>
            </div>

            <div class="judul_kanan2">
                <h3 style="margin-bottom: 0px;">
                    <asp:Literal ID="cabang" runat="server"></asp:Literal></h3>
                <asp:Literal ID="alamat" runat="server"></asp:Literal>
            </div>

        </div>
        <div class="clear"></div>

        <div class="sub_jdul">
            <div class="judul_kiri">
                <table cellspacing="0" cellpadding="0" border="0" class="table-no">
                    <tr>
                        <td style="vertical-align:top; width:160px;">No. </td>
                        <td style="vertical-align:top; width:20px;">&nbsp;:&nbsp;</td>
                        <td style="vertical-align:top;"><asp:Literal ID="noinvoice" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top; width:160px;"><em>Date</em> / Tanggal Faktur</td>
                        <td style="vertical-align:top; width:20px;">&nbsp;:&nbsp;</td>
                        <td style="vertical-align:top;"><asp:Literal ID="tgl" runat="server"></asp:Literal></td>
                    </tr>
                </table>
            </div>
            <div class="judul_kanan" style="width: 550px;">
                <table cellspacing="0" cellpadding="0" border="0" class="table-no">
					<tr>
                        <td style="vertical-align:top; width:250px;"><em>Membership Code</em> / Kode Member</td>
                        <td style="vertical-align:top; width:20px;">&nbsp;:&nbsp;</td>
                        <td style="vertical-align:top;"><asp:Literal ID="nocustomer" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top; width:250px;"><em>Membership Name</em> / Nama Member</td>
                        <td style="vertical-align:top; width:20px;">&nbsp;:&nbsp;</td>
                        <td style="vertical-align:top;"><asp:Literal ID="customer" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top; width:250px;"><em>To</em> / Kepada</td>
                        <td style="vertical-align:top; width:20px;">&nbsp;:&nbsp;</td>
                        <td style="vertical-align:top;"><asp:Literal ID="customerother" runat="server" Text="Admin"></asp:Literal></td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="clear"></div>

        <div id="menu1"></div>
        
        <asp:Literal ID="headerlist" runat="server" />

        <div class="clear"></div>
        <div class="mnu_bwh"></div>
        <div class="clear"></div>

        <asp:Literal ID="itemlist" runat="server" />
        <br />

        <div class="sub_jdul">
            <div class="judul_kiri" style="width:70%;">
                <b><em>Say</em> / Terbilang :</b> <i>
                    <asp:Literal ID="terbilang" runat="server"></asp:Literal></i>

                <div id="detail_payment_box" runat="server">
                    <br />
                    <br />
                     <!-- adding firman 20180829 detail payment control
                           <asp:Literal ID="detail_payment" runat="server" /> -->
                    <uc1:detailPaymentControl ID="detailPaymentControl" runat="server" />                    
                    <br /><br />
                    <asp:Literal ID="detail_invoice" runat="server" />
                </div>
            </div>

            <div class="judul_kanan" style="border: 1px solid; padding: 5px; width:29%;">
				<input type="hidden" runat = "server" id="CountLE"/>
                <table class="table_invoice">
                    <tr>
                        <td><em>Total</em></td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top;">
                            <asp:Literal ID="nominal" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td><em>Payment</em> / Pembayaran</td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top;">
                            <asp:Literal ID="totaldp" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="border-top: solid 1px #000;"><em>Balance Due</em> / Sisa</td>
                        <td style="border-top: solid 1px #000;" width="10"></td>
                        <td style="border-top: solid 1px #000;" class="nominal" style="vertical-align:top;">
                            <asp:Literal ID="sisa" runat="server"></asp:Literal></td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="clear">&nbsp;</div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <div style="float: right;">
            <div style="width: 200px; text-align: center; padding: 30px; float: right;">
                <u><b>
                    <asp:Literal ID="adminsales" runat="server"></asp:Literal></b></u><br />
                <em>Jewellery Representative</em><br />
                Staff Penjualan
            </div>

            <div style="width: 200px; text-align: center; padding: 30px; float: right;">
                <u><b>
                    <asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br />
                <em>Customer</em> / Pelanggan
            </div>

        </div>

        <div class="clear"></div>
    </body>
    </html>


</body>
</html>
