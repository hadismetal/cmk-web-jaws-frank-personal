<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales_Invoice.aspx.cs" Inherits="Page_Summary" %>

<%@ Register Src="DetailPaymentControl.ascx" TagName="detailPaymentControl" TagPrefix="uc1" %>

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
					<asp:Panel runat="server" ID="paymentwithlakuemaspanel" Visible="false">
					<tr>
                        <td width="390">Pembayaran Emas Murni Kadar 24 Karat</td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top">
                            <asp:Literal runat="server" ID="le"></asp:Literal>
                        </td>
                    </tr>
					</asp:Panel>
					<asp:Panel runat="server" ID="paymentwithdebitlakuemaspanel" Visible="false">
					<tr>
                        <td width="390">Pembayaran Emas PG 75%</td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top">
                            <asp:Literal runat="server" ID="debitlepg"></asp:Literal>
                        </td>
                    </tr>
					<tr>
                        <td width="390">Pembayaran Emas Murni Kadar 24 Karat</td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top">
                            <asp:Literal runat="server" ID="debitlelm"></asp:Literal>
                        </td>
                    </tr>
					</asp:Panel>
                    <tr>
                        <td width="390"><label runat="server" id="lblppn10"><em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN </label></td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top;">
                            <asp:Literal ID="dpp" runat="server"></asp:Literal></td>
                    </tr>
					<asp:Panel runat="server" ID="divuseppn">
                    <tr>
                        <td width="390" style="font-size: 14px;"><em>VAT</em> / PPN<div style="display:none"><br />(<label runat="server" id="lblppn">10%</label> x 20% x Harga Jual Tidak Termasuk PPN)</div></td>
                        <td width="10"></td>
                        <td class="nominal" style="vertical-align:top;">
                            <asp:Literal ID="ppn" runat="server"></asp:Literal></td>
                    </tr>
					</asp:Panel>
                    <tr>
                        <td style="border-top: solid 1px #000;"><em>Total</em></td>
                        <td style="border-top: solid 1px #000;" width="10"></td>
                        <td style="border-top: solid 1px #000;" class="nominal" style="vertical-align:top;">
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
        <div style="float: left; font-size: 14px;">
            <div id="frank_term" runat="server" visible="false">
                <br />
                <em>Catatan</em> / Notes :
                    <ol style="margin-top: 0px; padding-top: 0px;">
                        <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                        <asp:Panel runat="server" ID="lit_ppn_notice"><li><em>Price including VAT <div style="display:none"><asp:Literal runat="server" id="lit_ppn" /></div></em> / Harga Jual Termasuk PPN <div style="display:none"><asp:Literal runat="server" id="lit_ppn2" /></div></li></asp:Panel>
                        <li><label runat="server" id="lbl_ppn_notice"><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</label></li>
                        <li><em>Frank & Co. guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                            <br /><em>registered jeweler at lower price,  Frank & Co. will refund in full at purchase price.</em>
                            <br />Frank & Co. menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                            <br />di Indonesia yang terdaftar dengan harga yang lebih murah, Frank & Co. akan mengembalikan keseluruhan uang dari pembelian
                        </li>
                        
                        <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja</li>
                    </ol>
                </em>
            </div>
            <div id="mondial" runat="server" visible="false">
                <br />
                <em>Catatan</em> / Notes :
                    <ol style="margin-top: 0px; padding-top: 0px;">
                        <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                        <asp:Panel runat="server" id="lit_ppn_notice2"><li><em>Price including VAT <div style="display:none"><asp:Literal runat="server" id="lit_ppn3" /></div></em> / Harga Jual Termasuk PPN <div style="display:none"><asp:Literal runat="server" id="lit_ppn4" /></div></li></asp:Panel>
                        <li><label runat="server" id="lbl_ppn_notice2"><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</label></li>
                        <li><em>Mondial guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                            <br /><em>registered jeweler at lower price,  Mondial will refund in full at purchase price.</em>
                            <br />Mondial menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                            <br />di Indonesia yang terdaftar dengan harga yang lebih murah, Mondial akan mengembalikan keseluruhan uang dari pembelian
                        </li>
                        
                        <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja</li>
                    </ol>
                </em>
            </div>
            <div id="missmondial" runat="server" visible="false">
                <br />
                <em>Catatan</em> / Notes :
                    <ol style="margin-top: 0px; padding-top: 0px;">
                        <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                        <asp:Panel runat="server" id="lit_ppn_notice3"><li><em>Price including VAT <div style="display:none"><asp:Literal runat="server" id="lit_ppn5" /></div></em> / Harga Jual Termasuk PPN <div style="display:none"><asp:Literal runat="server" id="lit_ppn6" /></div></li></asp:Panel>
                        <li><label runat="server" id="lbl_ppn_notice3"><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</label></li>
                        <li><em>Miss Mondial guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                            <br /><em>registered jeweler at lower price, Miss Mondial will refund in full at purchase price.</em>
                            <br />Miss Mondial menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                            <br />di Indonesia yang terdaftar dengan harga yang lebih murah, Miss Mondial akan mengembalikan keseluruhan uang dari pembelian
                        </li>
                        
                        <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja</li>
                    </ol>
                </em>
            </div>
        </div>
        <%--div  style="float: left; margin-left: 150px;">Best Price Guarantee /<i> Garansi Harga Terbaik</i><div--%>
		<!-- Declaration hanya berlaku untuk DJ & LD, untuk PG & GJ tidak -->
		<div style="float:left;font-size:8px;font-family:arial; border:solid 1px #000;">
			<h2 style="margin-left:26px">DECLARATION<h2>
			<ol style="margin-top: 0px; margin-left:0px; padding-top: 0px;">
				<li><em>The diamonds herein invoiced have been purchased from legitimate sources not involved in funding conflict and in compliance with United Nations Resolutions.
						The seller hereby guarantees that these diamonds are conflict-free, based on the seller's knowledge and/or written guarantees provided by the supplier of these diamonds.</em>
				</li>
				<li><em>To the best of our knowledge and/or written assurance from our suppliers, we state that "Diamonds invoiced herein are natural and no properties of synthetics or CVD were noticed".
						The diamonds have been checked and verified by the customer at the time of delivery and we shall not be responsible for anything once goods leave our premises.</em>
				</li>
			</ol>
		</div>
    </body>
    </html>


</body>
</html>
