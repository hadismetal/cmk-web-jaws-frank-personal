<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="./MasterPagePrint.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

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

    .item-flexbox {
      flex: 1 1 0;
      width: 0;
    }

    .border-line-dotted {
      border: 0.5px dotted rgb(197, 197, 197);
      width: 100%;
    }

    .color-gray {
      color: #999;
    }
  </style>

  <div class="print-wrapper">
  
  <div class="container-fluid d-flex justify-content-between px-4 m-0">
    <div class="item-flexbox">
      <img
        src="/Images/web/logo_web_franknco.jpg"
        border="0"
        runat="server"
        id="img_logo_web"
        style="height: 50px"
      />
    </div>
    <div class="item-flexbox text-center">
      <span style="font-size: 22px; font-weight: bold"
        >&nbsp;Repair Work Order&nbsp;</span
      ><br />
      <span style="font-size: 20px" class="NomorRepair"></span>
    </div>
    <div
      class="item-flexbox text-right fw-bold"
      style="font-size: 14px"
      id="AddrAlamat"
    ></div>
  </div>
  <div class="border-line-dotted mt-3"></div>

  <div class="row p-3">
    <div class="col-4">
      <div class="mt-5">
        <h1>Repair Information</h1>
        <table>
          <tbody>
            <tr>
              <td style="width: 125px; font-size: 14px" class="color-gray">
                Tanggal Masuk
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="OperatorTgl"></td>
            </tr>
            <tr id="tgl-selesai">
              <td style="width: 125px; font-size: 14px" class="color-gray">
                Tanggal Selesai
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="OperatorTglSelesai"></td>
            </tr>
            <tr>
              <td style="width: 125px; font-size: 14px" class="color-gray">
                Customer
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="NamaCustomer"></td>
            </tr>
            <tr>
              <td style="width: 125px; font-size: 14px" class="color-gray">
                JR
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="NamaSales"></td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="mt-4">
        <h1>Product Information</h1>
        <table>
          <tbody>
            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                No. PLU
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="PLU"></td>
            </tr>
            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Material
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="FrameMaterial"></td>
            </tr>
            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Kode Karet
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="KodeKaret"></td>
            </tr>
            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Item
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="Item"></td>
            </tr>
            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Gross Weight (Data &rarr; Actual)
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px">
                <span class="BeratTimbangAwal"></span> &rarr;
                <span class="BeratTimbangActual"></span>
              </td>
            </tr>
            <tr id="content-repair-after">
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Gross Weight (Repair &rarr; Akhir)
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px">
                <span class="BeratTimbang"></span> &rarr;
                <span class="BeratTimbangAkhir"></span>
              </td>
            </tr>


            <tr id="content-repair">
              <td style="width: 250px; font-size: 14px" class="color-gray" id="lbl_size_actual">
                Ring Size Actual
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px">                                
                <span class="SizeAwal"></span> &rarr;
                <span class="SizeAkhir"></span>
              </td>
            </tr>

            <tr>
              <td style="width: 250px; font-size: 14px" class="color-gray">
                Keterangan
              </td>
              <td style="width: 10px" class="color-gray">:</td>
              <td style="width: 300px" class="Keterangan"></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="col-4">
      <div
        class="d-flex flex-column text-center justify-content-center align-items-center"
      >
        <span>Original:</span>
        <div
          class="border mt-3 rounded"
          style="width: 250px; height: 200px; padding: 5px"
        >
          <img style="width: 100%; height: 100%" id="ImgBefore" />
        </div>
      </div>
      <div
        class="d-flex flex-column text-center justify-content-center align-items-center mt-5"
      >
        <span>Repair:</span>
        <div
          class="border mt-3 rounded"
          style="width: 250px; height: 200px; padding: 5px"
        >
          <img style="width: 100%; height: 100%" id="ImgAfter" />
        </div>
      </div>
    </div>
    <div class="col-4">
      <div class="border border-dark p-5">
        <div class="row">
          <div class="col-6">
            <span class="fst-italic fw-bold">Sebelum</span>
          </div>
          <div class="col-6 d-flex">
            <div class="col text-center">TOKO</div>
            <div class="col text-center">CS</div>
          </div>
        </div>
        <div class="row">
          <div class="col-6"></div>
          <div class="col-6 d-flex">
            <div class="col text-center">YA</div>
            <div class="col text-center">TIDAK</div>
            <div class="col text-center">YA</div>
            <div class="col text-center">TIDAK</div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Berlian telah ditest keasliannya</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Berlian dalam kondisi baik</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Rangka dalam kondisi baik</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>

        <div class="row mt-5">
          <div class="col-6">
            <span class="fst-italic fw-bold">Sesudah</span>
          </div>
          <div class="col-6 d-flex">
            <div class="col text-center">TOKO</div>
            <div class="col text-center">CS</div>
          </div>
        </div>
        <div class="row">
          <div class="col-6"></div>
          <div class="col-6 d-flex">
            <div class="col text-center">YA</div>
            <div class="col text-center">TIDAK</div>
            <div class="col text-center">YA</div>
            <div class="col text-center">TIDAK</div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Berlian telah ditest keasliannya</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Berlian dalam kondisi baik</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>
        <div class="row">
          <div class="col-6">Rangka dalam kondisi baik</div>
          <div class="col-6 d-flex" style="gap: 3px">
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
            <div
              class="border border-dark"
              style="width: 100%; height: 25px"
            ></div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="row p-3 mt-5">
    <div class="col-4">
      <h1>List Repair</h1>
      <table class="table table-bordered text-center">
        <thead>
          <tr>
            <th style="width: 125px">No.</th>
            <th>Proses</th>
          </tr>
        </thead>
        <tbody id="tbody-paket"></tbody>
      </table>
    </div>
    <div class="col-8">
      <h1>List Stone</h1>
      <table class="table table-bordered text-center">
        <thead>
          <tr>
            <th style="width: 125px">No.</th>
            <th>Stone</th>
            <th>Total Butir</th>
            <th>Total Butir Actual</th>
            <th>Total Carat</th>
            <th>Total Carat Actual</th>
          </tr>
        </thead>
        <tbody id="tbody-parcel"></tbody>
      </table>
    </div>
  </div>

  <div class="row p-3 ">
    <div class="col-sm-6">
      <h1>Estimasi Biaya Reparasi</h1>
        <table class="table table-bordered">          
          <tbody id="tbody-estimasi"></tbody>
        </table>
    </div>    
  </div>
  
  <div class="row p-3">
    <div class="col-sm-12" style="font-size: 12px">            
        <div class="text-start">
          <h4>
            <span class="fw-bold">*</span>Dengan menandatangani form ini, saya menyatakan telah membaca, memahami, dan menyetujui risiko perbedaan wajar pada hasil reparasi dibanding kondisi awal perhiasan.
          </h4>
        </div>        
    </div>
  </div>

  <div style="display: flex; justify-content: end; padding: 125px">
    <div style="display: flex; gap: 125px">
      <table class="text-center">
        <tr>
          <td class="text-decoration-underline fw-bold NamaCustomer"></td>
        </tr>
        <tr>
          <td class="fst-italic">Customer / Pelanggan</td>
        </tr>
      </table>

      <table class="text-center">
        <tr>
          <td class="text-decoration-underline fw-bold NamaSales"></td>
        </tr>
        <tr>
          <td class="fst-italic">Jewellery Representative</td>
        </tr>
      </table>
    </div>
  </div>

  <div class="row p-3">
    <div class="col-xs-12" style="font-size: 12px">
      Catatan:
      <ol>
        <li>
          Tanda terima reparasi tidak berlaku jika tidak ditandatangani oleh, JR
          dan berstempel.
        </li>
        <li>
          Pengambilan barang reparasi harus membawa tanda terima reparasi yang
          asli dan berstempel.
        </li>
        <li>
          Biaya Material menunjukan rentang harga perbutir terendah hingga tertinggi pada barang tersebut.
        </li>
        <li>
          Estimasi Total Biaya bersifat perkiraan dan dapat berubah. Biaya final yang ditetapkan setelah reparasi selesai, 
          termasuk penyesuaian harga batu,</br> instruksi reparasi, atau penambahan komponen/emas bila diperlukan.
        </li>
      </ol>
    </div>
  </div>
