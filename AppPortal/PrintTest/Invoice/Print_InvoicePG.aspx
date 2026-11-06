<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_InvoicePG.aspx.cs" Inherits="Page_Summary" %>


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
                <b><em>Say</em> / Terbilang :</b> 
                <i><asp:Literal ID="terbilang" runat="server"></asp:Literal></i>
                    <br />
                    <br />

                <div id="detail_payment_box" runat="server">
                    <div>
                        <div style="float:left;">
                              <!-- adding firman 20180829 detail payment control
                            <asp:Literal ID="detail_payment" runat="server" /> -->

                            <uc1:detailPaymentControl ID="detailPaymentControl" runat="server" />
                        </div>
                        <div style="float:right;">
                            
                            <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" style="max-height:200px; max-width:200px; border:solid 1px black; margin-right:25px;" />
                        </div>
                        <div style="clear:both"></div>
                    </div>
                </div>
            </div>

            <div class="judul_kanan" style="border: 1px solid; padding: 5px; width:29%;">
                <table class="table_invoice">
					<tr>
                        <td width="390"><label runat="server" id="lblppn10"><em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN </label></td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="dpp" runat="server"></asp:Literal></td>
                    </tr>
					<asp:Panel runat="server" ID="divuseppn">
                    <tr>
                        <td width="390" style="font-size: 14px;"><em>VAT</em> / PPN <div style="display:none">(<label runat="server" id="lblppn">10%</label> x <label runat="server" id="lblppn2">20%</label> x Harga Jual Tidak Termasuk PPN )</div></td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="ppn" runat="server"></asp:Literal></td>
                    </tr>
					</asp:Panel>
					<asp:Panel runat="server" ID="pphpanel" Visible="false">
					<tr>
                        <td width="390" style="font-size: 14px;"><em>VAT</em> / PPH </td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="pph" runat="server"></asp:Literal></td>
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
            <table>
                <tr align="center">
                    <td style="width: 300px;"><em>Merchandise Exchange</em><br />
                        (Penukaran Barang)</td>
                    <td style="width: 500px;"><em>Trade In</em><br />
                        (Tukar Tambah)</td>
                    <td style="width: 400px;"><em>Customer's option to resell</em><br />
                        (Opsi Penjualan Kembali)</td>
                </tr>

                <tr align="center">
                    <td style="width: 300px;" class="font11"><
                        <asp:Literal ID="me1" runat="server"></asp:Literal>
                        month -
                        <asp:Literal ID="me2" runat="server"></asp:Literal>
                        month
                    </td>
                    <td style="width: 500px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td colspan="3" class="period">Period</td>
                            </tr>
                            <tr>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="t1" runat="server"></asp:Literal>
                                    year
                                </td>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="t2" runat="server"></asp:Literal>
                                    years
                                </td>
                                <td width="33%">
                                    <asp:Literal ID="t3" runat="server"></asp:Literal>
                                    years
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 400px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td colspan="2" class="period">Period</td>
                            </tr>
                            <tr>
                                <td width="50%" class="border_right">
                                    <asp:Literal ID="r1" runat="server"></asp:Literal>
                                    year
                                </td>
                                <td width="50%">
                                    <asp:Literal ID="r2" runat="server"></asp:Literal>
                                    years
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr align="center">
                    <td style="width: 300px;" class="font11">
                        <asp:Literal ID="me_rumus1" runat="server"></asp:Literal>
                        %
                        <br />
                        <em>Original sales amount</em>
                        <br />
                        Nilai Penjualan Asal</td>
                    <td style="width: 500px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="ti_rumus1" runat="server"></asp:Literal>
                                    %
                                    <br />
                                    <em>Original sales amount</em>
                                    <br />
                                    Nilai Penjualan Asal

                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="33%" class="border_right">
                        <asp:Literal ID="ti_rumus2" runat="server"></asp:Literal>
                        %
                                    <br />
                        <em>Original sales amount</em>
                        <br />
                        Nilai Penjualan Asal
                    </td>
                    <td width="33%">
                        <asp:Literal ID="brand" runat="server"></asp:Literal>
                        <em><u>buying price</u></em><br />
                        Harga beli
                        <asp:Literal ID="brand2" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <%--</td>
                    <td style="width: 400px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td width="50%" class="border_right">
                                    <asp:Literal ID="r1_rumus" runat="server"></asp:Literal>
                     s               %
                                    <br />
                                    <em>Original sales amount</em>
                                    <br />
                                    Nilai Penjualan Asal</td>
                    </td>
                            <td width="50%">
                                <asp:Literal ID="brand3" runat="server"></asp:Literal>
                                <em><u>buying price</u></em><br />
                                Harga beli
                                <asp:Literal ID="brand4" runat="server"></asp:Literal>
                            </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>--%>

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
        <div class="clear"></div>
        <div style="float: left; font-size: 14px;">
            <div id="general" runat="server" visible="false">
                <b><i>After Sales Policy</i> / Kebijakan Penjualan</b>
                <table>
					<tr>
                        <td style="width:40%;text-align: center"><em>Merchandise Exchange - PG to PG</em><br />(Penukaran Barang Plain Gold ke Plain Gold)</td>
                        <td style="width:60%;text-align: center"><em>Customer's Option to Resell</em><br />(Opsi Penjualan Kembali)</td>
                    </tr>
                    <tr>
                        <td style="text-align: center;"><em>Period</em> - Dalam Waktu</td>
                        <td style="text-align: center;"><em>Period</em> - Dalam Waktu</td>
                    </tr>
                    <tr>
                        <td style="text-align: center;"><em><= 1 Weeks</em> - 1 Minggu</td>
                        <td style="text-align: center;"><em>At anytime</em> - Kapan pun</td>
                    </tr>
                    <tr>
                        <td style="text-align: center;"><em>100% Original Sales Amount</em> - Nilai Penjualan Asal</td>
						<td style="text-align: center; width: 1200px; padding: 50px 20px;"><i>Only gold value calculated based on <asp:Literal runat="server" id="lit_brand1" /> today's buy back gold price</i><br />
                            Hanya nilai emas yang diperhitungkan berdasarkan harga beli <asp:Literal runat="server" id="lit_brand2" /> pada hari pengembalian
                        </td>
                    </tr>
                </table>
                <br />
                Catatan / <em>Notes</em> :
                <ol style="margin-top: 0px; padding-top: 0px;">
                    <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                    <asp:Panel runat="server" id="lit_ppn_notice"><li><em>Price including VAT <div style="display:none"><asp:Literal runat="server" id="lit_ppn" /></div></em> / Harga Jual Termasuk PPN <div style="display:none"><asp:Literal runat="server" id="lit_ppn2" /></div></li></asp:Panel>
                    <!--<li><em><asp:Literal runat="server" id="lit_brand3" /> guarantees that within 7 (seven) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                        <br />
                        <em>registered jeweler at lower price,  <asp:Literal runat="server" id="lit_brand4" /> will refund in full at purchase price.</em>
                        <br />
                        <asp:Literal runat="server" id="lit_brand5" /> menjamin bahwa dalam waktu kurang dari 7 (tujuh) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                        <br />
                        di Indonesia yang terdaftar dengan harga yang lebih murah, <asp:Literal runat="server" id="lit_brand6" /> akan mengembalikan keseluruhan uang dari pembelian
                    </li>-->

                    <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja</li>
                </ol>
                
            </div>
            <div id="thepalace" runat="server" visible="false">
                <b><i>After Sales Policy</i> / Kebijakan Penjualan</b>
                <table class="text-center" style="width:100%; padding:0 25px 0 25px;">
                    <tr>
                        <td style="width:25%;"><em>Merchandise Exchange - PG to PG</em><br />(Penukaran Barang Plain Gold ke Plain Gold)</td>
                        <td style="width:25%;"><em>TradeIn - PG to PG</em><br />(Tukar Tambah Plain Gold ke Plain Gold)</td>
                        <td style="width:25%;"><em>TradeIn - PG to DJ *</em><br />(Tukar Tambah Plain Gold ke Diamond Jewellery)</td>
                        <td style="width:25%;"><em>Customer's Option to Resell</em><br />(Opsi Penjualan Kembali)</td>
                    </tr>
                    <tr>
                        <td><em>Period</em> - Dalam Waktu</td>
                        <td><em>Period</em> - Dalam Waktu</td>
                        <td><em>Period</em> - Dalam Waktu</td>
                        <td><em>Period</em> - Dalam Waktu</td>
                    </tr>
                    <tr>
                        <td><em><= 1 Weeks</em> - 1 Minggu</td>
                        <td><em>> 1 Weeks</em> - 1 Minggu</td>
                        <td><em>At anytime</em> - Kapan pun</td>
                        <td><em>At anytime</em> - Kapan pun</td>
                    </tr>
                    <tr>
                        <td><em>100% Original Sales Amount</em> - Nilai Penjualan Asal</td>
                        <td><em>Today Buying Price <asp:Literal runat="server" id="lit_brand7" /></em></td>
                        <td><em>Today Selling Price <asp:Literal runat="server" id="lit_brand8" /></em></td>
                        <td><em>Today Buying Price <asp:Literal runat="server" id="lit_brand9" /></em></td>
                    </tr>
                </table>
                <br />
                <b>*) <i>Trade In</i> dari Plain Gold ke Diamond Jewellery</b>
                <br />
                <i>Harga Plain Gold akan diperhitungkan sesuai dengan harga <i>Today Selling Price</i> <asp:Literal runat="server" id="lit_brand10" /> apabila Customer hendak menukarkannya dengan Diamond Jewellery</i>
                <br />
                <ol style="margin-top: 0px; padding-top: 0px;">
                    <li><em>All Jewellery pieces are checked to be in good condition and accompanied by original certificate</em> / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli</li>
                    <asp:Panel runat="server" id="lit_ppn_notice2"><li><em>Price including VAT <div style="display:none"><asp:Literal runat="server" id="lit_ppn3" /></div></em> / Harga Jual Termasuk PPN <div style="display:none"><asp:Literal runat="server" id="lit_ppn4" /></div></li></asp:Panel>
                    <!--<li><em><asp:Literal runat="server" id="lit_brand11" /> guarantees that within 7 (seven) days of purchase from us, should a customer purchase an identical piece of diamond jewellery item from another Indonesia based </em>
                        <br />
                        <em>registered jeweler at lower price, <asp:Literal runat="server" id="lit_brand12" /> will refund in full at purchase price.</em>
                        <br />
                        <asp:Literal runat="server" id="lit_brand13" /> menjamin bahwa dalam waktu kurang dari 7 (tujuh) hari dari pembelian, jika pelanggan membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain 
                        <br />
                        di Indonesia yang terdaftar dengan harga yang lebih murah, <asp:Literal runat="server" id="lit_brand14" /> akan mengembalikan keseluruhan uang dari pembelian
                    </li>-->

                    <li><em>Resell payment will be done by transferring to customer's bank account minimum in 2 (two) working days</em> / Pembayaran Resell akan dilakukan melalui transfer bank ke rekening pelanggan minimal 2 (dua) hari kerja</li>
                </ol>
                
            </div>
        </div>
		<!-- Declaration hanya berlaku untuk DJ & LD, untuk PG & GJ tidak -->
		<!--<div style="float:left;font-size:8px;font-family:arial; border:solid 1px #000;">
			<h2 style="margin-left:26px">DECLARATION<h2>
			<ol style="margin-top: 0px; margin-left:0px; padding-top: 0px;">
				<li><em>The diamonds herein invoiced have been purchased from legitimate sources not involved in funding conflict and in compliance with United Nations Resolutions.
						The seller hereby guarantees that these diamonds are conflict-free, based on the seller's knowledge and/or written guarantees provided by the supplier of these diamonds.</em>
				</li>
				<li><em>To the best of our knowledge and/or written assurance from our suppliers, we state that "Diamonds invoiced herein are natural and no properties of synthetics or CVD were noticed".
						The diamonds have been checked and verified by the customer at the time of delivery and we shall not be responsible for anything once goods leave our premises.</em>
				</li>
			</ol>
		</div>-->
    </body>
    </html>


</body>
</html>
