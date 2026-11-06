<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepairInvoiceGabungan.aspx.cs"
Inherits="DOKRM_index" %> <%@ Register Src="~/UC/DetailPaymentControlR.ascx"
TagName="detailPaymentControlR" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Sales Invoice Repair Gabungan</title>
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
      style="width: 1050px;"
    >
      <tr>
        <th
          style="
            border: none;
            vertical-align: top;
            text-align: left;
            width: 200px;
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
      style="width: 1050px;"
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
              <td width="390" style="font-size: 14px">
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

    <script>
      
      $(document).ready(function () {
        
        var w1 = " style='border:none; vertical-align:top; text-align:left; width:180px;' ";
        var w2 = " style='border:none; vertical-align:top; text-align:left; ' ";
        var w3 = " style='border:none; vertical-align:top; text-align:center; width:60px;' ";
        var w4 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
        var w5 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
        var w6 = " style='border:none; vertical-align:top; text-align:right; width:180px;' ";

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
          url: "./RepairInvoiceGabungan.aspx/GetDetailInvoice",
          data: { IDDocRepair: id },
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            
            const response = data["d"];
            const IsUsePPN = response[0]["IsUsePPN"];
            let html=``;
            let totalbeforeTax = 0;
            let totalafterTax = 0;
            let totalTax = 0;
            if (IsUsePPN == 0) {
              $('#lblppn10').html('<em>Price</em> / Harga Jual');
              $('#divuseppn1').attr('style', 'display:none');
              
            }

            response.forEach((e,i)=>{
                console.log(e)
                totalafterTax+=parseFloat(e.AfterTax);
                totalbeforeTax+=parseFloat(e.BeforeTax);
                totalTax+=parseFloat(e.Tax);

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
                    ">1</td>    
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
            })            
            console.log(response)
            $('#tbody-data-invoice').html(html);
            // Object.entries(response[0]).forEach(([e, x]) => {                
            //   if ($(`.${e}`).hasClass("nominal")) {
            //     // $(`.${e}`).html(formatNumberToIDR(x));                
            //   }

            //   $(`.${e}`).html(x);
            // });
            // Object.entries(response[1]).forEach(([e, x]) => {                
            //   if ($(`.${e}`).hasClass("nominal")) {
            //     $(`.${e}`).html(formatNumberToIDR(x));                
            //   }

            //   $(`.${e}`).append(x);
            // });

            const alamatToBreakSpace = response[0]["AddrAlamat"]
              .split("\n")
              .join("<br />");

            $("#AddrAlamat").html(
              `(<span>${response[0]["Store"]}</span>)</br><span>${alamatToBreakSpace}</span>`
            );
            $('.NomorInvoice').html(response[0]["NomorSO"])
            $('.NoCustomer').html(response[0]["NoCustomer"])
            $('.SalesNama').html(response[0]["SalesNama"])
            $('.CustomerNama').html(response[0]["CustomerNama"])
            $('.TglNow').html(response[0]["TglNow"])
            $('.BeforeTax').html(formatNumberToIDR(totalbeforeTax))
            $('.AfterTax').html(formatNumberToIDR(totalafterTax))
            $('.Tax').html(formatNumberToIDR(totalTax))

            $("#terbilang").html(
              (terbilang(totalafterTax) + " Rupiah").toUpperCase()
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
    

</body>
</html>
