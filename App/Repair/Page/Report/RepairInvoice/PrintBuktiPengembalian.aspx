<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintBuktiPengembalian.aspx.cs"
Inherits="DOKRM_index" %> <br%@ Register Src="~/UC/DetailPaymentControlR.ascx"
TagName="detailPaymentControlR" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head id="Head1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Bukti Pengembalian Reparasi</title>
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
        }

        .print-wrapper {
          zoom: 50%; /* ✅ zoom LEBIH AMAN untuk print */
        }

        /* Hide buttons and non-print areas */
        .no-print {
          display: none !important;
        }
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
    <div id="judul1">
      <div class="judul_kiri">
        <h2 style="margin-bottom: 0px">BUKTI PENGEMBALIAN BARANG REPARASI</h2>
        Nomor <br />
        <label id="lblNorepair"></label>
      </div>

      <div class="judul_kanan2" id="AddrAlamat"></div>
    </div>
    <div class="clear"></div>

    <div class="sub_jdul" >
      <div class="judul_kiri" >
        <table cellspacing="0" cellpadding="0" border="0" class="table-no" style="margin-top: 10px;">
          <tr>
            <td style="vertical-align: top; width: 250px">
                <em>Customer Name</em> / Nama Customer
              </td>
              <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
              <td style="vertical-align: top" class="CustomerNama"></td>
          </tr>
          <tr>
            <td style="vertical-align: top; width: 160px">
              <em>Jewellery Representative</em>
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="SalesNama"></td>
          </tr>
        </table>
      </div>
      <div class="judul_kanan" style="width: 550px">
        <table cellspacing="0" cellpadding="0" border="0" class="table-no" style="margin-top: 10px;">
          <tr>
            <td style="vertical-align: top; width: 250px">
              <em></em> Tanggal Pengajuan
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="tglPengajuan"></td>
          </tr>
          <tr>
            <td style="vertical-align: top; width: 250px">
              <em></em>Tanggal Pengembalian
            </td>
            <td style="vertical-align: top; width: 20px">&nbsp;:&nbsp;</td>
            <td style="vertical-align: top" class="tglPengembalian"></td>
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
      style="width: 1200px"
    >
      <tr>

        <th style="border: none;vertical-align: top;text-align: left;width: 150px;">
            PLU
        </th>
        <th style="border: none;vertical-align: top;text-align: left;width: 150px;">
            Jenis Barang
        </th>
        <th style="border: none;vertical-align: top;text-align: left;width: 150px;">
            Berat Barang
        </th>
        <th style="border: none;vertical-align: top;text-align: left;width: 150px;">
            Berat Potongan
        </th>
        <th style="border: none;vertical-align: top;text-align: left;width: 200px;">
            Gambar
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
      style="width: 1200px"
    >
      <tr>
    
        <td style="border: none;vertical-align: top;text-align: left;width: 150px;" class="PLU">

        </td>        
        <td style="border: none;vertical-align: top;text-align: left;width: 150px;" class="NamaItem">

        </td>        
        <td style="border: none;vertical-align: top;text-align: left;width: 150px;" class="BeratBarang">

        </td>        
        <td style="border: none;vertical-align: top;text-align: left;width: 150px;" class="BeratPotongan">

        </td>        
        <td style="border: none;vertical-align: top;text-align: left;width: 200px;" class="Image">

        </td>
      </tr>
    </table>
    <br />


    <div class="clear">&nbsp;</div>
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
    <br />
    <br />
    <!--<br />
        <br />
        <br />-->
    <div class="row">

      <div class="col-md-6">
        <div style="float: left">          
          <div style="width: 500px;font-size: 15px; text-align: left; padding: 30px; float: left">            
            Keterangan:<br />
            <em><span style="font-weight: lighter;">Pastikan Anda menerima sisa potongan barang perhiasan Anda sesuai dengan nilai 
              yang tertera pada "Berat Potongan" sebelum menandatangani form ini.</span></em>
          </div>          
        </div>
      </div>
      <div class="col-md-6">
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
      </div>
    </div>
    

    <script>
      $(document).ready(function () {
        function formatNumberToIDR(number) {
          const IDR = new Intl.NumberFormat("id-ID").format(number);
          return IDR;
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
          url: "./PrintBuktiPengembalian.aspx/GetDetailInvoice",
          data: { IDDocRepair: id },
          contentType: "application/json; charset=utf-8",
          success: function (data) {            
            const response = data["d"];
            $("#lblNorepair").html(response["NomorRepair"]);
                        
            $(".tglPengajuan").html(response["TglRepair"]);
            $(".tglPengembalian").html(response["TglPengembalian"]);
            $(".CustomerNama").html(response["CustomerNama"]);
            $(".SalesNama").html(response["SalesNama"]);                        
            $(".PLU").html(response["PLU"]);
            $(".NamaItem").html(response["Item"]);
            $(".BeratBarang").html(response["BeratBarang"]);
            $(".BeratPotongan").html(response["BeratPotongan"]);
            // if (response["ImgPicture"]) {
            //     $(".Image").html(`<img src="https://frank.cmk.co.id/Images.aspx?path=${response["ImgPicture"]}" alt="Image" style="max-width:150px;max-height:100px;" />`);
            // }

            const alamatToBreakSpace = response["AddrAlamat"].split("\n").join("<br />");

            $("#AddrAlamat").html(
              `(<span>${response["Store"]}</span>)</br><span>${alamatToBreakSpace}</span>`
            );      
            
            const style = document.createElement('style');
            style.media = 'print';
            style.innerHTML = '@page { size: landscape; }';
            document.head.appendChild(style);

          if (response["ImgPicture"]) {
            const img = new Image();
            img.src = `https://frank.cmk.co.id/Images.aspx?path=${response["ImgPicture"]}`;
            img.alt = "Image";
            img.style.maxWidth = "150px";
            img.style.maxHeight = "100px";
            img.onload = function () {
              $(".Image").html(img);
              window.print(); // Only print when image is ready
            };
          } else {
            window.print(); // No image, print immediately
          }

            // window.print();
          },
          error: function (error) {
            console.error(error);
          },
        });        
      });
    </script>
  </body>
</html>