</div>

  <script>
    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }


    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get("id");

    $.ajax({
      type: "GET",
      url: "./index.aspx/GetDetailRepair",
      data: {
        IDDocRepair: id,
      },
      contentType: "application/json; charset=utf-8",
      success: function (data) {
        const response = data["d"];
        const detail = response["docRepairDetail"];
        const package = response["docRepairPackage"];
        const parcel = response["docRepairParcel"];
        const estimasi = response["docRepairEstimasi"];
        

        if (
              detail.Item.toUpperCase().includes("MENS JEWELLERY") || 
              detail.Item.toUpperCase().includes("RING") && 
              !detail.Item.toUpperCase().includes("EARRINGS")
          ) {
            $('#lbl_size_brj').text('Ring Size BRJ');
            $('#lbl_size_actual').text('Ring Size Actual');              
          }else{
            $('#lbl_size_brj').text('Size BRJ (cm)');
            $('#lbl_size_actual').text('Size Actual (cm)');              
          }
        Object.entries(detail).forEach(([key, value]) => {
          if (key !== AddrAlamat) $(`.${key}`).html(value);
        });
        

        $("#content-repair-after").addClass("d-none");
        if (detail["Status"] > 7 && detail["Status"] < 11) {
          $("#content-repair-after").removeClass("d-none");
        }

        $("#content-repair-after1").addClass("d-none");
        if (detail["Status"] > 7 && detail["Status"] < 11) {
          $("#content-repair-after1").removeClass("d-none");
        }

        $("#tgl-selesai").addClass("d-none");
        if (detail["Status"] == 9) {
          $(".OperatorTglSelesai").text(
            detail["OperatorTglSelesai"].split("-").reverse().join("-")
          );
          $("#tgl-selesai").removeClass("d-none");
        }

        $(".OperatorTgl").text(
          detail["OperatorTgl"].split("-").reverse().join("-")
        );

        $("#ImgBefore").attr(
          "src",
          "/Images.aspx?path=" + detail["ImgProduct"]
        );

        $("#ImgAfter").attr("src", detail["ImgPicture"]);

        const alamatToBreakSpace = detail["AddrAlamat"]
          .split("\n")
          .join("<br />");

        $("#AddrAlamat").html(
          `(<span>${detail["KodeStore"]}</span>)</br><span>${alamatToBreakSpace}</span>`
        );

        const packageHtml = package.reduce((accumulator, calculator, index) => {
          const { NamaPackage } = calculator;
          accumulator += `
            <tr>
              <td>${index + 1}</td>
              <td>${NamaPackage}</td>
            </tr>
          `;

          return accumulator;
        }, ``);        

        const parcelHtml = parcel.reduce((accumulator, calculator, index) => {
          const {
            Stone,
            StoneType,
            Clarity,
            Color,
            TotalButir,
            TotalCarat,
            TotalButirActual,
            TotalCaratActual,
          } = calculator;

          accumulator += `
            <tr>
              <td>${index + 1}</td>
              <td>${Stone} - ${Color} - ${Clarity}</td>
              <td>${TotalButir}</td>
              <td>${TotalButirActual}</td>
              <td>${TotalCarat}</td>
              <td>${TotalCaratActual}</td>
            </tr>
          `;

          return accumulator;
        }, ``);

        const estimasiHtml = estimasi.reduce((accumulator,calculator,index)=>{
          const {
            IDDocRepair,
            TipeEstimasi,
            HargaMinIDR,
            HargaMaxIDR
          } = calculator;

          let dataharga = HargaMinIDR==HargaMaxIDR ? formatNumberToIDR(HargaMaxIDR) : formatNumberToIDR(HargaMinIDR) + " - "+ formatNumberToIDR(HargaMaxIDR);
          harga = TipeEstimasi.includes("Services") && HargaMinIDR == 0 ? formatNumberToIDR(HargaMaxIDR) : dataharga;                
          let stylebold = TipeEstimasi.includes("Total") ? `class="fw-bold"` : ``;

          accumulator+=`
              <tr ${stylebold}>                                
                <td class="text-left">${TipeEstimasi}</td>
                <td class=text-right>${harga}</td>
              </tr>
          `;
          return accumulator;
        }, ``);

        $("#tbody-paket").html(packageHtml);
        $("#tbody-parcel").html(parcelHtml);
        $("#tbody-estimasi").html(estimasiHtml);
      },
      error: function (error) {
        console.error(error);
      },
      complete: function (data) {},
    });
  </script>
</asp:Content>
