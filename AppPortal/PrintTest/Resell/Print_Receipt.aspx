<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_Receipt.aspx.cs" Inherits="Page_Summary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Resell</title>
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
		<h2 style="margin-bottom:0px;">BUKTI PEMBELIAN</h2>
	</div>

	<div class="judul_kanan">
		<h3 style="margin-bottom:0px;"><asp:Literal ID="cabang" runat="server"></asp:Literal></h3>
	</div>

</div>   
<div class="clear"></div>

<div class="sub_jdul">
	<b>
		<div class="judul_kiri">Date : <br/> <asp:Literal id="tgl" runat="server"></asp:Literal></div> 
		<div class="judul_kanan" style="width:200px;">From : <br/> <asp:Literal ID="customer" runat="server"></asp:Literal></div>
	</b>
</div>

<div class="clear"></div>

<div id="menu1">
	<div class="dftr_menu" style="width:180px;">Code NO / <i>CODE</i></div>
	<div class="dftr_menu">ITEM & Description / <i>Nama Barang & Keterangan</i></div>
	<div class="dftr_menu">Qty</div>
	<div class="dftr_menu" style="width: 150px">Price / <i>Harga</i></div>
    <div style="float: left; width: 225px; text-align: right; padding: 10px 0px;">Total Bayar</div>
</div>

<div class="clear"></div>
<div class="mnu_bwh"></div>
<div class="clear"></div>

    <asp:Literal ID="itemlist" runat="server" />
<br/><br/>

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
		Store Manager
	</div>

</div>

	<div class="clear"></div>
	<br/><br/>


</body>
</html>


</body>
</html>
