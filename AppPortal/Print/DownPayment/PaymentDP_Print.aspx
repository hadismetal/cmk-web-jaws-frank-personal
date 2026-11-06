<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentDP_Print.aspx.cs"
Inherits="Page_Summary" %> <%@ Register Src="DetailPayment2Control.ascx"
TagName="detailPaymentControl" TagPrefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Invoice</title>
    <style>
      @charset "utf-8";
      body {
        width: 1200px;
        /*margin-left: 70px;*/
        font-size: 16px;
        margin: 0px auto 0px auto;
      }
      table {
        border-collapse: collapse;
      }
      table,
      th,
      td {
        border: 1px solid black;
        font-size: 16px;
      }
      #judul1 {
      }
      .judul_kiri {
        float: left;
      }
      .judul_kanan {
        float: right;
        width: 300px;
      }
      .judul_kanan2 {
        float: right;
        width: 300px;
      }

      .table_invoice {
        border: 0px solid #000;
        width: 100%;
      }

      .table_invoice td {
        padding: 5px;
        border: 0px solid #000;
        font-weight: bold;
      }

      .nominal {
        text-align: right;
      }

      .clear {
        clear: both;
      }
      .sub_jdul {
        border-top-width: 2px;
        border-top-style: solid;
        border-top-color: #000;
      }
      #menu1 {
        margin-top: 20px;
        border-top-width: 1px;
        border-top-style: solid;
        border-top-color: #000;
      }
      .dftr_menu {
        padding-top: 10px;
        padding-bottom: 10px;
        padding-left: 20px;
        padding-right: 20px;
        float: left;
      }
      .mnu_bwh {
        border-bottom-width: 1px;
        border-bottom-style: solid;
        border-bottom-color: #000;
      }

      .table_asp {
        width: 100%;
        text-align: center;
        height: 60px;
        font-size: 12px;
      }
      .table_asp td {
        border: none;
      }

      .period {
        border-bottom: solid 1px #000 !important;
      }

      .border_right {
        border-right: solid 1px #000 !important;
      }

      .font11 {
        font-size: 12px;
      }
      .table_dppo td {
        padding: 2px 10px;
      }

      body {
        font-weight: bold;
      }

      .table-no,
      .table-no td {
        border: 0px solid #000 !important;
      }

      .noborder,
      .noborder tr,
      .noborder td {
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
        <link href="style.css" rel="stylesheet" type="text/css" />
      </head>

      <body>
        <div id="judul1">
          <div class="judul_kiri">
            <h2 style="margin-bottom: 0px">Tanda Terima Titipan</h2>
          </div>

          <div class="judul_kanan">
            <h3 style="margin-bottom: 0px">
              <asp:Literal ID="cabang" runat="server"></asp:Literal>
            </h3>
          </div>
        </div>
        <div class="clear"></div>

        <div class="sub_jdul">
          <b>
            <div class="judul_kiri">
              Date :
              <br />
              <asp:Literal ID="tgl" runat="server"></asp:Literal>
            </div>
            <div class="judul_kanan" style="width: 450px">
              <table
                cellspacing="0"
                cellpadding="0"
                border="0"
                class="table-no"
              >
                <tr>
                  <td width="250"><em>To</em> / Kepada</td>
                  <td width="20">:</td>
                  <td width="180">
                    <asp:Literal ID="customer" runat="server"></asp:Literal>
                  </td>
                </tr>
                <tr>
                  <td width="150"><em>Customer Code</em> / Kode Pelanggan</td>
                  <td width="20">:</td>
                  <td width="180">
                    <asp:Literal
                      ID="kode_customer"
                      runat="server"
                    ></asp:Literal>
                  </td>
                </tr>
              </table>
            </div>
          </b>
        </div>

        <div class="clear"></div>

        <div id="menu1">
          <div class="dftr_menu">Code NO / <i>CODE</i></div>
          <div class="dftr_menu" style="width: 450px">
            ITEM & Description / <i>Nama Barang & Keterangan</i>
          </div>
          <div class="dftr_menu" style="width: 20px">Qty</div>
          <div class="dftr_menu" style="width: 200px">Price / <i>Harga</i></div>

          <div
            style="
              float: left;
              width: 180px;
              text-align: right;
              padding: 10px 0px;
            "
          >
            Total Bayar
          </div>
        </div>

        <div class="clear"></div>
        <div class="mnu_bwh"></div>
        <div class="clear"></div>

        <asp:Literal ID="itemlist" runat="server" />
        <br />
        <br />

        <div class="sub_jdul">
          <div class="judul_kiri" style="width: 70%">
            <b>Say / <i>Terbilang </i>:</b>
            <i> <asp:Literal ID="terbilang" runat="server"></asp:Literal></i>
            <br />
            <small runat="server" id="freegwptext">*Free GWP</small>
            <br />

            <div id="detail_payment_box" runat="server">
              <br />
              <br />
              <!-- adding firman 20180829 detail payment control
                       <asp:Literal ID="detail_payment" runat="server" /> -->
              <uc2:detailPaymentControl
                ID="detailPaymentControl"
                runat="server"
              />
              <br /><br />
              <br /><br />
              <br /><br />
              <br /><br />
              <asp:Literal ID="detail_invoice" runat="server" />
            </div>
          </div>

          <div class="judul_kanan" style="border: 1px solid; padding: 5px">
            <table class="table_invoice">
              <tr>
                <td>Total</td>
                <td class="nominal">
                  <asp:Literal ID="total" runat="server"></asp:Literal>
                </td>
              </tr>
              <tr>
                <td>Total Bayar</td>
                <td class="nominal">
                  <asp:Literal ID="nominal" runat="server"></asp:Literal>
                </td>
              </tr>
            </table>
          </div>
        </div>
        <div class="clear">&nbsp;</div>
        <div style="float: right">
          <div
            style="
              width: 200px;
              text-align: center;
              padding: 30px;
              float: right;
            "
          >
            <u
              ><b>
                <asp:Literal
                  ID="adminsales"
                  runat="server"
                ></asp:Literal></b></u
            ><br />
            Jewellery Representative<br />
            <i>Staff Penjualan</i>
          </div>

          <div
            style="
              width: 200px;
              text-align: center;
              padding: 30px;
              float: right;
            "
          >
            <u
              ><b>
                <asp:Literal ID="customer2" runat="server"></asp:Literal></b></u
            ><br />
            Customer /<i> Pelanggan </i>
          </div>
        </div>

        <div class="clear"></div>

        <div class="clear"></div>
        <br />
        <br />
        <br />
        <div style="float: left; font-size: 14px">
          <br />
          <table cellspacing="0" cellpadding="0" border="0" class="table_dppo">
            <tr>
              <td>
                Pesanan Khusus dan Barang Pesanan :
                <br />
                <ul>
                  <li>
                    Uang muka untuk barang pesanan tidak dapat dikembalikan.
                  </li>
                  <li>
                    Barang pesanan harus diambil dan dibayar lunas dalam waktu 1
                    bulan sejak <br />tanggal pemesanan, sesudahnya uang muka
                    dinyatakan tidak berlaku.
                  </li>
                  <li>
                    Tanda terima titipan harus dibawa pada saat pengambilan
                    barang.
                  </li>
                  <li>Harga jual tersebut hanya bersifat sementara.</li>
                  <li>Harga jual akan dihitung setelah barang selesai.</li>
                </ul>
              </td>
            </tr>
          </table>
        </div>
      </body>
    </html>
  </body>
</html>
