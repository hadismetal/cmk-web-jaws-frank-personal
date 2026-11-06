<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales_Packaging.aspx.cs" Inherits="Page_Summary" %>

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
        <link href="style.css" rel="stylesheet" type="text/css" />
        <style>
            
        </style>
    </head>

    <body>
        <div id="judul1">
            <div class="judul_kiri">
                <h2 style="margin-bottom: 0px;">TANDA TERIMA PACKAGING</h2>
                PT. CENTRAL MEGA KENCANA<br />
                NPWP : 01.764.536.7-073.000
            </div>

            <div class="judul_kanan2">
                <h3 style="margin-bottom: 0px;">
                    <asp:Literal ID="cabang" runat="server"></asp:Literal></h3>
                <asp:Literal ID="alamat" runat="server"></asp:Literal>
            </div>

        </div>
        <div class="clear"></div>

        <div class="sub_jdul">
            <b>
                <div class="judul_kiri">
                    <table cellspacing="0" cellpadding="0" border="0" class="table-no">
                        <tr>
                            <td width="150">No. </td>
                            <td width="20">:</td>
                            <td>
                                <asp:Literal ID="noinvoice" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td width="160"><em>Date</em> / Tanggal Faktur</td>
                            <td width="20">:</td>
                            <td>
                                <asp:Literal ID="tgl" runat="server"></asp:Literal></td>
                        </tr>
                    </table>
                </div>
                <div class="judul_kanan" style="width: 550px;">
                    <table cellspacing="0" cellpadding="0" border="0" class="table-no">
                        <tr>
                            <td width="250"><em>To</em> / Kepada</td>
                            <td width="20">:</td>
                            <td width="180">
                                <asp:Literal ID="customer" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td width="150"><em>Customer Code</em> / Kode Pelanggan</td>
                            <td width="20">:</td>
                            <td width="180">
                                <asp:Literal ID="nomember" runat="server"></asp:Literal>

                            </td>
                        </tr>
                    </table>

                </div>
            </b>
        </div>

        <div class="clear"></div>

        <div id="menu1">
            <div class="dftr_menu"><em>Code NO</em> / CODE</div>
            <div class="dftr_menu"><em>ITEM & Description</em> / Nama Barang & Keterangan</div>
            <div class="dftr_menu">Qty</div>
            <div class="dftr_menu" style="width: 100px">Price / <i>Harga</i></div>
            <div class="dftr_menu" style="width: 100px">Disc</div>
            <div style="float: left; width: 200px; text-align: right; padding: 10px 0px;"><em>Price</em> / Harga Jual (Rp)</div>
        </div>

        <div class="clear"></div>
        <div class="mnu_bwh"></div>
        <div class="clear"></div>

        <asp:Literal ID="itemlist" runat="server" />
        <br />
        <br />

        <div class="sub_jdul">
            <div class="judul_kiri">
                <b><em>Say</em> / Terbilang :</b> <i>
                    <asp:Literal ID="terbilang" runat="server"></asp:Literal></i>

                <div id="detail_payment_box" runat="server">
                    <br />
                    <br />
                    <asp:Literal ID="detail_payment" runat="server" />
                    <br /><br />
                    <asp:Literal ID="detail_invoice" runat="server" />
                </div>
            </div>

            <div class="judul_kanan" style="border: 1px solid; padding: 5px;">
                <table class="table_invoice">
                    <tr>
                        <td width="390"><em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN </td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="dpp" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td width="390" style="font-size: 14px;"><em>VAT</em> / PPN (10% x 20% x Harga Jual Tidak Termasuk PPN )</td>
                        <td width="10"></td>
                        <td class="nominal">
                            <asp:Literal ID="ppn" runat="server"></asp:Literal></td>
                    </tr>
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
                        <li><em>Price including VAT 2 %</em> / Harga Jual Termasuk PPN 2 %</li>
                        <li><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</li>
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
                        <li><em>Price including VAT 2 %</em> / Harga Jual Termasuk PPN 2 %</li>
                        <li><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</li>
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
                        <li><em>Price including VAT 2 %</em> / Harga Jual Termasuk PPN 2 %</li>
                        <li><em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN</li>
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
    </body>
    </html>


</body>
</html>
