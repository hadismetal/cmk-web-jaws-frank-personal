<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales_Invoice.aspx.cs" Inherits="Page_Summary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Invoice</title>
    <link rel="stylesheet" href="/UI/CSS/style_print.css" />
</head>

<body>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
<link href="style.css" rel="stylesheet" type="text/css" />
</head>

<body>
<div id="judul1">
	<div class="judul_kiri">
		<h2 style="margin-bottom:0px;">INVOICE</h2>
	</div>

	<div class="judul_kanan2">
		<h3 style="margin-bottom:0px;"><asp:Literal ID="cabang" runat="server"></asp:Literal></h3>
	</div>

</div>   
<div class="clear"></div>

<div class="sub_jdul">
	<b>
		<div class="judul_kiri">Invoice Date : <br/> <asp:Literal id="tgl" runat="server"></asp:Literal></div> 
		<div class="judul_kanan" style="width:200px;">From : <br/> <asp:Literal ID="customer" runat="server"></asp:Literal></div>
	</b>
</div>

<div class="clear"></div>

<div id="menu1">
	<div class="dftr_menu">Code NO / <i>CODE</i></div>
	<div class="dftr_menu">ITEM & Description / <i>Nama Barang & Keterangan</i></div>
	<div class="dftr_menu">Qty</div>
	<div class="dftr_menu">Price / <i>Harga</i></div>
	<div class="dftr_menu">Down Payment / <i>Uang Muka</i></div>
	<div class="dftr_menu">Payment / <i>Total Bayar</i></div>
</div>

<div class="clear"></div>
<div class="mnu_bwh"></div>
<div class="clear"></div>

<div style="padding-top:10px; width:1200px;">
	<div style="width:180px; float:left;">PLU0009978</div>
	<div style="width:370px; float:left;">CHAIN BERAT 3.3 KADAR 75% WARNA WHITE</div>
	<div style="width:60px; float:left;">1</div>
	<div style="width:120px; float:left;">1,652,000</div>
	<div style="width:220px; float:left;">&nbsp;</div>
	<div style="width:200px; float:left;">1,652,000</div>
</div>
<br/><br/><br/><br/><br/>

<div class="sub_jdul">
	<div class="judul_kiri">
		<b>Say / <i>Terbilang </i>:</b> <i><asp:Literal id="terbilang" runat="server"></asp:Literal></i>
	</div>

    <div class="judul_kanan" style="border:1px solid; padding: 5px;">
        <table class="table_invoice">
            <tr>
                <td>Total </td>
                <td class="nominal"><asp:Literal id="total" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Discount </td>
                <td class="nominal"><asp:Literal id="discount" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Discount Program  </td>
                <td class="nominal"><asp:Literal id="discountprogram" runat="server"></asp:Literal><br /></td>
            </tr>
            <tr>
                <td>Discount Gift Voucher</td>
                <td class="nominal"><asp:Literal id="discountgift" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Discount Nominal</td>
                <td class="nominal"><asp:Literal id="discountround" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td>Total Bayar </td>
                <td class="nominal"><asp:Literal id="nominal" runat="server"></asp:Literal></td>
            </tr>
        </table>
    </div>
	
</div>
<div class="clear">&nbsp;</div>
    <br />
    <br />
    <br />
    <br />

<div style="float:right;">
	<div style="width:200px; text-align:center; padding:30px; float:right;">
		<u><b><asp:Literal ID="adminsales" runat="server"></asp:Literal></b></u><br/>
		Jewellery Representative<br/>
		<i>Staff Penjualan</i>
	</div>

	<div style="width:200px; text-align:center; padding:30px; float:right;">
		<u><b><asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br/>
		Customer /<i> Pelanggan </i>
	</div>

</div>

	<div class="clear"></div>
	<br/><br/>
	
	<div>
	<b>After Sales Policy / <i>Kebijakan Penjualan</i></b>
	<table>
		<tr align="center">
			<td style="width:400px;">Merchandise Exchange<br/><i>(Penukaran Barang)</i></td>
			<td style="width:400px;">Trade In<br/><i>(Tukar Tambah)</i></td>
			<td style="width:400px;">Customer's option to resell<br/><i>(Opsi Penjualan Kembali)</i></td>
		</tr>
		
		<tr align="center">
			<td style="width:400px;">Period - <i>dalam waktu</i></td>
			<td style="width:400px;">Period - <i>dalam waktu</i></td>
			<td style="width:400px;">Period - <i>dalam waktu</i></td>
		</tr>
		
		<tr align="center">
			<td style="width:400px;">< 2 Weeks -<i> 2 Minggu</i></td>
			<td style="width:400px;">> 2 Weeks -<i> 2 Minggu</i></td>
			<td style="width:400px;">> 2 Weeks -<i> 2 Minggu</i></td>
		</tr>
		
		<tr align="center">
			<td style="width:400px;">100% <br/> Original sales amount <br/><i>Nilai Penualan Asal</i></td>
			<td style="width:400px;">Today Buying Price</td>
			<td style="width:400px;">Today Buying Price</td>
		</tr>
	</table>
	
	</div>
	
	<div class="clear"></div>
	<br/><br/><br/><br/>
	<div style="float:left; margin-left:">
		All Jewellery price are checked to be in good condition and accompanied by original certificate<br/>
		Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli
	</div>
	<div style="float:left; margin-left:150px;">Best Price Guarantee /<i> Garansi Harga Terbaik</i></div>

</body>
</html>


</body>
</html>
