<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_InvoiceDJ.aspx.cs" Inherits="Page_Summary" %>

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
        <style>
            
        </style>
    </head>

    <body>
        <div id="judul1">
            <div class="judul_kiri">
                <h2 style="margin-bottom: 0px;">INVOICE</h2>
            </div>

            <div class="judul_kanan">
                <h3 style="margin-bottom: 0px;">
                    <asp:Literal ID="cabang" runat="server"></asp:Literal></h3>
            </div>

        </div>
        <div class="clear"></div>

        <div class="sub_jdul">
            <b>
                <div class="judul_kiri">
                    Invoice Date :
                    <br />
                    <asp:Literal ID="tgl" runat="server"></asp:Literal>
                </div>
                <div class="judul_kanan" style="width: 200px;">
                    From :
                    <br />
                    <asp:Literal ID="customer" runat="server"></asp:Literal>
                </div>
            </b>
        </div>

        <div class="clear"></div>

        <div id="menu1">
            <div class="dftr_menu">Code NO / <i>CODE</i></div>
            <div class="dftr_menu">ITEM & Description / <i>Nama Barang & Keterangan</i></div>
            <div class="dftr_menu">Qty</div>
            <div class="dftr_menu" style="width: 150px">Price / <i>Harga</i></div>
            <div class="dftr_menu" style="width: 150px">Discount</div>
            <div style="float: left; width: 225px; text-align: right; padding: 10px 0px;">Total Bayar</div>
        </div>

        <div class="clear"></div>
        <div class="mnu_bwh"></div>
        <div class="clear"></div>

        <asp:Literal ID="itemlist" runat="server" />
        <br />
        <br />

        <div class="sub_jdul">
            <div class="judul_kiri">
                <b>Say / <i>Terbilang </i>:</b> <i>
                    <asp:Literal ID="terbilang" runat="server"></asp:Literal></i>
            </div>


        </div>
        <div class="clear">&nbsp;</div>
        <br />
        <br />
        <br />
        <br />

        <div style="float: right;">
            <div style="width: 200px; text-align: center; padding: 30px; float: right;">
                <u><b>
                    <asp:Literal ID="adminsales" runat="server"></asp:Literal></b></u><br />
                Jewellery Representative<br />
                <i>Staff Penjualan</i>
            </div>

            <div style="width: 200px; text-align: center; padding: 30px; float: right;">
                <u><b>
                    <asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br />
                Customer /<i> Pelanggan </i>
            </div>

        </div>

        <div class="clear"></div>
        <br />
        <br />

        <div id="asp" runat="server">
            <b>After Sales Policy / <i>Kebijakan Penjualan</i></b>
            <table>
                <tr align="center">
                    <td style="width: 400px;">Merchandise Exchange<br />
                        <i>(Penukaran Barang)</i></td>
                    <td style="width: 400px;">Trade In<br />
                        <i>(Tukar Tambah)</i></td>
                    <td style="width: 400px;">Customer's option to resell<br />
                        <i>(Opsi Penjualan Kembali)</i></td>
                </tr>

                <tr align="center">
                    <td style="width: 400px;" class="font11"><=
                        <asp:Literal ID="me1" runat="server"></asp:Literal>
                        month -
                        <asp:Literal ID="me2" runat="server"></asp:Literal>
                        month
                    </td>
                    <td style="width: 400px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td colspan="3" class="period">Period</td>
                            </tr>
                            <tr>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="ti1" runat="server"></asp:Literal> year
                                </td>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="ti2" runat="server"></asp:Literal> years
                                </td>
                                <td width="33%" >
                                    > <asp:Literal ID="ti3" runat="server"></asp:Literal> years
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
                                    <asp:Literal ID="r1" runat="server"></asp:Literal> year
                                </td>
                                <td width="50%">
                                    > <asp:Literal ID="r2" runat="server"></asp:Literal> years
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>

                <tr align="center">
                    <td style="width: 400px;" class="font11">
                        <asp:Literal ID="me_rumus1" runat="server"></asp:Literal> %
                        <br />
                        Original sales amount
                        <br />
                        <i>Nilai Penualan Asal</i></td>
                    <td style="width: 400px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="ti_rumus1" runat="server"></asp:Literal> %
                                    <br />
                                    Original sales amount<br />
                                    <i>Nilai Penjualan Asal</i>
                                </td>
                                <td width="33%" class="border_right">
                                    <asp:Literal ID="ti_rumus2" runat="server"></asp:Literal> %
                                    <br />
                                    Original sales amount<br />
                                    <i>Nilai Penjualan Asal</i>
                                </td>
                                <td width="33%" >
                                    
                                    The Palace buying price<br />
                                    <i>Harga beli The Palace</i>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 400px;">
                        <table class="table_asp" style="border: none !important; padding: 10px;">
                            <tr>
                                <td width="50%" class="border_right">
                                    <asp:Literal ID="r1_rumus" runat="server"></asp:Literal> %
                                    <br />
                                    Original sales amount<br />
                                    <i>Nilai Penjualan Asal</i>
                                </td>
                                <td width="50%">
                                    The Palace buying price<br />
                                    <i>Harga beli The Palace</i>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </div>

        <div class="clear"></div>
        <br />
        <br />
        <br />
        <br />
		<br />
		<br />
		<br />
		<br />
		<br />
		<br />
		<br />
        <div style="float: left; margin-left: ">
            All Jewellery price are checked to be in good condition and accompanied by original certificate<br />
            Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli
        </div>
        <div style="float: left; margin-left: 150px;">Best Price Guarantee /<i> Garansi Harga Terbaik</i></div>

    </body>
    </html>


</body>
</html>
