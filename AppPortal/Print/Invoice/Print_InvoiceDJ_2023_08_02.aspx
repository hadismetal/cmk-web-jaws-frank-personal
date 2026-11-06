<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_InvoiceDJ_2023_08_02.aspx.cs" Inherits="Page_Summary" %>

<%@ Register Src="DetailPaymentControl.ascx" TagName="detailPaymentControl" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Invoice</title>
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

        body{	
            width: 1200px;
            /*margin-left: 70px;*/
            font-size: 16px;
            margin: 0px auto 0px auto;
        }
        table
        {
        border-collapse:collapse;
        }
        table,th, td
        {
        border: 1px solid black;
        font-size: 16px;
        }
        #judul1 {
        }
        .judul_kiri {
            float:left;
        }
        .judul_kanan {
            float:right;
            width: 300px;
        }
        .judul_kanan2 {
        float: right;
        width: 300px;
        }

        .table_invoice
        {
            border: 0px solid #000;
            width: 100%;
        }

        .table_invoice td
        {
            padding: 5px;
            border: 0px solid #000;
            font-weight: bold;
        }

        .nominal
        {
            text-align: right;
        }

        .clear{
            clear:both;
        }
        .sub_jdul {
            border-top-width: 2px;
            border-top-style: solid;
            border-top-color: #000;
        }
        #menu1 {
            margin-top:20px;
            border-top-width: 1px;
            border-top-style: solid;
            border-top-color: #000;
        }
        .dftr_menu{
            padding-top:10px;
            padding-bottom:10px;
            padding-left:20px;
            padding-right:20px;
            float:left;
        }
        .mnu_bwh{
            border-bottom-width: 1px;
            border-bottom-style: solid;
            border-bottom-color: #000;
        }

        .table_asp
        {
            width: 100%;
            text-align: center;
            height: 60px;
            font-size: 12px;
        }
        .table_asp td
        {
            border: none;
        }

        .period
        {	
            border-bottom: solid 1px #000 !important;
        }

        .border_right
        {	
            border-right: solid 1px #000 !important;
        }

        .font11
        {
            font-size: 12px;
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
                <h2 style="margin-bottom: 0px;">INVOICE</h2>
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
                     <uc1:detailPaymentControl ID="detailPaymentControl" runat="server" />
                </div>

                   
            </div>

            <div class="judul_kanan" style="border: 1px solid; padding: 5px; width:29%;">
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
                        <td class="nominal">
                            <asp:Literal ID="dpp" runat="server"></asp:Literal></td>
                    </tr>
					<asp:Panel runat="server" ID="divuseppn">
                    <tr>
                        <td width="390" style="font-size: 14px;"><em>VAT</em> / PPN (<label runat="server" id="lblppn">10%</label> x <label runat="server" id="lblppn2">20%</label> x Harga Jual Tidak Termasuk PPN )</td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="ppn" runat="server"></asp:Literal></td>
                    </tr>
					</asp:Panel>
                    <tr>
                        <td style="border-top: solid 1px #000;"><em>Total</em></td>
                        <td style="border-top: solid 1px #000;" width="10"></td>
                        <td style="border-top: solid 1px #000;" class="nominal">
                            <asp:Literal ID="nominal" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td><em>Payment</em> / Pembayaran</td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="totaldp" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="border-top: solid 1px #000;"><em>Balance Due</em> / Sisa</td>
                        <td style="border-top: solid 1px #000;" width="10"></td>
                        <td style="border-top: solid 1px #000;" class="nominal">
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
        <br />
        <br />
        <!--<br />
        <br />
        <br />-->
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

        <div id="asp" runat="server">
            <b><em>After Sales Policy</em> / Kebijakan Penjualan</b>
            <table style="width:100%; text-align:center;" runat="server" id="asp_old">
		        <tr>
			        <td>
				        <em>Merchandise Exchange</em><br />
				        (Penukaran Barang)
			        </td>
                    <td colspan="3" style="width:45%;">
				        <em>Trade In</em><br />
				        (Tukar Tambah)
			        </td>
                    <td colspan="2" style="width:30%;">
				        <em>Customer's option to resell</em><br />
				        (Opsi Penjualan Kembali)
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <em>Period</em> - Dalam waktu
			        </td>
                    <td colspan="3" style="width:45%;">
				        <em>Period</em> - Dalam waktu
			        </td>
                    <td colspan="2" style="width:30%;">
				        <em>Period</em> - Dalam waktu
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="me" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="ti1" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="ti2" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="ti3" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="rs1" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="rs2" runat="server" />
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="me_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="ti_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="ti_rumus2" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="ti_rumus3" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="rs_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="rs_rumus2" runat="server" />
			        </td>
		        </tr>
		
	        </table>
		</div>
		<div id="aspdossier" runat="server" visible="false">
            <b><em>After Sales Policy</em> / Kebijakan Penjualan</b>
            <table style="width:100%; text-align:center;" runat="server" id="asp_doss">
		        <tr>
                    <td colspan="4" style="width:45%;">
				        <em>Trade In</em><br />
				        (Tukar Tambah)
			        </td>
                    <td colspan="2" style="width:30%;">
				        <em>Customer's option to resell</em><br />
				        (Opsi Penjualan Kembali)
			        </td>
		        </tr>
		        <tr>
                    <td colspan="4" style="width:45%;">
				        <em>Period</em> - Dalam waktu
			        </td>
                    <td colspan="2" style="width:30%;">
				        <em>Period</em> - Dalam waktu
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="tidoss0" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tidoss1" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tidoss2" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tidoss3" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="rsdoss1" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="rsdoss2" runat="server" />
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="tidoss_rumus0" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tidoss_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tidoss_rumus2" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tidoss_rumus3" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="rsdoss_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="rsdoss_rumus2" runat="server" />
			        </td>
		        </tr>
	        </table>
        </div>
        <div id="asp2" runat="server" visible="false">
                <b><i>After Sales Policy</i> / Kebijakan Penjualan</b>
            <table>
                <tr align="center">
                    <td style="width: 300px;">Merchandise Exchange<br />
                        <i>(Penukaran Barang)</i></td>
                    <td style="width: 500px;">Trade In<br />
                        <i>(Tukar Tambah)</i></td>
                    <td style="width: 400px;">Customer's option to resell<br />
                        <i>(Opsi Penjualan Kembali)</i></td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center; width: 100%; padding: 20px;"><i>"No Exchange - No Resell"</i>. <em>Tidak dapat ditukar dan Tidak dapat dijual kembali.</em> </td>
                </tr>
            </table>
        </div>
        <div id="asp3" runat="server" visible="false">
                <b><i>After Sales Policy</i> / Kebijakan Penjualan</b>
            <table>
                <tr align="center">
                    <td style="width: 300px;">Merchandise Exchange<br />
                        <i>(Penukaran Barang)</i></td>
                    <td style="width: 500px;">Trade In<br />
                        <i>(Tukar Tambah)</i></td>
                    <td style="width: 400px;">Customer's option to resell<br />
                        <i>(Opsi Penjualan Kembali)</i></td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: center; width: 100%; padding: 20px;"><i>"No Exchange - No Resell"</i>. <em>Tidak dapat ditukar dan Tidak dapat dijual kembali.</em> </td>
                </tr>
            </table>
        </div>
		<div id="aspbts" runat="server" visible="false">
            <b><em>After Sales Policy</em> / Kebijakan Penjualan</b>
            <table style="width:100%; text-align:center;">
		        <tr>
			        <td>
				        <em>Merchandise Exchange</em><br />
				        (Penukaran Barang)
			        </td>
                    <td colspan="3" style="width:45%;">
				        <em>Trade In</em><br />
				        (Tukar Tambah)
			        </td>
                    <td colspan="2" style="width:30%;">
				        <em>Customer's option to resell</em><br />
				        (Opsi Penjualan Kembali)
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <em>Period</em> - Dalam waktu
			        </td>
                    <td colspan="3" style="width:45%;">
				        <em>Period</em> - Dalam waktu
			        </td>
                    <td colspan="2" rowspan="3" style="width:30%;">
				        <i>Only gold value calculated based on <asp:Literal runat="server" id="Literalbts1" /> today's buy back gold price</i><br />
                        Hanya nilai emas yang diperhitungkan berdasarkan harga beli <asp:Literal runat="server" id="Literalbts2" /> pada hari pengembalian
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="mebts" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tibts1" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tibts2" runat="server" />
			        </td>
                    <td style="width:15%;">
				        <asp:Literal ID="tibts3" runat="server" />
			        </td>
		        </tr>
		        <tr>
			        <td>
				        <asp:Literal ID="mebts_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tibts_rumus1" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tibts_rumus2" runat="server" />
			        </td>
                    <td style="width:15%;">
                        <asp:Literal ID="tibts_rumus3" runat="server" />
			        </td>
		        </tr>
	        </table>
        </div>
        <div id="general" runat="server" visible="false">
                <b><i>After Sales Policy</i> / Kebijakan Penjualan</b>
            <table>
                <tr>
                    <td style="text-align: center; width: 1200px; padding: 50px 20px;"><i>Only gold value calculated based on <asp:Literal runat="server" id="Literal1" /> today's buy back gold price</i><br />
                        Hanya nilai emas yang diperhitungkan berdasarkan harga beli <asp:Literal runat="server" id="Literal2" /> pada hari pengembalian
                    </td>
                </tr>
            </table>
            </div>
        <div class="clear"></div>
        <div style="float: left; font-size: 14px;">
            <br />
            <asp:Literal runat="server" id="lit_test_msg" /><br />
            Catatan / <em>Notes</em> :
            <ol style="margin-top: 0px; padding-top: 0px;">
                <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                <asp:Panel runat="server" id="lit_ppn_notice"><li><em>Price including VAT <asp:Literal runat="server" id="lit_ppn" /></em> / Harga Jual Termasuk PPN <asp:Literal runat="server" id="lit_ppn2" /></li></asp:Panel>
                <li><label runat="server" id="lbl_ppn_notice"><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</label></li>
                <li><em><asp:Literal runat="server" id="lit_brand1" /> guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                    <br /><em>registered jeweler at lower price,  <asp:Literal runat="server" id="lit_brand2" /> will refund in full at purchase price.</em>
                    <br /><asp:Literal runat="server" id="lit_brand3" /> menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                    <br />di Indonesia yang terdaftar dengan harga yang lebih murah, <asp:Literal runat="server" id="lit_brand4" /> akan mengembalikan keseluruhan uang dari pembelian
                </li>
                        
                <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja</li>
				<li runat="server" id="onlinePolicy" visible="false"><em>After Sales Policy only apply for walk-in store</em> / After Sales Policy hanya berlaku di toko fisik</li>
            </ol>

        </div>
		<!-- Declaration hanya berlaku untuk DJ & LD, untuk PG & GJ tidak -->
		<div style="float:left;font-size:8px;font-family:arial; border:solid 1px #000;">
			<h2 style="margin-left:26px">DECLARATION<h2>
			<ol style="margin-top: 0px; margin-left:0px; padding-top: 0px;">
				<li><em>The diamonds herein invoiced have been purchased from legitimate sources not involved in funding conflict and in compliance with United Nations Resolutions.
						The seller hereby guarantees that these diamonds are conflict-free, based on the seller's knowledge and/or written guarantees provided by the supplier of these diamonds.</em>
				</li>
				<li><em>To the best of our knowledge and/or written assurance from our suppliers, we state that "Diamonds invoiced herein are natural and no properties of synthetics or CVD were noticed".
						</em>
				</li>
			</ol>
		</div>

    </body>
    </html>


</body>
</html>
