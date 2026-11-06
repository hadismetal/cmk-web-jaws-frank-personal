<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_Receipt.aspx.cs" Inherits="Page_Summary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Titipan</title>
    <link rel="stylesheet" href="/UI/CSS/style_print.css" />
</head>

<body>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Untitled Document</title>
<link href="style.css" rel="stylesheet" type="text/css" />
    <style>
        .table_info {
            border: none !important;
        }
        .table_info td {
            border: none !important;
        }
    </style>
</head>

<body>
<div id="judul1">
	<div class="judul_kiri">
		<h2 style="margin-bottom:0px;">TITIPAN CUSTOMER</h2>
        <b><span id="nomor" runat="server"></span> </b>
	</div>

    <div class="judul_kanan2">
        <h3 style="margin-bottom: 0px; text-align:right;">
            <asp:Literal id="cabang" runat="server"></asp:Literal></h3>
        <asp:Literal ID="alamat" runat="server"></asp:Literal>
    </div>
    <div style="clear:both;"></div>
</div>
<div>
    <div style="padding: 10px 0px;">
        <div style="border: solid 1px #000; padding: 10px;">
            <table cellspacing="0" cellpadding="5" border="0" class="table_info">
                <tr>
                    <td>Customer </td>
                    <td>&nbsp:&nbsp;</td>
                    <td><asp:Literal id="customer" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>JP / JR </td>
                    <td>&nbsp:&nbsp;</td>
                    <td><asp:Literal id="sales" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Keterangan </td>
                    <td>&nbsp:&nbsp;</td>
                    <td><asp:Literal id="keterangan" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td>Tanggal</td>
                    <td>&nbsp:&nbsp;</td>
                    <td><asp:Literal id="tgl" runat="server"></asp:Literal></td>
                </tr>
            </table>
            
        </div>
    </div>
	

</div>   
<div class="clear"></div>


<div id="menu1">
	<div class="dftr_menu" style="width: 200px;">PLU</div>
	<div class="dftr_menu" style="width: 700px">ITEM & Description / <i>Nama Barang & Keterangan</i></div>
	<div class="dftr_menu">Image</div>
	
</div>

<div class="clear"></div>
<div class="mnu_bwh"></div>
<div class="clear"></div>

    <asp:Literal ID="itemlist" runat="server" />
<br/><br/>

    <hr />
<div class="clear">&nbsp;</div>
    <br />
    <br />
    <br />
    <br />

<div style="float:right;">
	<div style="width:200px; text-align:center; padding:30px; float:right;">
		
		JP / JR<br/>
		<u><b><asp:Literal ID="sales2" runat="server"></asp:Literal></b></u><br/>
	</div>
    <%--<div style="width:200px; text-align:center; padding:30px; float:right;">
		<u><b>AE</b></u><br />
		<b>AE</b><br />
	</div>--%>
	<div style="width:200px; text-align:center; padding:30px; float:right;">
		
		Customer /<i> Pelanggan </i><br />
        <u><b><asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br/>
	</div>

</div>

<script type="text/javascript">
    window.print();
</script>
	

</body>
</html>


</body>
</html>
