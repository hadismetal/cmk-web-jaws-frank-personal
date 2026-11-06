<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepairInvoiceMaterial.aspx.cs"
Inherits="DOKRM_index" %> <%@ Register Src="~/UC/DetailPaymentControlR.ascx"
TagName="detailPaymentControlR" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Invoice Repair Material</title>
    <link rel="stylesheet" href="/UI/CSS/style_print.css" />
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <style>
      @media print {
        @page {
          size: A4 portrait;
          margin: 1cm;
        }        

        body {
          transform: none;
          width: auto;
          font-weight: bold;
        }
        
        /* .no-print {
          display: none !important;
          
        } */

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
    <div id="judul1">
      <div class="judul_kiri">
        <h2 style="margin-bottom: 0px">INVOICE</h2>
        PT. CENTRAL MEGA KENCANA<br />
        <label id="lblnpwp">NPWP : 01.764.536.7-073.000</label>
      </div>

      <div class="judul_kanan2" id="AddrAlamat"></div>
    </div>
    <div class="clear"></div>

    <div class="sub_jdul">
      <div class="judul_kiri">
        <table cellspacing="0" cellpadding="0" border="0" class="table-no">
          <tr>
            <td style="vertical-align: top; width: 160px">No.</td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="NomorInvoice"></td>
          </tr>
          <tr>
            <td style="vertical-align: top; width: 160px">
              <em>Date</em> / Tanggal Faktur
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="TglNow"></td>
          </tr>
        </table>
      </div>
      <div class="judul_kanan" style="width: 550px">
        <table cellspacing="0" cellpadding="0" border="0" class="table-no">
          <tr>
            <td style="vertical-align: top; width: 250px">
              <em>Membership Code</em> / Kode Member
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="NoCustomer"></td>
          </tr>
          <tr>
            <td style="vertical-align: top; width: 250px">
              <em>Membership Name</em> / Nama Member
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="CustomerNama"></td>
          </tr>
          <tr>
            <td style="vertical-align: top; width: 250px">
              <em>To</em> / Kepada
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="CustomerNama"></td>
          </tr>
        </table>
      </div>
    </div>

    <div class="clear"></div>

    <div id="menu1"></div>

    <table
      cellspacing="0"
      cellpadding="5"
      border="0"
      class="noborder"
      style="width: 1050px"
    >
      <tr>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: left;
            width: 180px;
          "
        >
          <em>Code NO</em> / CODE
        </th>
        <th style="border: none; vertical-align: top; text-align: left">
          <em>ITEM & Description</em> / Nama Barang & Keterangan
        </th>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: center;
            width: 60px;
          "
        >
          Qty
        </th>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: right;
            width: 120px;
          "
        ></th>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: right;
            width: 120px;
          "
        ></th>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: right;
            width: 180px;
          "
        >
          <em>Price</em> / Harga Jual (Rp)
        </th>
      </tr>
    </table>

    <div class="clear"></div>
    <div class="mnu_bwh"></div>
    <div class="clear"></div>

    <table
      cellspacing="0"
      cellpadding="5"
      border="0"
      class="noborder"
      style="width: 1050px"
      id="tbody-data-invoice"
    >      
    </table>
    <br />

    <div class="sub_jdul">
      <div class="judul_kiri" style="width: 70%">
        <b><em>Say</em> / Terbilang :</b>
        <i id="terbilang"></i>
        <br />
        <br />

        <div id="detail_payment_box" runat="server">
          <div>
            <div style="float: left">
              <!-- adding firman 20180829 detail payment control
                        <asp:Literal ID="detail_payment" runat="server" /> -->

              <uc1:detailPaymentControlR
                ID="detailPaymentControlR"
                runat="server"
              />
            </div>
            <div style="float: right">
              <asp:Image
                ID="imgpicture"
                runat="server"
                CssClass="img-thumbnail"
                style="
                  max-height: 200px;
                  max-width: 200px;
                  border: solid 1px black;
                  margin-right: 25px;
                "
              />
            </div>
            <div style="clear: both"></div>
          </div>
        </div>
      </div>

      <div
        class="judul_kanan"
        style="border: 1px solid; padding: 5px; width: 29%"
      >
        <table class="table_invoice">
          <tr>
            <td width="390">
              <label id="lblppn10"
                ><em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN
              </label>
            </td>
            <td width="10"></td>
            <td class="BeforeTax nominal"></td>
          </tr>
          <div id="divuseppn">
            <tr id="divuseppn1">
              <td width="390" style="font-size: 14px" >
                <em>VAT</em> / PPN
                <div style="display: none">
                  (<label id="lblppn">11%</label> x
                  <label id="lblppn2">20%</label> x Harga Jual Tidak Termasuk
                  PPN )
                </div>
              </td>
              <td width="10"></td>
              <td class="nominal Tax"></td>
            </tr>
          </div>

          <tr>
            <td style="border-top: solid 1px #000"><em>Total</em></td>
            <td style="border-top: solid 1px #000" width="10"></td>
            <td
              style="border-top: solid 1px #000"
              class="nominal AfterTax"
            ></td>
          </tr>
          <tr>
            <td><em>Payment</em> / Pembayaran</td>
            <td width="10"></td>
            <td class="nominal AfterTax"></td>
          </tr>
          <tr>
            <td style="border-top: solid 1px #000">
              <em>Balance Due</em> / Sisa
            </td>
            <td style="border-top: solid 1px #000" width="10"></td>
            <td style="border-top: solid 1px #000" class="nominal">0</td>
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
    <div style="float: right">
      <div
        style="width: 200px; text-align: center; padding: 30px; float: right"
      >
        <u><b class="SalesNama"> </b></u><br />
        <em>Jewellery Representative</em><br />
        Staff Penjualan
      </div>

      <div
        style="width: 200px; text-align: center; padding: 30px; float: right"
      >
        <u><b class="CustomerNama"></b></u><br />
        <em>Customer</em> / Pelanggan
      </div>
    </div>

    <script>
      $(document).ready(function () {
        function formatNumberToIDR(number) {
          // const IDR = new Intl.NumberFormat("id-ID").format(number);
          // return IDR;
          return new Intl.NumberFormat("id-ID", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        }).format(number);
        }

        function terbilang(a) {
          var bilangan = [
            "",
            "Satu",
            "Dua",
            "Tiga",
            "Empat",
            "Lima",
            "Enam",
            "Tujuh",
            "Delapan",
            "Sembilan",
            "Sepuluh",
            "Sebelas",
          ];

          // 1 - 11
          if (a < 12) {
            var kalimat = bilangan[a];
          }
          // 12 - 19
          else if (a < 20) {
            var kalimat = bilangan[a - 10] + " Belas";
          }
          // 20 - 99
          else if (a < 100) {
            var utama = a / 10;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 10;
            var kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
          }
          // 100 - 199
          else if (a < 200) {
            var kalimat = "Seratus " + terbilang(a - 100);
          }
          // 200 - 999
          else if (a < 1000) {
            var utama = a / 100;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 100;
            var kalimat = bilangan[depan] + " Ratus " + terbilang(belakang);
          }
          // 1,000 - 1,999
          else if (a < 2000) {
            var kalimat = "Seribu " + terbilang(a - 1000);
          }
          // 2,000 - 9,999
          else if (a < 10000) {
            var utama = a / 1000;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 1000;
            var kalimat = bilangan[depan] + " Ribu " + terbilang(belakang);
          }
          // 10,000 - 99,999
          else if (a < 100000) {
            var utama = a / 100;
            var depan = parseInt(String(utama).substr(0, 2));
            var belakang = a % 1000;
            var kalimat = terbilang(depan) + " Ribu " + terbilang(belakang);
          }
          // 100,000 - 999,999
          else if (a < 1000000) {
            var utama = a / 1000;
            var depan = parseInt(String(utama).substr(0, 3));
            var belakang = a % 1000;
            var kalimat = terbilang(depan) + " Ribu " + terbilang(belakang);
          }
          // 1,000,000 - 	99,999,999
          else if (a < 100000000) {
            var utama = a / 1000000;
            var depan = parseInt(String(utama).substr(0, 4));
            var belakang = a % 1000000;
            var kalimat = terbilang(depan) + " Juta " + terbilang(belakang);
          } else if (a < 1000000000) {
            var utama = a / 1000000;
            var depan = parseInt(String(utama).substr(0, 4));
            var belakang = a % 1000000;
            var kalimat = terbilang(depan) + " Juta " + terbilang(belakang);
          } else if (a < 10000000000) {
            var utama = a / 1000000000;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 1000000000;
            var kalimat = terbilang(depan) + " Milyar " + terbilang(belakang);
          } else if (a < 100000000000) {
            var utama = a / 1000000000;
            var depan = parseInt(String(utama).substr(0, 2));
            var belakang = a % 1000000000;
            var kalimat = terbilang(depan) + " Milyar " + terbilang(belakang);
          } else if (a < 1000000000000) {
            var utama = a / 1000000000;
            var depan = parseInt(String(utama).substr(0, 3));
            var belakang = a % 1000000000;
            var kalimat = terbilang(depan) + " Milyar " + terbilang(belakang);
          } else if (a < 10000000000000) {
            var utama = a / 10000000000;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 10000000000;
            var kalimat = terbilang(depan) + " Triliun " + terbilang(belakang);
          } else if (a < 100000000000000) {
            var utama = a / 1000000000000;
            var depan = parseInt(String(utama).substr(0, 2));
            var belakang = a % 1000000000000;
            var kalimat = terbilang(depan) + " Triliun " + terbilang(belakang);
          } else if (a < 1000000000000000) {
            var utama = a / 1000000000000;
            var depan = parseInt(String(utama).substr(0, 3));
            var belakang = a % 1000000000000;
            var kalimat = terbilang(depan) + " Triliun " + terbilang(belakang);
          } else if (a < 10000000000000000) {
            var utama = a / 1000000000000000;
            var depan = parseInt(String(utama).substr(0, 1));
            var belakang = a % 1000000000000000;
            var kalimat =
              terbilang(depan) + " Kuadriliun " + terbilang(belakang);
          }

          var pisah = kalimat.split(" ");
          var full = [];
          for (var i = 0; i < pisah.length; i++) {
            if (pisah[i] != "") {
              full.push(pisah[i]);
            }
          }
          return full.join(" ");
        }

        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get("id");
        $.ajax({
          type: "GET",
          url: "./RepairInvoiceMaterial.aspx/GetDetailInvoice",
          data: { IDDocRepair: id },
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            
            const Objek = data["d"];
            const response = Objek[0];                        
            if (response.IsUsePPN == 0) {
              $('#lblppn10').html('<em>Price </em> / Harga Jual');
              $('#divuseppn1').attr('style', 'display: none');
            }
            Object.entries(response).forEach(([e, x]) => {              
              if ($(`.${e}`).hasClass("nominal")) {
                $(`.${e}`).html(formatNumberToIDR(x));
                return;
              }

              $(`.${e}`).html(x);
            });
            let html = "";
            Objek.forEach((e, i) => {            
              html +=`
                <tr>
                    <td style="
                        border: none;
                        vertical-align: top;
                        text-align: left;
                        width: 180px;"
                    >${e.NomorRepair}</td>    
                    <td style="border: none; vertical-align: top; text-align: left"
                    >${e.NamaItem}</td>    
                    <td
                        style="
                        border: none;
                        vertical-align: top;
                        text-align: center;
                        width: 60px;
                    ">${e.Qty}</td>    
                    <td
                        style="
                        border: none;
                        vertical-align: top;
                        text-align: right;
                        width: 120px;"
                    >
                    &nbsp;
                    </td>
                    <td
                        style="
                        border: none;
                        vertical-align: top;
                        text-align: right;
                        width: 120px;"
                    >
                    &nbsp;
                    </td>
                    <td
                        style="
                        border: none;
                        vertical-align: top;
                        text-align: right;
                        width: 180px;
                    ">${formatNumberToIDR(e.AfterTax)}</td>                        
                </tr>
                `;
            });
            $('#tbody-data-invoice').html(html);

            const alamatToBreakSpace = response["AddrAlamat"]
              .split("\n")
              .join("<br />");

            $("#AddrAlamat").html(
              `(<span>${response["Store"]}</span>)</br><span>${alamatToBreakSpace}</span>`
            );

            $("#terbilang").html(
              (terbilang(response["AfterTax"]) + " Rupiah").toUpperCase()
            );


          },
          error: function (error) {
            console.error(error);
          },
        });
      });
    </script>
  </body>
</html>
